using System.Net.Sockets;
using System.Text;
using UnityEngine;
using LibraryNet;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    private TcpClient client;
    private NetworkStream stream;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ConnectToServer("127.0.0.1", 12345); // Укажите IP сервера
    }

    private void ConnectToServer(string ipAddress, int port)
    {
        client = new TcpClient(ipAddress, port);
        stream = client.GetStream();
        Debug.Log("Подключено к серверу.");

        // Начните получать сообщение от сервера
        ReceiveMessages();
    }

    public int lengthMes = 0;

    private async void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        string messageBuffer = string.Empty;

        while (true)
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead == 0) break;

            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            messageBuffer += message;
            int index = messageBuffer.IndexOf(NetMessage.ENDMES);
            lengthMes = messageBuffer.Length;
            while (index > 0)
            {
                var substr = messageBuffer.Substring(0, index);
                messageBuffer = messageBuffer.Substring(index + 1);
                index = messageBuffer.IndexOf(NetMessage.ENDMES);
                try
                {
                    MessageMap(substr);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"index was{index}, message was:{messageBuffer}");
                    //                Debug.Log($"{ex.Message}");
                }
            }
        }

        client.Close();
    }

    private void MessageMap(string netMessage)
    {
        var typeMes = NetMessage.GetTypeFromFullMessage(netMessage);
        var valMessage = NetMessage.GetMessageFromFullMessage(netMessage);
        switch (typeMes)
        {
            case TypeMessage.Step:
                UiManager.Instance.UpdateStepLabel(valMessage);
                break;
            case TypeMessage.Entity:
                EntityManager.Instance.UpdateEntity(valMessage);
                break;
        }
    }

    public void SendCommand(string command)
    {
        if (client != null && stream != null)
        {
            byte[] msg = Encoding.ASCII.GetBytes(command);
            stream.Write(msg, 0, msg.Length);

            Debug.Log("Команда отправлена: " + command);
        }
    }
}