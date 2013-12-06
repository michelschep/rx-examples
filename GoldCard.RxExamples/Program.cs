using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace GoldCard.RxExamples
{
    class Program
    {
        static void Main()
        {
            // Example1();
            //Example2();
            //Example3();
            //Example4();
            //Example5();
            //Example6();
            Example7();

            Console.WriteLine("Press the ANY key please.");
            Console.ReadLine();
        }

        /// <summary>
        /// Observe source that emits stream of characters
        /// </summary>
        private static void Example1()
        {
            var observable = ObservableStreamOfCharacters("Hello World");

            observable.Subscribe(DoSomethingWithChar);
        }

        private static IObservable<char> ObservableStreamOfCharacters(string content)
        {
            return content.ToObservable();
        }

        private static void Example2()
        {
            var observable = "Hello World"
                .ToObservable()
                .Delay(new TimeSpan(0, 0, 0, 5));

            observable.Subscribe(DoSomethingWithChar);
        }

        private static void Example3()
        {
            var observable = new[] { 1, 2, 3, 4, 5, 6, 7, 8 }
                .ToObservable()
                .Where(x => x % 2 == 0)
                .Delay(new TimeSpan(0, 0, 0, 5));

            observable.Subscribe(DoSomethingWithInteger);
        }

        private static void Example4()
        {
            var observable = Observable.Generate(
                                    0,
                                    x => x < 1000,
                                    x => x + 1,
                                    x => x,
                                    x => TimeSpan.FromSeconds(3.0))
                .Where(x => x % 2 == 0)
                .Delay(new TimeSpan(0, 0, 0, 5));

            observable.Subscribe(DoSomethingWithInteger);
        }

        private static void Example5()
        {
            var observable = Observable.Generate(
                                    0,
                                    x => x < 1000,
                                    x => x + 1,
                                    x => x,
                                    x => TimeSpan.FromSeconds(1.0))
                .Delay(new TimeSpan(0, 0, 0, 5))
                .Throttle(TimeSpan.FromSeconds(2));

            observable.Subscribe(DoSomethingWithInteger);
        }

        private static void Example6()
        {
            var observable = new[] { 10, 12, 20, 24, 30, 6, 7, 8 }
                .ToObservable()
                .Buffer(2)
                .Where(x => x[1] > 1.10 * x[1])
                .Select(x => x[1])
                .Delay(new TimeSpan(0, 0, 0, 5));

            observable.Subscribe(DoSomethingWithInteger);
        }

        private static void Example7()
        {
            var observable = AexStream();
                //.Where(t=>t.Contains("ajax"))
                //.Throttle(TimeSpan.FromSeconds(2));

            observable.Subscribe(WriteTweet);
        }

        private static void WriteTweet(string tweet)
        {
            Console.WriteLine(tweet);
        }


        //        private static void Example5() {
        //            var observable = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
        //                .ToObservable()
        //                .GroupBy(x => x % 2 == 0)
        //                .Delay(new TimeSpan(0, 0, 0, 5));
        //
        //            observable.Subscribe(DoSomethingWithInteger);
        //        } 

        private static IObservable<string> AexStream()
        {

            return Observable.Create<string>(
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
                            o.OnNext(message);
                        }
                    }
                    listener.Stop();
                    o.OnCompleted();
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
