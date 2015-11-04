using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows;

namespace FractalBrowser
{
    public class FractalPictureBox : PictureBox
    {

        /*_____________________________________________________Конструкторы_класса___________________________________________________________*/
        #region Public constructors
        public FractalPictureBox()
        {
            _set_2d_mode();
        }

        #endregion /Public constructors

        /*_________________________________________________Общедоступные_атрибуты_класса_____________________________________________________*/
        #region Public atribytes
        /// <summary>
        /// Перо для отрисовка зоны выделения.
        /// </summary>
        public Pen SelectionPen = Pens.Black;


        #endregion /Public atribytes

        /*____________________________________________________Частные_утилиты_класса_________________________________________________________*/
        #region Private utilities
        private void _set_2d_mode()
        {
            Paint += (o, e) =>
            {
                _draw_selection_rect(e.Graphics, _first_mouse_point, _second_mouse_point);
            };
            MouseDown += _onmousedown_worker;
            MouseUp += _onmouseup_worker;
            MouseMove += (o, e) => { if (_is_mouse_into)_second_mouse_point = e.Location; Invalidate(); };
            MouseLeave += (o, e) => { _is_mouse_into = false; };
            MouseEnter += (o, e) => { _is_mouse_into = true; };
        }
        private void _unset_2d_mode()
        {
            Paint -= (o, e) =>
            {
                _draw_selection_rect(e.Graphics, _first_mouse_point, _second_mouse_point);
            };
            MouseDown -= _onmousedown_worker;
            MouseUp -= _onmouseup_worker;
            MouseMove -= (o, e) => { _second_mouse_point = e.Location; Invalidate(); };
            MouseLeave -= (o, e) => { _is_mouse_into = false; };
            MouseEnter -= (o, e) => { _is_mouse_into = true; };
        }
        private void _draw_selection_rect(Graphics g, Point mouse_pos_a, Point mouse_pos_b)
        {
            if (!_is_pressed_mouse_left_button) return;
            g.DrawRectangle(SelectionPen, mouse_pos_a.X < mouse_pos_b.X ? mouse_pos_a.X : mouse_pos_b.X,
                mouse_pos_a.Y < mouse_pos_b.Y ? mouse_pos_a.Y : mouse_pos_b.Y, Math.Abs(mouse_pos_a.X - mouse_pos_b.X), Math.Abs(mouse_pos_a.Y - mouse_pos_b.Y));
        }
        private void _onmousedown_worker(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _first_mouse_point = e.Location;
                _is_pressed_mouse_left_button = true;
            }
            else if (e.Button == MouseButtons.Right) _is_pressed_mouse_left_button = false;
        }
        private Rectangle _get_selected_rectangle(Point p1, Point p2)
        {
            return new Rectangle(p1.X < p2.X ? p1.X : p2.X, p1.Y < p2.Y ? p1.Y : p2.Y, Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }
        private void _onmouseup_worker(object sender, MouseEventArgs e)
        {
            if (_is_pressed_mouse_left_button & _fpbm == FractalPictureBoxMode._2DViewer)
            {
                if (RectangleSelected != null)
                {
                    RectangleSelected(this, _get_selected_rectangle(_first_mouse_point, e.Location));
                }
            }
            _is_pressed_mouse_left_button = false;
        }
        #endregion /Private utilities

        /*___________________________________________________Частные_атрибуты_класса_________________________________________________________*/
        #region Private atribytes
        private Point _first_mouse_point;
        private Point _second_mouse_point;
        private bool _is_mouse_into;
        private bool _is_pressed_mouse_left_button;
        FractalPictureBoxMode _fpbm;
        #endregion /private atribytes

        /*__________________________________________________Делегаты_и_исобытия_класса_______________________________________________________*/
        #region Delegates and events

        public delegate void RectangleSelectedHandler(object sender, Rectangle SelectedRect);
        public event RectangleSelectedHandler RectangleSelected;
        #endregion /Delegates and events
    }
    public enum FractalPictureBoxMode
    {
        _2DViewer,
        _3DViewer
    }
}