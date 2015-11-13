using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public class Mandelbrot:_2DFractal
    {
        /*_______________________________________________________________Конструкторы_класса___________________________________________________________________*/
        #region Constructors
        public Mandelbrot(ulong IterationsCount=1000,double LeftEdge=-2.1D,double RightEdge=1D,double TopEdge=-1.1D,double BottomEdge=1.1D)
        {
            f_iterations_count = IterationsCount;
            _2df_left_edge = LeftEdge;
            _2df_right_edge = RightEdge;
            _2df_top_edge = TopEdge;
            _2df_bottom_edge=BottomEdge;
            f_number_of_using_threads_for_parallel = Environment.ProcessorCount;
            f_allow_change_iterations_count();
        }
        private Mandelbrot()
        {
            f_allow_change_iterations_count();
        }

        #endregion /Constructors

        /*___________________________________________________________Реализация_абстрактных_методов____________________________________________________________*/
        #region Realization abstract methods
        public override void CreateParallelFractal(int Width, int Height)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((p)=>{
            f_begin_parallel_process();
            _2df_reset_scale(Width, Height);
            m_create_fractal_double_version(Width, Height).SendResult();
            f_end_parallel_process();});
        }

        public override void CreateParallelFractal(int Width, int Height, int HorizontalStar, int VerticalStart, int SelectedWidth, int SelectedHeight, bool safe = false)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((p)=>{
            f_begin_parallel_process();
            if (safe) _2df_safe_set_scale(Width, Height, HorizontalStar, VerticalStart, SelectedWidth, SelectedHeight);
            else _2df_set_scale(Width, Height, HorizontalStar, VerticalStart, SelectedWidth, SelectedHeight);
            m_create_fractal_double_version(Width, Height).SendResult();
            f_end_parallel_process();
            });
        }
        public override FractalAssociationParametrs CreateFractal(int Width, int Height)
        {
            _2df_reset_scale(Width, Height);
            return m_create_fractal_double_version(Width, Height).GetResult();
        }
        public override FractalType GetFractalType()
        {
            return FractalType._2DStandartIterationType;
        }
        
        public override Fractal GetClone()
        {
            Mandelbrot Clone = new Mandelbrot();
            Mandelbrot.CopyTo(this, Clone);
            return Clone;
        }

        #endregion /Realizations absctract methods

        /*__________________________________________________________Защищённые_методы_для_реализации___________________________________________________________*/
        #region Protected methods for realization
        protected virtual _2DFractalHelper m_old_create_fractal_double_version(int width,int height)
        {
            ulong iter_count = f_iterations_count,iteration;
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, width, height);
            AbcissOrdinateHandler aoh = fractal_helper.AOH;
            ulong[][] matrix = fractal_helper.CommonMatrix;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point;
            int percent_length=fractal_helper.PercentLength,current_percent=percent_length;
            Complex z = new Complex(), z0 = new Complex();
            for (; aoh.abciss < width;aoh.abciss++)
            {
                abciss_point = abciss_points[aoh.abciss];
                for(aoh.ordinate=0;aoh.ordinate<height;aoh.ordinate++)
                {
                    z0.Real = abciss_point;
                    z0.Imagine = ordinate_points[aoh.ordinate];
                    z.Real = z0.Real;
                    z.Imagine = z0.Imagine;
                    for(iteration=0;iteration<iter_count&&(z.Real*z.Real+z.Imagine*z.Imagine)<4D;iteration++)
                    {
                        z.tsqr();
                        z.Real += z0.Real;
                        z.Imagine += z0.Imagine;
                    }
                    matrix[aoh.abciss][aoh.ordinate] = iteration;
                    if((--current_percent)==0)
                    {
                        current_percent = percent_length;
                        f_new_percent_in_parallel_activate();
                    }
                }
            }
            return fractal_helper;
        }
        protected virtual _2DFractalHelper m_create_fractal_double_version(int width,int height)
        {
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, width, height);
            Action<object> act=(abc)=>{_m_create_part_of_fractal((AbcissOrdinateHandler)abc,fractal_helper);};
            AbcissOrdinateHandler[] p_aoh = fractal_helper.CreateDataForParallelWork(f_number_of_using_threads_for_parallel);
            Task[] ts=new Task[f_number_of_using_threads_for_parallel];
            double[][] Ratio_matrix = new double[width][];
            for (int i = 0; i < width; i++) Ratio_matrix[i] = new double[height];
            fractal_helper.GiveUnique(Ratio_matrix);
                for (int i = 0; i < ts.Length; i++)
                {
                    ts[i] = new Task(act, p_aoh[i]);
                    ts[i].Start();
                }
            for (int i = 0; i < ts.Length;i++ )
            {
                ts[i].Wait();
            }
            return fractal_helper;
        }
        protected virtual void _m_create_part_of_fractal(AbcissOrdinateHandler p_aoh,_2DFractalHelper fractal_helper)
        {
            ulong iter_count = f_iterations_count, iteration;
            ulong[][] matrix = fractal_helper.CommonMatrix;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point,dist,pdist=0D;
            double[][] Ratio_matrix = (double[][])fractal_helper.GetUnique();
            int percent_length = fractal_helper.PercentLength, current_percent = percent_length;
            Complex z = new Complex(), z0 = new Complex();
            for (; p_aoh.abciss < p_aoh.end_of_abciss; p_aoh.abciss++)
            {
                abciss_point = abciss_points[p_aoh.abciss];
                for (; p_aoh.ordinate < p_aoh.end_of_ordinate; ++p_aoh.ordinate)
                {
                    z0.Real = abciss_point;
                    z0.Imagine = ordinate_points[p_aoh.ordinate];
                    z.Real = z0.Real;
                    z.Imagine = z0.Imagine;
                    dist = 0D;
                    for (iteration = 0; iteration < iter_count && dist < 4D; iteration++)
                    {
                        pdist = dist;
                        z.tsqr();
                        z.Real += z0.Real;
                        z.Imagine += z0.Imagine;
                        dist = (z.Real * z.Real + z.Imagine * z.Imagine);
                    }
                    Ratio_matrix[p_aoh.abciss][p_aoh.ordinate] = pdist;
                    matrix[p_aoh.abciss][p_aoh.ordinate] = iteration;
                    
                }
                p_aoh.ordinate = 0;
                if ((--current_percent) == 0)
                    {
                        current_percent = percent_length;
                        f_new_percent_in_parallel_activate();
                    }
            }
        }
        #endregion /Protected methods for realization

        /*______________________________________________________Общедоступные_статические_методы_класса________________________________________________________*/
        #region /Public static methods
        public static void CopyTo(Mandelbrot Source,Mandelbrot Destinator)
        {
            _2DFractal.CopyTo(Source, Destinator);
        }

        #endregion /Public static methods


    }
}
