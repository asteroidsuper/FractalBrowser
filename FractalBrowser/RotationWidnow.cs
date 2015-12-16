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
    public partial class RotationWidnow : Form
    {
        public RotationWidnow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            double[] vector;
            lastrot.Clear();
            foreach(string str in richTextBox1.Lines)
            {
                
                if (str.Length < 1) continue;
                if (!checkstr(str)) { i++; continue; }
                if ((vector=getvector(str)) == null) continue;
                if (Rotater == null) Rotater = new Quaternion(GetRad(str), vector[0], vector[1], vector[2]);
                else Rotater=Rotater*new Quaternion(GetRad(str), vector[0], vector[1], vector[2]);
                lastrot.Add(str);
            }
            DialogResult = DialogResult.Yes;
            if (Rotater == null) Rotater = Quaternion.Null;
            //if (Rotater.Radian == 0) Rotater = Quaternion.Null;
            this.Dispose();
        }

        /*________________________________________________________Частные_утилиты_класса______________________________________________________*/
        #region Private utilities of class
        private string GetSkoba(string arg)
        {
            int stindex = arg.IndexOfAny(new char[]{'{','['}),enindex=arg.IndexOfAny(new char[]{'}',']'});
            return arg.Substring(++stindex, enindex-stindex);
        }
        private double[] getvector(string arg)
        {
            string sk = GetSkoba(arg);
            string[] vecstr = sk.Split(',');
            if (vecstr.Length != 3) return null;
            double[] result = new double[3];
            for(int i=0;i<3;i++)
            {
                if(!double.TryParse(vecstr[i].Replace('.', ','), out result[i]))return null;
            }
            return result;
        }
        private double GetRotNum(string arg)
        {
            try
            {
                double res = 0;
                string rs = arg.Split(']', '}')[1];
                foreach(string str in rs.Split(' '))
                {
                    if(str.Length>0)if (double.TryParse(str.Replace('.', ','), out res)) return res;
                }
                return res;
            }
            catch
            {
                return 0;
            }
        }
        private double GetRad(string arg)
        {
            double rot = GetRotNum(arg);
            if (arg.IndexOfAny(new char[]{'g', 'G'}) > -1) ;
            else if(arg.IndexOfAny(new char[]{'r', 'R'})>-1)
            {
                return rot;
            }
            return (rot / 180D) * Math.PI;
        }
        private bool checkstr(string arg)
        {
            if (arg.IndexOfAny(new char[]{'{', '['}) < 0 || arg.IndexOfAny(new char[]{'}', ']'}) < 0) return false;
            if (arg.LastIndexOfAny(new char[] { '{', '[' }) != arg.IndexOfAny(new char[] { '{', '[' })) return false;
            if (arg.LastIndexOfAny(new char[] { '}', ']' }) != arg.IndexOfAny(new char[] { '}', ']' })) return false;
            return arg.Split(']', '}').Length == 2;
        }
        static List<string> lastrot;
        #endregion /Private utilities of class

        /*_____________________________________________________________Результаты______________________________________________________________*/
        #region Result
        public Quaternion Rotater;
        public IncisionOf3DMandelbrot IncisionOf3DMandlebrot
        {
         get
            {
                return new IncisionOf3DMandelbrot(Rotater);
            }
    
        }
        public IncisionOf3DJulia IncisionOf3DJulia
        {
            get
            {
                return new IncisionOf3DJulia(Rotater);
            }
        }
        #endregion /Result

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        
        private void RotationWidnow_Load(object sender, EventArgs e)
        {
            if (lastrot == null) lastrot = new List<string>();
            else richTextBox1.Lines = lastrot.ToArray();
            GlobalTemplates.SetTemplate(panel1, "Шрифт окна вращения фрактала");
        }

       

        private void подсказкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,"В текстовое поле вводятся последовательность вращений фрактала,\n"+
                "Каждое вращение надо вводить в отдельной строке в следующем формате:\n"+
                "В фигурных{} или квадратных[] скобках нужно вводить вектор XYZ, [OX,OY,OZ] или {OX,OY,OZ},"+
                " действительные числа должны ражделяться запятой \"[1,0,0]\" просто ось OX,\n"+
                "\"[12,3,4]\" или \"{12,3,4}\" некий вектор,\n"+"затем должна идти действительная цифра, которая обозначает вращение,"+
                " может представлятся в виде радианов или градусов (по умолчанию в градусах),\n"+
                "для указания используются латинские буквы\ng (или G) - градусы и r (или R) - радианы"+
                "\n\"[1,0,0] 90\" или \"[1,0,0] 90 g\" поворот вокруг OX на 90 градусов,\n"+
                "\"[1,0,0] 1.57 r\" поворот примерно на 90 градусов вокруг OX (если латинская буква не указана,"+
                " по умолчанию считается что используем градусы).\nНекорректные строки будут игнорироватся.\n\n"+
                "Пример входных данных:\"\n[1,0,0] 90\n[0,1,0] 45\n[0,0,1] 0.3 r\"","Подсказка",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
