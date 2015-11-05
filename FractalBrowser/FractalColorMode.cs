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
        public abstract Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP);

        #endregion /Public abstract methods

        /*___________________________________________________Делегаты_и_события_и_их_активарторы___________________________________________________*/
        #region Public delegates and events
        /// <summary>
        /// Происходить когда пользователь изменил настройки цветового режима через элементы управления.
        /// </summary>
        /// <param name="sender">Инициатор события</param>
        public delegate void FractalColorModeChangedHandler(FractalColorMode sender);
        public event FractalColorModeChangedHandler FractalColorModeChanged;
        public void OnFractalColorModeChanged(FractalColorMode sender)
        {
            if (FractalColorModeChanged != null)FractalColorModeChanged(sender);
        }
        #endregion /Public delegates and events
    }
}
