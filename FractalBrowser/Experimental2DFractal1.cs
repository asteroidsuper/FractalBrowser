using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    [Serializable]
    public class Experimental2DFractal1:_2DFractal
    {
        /*__________________________________________________________________Конструкторы__________________________________________________________________________*/
        #region Constructors
        public Experimental2DFractal1(ulong itercount,double leftedge,double rightedge,double topedge,double bottomedge)
        {
            f_iterations_count = itercount;
            _2df_left_edge = leftedge;
            _2df_right_edge = rightedge;
            _2df_top_edge = topedge;
            _2df_bottom_edge = bottomedge;
            f_allow_change_iterations_count();
        }
        private Experimental2DFractal1()
        {
            f_allow_change_iterations_count();
        }
        #endregion /Constructors

        /*___________________________________________________________Реализация_абстрактных_методов_______________________________________________________________*/
        #region Realization absract methods
        public override void CreateParallelFractal(int Width, int Height)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((_o_o_) => {
                f_begin_parallel_process();
                _2df_reset_scale(Width, Height);
                _create_fractal_double_version(Width, Height).SendResult();
                f_end_parallel_process();
            });
        }

        public override void CreateParallelFractal(int Width, int Height, int VerticalStart, int HorizontalStar, int SelectedWidth, int SelectedHeight, bool safe = false)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((_o_o_) =>
            {
                f_begin_parallel_process();
                if (safe) _2df_safe_set_scale(Width, Height, VerticalStart, HorizontalStar, SelectedWidth, SelectedHeight);
                else _2df_set_scale(Width, Height, VerticalStart, HorizontalStar, SelectedWidth, SelectedHeight);
                _create_fractal_double_version(Width, Height).SendResult();
                f_end_parallel_process();
            });
        }
        public override FractalAssociationParametrs CreateFractal(int Width, int Height)
        {
            _2df_reset_scale(Width, Height);
            return _create_fractal_double_version(Width, Height).GetResult();
        }
        public override FractalType GetFractalType()
        {
            return FractalType._2DStandartIterationType;
        }
        public override Fractal GetClone()
        {
            Experimental2DFractal1 Clone = new Experimental2DFractal1();
            Experimental2DFractal1.CopyTo(this, Clone);
            return Clone;
        }
        #endregion /Realization absract methods

        /*________________________________________________________Частные_методы_для_реализации_класса____________________________________________________________*/
        #region Private methods for realizations
        protected virtual _2DFractalHelper _create_fractal_double_version(int Width,int Height)
        {
            ulong iterations_count = f_iterations_count,iteration;
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, Width, Height);
            ulong[][] matrix = fractal_helper.CommonMatrix;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point,p,_2d3d=2d/3d;
            int percent_length=fractal_helper.PercentLength, _current_percent=percent_length;
            AbcissOrdinateHandler aoh = fractal_helper.AOH;
            Complex z=new Complex(), t=new Complex(), d=new Complex();
            for(;aoh.abciss<Width;aoh.abciss++)
            {
                abciss_point = abciss_points[aoh.abciss];
                for(aoh.ordinate=0;aoh.ordinate<Height;aoh.ordinate++)
                {
                    z.Real = abciss_point;
                    z.Imagine = ordinate_points[aoh.ordinate];
                    d.Real = z.Real;
                    d.Imagine = z.Imagine;
                    for(iteration=0;iteration<=iterations_count&&(z.Real * z.Real + z.Imagine * z.Imagine <= 1000000)&&(d.Real*d.Real+d.Imagine*d.Imagine>0.000001);iteration++)
                    {
                        t.Real = z.Real;
                        t.Imagine = z.Imagine;
                        p=Math.Pow(t.Real * t.Real + t.Imagine + t.Imagine, 2);
                        z.Real = _2d3d * t.Real + (t.Real * t.Real - t.Imagine * t.Imagine) / (3 * p);
                        z.Imagine = _2d3d * t.Imagine * (1 - t.Real / p);
                        d.Real = Math.Abs(z.Real - t.Real);
                        d.Imagine = Math.Abs(z.Imagine - t.Imagine);
                    }
                    matrix[aoh.abciss][aoh.ordinate] = iteration;
                }
                if((--_current_percent)==0)
                {
                    _current_percent = percent_length;
                    f_new_percent_in_parallel_activate();
                }
            }
            return fractal_helper;
        }

        #endregion /Private methods for realizations

        /*______________________________________________________Общедоступные_статические_методы_класса___________________________________________________________*/
        #region Public static methods
        public static void CopyTo(Experimental2DFractal1 Source,Experimental2DFractal1 Destinator)
        {
            _2DFractal.CopyTo(Source, Destinator);
        }

        #endregion /Public static methods
    }
}
