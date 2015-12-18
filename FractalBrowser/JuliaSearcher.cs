using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalBrowser
{
    public partial class JuliaSearcher : Form
    {
        public JuliaSearcher()
        {
            InitializeComponent();
            Text = "Тестовое окно поисковика Жюлиа";
            _julia = new Julia(250, -1.5, 1.5, -1, 1, new Complex());
            _j_fcm = new My2DClassicColorMode(1.85, 1.4, 1.8, 4);
            _using_complex = new Complex();
        }
        public JuliaSearcher(IUsingComplex JuliaLike)
        {
            InitializeComponent();
            _julia = JuliaLike;
            _j_fcm = new My2DClassicColorMode(1.85, 1.4, 1.8, 4);
            _using_complex = JuliaLike.GetComplex();
        }
        public JuliaSearcher(IUsingComplex JuliaLike,FractalColorMode JuliaMode)
        {
            InitializeComponent();
            _using_complex = JuliaLike.GetComplex();
            _julia = JuliaLike;
            _j_fcm = JuliaMode;  
        }
        public JuliaSearcher(IUsingComplex JuliaLike,_2DFractal BackSideFractal,FractalColorMode JuliaMode):this(JuliaLike,JuliaMode)
        {
            //if (!(BackSideFractal is _2DFractal)) throw new ArgumentException("Переданный объект для панели выбора должен быть двухмерным фракталом!");
            _mandelbrot =BackSideFractal;
        }
        public JuliaSearcher(IUsingComplex JuliaLike, _2DFractal BackSideFractal, FractalColorMode JuliaMode,FractalColorMode BackSideFractalMode)
            : this(JuliaLike, JuliaMode)
        {
            _mandelbrot = BackSideFractal;
            _m_fcm = BackSideFractalMode;
        }
        private void JuliaSearcher_Load(object sender, EventArgs e)
        {
            
            fpb1 = new FractalPictureBox();
            fpb1.SizeMode = PictureBoxSizeMode.AutoSize;
            fpb1.ContextMenuStrip = contextMenuStrip1;
            if(_m_fcm==null)_m_fcm = new Simple2DFractalColorMode();
            panel1.Controls.Add(fpb1);
            fpb1.ToClickMode();
            if(_mandelbrot==null)_mandelbrot = new Mandelbrot();
            _m_fap = _mandelbrot.CreateFractal(panel1.Width, panel1.Height);
            fpb1.Image = _m_fcm.GetDrawnBitmap(_m_fap);
            fpb1.MouseMove += (_sender, _e) => {label1.Text = _get_complex_loc(_mandelbrot,_e.X,_e.Y).ToString(); };
            fpb2 = new FractalPictureBox();
            fpb2.SizeMode = PictureBoxSizeMode.AutoSize;
            panel2.Controls.Add(fpb2);
            fpb1.MouseDown += (_sender, _e) =>
            {
                if (!clickable) return;
                if (_e.Button != MouseButtons.Left) return;
                _using_complex = _get_complex_loc(_mandelbrot, _e.X, _e.Y);
                label2.Text=_using_complex.ToString();
                _julia.SetComplex(_using_complex);
                fpb2.Image = _j_fcm.GetDrawnBitmap(((_2DFractal)_julia).CreateFractal(panel2.Width, panel2.Height));
            };
            fpb2.RectangleSelected += (_sender, rec) =>
            {
                ((_2DFractal)_julia).CreateParallelFractal(panel2.Width, panel2.Height, rec.X, rec.Y, rec.Width, rec.Height, true);
            };
            ((_2DFractal)_julia).ParallelFractalCreatingFinished += (s, fap) =>
            { 
            Action<Bitmap> act=(bmp)=>{fpb2.Image=bmp;};
            Invoke(act, _j_fcm.GetDrawnBitmap(fap));
            };
            fpb2.SelectionPen = null;
            fpb1.RectangleSelected += (_s, rec) => {
                _mandelbrot.CreateParallelFractal(panel1.Width, panel1.Height, rec.X, rec.Y, rec.Width, rec.Height,true);
            };
            _mandelbrot.ParallelFractalCreatingFinished += (s, fap) =>
            {
                Action<Bitmap> act = (bmp) => { fpb1.Image = bmp; };
                Invoke(act, _m_fcm.GetDrawnBitmap(fap));
            };
            fpb2.ContextMenuStrip = contextMenuStrip2;
            fpb1.SelectionPen = null;
            clickable = true;
            _fpb1_h_scale = this.Width / (double)panel1.Width;
            _fpb1_v_scale = this.Height / (double)panel1.Height;
            _fpb2_h_scale = this.Width / (double)panel2.Width;
            _fpb2_v_scale = this.Height / (double)panel2.Height;
            oldheight = panel1.Height;
            oldwidth = panel1.Width;
            вРежимВращенияToolStripMenuItem.Visible = вРежимВращенияToolStripMenuItem.Enabled = _mandelbrot is IUsingQuaternion;
            
        }


        /*_____________________________________________________________Частные_атрибуты_класса____________________________________________________*/
        #region private atribytes
        private FractalPictureBox fpb1,fpb2;
        private Complex _using_complex;
        private FractalColorMode _m_fcm;
        private FractalColorMode _j_fcm;
        private IUsingComplex _julia;
        private _2DFractal _mandelbrot;
        private FractalAssociationParametrs _m_fap;
        bool clickable;
        #endregion /private atribytes

        /*_____________________________________________________________Частные_утилиты_класса_____________________________________________________*/
        #region private utilites
        private double _get_real_loc(double left,double right,int loc,int length)
        {
            double dist = right - left;
            double step = dist / length;
            return left + (loc * step);
        }
        private Complex _get_complex_loc(_2DFractal fractal,int x,int y)
        {
            double real = fractal.AbcissStart + fractal.AbcissIntervalLength * x;
            double imagine = fractal.OrdinateStart + fractal.OrdinateIntervalLength * y;
            return new Complex(real,imagine);
        }
        #endregion /private utilites

        /*____________________________________________________________Данные_для_масштабирования__________________________________________________*/
        #region Data of scale
        private double _fpb1_h_scale,_fpb1_v_scale,_fpb2_h_scale,_fpb2_v_scale;
        private int oldwidth, oldheight;
        #endregion /Data of scale

        /*_____________________________________________________________Общедоступые_поля_класса___________________________________________________*/
        #region Public filds
        public Complex Complex
        {
            get { return _using_complex; }
        }
        public Julia Julia
        {
            get
            {
                return new Julia(((_2DFractal)_julia).Iterations, -1.503, 1.503, -1, 1, _using_complex.getclone());
            }
        }
        public AmoebaLasVegas AmoebaLasVegas
        {
            get
            {
                AmoebaLasVegas alv = (AmoebaLasVegas)_julia;
                return new AmoebaLasVegas(alv.ComplexConst, alv.Iterations, alv.LeftEdge, alv.RightEdge, alv.TopEdge, alv.BottomEdge);
            }
        }
      
        public FractalColorMode FractalColorMode
        {
            get
            {
                return _j_fcm.GetClone();
            }
        }
        #endregion /public fields

        private void изменитьЦветовуюМодельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisualColorControler vcc = new VisualColorControler(_mandelbrot.GetClone(), _m_fcm, panel1.Width, panel1.Height,null);
            if (vcc.ShowDialog(this) == DialogResult.Yes)
            {
                _m_fcm = vcc.FractalColorMode;
                fpb1.Image = _m_fcm.GetDrawnBitmap(_m_fap);
            }
        }

        private void вРежимМасштабированияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnCheckConmenu();
            вРежимМасштабированияToolStripMenuItem.Checked = true;
            fpb1.ToScaleMode();
            clickable = false;
        }

        private void вРежимВыбораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnCheckConmenu();
            вРежимВыбораToolStripMenuItem.Checked = true;
            fpb1.ToClickMode();
            clickable = true;
        }

        private void отменитьМасштабированияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mandelbrot.GetBack();
            _mandelbrot.CreateParallelFractal(panel1.Width, panel1.Height, 0, 0, panel1.Width, panel1.Height);
        }

        private void JuliaSearcher_SizeChanged(object sender, EventArgs e)
        {
            panel1.Size=new Size((int)(Width/_fpb1_h_scale),(int)(Height/_fpb1_v_scale));
            label1.Location = new Point(label1.Location.X,panel1.Location.Y+panel1.Height+4);
            panel2.Location = new Point(panel2.Location.X,label1.Location.Y + label1.Height + 4);
            panel2.Size = new Size((int)(Width / _fpb2_h_scale), (int)(Height / _fpb2_v_scale));
            label2.Location = new Point(panel2.Location.X, panel2.Location.Y + panel2.Height + 4);
            //_mandelbrot.CreateParallelFractal(panel1.Width, panel1.Height, 0, 0,oldwidth,oldheight );
            oldheight = panel1.Height;
            oldwidth = panel1.Width;
        }

        private void изменитьЦветовуюМодельToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            VisualColorControler vcc = new VisualColorControler(((_2DFractal)_julia).GetClone(), _j_fcm, panel2.Width, panel2.Height, null);
            if (vcc.ShowDialog(this) != DialogResult.Yes) return;
            _j_fcm = vcc.FractalColorMode;
            ((_2DFractal)_julia).CreateParallelFractal(panel2.Width, panel2.Height, 0, 0, panel2.Width, panel2.Height);
        }

        private void забратьФракталToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            this.Dispose();
        }
        private void UnCheckConmenu()
        {
           foreach(ToolStripMenuItem item in contextMenuStrip1.Items)
           {
               item.Checked = false;   
           }
        }
        private void вРежимВращенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnCheckConmenu();
        }
        
    }
}
