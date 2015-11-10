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
    public partial class IsolatedFractalWindowsCreator : Form
    {
        public IsolatedFractalWindowsCreator(Fractal fractal)
        {
            InitializeComponent();
            _fractal=fractal;
        }

        private void IsolatedFractalWindowsCreator_Load(object sender, EventArgs e)
        {
            differently_in_width=this.Width-progressBar1.Width;
            this.Text = _fractal.ToString();
            this.SizeChanged += (_sender, _e) => { progressBar1.Size = new Size(this.Width-differently_in_width,progressBar1.Height); };
        }


        /*______________________________________________________________Общедоступные_методы___________________________________________________________*/
        #region Public methods
        public void StartProcess(int Width,int Height)
        {
            if (Width < 1 || Height < 1) return;
            this.Show();
            this.Text = this.Text + " (" + Width + "x" + Height + ")";
            _fractal.MaxPercent = progressBar1.Maximum;
            Action<ProgressBar, int> SetProcessProgress = (bar, percent) => { bar.Increment(percent - bar.Value); };
            _fractal.ProgressChanged += (sender, percent) => { Invoke(SetProcessProgress,progressBar1,percent); };
            Action<Button> SetButton = (button) => { button.Text = "Забрать";
            button.Click -= First_main_button_Click_Worker;
            button.Click += (sender, e) => { if (FractalToken != null)FractalToken(_fractal, _fap); this.Dispose(); };
            };
            _fractal.ParallelFractalCreatingFinished += (fractal, FAP) =>
            {if (FractalReady != null)Invoke(FractalReady, fractal, FAP);    
            _fap = FAP;
            Invoke(SetButton, button1);
            Fractal.ClearProgressChangedEvents(fractal);
            Fractal.ClearParallelFractalCreatingFinishedEvents(fractal);
            };
            _fractal.CreateParallelFractal(Width, Height);
        }

        public void StartProcess(int Width,int Height,int HorizontalStart,int VerticalStart,int SelectedWidth,int SelectedHeight,bool UseSafeZoom=false)
        {
            this.Show();
            this.Text = this.Text + " (" + Width + "x" + Height + ")";
            _fractal.MaxPercent = progressBar1.Maximum;
            Action<ProgressBar, int> SetProcessProgress = (bar, percent) => { bar.Increment(percent - bar.Value); };
            _fractal.ProgressChanged += (sender, percent) => { Invoke(SetProcessProgress, progressBar1, percent); };
            Action<Button> SetButton = (button) =>
            {
                button.Text = "Забрать";
                button.Click -= First_main_button_Click_Worker;
                button.Click += (sender, e) => { if (FractalToken != null)FractalToken(_fractal, _fap); this.Dispose(); };
            };
            _fractal.ParallelFractalCreatingFinished += (fractal, FAP) =>
            {
                if(FractalReady!=null)Invoke(FractalReady, fractal, FAP);
                _fap = FAP;
                Invoke(SetButton, button1);
            };
            _fractal.CreateParallelFractal(Width, Height,HorizontalStart,VerticalStart,SelectedWidth,SelectedHeight,UseSafeZoom);

        }
        #endregion /Public methods
        /*_________________________________________________________________Частные_данные______________________________________________________________*/
        #region Private data
        private Fractal _fractal;
        private FractalAssociationParametrs _fap;
        #endregion /Private data

        /*_____________________________________________________________Данные_для_маштабирования_______________________________________________________*/
        #region Scale data
        private int differently_in_width;

        #endregion /Scale data

        /*_____________________________________________________________Делегаты_и_события_класса_______________________________________________________*/
        #region Delegates and events
        public delegate void FractalReadyHandler(Fractal fractal, FractalAssociationParametrs FAP);
        public event FractalReadyHandler FractalReady;
        public delegate void FractalTokenHandler(Fractal fractal, FractalAssociationParametrs Result);
        public event FractalTokenHandler FractalToken;

        #endregion Delegates and events

        private void First_main_button_Click_Worker(object sender, EventArgs e)
        {
            _fractal.CancelParallelCreating();
            button1.Text = "Закрыть";
            button1.Click -= First_main_button_Click_Worker;
            button1.Click += (_sender, _e) => { this.Dispose(); };
        }
    }
}
