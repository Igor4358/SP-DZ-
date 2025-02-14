using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Server : Form
    {
        private TcpListener listener;
        public Server()
        {
            InitializeComponent();
        }

        private void StartServerButton_Click(object sender, EventArgs e)
        {
            // Запуск сервера в отдельном потоке, чтобы не блокировать UI
            System.Threading.Thread serverThread = new System.Threading.Thread(StartServer);
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void StartServer()
        {
            try
            {
                // Устанавливаем IP-адрес и порт для сервера
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                int port = 11000;

                // Создаем TCP-сокет
                listener = new TcpListener(ipAddress, port);

                // Запускаем сервер
                listener.Start();
                LogMessage("Сервер запущен...");

                // Ожидаем подключения клиента
                TcpClient client = listener.AcceptTcpClient();
                LogMessage("Клиент подключен...");

                // Получаем поток для чтения и записи данных
                NetworkStream stream = client.GetStream();

                // Буфер для чтения данных
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                LogMessage($"{DateTime.Now:HH:mm} от [{(client.Client.RemoteEndPoint as IPEndPoint)?.Address}] получена строка: {receivedMessage}");

                // Отправляем ответ клиенту
                string responseMessage = "Привет, клиент!";
                byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);
                stream.Write(responseData, 0, responseData.Length);
                LogMessage($"{DateTime.Now:HH:mm} отправлено сообщение клиенту: {responseMessage}");

                // Закрываем соединение
                client.Close();
                listener.Stop();
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка: {ex.Message}");
            }
        }

        private void LogMessage(string message)
        {
            // Вывод сообщения в TextBox из другого потока
            if (LogTextBox.InvokeRequired)
            {
                LogTextBox.Invoke(new Action<string>(LogMessage), message);
            }
            else
            {
                LogTextBox.AppendText(message + Environment.NewLine);
            }
        }
    }
}