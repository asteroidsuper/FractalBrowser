using System;
using System.Drawing;

namespace FractalBrowser
{
    class CosColorMode:FractalColorMode
    {
        public CosColorMode()
        {

        }
        public override System.Drawing.Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP, object Extra = null)
        {
            int width = FAP.Width,height=FAP.Height;
            Bitmap bmp = new Bitmap(width,height);
            double[][] dm = (double[][])FAP.GetUniqueParameter();
            for(int x=0;x<width;x++)
            {
                for(int y=0;y<height;y++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(get_cos_color(dm[x][y]/4, 255, 1D), get_cos_color(dm[x][y]/4, 205, 1D), get_cos_color(dm[x][y]/4, 155, 1D)));
                }
            }
            return bmp;
        }
        private int get_cos_color(double ratio,int rgb,double scale)
        {
            double offset = Math.PI * (rgb / 255D);
            double result = Math.Cos(Math.PI * (rgb / 255D) + Math.PI + scale * ratio * 2D * Math.PI);
            return (int)(127.5 * (1D+(result)));
        }
        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            throw new NotImplementedException();
        }
    }
}
