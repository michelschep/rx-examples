using System;
using System.IO;
using System.Net.Sockets;

namespace TcpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
             using (var client = new TcpClient("localhost", 51111))
             using (var n = client.GetStream())
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
