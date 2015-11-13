using System;
using System.Numerics;
using System.Threading.Tasks;
namespace FractalBrowser
{
    public class Julia : _2DFractal
    {

        /*___________________________________________________________Конструкторы_класса______________________________________________________________*/
        #region Constructors of class
        public Julia(ulong IterCount, double LeftEdge, double RightEdge, double TopEdge, double BottomEdge, Complex ComplexConst)
        {
            f_iterations_count = IterCount;
            _2df_left_edge = LeftEdge;
            _2df_right_edge = RightEdge;
            _2df_top_edge = TopEdge;
            _2df_bottom_edge = BottomEdge;
            j_complex_const = ComplexConst;
            f_number_of_using_threads_for_parallel = Environment.ProcessorCount;
            f_allow_change_iterations_count();
            //f_new_percent_in_parallel += _j_new_percent_handler;
            //f_parallel_canceled += _j_unset_parallel_state;
        }
        private Julia()
        {
            f_allow_change_iterations_count();
        }
        #endregion /Constructors of class

        /*______________________________________________________________Данные_фрактала_______________________________________________________________*/
        #region Fractal data
        protected Complex j_complex_const;

        #endregion /Fractal data

        /*_______________________________________________________Реализация_абстрактных_методов_______________________________________________________*/
        #region Realization of abstract methods
        public override void CreateParallelFractal(int Width, int Height)
        {


            System.Threading.ThreadPool.QueueUserWorkItem((o) =>
            {
                f_begin_parallel_process();
                _2df_reset_scale(Width, Height);
                //_j_parallel_create_fractal_double_version(Width, Height).SendResult();
                _j_in_parallel_create_fractal_double_version(Width, Height).SendResult();
                f_end_parallel_process();
            });

        }

        public override void CreateParallelFractal(int Width, int Height, int HorizontalStar, int VerticalStart, int SelectedWidth, int SelectedHeight, bool safe = false)
        {

            System.Threading.ThreadPool.QueueUserWorkItem((o) =>
            {
                f_begin_parallel_process();
                if (safe) _2df_safe_set_scale(Width, Height, HorizontalStar, VerticalStart, SelectedWidth, SelectedHeight);
                else _2df_set_scale(Width, Height, HorizontalStar, VerticalStart, SelectedWidth, SelectedHeight);
                _j_in_parallel_create_fractal_double_version(Width, Height).SendResult();
                f_end_parallel_process();
            });
        }

        public override FractalType GetFractalType()
        {
            return FractalType._2DStandartIterationType;
        }
        public override FractalAssociationParametrs CreateFractal(int Width, int Height)
        {
            _2df_reset_scale(Width,Height);
            return _j_in_parallel_create_fractal_double_version(Width,Height).GetResult();
        }
        //protected override Fractal.fractal_resume_data get_resume_data()
        //{
         //   return new julia_resume_data(_2df_imagine_left, _2df_imagine_top,j_complex_const);
        //}
        #endregion /Realization of abstract methods

        /*___________________________________________________________Частные_методы_класса____________________________________________________________*/
        #region Private methods for realization
        protected  virtual _2DFractalHelper _j_parallel_create_fractal_double_version(int width, int height)
        {
            ulong max_iterations = f_iterations_count, iterations;
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, width, height);
            AbcissOrdinateHandler aoh = fractal_helper.AOH;
            ulong[][] result_matrix = fractal_helper.CommonMatrix;
            int percent_length = fractal_helper.PercentLength, percent_counter = percent_length;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point;
            Complex complex_iterator = new Complex();
            for (; aoh.abciss < width; ++aoh.abciss)
            {
                abciss_point = abciss_points[aoh.abciss];
                for (; aoh.ordinate < height; ++aoh.ordinate)
                {
                    complex_iterator.Real = abciss_point;
                    complex_iterator.Imagine = ordinate_points[aoh.ordinate];
                    for (iterations = 0; (complex_iterator.Real * complex_iterator.Real + complex_iterator.Imagine * complex_iterator.Imagine) < 4D && iterations < max_iterations; ++iterations)
                    {
                        complex_iterator.tsqr();
                        complex_iterator.Real += j_complex_const.Real;
                        complex_iterator.Imagine += j_complex_const.Imagine;
                    }
                    result_matrix[aoh.abciss][aoh.ordinate] = iterations;
                }
                aoh.ordinate = 0;
                if ((--percent_counter) == 0)
                {
                    percent_counter = percent_length;
                    f_new_percent_in_parallel_activate();
                }
            }
            fractal_helper.GiveUnique(j_complex_const);
            return fractal_helper;
        }
        protected virtual void _j_create_part_of_fractal(AbcissOrdinateHandler p_aoh,_2DFractalHelper fractal_helper)
        {
            ulong max_iterations = f_iterations_count,iterations;
            ulong[][] result_matrix = fractal_helper.CommonMatrix;
            int percent_length = fractal_helper.PercentLength, percent_counter = percent_length;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point,dist,pdist=0D;
            double[][] dmatrix = (double[][])fractal_helper.GetUnique();
            Complex complex_iterator = new Complex();
            for(;p_aoh.abciss<p_aoh.end_of_abciss;++p_aoh.abciss)
            {
                abciss_point = abciss_points[p_aoh.abciss];
                for (; p_aoh.ordinate < p_aoh.end_of_ordinate; ++p_aoh.ordinate)
                {
                    complex_iterator.Real = abciss_point;
                    complex_iterator.Imagine = ordinate_points[p_aoh.ordinate];
                    dist = 0D;
                    for (iterations = 0; dist < 4D && iterations < max_iterations; ++iterations)
                    {
                        pdist = dist;
                        complex_iterator.tsqr();
                        complex_iterator.Real += j_complex_const.Real;
                        complex_iterator.Imagine += j_complex_const.Imagine;
                        dist = (complex_iterator.Real * complex_iterator.Real + complex_iterator.Imagine * complex_iterator.Imagine);
                    }
                    result_matrix[p_aoh.abciss][p_aoh.ordinate] = iterations;
                    dmatrix[p_aoh.abciss][p_aoh.ordinate] = pdist;
                }
                p_aoh.ordinate = 0;
                if ((--percent_counter) == 0)
                {
                    percent_counter = percent_length;
                    f_new_percent_in_parallel_activate();
                }
            }
        }
        protected virtual _2DFractalHelper _j_in_parallel_create_fractal_double_version(int width,int height)
        {

            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, width, height);
            AbcissOrdinateHandler[] p_aoh = fractal_helper.CreateDataForParallelWork(f_number_of_using_threads_for_parallel);
            Task[] ts = new Task[p_aoh.Length];
            Action<object> act = (abc) => { _j_create_part_of_fractal((AbcissOrdinateHandler)abc,fractal_helper); };
            double[][] dmatrix =new double[width][];
            for (int i = 0; i < width; i++) dmatrix[i] = new double[height];
            fractal_helper.GiveUnique(dmatrix);
                for (int i = 0; i < ts.Length; i++)
                {
                    ts[i] = new Task(act, p_aoh[i]);
                    ts[i].Start();
                }
            for(int i=0;i<ts.Length;i++)
            {
                ts[i].Wait();
                ts[i].Dispose();
            }
            
            return fractal_helper;
        }


        #endregion /Private methods for realization

        /*__________________________________________________________Частные_утилиты_класса____________________________________________________________*/
        #region Private utilities of class
        
        #endregion /Private utilities of class

        /*________________________________________________Перегруженные_методы________________________________________________*/
        #region Overrided virtual methods
        protected override object GetResumeData()
        {
            return new object[] { _2df_left_edge, _2df_right_edge, _2df_top_edge, _2df_bottom_edge,j_complex_const };
        }
        public override Fractal GetClone()
        {
            Julia clone = new Julia();
            Julia.CopyTo(this, clone);
            return clone;
        }
        public override string ToString()
        {
            return "Fractal julia with const " + j_complex_const.Real + " + " + j_complex_const.Imagine + "i";
        }
        #endregion /Overrided virtual methods

        /*_____________________________________________________Общедоступные_статические_методы_______________________________________________________*/
        #region Public static methods
        public static void CopyTo(Julia Source,Julia Destinator)
        {
            _2DFractal.CopyTo(Source, Destinator);
            Destinator.j_complex_const = Source.j_complex_const;
        }
        #endregion /Public static methods

        
    }
}