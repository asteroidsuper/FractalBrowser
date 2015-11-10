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
    }
}
