using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldCard.RxExamples {
    class Program {
        static void Main(string[] args) {
            //Example1();
            //Example2();
            //Example3();
            //Example4();
            Example6();

            Console.WriteLine("Press the ANY key please.");
            Console.ReadLine();
        }

        private static void Example1() {
            var observable = "Hello World".ToObservable();

            observable.Subscribe(DoSomething);
        }

        private static void Example2() {
            var observable = "Hello World"
                .ToObservable()
                .Delay(new TimeSpan(0, 0, 0, 5));

            observable.Subscribe(DoSomething);
        }

        private static void Example3() {
            var observable = new[] { 1, 2, 3, 4, 5, 6, 7, 8 }
                .ToObservable()
                .Where(x => x % 2 == 0)
                .Delay(new TimeSpan(0, 0, 0, 5));

            observable.Subscribe(DoSomethingWithInt);
        }

        private static void Example4() {
            var observable = Observable.Generate(
                                    0,
                                    x => x < 1000,
                                    x => x + 1,
                                    x => x,
                                    x => TimeSpan.FromSeconds(3.0))
                .Where(x => x % 2 == 0)
                .Delay(new TimeSpan(0, 0, 0, 5));

            observable.Subscribe(DoSomethingWithInt);
        }

        private static void Example5() {
            var observable = Observable.Generate(
                                    0,
                                    x => x < 1000,
                                    x => x + 1,
                                    x => x,
                                    x => TimeSpan.FromSeconds(1.0))
                .Delay(new TimeSpan(0, 0, 0, 5))
                .Throttle(TimeSpan.FromSeconds(2));

            observable.Subscribe(DoSomethingWithInt);
        }
        
        /// <summary>
        /// Buffer
        /// </summary>
        private static void Example6() {
            var observable = new[] { 10, 12, 20, 24, 30, 6, 7, 8 }
                .ToObservable()
                .Buffer(2)
                .Where(x=>x[1] > 1.10 * x[1])
                .Select(x=>x[1])
                .Delay(new TimeSpan(0, 0, 0, 5));

            observable.Subscribe(DoSomethingWithInt);
        }
        //        private static void Example5() {
        //            var observable = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
        //                .ToObservable()
        //                .GroupBy(x => x % 2 == 0)
        //                .Delay(new TimeSpan(0, 0, 0, 5));
        //
        //            observable.Subscribe(DoSomethingWithInt);
        //        }

        private static void DoSomethingWithInt(int i) {
            Console.WriteLine(i);
        }

        private static void DoSomething(char c) {
            Console.WriteLine(c);
        }
    }
}
