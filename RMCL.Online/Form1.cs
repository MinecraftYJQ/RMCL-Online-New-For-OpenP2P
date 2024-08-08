using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using RMCL.Online.Cs;
using System.Net.NetworkInformation;

namespace RMCL.Online
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int get_post = 0;
        string UID;
        bool yxgg = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            yxgg = false;
            UID = Get_UID()+uiTextBox2.Text;
            uiTextBox1.Text = UID;
            yxgg = true;
            Directory.CreateDirectory("bin");
            File.WriteAllBytes("bin\\openp2p.zip", global::RMCL.Online.Properties.Resources.openp2p);
            try
            {
                ZipFile.ExtractToDirectory("bin\\openp2p.zip", "bin");
            }
            catch { }
            Task.Run(() =>
            {
                while (true)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Text = "联机模块 作者：B站 Minecraft一角钱";
                    }));
                    Thread.Sleep(5000);
                }
            });
        }
        private string Get_UID()
        {
            Random _random = new Random();
            StringBuilder sb = new StringBuilder();
            const string validChars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ,.-_+=&%$#@!?";

            // 生成16位随机十六进制字符
            for (int i = 0; i < 30; i++)
            {
                char randomChar = validChars[_random.Next(validChars.Length)];
                sb.Append(randomChar);
            }
            get_post = new Random().Next(1000, 25565);
            //return sb.ToString() + "|"+ get_post.ToString();
            return "RMCL.Online-"+sb.ToString() + "|";
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(yxgg)
            {
                if (uiTextBox1.Text != UID)
                {
                    yxgg = false;
                    uiTextBox1.Text = UID;
                    yxgg = true;
                }
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(uiTextBox1.Text);
        }

        Process process;
        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (uiButton3.Text == "启动房间")
            {
                if (int.Parse(uiTextBox2.Text) == 0)
                {
                    MessageBox.Show("不支持的端口!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string json = "" +
                        "{\r\n" +
                        "  \"network\": {\r\n" +
                        "    \"Token\": 17190022896174664900,\r\n" +
                       $"    \"Node\": \"{uiTextBox1.Text}\",\r\n" +
                        "    \"User\": \"MinecraftYJQ_\",\r\n" +
                        "    \"ShareBandwidth\": 10,\r\n" +
                        "    \"ServerHost\": \"api.openp2p.cn\",\r\n" +
                        "    \"ServerPort\": 27183,\r\n" +
                        "    \"UDPPort1\": 27182,\r\n" +
                        "    \"UDPPort2\": 27183,\r\n" +
                        "    \"TCPPort\": 50448\r\n" +
                        "  },\r\n" +
                        "  \"apps\": null,\r\n" +
                        "  \"LogLevel\": 2\r\n" +
                        "}";

                    File.WriteAllText("bin\\config.json", json);

                    // 创建进程对象
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "bin\\openp2p.exe"; // 控制台应用路径
                    startInfo.RedirectStandardOutput = true;
                    startInfo.StandardOutputEncoding = Encoding.UTF8;
                    startInfo.StandardErrorEncoding = Encoding.UTF8;
                    startInfo.RedirectStandardError = true;
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true; // 不显示新的命令行窗口

                    process = new Process();
                    process.StartInfo = startInfo;
                    process.Start();
                    uiButton3.Text = "关闭房间";

                    /*Server_Post.Server_Thread = new Thread(() =>
                    {
                        Server_Post.Server_Get(get_post, int.Parse(uiTextBox2.Text));
                    });
                    Server_Post.Server_Thread.Start();*/
                }
            }
            else
            {
                uiButton3.Text = "启动房间";
                Server_Post.Server_Thread.Abort();
                process.Kill();
            }
        }
        private async void uiButton1_Click(object sender, EventArgs e)
        {
            if (uiButton1.Text == "启动房间")
            {
                if (uiTextBox3.Text != "")
                {
                    string json = "" +
                        "{\r\n" +
                        "  \"network\": {\r\n" +
                        "    \"Token\": 17190022896174664900,\r\n" +
                       $"    \"Node\": \"{uiTextBox1.Text}\",\r\n" +
                        "    \"User\": \"MinecraftYJQ_\",\r\n" +
                        "    \"ShareBandwidth\": 10,\r\n" +
                        "    \"ServerHost\": \"api.openp2p.cn\",\r\n" +
                        "    \"ServerPort\": 27183,\r\n" +
                        "    \"UDPPort1\": 27182,\r\n" +
                        "    \"UDPPort2\": 27183,\r\n" +
                        "    \"TCPPort\": 55908\r\n" +
                        "  },\r\n" +
                        "  \"apps\": [\r\n" +
                        "    {\r\n" +
                        "      \"AppName\": \"Minecraft Server Info\",\r\n" +
                        "      \"Protocol\": \"tcp\",\r\n" +
                        "      \"UnderlayProtocol\": \"\",\r\n" +
                        "      \"Whitelist\": \"\",\r\n" +
                       $"      \"SrcPort\": {uiTextBox3.Text.Split('|')[1]},\r\n" +
                       $"      \"PeerNode\": \"{uiTextBox3.Text}\",\r\n" +
                       $"      \"DstPort\": {uiTextBox3.Text.Split('|')[1]},\r\n" +
                        "      \"DstHost\": \"localhost\",\r\n" +
                        "      \"PeerUser\": \"\",\r\n" +
                        "      \"RelayNode\": \"\",\r\n" +
                        "      \"ForceRelay\": 0,\r\n" +
                        "      \"Enabled\": 1\r\n" +
                        "    }\r\n  ],\r\n" +
                        "  \"LogLevel\": 2\r\n" +
                        "}";

                    File.WriteAllText("bin\\config.json", json);

                    // 创建进程对象
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "bin\\openp2p.exe"; // 控制台应用路径
                    startInfo.RedirectStandardOutput = true;
                    startInfo.StandardOutputEncoding = Encoding.UTF8;
                    startInfo.StandardErrorEncoding = Encoding.UTF8;
                    startInfo.RedirectStandardError = true;
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true; // 不显示新的命令行窗口

                    process = new Process();
                    process.StartInfo = startInfo;
                    process.Start();
                    uiButton1.Text = "关闭房间";

                    Server_Post.Post_Thread = new Thread(() =>
                    {
                        Server_Post.Post_Main(int.Parse(uiTextBox3.Text.Split('|')[1]));
                    });
                    Server_Post.Post_Thread.Start();

                    Server_Post.Server_Thread = new Thread(() =>
                    {
                        while (true)
                        {
                            this.Invoke(new Action(() =>
                            {
                                label8.Text = $"延迟：{Ms_Get.get_ms(int.Parse(uiTextBox3.Text.Split('|')[1]))} ms";
                            }));
                            Thread.Sleep(1000);
                        }
                    });
                    Server_Post.Server_Thread.Start();
                    MessageBox.Show("联机启动成功!\n已开放端口至游戏的局域网世界。\n开启房间后请等待5~20秒的时间才可联机!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("联机码不得为空!","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                uiButton1.Text = "启动房间";
                Server_Post.Post_Thread.Abort();
                Server_Post.Server_Thread.Abort();
                process.Kill();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Server_Post.Post_Thread!=null)
            {
                Server_Post.Post_Thread.Abort();
            }
        }

        private void uiTextBox2_TextChanged(object sender, EventArgs e)
        {
            UID = Get_UID() + uiTextBox2.Text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (uiTextBox1.Text != UID)
            {
                yxgg = false;
                uiTextBox1.Text = UID;
                yxgg = true;
            }
        }
    }
}
