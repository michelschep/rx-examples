using System;
using System.CodeDom;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Aex;

namespace GoldCard.RxExamples
{
    class Program
    {
        static void Main()
        {
            ExampleCountBytes();

//            ExampleShowAllStocks();
//            ExampleShowAllStocksWithDelay();
//            ExampleShowOneSpecificStock();
//            ExampleShowOnlyStockChanges();

            //ExampleWithFastChars();
            //ExampleWithIntegerStreamAndDelay();
            //ExampleWithThrottle();

            Console.WriteLine("Press the ANY key please.");
            Console.ReadLine();
        }

        private static void ExampleCountBytes()
        {
            var observable = ByteStream();


            long counter = 0;
            observable.Subscribe(i => ++counter, () => Console.WriteLine(counter));
        }

        private static void ExampleShowAllStocks()
        {
            var observable = AexStream();

            observable.Subscribe(writeStock);
        }
        
        private static void ExampleShowAllStocksWithDelay()
        {
            var observable = AexStream().Delay(TimeSpan.FromSeconds(5));

            observable.Subscribe(writeStock);
        }

        private static void ExampleShowOneSpecificStock()
        {
            var observable = AexStream()
                .Where(stock => stock.Name.Contains("ING"));

            observable.Subscribe(writeStock);
        }

        private static void ExampleShowOnlyStockChanges()
        {
            var observable = AexStream()
                .GroupBy(stock => stock.Name); // create multiple observable streams!

            observable.Subscribe(company =>
                {
                    var changedStocks = company.Buffer(2); // Keep the last two stock changes

                    changedStocks.Subscribe(stocks =>
                        {
                            var diff = ((stocks[1].Koers - stocks[0].Koers) / stocks[0].Koers) * 100;
                            
                            if (diff > 1)
                                Console.WriteLine("HIGHER  {0} {1:N2}%", stocks[0].Name.PadRight(20), diff);
                            else if (diff < -1)
                                Console.WriteLine("LOWER   {0} {1:N2}%", stocks[0].Name.PadRight(20), diff);
                            //                            else
                            //                                Console.WriteLine("EQUAL: {0}", s[0].Name);
                        });
                }
                );
        }

        /// <summary>
        /// Observe source that emits stream of characters
        /// </summary>
        private static void ExampleWithFastChars()
        {
            var observable = "Hello World".ToObservable();

            observable.Subscribe(DoSomethingWithChar);
        }

        private static void ExampleWithIntegerStreamAndDelay()
        {
            var observable = new[] { 1, 2, 3, 4, 5, 6, 7, 8 }
                .ToObservable()
                .Where(x => x % 2 == 0)
                .Delay(new TimeSpan(0, 0, 0, 5));

            observable.Subscribe(DoSomethingWithInteger);
        }

        private static void ExampleWithThrottle()
        {
            var observable = Observable.Generate(
                                    0,
                                    x => x < 1000000000,
                                    x => x + 1,
                                    x => x,
                                    x => TimeSpan.FromMilliseconds(1))
                .Throttle(TimeSpan.FromMilliseconds(5));

            observable.Subscribe(DoSomethingWithInteger);
        }

        private static void writeStock(Stock stock)
        {
            Console.WriteLine("{0} Stock: {1}", DateTime.Now.ToLongTimeString(), stock);
        }


        private static IObservable<Stock> AexStream()
        {

            return Observable.Create<Stock>(
                o =>
                {
                    Console.WriteLine("Start AEX listener");

                    var listener = new TcpListener(IPAddress.Any, 51111);
                    listener.Start();

                    using (var client = listener.AcceptTcpClient())
                    using (var n = client.GetStream())
                    {
                        while (true)
                        {
                            var message = new BinaryReader(n).ReadString();
                            o.OnNext(new Stock(message));
                        }
                    }
                    listener.Stop();
                    o.OnCompleted();
                    return Disposable.Create(() => Console.WriteLine("--Disposed--"));
                });
        }

        private static IObservable<int> ByteStream()
        {

            return Observable.Create<int>(
                o =>
                {
                    Console.WriteLine("Start byte listener");

//                    using (var fs = new FileStream(@"C:\data\test.txt", FileMode.Open))
                    using (var fs = new FileStream(@"C:\data\en_visual_studio_ultimate_2013_x86_dvd_3175319.iso", FileMode.Open))
                    {
                        int b;
                        while ((b = fs.ReadByte()) != -1)
                        {
                            o.OnNext(b);
                        }

                    }
                    o.OnCompleted();
                    //        o.OnNext(b);
                    return Disposable.Create(() => Console.WriteLine("--Disposed--"));
                });
        }

        private static void DoSomethingWithInteger(int i)
        {
            Console.WriteLine(i);
        }

        private static void DoSomethingWithChar(char c)
        {
            Console.WriteLine(c);
        }
    }
}
