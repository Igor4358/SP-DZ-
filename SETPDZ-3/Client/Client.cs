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

namespace Client
{
    public partial class Client : Form
    {
        private UdpClient udpClient;
        private IPEndPoint serverEndPoint;
        public Client()
        {
            InitializeComponent();
            udpClient = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string input = txtProducts.Text;
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Введите продукты.");
                return;
            }

            byte[] requestBytes = Encoding.UTF8.GetBytes(input);
            udpClient.Send(requestBytes, requestBytes.Length, serverEndPoint);

            // Получение текстового ответа
            byte[] responseBytes = udpClient.Receive(ref serverEndPoint);
            string response = Encoding.UTF8.GetString(responseBytes);
            txtResponse.Text = response;

            // Получение изображения
            byte[] imageBytes = udpClient.Receive(ref serverEndPoint);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Image image = Image.FromStream(ms);
                pictureBox.Image = image;
            }
        }

    }
    }
