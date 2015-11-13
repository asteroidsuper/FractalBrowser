using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace FractalBrowser
{
    public class My2DClassicColorMode:FractalColorMode
    {
        /*______________________________________________________________________Конструкторы_класса______________________________________________________________*/
        #region Constructors of class
        public My2DClassicColorMode(double Red=1.85D,double Green=1.4D,double Blue=1.8D)
        {
            if (Red == 0 || double.IsInfinity(Red) || double.IsNaN(Red) || Green == 0 || double.IsInfinity(Green) || double.IsNaN(Green) || Blue == 0 || double.IsInfinity(Blue) || double.IsNaN(Blue)) throw new ArgumentException("Посылаемые значение не могут быть равными нулю, неопределённости и бескончености!");
            this.Red = Math.Abs(Red);
            this.Green = Math.Abs(Green);
            this.Blue = Math.Abs(Blue);
        }

        #endregion /Constructors of class

        /*_________________________________________________________________________Данные_класса_________________________________________________________________*/
        #region Data of class
        public double Red,Green,Blue;
        #endregion /Data of class

        /*__________________________________________________________________Реализация_абстрактных_методов_______________________________________________________*/
        #region Realization abstract methods
        public override Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP,object Extra=null)
        {
            int width=FAP.Width, height=FAP.Height, x, y;
            Bitmap Result = new Bitmap(width,height);
            ulong[][] iter_matrix=FAP.Get2DOriginalIterationsMatrix();
            for(x=0;x<width;x++)
            {
                for(y=0;y<height;y++)
                {
                    Result.SetPixel(x, y, Color.FromArgb((255 - iter_matrix[x][y] / Red) >= 0 ? (int)(255 - iter_matrix[x][y] / Red) : 0,
                                                         (255 - iter_matrix[x][y] / Green) >= 0 ? (int)(255 - iter_matrix[x][y] / Green) : 0,
                                                         (255 - iter_matrix[x][y] / Blue) >= 0 ? (int)(255 - iter_matrix[x][y] / Blue) : 0));
                   // Result.SetPixel(x, y, Color.FromArgb(((int)iter_matrix[x][y] * 10) % 255, ((int)iter_matrix[x][y] * 10) % 255, ((int)iter_matrix[x][y] * 10) % 255));
                }
            }
            return Result;
        }

        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            return FAP.Is2D;
        }
        #endregion /Realization abstract methods

    }
}
