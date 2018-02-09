using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 客户端
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Socket socketClient = LinSocket();



            //不停的接收服务器端发送的消息
            Thread thread = new Thread(Recive);
            thread.IsBackground = true;
            thread.Start(socketClient);

            //不停的给服务器发送数据
            while (true)
            {
                //i++;
                //var buffter = Encoding.UTF8.GetBytes($"Test Send Message:{i}");
                //var temp = socketClient.Send(buffter);
                string a = Console.ReadLine();
                var cc = socketClient.Send(Encoding.UTF8.GetBytes($"测试使用{a}"));
                Thread.Sleep(1000);
            }
        }

        private static Socket LinSocket()
        {
            //创建实例
            Socket socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);

            IPAddress ip = IPAddress.Parse("192.168.188.177");
            //创建监听地址和端口
            IPEndPoint point = new IPEndPoint(ip, 2333);
            //进行连接
            bool aa = true;
            while (aa)
            {
                try
                {
                    Console.WriteLine("等待连接.....");
                    aa = false;
                    socketClient.Connect(point);
                    Console.WriteLine("连接成功！");

                }
                catch (Exception)
                {
                    aa = true;
                }
            }

            return socketClient;
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        static void Recive(object o)
        {
            var send = o as Socket;
            while (true)
            {
                //获取发送过来的消息
                byte[] buffer = new byte[1024 * 1024 * 2];
                int effective=0;
                while (true)
                {
                    try
                    {
                        effective = send.Receive(buffer);
                        if (effective == 0)
                        {
                            break;
                        }
                        var str = Encoding.UTF8.GetString(buffer, 0, effective);
                        Console.WriteLine(str);
                    }
                    catch (Exception)
                    {
                        LinSocket();
                        throw;
                    }
                }
            }
        }
    }
}
