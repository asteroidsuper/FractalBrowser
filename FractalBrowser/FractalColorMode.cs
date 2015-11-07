using System;
using System.Drawing;
namespace FractalBrowser
{
    /// <summary>
    /// Класс, который отвечает за отрисовку фрактала.
    /// </summary>
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
    }
}
