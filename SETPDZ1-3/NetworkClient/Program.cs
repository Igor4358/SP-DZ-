using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkClient
{
    internal class NetworkClient
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public async Task ConnectAsync(string ipAddress, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ipAddress, port);
            _stream = _client.GetStream();
        }
        public async Task SendMessageAsync(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(data, 0, data.Length);
        }
        public async Task<string> ReceiveMessageAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
        public void Disconnect()
        {
            _stream?.Close();
            _client?.Close();
        }
    }
    public class NetworkServer
    {
        private TcpListener _listener;
        private TcpClient _client;
        private NetworkStream _stream;

        public async Task StartAsync(string ipAddress, int port)
        {
            _listener = new TcpListener(System.Net.IPAddress.Parse(ipAddress), port);
            _listener.Start();
            _client = await _listener.AcceptTcpClientAsync();
            _stream = _client.GetStream();
        }

        public async Task SendMessageAsync(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(data, 0, data.Length);
        }

        public async Task<string> ReceiveMessageAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        public void Stop()
        {
            _stream?.Close();
            _client?.Close();
            _listener?.Stop();
        }
    }
}
