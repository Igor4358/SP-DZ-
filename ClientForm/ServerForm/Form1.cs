using System;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ServerFormm : Form
{
    private TextBox ipAddressTextBox;
    private TextBox portTextBox;
    private Button startButton;
    private TextBox messageTextBox;
    private Button sendButton;
    private ListBox chatListBox;

    private NetworkServer _networkServer;

    public ServerFormm()
    {
        InitializeComponents();
        _networkServer = new NetworkServer();
    }

    private void InitializeComponents()
    {
        // Инициализация элементов управления
        ipAddressTextBox = new TextBox { Location = new System.Drawing.Point(10, 10), Width = 150 };
        portTextBox = new TextBox { Location = new System.Drawing.Point(170, 10), Width = 50 };
        startButton = new Button { Location = new System.Drawing.Point(230, 10), Text = "Запустить сервер" };
        messageTextBox = new TextBox { Location = new System.Drawing.Point(10, 40), Width = 200 };
        sendButton = new Button { Location = new System.Drawing.Point(220, 40), Text = "Отправить" };
        chatListBox = new ListBox { Location = new System.Drawing.Point(10, 70), Size = new System.Drawing.Size(300, 100) };

        startButton.Click += StartButton_Click;
        sendButton.Click += SendButton_Click;

        Controls.Add(ipAddressTextBox);
        Controls.Add(portTextBox);
        Controls.Add(startButton);
        Controls.Add(messageTextBox);
        Controls.Add(sendButton);
        Controls.Add(chatListBox);
    }

    private async void StartButton_Click(object sender, EventArgs e)
    {
        string ipAddress = ipAddressTextBox.Text;
        int port = int.Parse(portTextBox.Text);

        await _networkServer.StartAsync(ipAddress, port);
        chatListBox.Items.Add("Сервер запущен. Ожидание подключения...");

        string message = await _networkServer.ReceiveMessageAsync();
        chatListBox.Items.Add($"Клиент: {message}");
    }

    private async void SendButton_Click(object sender, EventArgs e)
    {
        string message = messageTextBox.Text;
        await _networkServer.SendMessageAsync(message);
        chatListBox.Items.Add($"Вы: {message}");

        string response = await _networkServer.ReceiveMessageAsync();
        chatListBox.Items.Add($"Клиент: {response}");

        if (response == "Bye")
        {
            _networkServer.Stop();
            chatListBox.Items.Add("Соединение закрыто.");
        }
    }
}