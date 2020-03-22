using System;
using System.Collections.Generic;
using System.Text;

namespace Snake1
{
    public class Coordinate
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

        public static bool operator >(Coordinate valueLeft, Coordinate valueRight)
        {
            return (valueLeft.X > valueRight.X && valueLeft.Y > valueRight.Y);
        }

        public static bool operator <(Coordinate valueLeft, Coordinate valueRight)
        {
            return (valueLeft.X < valueRight.X && valueLeft.Y < valueRight.Y);
        }

        public static Coordinate operator +(Coordinate valueLeft, Coordinate valueRight)
        {
            return new Coordinate(valueLeft.X + valueRight.X, valueLeft.Y + valueRight.Y);
        }

        public static Coordinate operator -(Coordinate valueLeft, Coordinate valueRight)
        {
            return new Coordinate(valueLeft.X - valueRight.X, valueLeft.Y - valueRight.Y);
        }

        public static Coordinate operator +(Coordinate valueLeft, double valueRight)
        {
            return new Coordinate(valueLeft.X + valueRight, valueLeft.Y + valueRight);
        }

        public static Coordinate operator -(Coordinate valueLeft, double valueRight)
        {
            return new Coordinate(valueLeft.X - valueRight, valueLeft.Y - valueRight);
        }

        public static bool operator ==(Coordinate valueLeft, Coordinate valueRight)
        {
            return valueLeft.X == valueRight.X && valueLeft.Y == valueRight.Y;
        }

        public static bool operator !=(Coordinate valueLeft, Coordinate valueRight)
        {
            return valueLeft.X != valueRight.X && valueLeft.Y != valueRight.Y;
        }
    }
}
