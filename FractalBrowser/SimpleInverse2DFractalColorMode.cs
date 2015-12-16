﻿using System;
using System.Drawing;
using System.Windows.Forms;
namespace FractalBrowser
{
    [Serializable]
    public class SimpleInverse2DFractalColorMode : FractalColorMode,IColorReturnable
    {
        /*__________________________________________________________________Конструкторы_класса__________________________________________________________________*/
        #region Constructors
        public SimpleInverse2DFractalColorMode(double Red = 10D, double Green = 10D, double Blue = 10D)
        {
            if (double.IsInfinity(Red) || double.IsNaN(Red) || double.IsInfinity(Green) || double.IsNaN(Green) || double.IsInfinity(Blue) || double.IsNaN(Blue)) throw new ArgumentException("Нельзя передавать в качестве атрибута цвета бесконечность или неопределённость!");
            _red = Math.Abs(Red);
            _green = Math.Abs(Green);
            _blue = Math.Abs(Blue);
            _fcm_data_changed += Processor;
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
        public override System.Drawing.Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP, object Extra = null)
        {
            if (!FAP.Is2D) throw new ArgumentException("Данный цветовой режим может визуализировать только двухмерные фракталы!");
            int width = FAP.Width, height = FAP.Height;
            ulong[][] matrix = FAP._2DIterMatrix;
            int iter_count;
            System.Drawing.Bitmap Result = new System.Drawing.Bitmap(width, height);
            int y;
            for (int x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    iter_count = (int)matrix[x][y];
                    Result.SetPixel(x, y, Color.FromArgb(255 - (int)(iter_count * _red) % 256, 255 - (int)(iter_count * _green) % 256,255- (int)(iter_count * _blue) % 256)); ;
                }
            }
            return Result;
        }

        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            return FAP.Is2D;
        }
        public override System.Windows.Forms.Panel GetUniqueInterface(int width, int height)
        {
            Panel Result = new Panel();
            Result.Size = new Size(width, height);
            _add_standart_rgb_trackbar(Result, 0, 1000, (int)(_red * 10), Color.Red, 5, 1, 5);
            _add_standart_rgb_trackbar(Result, 10, 1000, (int)(_green * 10), Color.Green,5,1,5);
            _add_standart_rgb_trackbar(Result, 20, 1000, (int)(_blue * 10), Color.Blue,5,1,5);
            return Result;
        }


        public override FractalColorMode GetClone()
        {
            return new SimpleInverse2DFractalColorMode(_red, _green, _blue);
        }
        #endregion /Realization abstract methods

        /*________________________________________________________________Общедотупные_поля_класса_______________________________________________________________*/
        #region Public properties
        public double Red
        {
            get { return _red; }
            set
            {
                if (double.IsInfinity(value) || double.IsNaN(value)) throw new ArgumentException("Нельзя передавать в качестве атрибута цвета бесконечность или неопределённость!");
                _red = Math.Abs(value);
            }
        }
        public double Green
        {
            get { return _green; }
            set
            {
                if (double.IsInfinity(value) || double.IsNaN(value)) throw new ArgumentException("Нельзя передавать в качестве атрибута цвета бесконечность или неопределённость!");
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

        /*_______________________________________________________________Частные_инструменты_класса______________________________________________________________*/
        #region Private utilities 
        private void Processor(object value, int ui, Control sender)
        {
            switch (ui)
            {
                case 0:
                    {
                        _red = ((int)value) / 10D;
                        break;
                    }
                case 10:
                    {
                        _green = ((int)value) / 10D;
                        break;
                    }
                case 20:
                    {
                        _blue = ((int)value) / 10D;
                        break;
                    }
            }
            _fcm_on_FractalColorModeChangedHandler();
        }
        #endregion /Private utilities

        /*_________________________________________________________________Реализация_интерфейсов________________________________________________________________*/
        #region Realization of interfaces
        public Color GetColor(object optimizer, int X, int Y)
        {
            ulong iter_count = ((ulong[][])optimizer)[X][Y];
            return Color.FromArgb(255 - (int)(iter_count * _red) % 256, 255 - (int)(iter_count * _green) % 256, 255 - (int)(iter_count * _blue) % 256);
        }

        public object Optimize(FractalAssociationParametrs FAP, object Extra = null)
        {
            return FAP.Get2DOriginalIterationsMatrix();
        }
        #endregion /Realization of interfaces
    }
}
