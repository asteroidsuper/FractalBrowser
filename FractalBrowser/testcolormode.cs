using System;
using System.Drawing;

namespace FractalBrowser
{
    public class testcolormode:FractalColorMode
    {

        public override System.Drawing.Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP, object Extra = null)
        {
            int width = FAP.Width, height = FAP.Height;
            Bitmap bmp = new Bitmap(width, height);
            double[][] dm = ((RadianMatrix)FAP.GetUniqueParameter(typeof(RadianMatrix))).Matrix;
            Color cl = Color.Black; ;
            ulong[][] iter_matrix=FAP.Get2DOriginalIterationsMatrix();
            ulong iter;
            int deg;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    iter = (ulong)(iter_matrix[x][y]*0.8);
                    cl =Color.FromArgb((255 - iter / 1.85) >= 0 ? (int)(255 - iter / 1.85) : 0,
                                                         (255 - iter / 1.4) >= 0 ? (int)(255 - iter / 1.4) : 0,
                                                         (255 - iter / 1.8) >= 0 ? (int)(255 - iter / 1.8) : 0);
                    
                    bmp.SetPixel(x, y,cl);
                    if (iter < 85) { 
                        deg = (int)((dm[x][y] / Math.PI) * 180);
                        if (deg < 0) deg = 360 + deg;
                        if (deg < 181) cl = Color.FromArgb(255 - 255 * deg / 180,255- 255 * deg / 180, 255 -255 * deg / 180);
                        else
                        {
                            deg -= 180;
                            cl = Color.FromArgb(255 * deg / 180, 255 * deg / 180,  255 * deg / 180);
                        }
                    
                    bmp.SetPixel(x, y,cl);}
                        
                }
            }
            return bmp; 
        }

        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            return FAP.Is2D && FAP.GetUniqueParameter(typeof(RadianMatrix)) != null;
        }

        public override System.Windows.Forms.Panel GetUniqueInterface(int width, int height)
        {
            return null;
        }

        public override FractalColorMode GetClone()
        {
           return new testcolormode();
        }
    }
}
