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
    public partial class FontVisualControler : Form
    {
        public FontVisualControler()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            textBox1.Font =GlobalTemplates.GetTemplateFont((string)listBox1.SelectedItem);
            textBox1.ForeColor = GlobalTemplates.GetTemplateForeColor((string)listBox1.SelectedItem);
           button2.Enabled=button1.Enabled = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = GlobalTemplates.GetTemplateFont((string)listBox1.SelectedItem);
            if(fd.ShowDialog(this)!=DialogResult.None)
            {
                GlobalTemplates.ChangeTemplate((string)listBox1.SelectedItem, GlobalTemplates.GetTemplateForeColor((string)listBox1.SelectedItem), fd.Font);
            }
            listBox1_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void FontVisualControler_Load(object sender, EventArgs e)
        {
            GlobalTemplates.AddDefaultTemplate("Шрифт этого окна", GlobalTemplates.DefaultColor, GlobalTemplates.DefaultFont);
            GlobalTemplates.SetTemplate(panel1,"Шрифт этого окна");
            listBox1.HorizontalScrollbar = true;
            listBox1.Items.AddRange(GlobalTemplates.GetAllNamesOfTemplates());
            textBox1.Text="Пример шрифта";
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = GlobalTemplates.GetTemplateForeColor((string)listBox1.SelectedItem);
            if (cd.ShowDialog(this) != DialogResult.None)
            {
                GlobalTemplates.ChangeTemplate((string)listBox1.SelectedItem,cd.Color , GlobalTemplates.GetTemplateFont((string)listBox1.SelectedItem));
            }
            listBox1_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GlobalTemplates.ToDefault();
        }
    }
}
