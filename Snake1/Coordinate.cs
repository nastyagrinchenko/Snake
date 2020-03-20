using System;
using System.Collections.Generic;
using System.Text;

namespace Snake1
{
    class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordinate(double x = 0, double y = 0)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"Coordinates X: {this.X}, Y: {this.Y}";
        }

        public static bool operator >(Coordinate valueLeft, double valueRight)
        {
            return (valueLeft.X > valueRight || valueLeft.Y > valueRight);
        }

        public static bool operator <(Coordinate valueLeft, double valueRight)
        {
            return (valueLeft.X < valueRight || valueLeft.Y < valueRight);
        }
    }
}
