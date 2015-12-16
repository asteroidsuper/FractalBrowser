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
    public partial class OneStringEditor : Form
    {
        public OneStringEditor()
        {
            InitializeComponent();
        }
        public OneStringEditor(string StartLabel)
        {
            InitializeComponent();
            label1.Text = StartLabel; 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Dispose();
        }
        public string Result;
        private void OneStringEditor_Load(object sender, EventArgs e)
        {
           int half_label_width = label1.Width>>1;
           label1.Location = new Point((panel1.Width>>1)-half_label_width,label1.Location.Y);           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Result = textBox1.Text;
            this.Dispose();
        }
    }
}
