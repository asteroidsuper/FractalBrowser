using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public class JuliaWithClouds:Julia
    {

        /*____________________________________________________________Конструкторы_класса_____________________________________________________________________*/
        #region Constructors of class
        public JuliaWithClouds(ulong IterCount, double LeftEdge, double RightEdge, double TopEdge, double BottomEdge, Complex ComplexConst,int MaxAmmountAtTrace=100,int AbcissStepSize=20,int OrdinateStepSize=20):
        base(IterCount,LeftEdge,RightEdge,TopEdge,BottomEdge,ComplexConst)
        {
            _max_ammount_at_trace = MaxAmmountAtTrace;
            _abciss_step_length = AbcissStepSize;
            _ordinate_step_length = OrdinateStepSize;
        }
        private JuliaWithClouds():base(1,1,1,1,1,new Complex())
        {

        }
        #endregion /Constructors of class

        /*_______________________________________________________________Данные_класса________________________________________________________________________*/
        #region Data of class
        private int _max_ammount_at_trace;
        private int _abciss_step_length;
        private int _ordinate_step_length;
        #endregion /Data of class

        /*_________________________________________________________Перегруженные_методы_класса________________________________________________________________*/
        #region Overrided methods
        protected override _2DFractal._2DFractalHelper _j_in_parallel_create_fractal_double_version(int width, int height)
        {
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, width, height);
            AbcissOrdinateHandler[] p_aoh = fractal_helper.CreateDataForParallelWork(f_number_of_using_threads_for_parallel);
            Task[] ts = new Task[p_aoh.Length];
            Action<object> act = (abc) => { _j_create_part_of_fractal((AbcissOrdinateHandler)abc, fractal_helper); };
            fractal_helper.GiveUnique(new FractalCloudPoints(_max_ammount_at_trace,new FractalCloudPoint[width/_abciss_step_length+((width%_abciss_step_length)!=0? 1:0)][][]));
            fractal_helper.GiveUnique(new RadianMatrix(width));
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i] = new Task(act, p_aoh[i]);
                ts[i].Start();
            }
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i].Wait();
                ts[i].Dispose();
            }

            return fractal_helper;
        }
        protected override void _j_create_part_of_fractal(AbcissOrdinateHandler p_aoh, _2DFractalHelper fractal_helper)
        {
            ulong max_iterations = f_iterations_count, iterations;
            ulong[][] result_matrix = fractal_helper.CommonMatrix;
            int percent_length = fractal_helper.PercentLength, percent_counter = percent_length, height;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point, dist, pdist = 0D, sqr,abciss_interval_length=_2df_get_double_abciss_interval_length(),
                   ordinate_interval_length=_2df_get_double_ordinate_interval_length(),abciss_start=_2df_get_double_abciss_start(),
                   ordinate_start=_2df_get_double_ordinate_start();
            double[][] ratio_matrix = (double[][])fractal_helper.GetRatioMatrix();
            Complex complex_iterator = new Complex(), last_valid_complex = new Complex();
            double[][] radiad_matrix = ((RadianMatrix)fractal_helper.GetUnique(typeof(RadianMatrix))).Matrix;
            height = ordinate_points.Length;
            int fcp_height=ordinate_points.Length / _ordinate_step_length + (ordinate_points.Length % _ordinate_step_length != 0 ? 1 : 0);
            FractalCloudPoint[][][] fcp_matrix = ((FractalCloudPoints)fractal_helper.GetUnique(typeof(FractalCloudPoints))).fractalCloudPoint;
            List<FractalCloudPoint> fcp_list = new List<FractalCloudPoint>();
            FractalCloudPoint fcp;
            for (; p_aoh.abciss < p_aoh.end_of_abciss; ++p_aoh.abciss)
            {
                abciss_point = abciss_points[p_aoh.abciss];
                radiad_matrix[p_aoh.abciss] = new double[height];
                if (p_aoh.abciss % _abciss_step_length == 0) fcp_matrix[p_aoh.abciss / _abciss_step_length] = new FractalCloudPoint[fcp_height][];
                for (; p_aoh.ordinate < p_aoh.end_of_ordinate; ++p_aoh.ordinate)
                {
                    complex_iterator.Real = abciss_point;
                    complex_iterator.Imagine = ordinate_points[p_aoh.ordinate];
                    dist = 0D;
                    iterations = 0;
                    if (((p_aoh.abciss % _abciss_step_length) == 0) && ((p_aoh.ordinate % _ordinate_step_length) == 0))
                    {
                        fcp_list.Clear();
                        for (; dist < 4D && iterations <= (ulong)_max_ammount_at_trace; ++iterations)
                        {
                            pdist = dist;
                            last_valid_complex.Real = complex_iterator.Real;
                            last_valid_complex.Imagine = complex_iterator.Imagine;
                            sqr = complex_iterator.Real * 2;
                            complex_iterator.Real = complex_iterator.Real * complex_iterator.Real - complex_iterator.Imagine * complex_iterator.Imagine;
                            complex_iterator.Imagine *= sqr;
                            complex_iterator.Real += j_complex_const.Real;
                            complex_iterator.Imagine += j_complex_const.Imagine;
                            dist = (complex_iterator.Real * complex_iterator.Real + complex_iterator.Imagine * complex_iterator.Imagine);
                            fcp.AbcissLocation=(int)((complex_iterator.Real-abciss_start)/abciss_interval_length);
                            fcp.OrdinateLocation = (int)((complex_iterator.Imagine - ordinate_start) / ordinate_interval_length);
                            fcp_list.Add(fcp);
                        }
                        fcp_matrix[p_aoh.abciss / _abciss_step_length][p_aoh.ordinate / _ordinate_step_length] = fcp_list.ToArray();
                    }
                    for (; dist < 4D && iterations < max_iterations; ++iterations)
                    {
                        pdist = dist;
                        last_valid_complex.Real = complex_iterator.Real;
                        last_valid_complex.Imagine = complex_iterator.Imagine;
                        sqr = complex_iterator.Real * 2;
                        complex_iterator.Real = complex_iterator.Real * complex_iterator.Real - complex_iterator.Imagine * complex_iterator.Imagine;
                        complex_iterator.Imagine *= sqr;
                        complex_iterator.Real += j_complex_const.Real;
                        complex_iterator.Imagine += j_complex_const.Imagine;
                        dist = (complex_iterator.Real * complex_iterator.Real + complex_iterator.Imagine * complex_iterator.Imagine);
                    }
                    result_matrix[p_aoh.abciss][p_aoh.ordinate] = iterations;
                    ratio_matrix[p_aoh.abciss][p_aoh.ordinate] = pdist;
                    radiad_matrix[p_aoh.abciss][p_aoh.ordinate] = Math.Atan2(last_valid_complex.Imagine, last_valid_complex.Real);
                }
                p_aoh.ordinate = 0;
                if ((--percent_counter) == 0)
                {
                    percent_counter = percent_length;
                    f_new_percent_in_parallel_activate();
                }
            }
        }
        public override Fractal GetClone()
        {
            JuliaWithClouds clone = new JuliaWithClouds();
            JuliaWithClouds.CopyTo(this, clone);
            return clone;
        }
        public override FractalType GetFractalType()
        {
            return FractalType._2DStandartIterationTypeWithCloudPoints;
        }
        #endregion /Overrided methods

        /*__________________________________________________________Статические_методы_класса_________________________________________________________________*/
        #region Public static methods
        public static void CopyTo(JuliaWithClouds Source,JuliaWithClouds Destinator)
        {
            Julia.CopyTo(Source, Destinator);
            Destinator._max_ammount_at_trace = Source._max_ammount_at_trace;
            Destinator._abciss_step_length = Source._abciss_step_length;
            Destinator._ordinate_step_length = Source._ordinate_step_length;
        }
        #endregion Public Static methods
    }
}
