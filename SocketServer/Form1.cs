using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketServer
{
    public partial class Form1 : Form
    {
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;
        Socket sListener;
        Socket handler;

        public Form1()
        {
            InitializeComponent();
        }

        public void ServerSynch()
        {
            ipHost = Dns.GetHostEntry(tb_ip.Text);
            ipAddr = ipHost.AddressList[1];
            ipEndPoint = new IPEndPoint(ipAddr, int.Parse(tb_port.Text));
            // Создаем сокет Tcp/Ip
            sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                //lb_server.Items.Add("Слушаем: " + ipEndPoint);

                // Начинаем слушать соединения
                while (true)
                {
                    //Application.DoEvents();


                    // Программа приостанавливается, ожидая входящее соединение
                    handler = sListener.Accept();
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    // Показываем данные на консоли
                    //lb_server.Items.Add("Получено от клиента: " + data + Environment.NewLine);

                    // Отправляем ответ клиенту\
                    //string reply = "Получено от клиента " + data.Length.ToString() + " символов";
                    string reply = "Спасибо за " + data;
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    //if (data.IndexOf("<TheEnd>") > -1)
                    //{
                    //    lb_server.Items.Add("Сервер завершил соединение с клиентом.");
                    //    //break;
                    //}

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                lb_server.Items.Add(ex.ToString());
            }
            finally
            {
                //Console.ReadLine();
            }
        }



        public async Task StartServerAsynch()
        {
            await Task.Run(() =>
            {
                ipHost = Dns.GetHostEntry(tb_ip.Text);
                ipAddr = ipHost.AddressList[1];
                ipEndPoint = new IPEndPoint(ipAddr, int.Parse(tb_port.Text));
                sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //sListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                try
                {
                    sListener.Bind(ipEndPoint);
                    sListener.Listen(10);
                    lb_server.Items.Add("Слушаем: " + ipEndPoint);

                    // Начинаем слушать соединения
                    while (true)
                    {
                        byte[] bytes = new byte[1024 * 1];



                        handler = sListener.Accept();

                        string data = null;


                        int bytesRec = handler.Receive(bytes);

                        var st = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        lb_server.Items.Add(st);

                        data += Encoding.UTF8.GetString(bytes, 0, bytesRec);





                        string reply = "Спасибо за " + data;
                        byte[] msg = Encoding.UTF8.GetBytes(reply);
                        handler.Send(msg);


                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
                catch (Exception ex)
                {
                    //lb_server.Items.Add(ex.ToString());
                }
                finally
                {
                    //Console.ReadLine();
                }

            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartServerAsynch();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartTCPServer();
        }

        public async Task StartTCPServer()
        {
            await Task.Run(() =>
            {
                TcpListener tcpListener = null;
                try
                {
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                    tcpListener = new TcpListener(localAddr, int.Parse(tb_port.Text) + 1);

                    // запуск слушателя
                    tcpListener.Start();

                    while (true)
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        NetworkStream stream = client.GetStream();

                        StreamReader reader = new StreamReader(stream);
                        string message = reader.ReadLine();
                        lb_server.Items.Add("Получено: " + message);

                        StreamWriter writer = new StreamWriter(stream);                        
                        writer.WriteLine(tb_text.Text + message);

                        writer.Close();
                        reader.Close();
                        stream.Close();
                        client.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    if (tcpListener != null)
                        tcpListener.Stop();
                }
            });
        }
    }


}
