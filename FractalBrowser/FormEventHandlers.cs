using System;
using System.Windows.Forms;

namespace FractalBrowser
{
    public static class FormEventHandlers
    {
        public static readonly KeyPressEventHandler OnlyPositiveNumber = (sender, e) => {
            if (e.KeyChar < '0' || e.KeyChar > '9') e.Handled = true;
            if (e.KeyChar == (char)Keys.Back) e.Handled = false;
        };
        public static KeyPressEventHandler OnlyNumeric = (sender, e) => {
            string text = ((Control)sender).Text;
            char key = e.KeyChar;
            int selectindex=((TextBox)sender).SelectionStart;
            switch(key)
            {
                case (char)Keys.Back: {return; }
                case '.': {e.Handled=(text.IndexOf(key)>=0)||(text.IndexOf(',')>=0)||(selectindex==0&&text.IndexOf('-')>=0); return; }
                case ',': { e.Handled = (text.IndexOf(key) >= 0) || (text.IndexOf('.') >= 0) || (selectindex == 0 && text.IndexOf('-') >= 0); return; }
                case '-': { e.Handled = (text.IndexOf('-') == 0) || (selectindex > 0); return; }
                default: { e.Handled = (key < '0') || (key > '9'); return; }
            }
        };
    }
}
