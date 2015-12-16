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
    public partial class JuliaEditor : Form
    {
        /*____________________________________________________________Конструкторы_________________________________________________________*/
        #region Constructors
        public JuliaEditor()
        {
            InitializeComponent();
            numericUpDown1.Maximum = (decimal)ulong.MaxValue;
            numericUpDown1.Value = 10000M;
            numericUpDown1.Minimum = 1M;
        }
        public JuliaEditor(Julia BaseJulia)
        {
            InitializeComponent();
            numericUpDown1.Maximum = (decimal)ulong.MaxValue;
            numericUpDown1.Value = 10000M;
            numericUpDown1.Minimum = 1M;
            if (BaseJulia == null) return;
            numericUpDown1.Value = BaseJulia.Iterations;
            LeftEdge = BaseJulia.LeftEdge;
            RightEdge = BaseJulia.RightEdge;
            TopEdge = BaseJulia.TopEdge;
            BottomEdge = BaseJulia.BottomEdge;
            RealPart = BaseJulia.ComplexConst.Real;
            ImaginePart = BaseJulia.ComplexConst.Imagine;
            CreateButton.Select();
        }
        public JuliaEditor(IUsingComplex Base)
        {
            InitializeComponent();
            numericUpDown1.Maximum = (decimal)ulong.MaxValue;
            numericUpDown1.Value = 10000M;
            numericUpDown1.Minimum = 1M;
            if (Base == null) return;
            numericUpDown1.Value = ((_2DFractal)Base).Iterations;
            LeftEdge = ((_2DFractal)Base).LeftEdge;
            RightEdge = ((_2DFractal)Base).RightEdge;
            TopEdge = ((_2DFractal)Base).TopEdge;
            BottomEdge = ((_2DFractal)Base).BottomEdge;
            RealPart = Base.GetComplex().Real;
            ImaginePart = Base.GetComplex().Imagine;
            CreateButton.Select();
        }
        #endregion /Constructors

        /*_________________________________________________________Обработка_событий_______________________________________________________*/
        #region Event handles
        private void JuliaEditor_Load(object sender, EventArgs e)
        {
            LeftEdgeEdit.Text = LeftEdge.ToString();
            RightEdgeEdit.Text = RightEdge.ToString();
            TopEdgeEdit.Text = TopEdge.ToString();
            BottomEdgeEdit.Text = BottomEdge.ToString();
            RealPartEdit.Text = RealPart.ToString();
            ImaginePartEdit.Text = ImaginePart.ToString();
            LeftEdgeEdit.KeyPress += FormEventHandlers.OnlyNumeric;
            RightEdgeEdit.KeyPress += FormEventHandlers.OnlyNumeric;
            TopEdgeEdit.KeyPress += FormEventHandlers.OnlyNumeric;
            BottomEdgeEdit.KeyPress += FormEventHandlers.OnlyNumeric;
            RealPartEdit.KeyPress += FormEventHandlers.OnlyNumeric;
            ImaginePartEdit.KeyPress += FormEventHandlers.OnlyNumeric;
            EditDescriptor.SetToolTip(LeftEdgeLabel, FractalGlobalDescriptions.LeftEdgeOf2DFractal);
            EditDescriptor.SetToolTip(LeftEdgeEdit, "Здесь необходимо вводить левую границу двухмерного фрактала джулии (десятиричное представления числа).\n" + FractalGlobalDescriptions.LeftEdgeOf2DFractal);
            EditDescriptor.SetToolTip(RightEdgeLabel, FractalGlobalDescriptions.RightEdgeOf2DFractal);
            EditDescriptor.SetToolTip(RightEdgeEdit,"Здесь необходимо вводить правую границу двухмерного фрактала джулии (десятиричной представления числа, в качестве разделителей на дроную часть используйте \",\" или \".\").\n"+ FractalGlobalDescriptions.RightEdgeOf2DFractal);
            GlobalTemplates.SetTemplate(MainPanelOfJuliaEditor, "Шрифт окна для ввода фрактала джулии");
        }

        private void ReturnEditedData(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            IterationsCount = (ulong)numericUpDown1.Value;
            double.TryParse(LeftEdgeEdit.Text.Replace('.', ','), out LeftEdge);
            double.TryParse(RightEdgeEdit.Text.Replace('.', ','), out RightEdge);
            double.TryParse(TopEdgeEdit.Text.Replace('.', ','), out TopEdge);
            double.TryParse(BottomEdgeEdit.Text.Replace('.', ','), out BottomEdge);
            double.TryParse(RealPartEdit.Text.Replace('.', ','), out RealPart);
            double.TryParse(ImaginePartEdit.Text.Replace('.', ','), out ImaginePart);
            if(Mandelbrot.GetIterAtRealPoint(new Complex(RealPart,ImaginePart))>999UL)
            {
                if (MessageBox.Show(this, "Фрактал Жюлиа из введённого вами комплексного числа можеть быть вырожденным!\n"
                    + "Вы действительно хотите создать этот фрактал?", "Проблемное комплексное число!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
            }
            if(TopEdge>BottomEdge)
            {
                double swap = TopEdge;
                TopEdge = BottomEdge;
                BottomEdge = swap;
            }
            this.Dispose();
        }

        private void CancelReturn(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Dispose();
        }
        #endregion /Event handlers
        
        /*__________________________________________________________Выходные_данные________________________________________________________*/
        #region Result data
        public ulong IterationsCount;
        public double LeftEdge;
        public double RightEdge;
        public double TopEdge;
        public double BottomEdge;
        public double RealPart;
        public double ImaginePart;
        public Complex Complex
        {
            get { return new Complex(RealPart, ImaginePart); }
        }
        public Julia Julia
        {
            get { return new Julia(IterationsCount, LeftEdge, RightEdge, TopEdge, BottomEdge, new Complex(RealPart, ImaginePart)); }
        }
        #endregion /Result data

    }
}
