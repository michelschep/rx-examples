using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace AexServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Start AEX Server");

            var fondsen = new Fonds[] {new Fonds("Aegon", 385.14)};

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
                            writer.Write(fondsen[0].ToString());
                            writer.Flush();
                            Thread.Sleep(100);
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
    }

    internal class Fonds
    {
        private readonly string _name;
        private readonly double _koers;

        public Fonds(string name, double koers)
        {
            _name = name;
            _koers = koers;
        }

        public string Name
        {
            get { return _name; }
        }

        public double Koers
        {
            get { return _koers; }
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", Name, Koers);
        }
    }
}
