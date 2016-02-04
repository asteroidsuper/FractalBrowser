using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalBrowser
{
    public class FractalIterationsGradientColorMode : FractalColorMode
    {
        /*__________________________________________________________________Конструкторы__________________________________________________________________*/
        #region Constructors
        public FractalIterationsGradientColorMode()
        {
            thread_count = Environment.ProcessorCount;
            int_colors = new int[] { Color.White.ToArgb(), Color.Red.ToArgb(), Color.Blue.ToArgb(), Color.Black.ToArgb() }.Select(arg=>
            ((255 - ((arg & (255 << 24)) >> 24)) << 24)|
            ((255 - ((arg & (255 << 16)) >> 16)) << 16)|
            ((255 - ((arg & (255 << 8)) >>  8)) <<  8)|
            (255 - (arg & (255)))).ToArray();
            color_positions = new double[] { 0D, 0.02, 0.9, 1D };
            argb_colors = int_colors.Select(arg => new int[] {(arg&(255<<24))>>24,(arg&(255<<16))>>16,(arg&(255<<8))>>8,arg&(255) }).ToArray();
            _fcm_data_changed += Processor;
        }
        public FractalIterationsGradientColorMode(FractalIterationsGradientColorMode original)
        {
            thread_count = original.thread_count;
            int_colors = (int[])original.int_colors.Clone();
            color_positions = (double[])original.color_positions.Clone();
            argb_colors = (int[][])original.argb_colors.Clone();
            _fcm_data_changed += Processor;
        }
        #endregion /Constructors

        /*_____________________________________________________________Частные_атрибуты_класса____________________________________________________________*/
        #region Private atribytes
        private double[] color_positions;
        private int[] int_colors;
        private int[][] argb_colors;
        private int thread_count;
        #endregion /Private atribytes

        /*_____________________________________________________________Частные_утилиты_класса_____________________________________________________________*/
        #region Private utilities
        unsafe private int get_int_grad_color(long iter,long[] ulong_pos)
        {
            int index = 0;
            while (iter > ulong_pos[index]) ++index;
            long dist = ulong_pos[index] - ulong_pos[index-1],way=iter-ulong_pos[index-1];
            int[] lc = argb_colors[index-1], rc = argb_colors[index],resc=new int[4];
            resc[0] = lc[0] + (int)(((rc[0] - lc[0]) * way) / dist);
            resc[1] = lc[1] + (int)(((rc[1] - lc[1]) * way) / dist);
            resc[2] = lc[2] + (int)(((rc[2] - lc[2]) * way) / dist);
            resc[3] = lc[3] + (int)(((rc[3] - lc[3]) * way) / dist);

            return (resc[0]<<24)|(resc[1]<<16)|(resc[2]<<8)|(resc[3]);
        }
        private void Processor(object value,int ui,Control sender)
        {
            switch(ui)
            {
                case 0:
                    {
                        ColorGradientEventArgs e = value as ColorGradientEventArgs;
                        int_colors = e.Argbs;
                        color_positions = e.Positions;
                        argb_colors = int_colors.Select(arg => new int[] { (arg & (255 << 24)) >> 24, (arg & (255 << 16)) >> 16, (arg & (255 << 8)) >> 8, arg & (255) }).ToArray();
                        break;
                    }
            }
            _fcm_on_FractalColorModeChangedHandler();
        }
        #endregion /Private utilities

        /*_________________________________________________________Реализация_абстрактных_методов_________________________________________________________*/
        #region Realization of abstract methods
        public override FractalColorMode GetClone()
        {
            return new FractalIterationsGradientColorMode(this);
        }

        public override Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP, object Extra = null)
        {

            int width = FAP.Width, height = FAP.Height;
            ulong[][] matrix = FAP.Get2DOriginalIterationsMatrix();
            ulong umax = FAP.ItersCount;
            long[] color_ulong_pos = color_positions.Select(arg =>(long)(arg*umax)).ToArray();
            Bitmap Result = new Bitmap(width,height,PixelFormat.Format32bppRgb);
            BitmapData ResultData = Result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                int* scanpointer = (int*)ResultData.Scan0.ToPointer();
                    Action<int, int,int> ParallelDelegate = new Action<int,int, int>((th,shift, count) =>
                    {
                        int* ptr = scanpointer + (shift*width);
                        int x;
                        for(int i=0;i<count;++i,++shift)
                        {
                            for(x=0;x<width;++x)
                            {
                                *(ptr++) = get_int_grad_color((long)matrix[x][shift], color_ulong_pos);
                            }
                        }
                    });
                IAsyncResult[] waiters = new IAsyncResult[thread_count];
                int pr_count = height / thread_count, pr_shift = 0;
                for (int i=0;i<thread_count-1;++i)
                {
                   waiters[i]= ParallelDelegate.BeginInvoke(0,pr_shift,pr_count,null,null);
                    pr_shift += pr_count;
                }
                pr_count += height % thread_count;
                waiters[thread_count - 1] = ParallelDelegate.BeginInvoke(3,pr_shift, pr_count, null, null);
                for(int i=0;i<thread_count;++i)
                {
                    ParallelDelegate.EndInvoke(waiters[i]);
                }
            }
            Result.UnlockBits(ResultData);
            return Result;
        }

        public override Panel GetUniqueInterface(int width, int height)
        {
            Panel Result = new Panel();
            Result.Size = new Size(width, height);
            _add_standart_color_gradient_bar(Result, 0, int_colors, color_positions);
            return Result;
        }

        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            return FAP.Is2D;
        }
        #endregion /Realization of abstract methods
    }
}
