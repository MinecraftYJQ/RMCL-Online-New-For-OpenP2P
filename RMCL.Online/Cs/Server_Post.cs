using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace RMCL.Online.Cs
{
    internal class Server_Post
    {
        public static Thread Post_Thread = null;
        public static Thread Server_Thread = null;
        public static void Post_Main(int post)
        {
            string multicastGroup = "224.0.2.60";
            int multicastPort = 4445;

            using (UdpClient client = new UdpClient(post))
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(multicastGroup), multicastPort);

                byte[] ttl = new byte[] { 2 }; // 多播数据包的存活时间
                client.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, ttl);

                while (true)
                {
                    string message = $"[MOTD]§b§l[RMCL.Online] §2局域网世界 §b by Minecraft一角钱[/MOTD][AD]{post}[/AD]";
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    client.Send(data, data.Length, remoteEP);

                    Thread.Sleep(100);
                }
            }
        }

        public static void Server_Get(int post,int mbpost)
        {
            int port = post;  // 假设GetPort是一个返回端口号的方法

            TcpListener server = new TcpListener(IPAddress.Loopback, port);
            server.Start();
            Console.WriteLine("Server started. Listening for connections...");

            try
            {
                while (true)  // 无限循环等待客户端连接
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    // 处理客户端连接的独立线程或任务
                    // 使用异步方式可以提高性能，这里使用同步方式演示
                    HandleClient(client, mbpost);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server exception: " + ex);
            }
        }

        private static void HandleClient(TcpClient client,int mbdk)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);

                while (true)
                {
                    string request = reader.ReadLine();
                    if (request == null) break;

                    Console.WriteLine("Received: " + request);
                    string response = "Echo: " + request; // 示例响应
                    if (request == "post")
                    {
                        response= mbdk.ToString();
                    }
                    else
                    {

                    }
                    Console.WriteLine($"{response}\n{response}");
                    writer.WriteLine(response);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while handling client: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        public static string get_web(string serverIp,int port,string text)
        {
            try
            {
                // 创建TcpClient对象，连接到服务器
                TcpClient client = new TcpClient(serverIp, port);
                Console.WriteLine("Connected to server.");

                // 获取流对象，用于读写数据
                NetworkStream stream = client.GetStream();

                // 发送信息到服务器
                string messageToSend = text;
                byte[] data = Encoding.ASCII.GetBytes(messageToSend);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Message sent to server.");

                // 接收服务器返回的信息
                byte[] buffer = new byte[1024]; // 定义接收缓冲区大小
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Response received: " + response);

                
                // 关闭TcpClient连接
                client.Close();

                return response;
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException: " + ex);
                return "dontget";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                return "dontget";
            }
        }
    }
}
