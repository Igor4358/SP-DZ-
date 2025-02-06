using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_DZ_5_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                RunTasks();
                Console.WriteLine(" все закончили");
            }

             void RunTasks()
            {
                // Start
                Task task1 = new Task(DisplayCurrentDateTime);
                task1.Start();
                task1.Wait();

                // StartNew
                Task task2 = Task.Factory.StartNew(DisplayCurrentDateTime);
                task2.Wait();

                // Run
                Task task3 = Task.Run(DisplayCurrentDateTime);
                task3.Wait();
            }

             void DisplayCurrentDateTime()
            {
                Console.WriteLine($"Текущая {DateTime.Now}");
            }
        }
    }
}
