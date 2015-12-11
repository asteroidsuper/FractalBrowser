using System;
using System.Drawing;
using System.Windows.Forms;
namespace FractalBrowser
{
    /// <summary>
    /// Класс, который отвечает за отрисовку фрактала.
    /// </summary>
    [Serializable]
    public abstract class FractalColorMode
    {
        /*_________________________________________________Общедостуные_абстрактные_методы_класса__________________________________________________*/
        #region Public abstract methods
        /// <summary>
        /// Возвращает экземпляр класса Bitmap, в котором был визуализирован фрактал.
        /// </summary>
        /// <param name="FAP">FractalAssociationParameters содержащий фрактал, который нужно визуализировать.</param>
        /// <returns>Экземпляр класса Bitmap, в котором был визуализирован фрактал.</returns>
        public abstract Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP,object Extra=null);
        
        public abstract bool IsCompatible(FractalAssociationParametrs FAP);
        public abstract System.Windows.Forms.Panel GetUniqueInterface(int width,int height);
        public abstract FractalColorMode GetClone();
        #endregion /Public abstract methods

        /*___________________________________________________Делегаты_и_события_и_их_активарторы___________________________________________________*/
        #region Public delegates and events
        /// <summary>
        /// Происходить когда пользователь изменил настройки цветового режима через элементы управления.
        /// </summary>
        /// <param name="sender">Инициатор события</param>
        /// <param name="Control">Control инициировавший событие.</param>
        public delegate void FractalColorModeChangedHandler(FractalColorMode sender,System.Windows.Forms.Control Control);
        public event FractalColorModeChangedHandler FractalColorModeChanged;
        public void OnFractalColorModeChanged(FractalColorMode sender,System.Windows.Forms.Control Control)
        {
            if (FractalColorModeChanged != null)FractalColorModeChanged(sender,Control);
        }
        #endregion /Public delegates and events

        /*______________________________________________________Защищенные_делегаты_и_события______________________________________________________*/
        #region Protected delegates and events
        protected delegate void _fcm_data_changed_handler(object Value,int ui,Control sender);
        protected event _fcm_data_changed_handler _fcm_data_changed;
        #endregion /Protected delegates and events

        /*________________________________________________________Защищённые_методы_класса_________________________________________________________*/
        #region Protected methods of class
        protected void _fcm_on_fcm_data_changed(object Value,int ui,Control sender=null)
        {
            if (_fcm_data_changed != null) _fcm_data_changed(Value,ui,sender);
        }
        protected void _fcm_on_FractalColorModeChangedHandler()
        {
            if (FractalColorModeChanged != null) FractalColorModeChanged(this, null);
        }
        protected virtual  Control _add_standart_rgb_trackbar(System.Windows.Forms.Panel Panel,int ui,int Maximum,int BaseValue,Color color,int HorizontalShift=1,int VerticalShift=5)
        {
            TrackBar trackbar = new TrackBar();
            trackbar.Maximum = Maximum;
            trackbar.BackColor = color;
            trackbar.Value = BaseValue;
            trackbar.ValueChanged += (sender, e) =>
            {
                _fcm_on_fcm_data_changed(trackbar.Value, ui,trackbar);
            };
            if(Panel.Controls.Count>0)
            {
                Control lcontrol = Panel.Controls[Panel.Controls.Count - 1];
                trackbar.Location = new Point(HorizontalShift, lcontrol.Location.Y + lcontrol.Height + VerticalShift);
            }
            trackbar.Size = new Size(Panel.Width-HorizontalShift-15,trackbar.Height);
            Panel.Controls.Add(trackbar);
            return trackbar;
        }
        protected virtual Control _add_standart_combo_box(Panel Panel,int ui,int SelectedIndex,object[] Items,Font font,int HorizontalShift=1,int VerticalShift=5)
        {
            ComboBox Result = new ComboBox();
            if(font==null)font=new Font(FontFamily.GenericSansSerif,12.25F);
            Result.Size = new Size(Panel.Width - HorizontalShift*2-15, Result.Height);
            Result.Font = font;
            Result.Items.AddRange(Items);
            Result.SelectedIndex = SelectedIndex;
            Result.SelectedValueChanged += (sender, e) => { _fcm_on_fcm_data_changed(Result.SelectedIndex, ui, Result); };
            Control lcontrol =Panel.Controls.Count>0? Panel.Controls[Panel.Controls.Count - 1]:null;
            Point lloc=lcontrol!=null? lcontrol.Location:new Point(HorizontalShift,VerticalShift);
            if(lcontrol!=null)lloc=new Point(HorizontalShift,lloc.Y+VerticalShift+lcontrol.Height);
            Result.Location = lloc;
            Panel.Controls.Add(Result);
            Result.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Result.Show();
            return Result;
        }
        #endregion /Protected methods of class

        /*___________________________________________________Защищённые_подклассы_и_структуры______________________________________________________*/
        #region Protected subclasses and substructs
        

        #endregion /Protected subclasses and substructs

        /*_____________________________________________________Защищённые_инструменты_класса_______________________________________________________*/
        #region Protected utilities
        protected virtual double GetNormalize(double arg)
        {
            long suber = (long)arg;
            if (suber > 0) suber--;
            return arg - (double)suber;
        }
        protected virtual double GetDenormalize(double arg)
        {
            long result = (long)arg;
            return arg == 1 ? --result : result;
        }
        protected virtual int get_cos_color(double ratio, int rgb, double scale)
        {
            double offset = Math.PI * (rgb / 255D);
            double result = Math.Cos((Math.PI * (rgb / 255D) + Math.PI + scale * ratio * 2D * Math.PI));
            return (int)(127.5 * (1D + (result)));
            //return (int)(rgb*Math.Pow(Math.Cos(Math.PI/2-Math.Abs(Math.Tan(ratio*2-1))),2));
        }
        protected virtual Color cycle_get_gradient_color(int[] percents,Color[] colors,int percent)
        {
            int max_percent = percents[percents.Length - 1];
            percent %= max_percent+1;
            int g_pos=0;
            for (; percents[g_pos] < percent; g_pos++) ;
            if (percent == percents[g_pos]) return colors[g_pos];
            g_pos--;
            int red_dif = colors[g_pos + 1].R - colors[g_pos].R;
            int green_dif = colors[g_pos + 1].G - colors[g_pos].G;
            int blue_dif = colors[g_pos + 1].B- colors[g_pos].B;
            int percent_dif = (percents[g_pos + 1] - percents[g_pos]),percent_grad=percent-percents[g_pos];
            return Color.FromArgb(colors[g_pos].R + (red_dif * percent_grad) / percent_dif, colors[g_pos].G + (green_dif * percent_grad) / percent_dif, colors[g_pos].B+(blue_dif * percent_grad) / percent_dif);

        }
        #endregion /Protected utilities

        /*________________________________________________________Перегруженные_методы_____________________________________________________________*/
        #region Overrided methods
        public override bool Equals(object obj)
        {
            return GetType().Equals(obj.GetType());
        }

        #endregion /Overrided methods

    }
}
