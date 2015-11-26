using System;
using System.Collections.Generic;

namespace FractalBrowser
{
    public class FractalDataHandlerControler
    {
        /*___________________________________________________________Конструкторы_класса______________________________________________________________*/
        #region Constructors


        #endregion /Constructors

        /*________________________________________________________Делегаты_и_эвенты_класса____________________________________________________________*/
        #region Delegates and events
        public delegate void DeactivateHandler();
        public event DeactivateHandler Deactivate;
        public delegate void SetZoomHandler(double Degree);
        public event SetZoomHandler SetZoomEvent;
        public delegate void GetFractalDataHandlersHasStartedHandler(List<FractalDataHandler> Handler, bool ActiveOnly);
        public event GetFractalDataHandlersHasStartedHandler GetFractalDataHandlersHasStarted;
        public delegate void GetBackHandler();
        public event GetBackHandler GetBack;
        public delegate void ChangeSizeFromSizeEditorHandler();
        public event ChangeSizeFromSizeEditorHandler ChangeSizeFromSizeEditor;
        #endregion /Delegates and events

        /*__________________________________________________________Общедоступные_методы______________________________________________________________*/
        #region Public methods
        public void DeactivateHandlers()
        {
            if (Deactivate != null) Deactivate();
        }
        public void SetZoom(double Degree)
        {
            if(SetZoomEvent!=null)SetZoomEvent(Degree);
        }
        public FractalDataHandler[] GetFractalDataHandlers(bool ActiveOnly=false)
        {
            List<FractalDataHandler> Handler = new List<FractalDataHandler>();
            if (GetFractalDataHandlersHasStarted != null) GetFractalDataHandlersHasStarted(Handler, ActiveOnly);
            return Handler.ToArray();
        }
        public void FractalGetBack()
        {
            if (GetBack != null) GetBack();
        }
        public void ChangeSize()
        {
            if (ChangeSizeFromSizeEditor != null) ChangeSizeFromSizeEditor();
        }
        #endregion /Public methods
    }
}
