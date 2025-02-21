using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RecipeServer
{
    class Program
    {
        private static Dictionary<string, List<string>> recipes = new Dictionary<string, List<string>>()
        {
            { "Омлет", new List<string> { "яйца", "молоко", "соль", "масло" } },
            { "Салат Цезарь", new List<string> { "курица", "салат", "сухарики", "сыр", "соус" } },
            { "Паста Карбонара", new List<string> { "паста", "бекон", "яйца", "сыр", "чеснок" } },
            { "Борщ", new List<string> { "свекла", "картофель", "морковь", "лук", "капуста", "мясо" } }
        };

        // Словарь для хранения информации о запросах клиентов
        private static Dictionary<IPAddress, ClientRequestInfo> clientRequests = new Dictionary<IPAddress, ClientRequestInfo>();

       
        private const int RequestLimit = 10; 
        private static readonly TimeSpan RequestInterval = TimeSpan.FromHours(1);

       
        private const int MaxClients = 100; 
        private static int activeClients = 0; 

       
        private static readonly TimeSpan ClientTimeout = TimeSpan.FromMinutes(10); 

        static void Main(string[] args)
        {
            UdpClient udpServer = new UdpClient(5000); 

            Console.WriteLine("Сервер запущен и ожидает запросов...");
            Thread cleanupThread = new Thread(CleanupInactiveClients);
            cleanupThread.IsBackground = true;
            cleanupThread.Start();

            while (true)
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] requestBytes = udpServer.Receive(ref clientEndPoint);
                string request = Encoding.UTF8.GetString(requestBytes);

                Console.WriteLine($"Получен запрос от {clientEndPoint.Address}: {request}");

                if (activeClients >= MaxClients)
                {
                    string response = "Сервер перегружен. Попробуйте позже.";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    udpServer.Send(responseBytes, responseBytes.Length, clientEndPoint);
                    continue;
                }

                if (IsRequestAllowed(clientEndPoint.Address))
                {
                    string[] products = request.Split(',');
                    List<string> matchingRecipes = FindRecipes(products);

                    string response = matchingRecipes.Any() ? string.Join(", ", matchingRecipes) : "Рецепты не найдены";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                    udpServer.Send(responseBytes, responseBytes.Length, clientEndPoint);
                }
                else
                {
                    string response = "Превышен лимит запросов. Попробуйте позже.";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    udpServer.Send(responseBytes, responseBytes.Length, clientEndPoint);
                }
            }
        }

        private static List<string> FindRecipes(string[] products)
        {
            List<string> result = new List<string>();

            foreach (var recipe in recipes)
            {
                if (products.All(product => recipe.Value.Contains(product)))
                {
                    result.Add(recipe.Key);
                }
            }
            return result;
        }

        private static bool IsRequestAllowed(IPAddress clientAddress)
        {
            if (!clientRequests.ContainsKey(clientAddress))
            {
                clientRequests[clientAddress] = new ClientRequestInfo
                {
                    RequestCount = 1,
                    LastRequestTime = DateTime.UtcNow
                };
                activeClients++; 
                return true;
            }

            var clientInfo = clientRequests[clientAddress];
            if (DateTime.UtcNow - clientInfo.LastRequestTime > RequestInterval)
            {
                clientInfo.RequestCount = 1;
                clientInfo.LastRequestTime = DateTime.UtcNow;
                return true;
            }
            if (clientInfo.RequestCount >= RequestLimit)
            {
                return false;
            }
            clientInfo.RequestCount++;
            clientInfo.LastRequestTime = DateTime.UtcNow;
            return true;
        }
        private static void CleanupInactiveClients()
        {
            while (true)
            {
                lock (clientRequests)
                {
                    var inactiveClients = clientRequests
                        .Where(c => DateTime.UtcNow - c.Value.LastRequestTime > ClientTimeout)
                        .ToList();
                    foreach (var client in inactiveClients)
                    {
                        clientRequests.Remove(client.Key);
                        activeClients--; 
                        Console.WriteLine($"Клиент {client.Key} отключен за неактивность.");
                    }
                }
                Thread.Sleep(60000); 
            }
        }
    }
    class ClientRequestInfo
    {
        public int RequestCount { get; set; } 
        public DateTime LastRequestTime { get; set; } 
    }
}