using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RecipeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

            while (true)
            {
                Console.WriteLine("Введите продукты через запятую:");
                string input = Console.ReadLine();

                byte[] requestBytes = Encoding.UTF8.GetBytes(input);
                udpClient.Send(requestBytes, requestBytes.Length, serverEndPoint);

                byte[] responseBytes = udpClient.Receive(ref serverEndPoint);
                string response = Encoding.UTF8.GetString(responseBytes);

                Console.WriteLine($"Рецепты: {response}");
            }
        }
    }
}