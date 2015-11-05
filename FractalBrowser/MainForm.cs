using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*Тестовый прогон*/
namespace FractalBrowser
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        Bitmap bmp;
        private void MainForm_Load(object sender, EventArgs e)
        {
            MainPanel.Paint += (o, v) => { if (bmp != null) { v.Graphics.DrawImage(bmp,0,0); } };
            Experimental2DFractal1 ep = new Experimental2DFractal1(150, -4, 4, -4, 4);
            ep.ParallelFractalCreatingFinished += (s, f) => { bmp = (new Simple2DFractalColorMode()).GetDrawnBitmap(f); MainPanel.Invalidate(); };
            ep.CreateParallelFractal(960, 640);
            _differenсe_in_width = this.Width - MainPanel.Width;
            _difference_in_height = this.Height - MainPanel.Height;

        }







        /*___________________________________________________________________________Прочие_данные_формы___________________________________________________________*/
        #region Other data
        private int _differenсe_in_width;
        private int _difference_in_height;


        #endregion /Other data

        /*_________________________________________________________________________Прочие_обработки_событий________________________________________________________*/
        #region Other event workers
        private void _on_size_of_MainForm_changed(object sender,EventArgs e)
        {
            MainPanel.Size = new Size(this.Width - _differenсe_in_width, this.Height - _difference_in_height);
        }

        #endregion /Other event workers

        
    }
}
