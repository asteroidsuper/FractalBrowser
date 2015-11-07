using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public class FractalDataHandlerDeactivator
    {
        /*___________________________________________________________Конструкторы_класса______________________________________________________________*/
        #region Constructors


        #endregion /Constructors

        /*________________________________________________________Делегаты_и_эвенты_класса____________________________________________________________*/
        #region Delegates and events
        public delegate void DeactivateHandler();
        public event DeactivateHandler Deactivate;

        #endregion /Delegates and events

        /*__________________________________________________________Общедоступные_методы______________________________________________________________*/
        #region Public methods
        public void DeactivateHandlers()
        {
            if (Deactivate != null) Deactivate();
        }

        #endregion /Public methods
    }
}
