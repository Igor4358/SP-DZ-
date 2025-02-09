using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DS_SP6_1
{
    internal class Program
    {
            private static EventWaitHandle generationCompleted = new ManualResetEvent(false);
            private static EventWaitHandle sumCompleted = new ManualResetEvent(false);
            private static EventWaitHandle productCompleted = new ManualResetEvent(false);

         
            private static string numbersFile = "numbers.txt";
            private static string sumsFile = "sums.txt";
            private static string productsFile = "products.txt";

            static void Main(string[] args)
            {
               
                Thread generatorThread = new Thread(GenerateNumbers);
                Thread sumThread = new Thread(CalculateSums);
                Thread productThread = new Thread(CalculateProducts);

                generatorThread.Start();
                sumThread.Start();
                productThread.Start();

              
                generatorThread.Join();
                sumThread.Join();
                productThread.Join();

                Console.WriteLine("Все операции завершены.");
            }

            static void GenerateNumbers()
            {
                Random rand = new Random();
                using (StreamWriter writer = new StreamWriter(numbersFile))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int a = rand.Next(1, 100);
                        int b = rand.Next(1, 100);
                        writer.WriteLine($"{a} {b}");
                    }
                }
                Console.WriteLine("Генерация чисел завершена.");
                generationCompleted.Set(); 
            }

            static void CalculateSums()
            {
                generationCompleted.WaitOne(); 

                using (StreamReader reader = new StreamReader(numbersFile))
                using (StreamWriter writer = new StreamWriter(sumsFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(' ');
                        int a = int.Parse(parts[0]);
                        int b = int.Parse(parts[1]);
                        writer.WriteLine(a + b);
                    }
                }
                Console.WriteLine("Подсчет сумм завершен.");
                sumCompleted.Set(); 
            }

            static void CalculateProducts()
            {
                generationCompleted.WaitOne();

                using (StreamReader reader = new StreamReader(numbersFile))
                using (StreamWriter writer = new StreamWriter(productsFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(' ');
                        int a = int.Parse(parts[0]);
                        int b = int.Parse(parts[1]);
                        writer.WriteLine(a * b);
                    }
                }
                Console.WriteLine("Подсчет произведений завершен.");
                productCompleted.Set(); 
            }
        }
    }