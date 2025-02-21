using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        static void Main(string[] args)
        {
            UdpClient udpServer = new UdpClient(5000); // Порт сервера

            Console.WriteLine("Сервер запущен и ожидает запросов...");

            while (true)
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] requestBytes = udpServer.Receive(ref clientEndPoint);
                string request = Encoding.UTF8.GetString(requestBytes);

                Console.WriteLine($"Получен запрос от {clientEndPoint}: {request}");

                string[] products = request.Split(',');
                List<string> matchingRecipes = FindRecipes(products);

                string response = matchingRecipes.Any() ? string.Join(", ", matchingRecipes) : "Рецепты не найдены";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                udpServer.Send(responseBytes, responseBytes.Length, clientEndPoint);
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
    }
}