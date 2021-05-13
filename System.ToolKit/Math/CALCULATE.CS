using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace System.ToolKit
{
    public class Calculate
    {
        public static ArcParam<double> ArcCalculate(Point<double> startPoint, Point<double> secondPoint, Point<double> endPoint)
        {
            double a, b, c, d, e, f;
            ArcParam<double> Arc;
            try
            {
                if (startPoint.X == secondPoint.X && startPoint.Y == secondPoint.Y)
                    throw new Exception("第一组数据与第二组数据相同！");
                if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y)
                    throw new Exception("第一组数据与第三组数据相同！");
                if (secondPoint.X == endPoint.X && secondPoint.Y == endPoint.Y)
                    throw new Exception("第二组数据与第三组数据相同！");
                double Dir;
                Dir = (secondPoint.X - startPoint.X) * (endPoint.Y - secondPoint.Y) 
                    - (secondPoint.Y - startPoint.Y) * (endPoint.X - secondPoint.X);
                if (Dir > 0) Arc.DIR = -1;
                else if (Dir < 0) Arc.DIR = 0;
                else throw new Exception("三个点坐标在同一直线上！");

                a = 2 * (secondPoint.X - startPoint.X);
                b = 2 * (secondPoint.Y - startPoint.Y);
                c = secondPoint.X * secondPoint.X + secondPoint.Y * secondPoint.Y
                    - startPoint.X * startPoint.X - startPoint.Y * startPoint.Y;
                d = 2 * (endPoint.X - secondPoint.X);
                e = 2 * (endPoint.Y - endPoint.Y);
                f = endPoint.X * endPoint.X + endPoint.Y * endPoint.Y
                    - secondPoint.X * secondPoint.X - secondPoint.Y * secondPoint.Y;
                Arc.X = (b * f - e * c) / (b * d - e * a);
                Arc.Y = (d * c - a * f) / (b * d - e * a);
                Arc.R = Math.Sqrt((Arc.X - startPoint.X) * (Arc.X - startPoint.X)
                    + (Arc.Y - startPoint.Y) * (Arc.Y - startPoint.Y));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return Arc;
        }
        public static ArcParam<double> ArcCalculate(Point3D<double> startPoint, Point3D<double> secondPoint, Point3D<double> endPoint)
        {
            double a, b, c, d, e, f;
            ArcParam<double> Arc;
            try
            {
                if (startPoint.X == secondPoint.X && startPoint.Y == secondPoint.Y)
                    throw new Exception("第一组数据与第二组数据相同！");
                if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y)
                    throw new Exception("第一组数据与第三组数据相同！");
                if (secondPoint.X == endPoint.X && secondPoint.Y == endPoint.Y)
                    throw new Exception("第二组数据与第三组数据相同！");
                double Dir;
                Dir = (secondPoint.X - startPoint.X) * (endPoint.Y - secondPoint.Y)
                    - (secondPoint.Y - startPoint.Y) * (endPoint.X - secondPoint.X);
                if (Dir > 0) Arc.DIR = -1;
                else if (Dir < 0) Arc.DIR = 0;
                else throw new Exception("三个点坐标在同一直线上！");

                a = 2 * (secondPoint.X - startPoint.X);
                b = 2 * (secondPoint.Y - startPoint.Y);
                c = secondPoint.X * secondPoint.X + secondPoint.Y * secondPoint.Y
                    - startPoint.X * startPoint.X - startPoint.Y * startPoint.Y;
                d = 2 * (endPoint.X - secondPoint.X);
                e = 2 * (endPoint.Y - endPoint.Y);
                f = endPoint.X * endPoint.X + endPoint.Y * endPoint.Y
                    - secondPoint.X * secondPoint.X - secondPoint.Y * secondPoint.Y;
                Arc.X = (b * f - e * c) / (b * d - e * a);
                Arc.Y = (d * c - a * f) / (b * d - e * a);
                Arc.R = Math.Sqrt((Arc.X - startPoint.X) * (Arc.X - startPoint.X)
                    + (Arc.Y - startPoint.Y) * (Arc.Y - startPoint.Y));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return Arc;
        }
    }
}
