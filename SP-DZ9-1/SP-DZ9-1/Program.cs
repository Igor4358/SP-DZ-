using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLibrary;
    
namespace GeometryApp

{ 
    class Program
        {
            static void Main(string[] args)
            {
                double squareSide = 5.0;
                double rectangleWidth = 4.0;
                double rectangleHeight = 6.0;
                double triangleBase = 3.0;
                double triangleHeight = 7.0;

                Console.WriteLine($"Площадь квадрата {squareSide} это {Geometry.SquareArea(squareSide)}");
                Console.WriteLine($"Площадь прямоугольника {rectangleWidth} и высота {rectangleHeight} это {Geometry.RectangleArea(rectangleWidth, rectangleHeight)}");
                Console.WriteLine($"Площадь треугольника {triangleBase} и высота {triangleHeight} это {Geometry.TriangleArea(triangleBase, triangleHeight)}");
            }
        }
    }