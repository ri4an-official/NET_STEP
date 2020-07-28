using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCP_CLIENT
{
    class Program
    {
        static Thread thread1;
        static Thread thread2;
        static Thread thread3;
        static Thread thread4;
        static Thread thread5;

        static Thread thread6;
        static Thread thread7;

        static TcpListener listen1;
        static TcpListener listen2;
        static TcpListener listen3;
        static TcpListener listen4;
        static UdpClient listen5;
        static TcpListener listen6;
        static TcpListener listen7;



        static void Main(string[] args)
        {

            //byte[] bytes = File.ReadAllBytes(@"C:\DISTR\NetCat\1.pdf");

            //using (FileStream fs = new FileStream(@"C:\DISTR\ШАГ\STEP_MVC\!!!!!СЕТЕВОЕ\CT_NP.gz", FileMode.OpenOrCreate))
            //{
            //    using (GZipStream gZipStream = new GZipStream(fs, CompressionMode.Compress, false))
            //    {
            //        gZipStream.Write(bytes, 0, bytes.Length);
            //    }
            //}

            //return;

            //getInfoTCP("HELLO STEP");
            //return;
            ////UdpClient client = new UdpClient("127.0.0.10", 9000);
            ////string message = "Hello world_";
            ////byte[] data = Encoding.UTF8.GetBytes(message);
            ////int numberOfSentBytes = client.Send(data, data.Length, null);
            ////client.Close();
            //getInfoTCP("Привет мир");

            listen1 = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 12345);
            listen2 = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 12346);
            listen3 = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 12347);
            listen4 = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 12348);
            listen5 = new UdpClient("127.0.0.1", 12350);
            listen6 = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 12351);
            listen7 = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 12352);

            thread1 = new Thread(new ThreadStart(StartServer1));
            thread2 = new Thread(new ThreadStart(StartServer2));
            thread3 = new Thread(new ThreadStart(StartServer3));
            thread4 = new Thread(new ThreadStart(StartServer4));
            thread5 = new Thread(new ThreadStart(StartServer5));
            thread6 = new Thread(new ThreadStart(StartServer6));
            thread7 = new Thread(new ThreadStart(StartServer7));

            //thread1.Start();
            //thread2.Start();
            //thread3.Start();
            //thread4.Start();
            //thread5.Start();
            thread6.Start();
            thread7.Start();
        }

        private static void StartServer1()
        {
            listen1.Start();
            try
            {
                while (true)
                {

                    Console.WriteLine("127.0.0.1:12345: started (XLSX)...");
                    Console.WriteLine("Server: Waited");
                    TcpClient client = listen1.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();


                    //var buffer = new byte[256];
                    //var readBytes = 0;
                    //while ((readBytes = stream.Read(buffer, 0, buffer.Length)) > 0)
                    //{
                    //    //do something with data in buffer, up to the size indicated by bytesRead
                    //}
                    //for (int i = 0; i < buffer.Length; i++)
                    //{
                    //    Console.WriteLine(buffer[i]);
                    //}


                    //byte[] bHeader1 = buffer.Take<byte>(10).ToArray();
                    //byte[] bHeader2 = buffer.Skip<byte>(10).Take<byte>(100).ToArray();
                    //byte[] bContent = bytes.Skip<byte>(200).ToArray();




                    BinaryReader br = new BinaryReader(stream);
                    byte[] arr = br.ReadBytes(10);
                    Console.WriteLine("Получено: " + string.Join(";", arr.ToList()));
                    byte[] bHeader1 = arr.Take<byte>(5).ToArray();
                    byte[] bHeader2 = arr.Skip<byte>(5).Take<byte>(5).ToArray();




                    //StreamReader reader = new StreamReader(stream);
                    //string message = reader.ReadLine();
                    //Console.WriteLine("Получено: " + message);


                    //StreamWriter writer = new StreamWriter(stream);
                    //writer.WriteLine(message);

                    //WriteLine(Encoding.UTF8.GetString(arr));
                    //using (var ms = new MemoryStream())
                    //{
                    //    TextWriter tw = new StreamWriter(ms);
                    //    tw.Write(System.Text.Encoding.UTF8.GetString(bHeader1) + "_");
                    //    tw.Write(System.Text.Encoding.UTF8.GetString(bHeader2) + "_");
                    //    tw.Flush();
                    //    ms.Position = 0;
                    //    arr = ms.ToArray();
                    //    BinaryWriter bw = new BinaryWriter(stream);
                    //    bw.Write(arr);
                    //}


                    //writer.Close();
                    //reader.Close();

                    using (var ms = new MemoryStream())
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Field1", typeof(string));
                        dt.Columns.Add("Field2", typeof(string));
                        for (int i = 0; i < 10; i++)
                        {
                            //Thread.Sleep(200);
                            dt.Rows.Add(new object[] { Encoding.UTF8.GetString(bHeader1), Encoding.UTF8.GetString(bHeader2) });
                        }
                        var workbook = new XLWorkbook();
                        workbook.Worksheets.Add(dt, "TcpReport");
                        workbook.SaveAs(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        //arr = ms.ToArray();
                        BinaryWriter bw = new BinaryWriter(stream);
                        bw.Write(ms.ToArray());
                        Thread.Sleep(500);
                    }

                    stream.Close();
                    client.Close();
                }

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

        }
        private static void StartServer2()
        {
            listen2.Start();
            try
            {
                while (true)
                {

                    Console.WriteLine("127.0.0.1:12346: started (ENCRYPT)...");
                    TcpClient client = listen2.AcceptTcpClient();

                    using (NetworkStream stream = client.GetStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string message = reader.ReadLine();
                            Console.WriteLine("Получено: " + message);
                            var sEncrypted = Sha.Encrypt(message.Split(' ')[0], message.Split(' ')[1]);
                            using (StreamWriter writer = new StreamWriter(stream))
                            {
                                writer.WriteLine(sEncrypted);
                            }

                        }
                    }
                    Thread.Sleep(500);
                    client.Close();
                }

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

        }
        private static void StartServer3()
        {
            listen3.Start();
            try
            {
                while (true)
                {

                    Console.WriteLine("127.0.0.1:12347: started (DECRYPT)...");
                    TcpClient client = listen3.AcceptTcpClient();
                    using (NetworkStream stream = client.GetStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string message = reader.ReadLine();
                            Console.WriteLine("Получено: " + message);

                            var sDecrypted = Sha.Decrypt(message.Split(' ')[0], message.Split(' ')[1]);
                            using (StreamWriter writer = new StreamWriter(stream))
                            {
                                writer.WriteLine(sDecrypted);
                            }

                        }
                    }
                    Thread.Sleep(500);
                    client.Close();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

        }

        private static void StartServer4()
        {
            listen4.Start();
            try
            {
                while (true)
                {

                    Console.WriteLine("127.0.0.1:12348: started (Aryphmetic)...");
                    TcpClient client = listen4.AcceptTcpClient();
                    using (NetworkStream stream = client.GetStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string message = reader.ReadLine();
                            Console.WriteLine("Получено: " + message);
                            using (StreamWriter writer = new StreamWriter(stream))
                            {
                                string result = null;
                                string[] Params = message.Split(' ');
                                switch (Params[0])
                                {
                                    case "+":
                                        result = (int.Parse(Params[1]) + int.Parse(Params[2])).ToString();
                                        break;
                                    case "*":
                                        result = (int.Parse(Params[1]) * int.Parse(Params[2])).ToString();
                                        break;
                                    case "-":
                                        result = (int.Parse(Params[1]) - int.Parse(Params[2])).ToString();
                                        break;
                                    case "/":
                                        result = (int.Parse(Params[1]) / int.Parse(Params[2])).ToString();
                                        break;

                                    default:
                                        result = null;
                                        break;
                                }
                                writer.WriteLine(result);
                            }

                        }
                    }
                    Thread.Sleep(500);
                    client.Close();

                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

        }

        private static void StartServer5()
        {
            IPEndPoint remoteIp = null;
            var listen5 = new UdpClient(12350);
            //listen5 = new UdpClient(6666);
            try
            {
                while (true)
                {

                    Console.WriteLine("127.0.0.1:12350: UDP started...");
                    byte[] data_receive = listen5.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.UTF8.GetString(data_receive);
                    Console.WriteLine("Получено: ", message);



                }

                //byte[] data_send = Encoding.UTF8.GetBytes("Спасибо за: ");
                //Sender.Send(data_send, data_send.Length); // отправка
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            finally
            {
                listen5.Close();
            }

        }

        private static void StartServer6()
        {
            listen6.Start();
            try
            {
                while (true)
                {
                    Console.WriteLine("127.0.0.1:12351: started (compress)...");
                    TcpClient client = listen6.AcceptTcpClient();
                    using (NetworkStream stream = client.GetStream())
                    {
                        BinaryReader br = new BinaryReader(stream);
                        List<byte> lBytes = new List<byte>();
                        do
                        {
                            byte b = (byte)br.ReadByte();
                            lBytes.Add(b);
                        }
                        while (stream.DataAvailable);
                        Console.WriteLine("Байтов до компрессии: " + lBytes.Count.ToString());

                        byte[] bytes = lBytes.ToArray();



                        using (var Stream = new MemoryStream())
                        {
                            using (var tinyStream = new GZipStream(Stream, CompressionMode.Compress))
                            using (var mStream = new MemoryStream(bytes))
                                mStream.CopyTo(tinyStream);
                            using (BinaryWriter bw = new BinaryWriter(stream))
                            {
                                byte[] compressed = Stream.ToArray();
                                bw.Write(compressed);
                                Console.WriteLine("Байтов после компрессии: " + compressed.Length);

                            }
                        }




                        Thread.Sleep(500);
                        stream.Close();
                        client.Close();
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }


        }

        private static void StartServer7()
        {
            listen7.Start();
            try
            {
                while (true)
                {
                    Console.WriteLine("127.0.0.1:12352: started (decompress)...");
                    TcpClient client = listen7.AcceptTcpClient();
                    using (NetworkStream stream = client.GetStream())
                    {
                        BinaryReader br = new BinaryReader(stream);
                        List<byte> lBytes = new List<byte>();
                        do
                        {
                            byte b = (byte)br.ReadByte();
                            lBytes.Add(b);
                        }
                        while (stream.DataAvailable);
                        Console.WriteLine("Байтов в архиве: " + lBytes.Count.ToString());

                        byte[] bytes = lBytes.ToArray();
                        using (var Stream = new MemoryStream(bytes))
                        {
                            using (var tinyStream = new GZipStream(Stream, CompressionMode.Decompress))
                            using (var mStream = new MemoryStream())
                            {
                                tinyStream.CopyTo(mStream);
                                using (BinaryWriter bw = new BinaryWriter(stream))
                                {
                                    byte[] decompressed = mStream.ToArray();
                                    bw.Write(decompressed);
                                    Console.WriteLine("Байтов после деархивации: " + decompressed.Length);

                                }
                            }
                        }




                        Thread.Sleep(500);
                        stream.Close();
                        client.Close();
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }


        }


        private static void StartClient(object client)
        {
            // Read data
            TcpClient tClient = (TcpClient)client;

            Console.WriteLine("Client (Thread: {0}): Connected!", Thread.CurrentThread.ManagedThreadId);
            do
            {
                if (!tClient.Connected)
                {
                    tClient.Close();
                    Thread.CurrentThread.Abort();       // Kill thread.
                }

                byte[] bytes = new byte[1024];
                int istart = 0;

                MemoryStream ms = new MemoryStream();
                BinaryWriter binaryWriter = new BinaryWriter(ms);

                while (tClient.Available > 0)
                {
                    tClient.GetStream().Read(bytes, istart, 1024);

                }


                //if (tClient.Available > 0)
                //{
                //    // Resend
                //    byte pByte = (byte)tClient.GetStream().ReadByte();
                //    Console.WriteLine("Client (Thread: {0}): Data {1}", Thread.CurrentThread.ManagedThreadId, pByte);
                //    tClient.GetStream().WriteByte(pByte);
                //}

                // Pause
                Thread.Sleep(100);
                //tClient.Close();
            } while (true);

        }

        private static void getInfoTCP(string message)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient("tcpbin.com", 4242);
                NetworkStream stream = client.GetStream();


                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                    Console.WriteLine("Отправлено по TextStream: " + message);
                    // BinaryReader reader = new BinaryReader(new BufferedStream(stream));
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        message = reader.ReadLine();
                        Console.WriteLine("Получено по TextStream: " + message);
                    }
                }


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
        }



    }
}
