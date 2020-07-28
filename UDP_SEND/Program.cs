using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_SEND
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient("127.0.0.1", 6666);
            
            //UdpClient client = new UdpClient();
            //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("235.5.5.11"), 6666);
            //client.Connect(6666);
            string message = "Hello world!";
            byte[] data = Encoding.UTF8.GetBytes(message);
            int numberOfSentBytes = client.Send(data, data.Length);
            //int numberOfSentBytes = client.Send(data, data.Length, endPoint);
            client.Close();
            Console.ReadKey();
        }
    }
}
