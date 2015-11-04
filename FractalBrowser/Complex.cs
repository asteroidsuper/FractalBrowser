using System;
using System.Text;
using System.Drawing;

namespace FractalBrowser
{
    [Serializable]
    public class Complex
    {
        public double real, imagine;
        public Complex getclone()
        {

            return new Complex(real, imagine);
        }
        public Complex()
        {
            real = 0; imagine = 0;
        }
        public Complex(double real, double imagine)
        {
            this.real = real;
            this.imagine = imagine;
        }
        public Complex sqr()
        {
            return new Complex(real * real - imagine * imagine, 2 * real * imagine);
        }
        public void tsqr()
        {
            double rea = real * real - imagine * imagine;
            imagine *= real * 2;
            real = rea;
        }
        public Complex Pow(double degree)
        {
            if (real == 0 && imagine == 0) return 0;
            double argz = Math.Atan(imagine / real) * degree;
            double md = abs;
            md = Math.Pow(md, degree);
            return new Complex(md * Math.Cos(argz), md * Math.Sin(argz));
        }
        public void tpow(double power)
        {
            if (real == 0 && imagine == 0) return;
            double argz = Math.Atan(imagine / real) * power;
            double md = Math.Pow(abs, power);
            real = md * Math.Cos(argz);
            imagine = md * Math.Sin(argz);
        }
        public Complex Sin { get { return new Complex(Math.Cosh(imagine) * Math.Sin(real), Math.Cos(real) * Math.Sinh(imagine)); } }
        public Complex Cos { get { return new Complex(Math.Cos(real) * Math.Cosh(imagine), Math.Sin(real) * Math.Sinh(imagine)); } }
        public Complex Tan
        {
            get
            {
                double re = real * 2, im = imagine * 2, cs = Math.Cos(re) + Math.Cosh(im);
                return new Complex(Math.Sin(re) / cs, Math.Sinh(im) / cs);
            }
        }
        public Complex Ctan
        {
            get
            {
                double re = real * 2, im = imagine * 2, cs = Math.Cos(re) - Math.Cosh(im);
                return new Complex(Math.Sin(re) / cs, Math.Sinh(im) / cs);
            }
        }
        public Complex Sinh { get { return new Complex(Math.Cos(imagine) * Math.Sinh(real), Math.Cosh(real) * Math.Sin(imagine)); } }
        public Complex Cosh { get { return new Complex(Math.Cos(imagine) * Math.Cosh(real), Math.Sinh(real) * Math.Sin(imagine)); } }
        public Complex Tanh()
        {
            return new Complex(Math.Tanh(real), Math.Cos(imagine) * Math.Sin(imagine) / 2);
        }
        public Complex Ln { get { return new Complex(Math.Log(abs), Math.Atan(imagine / real)); } }
        public Complex Log(double Base)
        {
            double l = Math.Log(Base);
            return new Complex(Math.Log(abs) / (2 * l), Math.Atan(imagine / real) / l);
        }
        public Complex Exp()
        {
            return new Complex(Math.Exp(real) * Math.Cos(imagine), Math.Sin(imagine) * Math.Exp(real));
        }
        public double abs
        {
            get { return Math.Sqrt(real * real + imagine * imagine); }
        }
        public static Complex operator +(Complex arg1, Complex arg2)
        {
            return new Complex(arg1.real + arg2.real, arg1.imagine + arg2.imagine);
        }
        public static implicit operator Point(Complex arg)
        {
            return new Point((int)Math.Round(arg.real), (int)Math.Round(arg.imagine));
        }
        public static implicit operator Complex(Point arg)
        {
            return new Complex(arg.X, arg.Y);
        }
        public static Complex operator +(Point arp, Complex arg)
        {
            return new Complex(arp.X + arg.real, arp.Y + arg.imagine);
        }
        public static Complex operator *(Complex carg1, Complex carg2)
        {
            return new Complex(carg1.real * carg2.real - carg1.imagine - carg2.imagine, carg1.real * carg2.imagine + carg2.real * carg1.imagine);
        }
        public static implicit operator Complex(double arg)
        {
            return new Complex(arg, 0);
        }
        public static explicit operator double(Complex arg)
        {
            return arg.real;
        }
        public static Complex operator /(Complex arg1, Complex arg2)
        {
            double div = arg2.real * arg2.real + arg2.imagine * arg2.imagine;
            return new Complex((arg1.real * arg2.real + arg1.imagine * arg2.imagine) / div, (arg2.real * arg1.imagine - arg2.imagine * arg1.real) / div);
        }
        public static Complex operator -(Complex arg1, Complex arg2)
        {
            return new Complex(arg1.real - arg2.real, arg1.imagine - arg2.imagine);
        }
    }
}