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
    public partial class OneNumberEditor : Form
    {
        public OneNumberEditor()
        {
            InitializeComponent();
        }
        public OneNumberEditor(decimal BaseVaule)
        {
            InitializeComponent();
            numericUpDown1.Value = BaseVaule;
        }
        public OneNumberEditor(decimal BaseValue,decimal MaxValue)
        {
            InitializeComponent();
            numericUpDown1.Maximum = MaxValue;
            numericUpDown1.Value = BaseValue;
        }
        public OneNumberEditor(decimal BaseValue, decimal MaxValue,int IncremenLength)
        {
            InitializeComponent();
            numericUpDown1.Maximum = MaxValue;
            numericUpDown1.Increment = IncremenLength;
            numericUpDown1.Value = BaseValue;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            value=numericUpDown1.Value;
            this.Dispose();
        }
        public decimal value;
    }
}
