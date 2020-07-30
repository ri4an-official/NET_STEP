using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams
{
    class Program
    {
        private static byte[] getContent_1(string fileName)
        {
            byte[] Content = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                Content = new byte[fs.Length];
                fs.Read(Content, 0, (int)fs.Length);
            }
            return Content;
        }
        private static byte[] getContent_2(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }
        private static byte[] getContent_3(string fileName)
        {
            byte[] Content = null;
            using (FileStream fs = File.OpenRead(fileName))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    Content = br.ReadBytes((int)fs.Length);
                }
            }
            return Content;
        }

        private static byte[] getContent_4(string fileName)
        {
            byte[] Content = null;
            using (FileStream fs = File.OpenRead(fileName))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    Content = ms.ToArray();
                }

            }
            return Content;
        }

        private static void Binary()
        {
            string FileName = @"C:\Temp\Data.xlsx";
            var bytesContent = getContent_3(FileName);
            var bytesFileName = new byte[1000];
            var bytesTotal = new byte[bytesFileName.Length + bytesContent.Length];

            using (BinaryWriter bw = new BinaryWriter(new MemoryStream(bytesTotal)))
            {
                bw.Write(Encoding.UTF8.GetBytes(Path.GetFileName(FileName)));
                bw.Seek(999, SeekOrigin.Begin);
                bw.Write(bytesContent);
            }

            using (BinaryReader br = new BinaryReader(new MemoryStream(bytesTotal)))
            {
                var bytesTemp = br.ReadBytes(999);
                string FileNameTemp = Encoding.UTF8.GetString(bytesTemp).Replace("\0", string.Empty);
                bytesTemp = br.ReadBytes(bytesTotal.Length - 999 - 1);
                File.WriteAllBytes(@"C:\Temp\todo\" + FileNameTemp, bytesTemp);
            }
        }

        private static void Memory()
        {
            string FileName = @"C:\Temp\Data.xlsx";
            var bytesContent = getContent_3(FileName);
            var bytesFileName = new byte[1000];
            var bytesTotal = new byte[bytesFileName.Length + bytesContent.Length];

            byte[] total = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var bFIleName = Encoding.UTF8.GetBytes(Path.GetFileName(FileName));
                ms.Write(bFIleName, 0, bFIleName.Length);
                var b = ms.ToArray();
                ms.Position = 1000;
                ms.Write(bytesContent, 0, bytesContent.Length);
                total = ms.ToArray();
            }

            using (MemoryStream ms = new MemoryStream(total))
            {
                var bFIleName = new byte[1000];
                ms.Read(bFIleName, 0, 999);
                string FileNameTemp = Encoding.UTF8.GetString(bFIleName).Replace("\0", string.Empty);
                ms.Position = 1000;
                byte[] bContent = new byte[total.Length - 1000];
                ms.Read(bContent, 0, (int)ms.Length - 1000);
                File.WriteAllBytes(@"C:\Temp\todo\" + FileNameTemp, bContent);
            }


        }
        static void Main(string[] args)
        {
            Memory();
        }
    }
}
