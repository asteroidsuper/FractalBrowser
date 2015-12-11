using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    [Serializable]
    public class IncisionOf3DJulia:_2DFractal,IUsingComplex
    {
        /*_________________________________________________________Конструкторы_класса_____________________________________________________________*/
        #region Constructors of class
        public IncisionOf3DJulia(Quaternion Rotater=null,ulong IterationsCount = 40UL, double LeftEdge = -1.5D, double RightEdge = 1.5D, double TopEdge = -1.1D, double BottomEdge = 1.1D,Triplex Const=null)
        {
            f_iterations_count = IterationsCount;
            _2df_left_edge = LeftEdge;
            _2df_right_edge = RightEdge;
            _2df_top_edge = TopEdge;
            _2df_bottom_edge = BottomEdge;
            f_number_of_using_threads_for_parallel = Environment.ProcessorCount;
            if (Const == null) inc_const = new Triplex();
            else inc_const = (Triplex)Const.Clone();
            if (Rotater == null) inc_rotater = Quaternion.Null;
            else if (Rotater.Radian == 0D) inc_rotater = Quaternion.Null;
            else if (Rotater is Quaternion.QuaternionNull) inc_rotater = Quaternion.Null;
            else inc_rotater = (Quaternion)Rotater.Clone();
            f_allow_change_iterations_count();
        }
        protected IncisionOf3DJulia()
        {
            f_allow_change_iterations_count();
        }
        #endregion /Constructors of class

        /*____________________________________________________________Данные_класса________________________________________________________________*/
        #region Data of class
        protected Quaternion inc_rotater;
        protected Triplex inc_const;
        #endregion /Data of class

        /*___________________________________________________Реализация_абстрактных_методов________________________________________________________*/
        #region Realization of abstract methods
        public override FractalAssociationParametrs CreateFractal(int Width, int Height)
        {
            _2df_reset_scale(Width, Height);
            return inc_create_fractal(Width, Height).GetResult();
        }

        public override void CreateParallelFractal(int Width, int Height)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((o_o) =>
            {
                f_begin_parallel_process();
                _2df_reset_scale(Width, Height);
                inc_create_fractal(Width, Height).SendResult();
                f_end_parallel_process();
            });
        }

        public override void CreateParallelFractal(int Width, int Height, int HorizontalStar, int VerticalStart, int SelectedWidth, int SelectedHeight, bool safe = false)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((o_o) =>
            {
                f_begin_parallel_process();
                if (safe) _2df_safe_set_scale(Width, Height, HorizontalStar, VerticalStart, SelectedWidth, SelectedHeight);
                else _2df_set_scale(Width, Height, HorizontalStar, VerticalStart, SelectedWidth, SelectedHeight);
                inc_create_fractal(Width, Height).SendResult();
                f_end_parallel_process();
            });
        }

        public override FractalType GetFractalType()
        {
            return FractalType._2DStandartIterationType;
        }

        public override Fractal GetClone()
        {
            IncisionOf3DJulia clone = new IncisionOf3DJulia();
            CopyTo(this, clone);
            return clone;
        }
        #endregion /realization of abstract methods

        /*__________________________________________________Защищённые_методы_для_реализации_______________________________________________________*/
        #region Protected methods for realizations of class
        protected virtual _2DFractalHelper inc_create_fractal(int width,int height)
        {
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, width, height);
            Action<object> act = (abc) => { inc_create_part_of_fractal((AbcissOrdinateHandler)abc, fractal_helper); };
            AbcissOrdinateHandler[] p_aoh = fractal_helper.CreateDataForParallelWork(f_number_of_using_threads_for_parallel);
            Task[] ts = new Task[f_number_of_using_threads_for_parallel];
            fractal_helper.GiveUnique(new RadianMatrix(width));
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i] = new Task(act, p_aoh[i]);
                ts[i].Start();
            }
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i].Wait();
            }
            return fractal_helper;
        }
        protected virtual void inc_create_part_of_fractal(AbcissOrdinateHandler p_aoh,_2DFractalHelper fractal_helper)
        {
            ulong iter_count = f_iterations_count, iteration;
            ulong[][] matrix = fractal_helper.CommonMatrix;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point, dist, pdist = 0D;
            double[][] Ratio_matrix = (double[][])fractal_helper.GetRatioMatrix();
            int percent_length = fractal_helper.PercentLength, current_percent = percent_length;
            double[][] Radian_matrix = ((RadianMatrix)fractal_helper.GetUnique(typeof(RadianMatrix))).Matrix;
            int height = ordinate_points.Length;
            double cosrad = Math.Cos(inc_rotater.Radian),sinrad=Math.Sin(inc_rotater.Radian);
            Triplex z=new Triplex(), z0=(Triplex)inc_const.Clone(),last_valid_z=new Triplex();
            inc_rotater.Rotate(z0);
            for (; p_aoh.abciss < p_aoh.end_of_abciss; p_aoh.abciss++)
            {
                abciss_point = abciss_points[p_aoh.abciss];
                Radian_matrix[p_aoh.abciss] = new double[height];
                for (; p_aoh.ordinate < p_aoh.end_of_ordinate; ++p_aoh.ordinate)
                {
                    z.x = abciss_point;
                    z.y = ordinate_points[p_aoh.ordinate];
                    z.z =0;
                    inc_rotater.Rotate(z);
                    dist = 0D;
                    for (iteration = 0; iteration < iter_count && dist < 4D; iteration++)
                    {
                        pdist = dist;
                        last_valid_z.x = z.x;
                        last_valid_z.y = z.y;
                        last_valid_z.z = z.z;
                        z.tsqr();
                        z.tadd(z0);
                        dist = (z.x * z.x + z.y * z.y + z.z * z.z);
                    }
                    Ratio_matrix[p_aoh.abciss][p_aoh.ordinate] = pdist;
                    matrix[p_aoh.abciss][p_aoh.ordinate] = iteration;
                    Radian_matrix[p_aoh.abciss][p_aoh.ordinate] = Math.Atan2(last_valid_z.y, last_valid_z.x) * cosrad + sinrad * Math.Atan2(last_valid_z.z, last_valid_z.x);
                }
                    p_aoh.ordinate = 0;
                    if ((--current_percent) == 0)
                    {
                        current_percent = percent_length;
                        f_new_percent_in_parallel_activate();
                    }
                
            }
        }
        #endregion /Protected methods for realizations of class

        /*__________________________________________________Общедоступные_статические_методы_______________________________________________________*/
        #region Public static methods
        public static void CopyTo(IncisionOf3DJulia Source,IncisionOf3DJulia Destinator)
        {
            _2DFractal.CopyTo(Source, Destinator);
            Destinator.inc_rotater =Source.inc_rotater is Quaternion.QuaternionNull? new Quaternion.QuaternionNull():(Quaternion)Source.inc_rotater.Clone();
            Destinator.inc_const = (Triplex)Source.inc_const.Clone();
        }
        #endregion /public static methods



        void IUsingComplex.SetComplex(Complex Complex)
        {
            if (Complex == null)
            {
                throw new ArgumentNullException();
            }
            inc_const = Complex;
        }

        Complex IUsingComplex.GetComplex()
        {
            return new Complex(inc_const.x,inc_const.y);
        }
    }
}
