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
        /*___________________________________________________________________________Фрактальная_часть_____________________________________________________________*/
        #region Fractal part of Form
        private FractalDataHandlerControler FractalControler;
        private FractalPictureBox MainFractalPictureBox;
        #region Julia Handlers
        /// <summary>
        /// Джулия с комплексной константой -0.8+0.156i
        /// </summary>
        private FractalDataHandler FirstJulia;
        private FractalDataHandler SecondJulia;
        private FractalDataHandler ThirdJulia;
        private FractalDataHandler FourthJulia;
        private FractalDataHandler FifthJulia;
        #endregion /Julia handlers

        #endregion /Fractal part of Form
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            FractalControler = new FractalDataHandlerControler();
            MainFractalPictureBox = new FractalPictureBox();
            MainFractalPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            MainPanel.Controls.Add(MainFractalPictureBox);
            #region Julia creating
            FirstJulia = new FractalDataHandler(this, new Julia(FractalStaticData.RecomendJuliaIterationsCount, -1.523D, 1.523D, -0.9D, 0.9D, new Complex(-0.8D, 0.156D)),MainFractalPictureBox,new My2DClassicColorMode(),new Size(960,640),FractalControler);
            FirstJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            FirstJulia.ConnectShowToMenuItem(первыйФракталToolStripMenuItem, FractalControler,32,32);
            FirstJulia.ConnectStandartResetToMenuItem(новыйСтандартногоРазмераToolStripMenuItem, FractalControler);
            FirstJulia.ConnectResetWithSizeFromWindowToMenuItem(новыйСЗаданнымРазмеромToolStripMenuItem, FractalControler);
            SecondJulia = new FractalDataHandler(this, new Julia(FractalStaticData.RecomendJuliaIterationsCount, -0.91D, 0.91D, -1.12D, 1.12D, new Complex(0.285D, 0.0126D)), MainFractalPictureBox, new My2DClassicColorMode(), new Size(960, 640), FractalControler);
            SecondJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            SecondJulia.ConnectShowToMenuItem(второйФракталToolStripMenuItem, FractalControler,32,32);
            ThirdJulia = new FractalDataHandler(this, new Julia(FractalStaticData.RecomendJuliaIterationsCount, -1.35D, 1.35D, -1.12D, 1.12D, new Complex(-0.0085D, 0.71D)), MainFractalPictureBox, new My2DClassicColorMode(1.1D,1.1D,1.1D), new Size(960, 640), FractalControler);
            ThirdJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            ThirdJulia.ConnectShowToMenuItem(третийToolStripMenuItem, FractalControler, 32, 32);
            FourthJulia = new FractalDataHandler(this, new Julia(FractalStaticData.RecomendJuliaIterationsCount, -0.88D, 0.88D, -1.12D, 1.12D, new Complex(0.285D, 0.01D)), MainFractalPictureBox, new My2DClassicColorMode(), new Size(960, 640), FractalControler);
            FourthJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            FourthJulia.ConnectShowToMenuItem(четвёртыйФракталToolStripMenuItem, FractalControler, 32, 32);
            FifthJulia = new FractalDataHandler(this,new Julia(FractalStaticData.RecomendJuliaIterationsCount, -1.505D, 1.505D, -0.9D, 0.9D, new Complex(-0.74534D, 0.11301D)), MainFractalPictureBox, new My2DClassicColorMode(), new Size(960, 640), FractalControler);
            FifthJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            FifthJulia.ConnectShowToMenuItem(пятыйФракталToolStripMenuItem, FractalControler, 32, 32);
            #endregion /Julia creating
            _differenсe_in_width = this.Width - MainPanel.Width;
            _difference_in_height = this.Height - MainPanel.Height;
            FractalDataHandler.UseSafeZoom = true;
            FractalDataHandler.MaxGlobalPercent = toolStripProgressBar1.Maximum;
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
