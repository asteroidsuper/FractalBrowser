/*
 Спасибо за идею http://fractalworld.xaoc.ru/Mandelbrot_clouds.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public class MandelbrotWithClouds:Mandelbrot
    {
        /*__________________________________________________Конструкторы_класса_____________________________________________________________________*/
        #region Constructors
        public MandelbrotWithClouds(ulong IterationsCount = 1000, double LeftEdge = -2.1D, double RightEdge = 1D, double TopEdge = -1.1D, double BottomEdge = 1.1D,double MaxSqrRadius=16D,int MaxAmmountAtTrace=100,int AbcissStepSize=20,int OrdinateStepSize=20)
        {
            f_iterations_count = IterationsCount;
            _2df_left_edge = LeftEdge;
            _2df_right_edge = RightEdge;
            _2df_top_edge = TopEdge;
            _2df_bottom_edge=BottomEdge;
            f_allow_change_iterations_count();
            _max_sqr_radius = MaxSqrRadius;
            _max_ammount_at_trace = MaxAmmountAtTrace;
            _abciss_step_length = AbcissStepSize;
            _ordinate_step_length = OrdinateStepSize;
            f_number_of_using_threads_for_parallel = Environment.ProcessorCount;

        }
        private MandelbrotWithClouds()
        {
            f_allow_change_iterations_count();
        }
        #endregion /Constructors

        /*_________________________________________________Частные_данные_класса____________________________________________________________________*/
        #region Private data of class
        private double _max_sqr_radius;
        private int _max_ammount_at_trace;
        private int _abciss_step_length;
        private int _ordinate_step_length;


        #endregion /private data of class

        /*_______________________________________________Перегруженные_методы_класса________________________________________________________________*/
        #region Override methods
        protected override _2DFractalHelper m_old_create_fractal_double_version(int width, int height)
        {
            ulong iter_count = f_iterations_count, iteration;
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, width, height);
            AbcissOrdinateHandler aoh = fractal_helper.AOH;
            ulong[][] matrix = fractal_helper.CommonMatrix;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point,abciss_interval_length=_2df_get_double_abciss_interval_length(),ordinate_interval_length=_2df_get_double_ordinate_interval_length(),
                abciss_start=_2df_get_double_abciss_start(),ordinate_start=_2df_get_double_ordinate_start();
            int percent_length = fractal_helper.PercentLength, current_percent = percent_length;
            Complex z = new Complex(), z0 = new Complex();
            int fcp_width = width / _abciss_step_length + (width % _abciss_step_length!=0?1:0), fcp_height = height / _ordinate_step_length+(height%_ordinate_step_length!=0?1:0);
            FractalCloudPoint[][][] fcp=new FractalCloudPoint[fcp_width][][];
            List<FractalCloudPoint> fcp_list = new List<FractalCloudPoint>();
            FractalCloudPoint cfcp;
            for (; aoh.abciss < width; aoh.abciss++)
            {
                abciss_point = abciss_points[aoh.abciss];
                if ((aoh.abciss % _abciss_step_length) == 0) fcp[aoh.abciss / _abciss_step_length] = new FractalCloudPoint[fcp_height][];
                for (aoh.ordinate = 0; aoh.ordinate < height; aoh.ordinate++)
                {
                    
                    z0.Real = abciss_point;
                    z0.Imagine = ordinate_points[aoh.ordinate];
                    z.Real = z0.Real;
                    z.Imagine = z0.Imagine;
                    iteration = 0;
                    if ((aoh.abciss % _abciss_step_length) == 0 && (aoh.ordinate % _ordinate_step_length) == 0) { 
                        fcp_list.Clear();
                        for (; iteration < (ulong)_max_ammount_at_trace && (z.Real * z.Real + z.Imagine * z.Imagine) < _max_sqr_radius; iteration++)
                    {
                        z.tsqr();
                        z.Real += z0.Real;
                        z.Imagine += z0.Imagine;
                        cfcp = new FractalCloudPoint();
                        cfcp.AbcissLocation=(int)((z.Real-abciss_start)/abciss_interval_length);
                        cfcp.OrdinateLocation=(int)((z.Imagine-ordinate_start)/ordinate_interval_length);
                        fcp_list.Add(cfcp);
                    }
                        fcp[aoh.abciss / _abciss_step_length][aoh.ordinate / _ordinate_step_length] = fcp_list.ToArray();
                    }

                    for (; iteration < iter_count && (z.Real * z.Real + z.Imagine * z.Imagine) < 4D; iteration++)
                    {
                        z.tsqr();
                        z.Real += z0.Real;
                        z.Imagine += z0.Imagine;
                    }
                    matrix[aoh.abciss][aoh.ordinate] = iteration;
                }
                    if ((--current_percent) == 0)
                    {
                        current_percent = percent_length;
                        f_new_percent_in_parallel_activate();
                    }
                
            }
            fractal_helper.GiveUnique(new FractalCloudPoints(_max_ammount_at_trace,fcp));
            return fractal_helper;
        }
        protected override _2DFractal._2DFractalHelper m_create_fractal_double_version(int width, int height)
        {
            _2DFractalHelper fractal_helper = new _2DFractalHelper(this, width, height);
            Action<object> act = (abc) => { _m_create_part_of_fractal((AbcissOrdinateHandler)abc, fractal_helper); };
            AbcissOrdinateHandler[] p_aoh = fractal_helper.CreateDataForParallelWork(f_number_of_using_threads_for_parallel);
            Task[] ts = new Task[f_number_of_using_threads_for_parallel];
            FractalCloudPoints fcps=new FractalCloudPoints(_max_ammount_at_trace,new FractalCloudPoint[width/_abciss_step_length+(width%_abciss_step_length!=0?1:0)][][]);
            fractal_helper.GiveUnique(fcps);
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
            fcps.Clear();
            return fractal_helper;
        }
        protected override void _m_create_part_of_fractal(_2DFractal.AbcissOrdinateHandler p_aoh, _2DFractal._2DFractalHelper fractal_helper)
        {
            ulong max_iter=f_iterations_count, iterations;
            ulong[][] iter_matrix=fractal_helper.CommonMatrix;
            double[][] Ratio_matrix = fractal_helper.GetRatioMatrix(),radian_matrix=((RadianMatrix)fractal_helper.GetUnique(typeof(RadianMatrix))).Matrix;
            double[] abciss_points = fractal_helper.AbcissRealValues, ordinate_points = fractal_helper.OrdinateRealValues;
            double abciss_point, dist, pdist=0D,abciss_start=_2df_get_double_abciss_start(),abciss_interval_length=_2df_get_double_abciss_interval_length(),
            ordinate_start=_2df_get_double_ordinate_start(),ordinate_interval_length=_2df_get_double_ordinate_interval_length();
            FractalCloudPoints fcps = (FractalCloudPoints)fractal_helper.GetUnique();
            FractalCloudPoint[][][] fcp_matrix = fcps.fractalCloudPoint;
            int percent_length = fractal_helper.PercentLength, current_percent_state = percent_length;
            List<FractalCloudPoint> fcp_list=new List<FractalCloudPoint>();
            FractalCloudPoint fcp;
            int fcp_height = ordinate_points.Length / _ordinate_step_length + (ordinate_points.Length % _ordinate_step_length != 0 ? 1 : 0);
            Complex z = new Complex(), z0 = new Complex(),last_valid_z=new Complex();
            for (; p_aoh.abciss < p_aoh.end_of_abciss;p_aoh.abciss++ )
            {
                abciss_point = abciss_points[p_aoh.abciss];
                radian_matrix[p_aoh.abciss] = new double[ordinate_points.Length];
                if (p_aoh.abciss % _abciss_step_length == 0) fcp_matrix[p_aoh.abciss / _abciss_step_length] = new FractalCloudPoint[fcp_height][];
                for(;p_aoh.ordinate<p_aoh.end_of_ordinate;p_aoh.ordinate++)
                {
                    iterations = 0;
                    dist = 0D;
                    
                    z0.Real = abciss_point;
                    z0.Imagine = ordinate_points[p_aoh.ordinate];
                    z.Real = z0.Real;
                    z.Imagine = z0.Imagine;
                    if(((p_aoh.abciss%_abciss_step_length)==0)&&((p_aoh.ordinate%_ordinate_step_length)==0))
                    {
                        fcp_list.Clear();
                        for(;iterations<(ulong)_max_ammount_at_trace&&dist<=4D;iterations++)
                        {
                            pdist = dist;
                            last_valid_z.Real = z.Real;
                            last_valid_z.Imagine = z.Imagine;
                            z.tsqr();
                            z.Real += z0.Real;
                            z.Imagine += z0.Imagine;
                            dist = (z.Real * z.Real + z.Imagine * z.Imagine);
                            fcp.AbcissLocation = (int)((z.Real - abciss_start) / abciss_interval_length);
                            fcp.OrdinateLocation = (int)((z.Imagine - ordinate_start) / ordinate_interval_length);
                            fcp_list.Add(fcp);
                        }
                        fcp_matrix[p_aoh.abciss/_abciss_step_length][p_aoh.ordinate/_ordinate_step_length] = fcp_list.ToArray();
                    }
                    for (; iterations < max_iter && dist <= 4D; iterations++)
                    {
                        pdist = dist;
                        last_valid_z.Real = z.Real;
                        last_valid_z.Imagine = z.Imagine;
                        z.tsqr();
                        z.Real += z0.Real;
                        z.Imagine += z0.Imagine;
                        dist = (z.Real * z.Real + z.Imagine * z.Imagine);
                    }
                    Ratio_matrix[p_aoh.abciss][p_aoh.ordinate] = pdist;
                    iter_matrix[p_aoh.abciss][p_aoh.ordinate] = iterations;
                    radian_matrix[p_aoh.abciss][p_aoh.ordinate] = Math.Atan2(last_valid_z.Imagine,last_valid_z.Real);
                }
                p_aoh.ordinate = 0;
                if((--current_percent_state)==0)
                {
                    current_percent_state = percent_length;
                    f_new_percent_in_parallel_activate();
                }
            }
        }
        public override FractalType GetFractalType()
        {
            return FractalType._2DStandartIterationTypeWithCloudPoints;
        }
        public override Fractal GetClone()
        {
            MandelbrotWithClouds Clone = new MandelbrotWithClouds();
            MandelbrotWithClouds.CopyTo(this,Clone);
            return Clone;
        }
        #endregion /override methods

        /*_____________________________________________Общедоступные_статические_методы_____________________________________________________________*/
        #region Public static methods
        public static void CopyTo(MandelbrotWithClouds Source,MandelbrotWithClouds Destinator)
        {
            Mandelbrot.CopyTo(Source, Destinator);
            Destinator._max_ammount_at_trace = Source._max_ammount_at_trace;
            Destinator._max_sqr_radius = Source._max_sqr_radius;
            Destinator._abciss_step_length = Source._abciss_step_length;
            Destinator._ordinate_step_length = Source._ordinate_step_length;
        }

        #endregion /Public static methods
    }
}
