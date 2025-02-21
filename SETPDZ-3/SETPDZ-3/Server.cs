using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;


namespace RecipeServer
{
    public partial class ServerForm : Form
    {
        private UdpClient udpServer;
        private bool isRunning = false;
        private Dictionary<IPAddress, ClientRequestInfo> clientRequests = new Dictionary<IPAddress, ClientRequestInfo>();
        private const int RequestLimit = 10;
        private static readonly TimeSpan RequestInterval = TimeSpan.FromHours(1);
        private const int MaxClients = 100;
        private int activeClients = 0;
        private static readonly TimeSpan ClientTimeout = TimeSpan.FromMinutes(10);
        private string logFilePath = "server_log.txt";

        private static Dictionary<string, List<string>> recipes = new Dictionary<string, List<string>>()
        {
            { "Омлет", new List<string> { "яйца", "молоко", "соль", "масло" } },
            { "Салат Цезарь", new List<string> { "курица", "салат", "сухарики", "сыр", "соус" } },
            { "Паста Карбонара", new List<string> { "паста", "бекон", "яйца", "сыр", "чеснок" } },
            { "Борщ", new List<string> { "свекла", "картофель", "морковь", "лук", "капуста", "мясо" } }
        };
        // Словарь с изображениями блюд
        private Dictionary<string, string> recipeImages = new Dictionary<string, string>()
        {
            { "Омлет", "omelet.jpg" },
            { "Салат Цезарь", "caesar_salad.jpg" },
            { "Паста Карбонара", "carbonara.jpg" },
            { "Борщ", "borscht.jpg" }
        };

        public ServerForm()
        {
            InitializeComponent();
            btnStop.Enabled = false;
            InitializeLogFile();
        }

        private void InitializeLogFile()
        {
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isRunning) return;

            udpServer = new UdpClient(5000);
            isRunning = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            LogMessage("Сервер запущен.");

            Thread serverThread = new Thread(StartServer);
            serverThread.IsBackground = true;
            serverThread.Start();

            Thread cleanupThread = new Thread(CleanupInactiveClients);
            cleanupThread.IsBackground = true;
            cleanupThread.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!isRunning) return;

            udpServer.Close();
            isRunning = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            LogMessage("Сервер остановлен.");
        }

        private void StartServer()
        {
            while (isRunning)
            {
                try
                {
                    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] requestBytes = udpServer.Receive(ref clientEndPoint);
                    string request = Encoding.UTF8.GetString(requestBytes);

                    LogMessage($"Получен запрос от {clientEndPoint.Address}: {request}");
                    LogToFile($"Запрос от {clientEndPoint.Address}: {request}");

                    if (activeClients >= MaxClients)
                    {
                        string response = "Сервер перегружен. Попробуйте позже.";
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        udpServer.Send(responseBytes, responseBytes.Length, clientEndPoint);
                        LogToFile($"Отказ клиенту {clientEndPoint.Address}: сервер перегружен.");
                        continue;
                    }

                    if (IsRequestAllowed(clientEndPoint.Address))
                    {
                        string[] products = request.Split(',');
                        List<string> matchingRecipes = FindRecipes(products);

                        if (matchingRecipes.Any())
                        {
                            foreach (var recipe in matchingRecipes)
                            {
                                // Отправка текстового рецепта
                                string response = $"Рецепт: {recipe}";
                                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                                udpServer.Send(responseBytes, responseBytes.Length, clientEndPoint);

                                // Отправка изображения
                                if (recipeImages.ContainsKey(recipe))
                                {
                                    string imagePath = recipeImages[recipe];
                                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                                    udpServer.Send(imageBytes, imageBytes.Length, clientEndPoint);
                                }
                            }
                        }
                        else
                        {
                            string response = "Рецепты не найдены.";
                            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                            udpServer.Send(responseBytes, responseBytes.Length, clientEndPoint);
                        }
                    }
                    else
                    {
                        string response = "Превышен лимит запросов. Попробуйте позже.";
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        udpServer.Send(responseBytes, responseBytes.Length, clientEndPoint);
                        LogToFile($"Отказ клиенту {clientEndPoint.Address}: превышен лимит запросов.");
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"Ошибка: {ex.Message}");
                    LogToFile($"Ошибка: {ex.Message}");
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

        private bool IsRequestAllowed(IPAddress clientAddress)
        {
            if (!clientRequests.ContainsKey(clientAddress))
            {
                clientRequests[clientAddress] = new ClientRequestInfo
                {
                    RequestCount = 1,
                    LastRequestTime = DateTime.UtcNow,
                    FirstConnectionTime = DateTime.UtcNow
                };
                activeClients++;
                LogToFile($"Новый клиент: {clientAddress}, время подключения: {DateTime.UtcNow}");
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

        private void CleanupInactiveClients()
        {
            while (isRunning)
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
                        LogMessage($"Клиент {client.Key} отключен за неактивность.");
                        LogToFile($"Клиент {client.Key} отключен за неактивность. Время подключения: {client.Value.FirstConnectionTime}, время отключения: {DateTime.UtcNow}");
                    }
                }

                Thread.Sleep(60000);
            }
        }

        private void LogMessage(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string>(LogMessage), message);
            }
            else
            {
                txtLog.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
            }
        }

        private void LogToFile(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при записи в лог: {ex.Message}");
            }
        }
    }

    class ClientRequestInfo
    {
        public int RequestCount { get; set; }
        public DateTime LastRequestTime { get; set; }
        public DateTime FirstConnectionTime { get; set; }
    }
}
