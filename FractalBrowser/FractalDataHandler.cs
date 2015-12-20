using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;
using System.Threading;
namespace FractalBrowser
{
    public class FractalDataHandler
    {
        /*__________________________________________________________Конструкторы_класса_________________________________________________________________*/
        #region Constructors
        public FractalDataHandler(Control Owner,Fractal Fractal,FractalPictureBox FPB,FractalColorMode FCM,Size Size,FractalAssociationParametrs FAP=null)
        {
            if (Owner==null||Fractal == null || FPB == null || FCM == null) throw new ArgumentNullException("Нельзя передавать пустые значения!");
            if (Size.Width < 1 || Size.Height < 1) throw new ArgumentException("Ширина и Высота не могут быть меньше единицы!");
            _fractal = Fractal;
            _fpb = FPB;
            _fcm = FCM;
            _width = Size.Width;
            _height = Size.Height;
            _fap = FAP;
            _owner=Owner;
            Connect();
        }

        public FractalDataHandler(Control Owner, Fractal Fractal, FractalPictureBox FPB, FractalColorMode FCM, Size Size,FractalDataHandlerControler Deactivator, FractalAssociationParametrs FAP = null)
        {
            if (Owner == null || Fractal == null || FPB == null || FCM == null) throw new ArgumentNullException("Нельзя передавать пустые значения!");
            if (Size.Width < 1 || Size.Height < 1) throw new ArgumentException("Ширина и Высота не могут быть меньше единицы!");
            _fractal = Fractal;
            _fpb = FPB;
            _fcm = FCM;
            _width = Size.Width;
            _height = Size.Height;
            _fap = FAP;
            _owner = Owner;
            Connect();
            ConnectToControler(Deactivator);
        }

        #endregion /Constructors

        /*__________________________________________________________Общедостуные_методы_________________________________________________________________*/
        #region Public methods
        public void ConnectProgressProcessTo(Fractal._f_progress_changed_delegate ProgressChangedHandler)
        {
            _fractal.ProgressChanged += ProgressChangedHandler;
            
        }
        public void ConnectToolStripProgressBar(ToolStripProgressBar arg)
        {
            if (arg == null) throw new ArgumentNullException("Нельзя посылать null!");
            _tool_progress_bar = arg;
            Action<int> ActIncremetion = arg.Increment;
            _fractal.ProgressChanged += (f, inc) =>{_owner.Invoke(ActIncremetion,(inc - arg.Value));};
        }
        public void Show()
        {
            if (NeedDeactivate != null) NeedDeactivate();
            _isactive = true;
            if (MaxGlobalPercent > 0) _fractal.MaxPercent = MaxGlobalPercent;
            if (FractalShowing != null && _fap != null) FractalShowing(this);
            if (_fap == null) _fractal.CreateParallelFractal(_width, _height);    
            else ThreadPool.QueueUserWorkItem((parallel) => { _owner.Invoke(SetNewBitmap, _fcm.GetDrawnBitmap(_fap), _fpb); if(FractalShowed!=null)_owner.Invoke(FractalShowed, this); });
        }
        public void Show(int Width,int Height)
        {
            if (NeedDeactivate != null) NeedDeactivate();
            _isactive = true;
            if (MaxGlobalPercent > 0) _fractal.MaxPercent = MaxGlobalPercent;
            _width = Width;
            _height = Height;
            _fractal.CreateParallelFractal(Width, Height, 0, 0, Width, Height,UseSafeZoom);
        }
        public void ReShow(int Width, int Height)
        {
            if (NeedDeactivate != null) NeedDeactivate();
            _isactive = true;
            if (MaxGlobalPercent > 0) _fractal.MaxPercent = MaxGlobalPercent;            
            _fractal.CreateParallelFractal(Width, Height, 0, 0, _width, _height, UseSafeZoom);
            _width = Width;
            _height = Height;
        }
        public void ReShow(int Width, int Height,int HorizontalStart,int VerticalStart,int selectedwidth,int selectedheight)
        {
            if (NeedDeactivate != null) NeedDeactivate();
            _isactive = true;
            if (MaxGlobalPercent > 0) _fractal.MaxPercent = MaxGlobalPercent;
            _width = Width;
            _height = Height;
            _fractal.CreateParallelFractal(Width, Height, HorizontalStart, VerticalStart, selectedwidth, selectedheight);
            
        }
        public void ConnectToControler(FractalDataHandlerControler Controler)
        {
            Controler.Deactivate += () => { this._isactive = false;
            _fractal.CancelParallelCreating();
            };
            Controler.SetZoomEvent += (Degree) => {if(_isactive)GetZoom(Degree);};
            Controler.GetFractalDataHandlersHasStarted += (Handler, ActiveOnly) => {
                if (ActiveOnly && (!IsActive)) return;
                Handler.Add(this);
            };
            Controler.GetBack += () => {
                if (!_isactive) return;
                Controler.DeactivateHandlers();
                if(this._fractal.GetBack())
                this.Show(_width, _height);
            };
            Controler.ChangeSizeFromSizeEditor += () => { if(_isactive)ChangeSizeFromWindow(Controler); };
            this.NeedDeactivate += () => { Controler.DeactivateHandlers(); };
        }
        public void Reset(int Width,int Height)
        {
            _fap = null;
            _width = Width;
            _height = Height;
            Show();
        }
        public void ConnectShowToMenuItem(ToolStripMenuItem menuitem,FractalDataHandlerControler Deactivator)
        {
            menuitem.Click+=(sender,EventArg)=>{/*Deactivator.DeactivateHandlers();*/ this.Show();};
        }
        public void ConnectShowToMenuItem(ToolStripMenuItem menuitem,FractalDataHandlerControler Deactivator,int Width,int Height)
        {
            _owner.Invoke(SetMenuItemImage, menuitem, _fcm.GetDrawnBitmap(_fractal.GetClone().CreateFractal(Width, Height)));
            menuitem.Click += (sender, EventArg) => { Deactivator.DeactivateHandlers(); this.Show(); };
            this.FractalChanged += (sender, fractal) => { _owner.Invoke(SetMenuItemImage, menuitem,_fcm.GetDrawnBitmap(fractal.GetClone().CreateFractal(Width, Height))); };
            this.FractalColorModeChanged += (sender, mode) => { _owner.Invoke(SetMenuItemImage, menuitem, _fcm.GetDrawnBitmap(_fractal.GetClone().CreateFractal(Width, Height))); };
            this.SetNewFractalEvent += (sender) => { _owner.Invoke(SetMenuItemImage, menuitem, _fcm.GetDrawnBitmap(_fractal.GetClone().CreateFractal(Width, Height))); };
        }
        public void ConnectStandartResetToMenuItem(ToolStripMenuItem menuitem, FractalDataHandlerControler Controler)
        {
            menuitem.Click += (sender, e) => { Controler.DeactivateHandlers();
            this.Reset(960,640);
            };
        }
        public void ConnectResetWithSizeFromWindowToMenuItem(ToolStripMenuItem menuitem, FractalDataHandlerControler Controler)
        {
            menuitem.Click+=(sender,e)=>{
            SizeEditor se = new SizeEditor("Новый фрактал");
            se.BuldButtonClick += (o, size) => { Controler.DeactivateHandlers(); this.Reset(size.Width,size.Height); };
            se.OtherWindowButtonClick += (o, size) => { this.Create_in_other_window(_fractal.GetClone(), size.Width, size.Height, Controler); };
            se.ShowDialog(_owner);
        };
        }
        public void ChangeSizeFromWindow(FractalDataHandlerControler controler)
        {
            SizeEditor se = new SizeEditor("Новый размер");
            se.BuldButtonClick += (sender, size) => { controler.DeactivateHandlers(); this.ReShow(size.Width,size.Height); };
            se.OtherWindowButtonClick += (sender, size) => {this.Create_in_other_window(_fractal.GetClone(),size.Width,size.Height,0,0,_width,_height,controler); };
            se.ShowDialog(_owner);
        }
        public void ConntectToStatusLabel(ToolStripStatusLabel StatusLabel)
        {
            this.FractalShowing += (fdh) => { StatusLabel.Text = "Идёт визуализация фрактала!"; };
            this.FractalShowed += (fdh) => { StatusLabel.Text = "Фрактал успешно визуализирован!"; };
            this.FractalRenderingFailed += (fdh) => { StatusLabel.Text = "Визуализация фрактала была прервана!"; };
        }
        public void GetZoom(double Degree)
        {
            if (FractalShowing != null) FractalShowing(this);
            _isactive = true;
            int n_width=(int)(_width/Degree),shift_to_right=(_width>>1)-(n_width>>1);
            int n_height = (int)(_height / Degree), shift_to_down = (_height>> 1) - (n_height>>1) ;
            _fractal.CreateParallelFractal(_width, _height, shift_to_right, shift_to_down, n_width, n_height);
        }
        public void ChangeColorMode()
        {
            VisualColorControler vcc = new VisualColorControler(_fractal.GetClone(), _fcm.GetClone(), _width, _height, _fap);
            if(vcc.ShowDialog(_owner)==DialogResult.Yes)
            {
                _fcm = vcc.FractalColorMode.GetClone();
                if (FractalColorModeChanged != null) FractalColorModeChanged(this, _fcm);
                this.Show();
            }
        }
        public void CreateInSize(FractalDataHandlerControler Controler)
        {
            SizeEditor se = new SizeEditor("Фрактал джулии");
            se.BuldButtonClick += (o, size) => { Controler.DeactivateHandlers(); this.Reset(size.Width, size.Height); };
            se.OtherWindowButtonClick += (o, size) => { this.Create_in_other_window(_fractal.GetClone(), size.Width, size.Height, Controler); };
            se.ShowDialog(_owner);
        }
        public void SetNewFractal(Fractal Fractal,FractalColorMode FractalColorMode)
        {
            if (Fractal == null || FractalColorMode == null) throw new ArgumentNullException();
            if(!FractalColorMode.IsCompatible(Fractal.GetClone().CreateFractal(8,8)))throw new ArgumentException();
            Disconnect();
            _fractal = Fractal.GetClone();
            Connect();
            _fcm = FractalColorMode.GetClone();
            if (SetNewFractalEvent != null) SetNewFractalEvent(this);
        }
        #endregion /Public methods

        /*_________________________________________________________Частные_данные_класса________________________________________________________________*/
        #region Private data of class
        private Fractal _fractal;
        private FractalPictureBox _fpb;
        private FractalColorMode _fcm;
        int _width, _height;
        private FractalAssociationParametrs _fap;
        private Control _owner;
        private bool _isactive;
        private ToolStripProgressBar _tool_progress_bar;
        #endregion /Private data of class

        /*________________________________________________________Частные_утилиты_класса________________________________________________________________*/
        #region Private utilities of class
        private void Connect()
        {
            _fpb.RectangleSelected+=RectangleSelectedHandler;
            _fractal.ParallelFractalCreatingFinished += FractalCreatingFinishedHandler;
            if (_tool_progress_bar != null) {
                Action<int> ActIncremetion = _tool_progress_bar.Increment;
                _fractal.ProgressChanged += (f, inc) => { _owner.Invoke(ActIncremetion, (inc - _tool_progress_bar.Value)); };
            };
        }
        private void Disconnect()
        {
            _fpb.RectangleSelected -= RectangleSelectedHandler;
            _fractal.ParallelFractalCreatingFinished -= FractalCreatingFinishedHandler;
            if (_tool_progress_bar != null)
            {
                Action<int> ActIncremetion = _tool_progress_bar.Increment;
                _fractal.ProgressChanged -= (f, inc) => { _owner.Invoke(ActIncremetion, (inc - _tool_progress_bar.Value)); };
            };
        }

        private void Create_in_other_window(Fractal Fractal,int Width,int Height,FractalDataHandlerControler Controler)
        {
            IsolatedFractalWindowsCreator OtherWindow = new IsolatedFractalWindowsCreator(Fractal,_fcm);
            OtherWindow.FractalToken+=(fractal, fap) =>
            {
                Disconnect();
                _fractal = fractal;
                Connect();
                _fap = fap;
                _width = fap.Width;
                _height = fap.Height;
                Controler.DeactivateHandlers();
                Show();
            };
            OtherWindow.StartProcess(Width,Height);
        }
        private void Create_in_other_window(Fractal Fractal, int Width, int Height, int HorizontalStart, int VerticalStart, int SelectedWidth, int SelectedHeight, FractalDataHandlerControler Controler)
        {
            IsolatedFractalWindowsCreator OtherWindow = new IsolatedFractalWindowsCreator(Fractal,_fcm);
            OtherWindow.FractalToken += (fractal, fap) =>
            {
                Disconnect();
                _fractal = fractal;
                Connect();
                _fap = fap;
                _width = fap.Width;
                _height = fap.Height;
                Controler.DeactivateHandlers();
                Show();
            };
            OtherWindow.StartProcess(Width, Height,HorizontalStart,VerticalStart,SelectedWidth,SelectedHeight,UseSafeZoom);
        }
        #endregion /Private utilities of class

        /*______________________________________________________Методы_для_обработки_событий____________________________________________________________*/
        #region Private Event processing methods
        private void RectangleSelectedHandler(object FPB,Rectangle Rec)
        {
            if (!_isactive) return;
            if (Showing) { MessageBox.Show("В данный момент визуализируеться фрактал, пожалуйста подождите!", "Идет визуализация"); return; }
            if (MaxGlobalPercent > 0) _fractal.MaxPercent = MaxGlobalPercent;
            _fractal.CancelParallelCreating();
            _fractal.CreateParallelFractal(_width,_height,Rec.X,Rec.Y,Rec.Width,Rec.Height,UseSafeZoom);
        }
        private void FractalCreatingFinishedHandler(Fractal f,FractalAssociationParametrs fap)
        {
            ThreadPool.QueueUserWorkItem((Parallel) =>
            {
                if (_fap != null) if ((ulong)_fap.Width * (ulong)_fap.Height * 4UL > int.MaxValue)
                    {
                        MessageBox.Show("Размер данного фрактала слишком велик, чтобы быть преобразованным в изображение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                _fap = fap;
                try
                {
                    if (FractalShowing != null) _owner.Invoke(FractalShowing,this);
                    Showing = true;
                    Bitmap bmp = _fcm.GetDrawnBitmap(fap);
                    if(_isactive)_owner.Invoke(SetNewBitmap, bmp, _fpb);
                    Showing = false;
                    if (FractalShowed != null)_owner.Invoke(FractalShowed,this);
                }
                catch
                {
                    Showing = false;
                    MessageBox.Show("При преобразовании фрактала в изображение произошла ошибка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (FractalRenderingFailed != null) _owner.Invoke(FractalRenderingFailed,this);
                    //if ((ulong)_fap.Width * (ulong)_fap.Height * 4UL > int.MaxValue) MessageBox.Show("Размер данного фрактала слишком велик, чтобы быть преобразованным в изображение!","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                Showing = false;
                GC.Collect();
            });
        }



        Action<Bitmap, FractalPictureBox> SetNewBitmap = (bitmap, fpb) => { fpb.Image = bitmap;};
        Action<ToolStripMenuItem, Bitmap> SetMenuItemImage = (MenuItem, bitmap) => { MenuItem.Image = bitmap; };
        #endregion /Private Event processing methods

        /*________________________________________________________Делегаты_и_эвенты_класса______________________________________________________________*/
        #region Delegates and events
        public delegate void FractalShowingHandler(FractalDataHandler FDH);
        public event FractalShowingHandler FractalShowing;
        public delegate void FractalShowedHandler(FractalDataHandler FDH);
        public event FractalShowedHandler FractalShowed;
        public delegate void FractalColorModeChangedHandler(FractalDataHandler FDH, FractalColorMode NewMode);
        public event FractalColorModeChangedHandler FractalColorModeChanged;
        public delegate void FractalRenderingFailedHandler(FractalDataHandler Handler);
        public event FractalRenderingFailedHandler FractalRenderingFailed;
        public delegate void NeedDeactivateHandler();
        public event NeedDeactivateHandler NeedDeactivate;
        public delegate void FractalChangedHandler(FractalDataHandler sender,Fractal fractal);
        public event FractalChangedHandler FractalChanged;
        public delegate void SetNewFractalHandler(FractalDataHandler FDH);
        public event SetNewFractalHandler SetNewFractalEvent;
        #endregion /Delegates and events

        /*_________________________________________________________Статические_данные___________________________________________________________________*/
        #region Public static data
        public static int MaxGlobalPercent=-1;
        public static bool UseSafeZoom;
        public static bool Showing;
        #endregion /Public static data

        /*_____________________________________________________Общедоступные_свойства_класса____________________________________________________________*/
        #region Public properties of class
        public Fractal Fractal
        {
            get { return _fractal; }
            set { if (value == null)throw new ArgumentNullException();
            Disconnect();
            _fractal = value;
            Connect();
            if (FractalChanged != null) FractalChanged(this, value);
            }
        }
        public string ZoomStatus
        {
            get
            {
                if (_fractal is _2DFractal)
                {
                    _2DFractal f = (_2DFractal)_fractal;
                    System.Numerics.BigInteger hzoom = f.ImagineWidth / (System.Numerics.BigInteger)_width, vzoom=f.ImagineHeight/(System.Numerics.BigInteger)_height;
                    return (!UseSafeZoom? "Horizontal zoom: " + hzoom+" Vertical zoom: "+vzoom+" Common ":"")+ "zoom: "+((hzoom+vzoom)>>1);
                }

                return "Cannon get a zoom status!";
            }
        }

        public FractalColorMode FractalColorMode
        {
            get { return _fcm; }
            set { if (value == null)throw new ArgumentNullException();
            _fcm = value;
            if (FractalColorModeChanged != null) FractalColorModeChanged(this, value);
            }
        }
        public bool IsActive
        {
            get { return _isactive; }
        }
        public FractalAssociationParametrs FractalAssociationParameters
        {
            get { return _fap; }
        }
        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }
        #endregion /Public properties of class
    }
}
