using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
             using (TcpClient client = new TcpClient("localhost", 51111))
             using (NetworkStream n = client.GetStream())
             {
                 var writer = new BinaryWriter(n);
                 while (true)
                 {
                     var tweet = Console.ReadLine();
                     writer.Write(tweet);
                     writer.Flush();
                 }
             }
        }
    }
}
