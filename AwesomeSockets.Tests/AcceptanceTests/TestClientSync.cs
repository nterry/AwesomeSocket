﻿using System;
using System.Net;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace AwesomeSockets.Tests.AcceptanceTests
{
    class TestClientSync
    {
        private ISocket _server;

        private readonly Buffer _sendBuffer;
        private readonly Buffer _receiveBuffer;

        public TestClientSync()
        {
            _sendBuffer = Buffer.New();
            _receiveBuffer = Buffer.New();
            Console.WriteLine("Connecting to server... Please Wait");
            _server = AweSock.TcpConnect("127.0.0.1", 14804);
            ReceiveTestMessage();
            SendTestMessage();
            while(true)
            {
                //Here so we run continuously...
            }
        }

        private void SendTestMessage()
        {
            Buffer.ClearBuffer(_sendBuffer);
            Buffer.Add(_sendBuffer, 10);
            Buffer.Add(_sendBuffer, 20.0F);
            Buffer.Add(_sendBuffer, 40.0);
            Buffer.Add(_sendBuffer, 'A');
            Buffer.Add(_sendBuffer, "The quick brown fox jumped over the lazy dog");
            Buffer.Add(_sendBuffer, (byte)255);
            Buffer.FinalizeBuffer(_sendBuffer);

            var bytesSent = AweSock.SendMessage(_server, _sendBuffer);
            Console.WriteLine("Sent payload. {0} bytes written.", bytesSent);
        }

        private void ReceiveTestMessage()
        {
            var bytesReceived = AweSock.ReceiveMessage(_server, _receiveBuffer);
            MessageReceived(bytesReceived);
        }

        private void TcpConnected(ISocket socket)
        {
            Console.WriteLine("Connected to server. Waiting for server to send message...");
            _server = socket;
            ReceiveTestMessage();
        }

        private void MessageReceived(Tuple<int, EndPoint> bytesReceived)
        {
            Console.WriteLine("Received message from server. Size is {0}. Details are as follows: {1} (int)\n{2} (float)\n{3} (double)\n{4} (char)\n{5} (string)\n{6} (byte)", bytesReceived, Buffer.Get<int>(_receiveBuffer), Buffer.Get<float>(_receiveBuffer), Buffer.Get<double>(_receiveBuffer), Buffer.Get<char>(_receiveBuffer), Buffer.Get<string>(_receiveBuffer), Buffer.Get<byte>(_receiveBuffer));
        }
    }
}
