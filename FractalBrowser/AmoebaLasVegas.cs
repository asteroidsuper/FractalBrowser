using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public class AmoebaLasVegas:_2DFractal
    {
        /*__________________________________________________________________Конструкторы_класса_______________________________________________________________*/
        #region Constructors
        private AmoebaLasVegas()
        {
            f_allow_change_iterations_count();
        }
        public AmoebaLasVegas(ulong IterationsCount = 25UL, double LeftEdge = -2.943249548891167D, double RightEdge = 5.426275000000004D, double TopEdge = -3.0069916973684219D, double BottomEdge = 3.0197062500000018D)
        {
            f_iterations_count = IterationsCount;
            _2df_left_edge = LeftEdge;
            _2df_right_edge = RightEdge;
            _2df_top_edge = TopEdge;
            _2df_bottom_edge = BottomEdge;
            f_number_of_using_threads_for_parallel = Environment.ProcessorCount;
            f_allow_change_iterations_count();
            _P = new Complex(0.318623D, 0.429799D);
        }
        #endregion /Constructors

        /*_____________________________________________________________________Данные_класса__________________________________________________________________*/
        #region Data of class
        private Complex _P;
        #endregion /Data of class

        /*_____________________________________________________________Реализация_абстрактных_методов_________________________________________________________*/
        #region Realization of absctract methods
        public override FractalAssociationParametrs CreateFractal(int Width, int Height)
        {
            _2df_reset_scale(Width,Height);
            return CreateFractalInParralels(Width, Height).GetResult();
        }

        public override void CreateParallelFractal(int Width, int Height)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((o_o) => {
            f_begin_parallel_process();
            _2df_reset_scale(Width, Height);
            CreateFractalInParralels(Width, Height).SendResult();
            f_end_parallel_process();
            });
            
        }

        public override void CreateParallelFractal(int Width, int Height, int HorizontalStar, int VerticalStart, int SelectedWidth, int SelectedHeight, bool safe = false)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((o_o) => {
            f_begin_parallel_process();
            if(safe)_2df_safe_set_scale(Width, Height,HorizontalStar,VerticalStart,SelectedWidth,SelectedHeight);
            else _2df_set_scale(Width, Height, HorizontalStar, VerticalStart, SelectedWidth, SelectedHeight);
                CreateFractalInParralels(Width, Height).SendResult();
            f_end_parallel_process();
            });
        }

        public override FractalType GetFractalType()
        {
            return FractalType._2DTrioDoubleUniqueMatrix;
        }

        public override Fractal GetClone()
        {
            AmoebaLasVegas clone = new AmoebaLasVegas();
            _2DFractal.CopyTo(this, clone);
            clone._P = this._P.getclone();
            return clone;
        }
        #endregion /Realization of absctract methods

        /*__________________________________________________________________Частные_методы_класса_____________________________________________________________*/
        #region Private methods
        private _2DFractalHelper CreateFractalInParralels(int Width,int Height)
        {
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, Width, Height);
            fractal_helper.GiveUnique(new double[Width][][]);
            Task[] tasks = new Task[f_number_of_using_threads_for_parallel];
            AbcissOrdinateHandler[] aohs = fractal_helper.CreateDataForParallelWork(f_number_of_using_threads_for_parallel);
            Action<object> act = (aoh) => { Create_part_of_fractal(fractal_helper, (AbcissOrdinateHandler)aoh); };
            //fractal_helper.AOH.end_of_abciss = Width;
            //fractal_helper.AOH.end_of_ordinate = Height;
           // Create_part_of_fractal(fractal_helper, fractal_helper.AOH);
            for (int i = 0; i < tasks.Length;i++ )
            {
                tasks[i] = new Task(act, aohs[i]);
                tasks[i].Start();
            }
            for (int i = 0; i < tasks.Length;i++ )
            {
                tasks[i].Wait();
                tasks[i].Dispose();
            }
                return fractal_helper;
        }
        private void Create_part_of_fractal(_2DFractalHelper fractal_helper,AbcissOrdinateHandler p_aoh)
        {
            
            ulong iterations;
            int percent_length=fractal_helper.PercentLength, current_percent=percent_length,trio_height;
            ulong[][] iter_matrix = fractal_helper.CommonMatrix;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point,ratio,lratio;
            double[][] Ratio_matrix = fractal_helper.GetRatioMatrix();
            double[][][] trio_matrix = (double[][][])fractal_helper.GetUnique(typeof(double[][][]));
            double  v1, v2, v3, v1total, v2total, v3total, v1min, v1max, v2min, v2max, v3min, v3max;
            Complex value=new Complex(), z=new Complex();
            trio_height=Ratio_matrix[0].Length;
            for(;p_aoh.abciss<p_aoh.end_of_abciss;p_aoh.abciss++)
            {
                abciss_point = abciss_points[p_aoh.abciss];
                trio_matrix[p_aoh.abciss] = new double[trio_height][];
                for(;p_aoh.ordinate<p_aoh.end_of_ordinate;p_aoh.ordinate++)
                {
                    z.Real=abciss_point;
                    z.Imagine = ordinate_points[p_aoh.ordinate];
                    v1 = v2 = v3 = v1total = v1max = v1min = v2total = v2max = v2min = v3total = v3max = v3min = 0D;
                    ratio = (z.Real * z.Real + z.Imagine * z.Imagine);
                    lratio = 0;
                    for(iterations=0;iterations<f_iterations_count&&ratio<400D;iterations++)
                    {
                        lratio = ratio;
                        z = Complex.SSin(z) +_P;
                        value = Complex.Sec(z);
                        ratio = z.Real*z.Real+z.Imagine*z.Imagine;
                        if (double.IsNaN(value.Real) || double.IsNaN(value.Imagine)) {
                            v1 = v2 = v3 = 0;
                        }
                        else{v1 = value.abs;
                        v2 = value.sqr().abs;
                        v3 = value.Pow(3).abs;
                        v1total += v1;
                        v2total += v2;
                        v3total += v3;
                        if (v1 < v1min) v1min = v1;
                        else if (v1 > v1max) v1max = v1;
                        if (v2 < v2min) v2min = v2;
                        else if (v2 > v2max) v2max = v2;
                        if (v3 < v3min) v3min = v3;                   
                        else if (v3 > v3max) v3max = v3;}
                    }
                    //$r = (($v1total / $count) - $v1min) / ($v1max - $v1min);
                    Ratio_matrix[p_aoh.abciss][p_aoh.ordinate] = lratio;
                    if (iterations > 0)
                    {
                        trio_matrix[p_aoh.abciss][p_aoh.ordinate] = new double[] {((v1total/(double)iterations)-v1min)/(v1max-v1min),
                        ((v2total/(double)iterations)-v2min)/(v2max-v2min),
                        ((v3total/(double)iterations)-v3min)/(v3max-v3min)};
                    }
                    else trio_matrix[p_aoh.abciss][p_aoh.ordinate] = new double[3];
                    iter_matrix[p_aoh.abciss][p_aoh.ordinate] = iterations;
                }
                p_aoh.ordinate = 0;
                if((--current_percent)==0)
                {
                    current_percent = percent_length;
                    f_new_percent_in_parallel_activate();
                }

            }
        }
        #endregion /Private methods
    }
}
