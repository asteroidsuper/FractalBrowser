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
    public partial class VisualColorControler : Form
    {
        public VisualColorControler()
        {
            InitializeComponent();
            _old_width = panel1.Width;
            _old_height = panel1.Height;
        }
        public VisualColorControler(Fractal fractal)
        {
            _fractal = fractal;
            InitializeComponent();
            _old_width = panel1.Width;
            _old_height = panel1.Height;
        }
        public VisualColorControler(Fractal fractal,FractalColorMode FractalColorMode)
        {
            _fractal = fractal;
            this.FractalColorMode = FractalColorMode;
            _old_width = panel1.Width;
            _old_height = panel1.Height;
            InitializeComponent();

        }
        public VisualColorControler(Fractal fractal, FractalColorMode FractalColorMode,int OldWidth,int OldHeight,FractalAssociationParametrs fap)
        {
            _fractal = fractal;
            this.FractalColorMode = FractalColorMode;
            InitializeComponent();
            _old_height = OldHeight;
            _old_width = OldWidth;
            _fap = fap;
        }
        private void VisualColorControler_Load(object sender, EventArgs e)
        {
            int width_difference = this.Width - panel1.Width, height_difference = this.Height - panel1.Height,x_pos_difference=this.Width-panel2.Location.X,_2heigth_difference=this.Height-panel2.Height;
            SizeChanged += (_sender, _e) => { panel1.Size = new Size(this.Width-width_difference,this.Height-height_difference);
            panel2.Location = new Point(this.Width - x_pos_difference, panel2.Location.Y);
            comboBox1.Location = new Point(this.Width - x_pos_difference, comboBox1.Location.Y);
            panel2.Size = new Size(panel2.Width,this.Height-_2heigth_difference);
            };
            _fractal_picture_box = new FractalPictureBox();
            panel1.Controls.Add(_fractal_picture_box);
            _fractal_picture_box.SizeMode = PictureBoxSizeMode.AutoSize;
            _fractal_picture_box.Visible = true;
            ConnectFractalToClass(_fractal);
            _fractal_picture_box.SelectionPen = null;
            _fractal.CreateParallelFractal(panel1.Width, panel1.Height, 0, 0, _old_width, _old_height);
            panel2.Controls.Add(FractalColorMode.GetUniqueInterface(panel2.Width,panel2.Height));
            fcm_list = new List<FractalColorMode>();
            compatible_fcm_list = new List<FractalColorMode>();
            //list_up(FractalColorMode);
            comboBox1.SelectedIndexChanged += (_sender, _e) => { FractalColorMode = (FractalColorMode)comboBox1.SelectedItem; Visualizate(); 
                panel2.Controls.Clear(); 
                panel2.Controls.Add(FractalColorMode.GetUniqueInterface(panel2.Width,panel2.Height)); };
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            if (_fap != null) Visualizate();
            //DrawFractalParallel();
        }


        /*____________________________________________________________________Частные_методы_класса____________________________________________________________*/
        #region Private methods of class
        private void DrawFractalParallel()
        {
            _fractal.CreateParallelFractal(panel1.Width, panel1.Height,0,0,panel1.Width,panel1.Height);
        }
        private void Visualizate()
        {
            
                Action<Bitmap> act = (Bmp) => { _fractal_picture_box.Image = Bmp; };
                Invoke(act, FractalColorMode.GetDrawnBitmap(_fap));
            
            //_fractal_picture_box.Image = FractalColorMode.GetDrawnBitmap(_fap);
        }
        private void ConnectFractalToClass(Fractal fractal)
        {
            Action<FractalPictureBox, Bitmap> actfinish = (fpb, bmp) => { fpb.Image = bmp; };
            fractal.ParallelFractalCreatingFinished += (f, fap) =>
            {
                Bitmap bmp = FractalColorMode.GetDrawnBitmap(fap);
                Invoke(actfinish,_fractal_picture_box, bmp);
                _fap = fap;
            };
            Action<ToolStripProgressBar, int> actpc = (pb, percent) => { pb.Increment(percent - pb.Value); };
            fractal.ProgressChanged+=(f,percent)=>{
                Invoke(actpc,toolStripProgressBar1,percent);
            };
            _fractal_picture_box.RectangleSelected += (sender, rect) => { if (_fractal.IsBusy)return;
            _fractal.CreateParallelFractal(panel1.Width,panel1.Height,rect.X,rect.Y,rect.Width,rect.Height);
            };
            fractal.ParallelFractalCreatingFinished += FirstSetColorModes;
        }

        private void list_up(FractalAssociationParametrs fap)
        {
            
        }
        private void list_up(FractalColorMode fcm_c)
        {
            comboBox1.Items.Add(fcm_c);
            FractalColorMode fcm = new My2DClassicColorMode();
            if (fcm.IsCompatible(_fap)&&!(fcm.GetType().Equals(fcm_c.GetType()))) comboBox1.Items.Add(fcm);
            fcm = new Simple2DFractalColorMode();
            if (fcm.IsCompatible(_fap) && !(fcm.GetType().Equals(fcm_c.GetType()))) comboBox1.Items.Add(fcm);
            fcm = new SimpleInverse2DFractalColorMode();
            if (fcm.IsCompatible(_fap) && !(fcm.GetType().Equals(fcm_c.GetType()))) comboBox1.Items.Add(fcm);
            fcm = new SimpleClouds2DFractalColorMode(100);
            if (fcm.IsCompatible(_fap) && !(fcm.GetType().Equals(fcm_c.GetType()))) comboBox1.Items.Add(fcm);
            fcm = new CosColorMode();
            if (fcm.IsCompatible(_fap) && !(fcm.GetType().Equals(fcm_c.GetType()))) comboBox1.Items.Add(fcm);
            fcm = new CycleGradientColorMode(1000,0);
            if (fcm.IsCompatible(_fap) && !(fcm.GetType().Equals(fcm_c.GetType()))) comboBox1.Items.Add(fcm);
            fcm = new TrioArgsCosColorMode();
            if (fcm.IsCompatible(_fap) && !(fcm.GetType().Equals(fcm_c.GetType()))) comboBox1.Items.Add(fcm);
            foreach(object o in comboBox1.Items)
            {
                ((FractalColorMode)o).FractalColorModeChanged += (sender, c) => { Visualizate(); };
            }
        }
        private void FirstSetColorModes(Fractal fractal,FractalAssociationParametrs FAP)
        {
            Action act = () =>
            {
                _fap = FAP;
                list_up(FractalColorMode);
                comboBox1.SelectedIndex = 0;
            };
            Invoke(act);
            fractal.ParallelFractalCreatingFinished -= FirstSetColorModes;
        }
        #endregion /Private methods of class

        /*_________________________________________________________________Общедоступные_данные_класса_________________________________________________________*/
        #region Public data
        public FractalColorMode FractalColorMode;

        #endregion /Public data

        /*_______________________________________________________________________Данные_класса_________________________________________________________________*/
        #region Data of class
        private Fractal _fractal;
        private FractalPictureBox _fractal_picture_box;
        FractalAssociationParametrs _fap;
        List<FractalColorMode> fcm_list,compatible_fcm_list;
        private int _old_width, _old_height;
        #endregion /Data of class

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            this.Dispose();
        }
    }
}
