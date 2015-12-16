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
    public partial class TemplatesViewer : Form
    {
        public TemplatesViewer(FractalTemplates ts)
        {
            InitializeComponent();
            tms = ts.Templates;
            fractalTemplates = ts;
        }
        FractalTemplate[] tms;
        FractalTemplates fractalTemplates;
        List<Template> list_temp;
        private void TemplatesViewer_Load(object sender, EventArgs e)
        {
            GlobalTemplates.SetTemplate(panel1, "Шрифт окна с шаблонами");
            list_temp = new List<Template>();
            Template temp;
            foreach(FractalTemplate ft in tms)
            {
                temp = new Template(ft, panel1, this,toolTip1);
                temp.selected += (received) => { selectedtemplate = received; DialogResult = DialogResult.Yes; this.Dispose(); };
                temp.removed += (received) => { fractalTemplates.Remove(received); };
                list_temp.Add(temp);
            }
            this.Disposed += (_sender, _e) => { toolTip1.RemoveAll(); };
        }
        public FractalTemplate selectedtemplate;


        /*____________________________________________________________________Частные_подклассы___________________________________________________________*/
        #region Private subclasses
        private class Template
        {   public Template(FractalTemplate fractaltemplate,Panel paneltoadd,Form owner)
            {
                ft = fractaltemplate;
                panel = new Panel();       
                panel.Size=new Size(paneltoadd.Width-10,80);
                PictureBox pb = new PictureBox();
                pb.SizeMode = PictureBoxSizeMode.AutoSize;
                pb.Location = new Point(3, 10);
                fr = ft.Fractal.GetClone();
                fr.ParallelFractalCreatingFinished += (sender, fap) => { 
                    Action<Bitmap> setbit=(bit)=>{pb.Image=bit;};
                    owner.Invoke(setbit,ft.FractalColorMode.GetDrawnBitmap(fap)); };
                //if (fr.CanBack()) 
            fr.CreateParallelFractal(60, 60, 0, 0, 60, 60);
                //else fr.CreateParallelFractal(60, 60);
                panel.Controls.Add(pb);
                Label namelabel = new Label();
                namelabel.Size = new Size(190,namelabel.Height);;
                namelabel.Text = ft.Name;
                namelabel.Location = new Point(66, 10);
            if(paneltoadd.Controls.Count<1)
            {
                panel.Location = new Point(3, 1);
                paneltoadd.Controls.Add(panel);
            }
            else
            {
                Control lcontrol = paneltoadd.Controls[paneltoadd.Controls.Count - 1];
                panel.Location = new Point(3, lcontrol.Height + lcontrol.Location.Y + 3);
                paneltoadd.Controls.Add(panel);
            }
            panel.Controls.Add(namelabel);
            Label datalabel = new Label();
            datalabel.Location = new Point(66, 45);
            datalabel.Text = ft.Date.ToString();
            panel.Controls.Add(datalabel);
            panel.BorderStyle = BorderStyle.Fixed3D;
            panel.BackColor = Color.White;
            Button bt = new Button();
            bt.Text = "Достать";
            bt.Size = new Size(90, 30);
            bt.Location = new Point(panel.Width-bt.Width-3,10);
            panel.Controls.Add(bt);
            bt.Show();
            bt.Click += (s, e) => { if (selected != null)selected(ft); };
            bt = new Button();
            bt.Size = new Size(90, 30);
            bt.Location = new Point(panel.Width - bt.Width - 3, 40);
            bt.Click += (s, e) => { if (removed != null)removed(ft); };
            panel.Controls.Add(bt);
            bt.Text = "Удалить";
            }
            public Template(FractalTemplate fractaltemplate,Panel paneltoadd,Form owner,ToolTip tooltip):this(fractaltemplate,paneltoadd,owner)
            {
                foreach(Control control in panel.Controls)
                {
                    if(control is Label)
                    {
                        tooltip.SetToolTip(control, ft.Name);
                        break;
                    }
                }
                    if(ft.Fractal is IUsingComplex)
                    {
                        IUsingComplex complex = (IUsingComplex)ft.Fractal;
                        foreach(Control control in panel.Controls)
                        {
                            if(control is PictureBox)
                            {
                                tooltip.SetToolTip(control,"Использует комплексное число = "+ complex.GetComplex().ToString());
                                break;
                            }
                        }
                    }
                
            }
            public FractalTemplate ft;
            public delegate void selected_handler(FractalTemplate selectedtemplate);
            public event selected_handler selected;
            public selected_handler removed;
            public Panel panel;
            private Fractal fr;
        }
        #endregion /Private subclasses
    }
}
