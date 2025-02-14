using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ClientSETPDZ_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string serverIp = "127.0.0.1";
            int port = 11000;
            TcpClient client = new TcpClient(serverIp, port);
            Console.WriteLine("Подключение к серверу...");
            NetworkStream stream = client.GetStream();
            string message = "Привет сервер!";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine($"{DateTime.Now:HH:mm} отправлено сообщение серверу:{message}");

            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string receivedMessage = Encoding.UTF8.GetString(buffer,0,bytesRead);
            Console.WriteLine($"{DateTime.Now:HH:mm} от [{(client.Client.RemoteEndPoint as IPEndPoint)?.Address}] " +
                $"получена строка:{receivedMessage}");
            client.Close();
        }
    }
}
