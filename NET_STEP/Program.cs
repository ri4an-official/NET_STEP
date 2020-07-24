using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NET_STEP
{
    class Program
    {
        private static void Download_pdf()
        {
            WebClient client = new WebClient();
            client.DownloadFile("http://www.africau.edu/images/default/sample.pdf", "myBook.pdf");
            Console.WriteLine("Файл загружен");
        }
        private static void Download_pdf_2()
        {
            WebClient client = new WebClient();

            using (Stream stream = client.OpenRead("https://www.w3.org/TR/PNG/iso_8859-1.txt"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }

            Console.WriteLine("Файл загружен");
            Console.Read();
        }

        private static void WebResponse_download()
        {
            WebRequest request = WebRequest.Create("https://www.w3.org/TR/PNG/iso_8859-1.txt");
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            response.Close();
            Console.WriteLine("Запрос выполнен");
            Console.Read();
        }

        private static async Task HeadersAsync()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.w3.org/TR/PNG/iso_8859-1.txt");
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            WebHeaderCollection headers = response.Headers;
            for (int i = 0; i < headers.Count; i++)
            {
                Console.WriteLine("{0}: {1}", headers.GetKey(i), headers[i]);
            }
            response.Close();
            Console.ReadLine();
        }


        private static void IPAddress_ ()
        {
            IPAddress ip = IPAddress.Parse("234.58.78.9");
            byte[] adress = ip.GetAddressBytes();
            string ipString = ip.ToString();
        }

        private static void Socket_server()
        {
            int port = 8005; // порт для приема входящих запросов
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    // отправляем ответ
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private static void Socket_client()
        {
            int port = 8005; // порт для приема входящих запросов
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                Console.Write("Введите сообщение:");
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                Console.WriteLine("ответ сервера: " + builder.ToString());

                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }


        static void Main(string[] args)
        {
            //var res = Dns.GetHostAddresses("mail.ru");
            //foreach (var item in res)
            //{
            //    Console.WriteLine(item.ToString());
            //}
            //Console.WriteLine(Dns.GetHostName());
            //var res = Dns.GetHostEntry("www.mail.ru");
            //Console.WriteLine(res.);

            //Uri uri = new Uri("https://kaspi.kz/shop/c/smartphones/class-2-sim-cards/");
            Uri mainUri = new Uri("https://kaspi.kz");

            //Uri secondUri = new Uri(mainUri, "/shop/c/smartphones/class-2-sim-cards/");

            Uri secondUri = new Uri("https://news.mail.ru/economics/42672044/?frommail=1");
        
            Console.WriteLine(secondUri.PathAndQuery);

            //Download_pdf();
            //IPAddress_();
            //Socket_server();
            //Socket_client();
        }
    }
}
