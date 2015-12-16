using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public static class FractalImageSaver
    {
        public static readonly string Filter = "Target Image File Format|*.Tiff|Joint Photographic Experts Group|*.Jpeg|Graphics Interchange Format|*.Gif";
        //public string GetFilter() { return Filter; }
        public static System.Drawing.Imaging.ImageFormat GetImageFormatFromText(string format)
        {
            if (format == "tiff") return System.Drawing.Imaging.ImageFormat.Tiff;
            if (format == "gif") return System.Drawing.Imaging.ImageFormat.Gif;
            return null;
        }
        public static string GetFormatStringFromName(string arg)
        {
            string[] arr = arg.Split('.');
            return arr[arr.Length - 1];
        }
        public static System.Drawing.Imaging.ImageFormat GetFormatFromIndex(int Index)
        {
            switch(Index)
            {
                case 0:
                    {
                        return System.Drawing.Imaging.ImageFormat.Tiff;
                   }
                case 1:{return System.Drawing.Imaging.ImageFormat.Jpeg;}
                case 2:{return System.Drawing.Imaging.ImageFormat.Gif;}
                
            }
            return System.Drawing.Imaging.ImageFormat.Tiff;
        }
        public static System.Drawing.Imaging.ImageFormat GetFormaFromName(string name)
        {
            return GetImageFormatFromText(GetFormatStringFromName(name));
        }
       
    }
}
