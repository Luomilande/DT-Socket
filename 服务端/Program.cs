using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 服务端
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Socket serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Any;
            ///创建ip地址和端口
            IPEndPoint point = new IPEndPoint(ip, 2333);
            //Socket绑定监听地址
            serverSocket.Bind(point);
            Console.WriteLine("Listen Success");
            //设置同时连接个数
            serverSocket.Listen(10);

            //利用线程后台执行监听,否则程序会假死
            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start(serverSocket);
            //不停的给服务器发送数据
            while (true)
            {
                //i++;
                //var buffter = Encoding.UTF8.GetBytes($"Test Send Message:{i}");
                //var temp = socketClient.Send(buffter);
                string a = Console.ReadLine();
                 serverSocket.Accept();
                var cc = serverSocket.Send(Encoding.UTF8.GetBytes($"测试使用{a}"));
              
                Thread.Sleep(1000);
            }
        }
        static void Listen(object o)
        {
            var serverSocket = o as Socket;
            while (true)
            {
                //等待连接并且创建一个负责通讯的socket
                var send = serverSocket.Accept();
                //获取链接的IP地址
                var sendIpoint = send.RemoteEndPoint.ToString();
                Console.WriteLine($"{sendIpoint}已连接。");
                //开启一个新线程不停接收消息
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(send);

            }
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
                //获取发送过来的消息容器
                byte[] buffer = new byte[1024 * 1024 * 2];
                var effective = send.Receive(buffer);
                //有效字节为0则跳过
                if (effective == 0)
                {
                    break;
                }
                var str = Encoding.UTF8.GetString(buffer, 0, effective);
                Console.WriteLine(str);
                //string a = Console.ReadLine();
                var buffers = Encoding.UTF8.GetBytes($"收到！");
                send.Send(buffers);
                Thread thread = new Thread(Recive2);
                thread.IsBackground = true;
                thread.Start(send);

            }
        }
        static void Recive2(object o)
        {
            var send = o as Socket;
            string a = Console.ReadLine();
            var buffers = Encoding.UTF8.GetBytes(a);
            send.Send(buffers);
        }
    }
}
