using System;
using System.Windows.Forms;
using System.Drawing;
namespace FractalBrowser
{
    [Serializable]
    public class My2DClassicColorMode:FractalColorMode,IColorReturnable
    {
        /*______________________________________________________________________Конструкторы_класса______________________________________________________________*/
        #region Constructors of class
        public My2DClassicColorMode(double Red=1.85D,double Green=1.4D,double Blue=1.8D,double Muller=1D)
        {
            if (Red == 0 || double.IsInfinity(Red) || double.IsNaN(Red) || Green == 0 || double.IsInfinity(Green) || double.IsNaN(Green) || Blue == 0 || double.IsInfinity(Blue) || double.IsNaN(Blue)) throw new ArgumentException("Посылаемые значение не могут быть равными нулю, неопределённости и бескончености!");
            this.Red = Math.Abs(Red);
            this.Green = Math.Abs(Green);
            this.Blue = Math.Abs(Blue);
            this.Muller = Muller;
            
        }

        #endregion /Constructors of class

        /*_________________________________________________________________________Данные_класса_________________________________________________________________*/
        #region Data of class
        public double Red,Green,Blue,Muller;
        #endregion /Data of class

        /*__________________________________________________________________Реализация_абстрактных_методов_______________________________________________________*/
        #region Realization abstract methods
        public override Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP,object Extra=null)
        {
            int width=FAP.Width, height=FAP.Height, x, y;
            Bitmap Result = new Bitmap(width,height);
            ulong[][] iter_matrix=FAP.Get2DOriginalIterationsMatrix();
            ulong iter;
            for(x=0;x<width;x++)
            {
                for(y=0;y<height;y++)
                {
                    iter = (ulong)(iter_matrix[x][y]*Muller);
                    Result.SetPixel(x, y, Color.FromArgb((255 - iter / Red) >= 0 ? (int)(255 - iter / Red) : 0,
                                                         (255 - iter / Green) >= 0 ? (int)(255 - iter / Green) : 0,
                                                         (255 - iter / Blue) >= 0 ? (int)(255 - iter / Blue) : 0));
                }
            }
            return Result;
        }

        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            return FAP.Is2D;
        }
        public override System.Windows.Forms.Panel GetUniqueInterface(int width,int height)
        {
            _fcm_data_changed -= receiver;
            Panel result = new Panel();
            result.Size = new Size(width, height);
            _add_standart_rgb_trackbar(result, 0, 10000, (int)(Red*100), Color.Red,5, 1, 3);
            _add_standart_rgb_trackbar(result, 10, 10000, (int)(Green*100), Color.Green,5, 1, 3);
            _add_standart_rgb_trackbar(result, 20, 10000, (int)(Blue*100), Color.Blue,5, 1, 3);
            _add_standart_rgb_trackbar(result, 30, 10000, (int)(Muller * 100), Color.White,5, 1, 3);
            _fcm_data_changed += receiver;
            return result;
        }
        public override FractalColorMode GetClone()
        {
            return new My2DClassicColorMode(Red, Green, Blue, Muller);
        }
        #endregion /Realization abstract methods


        
        private void receiver(object Value,int ui,Control control)
        {
            switch(ui)
            {
                case 0:
                    {
                        int value = (int)Value;
                        Red =value/100D;
                        break;
                    }
                case 10:
                    {
                        int value = (int)Value;
                        Green = value/100D;
                        break;
                    }
                case 20:
                    {
                        int value = (int)Value;
                        Blue =value/100D;
                        break;
                    }
                case 30:
                    {
                        Muller = (int)Value / 100D;
                        break;
                    }

            }
            _fcm_on_FractalColorModeChangedHandler();
        }
        
        public override string ToString()
        {
            return "Classical color mode";
        }

        Color IColorReturnable.GetColor(object optimizer, int X, int Y)
        {
            ulong iter = (ulong)(((ulong[][])optimizer)[X][Y]*Muller);
            return Color.FromArgb((255 - iter / Red) >= 0 ? (int)(255 - iter / Red) : 0,
                                  (255 - iter / Green) >= 0 ? (int)(255 - iter / Green) : 0,
                                  (255 - iter / Blue) >= 0 ? (int)(255 - iter / Blue) : 0);
        }

        object IColorReturnable.Optimize(FractalAssociationParametrs FAP, object Extra)
        {
            return FAP.Get2DOriginalIterationsMatrix();
        }
    }
}
