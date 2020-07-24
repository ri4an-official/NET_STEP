using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async Task Client(int Port)
        {
            await Task.Run(() =>
            {
                byte[] bytes = new byte[1024];

                // Соединяемся с удаленным устройством

                // Устанавливаем удаленную точку для сокета
                IPHostEntry ipHost = Dns.GetHostEntry(tb_ip.Text);
                IPAddress ipAddr = ipHost.AddressList[1];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, int.Parse(tb_port.Text));
                // Создаем сокет Tcp/Ip
                Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                // Соединяем сокет с удаленной точкой
                sender.Connect(ipEndPoint);

                string message = tb_text.Text;

                //lb_client.Items.Add("Сокет соединяется с " +sender.RemoteEndPoint.ToString());
                byte[] msg = Encoding.UTF8.GetBytes(message);

               

                // Отправляем данные через сокет
                int bytesSent = sender.Send(msg);

                // Получаем ответ от сервера
                int bytesRec = sender.Receive(bytes);

                lb_client.Items.Add("Ответ от сервера: " + Encoding.UTF8.GetString(bytes, 0, bytesRec));

                // Освобождаем сокет
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            });
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Client(int.Parse(tb_port.Text));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartTCPClient();
        }

        public async Task StartTCPClient()
        {

            await Task.Run(() =>
            {
                TcpClient client = null;
                try
                {
                    string message = tb_text.Text;
                    client = new TcpClient("127.0.0.1", int.Parse(tb_port.Text) + 1);
                    NetworkStream stream = client.GetStream();

                    // отправляем сообщение
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine(message);
                    writer.Flush();

                    // BinaryReader reader = new BinaryReader(new BufferedStream(stream));
                    StreamReader reader = new StreamReader(stream);
                    message = reader.ReadLine();
                    lb_client.Items.Add("Получен ответ: " + message);

                    reader.Close();
                    writer.Close();
                    stream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (client != null)
                        client.Close();
                }

            });

        }
    }
}
