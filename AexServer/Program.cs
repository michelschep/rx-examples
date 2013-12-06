using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Aex;

namespace AexServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Start AEX Server");

            var fondsen = new Fonds[] {new Fonds("Aegon", 385.14m), new Fonds("Air France KLM", 7.282m), new Fonds("ING Groep", 9.242m), new Fonds("TNT Express", 6.298m)  };

            while (true)
            {
                try
                {
                    using (var client = new TcpClient("localhost", 51111))
                    using (var n = client.GetStream())
                    {
                        var writer = new BinaryWriter(n);
                        while (true)
                        {
                            EmitKoersen(fondsen, writer);
                      
                            Thread.Sleep(2000);
                        }
                    }
                }
                catch
                {
                    // ignore and wait some time
                    Thread.Sleep(1000);
                }
            }
        }

        private static void EmitKoersen(Fonds[] fondsen, BinaryWriter writer)
        {
            var random = new Random();
            Console.WriteLine("");
            foreach (var fonds in fondsen)
            {
                var change = (decimal) random.NextDouble();
                var sign = random.Next(-1, 2);

                fonds.Koers = fonds.Koers + sign * change;
                writer.Write(fonds.ToString());
                writer.Flush();
                Console.WriteLine(fonds);
            }
        }
    }
}
