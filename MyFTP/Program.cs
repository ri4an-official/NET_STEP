using FluentFTP;
using Renci.SshNet;
using System;
using System.IO;
using System.Net;

namespace MyFTP
{
    class Program
    {
        private static void WebClientUpload()
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential("MBaibatyrov", "4R5t6y7u_");
                client.UploadFile("ftp://192.168.31.30/TCP.pptx.gz", WebRequestMethods.Ftp.UploadFile, @"C:\DISTR\NetCat\TCP.pptx.gz");
            }
        }

        private static void WebClientDownload()
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential("MBaibatyrov", "4R5t6y7u_");
                client.DownloadFile("ftp://192.168.31.30/TCP.pptx.gz", "TCP123.pptx.gz");
            }
        }

        private static void FtpWebRequestDownload()
        {
            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create("ftp://192.168.31.30/TCP.pptx.gz");
            ftp.Credentials = new NetworkCredential("MBaibatyrov", "4R5t6y7u_");
            ftp.Method = WebRequestMethods.Ftp.DownloadFile;
            using (Stream ftpStream = ftp.GetResponse().GetResponseStream())
            using (Stream fileStream = File.Create("TCP.pptx.gz"))
            {
                ftpStream.CopyTo(fileStream);
            }
        }
        private static void FtpWebRequestUpload()
        {
            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create("ftp://192.168.31.30/TCP.pptx.gz");
            ftp.Credentials = new NetworkCredential("MBaibatyrov", "4R5t6y7u_");
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            var bytes = File.ReadAllBytes("TCP.pptx.gz");
            ftp.ContentLength = bytes.Length;
            using (Stream stream = ftp.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private static void FtpClientDownload()
        {
            FtpClient client = new FtpClient();
            client.Host = "192.168.31.30";
            client.Credentials = new NetworkCredential("MBaibatyrov", "4R5t6y7u_");
            client.Connect();
            FtpStatus Status = client.DownloadFile("TCP.pptx.gz", "/sdf/fTCP.pptx.gz");
        }

        private static void FtpClientUpload()
        {
            FtpClient client = new FtpClient();
            client.Host = "192.168.31.30";
            client.Credentials = new NetworkCredential("MBaibatyrov", "4R5t6y7u_");
            client.Connect();
            var Status = client.UploadFile("TCP.pptx.gz", "/TCP.pptx.gz");
        }


        private static void FtpClientBoth()
        {
            FtpClient client = new FtpClient();
            client.Host = "ftp://ftp.dlptest.com/";
            client.Credentials = new NetworkCredential("dlpuser@dlptest.com", "eUj8GeW55SvYaswqUyDSm5v6N");
            client.Connect();
            var Status = client.UploadFile("TCP.pptx.gz", "/TCP.pptx.gz");
            Console.WriteLine($"UploadFile: {Status.ToString()}");
            client.Rename("/TCP.pptx.gz", "/TCP666666.pptx.gz");

            Status = client.DownloadFile("TCP666666.pptx.gz", "/TCP666666.pptx.gz");
            Console.WriteLine($"DownloadFile: {Status.ToString()}");


            ////client.DeleteFile("/TCP666666.pptx.gz");
        }

        private static void FtpClientDelete()
        {
            FtpClient client = new FtpClient();
            client.Host = "ftp://ftp.dlptest.com/";
            client.Credentials = new NetworkCredential("dlpuser@dlptest.com", "eUj8GeW55SvYaswqUyDSm5v6N");
            client.Connect();
            client.DeleteFile("/TCP666666.pptx.gz");
        }


        static void Main(string[] args)
        {
            //WebClientUpload();
            //WebClientDownload();
            //FtpWebRequestDownload();
            //FtpWebRequestUpload();
            //FtpClientDownload();
            //FtpClientUpload();
            //FtpClientBoth();
            //FtpClientDelete();
            SFTP();

        }


        private static void SFTP()
        {
            String Host = "test.rebex.net";
            int Port = 22;
            String RemoteFileName = "FluentFTP.dll";
            String LocalDestinationFilename = "readme666.txt";
            String Username = "demo";
            String Password = "password";

            using (var sftp = new SftpClient(Host, Port, Username, Password))
            {
                sftp.Connect();

                //var files = sftp.ListDirectory("//");

                //foreach (var file in files)
                //{
                //    Console.WriteLine(file.Name);
                //}

                //using (var file = File.OpenRead(LocalDestinationFilename))
                //{
                //    sftp.UploadFile(file, RemoteFileName);
                //}



                using (var file = File.OpenWrite(LocalDestinationFilename))
                {
                    sftp.DownloadFile("readme.txt", file);
                }

                sftp.Disconnect();
            }
        }
    }
}
