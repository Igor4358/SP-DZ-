using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SETPDZ_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 11000;
            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();
            Console.WriteLine("Сервер запущен");
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Клиент подключен");
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"{DateTime.Now:HH:mm} от[{(client.Client.RemoteEndPoint as IPEndPoint)?.Address}]" +
                $"получена строка: {receivedMessage}");
            string responseMessage = "Привет клиент!";
            byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);
            stream.Write(responseData, 0, responseData.Length);
            Console.WriteLine($"{DateTime.Now:HH:mm}отправлено сообщение клиенту: {responseMessage}");
            client.Close();
            listener.Stop();
        }
    }
}
