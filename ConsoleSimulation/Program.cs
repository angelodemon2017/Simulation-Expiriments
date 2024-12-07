using LibraryNet;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleSimulation
{
    class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();
        static List<Entity> entities;
        static TcpListener server;
        static Random random;

        private static readonly object lockObject = new object();

        static void Main(string[] args)
        {
            entities = new List<Entity>();
            for (var i = 0; i < 10; i++)
            {
                entities.Add(new Entity(i));
            }
            random = new Random();

            server = new TcpListener(IPAddress.Any, 12345);
            server.Start();
            Console.WriteLine("Сервер запущен...");

            Thread listenerThread = new Thread(Listener);
            listenerThread.Start();

            while (true)
            {
                // Обработка сущностей в цикле
                ProcessEntities();
            }
        }

        static void Listener()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("Клиент подключён.");
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    lock (lockObject)
                    {
                        entities.Add(new Entity(entities.Count));
                    }
                    Console.WriteLine($"Получено от клиента: {message}");
                    // Здесь вы можете обработать команды
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"EX: {ex.Message}");
                    Console.ReadLine();
                }
            }

            clients.Remove(client);
            client.Close();
        }

        static long step;
        static byte counter = 0;
        static void ProcessEntities()
        {
            try
            {
                counter++;
                step++;
                lock (lockObject)
                {
                    foreach (var entity in entities)
                    {
                        TryUpdate(entity, random.Next(100));
                    }
                    if (counter > 100)
                    {
                        counter = 0;
                        var stepMessage = new NetMessage(TypeMessage.Step, $"{step}");
                        SendUpdate(stepMessage.FullMessage);
                        entities.ForEach(e => SendUpdate(new NetMessage(e).FullMessage));
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"EX: {ex.Message}");
                Console.ReadLine();
            }
        }

        static bool TryUpdate(Entity entity, int rndVal)
        {
            if (rndVal < 10)
            {
                entity.FieldValue += 0.01f;
                Console.WriteLine(entity.Message);
            }
            return rndVal < 10;
        }

        static void SendUpdate(string entity)
        {
            string updateMessage = $"{entity}";
            byte[] msg = Encoding.ASCII.GetBytes(updateMessage);

            foreach (var client in clients)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(msg, 0, msg.Length);
            }
        }
    }
}
