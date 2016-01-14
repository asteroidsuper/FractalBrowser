using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace FractalBrowser
{
    public class FractalPictureBox : PictureBox
    {

        /*_____________________________________________________Конструкторы_класса___________________________________________________________*/
        #region Public constructors
        public FractalPictureBox()
        {
            _min_selection_size = 3;
            _set_2d_mode();
            //_set_click_mode();
        }

        #endregion /Public constructors

        /*_________________________________________________Общедоступные_атрибуты_класса_____________________________________________________*/
        #region Public atribytes
        /// <summary>
        /// Перо для отрисовка зоны выделения.
        /// </summary>
        public Pen SelectionPen = Pens.Black;
        #endregion /Public atribytes

        /*____________________________________________________Частные_утилиты_класса_________________________________________________________*/
        #region Private utilities
        private void _set_2d_mode()
        {
            if (_fpbm == FractalPictureBoxMode._2DViewer) return;
            _fpbm = FractalPictureBoxMode._2DViewer;
            Paint += (o, e) =>
            {
                _draw_selection_rect(e.Graphics, _first_mouse_point, _second_mouse_point);
            };
            MouseDown += _onmousedown_worker;
            MouseUp += _onmouseup_worker;
            MouseMove += (o, e) => { if (_is_mouse_into)_second_mouse_point = e.Location; Invalidate(); };
            MouseLeave += (o, e) => { _is_mouse_into = false; };
            MouseEnter += (o, e) => { _is_mouse_into = true; };
        }
        private void _unset_2d_mode()
        {
            if (_fpbm != FractalPictureBoxMode._2DViewer) return;
            _fpbm =FractalPictureBoxMode.none;
            Paint -= (o, e) =>
            {
                _draw_selection_rect(e.Graphics, _first_mouse_point, _second_mouse_point);
            };
            MouseDown -= _onmousedown_worker;
            MouseUp -= _onmouseup_worker;
            MouseMove -= (o, e) => { _second_mouse_point = e.Location; Invalidate(); };
            MouseLeave -= (o, e) => { _is_mouse_into = false; };
            MouseEnter -= (o, e) => { _is_mouse_into = true; };
        }
        private void _set_click_mode()
        {
            if (_fpbm == FractalPictureBoxMode.clickmode) return;
            _fpbm = FractalPictureBoxMode.clickmode;
            _click_pt = new Point();
            Paint += (o, e) =>
            {
                if (_click_pt.X <0 && _click_pt.Y <0) return;
            _draw_inverse_color_vertical_line(new Point(_click_pt.X,0),new Point(_click_pt.X,Height),e.Graphics);
            _draw_inverse_color_horizontal_line(new Point(0, _click_pt.Y), new Point(Width, _click_pt.Y),e.Graphics);
            };
            MouseMove += _mouse_move_clmode;
        }
        private void _unset_click_mode()
        {
            _click_pt = new Point(-1, -1);
            Paint -= (o, e) =>
            {
                _draw_inverse_color_vertical_line(new Point(_click_pt.X, 0), new Point(_click_pt.X, Height), e.Graphics);
                _draw_inverse_color_horizontal_line(new Point(0, _click_pt.Y), new Point(Width, _click_pt.Y), e.Graphics);
            };
            MouseMove -= _mouse_move_clmode;
        }
        private void _mouse_move_clmode(object sender,MouseEventArgs e)
        {
            _click_pt = e.Location;
            Invalidate();
        }
        private Bitmap GetInversed(Bitmap arg,Rectangle rect)
        {
            Bitmap Result =new Bitmap(rect.Width,rect.Height,PixelFormat.Format24bppRgb);
            int shiftmull = 4;
            BitmapData ResultData = Result.LockBits(new Rectangle(0, 0, rect.Width, rect.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            BitmapData ArgData = arg.LockBits(new Rectangle(0,0,arg.Width,arg.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
            int count = rect.Width * rect.Height;
            unsafe
            {
                byte* ArgPtr = (byte*)(ArgData.Scan0)+rect.X*shiftmull+rect.Y*shiftmull*arg.Width, ResultPtr = (byte*)ResultData.Scan0;
                for (int y = 0; y < rect.Height; y++)
                {
                    for (int x = 0; x < rect.Width; x++)
                    {
                        *(ResultPtr++) = (byte)(255 - *(ArgPtr++));
                        *(ResultPtr++) = (byte)(255 - *(ArgPtr++));
                        *(ResultPtr++) = (byte)(255 - *(ArgPtr++));
                        *(ResultPtr++) = 255;
                        ArgPtr++;
                    }
                    ArgPtr += arg.Width*shiftmull-rect.Width*shiftmull;
                }
            }
            arg.UnlockBits(ArgData);
            Result.UnlockBits(ResultData);
            return Result;
        }
        private Rectangle GetRectangle(Point one,Point two)
        {
            int lx, rx, ry, ly;
            if (one.X < two.X)
            {
                lx = one.X;
                rx = two.X;
            }
            else
            {
                rx = one.X;
                lx = two.X;
            }
            if (one.Y < two.Y)
            {
                ly = one.Y;
                ry = two.Y;
            }
            else
            {
                ry = one.Y;
                ly = two.Y;
            }
            return new Rectangle(lx, ly, rx - lx, ry - ly);
        }
        private void _draw_selection_rect(Graphics g, Point mouse_pos_a, Point mouse_pos_b)
        {
            if (!_is_pressed_mouse_left_button) return;
            Rectangle rect = GetRectangle(mouse_pos_a, mouse_pos_b);
            if (rect.Width < 3 || rect.Height < 3) return;
            Bitmap inversed_bitmap = GetInversed((Bitmap)Image, rect);
            g.DrawImage(inversed_bitmap, new Point(rect.X,rect.Y));
            inversed_bitmap.Dispose();
            /*if(SelectionPen!=null)g.DrawRectangle(SelectionPen, mouse_pos_a.X < mouse_pos_b.X ? mouse_pos_a.X : mouse_pos_b.X,
                mouse_pos_a.Y < mouse_pos_b.Y ? mouse_pos_a.Y : mouse_pos_b.Y, Math.Abs(mouse_pos_a.X - mouse_pos_b.X), Math.Abs(mouse_pos_a.Y - mouse_pos_b.Y));
            else _draw_inverse_color_rectangle(g,mouse_pos_a.X,mouse_pos_a.Y,mouse_pos_b.X,mouse_pos_b.Y); */
        }
        private void _onmousedown_worker(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _first_mouse_point = e.Location;
                _is_pressed_mouse_left_button = true;
            }
            else if ((e.Button == MouseButtons.Right) && !_is_pressed_mouse_left_button&&OpenMenuEvent!=null) OpenMenuEvent();
            else if (e.Button == MouseButtons.Right) _is_pressed_mouse_left_button = false;
        }
        private Rectangle _get_selected_rectangle(Point p1, Point p2)
        {
            return new Rectangle(p1.X < p2.X ? p1.X : p2.X, p1.Y < p2.Y ? p1.Y : p2.Y, Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }
        private void _onmouseup_worker(object sender, MouseEventArgs e)
        {
            if (_is_pressed_mouse_left_button & _fpbm == FractalPictureBoxMode._2DViewer)
            {
                if (RectangleSelected != null)
                {
                    Rectangle rec=_get_selected_rectangle(_first_mouse_point, e.Location);
                    if(rec.Width>=_min_selection_size&&rec.Height>=_min_selection_size)RectangleSelected(this,rec);
                }
            }
            _is_pressed_mouse_left_button = false;
        }
        private void _draw_inverse_color_horizontal_line(Point one,Point two,Graphics g)
        {
            if (one.X > two.X)
            {
                Point sw = one;
                one = two;
                two = sw;
            }
            Bitmap bmp = (Bitmap)Image;
            BitmapData bmpdata = bmp.LockBits(new Rectangle(new Point(), bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Bitmap res = new Bitmap(two.X - one.X, 2);
            BitmapData resdata=res.LockBits(new Rectangle(new Point(), res.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int count = two.X - one.X;
            unsafe
            {
                byte* bmpptr = ((byte*)bmpdata.Scan0)+one.Y*4*bmp.Width+one.X*4,resptr=(byte*)resdata.Scan0;
                byte* buffer = stackalloc byte[res.Width *4],sbuffer=buffer;
                for(int i=0;i<count;++i)
                {
                    *(resptr) = (byte)(255 - *(bmpptr++));
                    *(buffer++) = *(resptr++);
                    *(resptr) = (byte)(255 - *(bmpptr++));
                    *(buffer++) = *(resptr++);
                    *(resptr) = (byte)(255 - *(bmpptr++));
                    *(buffer++) = *(resptr++);
                    *(resptr) = 255;
                    *(buffer++) = *(resptr++);
                    bmpptr++;
                }
                for(int h=1;h<res.Height;++h)
                {
                    buffer = sbuffer;
                    for(int i=0;i<count;++i)
                    {
                        (*resptr++) = *(buffer++);
                        (*resptr++) = *(buffer++);
                        (*resptr++) = *(buffer++);
                        (*resptr++) = *(buffer++);
                    }
                }
            }
            bmp.UnlockBits(bmpdata);
            res.UnlockBits(resdata);
            g.DrawImage(res, one);
            res.Dispose();
            /* try {
             Bitmap bmp = (Bitmap)Image;
             if(one.X>two.X)
             {
                 Point sw = one;
                 one = two;
                 two = sw;
             }
             int nextpoint;
             Color inverse_color,color;
             for(;one.X<=two.X;one.X++)
             {
                 nextpoint = one.X;
                 color = bmp.GetPixel(one.X, one.Y);
                 inverse_color = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                 g.DrawLine(new Pen(inverse_color, 2), one, two);
             } 
             }
             catch { }*/
        }
        private void _draw_inverse_color_vertical_line(Point one, Point two, Graphics g)
        {
            if (one.Y > two.Y)
            {
                Point sw = one;
                one = two;
                two = sw;
            }
            Bitmap bmp = (Bitmap)Image;
            BitmapData bmpdata = bmp.LockBits(new Rectangle(new Point(), bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Bitmap res = new Bitmap(2, two.Y-one.Y);
            BitmapData resdata = res.LockBits(new Rectangle(new Point(), res.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int count = two.Y - one.Y;
            unsafe
            {
                byte* bmpptr/* = ((byte*)bmpdata.Scan0.ToPointer()) + one.Y * 4 * bmp.Width + one.X * 4*/, resptr = (byte*)resdata.Scan0.ToPointer();
                //byte* buffer = stackalloc byte[res.Width * 4], sbuffer = buffer;
                int* inp;
                for (int i = 0; i < count; ++i)
                {
                    bmpptr = ((byte*)bmpdata.Scan0.ToPointer()) + (one.Y + i) * 4 * bmp.Width + one.X * 4;
                    for(int j=0;j<res.Width;++j)
                   {
                    *(resptr++) = (byte)(255 - *(bmpptr++));
                    *(resptr++) = (byte)(255 - *(bmpptr++));
                    *(resptr++) = (byte)(255 - *(bmpptr++));
                    *(resptr++) = 255;
                    ++bmpptr;
                    }
                    /*inp = (int*)(resptr - 4);
                    for(int j=1;j<res.Width;j++)
                    {
                        *(++inp) = *(inp - 1);
                    }
                    resptr += (res.Width - 1) * 4;*/
                    /*for(int j=1;j<res.Width;j++)
                    {
                        *(resptr) = *(resptr-4);
                        resptr++;
                        *(resptr) = *(resptr-4);
                        resptr++;
                        *(resptr) = *(resptr-4);
                        resptr++;
                        *(resptr) = *(resptr-4);
                        resptr++;
                    }*/
                }
            }
            bmp.UnlockBits(bmpdata);
            res.UnlockBits(resdata);
            g.DrawImage(res, one);
            res.Dispose();
           // bmp.Dispose();
            //res.Dispose();
             /*try { 
             Bitmap bmp = (Bitmap)Image;
             if (one.Y > two.Y)
             {
                 Point sw = one;
                 one = two;
                 two = sw;
             }
             int nextpoint;
             Color inverse_color, color;
             for (; one.Y <= two.Y; one.Y++)
             {
                 nextpoint = one.Y;
                 color = bmp.GetPixel(one.X, one.Y);
                 inverse_color = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                 g.DrawLine(new Pen(inverse_color, 2), one, two);
             }
             }
             catch { }*/
        }
        private void _draw_inverse_color_rectangle(Graphics g,int x,int y,int ex,int ey)
        {
            _draw_inverse_color_horizontal_line(new Point(x, y), new Point(ex, y), g);
            _draw_inverse_color_horizontal_line(new Point(x, ey), new Point(ex, ey), g);
            _draw_inverse_color_vertical_line(new Point(x, y), new Point(x, ey),g);
            _draw_inverse_color_vertical_line(new Point(ex, y), new Point(ex, ey), g);
        }
        private void _om_mouse_left(object sender, MouseEventArgs e)
        {
            _is_pressed_mouse_left_button = false;
        }
        #endregion /Private utilities

        /*___________________________________________________Частные_атрибуты_класса_________________________________________________________*/
        #region Private atribytes
        private int _min_selection_size;
        private Point _first_mouse_point;
        private Point _second_mouse_point;
        private bool _is_mouse_into;
        private bool _is_pressed_mouse_left_button;
        FractalPictureBoxMode _fpbm;
        private Point _click_pt;
        #endregion /private atribytes

        /*__________________________________________________Делегаты_и_исобытия_класса_______________________________________________________*/
        #region Delegates and events
        /// <summary>
        /// Возникает когда пользователь выделил прямоугольную область на элементе управления в режиме 2D.
        /// </summary>
        /// <param name="sender">Объект вызвавший событие.</param>
        /// <param name="SelectedRect">Выделенный прямоугольник.</param>
        public delegate void RectangleSelectedHandler(object sender, Rectangle SelectedRect);
        /// <summary>
        /// Возникает когда пользователь выделил прямоугольную область на элементе управления в режиме 2D.
        /// </summary>
        public event RectangleSelectedHandler RectangleSelected;

        public delegate void OpenMenuHandler();

        public event OpenMenuHandler OpenMenuEvent;
        #endregion /Delegates and events

        /*________________________________________________Общедоступные_свойства_класса______________________________________________________*/
        #region Public properties
        public int MinSelectionSize
        {
            get
            {
                return _min_selection_size;
            }
            set
            {
                if (value < 1) throw new ArgumentException("MinSelectionSize не может быть меньше единицы.");
                _min_selection_size = value;
            }
        }


        #endregion /Public properties

        /*_________________________________________________Общедоступные_методы_класса_______________________________________________________*/
        #region Public methods
        public void ToClickMode()
        {
            _unset_2d_mode();
            _set_click_mode();
        }
        public void ToScaleMode()
        {
            _unset_click_mode();
            _set_2d_mode();
        }
        #endregion /Public methods
    }
    public enum FractalPictureBoxMode
    {
        none,
        _2DViewer,
        _3DViewer,
        clickmode
    }
}