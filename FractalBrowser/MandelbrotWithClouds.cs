﻿/*
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
            _abciss_step_size = AbcissStepSize;
            _ordinate_step_size = OrdinateStepSize;

        }
        #endregion /Constructors

        /*_________________________________________________Частные_данные_класса____________________________________________________________________*/
        #region Private data of class
        private double _max_sqr_radius;
        private int _max_ammount_at_trace;
        private int _abciss_step_size;
        private int _ordinate_step_size;


        #endregion /private data of class

        /*_______________________________________________Перегруженные_методы_класса________________________________________________________________*/
        #region Override methods
        protected override _2DFractalHelper m_create_fractal_double_version(int width, int height)
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
            int fcp_width = width / _abciss_step_size + (width % _abciss_step_size!=0?1:0), fcp_height = height / _ordinate_step_size+(height%_ordinate_step_size!=0?1:0);
            FractalCloudPoint[][][] fcp=new FractalCloudPoint[fcp_width][][];
            List<FractalCloudPoint> fcp_list = new List<FractalCloudPoint>();
            FractalCloudPoint cfcp;
            for (; aoh.abciss < width; aoh.abciss++)
            {
                abciss_point = abciss_points[aoh.abciss];
                if ((aoh.abciss % _abciss_step_size) == 0) fcp[aoh.abciss / _abciss_step_size] = new FractalCloudPoint[fcp_height][];
                for (aoh.ordinate = 0; aoh.ordinate < height; aoh.ordinate++)
                {
                    
                    z0.Real = abciss_point;
                    z0.Imagine = ordinate_points[aoh.ordinate];
                    z.Real = z0.Real;
                    z.Imagine = z0.Imagine;
                    iteration = 0;
                    if ((aoh.abciss % _abciss_step_size) == 0 && (aoh.ordinate % _ordinate_step_size) == 0) { 
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
                        fcp[aoh.abciss / _abciss_step_size][aoh.ordinate / _ordinate_step_size] = fcp_list.ToArray();
                    }

                    for (; iteration < iter_count && (z.Real * z.Real + z.Imagine * z.Imagine) < 4D; iteration++)
                    {
                        z.tsqr();
                        z.Real += z0.Real;
                        z.Imagine += z0.Imagine;
                    }
                    matrix[aoh.abciss][aoh.ordinate] = iteration;
                    if ((--current_percent) == 0)
                    {
                        current_percent = percent_length;
                        f_new_percent_in_parallel_activate();
                    }
                }
            }
            aoh.Disconnect();
            fractal_helper.GiveUnique(fcp);
            return fractal_helper;
        }
        public override FractalType GetFractalType()
        {
            return FractalType._2DStandartIterationTypeWithCloudPoints;
        }
        #endregion /override methods
    }
}