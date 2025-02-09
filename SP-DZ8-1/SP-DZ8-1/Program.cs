using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SP_DZ8_1
{
class Program
        {
            static void Main()
            {
                string filePath = "Probnic.txt";
                var lines = File.ReadAllLines(filePath);
                var numbers = lines.Select(int.Parse).ToList();
                int uniqueCount = numbers.AsParallel().Distinct().Count();
                Console.WriteLine($"Количество уникальных значений: {uniqueCount}");
            }
        }
    }