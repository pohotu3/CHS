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
//using System.Windows.Threading;
using System.Threading;
using ConnectionData;

namespace Nerv
{
    public class Client
    {

        private int port;
        private string ipAddress, guid, serverGuid;
        private bool running = false;
        public bool connected = false; // unable to use Data_OUT() if this is set to false

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
				Core.GetCore().Notify("Connected to Heart");
            }
            catch
            {
				Core.GetCore().Notify("Could not connect to Heart");
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
					Core.GetCore().Notify("Not connected to Heart. Cannot send packet. Packet type: " + p.packetType.ToString() + " Packet String: " + p.packetString);
                     return;
                }
            }
        }

        // for handling received data
        private void DataManager(Packet p)
        {
            if (p.senderID != serverGuid && connected) // if the packet ID isn't from the server
            {
				Core.GetCore().Notify("Unauthorized Packet SenderID found. Closing connection");
                Core.GetCore().Shutdown();
            }

            switch (p.packetType)
            {
                case Packet.PacketType.Registration: // if the server is sending the registration packet (start of connection)
					Core.GetCore().Notify("Received registration packet from Heart");
                    serverGuid = p.senderID;
                    Core.commandKey = p.packetString; // set the commandKey from the server registration packet

                    Packet temp = new Packet(Packet.PacketType.Registration, guid); // client registration packet
					Core.GetCore().Notify("Sending Registration");
                    socket.Send(temp.ToBytes());    // send packet to heart

					Core.GetCore().Notify("Sent registration packet to Heart");
                    break;
				case Packet.PacketType.Handshake:
					connected = true;
					Core.GetCore ().Notify ("Handshake received");
					Core.GetCore ().Notify ("Connection Established");
                    break;
                case Packet.PacketType.CloseConnection:
                    string reason = p.packetString;
                    if (reason == null)
                        reason = "No reason. Please reboot Heart";
					Core.GetCore().Notify("Server closed connection. Reason: " + reason);
                    Close();
                    break;
                case Packet.PacketType.Command:
                    Core.GetCore().Notify("Server sent command: " + p.packetString);
                    HandleCommand(p.packetString);
                    break;
                case Packet.PacketType.Error:
                    Core.GetCore().Notify("Error message: " + p.packetString + ". Shutting down.");
                    Core.GetCore().Shutdown();
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
            Core.GetCore().Notify("Closing connection with Heart.");
            running = false;
            socket.Close();
            socket.Dispose();
            listeningThread.Abort();
        }
    }
}

