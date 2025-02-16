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
using System.Diagnostics.Eventing.Reader;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void SendRequest(string request)
        {
            try
            {
                string serverIp = "127.0.0.1";
                int port = 12345;
                TcpClient client = new TcpClient(serverIp, port);

                NetworkStream stream = client.GetStream();
                byte[] requestData = Encoding.UTF8.GetBytes(request);
                stream.Write(requestData, 0, requestData.Length);
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer,0,bytesRead);
                UpdateResponce(response);
                client.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.Message);
            }
        }
        private void UpdateResponce(string response)
        {
            if (responseLabel.InvokeRequired) 
            {
                responseLabel.Invoke(new Action(() => responseLabel.Text = response));
            }
            else
            {
                responseLabel.Text = response;
            }     
        }
        private void timeButton_Click(object sender, EventArgs e)
        {
            SendRequest("time");
        }
        private void dateButton_Click(object sender, EventArgs e)
        {
            SendRequest("date");
        }
    }
}
