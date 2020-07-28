using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_GET
{
    class Program
    {
        static void Main(string[] args)
        {
            //UdpClient server = new UdpClient(new IPEndPoint(IPAddress.Parse("172.28.110.243"), 6666));
            UdpClient server = new UdpClient(6666);
            //server.JoinMulticastGroup(IPAddress.Parse("235.5.5.11"), 20);
            IPEndPoint ip = null;
            while (true)
            {
                Console.WriteLine("Готов ....");
                byte[] data = server.Receive(ref ip);
                Console.WriteLine(Encoding.UTF8.GetString(data) + " - " + ip.Address + " - " + ip.Port);

            }
            server.Close();
            Console.ReadKey();
        }
    }
}
