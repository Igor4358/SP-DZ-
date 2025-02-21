using System;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ClientForm : Form
{
    private TextBox ipAddressTextBox;
    private TextBox portTextBox;
    private Button connectButton;
    private TextBox messageTextBox;
    private Button sendButton;
    private ListBox chatListBox;

    private NetworkClient _networkClient;

    public ClientForm()
    {
        InitializeComponents();
        _networkClient = new NetworkClient();
    }

    private void InitializeComponents()
    {
        // Инициализация элементов управления
        ipAddressTextBox = new TextBox { Location = new System.Drawing.Point(10, 10), Width = 150 };
        portTextBox = new TextBox { Location = new System.Drawing.Point(170, 10), Width = 50 };
        connectButton = new Button { Location = new System.Drawing.Point(230, 10), Text = "Подсоединиться" };
        messageTextBox = new TextBox { Location = new System.Drawing.Point(10, 40), Width = 200 };
        sendButton = new Button { Location = new System.Drawing.Point(220, 40), Text = "Отправить" };
        chatListBox = new ListBox { Location = new System.Drawing.Point(10, 70), Size = new System.Drawing.Size(300, 100) };

        connectButton.Click += ConnectButton_Click;
        sendButton.Click += SendButton_Click;

        Controls.Add(ipAddressTextBox);
        Controls.Add(portTextBox);
        Controls.Add(connectButton);
        Controls.Add(messageTextBox);
        Controls.Add(sendButton);
        Controls.Add(chatListBox);
    }
    private async void ConnectButton_Click(object sender, EventArgs e)
    {
        string ipAddress = ipAddressTextBox.Text;
        int port = int.Parse(portTextBox.Text);

        await _networkClient.ConnectAsync(ipAddress, port);
        chatListBox.Items.Add("Подключено к серверу.");
    }
    private async void SendButton_Click(object sender, EventArgs e)
    {
        string message = messageTextBox.Text;
        await _networkClient.SendMessageAsync(message);
        chatListBox.Items.Add($"Вы: {message}");

        string response = await _networkClient.ReceiveMessageAsync();
        chatListBox.Items.Add($"Сервер: {response}");

        if (response == "Bye")
        {
            _networkClient.Disconnect();
            chatListBox.Items.Add("Соединение закрыто.");
        }
    }
}