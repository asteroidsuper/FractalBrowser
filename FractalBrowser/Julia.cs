using System;
using System.Numerics;
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
            f_allow_change_iterations_count();
            //f_new_percent_in_parallel += _j_new_percent_handler;
            //f_parallel_canceled += _j_unset_parallel_state;
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
                _j_parallel_create_fractal_double_version(Width, Height).SendResult();
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
                _j_parallel_create_fractal_double_version(Width, Height).SendResult();
                f_end_parallel_process();
            });

        }

        public override FractalType GetFractalType()
        {
            return FractalType._2DStandartIterationType;
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
            for (aoh.abciss = 0; aoh.abciss < width; ++aoh.abciss)
            {
                abciss_point = abciss_points[aoh.abciss];
                for (aoh.ordinate = 0; aoh.ordinate < height; ++aoh.ordinate)
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
                if ((--percent_counter) == 0)
                {
                    percent_counter = percent_length;
                    f_new_percent_in_parallel_activate();
                }
            }
            aoh.Disconnect();
            fractal_helper.GiveUnique(j_complex_const);
            return fractal_helper;
        }




        #endregion /Private methods for realization

        /*__________________________________________________________Частные_утилиты_класса____________________________________________________________*/
        #region Private utilities of class
        
        #endregion /Private utilities of class

        /*________________________________________________Защищённые_перегрузки_виртуальных_методов________________________________________________*/
        #region Protected overriding virtual methods
        protected override object GetResumeData()
        {
            return new object[] { _2df_left_edge, _2df_right_edge, _2df_top_edge, _2df_bottom_edge,j_complex_const };
        }
        #endregion /Protected overriding virtual methods

        /*_____________________________________________________Общедоступные_статические_методы_______________________________________________________*/
        #region Public static methods
        
        #endregion /Public static methods
    }
}