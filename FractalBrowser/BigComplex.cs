using System;
using System.Numerics;

namespace FractalBrowser
{
    public class BigComplex
    {
        public BigRational Real, Imagine;

        //Статичные данные для вычислений
        #region Static Atributes
        private static readonly BigRational _twice = new BigRational((BigInteger)2);


        #endregion Static Atributes


        #region Public Constructors
        public BigComplex()
        {
            Real = Imagine = BigRational.Zero;
        }
        public BigComplex(BigRational r, BigRational i)
        {
            Real = r;
            Imagine = i;
        }
        public BigComplex(double Re, double Im)
        {
            Real = BigRational.ConvertToBigRational(Re);
            Imagine = BigRational.ConvertToBigRational(Im);
        }
        public BigComplex(long value)
        {
            Real = new BigRational(value, 1);
            Imagine = BigRational.Zero;
        }
        #endregion Public Constructors

        #region Public methods
        /// <summary>
        /// Возводить число в квадрат
        /// </summary>
        /// <returns>Большое комплексное число возведенное в квадрат</returns>
        public BigComplex sqr()
        {
            return new BigComplex(Real * Real - Imagine * Imagine, _twice * Real * Imagine);
        }
        /// <summary>
        /// Возводить текущее число в квадрат
        /// </summary>
        public void t_sqr()
        {
            BigRational i = Imagine;
            Imagine *= Real * _twice;
            Real *= Real;
            Real -= i * i;
        }
        /// <summary>
        /// Возвращает модуль в квадрате
        /// </summary>
        /// <returns>Модуль, возведенный в квадрат, большого комплексного числа</returns>
        public BigRational SqrAbs()
        {
            return Real * Real + Imagine * Imagine;
        }


        #endregion Public methods

        /*______________________________________________________________Перегруженные_операторы_______________________________________________________*/
        #region  Overraded operators
        public static BigComplex operator +(BigComplex arg1, BigComplex arg2)
        {
            return new BigComplex(arg1.Real + arg2.Real, arg1.Imagine + arg2.Imagine);
        }

        #endregion Overraded operators
    }
}
