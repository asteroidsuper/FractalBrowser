using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    [Serializable]
    public class FractalTemplate
    {
        /*_________________________________________________________Конструкторы_______________________________________________________*/
        #region Constructors
        public FractalTemplate(Fractal Fractal,FractalColorMode FractalColorMode,string Name)
        {
            _name = Name;
            _fractal = Fractal.GetClone();
            _fcm = FractalColorMode.GetClone();
            _date_of_creating = DateTime.Now;
        }
        #endregion /Constructors

        /*__________________________________________________________Информация________________________________________________________*/
        #region Information
        private readonly DateTime _date_of_creating;
        private readonly string _name;
        public ulong index;
        #endregion Information

        /*____________________________________________________________Данные__________________________________________________________*/
        #region Data
        private readonly Fractal _fractal;
        private readonly FractalColorMode _fcm;
        #endregion /Data

        /*______________________________________________________Общедоступные_поля____________________________________________________*/
        #region Public fields
        public Fractal Fractal
        {
            get
            {
                return _fractal.GetClone();
            }
        }
        public FractalColorMode FractalColorMode
        {
            get
            {
                return _fcm.GetClone();
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public DateTime Date
        {
            get { return _date_of_creating; }
        }
        #endregion /Public fields
    }
}
