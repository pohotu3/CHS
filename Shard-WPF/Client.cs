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
using System.Threading;
using ConnectionData;

namespace Shard_WPF
{
    public class Client
    {

        private int port;
        private string ipAddress, guid, serverGuid;
        private bool running = false, connected = false;

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

        public void Data_OUT(Packet p)
        {
            if (connected && p.packetType != Packet.PacketType.Registration)
                socket.Send(p.ToBytes());
        }

        // for handling received data
        private void DataManager(Packet p)
        {
            switch (p.packetType)
            {
                case Packet.PacketType.Registration:
                    // if the server is sending the registration packet (start of connection)
                    ShardCore.GetCore().Write("Received registration packet from Heart.");
                    serverGuid = p.senderID;

                    // now save that to a file if this is the first connection, otherwise compare it to the file

                    // set the commandKey from the server registration packet
                    ShardCore.commandKey = p.packetString;

                    // send client registration packet
                    Data_OUT(new Packet(Packet.PacketType.Registration, guid));
                    ShardCore.GetCore().Write("Sent registration packet to Heart.");
                    break;
                case Packet.PacketType.Handshake:
                    ShardCore.GetCore().Write("Handshake received. Connection Established.");
                    connected = true;
                    break;
                case Packet.PacketType.CloseConnection:
                    string reason = p.packetString;
                    if (reason == null)
                        reason = "No Reason Given.";
                    ShardCore.GetCore().Write("Server is closing the connection. Reason: " + reason);
                    ShardCore.GetCore().Write("Server is closing the connection. Reason: " + reason);
                    Close();
                    break;
                case Packet.PacketType.Command:
                    ShardCore.GetCore().Write("Server sent the response: " + p.packetString);
                    ShardCore.GetCore().Write(p.packetString);
                    break;
                default:
                    break;
            }
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

