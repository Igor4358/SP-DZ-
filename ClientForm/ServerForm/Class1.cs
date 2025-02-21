using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetworkServer
{
    private TcpListener _listener;
    private TcpClient _client;
    private NetworkStream _stream;

    // Запуск сервера
    public async Task StartAsync(string ipAddress, int port)
    {
        _listener = new TcpListener(System.Net.IPAddress.Parse(ipAddress), port);
        _listener.Start();
        _client = await _listener.AcceptTcpClientAsync();
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

    // Остановка сервера
    public void Stop()
    {
        _stream?.Close();
        _client?.Close();
        _listener?.Stop();
    }
}