using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace AlGoLiv.OOP
{
    //https://www.interviewsansar.com/print-even-and-odd-number-sequence-with-two-threads-in-csharp/
    public class PrintOddEvenTwoThreadImproved{

        const int NUM = 10;
        static object monitor = new object();
        
        public void Print()
        {

            Thread t1 = new Thread(PrintOdd);
            Thread t2 = new Thread(PrintEven);

            
            t1.Start();
            //puase for 10 ms, to make sure even/odd thread has started
            //or else odd thread may start first resulting other sequence.
            Thread.Sleep(100);

            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("\nPrinting done!!!");




        }
        private void PrintOdd()
        {
            try
            {
                //hold lock as console is shared between threads.
                Monitor.Enter(monitor);
                
                for(int i=1; i<=NUM; i+=2)
                {
                    //Complete the task ( printing odd number on console)
                    Console.WriteLine("Thread {0} - {1}", Thread.CurrentThread.ManagedThreadId.ToString(), i);

                    //Notify other thread that is to even thread
                    //that I'm done you do your job
                    Monitor.Pulse(monitor);


                    ////I will wait here till even thread notify me
                    //// without this logic application will wait forever
                    bool isLast = i == NUM-1;
                    if (!isLast)
                        Monitor.Wait(monitor); //I will wait here till even thread notify me




                }

            }
            finally
            {
                //Release lock

                Monitor.Exit(monitor);
            }

        }

        private void PrintEven()
        {
            try
            {
                //hold lock as console is shared between threads.
                Monitor.Enter(monitor);

                for (int i = 2; i <= NUM; i += 2)
                {
                    //Complete the task ( printing odd number on console)
                    Console.WriteLine("Thread {0} - {1}", Thread.CurrentThread.ManagedThreadId.ToString(), i);

                    //Notify other thread that is to even thread
                    //that I'm done you do your job
                    Monitor.Pulse(monitor);

                    
                    
                    ////I will wait here till even thread notify me
                    //// without this logic application will wait forever
                    bool isLast = i == NUM;
                    if (!isLast)
                        Monitor.Wait(monitor); //I will wait here till even thread notify me




                }

            }
            finally
            {
                //Release lock

                Monitor.Exit(monitor);
            }

        }
    }

    public class PrintOddEvenTwoThread
    {
        int counter = 1;
        private int _n;

        static object monitor = new object();
        public PrintOddEvenTwoThread()
        {
            _n = 10;


        }
        public void Print()
        {
            Thread t = new Thread(PrintOdd);
            t.Start();

            Thread.Sleep(100);

            Thread t1 = new Thread(PrintEven);
            t1.Start();

            //Task task1 = Task.Factory.StartNew(() => PrintOdd());

            //Task task2 = Task.Factory.StartNew(() => PrintEven());
            //Task.WaitAll(task1, task2);

            //t.Join();
            //t1.Join();
        }

        private void PrintOdd()
        {

            //while (counter <= _n)
            //{
            //    if (counter % 2 != 0)
            //    {
            //        Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString()+"{0}","-"+counter);
            //        counter++;
            //    }

            //}

            lock (monitor)
            {
                while (counter < _n)
                {
                    if (counter % 2 == 0)
                    {
                        Monitor.Wait(monitor);
                    }
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "{0}", "-" + counter);
                    counter++;
                    Monitor.Pulse(monitor);

                }
            }
        }

        private void PrintEven()
        {
            //while (counter <= _n)
            //{
            //    if (counter % 2 == 0)
            //    {
            //        Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString()+"{0}","-"+counter);
            //        counter++;
            //    }

            //}
            lock (monitor)
            {
                while (counter <= _n)
                {
                    if (counter % 2 != 0)
                    {
                        Monitor.Wait(monitor);

                    }
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "{0}", "-" + counter);
                    counter++;
                    Monitor.Pulse(monitor);

                }
            }

        }
    }
    }

