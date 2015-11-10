/*
* Crystal Home Systems 
* Created by Austin and Ezra
* Open Source with Related GitHub Repo
* UNDER DEVELOPMENT
*
* Copyright© 2015 Austin VanAlstyne, Bailey Thorson
*/

/*
*This file is part of Cyrstal Home Systems.
*
*Cyrstal Home Systems is free software: you can redistribute it and/or modify
*it under the terms of the GNU General Public License as published by
*the Free Software Foundation, either version 3 of the License, or
*(at your option) any later version.
*
*Cyrstal Home Systems is distributed in the hope that it will be useful,
*but WITHOUT ANY WARRANTY; without even the implied warranty of
*MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*GNU General Public License for more details.
*
*You should have received a copy of the GNU General Public License
*along with Cyrstal Home Systems.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ConnectionData;

namespace HeartConsole
{
    public class Server
    {

        private int port;
        public static string guid;
        public static bool listening = false;

        private static Socket listenerSocket;
        private static List<ClientData> _clients;
        private static Thread listenThread;
        public IPEndPoint ip;

        // create all the connection objects
        public Server(int listeningPort, Guid g)
        {
            port = listeningPort;
            guid = g.ToString();

            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clients = new List<ClientData>();

            //ip = new IPEndPoint (IPAddress.Parse (Packet.GetIP4Address ()), port);
            ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listenerSocket.Bind(ip);

            listenThread = new Thread(ListenThread);
        }

        public void Start()
        {
            listening = true;
            listenThread.Start();
        }

        // listener - listens for clients trying to connect
        private void ListenThread()
        {
            while (listening)
            {
                // parameter 5 is the backlog, allows 5 connection attempts at the same time
                // prevents DDOSING in a way
                listenerSocket.Listen(5);

                try
                {
                    _clients.Add(new ClientData(listenerSocket.Accept()));
                }
                catch
                {
                }
            }
        }

        // closes all connections and releases resources
        public void Close()
        {
            listening = false;
            listenerSocket.Close();
            listenerSocket.Dispose();
            listenThread.Join();
            listenThread.Abort();

            // send the close connection status to all clients
            foreach (ClientData cd in _clients)
            {
                Packet p = new Packet(Packet.PacketType.CloseConnection, guid);
                p.packetString = "maintenence";
                cd.Data_OUT(p);
                cd.Close();
            }

            _clients.Clear();
        }
    }

    class ClientData
    {
        public Socket clientSocket;
        public Thread clientThread;
        public string id;
        private bool verified = false;

        // shard details loaded from verification
        private string shardName = "", shardType = "", shardLocation = "";

        public ClientData(Socket clientSocket)
        {
            this.clientSocket = clientSocket;

            HeartCore.GetCore().Write("Incoming connection from Shard. IP: " + clientSocket.AddressFamily.ToString());

            if (HeartCore.cfg_set)
            {
                HeartCore.GetCore().Write("Registering with client " + clientSocket.AddressFamily.ToString());
                clientThread = new Thread(Data_IN);
                clientThread.Start(clientSocket);
                Register();
            }
            else
            {
                HeartCore.GetCore().Write("Refusing connection with client " + clientSocket.AddressFamily.ToString() + ". Please set up the config file.");
                HeartCore.GetCore().Write("After config is set up, try again.");
                SendClientError("Heart configuration is not set up. All connections will be refused until it is.");
                Close();
            }
        }

        public void Data_OUT(Packet p)
        {
            try
            {
                clientSocket.Send(p.ToBytes());
            }
            catch (Exception e)
            {
                HeartCore.GetCore().Write("Unable to send data to the Shard at IP " + clientSocket.AddressFamily.ToString() + ". Closing connection. Restart the Shard.");
                Close();
            }
        }

        private void Register()
        {
            // this function will send the client the server GUID BEFORE the client sends theirs
            Packet p = new Packet(Packet.PacketType.Registration, Server.guid);
            p.packetString = HeartCore.commandKey;

            Data_OUT(p);
            HeartCore.GetCore().Write("Sent registration packet to " + clientSocket.AddressFamily.ToString());
        }

        public void Close()
        {
            clientSocket.Close();
            clientSocket.Dispose();
            clientThread.Abort();
        }

        // clientdata thread - receives data from each client individually
        public void Data_IN(object cSocket)
        {
            Socket clientSocket = (Socket)cSocket;

            byte[] buffer;
            int readBytes;

            // infinite loop, i'm thinking about doing it a different way later
            while (Server.listening)
            {
                // sets our buffer array to the max size we're able to receive
                buffer = new byte[clientSocket.SendBufferSize];

                // gets the amount of bytes we've received
                try
                {
                    readBytes = clientSocket.Receive(buffer);
                }
                catch (ObjectDisposedException e)
                {
                    return;
                }

                // if we actually recieve something, then sort through it
                if (readBytes > 0)
                {
                    // handle data
                    Packet packet = new Packet(buffer);
                    DataManager(packet);
                }
            }
        }

        // this will handle everything about the packet
        public void DataManager(Packet p)
        {
            // if the client is not verified, we will not accept any other type of packet
            // this if statement checks if the connected client has been set up before
            if (!verified)
            {
                // if the client is sending something other than a registration packet, bounce the request
                // (only the registration packet is allowed when the connection isn't verified)
                if (p.packetType != Packet.PacketType.Registration)
                {
                    return;
                }

                HeartCore.GetCore().Write("Recieved registration packet");

                // assign the local GUID var to the packet's sender ID
                id = p.senderID;

                // search shard cfg files that have been created previously for the client's id. if there's a match, continue
                if (RetrieveShard(id))
                {
                    verified = true;

                    // shard information is loaded into the local var's from the RetrieveShard() function
                    // confirm successful connection
                    HeartCore.GetCore().Write("Shard " + shardName + " registered and connected successfully. Sending handshake.");
                    Data_OUT(new Packet(Packet.PacketType.Handshake, Server.guid));
                    return;
                }
                // if there isnt a match, inform the client that it needs to be initialized. Maintain connection though, don't close it.
                else
                {
                    HeartCore.GetCore().Write("Shard using GUID " + id + " and IP " + clientSocket.AddressFamily.ToString() + " tried to connect, verification failed. Please set up the Shard.");
                    SendClientError("Shard is not set up. Please set up the shard through your internet browser.");
                    return;
                }
            }

            switch (p.packetType)
            {
                case Packet.PacketType.CloseConnection:
                    HeartCore.GetCore().Write("Shard " + shardName + " has disconnected.");
                    Close();
                    break;
                case Packet.PacketType.Command:
                    string[] splitCommandArray = p.packetString.ToLower().Split();
                    string commandWord = splitCommandArray[0]; // I want a better way to do this part

                    // analyze command and respond appropriately
                    switch (commandWord)
                    {
                        case "":
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    HeartCore.GetCore().Write(shardName + " sent an unrecognized packet type. Command ignored.");
                    SendClientError("Unrecognized packet type was sent. Please restart the Shard and try again.");
                    break;
            }
        }

        private void SendClientError(String msg)
        {
            Packet vPacket = new Packet(Packet.PacketType.Error, Server.guid);
            vPacket.packetString = msg;
            Data_OUT(vPacket);
            return;
        }

        // Loads all of the shard files, pulls their GUID from the data, checks for a match and returns if there is one.
        // if there is a match, load the info to variables
        private bool RetrieveShard(string guid)
        {
            string baseDir = Variables.Default.baseDir + Variables.Default.shardFileDir;

            // make sure the baseDir exists first
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            // get a list of all current .shard files including the filepaths to them
            string[] shards = Directory.GetFiles(baseDir, "*.shard", SearchOption.TopDirectoryOnly);

            foreach (string shardFile in shards)
            {
                // load the shard file into a config object for manipulation
                Config t = new Config(shardFile);

                // if the connected client ID matches that in a cfg file
                if (id == t.get("guid"))
                {
                    // load the information from the file to the local var's
                    shardName = t.get("shardName");
                    shardType = t.get("shardType");
                    shardLocation = t.get("shardLocation");
                    return true;
                }
            }

            return false; // returns false if there's no match
        }
    }
}
