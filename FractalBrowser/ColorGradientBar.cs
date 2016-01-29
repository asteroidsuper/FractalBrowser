using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalBrowser
{
    public partial class ColorGradientBar : UserControl
    {
        public ColorGradientBar()
        {
            InitializeComponent();
            m_left_color = Color.Black;
            m_right_color = Color.White;
            standart_of_gpolygon = new double[][] {
            new double[] {0D, 0D},
            new double[] {-gpol_width, gpol_height/2D},
            new double[] {-gpol_width, gpol_height},
            new double[] {gpol_width, gpol_height},
            new double[] {gpol_width, gpol_height/2D},
            new double[] {0D, 0D}
        };
            gpolygons = new List<gpolygon>();/*(new gpolygon[] {
                new gpolygon(this,Color.Black, 30000 / 300000d),
                new gpolygon(this,Color.Red, 1 / 3d),
                new gpolygon(this,Color.Blue, 2 / 3d)});*/
            g_left = 0;
            g_top = 0.15;
            g_width = 1;
            g_height = 1-g_top*2D;
            m_color_gradient_map = bitmap_liner;
            SizeChanged += size_changed_handler;
            KeyDown += (o, e) =>
            {
                if (e.KeyCode == Keys.Delete && selected_gpol != null)
                {
                    gpolygons.Remove(selected_gpol);
                    selected_gpol = null;
                    OnColorGradientChanged();
                    m_color_gradient_map = bitmap_liner;
                    Invalidate();
                }
            };
            MouseDown += (obj, e) => {
                gpolygon taked = gpolygons.OrderByDescending(arg => arg.cp.position).FirstOrDefault(arg => arg.IsMyPoint(e.Location));
                if (e.Clicks == 2)
                {
                    if (taked != null)
                    {
                        ColorDialog cd = new ColorDialog();
                        cd.Color = taked.cp.color;
                        if (cd.ShowDialog() == DialogResult.OK)
                        {
                            taked.cp.color = cd.Color;
                            taked.cp.argb = cd.Color.ToArgb();
                            m_color_gradient_map = bitmap_liner;
                            OnColorGradientChanged();
                        }
                    }
                    else { gpolygons.Add(new gpolygon(this, e.X)); OnColorGradientChanged(); }
                    Invalidate();
                }
                else if (taked != null) { taked.TakeAim(); }
                selected_gpol = taked;
            };
            DoubleBuffered = true;
        }

        /*__________________________________________________Общедоступные_методы__________________________________________________*/
        #region Public methods
        public Color GetColorFromX(int X)
        {
            double liner_pos = (X - g_left * Width) / (Width * g_width);
            if (liner_pos < 0) return left_col;
            if (liner_pos > 1) return right_col;
            gpolygon gpol_l = gpolygons.FindLast(arg => arg.cp.position <= liner_pos),
                gpol_r = gpolygons.Find(arg => arg.cp.position > liner_pos);
            Color left = left_col, right = right_col;
            double l_d = 0, r_d = 1;
            if (gpol_l != null) { left = gpol_l.cp.color; l_d = gpol_l.cp.position; }
            if (gpol_r != null) { right = gpol_r.cp.color; r_d = gpol_r.cp.position; }
            double value = (liner_pos - l_d) / (r_d - l_d);
            return Color.FromArgb(left.R+(int)((right.R-left.R)*value), left.G + (int)((right.G - left.G) * value), left.B + (int)((right.B - left.B) * value));
        }
        #endregion /Public methods

        /*_______________________________________________Общедоступные_поля_класса________________________________________________*/
        #region Public fields
        public double[] GradientPositions
        {
            get
            {
                return new double[]{0}.Concat(gpolygons.OrderBy(arg=>arg.cp.position).Select(arg => arg.cp.position)).Concat(new double[] {1}).ToArray();
            }
        }
        public Color[] GradientColors
        {
            get
            {
                return new Color[] { left_col}.Concat(gpolygons.OrderBy(arg => arg.cp.position).Select(arg => arg.cp.color)).Concat(new Color[] {right_col}).ToArray();
            }
        }
        #endregion /Public fields

        /*_____________________________________________________Частные_данные_____________________________________________________*/
        #region Private data
        private Bitmap m_color_gradient_map;
        private Color m_left_color;
        private Color m_right_color;
        private double g_left;
        private double g_top;
        private double g_width;
        private double g_height;
        private gpolygon selected_gpol;
        private List<gpolygon> gpolygons;
        private double gpol_width = 0.016D;
        private double gpol_height = 0.15D;
        private double[][] standart_of_gpolygon;
        private Color standart_of_gpolygon_color=Color.Gray;
        #endregion Private data

        /*____________________________________________________Частные_подклассы___________________________________________________*/
        #region Private subclasses
        private class colorpoint
        {
            public colorpoint() { }
            public colorpoint(Color color,double pos)
            {
                this.color = color;
                position = pos;
                argb = color.ToArgb();
            }
            public Color color;
            public double position;
            public int argb;
            public int l_int_pos;
            public void set_l_int_pos(int width) { l_int_pos = (int)(width*position); }
        }
        private class gpolygon
        {
            public gpolygon(ColorGradientBar owner,colorpoint _cp)
            {
                RePoints = owner.standart_of_gpolygon.Select(arg => (double[])arg.Clone()).ToArray();
                cp = _cp;
                LineColor = owner.standart_of_gpolygon_color;
                Owner = owner;
                act_move = (obj, e) => {
                    double res = (e.X - Owner.g_left * Owner.Width) / (Owner.g_width * Owner.Width);
                    if (res < 0) res = 0;
                    else if (res > 1) res = 1;
                    cp.position = res;
                    Owner.gpolygons.Sort(new Compar<gpolygon>((x,y)=>x.cp.position.CompareTo(y.cp.position)));
                    Owner.m_color_gradient_map = Owner.bitmap_liner;
                    Owner.Invalidate();
                };
                act_up = (obj, e) => {
                    ReturnAim();
                };
            }
            public gpolygon(ColorGradientBar owner,Color color,double pos):this(owner,new colorpoint(color, pos)) { }
            public gpolygon(ColorGradientBar owner,int X):this(owner,
                new colorpoint(owner.GetColorFromX(X), (X - owner.g_left * owner.Width) / (owner.Width * owner.g_width))){ }
            public double[][] RePoints;
            public colorpoint cp;
            public Color LineColor;
            public ColorGradientBar Owner;
            MouseEventHandler act_move;
            MouseEventHandler act_up;
        public IEnumerable<Point> IntPoints
            {
                get
                {
                    int int_x_pos=(int)(Owner.g_left*Owner.Width+Owner.g_width*cp.position*Owner.Width),
                        int_y_pos=(int)((Owner.g_top+Owner.g_height)*Owner.Height);
                   
                    return RePoints.Select(arg=>new Point(int_x_pos+(int)(arg[0]*Owner.Width),
                        int_y_pos+(int)(arg[1]*Owner.Height)));
                }
            }
            public bool IsMyPoint(Point point)
            {
                int int_x_pos = (int)(Owner.g_left * Owner.Width + Owner.g_width * cp.position * Owner.Width),
                    int_y_pos = (int)((Owner.g_top + Owner.g_height) * Owner.Height);
                return Math.Abs(point.X - int_x_pos) <= Width && int_y_pos <=point.Y;
            }
            public void TakeAim()
            {
                
                Owner.MouseMove += act_move;
                Owner.MouseUp += act_up;
            }
            public void ReturnAim()
            {
                Owner.MouseMove -= act_move;
                Owner.MouseUp -= act_up;
                Owner.OnColorGradientChanged();
            }
            public void Draw(Graphics g)
            {
                Point[] points = IntPoints.ToArray();
                g.FillPolygon(new SolidBrush(cp.color), points);
                g.DrawPolygon(new Pen(LineColor,1.5f), points);
            }
            public double Width
            {
                get
                {
                    return Owner.gpol_width * Owner.g_width * Owner.Width;
                }
            }
            public double Height
            {
                get
                {
                    return Owner.gpol_height * Owner.g_height * Owner.Height;
                }
            }
        }
        private class Equality<T> : IEqualityComparer<T>
        {
            public Equality(Func<T,T,bool> EqualFunc,Func<T,int> HashFunc)
            {
                equal_func = EqualFunc;
                hash_func = HashFunc;
            }
            private Func<T, T, bool> equal_func;
            private Func<T, int> hash_func;
            public bool Equals(T x, T y)
            {
                return equal_func(x, y);
            }

            public int GetHashCode(T obj)
            {
                return hash_func(obj);
            }
        }
        private class Compar<T>:IComparer<T>
        {
            public Compar(Func<T,T,int> funcer) { func = funcer; }
            private Func<T, T, int> func;
            public int Compare(T x, T y)
            {
                return func(x, y);
            }
        }
        #endregion /Pivate subclasses

        /*_________________________________________________Общедоступные_подклассы________________________________________________*/
        #region Public subclasses
        
        #endregion /Public subclasses

        /*__________________________________________________Частные_утилиты_класса________________________________________________*/
        #region Private utilities of class
        private Color right_col
        {
            get
            {
                gpolygon gpol = gpolygons.FindLast(arg => arg.cp.position == 1);
                if (gpol != null) return gpol.cp.color;
                return m_right_color;
            }
        }
        private Color left_col
        {
            get
            {
                gpolygon gpol = gpolygons.FindLast(arg => arg.cp.position == 0);
                if (gpol != null) return gpol.cp.color;
                return m_left_color;
            }
        }
        private IEnumerable<colorpoint> ColorPoints
        {
            get
            {
                return  new colorpoint[]{ new colorpoint(left_col, 0), new colorpoint(right_col, 1) }.Concat(gpolygons.Select(arg=>arg.cp));
            }
        }
        private Bitmap bitmap_liner
        {
            get
            {
                int height = (int)(Height * g_height),width= (int)(Width * g_width);
                Bitmap result = new Bitmap(width,height,PixelFormat.Format24bppRgb);
                BitmapData result_data = result.LockBits(new Rectangle(0, 0, width, height),ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);
                unsafe
                {
                    int* _color = stackalloc int[width];
                    int* last = stackalloc int[1];
                    *last = width+1;
                    IEnumerable<colorpoint> ordered=ColorPoints.OrderByDescending(arg => {
                        arg.set_l_int_pos(width);
                        return arg.position;
                    }).Distinct(new Equality<colorpoint>((x,y)=>x.l_int_pos==y.l_int_pos,
                    obj=>obj.l_int_pos)).Reverse();
                    fixed(int* inter_st=ordered.Select(arg => arg.l_int_pos).ToArray())
                    fixed(int* coler_st=ordered.Select((arg) => arg.argb).ToArray())
                    {
                        double dif;
                        int* inter_ptr = inter_st;
                        byte* coler_ptr =(byte*)coler_st;
                        byte* _color_ptr = (byte*)_color;
                        for(int i=0;i<width;++i)
                        {
                            if (i >= *inter_ptr) { ++inter_ptr; coler_ptr+=4; }
                            dif = (double)(i-*(inter_ptr-1))/(*inter_ptr - *(inter_ptr-1));
                            //if (dif == 0) { *((int*)(_color_ptr)) = *(int*)(coler_ptr);_color_ptr += 4;continue; }
                            *(_color_ptr++) = (byte)(*(coler_ptr-4) + (int)((*(coler_ptr) - *(coler_ptr-4)) * dif)); 
                            *(_color_ptr++) = (byte)(*(coler_ptr-3) + (int)((*(coler_ptr+1) - *(coler_ptr-3)) * dif)); 
                            *(_color_ptr++) = (byte)(*(coler_ptr-2) + (int)((*(coler_ptr+2) - *(coler_ptr-2)) * dif)); 
                            *(_color_ptr++) = (byte)(*(coler_ptr-1) + (int)((*(coler_ptr+3) - *(coler_ptr-1)) * dif));
                            //Save for future
                            /*dif = (double)(i-*(inter_ptr-1))/(*inter_ptr - *(inter_ptr-1));
                            *(_color_ptr++) = (byte)(*(coler_ptr) + (int)((*(coler_ptr) - *(coler_ptr-4)) * dif)); 
                            *(_color_ptr++) = (byte)(*(coler_ptr+1) + (int)((*(coler_ptr+1) - *(coler_ptr-3)) * dif)); 
                            *(_color_ptr++) = (byte)(*(coler_ptr+2) + (int)((*(coler_ptr+2) - *(coler_ptr-2)) * dif)); 
                            *(_color_ptr++) = (byte)(*(coler_ptr+3) + (int)((*(coler_ptr+3) - *(coler_ptr-1)) * dif));
                            */
                        }
                    }
                    int* ptr,rptr=(int*)result_data.Scan0.ToPointer();
                    for (int j = 0; j < height; j++)
                    {
                        ptr = _color;
                        for (int i = 0; i < width; ++i) *(rptr++) = *(ptr++);
                    }
                }
                result.UnlockBits(result_data);
                return result;
            }
        }
        private void size_changed_handler(object sender,EventArgs e)
        {
            m_color_gradient_map = bitmap_liner;
        }

        public void OnColorGradientChanged()
        {
            if (ColorGradientChanged != null) ColorGradientChanged(this, new ColorGradientEventArgs(GradientColors, GradientPositions));
        }
        #endregion /private utilities of class

        /*_________________________________________________Делегаты_и_эвенты_класса_______________________________________________*/
        #region Delegates and events
        public delegate void ColorGradientChangedHandler(object sender,ColorGradientEventArgs e);
        public event ColorGradientChangedHandler ColorGradientChanged;
        #endregion /Delegates and events
        private void ColorGradientBar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(m_color_gradient_map,new Point((int)(g_left * Width), (int)(g_top * Height)));
            foreach (gpolygon gpol in gpolygons) gpol.Draw(e.Graphics);
        }
    }
    public class ColorGradientEventArgs : EventArgs
    {
        public ColorGradientEventArgs(Color[] colors,double[] positions)
        {
            Colors = colors;
            Positions = positions;
            Argbs = Colors.Select(arg => arg.ToArgb()).ToArray();
        }
        public readonly Color[] Colors;
        public readonly double[] Positions;
        public readonly int[] Argbs;
    }
}
