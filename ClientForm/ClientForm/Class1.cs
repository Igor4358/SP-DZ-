using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetworkClient
{
    private TcpClient _client;
    private NetworkStream _stream;

    // Подключение к серверу
    public async Task ConnectAsync(string ipAddress, int port)
    {
        _client = new TcpClient();
        await _client.ConnectAsync(ipAddress, port);
        _stream = _client.GetStream();
    }

    // Отправка сообщения
    public async Task SendMessageAsync(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        await _stream.WriteAsync(data, 0, data.Length);
    }

    // Получение сообщения
    public async Task<string> ReceiveMessageAsync()
    {
        byte[] buffer = new byte[1024];
        int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    // Закрытие соединения
    public void Disconnect()
    {
        _stream?.Close();
        _client?.Close();
    }
}
