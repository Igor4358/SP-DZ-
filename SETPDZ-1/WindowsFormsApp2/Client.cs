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
using System.Runtime.Remoting.Messaging;

namespace WindowsFormsApp2
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            System.Threading.Thread clientThread = new System.Threading.Thread(SendMessage);
            clientThread.IsBackground = true;
            clientThread.Start();
        }
        private void SendMessage()
        {
            try
            {
                // Устанавливаем IP-адрес и порт сервера
                string serverIp = "127.0.0.1";
                int port = 11000;

                // Создаем TCP-клиент
                TcpClient client = new TcpClient(serverIp, port);
                LogMessage("Подключено к серверу...");

                // Получаем поток для чтения и записи данных
                NetworkStream stream = client.GetStream();

                // Отправляем сообщение серверу
                string message = MessageTextBox.Text;
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                LogMessage($"{DateTime.Now:HH:mm} отправлено сообщение серверу: {message}");

                // Получаем ответ от сервера
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                LogMessage($"{DateTime.Now:HH:mm} от [{(client.Client.RemoteEndPoint as IPEndPoint)?.Address}] получена строка: {receivedMessage}");

                // Закрываем соединение
                client.Close();
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
