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
        public My2DClassicColorMode(double Red=1.85D,double Green=1.4D,double Blue=1.8D,ulong dark=160)
        {
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            Dark = dark;
        }

        #endregion /Constructors of class

        /*_________________________________________________________________________Данные_класса_________________________________________________________________*/
        #region Data of class
        public double Red,Green,Blue;
        public ulong Dark;
        #endregion /Data of class

        /*__________________________________________________________________Реализация_абстрактных_методов_______________________________________________________*/
        #region Realization abstract methods
        public override Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP)
        {
            int width=FAP.Width, height=FAP.Height, x, y;
            Bitmap Result = new Bitmap(width,height);
            ulong[][] iter_matrix=FAP._2DIterMatrix;
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

        #endregion /Realization abstract methods
    }
}
