using System;

namespace FractalBrowser
{
    [Serializable]
    public class Complex:ICloneable
    {
        public double Real, Imagine;
        public Complex getclone()
        {

            return new Complex(Real, Imagine);
        }
        public Complex()
        {
            Real = 0; Imagine = 0;
        }
        public Complex(double real, double imagine)
        {
            this.Real = real;
            this.Imagine = imagine;
        }
        public Complex sqr()
        {
            return new Complex(Real * Real - Imagine * Imagine, 2 * Real * Imagine);
        }
        public void tsqr()
        {
            double rea = Real * Real - Imagine * Imagine;
            Imagine *= Real * 2;
            Real = rea;
        }
        public Complex Pow(double degree)
        {
            if (Real == 0 && Imagine == 0) return 0;
            double argz = Math.Atan(Imagine / Real) * degree;
            double md = abs;
            md = Math.Pow(md, degree);
            return new Complex(md * Math.Cos(argz), md * Math.Sin(argz));
        }
        public void tpow(double power)
        {
            if (Real == 0 && Imagine == 0) return;
            double argz = Math.Atan(Imagine / Real) * power;
            double md = Math.Pow(abs, power);
            Real = md * Math.Cos(argz);
            Imagine = md * Math.Sin(argz);
        }
        public Complex Sin { get { return new Complex(Math.Cosh(Imagine) * Math.Sin(Real), Math.Cos(Real) * Math.Sinh(Imagine)); } }
        public Complex Cos { get { return new Complex(Math.Cos(Real) * Math.Cosh(Imagine), Math.Sin(Real) * Math.Sinh(Imagine)); } }
        public Complex Tan
        {
            get
            {
                double re = Real * 2, im = Imagine * 2, cs = Math.Cos(re) + Math.Cosh(im);
                return new Complex(Math.Sin(re) / cs, Math.Sinh(im) / cs);
            }
        }
        public Complex Ctan
        {
            get
            {
                double re = Real * 2, im = Imagine * 2, cs = Math.Cos(re) - Math.Cosh(im);
                return new Complex(Math.Sin(re) / cs, Math.Sinh(im) / cs);
            }
        }
        public Complex Sinh { get { return new Complex(Math.Cos(Imagine) * Math.Sinh(Real), Math.Cosh(Real) * Math.Sin(Imagine)); } }
        public Complex Cosh { get { return new Complex(Math.Cos(Imagine) * Math.Cosh(Real), Math.Sinh(Real) * Math.Sin(Imagine)); } }
        public Complex Tanh()
        {
            return new Complex(Math.Tanh(Real), Math.Cos(Imagine) * Math.Sin(Imagine) / 2);
        }
        public Complex Ln { get { return new Complex(Math.Log(abs), Math.Atan(Imagine / Real)); } }
        public Complex Log(double Base)
        {
            double l = Math.Log(Base);
            return new Complex(Math.Log(abs) / (2 * l), Math.Atan(Imagine / Real) / l);
        }
        public Complex Exp()
        {
            return new Complex(Math.Exp(Real) * Math.Cos(Imagine), Math.Sin(Imagine) * Math.Exp(Real));
        }
        public double abs
        {
            get { return Math.Sqrt(Real * Real + Imagine * Imagine); }
        }
        public static Complex operator +(Complex arg1, Complex arg2)
        {
            return new Complex(arg1.Real + arg2.Real, arg1.Imagine + arg2.Imagine);
        }
        public static Complex operator *(Complex carg1, Complex carg2)
        {
            return new Complex(carg1.Real * carg2.Real - carg1.Imagine - carg2.Imagine, carg1.Real * carg2.Imagine + carg2.Real * carg1.Imagine);
        }
        public static implicit operator Complex(double arg)
        {
            return new Complex(arg, 0);
        }
        public static explicit operator double(Complex arg)
        {
            return arg.Real;
        }
        public static Complex operator /(Complex arg1, Complex arg2)
        {
            double div = arg2.Real * arg2.Real + arg2.Imagine * arg2.Imagine;
            return new Complex((arg1.Real * arg2.Real + arg1.Imagine * arg2.Imagine) / div, (arg2.Real * arg1.Imagine - arg2.Imagine * arg1.Real) / div);
        }
        public static Complex operator -(Complex arg1, Complex arg2)
        {
            return new Complex(arg1.Real - arg2.Real, arg1.Imagine - arg2.Imagine);
        }

        object ICloneable.Clone()
        {
            return getclone();
        }
    }
}