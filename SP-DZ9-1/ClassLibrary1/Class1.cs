using System;
namespace GeometryLibrary
{
    public class Geometry
    {
        public static double SquareArea(double side)
        {
            return side * side;
        }
        public static double RectangleArea(double width,double height)
        {
            return width * height;
        }
        public static double TriangleArea(double baseLength,double height)
        {
            return 0.5* baseLength * height;
        }
    }
}
