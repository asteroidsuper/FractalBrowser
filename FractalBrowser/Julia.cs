using System;

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
        #region Realization abstract methods
        public override void CreateParallelFractal(int Width, int Height)
        {


            System.Threading.ThreadPool.QueueUserWorkItem((o) =>
            {
                

            });

        }

        public override void CreateParallelFractal(int Width, int Height, int VerticalStart, int HorizontalStar, int SelectedWidth, int SelectedHeight, bool safe = false)
        {

            System.Threading.ThreadPool.QueueUserWorkItem((o) =>
            {



            });

        }

        public override FractalType GetFractalType()
        {
            return FractalType._2DStandartIterationType;
        }
        #endregion /Realization abstract methods

        /*___________________________________________________________Частные_методы_класса____________________________________________________________*/
        #region Private methods for realization
        private _2DFractalHelper _j_parallel_create_fractal_double_version(int width, int height)
        {
            ulong max_iterations = f_iterations_count, iterations;
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, width, height);
            ulong[][] result_matrix = fractal_helper.CommonMatrix;
            int percent_length = fractal_helper.PercentLength, percent_counter = percent_length, abciss, ordinate;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point;
            Complex complex_iterator = new Complex();
            for (abciss = 0; abciss < width; ++abciss)
            {
                abciss_point = abciss_points[abciss];
                for (ordinate = 0; ordinate < height; ++ordinate)
                {
                    complex_iterator.real = abciss_point;
                    complex_iterator.imagine = ordinate_points[ordinate];
                    for (iterations = 0; (complex_iterator.real * complex_iterator.real + complex_iterator.imagine * complex_iterator.imagine) < 4D && iterations < max_iterations; ++iterations)
                    {
                        complex_iterator.tsqr();
                        complex_iterator.real += j_complex_const.real;
                        complex_iterator.imagine += j_complex_const.imagine;
                    }
                    result_matrix[abciss][ordinate] = iterations;
                }
                if ((--percent_counter) == 0)
                {
                    percent_counter = percent_length;
                    f_new_percent_in_parallel_activate();
                    if (f_parallel_must_cancel) break;
                }
            }
            return fractal_helper;
        }




        #endregion /Private methods for realization

        /*__________________________________________________________Частные_утилиты_класса____________________________________________________________*/
        #region Private utilities of class
        private void _j_set_parallel_state()
        {
            f_parallel_isbusy = true;
            f_parallel_percent = 0;
            if (f_parallel_must_cancel)
            {
                f_new_percent_in_parallel += _j_new_percent_handler;
                f_parallel_must_cancel = false;
            }
        }
        private void _j_unset_parallel_state()
        {
            f_new_percent_in_parallel -= _j_new_percent_handler;
            f_parallel_must_cancel = true;
            f_parallel_isbusy = false;
        }
        private void _j_new_percent_handler()
        {
            f_activate_progresschanged(++f_parallel_percent);
        }
        #endregion /Private utilities of class
    }
}