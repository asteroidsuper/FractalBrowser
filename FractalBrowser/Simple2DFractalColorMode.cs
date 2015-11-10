using System;
using System.Drawing;

namespace FractalBrowser
{
    public class Simple2DFractalColorMode:FractalColorMode
    {
        /*__________________________________________________________________Конструкторы_класса__________________________________________________________________*/
        #region Constructors
        public Simple2DFractalColorMode(double Red=10D,double Green=10D,double Blue=10D)
        {
            if (double.IsInfinity(Red) || double.IsNaN(Red) || double.IsInfinity(Green) || double.IsNaN(Green) || double.IsInfinity(Blue) || double.IsNaN(Blue)) throw new ArgumentException("Нельзя передавать в качестве атрибута цвета бесконечность или неопределённость!");
            _red = Math.Abs(Red);
            _green = Math.Abs(Green);
            _blue = Math.Abs(Blue);
        }

        #endregion /Constructors

        /*_________________________________________________________________Частные_данные_класса_________________________________________________________________*/
        #region Private data of class
        private double _red;
        private double _green;
        private double _blue;
        #endregion /Private data of class

        /*_____________________________________________________________Реализация_абстрактных_методов____________________________________________________________*/
        #region Realization abstract methods
        public override System.Drawing.Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP,object Extra=null)
        {
            if (!FAP.Is2D) throw new ArgumentException("Данный цветовой режим может визуализировать только двухмерные фракталы!");
            int width=FAP.Width, height=FAP.Height;
            ulong[][] matrix = FAP._2DIterMatrix;
            int iter_count;
            System.Drawing.Bitmap Result = new System.Drawing.Bitmap(width, height);
            int y;
            for(int x=0;x<width;x++)
            {
                for(y=0;y<height;y++)
                {
                    iter_count = (int)matrix[x][y];
                    Result.SetPixel(x, y, Color.FromArgb((int)(iter_count*_red)%256,(int)(iter_count*_green)%256,(int)(iter_count*_blue)%256));;
                }
            }
            return Result;
        }

        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            return FAP.Is2D;
        }
        #endregion /Realization abstract methods

        /*________________________________________________________________Общедотупные_поля_класса_______________________________________________________________*/
        #region Public properties
        public double Red
        {
            get { return _red; }
            set { if (double.IsInfinity(value) || double.IsNaN(value))throw new ArgumentException("Нельзя передавать в качестве атрибута цвета бесконечность или неопределённость!");
            _red = Math.Abs(value);
            }
        }
        public double Green
        {
            get { return _green; }
            set { if (double.IsInfinity(value) || double.IsNaN(value))throw new ArgumentException("Нельзя передавать в качестве атрибута цвета бесконечность или неопределённость!");
            _green = Math.Abs(value);
            }
        }
        public double Blue
        {
            get { return _blue; }
            set
            {
                if (double.IsInfinity(value) || double.IsNaN(value)) throw new ArgumentException("Нельзя передавать в качестве атрибута цвета бесконечность или неопределённость!");
                _blue = Math.Abs(value);
            }
        }


        #endregion /Public properties

    }
    
}
