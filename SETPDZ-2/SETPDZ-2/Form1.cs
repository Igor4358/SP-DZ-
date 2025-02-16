using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;

namespace SETPDZ_2
{
    public partial class Form1 : Form
    {
        private TcpListener listener;
        public Form1()
        {
            InitializeComponent();
        }
        private void StartServer()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                int port = 12345;
                listener = new TcpListener(ipAddress, port);
                listener.Start();
                UpdateStatus("Сервер запущен. Ожидание полключений...");
                listener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка "+ ex.Message);
            }
        }
        private void HandleClient(IAsyncResult result)
        {
            try
            {
                TcpClient client = listener.EndAcceptTcpClient(result);
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string response = " ";
                if (request.ToLower() == "time")
                {
                    response = DateTime.Now.ToShortTimeString();
                }
                else if (request.ToLower() == "date")
                {
                    response = DateTime.Now.ToShortDateString();
                }
                else
                {
                    response = "Неверный запрос";
                }
                byte[] responseData = Encoding.UTF8.GetBytes(response);
                stream.Write(responseData, 0, responseData.Length);
                client.Close();
                UpdateStatus($"Клиент подключен. Запрос: {request}. Ответ:{response}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.Message);
            }
            finally
            {
                listener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
            }
        }
            private void UpdateStatus(string message)
            {
            if(statusLabel.InvokeRequired)
              {
                statusLabel.Invoke(new Action(() => statusLabel.Text = message));
              }
            else
              {
                statusLabel.Text = message;
              }
            }
        private void startButton_Click(object sender, EventArgs e)
        {
            StartServer();
        }
    }
}