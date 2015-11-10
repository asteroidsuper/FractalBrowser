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
    public partial class SizeEditor : Form
    {
        public SizeEditor()
        {
            InitializeComponent();
        }

        public SizeEditor(string Title)
        {
            InitializeComponent();
            this.Text = Title;
        }

        private void SizeEditor_Load(object sender, EventArgs e)
        {
            textBox1.KeyPress += FormEventHandlers.OnlyPositiveNumber;
            textBox2.KeyPress += FormEventHandlers.OnlyPositiveNumber;
            int difference_in_width = panel1.Width - textBox1.Width;
            panel1.SizeChanged += (_sender, _e) => { textBox1.Size = textBox2.Size = new Size(panel1.Width-difference_in_width,textBox2.Height); };
            int difference_in_width_pt = this.Width - panel1.Width;
            this.SizeChanged += (_sender, _e) => { panel1.Size = new Size(this.Width-difference_in_width_pt,panel1.Height); };
            button1.Click += (_sender, _e) =>
            {int width=0,height=0;
            int.TryParse(textBox1.Text, out width);
            int.TryParse(textBox2.Text, out height);
            if (BuldButtonClick != null) Invoke(BuldButtonClick, _sender, new Size(width, height));
            this.Close();
            };
            button2.Click+=(_sender,_e)=>{
            int width = 0, height = 0;
            int.TryParse(textBox1.Text, out width);
            int.TryParse(textBox2.Text, out height);
            if (OtherWindowButtonClick != null) Invoke(OtherWindowButtonClick, _sender, new Size(width, height));
            this.Close();
            };
            button3.Click += (_sender, _e) =>{this.Close();};
        }
        /*_____________________________________________________Делегаты_и_эвенты_класса_____________________________________________*/
        #region Delegates and events
        public delegate void BuldButtonClickHandler(object sender, Size size);
        public event BuldButtonClickHandler BuldButtonClick;
        public delegate void OtherWindowButtonClickHandler(object sender, Size size);
        public event OtherWindowButtonClickHandler OtherWindowButtonClick;
        #endregion /Delegates and events
    }
}
