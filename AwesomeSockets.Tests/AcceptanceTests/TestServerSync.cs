﻿using System;
using System.Net.Sockets;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace AwesomeSockets.Tests.AcceptanceTests
{
    class TestServerSync
    {
        private ISocket _client;

        private readonly Buffer _sendBuffer;
        private readonly Buffer _receiveBuffer;

        public TestServerSync()
        {
            Console.WriteLine("Waiting for client to connect...");
            var listenSocket = AweSock.TcpListen(14804);
            _sendBuffer = Buffer.New();
            _receiveBuffer = Buffer.New();
            _client = AweSock.TcpAccept(listenSocket);
            SendTestMessage();
            ReceiveTestMessage();
            while(true)
            {
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

            var bytesSent = AweSock.SendMessage(_client, _sendBuffer);
            Console.WriteLine(string.Format("Sent payload. {0} bytes written.", bytesSent));
        }

        private void ReceiveTestMessage()
        {
            var bytesReceived = AweSock.ReceiveMessage(_client, _receiveBuffer);
            //MessageReceived(bytesReceived);
        }

        private void TcpAccepted(ISocket socket)
        {
            _client = socket;
            SendTestMessage();
        }

        private void MessageReceived(int bytesReceived)
        {
            Console.WriteLine(string.Format("Received message from client. Size is {0}. Details are as follows: {1} (int)\n{2} (float)\n{3} (double)\n{4} (char)\n{5} (string)\n{6} (byte)", bytesReceived,
                                                                                                                                                                                Buffer.Get<int>(_receiveBuffer), 
                                                                                                                                                                                Buffer.Get<float>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<double>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<char>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<string>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<byte>(_receiveBuffer)));
        }
    }
}
