using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private UdpClient udpServer;
        private bool isRunning = false;
        private Dictionary<IPAddress, ClientRequestInfo> clientRequests = new Dictionary<IPAddress, ClientRequestInfo>();
        private const int RequestLimit = 10;
        private static readonly TimeSpan RequestInterval = TimeSpan.FromHours(1);
        private const int MaxClients = 100;
        private int activeClients = 0;
        private static readonly TimeSpan ClientTimeout = TimeSpan.FromMinutes(10);
        public Form1()
        {
            InitializeComponent();
            btnStop.Enabled = false;
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
                catch (Exception ex)
                {
                    LogMessage($"Ошибка: {ex.Message}");
                }
            }
        }
        private List<string> FindRecipes(string[] products)
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
                    }
                }

                Thread.Sleep(60000);
            }
        }


    }
}
