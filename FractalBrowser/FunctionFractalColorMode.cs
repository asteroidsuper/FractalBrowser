using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Drawing.Imaging;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace FractalBrowser
{
    [Serializable]
    public class FunctionFractalColorMode : FractalColorMode
    {
        /*__________________________________________________________________Конструкторы____________________________________________________________________*/
        #region Constructors
        public FunctionFractalColorMode()
        {
            //check_all_functions();
           _fcm_data_changed += processor;
        }
        private FunctionFractalColorMode(FunctionFractalColorMode original)
        {
            _m_red_func = original._m_red_func;
            _m_green_func = original._m_green_func;
            _m_blue_func = original._m_blue_func;
            _red_str = original._red_str;
            _green_str = original._green_str;
            _blue_str = original._blue_str;
            _fcm_data_changed += processor;
        }
        #endregion /Constructors

        /*__________________________________________________________Реализация_абстрактных_методов__________________________________________________________*/
        #region Realization of abstract methods
        public override FractalColorMode GetClone()
        {
            return new FunctionFractalColorMode(this);
        }

        public override Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP, object Extra = null)
        {
            check_all_functions();
            int width=FAP.Width, height=FAP.Height;
            Bitmap Result = new Bitmap(width, height);
            ulong[][] Iter_matrix = FAP.Get2DOriginalIterationsMatrix();
            //double maxiters=
            double[][] Ratio_matrix = FAP.Get2DRatioMatrix();
            double[][] Radian_Matrix = ((RadianMatrix)FAP.GetUniqueParameter(typeof(RadianMatrix))).Matrix;
            BitmapData ResultData=Result.LockBits(new Rectangle(new Point(),Result.Size),ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);
            unsafe
            {
                double[] arg = new double[8];
                int Parameter=-1;
                byte* blue = (byte*)&Parameter, green = blue + 1, red = green + 1;
                int* ptr = (int*)ResultData.Scan0.ToPointer();
                int x, y;
                double* iter, ratio, radian,maxofiter,xloc,yloc;
                fixed(double* iterator = arg)
                {
                    iter = iterator;
                    ratio = iter + 1;
                    radian = ratio + 1;
                    maxofiter = radian + 1;
                    xloc = maxofiter + 1;
                    yloc = xloc + 1;
                    arg[7] = height;
                    arg[6] = width;
                    *maxofiter = FAP.ItersCount;
                    for (y = 0; y < height; ++y)
                    {
                        for (x = 0; x < width; ++x)
                        {
                            *iter = Iter_matrix[x][y];
                            *ratio = Ratio_matrix[x][y];
                            *radian = Radian_Matrix[x][y];
                            *xloc = x;
                            *yloc = y;
                            *red = (byte)(_m_red_func(arg)%256D);
                            *green = (byte)(_m_green_func(arg) % 256D);
                            *blue = (byte)(_m_blue_func(arg) % 256D);
                            *(ptr++) = Parameter;
                        }
                    }
                }
            }
            Result.UnlockBits(ResultData);
            return Result;
        }

        public override Panel GetUniqueInterface(int width, int height)
        {
            Panel Result = new Panel();
            Result.Size = new Size(width, height);
            TextBox Box = new TextBox();
            Box.Size = new Size(width-25,Box.Height);
            red_text = Box;
            Box.Font = new Font(Box.Font.FontFamily, 12.25f);
            Result.Controls.Add(Box);
            Box = new TextBox();
            Box.Font = red_text.Font;
            Box.Size = red_text.Size;
            Box.Location = new Point(0,red_text.Location.Y+red_text.Height+15);
            Result.Controls.Add(Box);
            green_text = Box;
            Box = new TextBox();
            Box.Font = red_text.Font;
            Box.Size = red_text.Size;
            Box.Location = new Point(0, green_text.Location.Y + red_text.Height + 15);
            Result.Controls.Add(Box);
            blue_text = Box;
            Button set = new Button();
            set.Location = new Point(5, Box.Location.Y+Box.Height + 15);
            Result.Controls.Add(set);
            set.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            set.Font = blue_text.Font;
            set.Text = "Установить";
            set.Click += (s, e) => { _fcm_on_fcm_data_changed(null,0,set); };
            red_text.Text = _red_str;
            green_text.Text = _green_str;
            blue_text.Text = _blue_str;
            return Result;
        }

        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            return FAP.GetUniqueParameter(typeof(RadianMatrix)) != null && FAP.Get2DRatioMatrix() != null;
        }
        #endregion

        /*_____________________________________________________________Частные_данные_класса________________________________________________________________*/
        #region Private DATA
        [NonSerialized]
        private Func<double[], double> _m_red_func, _m_green_func, _m_blue_func;
        [NonSerialized]
        private TextBox red_text, green_text, blue_text;
        private string _red_str, _green_str, _blue_str;
        private Func<double[],double> _default_color_func(double arg,int index)
        {
            ParameterExpression _param = Expression.Parameter(typeof(double[]));
            ConstantExpression _index = Expression.Constant(index);
            BinaryExpression ie = Expression.ArrayIndex(_param, _index);
            BinaryExpression res = Expression.Divide(ie, Expression.Constant(arg));
            ConstantExpression max = Expression.Constant(255d);
            BinaryExpression test = Expression.LessThan(res, max);
            ConditionalExpression condi = Expression.Condition(test, res, max);
            return (Func<double[], double>)Expression.Lambda(condi, _param).Compile();
        }
        private string Replacer(string arg)
        {
            return arg.Replace("max","man").Replace("x","x4").Replace("man","max").Replace("count","x3").Replace("y","x5").Replace("iter", "x0").Replace("ratio", "x1").Replace("radian", "x2")
                .Replace("width","x6").Replace("height","x7");
        }
        private void processor(object value,int ui,Control sender)
        {
            if (ui == 0)
            {
                try
                {
                    if (red_text.Text.Length < 1) _m_red_func = _default_color_func(1.85, 0);
                    else {
                        Delegate del = RuntimeFunctionCompilator.CompileFunc(Replacer(red_text.Text));
                        _red_str = red_text.Text;
                        _m_red_func = (Func<double[], double>)del;
                    }
                    
                }
                catch
                {
                    MessageBox.Show("Неправильно введена функция для красного компонента!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                try
                {
                    if (green_text.Text.Length < 1) _m_green_func = _default_color_func(1.85, 0);
                    else {
                        Delegate del = RuntimeFunctionCompilator.CompileFunc(Replacer(green_text.Text));
                        _green_str = green_text.Text;
                        _m_green_func = (Func<double[], double>)del;
                    }
                }
                catch
                {
                    MessageBox.Show("Неправильно введена функция для зелёного компонента!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                try
                {
                    if (blue_text.Text.Length < 1) _m_blue_func = _default_color_func(1.85, 0);
                    else {
                        Delegate del = RuntimeFunctionCompilator.CompileFunc(Replacer(blue_text.Text));
                        _blue_str = blue_text.Text;
                        _m_blue_func = (Func<double[], double>)del;
                    }
                }
                catch
                {
                    MessageBox.Show("Неправильно введена функция для синего компонента!", "Ошибка!", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            _fcm_on_FractalColorModeChangedHandler();
        }
        #endregion /Private data

        /*______________________________________________________________Перегруженные_методы________________________________________________________________*/
        #region Overrided methods
        public override string ToString()
        {
            return "Function color mode";
        }
        #endregion /Overrided methods

        /*_____________________________________________________________Частные_утилиты_класса_______________________________________________________________*/
        #region Private utilities of class
        private void check_all_functions()
        {
            if(_m_red_func==null)
            {
                _m_red_func = string.IsNullOrEmpty(_red_str) ? 
                    _default_color_func(1.85, 0) : 
                    (Func<double[], double>)RuntimeFunctionCompilator.CompileFunc(Replacer(_red_str));
            }
            if(_m_green_func==null)
            {
                _m_green_func = string.IsNullOrEmpty(_green_str) ? _default_color_func(1.4, 0) : (Func<double[], double>)RuntimeFunctionCompilator.CompileFunc(Replacer(_green_str)); 
            }
            if(_m_blue_func==null)
            {
                _m_blue_func = string.IsNullOrEmpty(_blue_str) ? _default_color_func(1.8, 0) : (Func<double[], double>)RuntimeFunctionCompilator.CompileFunc(Replacer(_blue_str));
            }
        }
        #endregion /Private utlities of class
    }
}
