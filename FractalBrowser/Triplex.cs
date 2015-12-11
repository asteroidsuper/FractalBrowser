using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    [Serializable]
    public class Triplex : ICloneable
    {
        public double x, y, z;
        public Triplex(){}
        public Triplex(double x, double y, double z)
        {

            this.x = x;
            this.y = y;
            this.z = z;
        }
        public double Radius
        {
            get { return Math.Sqrt(x * x + y * y + z * z); }
        }
        public double Theta
        {
            get { return Math.Atan2(Math.Sqrt(x * x + y * y), z); }
        }
        public double Phi
        {
            get { return Math.Atan2(y, x); }
        }
        public Triplex Pow(double Stepen)
        {
            double phin = Phi * Stepen + Math.PI, thetan = Theta * Stepen + Math.PI / 2, rn = Math.Pow(Radius, Stepen);
            return new Triplex(rn * Math.Sin(thetan) * Math.Cos(phin), rn * Math.Sin(thetan) * Math.Sin(phin), rn * Math.Cos(thetan));
        }
        public void tsqr()
        {
            double phin = Phi * 2 + Math.PI, thetan = Theta * 2 + Math.PI / 2, rn = Math.Pow(Radius, 2);
            x = rn * Math.Sin(thetan) * Math.Cos(phin);
            y = rn * Math.Sin(thetan) * Math.Sin(phin);
            z = rn * Math.Cos(thetan);
        }
        public void tadd(Triplex arg)
        {
            x += arg.x;
            y += arg.y;
            z += arg.z;
        }

        public static Triplex operator +(Triplex arg1, Triplex arg2)
        {
            return new Triplex(arg1.x + arg2.x, arg1.y + arg2.y, arg1.z + arg2.z);
        }
        public static Triplex operator +(Triplex arg1, double arg2)
        {
            return new Triplex(arg1.x + arg2, arg1.y, arg1.z);
        }
        public static Triplex operator +(double arg2, Triplex arg1)
        {
            return new Triplex(arg1.x + arg2, arg1.y, arg1.z);
        }
        public static Triplex operator *(double arg2, Triplex arg1)
        {
            return new Triplex(arg1.x * arg2, arg1.y * arg2, arg1.z * arg2);
        }
        public static Triplex operator *(Triplex arg1, double arg2)
        {
            return new Triplex(arg1.x * arg2, arg1.y * arg2, arg1.z * arg2);
        }
        public static implicit operator Triplex(double arg)
        {
            return new Triplex(arg, 0, 0);
        }
        public void Rotatex(double radian)
        {
            double ny = y * Math.Cos(radian) + z * Math.Sin(radian),
                   nz = -y * Math.Sin(radian) + z * Math.Cos(radian);
            y = ny;
            z = nz;
        }
        public void Rotatey(double radian)
        {
            double nx = x * Math.Cos(radian) - z * Math.Sin(radian),
                   nz = x * Math.Sin(radian) + z * Math.Cos(radian);
            x = nx;
            z = nz;

        }
        public void Rotatez(double radian)
        {
            double nx = x * Math.Cos(radian) + y * Math.Sin(radian),
                   ny = -x * Math.Sin(radian) + y * Math.Cos(radian);
            x = nx;
            y = ny;
        }
        public object Clone()
        {
            return new Triplex(x, y, z);
        }
        public static implicit operator Triplex(Complex arg)
        {
            return new Triplex(arg.Real, arg.Imagine, 0D);
        }
    }
}
