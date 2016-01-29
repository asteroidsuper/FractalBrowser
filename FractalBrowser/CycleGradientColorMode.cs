using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
namespace FractalBrowser
{
    [Serializable]
    class CycleGradientColorMode:FractalColorMode
    {
        /*______________________________________________________________Конструкторы_класса___________________________________________________________________*/
        #region Constructors
        public CycleGradientColorMode()
        {
            _percents = new int[] { 0, 180, 360 };
            _color = new Color[] {Color.Black,Color.White,Color.Black};
            _gradient_void = Color.Silver;
            _gradient_iterations_count = 70;
            _gradient_void_iterations_count = 20;
            _using_mod = new VoidColorMode(this);
            _using_color_modes = WeAreColorReturnable.GetWith(_using_mod);
        }
        public CycleGradientColorMode(int[] Percents,Color[] Colors,Color ColorOfVoid,ulong GradientIterationsCount,ulong GradientVoidIterationsCount)
        {
            _percents = Percents;
            _color = Colors;
            _gradient_void = ColorOfVoid;
            _gradient_iterations_count = GradientIterationsCount;
            _gradient_void_iterations_count = GradientVoidIterationsCount;
            _using_mod = new VoidColorMode(this);
            _using_color_modes = WeAreColorReturnable.GetWith(_using_mod);
            
        }
        public CycleGradientColorMode(ulong GradientIterationsCount, ulong GradientVoidIterationsCount): this(new int[] {0,18,360},new Color[]{Color.Black,Color.White,Color.Black},Color.Silver,GradientIterationsCount,GradientVoidIterationsCount,new My2DClassicColorMode(2,2,2))
        {
            
        }
        public CycleGradientColorMode(int[] Percents,Color[] Colors,Color ColorOfVoid,ulong GradientIterationsCount,ulong GradientVoidIterationsCount,IColorReturnable UsingMod)
        {
            _percents = Percents;
            _color = Colors;
            _gradient_void = ColorOfVoid;
            _gradient_iterations_count = GradientIterationsCount;
            _gradient_void_iterations_count = GradientVoidIterationsCount;
            _using_color_modes = WeAreColorReturnable.GetWith(new VoidColorMode(this));
            _using_mod = UsingMod;
            for (int i = 0; i < _using_color_modes.Length - 1; i++)
            {
                
                if (_using_color_modes[i].GetType().Equals(UsingMod.GetType()))
                {
                    _using_color_modes[i] = UsingMod;
                }
                if (_using_color_modes[i] is FractalColorMode)
                    ((FractalColorMode)_using_color_modes[i]).FractalColorModeChanged += (fcm, control) => { this._fcm_on_FractalColorModeChangedHandler(); };
            }
            
        }
        #endregion /Constructors

        /*__________________________________________________________________Дата_класса_______________________________________________________________________*/
        #region Data of class
        private int[] _percents;
        private Color[] _color;
        private Color _gradient_void;
        private ulong _gradient_void_iterations_count;
        private ulong _gradient_iterations_count;
        private IColorReturnable[] _using_color_modes;
        private IColorReturnable _using_mod;
        private bool spec_mode;
        [NonSerialized]
        private object _optimizer;
        [NonSerialized]
        private Panel _unique_subinterface;
        [NonSerialized]
        private ColorGradientBar cgb;
        #endregion /Data of class

        /*______________________________________________________Реализация_абстрактных_методов_класса_________________________________________________________*/
        #region Realization of abstract methods of class
        public override System.Drawing.Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP, object Extra = null)
        {
            int width = FAP.Width, height = FAP.Height;
            Bitmap Result = new Bitmap(width, height);
            ulong iter, grad_void_iters = _gradient_void_iterations_count+_gradient_iterations_count;
            ulong[][] iter_matrix = FAP.Get2DOriginalIterationsMatrix();
            double[][] Radian_matrix = ((RadianMatrix)FAP.GetUniqueParameter(typeof(RadianMatrix))).Matrix;
            int deg;
            Color color;
            _optimizer = _using_mod.Optimize(FAP);
            for(int x=0;x<width;x++)
            {
                for(int y=0;y<height;y++)
                {
                    deg=((int)((Radian_matrix[x][y]/Math.PI)*180D)+360)%360;
                    iter=iter_matrix[x][y];
                    if (iter <= _gradient_iterations_count) color = cycle_get_gradient_color(_percents, _color, deg);
                    else if (iter <= grad_void_iters) color = _gradient_void;
                    else color = _using_mod.GetColor(_optimizer, x, y);
                    Result.SetPixel(x, y, color);
                }
            }
            return Result;
        }

        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            return FAP.Is2D && FAP.GetUniqueParameter(typeof(RadianMatrix)) != null;
        }

        public override System.Windows.Forms.Panel GetUniqueInterface(int width, int height)
        {
            _fcm_data_changed -= Processor;
            Panel Result = new Panel();
            Result.Size = new Size(width, height);
            TrackBar tr=(TrackBar)_add_standart_rgb_trackbar(Result, 0, 10000, (int)(_gradient_iterations_count % ((ulong)int.MaxValue)), Color.White,5, 1, 3);
            TrackBar trvoid = (TrackBar)_add_standart_rgb_trackbar(Result, 1, 10000, (int)(_gradient_void_iterations_count % (ulong)int.MaxValue), _gradient_void, 5,1, 3);
            _gradient_color_changed+=(Neo_color)=>{
                trvoid.BackColor = Neo_color;
            };
            cgb=_add_standart_color_gradient_bar(Result, 2);
            ComboBox cm= (ComboBox)_add_standart_combo_box(Result, 10, 0,_using_color_modes, null, 1, 3);
            _unique_subinterface = new Panel();
            _unique_subinterface.Location = new Point(1,cm.Location.Y+cm.Height+3);
            _unique_subinterface.Size = new Size(Result.Width - 1, Result.Height - _unique_subinterface.Location.Y);
            Result.Controls.Add(_unique_subinterface);
            if (_using_mod is FractalColorMode) _unique_subinterface.Controls.Add(((FractalColorMode)_using_mod).GetUniqueInterface(_unique_subinterface.Width-1,
                                                                                                                                    _unique_subinterface.Height-1));
            cm.SelectedIndex = get_index_of_using_modes();
            _fcm_data_changed += Processor;
            ToolTip tl = new ToolTip();
            tl.SetToolTip(tr, "Уровень итераций закрашиваемый циклическим градиентом");
            tl.SetToolTip(trvoid, "Уровень итераций после градиентового, который будет закрашиваться цветом пустоты");

            return Result;
        }

        public override FractalColorMode GetClone()
        {
            CycleGradientColorMode clone = new CycleGradientColorMode(this._percents, this._color, this._gradient_void,
                                                                    this._gradient_iterations_count, this._gradient_void_iterations_count, this._using_mod);
            clone._using_color_modes=this._using_color_modes;
            return clone;
        }
        #endregion /Realization of abstract methods of class

        /*_________________________________________________________Частные_делегаты_и_эвенты_класса___________________________________________________________*/
        #region Private delegates and events
        private delegate void _gradient_void_color_changed_handler(Color Neo_color);
        private event _gradient_void_color_changed_handler _gradient_color_changed;
        #endregion /Private delegates and events

        /*______________________________________________________________Частные_утилиты_класса________________________________________________________________*/
        #region Private utilities of class
        [Serializable]
        private class VoidColorMode:IColorReturnable
        {
            public VoidColorMode(CycleGradientColorMode mode)
            {
                _mode = mode;
            }
            private CycleGradientColorMode _mode;
            Color IColorReturnable.GetColor(object optimizer, int X, int Y)
            {
                return _mode._gradient_void;
            }
            object IColorReturnable.Optimize(FractalAssociationParametrs FAP, object Extra)
            {
                return null;
            }
            public override string ToString()
            {
                return "Void color";
            }
        }
        private void Processor(object Value,int ui, Control sender)
        {
            switch(ui)
            {
                case 0: {
                    _gradient_iterations_count = (ulong)((int)Value);
                    break;
                }
                case 1:
                    {
                        _gradient_void_iterations_count = (ulong)((int)Value);
                        break;
                    }
                case 2:
                    {
                        ColorGradientEventArgs ee = (ColorGradientEventArgs)Value;
                        _percents = ee.Positions.Select(arg => (int)(arg * 360)).ToArray();
                        _color = ee.Colors;
                        if (spec_mode)
                        {
                            int swap = _percents[1];
                            _percents[1] = _percents.Last();
                            _percents[_percents.Length - 1] = swap;
                            Color sw = _color[1];
                            _color[1] = _color.Last();
                            _color[_color.Length - 1] = sw;
                        }
                        //_percents[1] = 360;
                        break;
                    }
                case 10:
                    {
                        _using_mod = (IColorReturnable)(_using_color_modes[(int)Value]);
                        _unique_subinterface.Controls.Clear();
                        if(_using_mod is FractalColorMode)_unique_subinterface.Controls.Add(((FractalColorMode)_using_mod).GetUniqueInterface(_unique_subinterface.Width,_unique_subinterface.Height));
                        break;
                    }


            }
            _fcm_on_FractalColorModeChangedHandler();
        }
        private int get_index_of_using_modes()
        {
            for(int index=0;index<_using_color_modes.Length;index++)
            {
                if (_using_color_modes[index].GetType().Equals(_using_mod.GetType())) return index;
            }
            return -1;
        }
        #endregion /Private utilities of class

        /*____________________________________________________________Общедоступные_поля_класса______________________________________________________________*/
        #region Public fields
        public Color GradientVoidColor
        {
            get { return _gradient_void; }
            set
            {
                if(value!=null&&value!=_gradient_void)
                {
                    _gradient_void = value;
                    if (_gradient_color_changed != null) _gradient_color_changed(value);
                }
            }
        }
        #endregion /Public fields
    }
}
