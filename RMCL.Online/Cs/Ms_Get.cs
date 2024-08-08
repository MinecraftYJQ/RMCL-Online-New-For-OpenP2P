using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RMCL.Online.Cs
{
    internal class Ms_Get
    {
        public static string get_ms(int port)
        {
            try
            {
                // 创建一个TcpClient实例
                TcpClient client = new TcpClient();
                Stopwatch stopwatch = new Stopwatch();

                // 测量连接时间
                stopwatch.Start();
                client.Connect("127.0.0.1", port);
                stopwatch.Stop();

                // 计算延迟时间
                TimeSpan delay = stopwatch.Elapsed;
                Console.WriteLine("延迟: " + delay.TotalMilliseconds + " 毫秒");

                // 关闭连接
                client.Close();
                return delay.TotalMilliseconds.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("连接失败: " + ex.Message);
                return "???";
            }
        }
    }
}
