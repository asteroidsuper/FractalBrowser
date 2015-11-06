using System;
using System.Drawing;

namespace FractalBrowser
{
    public class SimpleRandomClouds2DFractalColorMode:FractalColorMode
    {
        public override System.Drawing.Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP)
        {
            if (!FAP.Is2D) throw new ArgumentException("Этот цветовой режим предназначен для двухмерных фракталов!");
            if (FAP.FractalType != FractalType._2DStandartIterationTypeWithCloudPoints) throw new ArgumentException("Данный фрактал не имеет трёхмерную матрицу FractalCloudPoint!");
            int width=FAP.Width, height=FAP.Height;
            Bitmap Result = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(Result);
            g.FillRectangle(Brushes.Black, 0, 0, width, height);
            g.Dispose(); 
            FractalCloudPoint[][][] fcp_matrix = (FractalCloudPoint[][][])FAP.GetUniqueParameter();
            Color using_color;
            Random rand = new Random();
            for(int _x=0;_x<fcp_matrix.Length;_x++)
            {
                for(int _y=0;_y<fcp_matrix[0].Length;_y++)
                {
                    if (fcp_matrix[_x][_y].Length <99) continue;
                    using_color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                    for(int i=0;i<fcp_matrix[_x][_y].Length;i++)
                    {
                        if (fcp_matrix[_x][_y][i].AbcissLocation < 0 || fcp_matrix[_x][_y][i].OrdinateLocation < 0 || fcp_matrix[_x][_y][i].AbcissLocation >=width|| fcp_matrix[_x][_y][i].OrdinateLocation >=height) continue;
                        Result.SetPixel(fcp_matrix[_x][_y][i].AbcissLocation, fcp_matrix[_x][_y][i].OrdinateLocation, using_color);
                    }
                }
            }
            return Result;
        }
    }
}
