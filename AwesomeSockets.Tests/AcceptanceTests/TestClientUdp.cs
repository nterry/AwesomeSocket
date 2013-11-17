﻿using System;
using System.Net;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace AwesomeSockets.Tests.AcceptanceTests
{
    class TestClientUdp
    {
        private readonly ISocket _localSocket;

        private const int LocalUdpPort = 14806;
        private const int RemoteUdpPort = 14805;

        private readonly Buffer _receiveBuffer;
        private readonly Buffer _sendBuffer;

        public TestClientUdp()
        {
            _receiveBuffer = Buffer.New();
            _sendBuffer = Buffer.New();

            var tempSocket = AweSock.TcpConnect("127.0.0.1", 14804);
            var clientIp = AweSock.GetRemoteIpAddress(tempSocket);

            _localSocket = AweSock.UdpConnect(LocalUdpPort);

            ReceiveTestMessage();

            SendTestResponse(clientIp);

            while (true)
            {
                //Here so the main thread runs continuously.
            }
        }

        private void ReceiveTestMessage()
        {
            var bytesReceived = AweSock.ReceiveMessage(_localSocket, _receiveBuffer);
            Console.WriteLine("Received message from server. Size is {0}. Details are as follows: {1} (int)\n{2} (float)\n{3} (double)\n{4} (char)\n{5} (string)\n{6} (byte)", bytesReceived, Buffer.Get<int>(_receiveBuffer), Buffer.Get<float>(_receiveBuffer), Buffer.Get<double>(_receiveBuffer), Buffer.Get<char>(_receiveBuffer), Buffer.Get<string>(_receiveBuffer), Buffer.Get<byte>(_receiveBuffer));
        }


        private void SendTestResponse(IPAddress ipAddress)
        {
            Console.WriteLine("Sending response to server");
            Buffer.ClearBuffer(_sendBuffer);
            Buffer.Add(_sendBuffer, 10);
            Buffer.Add(_sendBuffer, 345.0F);
            Buffer.Add(_sendBuffer, 2020.0);
            Buffer.Add(_sendBuffer, 'C');
            Buffer.Add(_sendBuffer, "Blarginess....");
            Buffer.Add(_sendBuffer, (byte)63);
            Buffer.FinalizeBuffer(_sendBuffer);

            AweSock.SendMessage(_localSocket, ipAddress.ToString(), RemoteUdpPort, _sendBuffer);
        }
    }
}
