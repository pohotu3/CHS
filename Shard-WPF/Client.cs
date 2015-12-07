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
using System.IO;
using System.Windows.Threading;
using System.Threading;
using ConnectionData;

namespace Shard_WPF
{
    public class Client
    {

        private int port;
        private string ipAddress, guid, serverGuid;
        private bool running = false;
        private bool connected = false; // unable to use Data_OUT() if this is set to false

        public Socket socket;
        private Thread listeningThread;

        // requires an IP and socket constructor
        public Client(string ipAddress, int port, Guid guid)
        {
            this.port = port;
            this.ipAddress = ipAddress;
            this.guid = guid.ToString();

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            try
            {
                // tries to connect to the socket located at the IP and socket given
                socket.Connect(ip);
                ShardCore.GetCore().Write("Connected to Heart at IP " + ipAddress + " Socket: " + port); // ########################
                ShardCore.GetCore().Speak("Connected.");
            }
            catch
            {
                ShardCore.GetCore().Write("Could not connect to " + ipAddress + " on Socket: " + port);            // ##########################
                ShardCore.GetCore().Speak("I was unable to connect to my Heart. Please reboot me and try again.");
                Thread.Sleep(1000);

                return; // we dont want the client socket continuing if we were not able to connect
            }

            // creates the new listening thread
            listeningThread = new Thread(Data_IN);
            running = true;
            listeningThread.Start(); // Data_IN is now constantly being run
        }

        private void Data_IN()
        {
            byte[] buffer;
            int readBytes;

            while (running)
            {
                buffer = new byte[socket.SendBufferSize]; // creates the buffer array to be as large as possible to receive

                try
                {
                    readBytes = socket.Receive(buffer); // gets the number of bytes received
                }
                catch (Exception e)
                {
                    return;
                }

                // as long as we actually received bytes, we can process them
                if (readBytes > 0)
                    DataManager(new Packet(buffer));
            }
        }

        // Unable to send data if bool connected = false
        public void Data_OUT(Packet p)
        {
            if (connected)
            {
                ShardCore.GetCore().Write("Sending " + p.packetType.ToString());
                try
                {
                    socket.Send(p.ToBytes());
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                if (p.packetType != Packet.PacketType.CloseConnection)
                {
                    ShardCore.GetCore().Write("I am not connected to my Heart, so I cant send data. Please restart me to attempt to fix the problem.");
                    return;
                }
            }
        }

        // for handling received data
        private void DataManager(Packet p)
        {
            if (p.senderID != serverGuid && connected) // if the packet ID isn't from the server
            {
                ShardCore.GetCore().Write("An unauthorized connection is attempting to control me. Shutting down.");
                ShardCore.GetCore().Shutdown();
            }

            switch (p.packetType)
            {
                case Packet.PacketType.Registration: // if the server is sending the registration packet (start of connection)
                    ShardCore.GetCore().Write("Received registration packet from Heart.");                  // #######################################
                    serverGuid = p.senderID;
                    ShardCore.commandKey = p.packetString; // set the commandKey from the server registration packet

                    Packet temp = new Packet(Packet.PacketType.Registration, guid); // client registration packet
                    ShardCore.GetCore().Write("Sending Registration");
                    socket.Send(temp.ToBytes());    // send packet to heart

                    ShardCore.GetCore().Write("Sent registration packet to Heart.");                // ###########################
                    break;
                case Packet.PacketType.Handshake:
                    ShardCore.GetCore().Write("Handshake received. Connection Established.");       //############################
                    connected = true;
                    break;
                case Packet.PacketType.CloseConnection:
                    string reason = p.packetString;
                    if (reason == null)
                        reason = "It didn't specify a reason, so please reboot both my Heart and I.";
                    ShardCore.GetCore().Write("Server is closing the connection. Reason: " + reason); // ########################
                    ShardCore.GetCore().Speak("My Heart decided to close the connection. " + reason + ".");
                    Close();
                    break;
                case Packet.PacketType.Command:
                    ShardCore.GetCore().Write("Server sent the command: " + p.packetString);  // ####################
                    HandleCommand(p.packetString);
                    break;
                case Packet.PacketType.Error:
                    ShardCore.GetCore().Write("The Heart sent us an error message. Message: " + p.packetString + " I'm shutting down the connection. Please address the problem, and then start me back up.");
                    ShardCore.GetCore().Shutdown();
                    break;
                default:
                    break;
            }
        }

        private void HandleCommand(string c)
        {

        }

        public void Close()
        {
            Data_OUT(new Packet(Packet.PacketType.CloseConnection, guid));
            ShardCore.GetCore().Write("Closing connection with Heart.");
            running = false;
            socket.Close();
            socket.Dispose();
            listeningThread.Abort();
        }
    }
}

