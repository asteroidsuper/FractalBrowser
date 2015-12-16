using System;
using System.Drawing;
using System.Windows.Forms;
namespace FractalBrowser
{
    [Serializable]
    class CosColorMode:FractalColorMode
    {
        public CosColorMode(int Red=255,double RedScale=1D,int Green=205,double GreenScale=1D,int Blue=155,double BlueScale=1D)
        {
            if (Red < 0 || Red > 255 || Green < 0 || Green > 255 || Blue < 0 || Blue > 255) throw new ArgumentException("Не правильные значения (значения должны находиться в диапозоне от 0 до 255)!");
            _red = Red;
            _green = Green;
            _blue = Blue;
            
            _red_scale = RedScale;
            _green_scale = GreenScale;
            _blue_scale = BlueScale;
        }
        public override System.Drawing.Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP, object Extra = null)
        {
            int width = FAP.Width,height=FAP.Height;
            Bitmap bmp = new Bitmap(width,height);
            double[][] dm = (double[][])FAP.Get2DRatioMatrix();
            for(int x=0;x<width;x++)
            {
                for(int y=0;y<height;y++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(get_cos_color(dm[x][y]/4, _red, _red_scale), get_cos_color(dm[x][y]/4, _green, _green_scale), get_cos_color(dm[x][y]/4, _blue, _blue_scale)));
                }
            }
            return bmp;
        }
        
        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            return FAP.Is2D && FAP.Get2DRatioMatrix() is double[][];
        }

        /*___________________________________________________________________Данные_класса_______________________________________________________________________*/
        #region Data of class
        private int _red;
        private int _green;
        private int _blue;
        private double _red_scale;
        private double _green_scale;
        private double _blue_scale;
        private Control[] controls;
        #endregion /Data of class

        public override System.Windows.Forms.Panel GetUniqueInterface(int width,int height)
        {
            _fcm_data_changed -= EventHandler;
            Panel Result = new Panel();
            Result.Size = new Size(width, height);
            controls=new Control[]{
            _add_standart_rgb_trackbar(Result, 0, 255, _red, Color.Red,1,1,3),
            _add_standart_rgb_trackbar(Result,1,10000,(int)(10000D *GetNormalize(_red_scale)),Color.FromArgb(255,80,80),5,1,3),
            _add_standart_rgb_trackbar(Result, 10, 255, _green, Color.Green,1,1,3),
            _add_standart_rgb_trackbar(Result, 11, 10000, (int)(10000D * GetNormalize(_green_scale)), Color.FromArgb(80, 255, 80),5, 1, 3),
            _add_standart_rgb_trackbar(Result, 20, 255, _blue, Color.Blue,1,1,3),
            _add_standart_rgb_trackbar(Result, 21, 10000, (int)(10000D * GetNormalize(_blue_scale)), Color.FromArgb(80, 80, 255),5, 1, 3),
            _add_standart_rgb_trackbar(Result,2,10000,(int)(1000 *(int)(GetDenormalize(_red_scale))),Color.FromArgb(255,128,128),5,1,3)};
            _fcm_data_changed += EventHandler;
            return Result;
        }
        
        private void EventHandler(object Value,int ui,Control sender)
        {
            switch(ui)
            {
                case 0: 
                    {
                        _red = (int)Value;
                        break;
                    }
                case 1:
                    {
                        TrackBar trackbar = (TrackBar)sender;
                        _red_scale = ((double)((int)Value))/trackbar.Maximum+GetDenormalize(_red_scale);
                        break;
                    }
                case 2:
                    {
                        _red_scale = GetNormalize(_red_scale) + (double)((int)Value);
                        break;
                    }
                case 10:
                    {
                        _green = (int)Value;
                        break;
                    }
                case 11:
                    {
                        TrackBar trackbar = (TrackBar)sender;
                        _green_scale = (double)((int)Value) / trackbar.Maximum;
                        break;
                    }
                case 20:
                    {
                        _blue = (int)Value;
                        break;
                    }
                case 21:
                    {
                        TrackBar trackbar = (TrackBar)sender;
                        _blue_scale = (double)((int)Value) / trackbar.Maximum;
                        break;
                    }
            }
            _fcm_on_FractalColorModeChangedHandler();
        }
        public override FractalColorMode GetClone()
        {
            return new CosColorMode(_red,_red_scale, _green,_green_scale, _blue,_blue_scale);
        }
    }
}
