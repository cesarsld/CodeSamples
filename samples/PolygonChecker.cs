using System;
using System.Linq;

namespace CodeSubmission
{
    public class Point2D
    {
        public int X;
        public int Y;

        // is the distance between the point's coordinats and the origin (0;0)
        public double Distance 
        {
            get
            {
                return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            }
        }

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double DistanceFromOther(Point2D otherPoint)
        {
            int relativeX = X - otherPoint.X;
            int relativeY = Y - otherPoint.Y;
            return Math.Sqrt(Math.Pow(relativeX, 2) + Math.Pow(relativeY, 2));
        }
    }

    class PolygonChecker
    {
        public Point2D[] ReturnAscendingOrder(Point2D[] pointArray)
        {
            pointArray = pointArray.OrderBy(point => point.Distance).ToArray();
            return pointArray;
        }

        public int FindRotation2(Point2D pointA, Point2D pointB, Point2D pointC)
        {
            int twiceSignedArea = (pointB.X - pointA.X) * (pointC.Y - pointA.Y) - (pointB.Y - pointA.Y) * (pointC.X - pointA.X);

            // anti clockwise
            if (twiceSignedArea > 0) return 1;
            // collinear
            if (twiceSignedArea == 0) return 0;
            // clockwise
            return -1; 
        }

        //lines are infinite so only need to check line coefficients
        public bool IsLineIntersecting(Point2D pointA, Point2D pointB, Point2D pointC, Point2D pointD)
        {
            float lineABCoef = (pointB.Y - pointA.Y) / (pointB.X - pointA.X);
            float lineCDCoef = (pointD.Y - pointC.Y) / (pointD.X - pointC.X);
            if (lineABCoef == lineCDCoef)
            {
                float lineACCoef = (pointC.Y - pointA.Y) / (pointC.X - pointA.X);
                // checks if both lines are collinear
                if (lineABCoef == lineACCoef) return true; 
                return false;
            }
            return true;
        }

        public bool IsOnEdge(Point2D pointA, Point2D pointB, Point2D startPoint)
        {
            if (FindRotation2(pointA, startPoint, pointB) == 0)
            {
                int minX, minY, maxX, maxY;
                if (pointA.Y > pointB.Y)
                {
                    maxY = pointA.Y;
                    minY = pointB.Y;
                }
                else
                {
                    minY = pointA.Y;
                    maxY = pointB.Y;
                }
                if (pointA.X > pointB.X)
                {
                    maxX = pointA.X;
                    minX = pointB.X;
                }
                else
                {
                    minX = pointA.X;
                    maxX = pointB.X;
                }
                // check if startpoint is in between point A and B in the X and Y axis
                if (startPoint.Y <= maxY && startPoint.Y >= minY && startPoint.X <= maxX && startPoint.X >= minX) return true;
            }
            return false;
        }

        public bool IsInPolygon(Point2D[] polygon, Point2D startPoint)
        {
            int j = polygon.Length - 1;
            bool isIn = false;
            for (int i = 0; i < polygon.Length; i++)
            {
                // check that the intersection point's Y coordinate has to be between the 2 vertices tested
                // 2nd condition checks if the intersection of ray is made right or left of tested point which is true if on the right
                if ((polygon[i].Y > startPoint.Y) != (polygon[j].Y > startPoint.Y) &&
                     startPoint.X < (polygon[j].X - polygon[i].X) * (startPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X) 
                {
                    isIn = !isIn;
                }
                // test for edge cases
                if (IsOnEdge(polygon[j], polygon[i], startPoint)) return true;
                j = i;
            }
            return isIn;
        }
    }
}
