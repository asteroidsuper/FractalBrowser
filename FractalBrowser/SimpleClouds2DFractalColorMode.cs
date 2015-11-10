using System;
using System.Drawing;

namespace FractalBrowser
{
    public class SimpleClouds2DFractalColorMode:FractalColorMode
    {
        /*______________________________________________________________Конструкторы_класса_____________________________________________________________*/
        #region Constructors
        public SimpleClouds2DFractalColorMode(Color[] ColorArray=null)
        {
            _color_array = ColorArray;
        }
        public SimpleClouds2DFractalColorMode(int ColorCount)
        {
            CreateRandColorArray(ColorCount);
        }
        #endregion /Constructors

        /*_____________________________________________________________Частные_данные_класса____________________________________________________________*/
        #region Private data of class
        private Color[] _color_array;
        #endregion /Private data of class

        /*_________________________________________________________Реализация_асбтрактных_методов_______________________________________________________*/
        #region Realization of abstract methods
        public override System.Drawing.Bitmap GetDrawnBitmap(FractalAssociationParametrs FAP,object Extra=null)
        {
            if (FAP == null) throw new ArgumentNullException("FAP не содержить значения!");
            if (!IsCompatible(FAP)) throw new ArgumentException("Переданный FractalAssociationParameters не совместим с данной цветовой моделью, используйте другую цветовую модель!");
            if (_color_array == null) throw new InvalidOperationException("Требуеться задать массив цветов (используйте CreateRandColorArray).");
            if (_color_array.Length < 1) throw new InvalidOperationException("Нельзя использовать пустой массив цветов (используйте CreateRandColorArray).");
            int width = FAP.Width, height = FAP.Height;
            Bitmap Result = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(Result);
            g.FillRectangle(Brushes.Black, 0, 0, width, height);
            g.Dispose();
            FractalCloudPoint[][][] fcp_matrix = ((FractalCloudPoints)FAP.GetUniqueParameter()).fractalCloudPoint;
            int abciss_step_size = width / fcp_matrix.Length + (width % fcp_matrix.Length != 0 ? 1 : 0);
            int ordinate_step_size = height / fcp_matrix[0].Length + (height % fcp_matrix[0].Length != 0 ? 1 : 0);
            Color using_color;
            int TraceLimit=((FractalCloudPoints)FAP.GetUniqueParameter()).MaxAmmountAtTrace;
            for (int _x = 0; _x < fcp_matrix.Length; _x++)
            {
                for (int _y = 0; _y < fcp_matrix[0].Length; _y++)
                {
                    if (fcp_matrix[_x][_y].Length < TraceLimit) continue;
                    using_color = _color_array[(_x + _y) % _color_array.Length];
                    for (int i = 0; i < fcp_matrix[_x][_y].Length; i++)
                    {
                        if (fcp_matrix[_x][_y][i].AbcissLocation < 0 || fcp_matrix[_x][_y][i].OrdinateLocation < 0 || fcp_matrix[_x][_y][i].AbcissLocation >= width || fcp_matrix[_x][_y][i].OrdinateLocation >= height) continue;
                        Result.SetPixel(fcp_matrix[_x][_y][i].AbcissLocation, fcp_matrix[_x][_y][i].OrdinateLocation, using_color);
                    }
                }
            }
            return Result;
        }

        #endregion /Realization of abstract methods

        /*__________________________________________________________Общедоступные_методы_класса_________________________________________________________*/
        #region Public methods
        public void CreateRandColorArray(int Count)
        {
            _color_array = new Color[Count];
            Random rand = new Random();
            for(int i=0;i<Count;i++)
            {
                _color_array[i] = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            }
        }
        public void CreateRandColorArray(int Count,int Seed)
        {
            if (Count < 1) throw new ArgumentException("Размер массива не может быть меньше единицы");
            _color_array = new Color[Count];
            Random rand = new Random(Seed);
            for (int i = 0; i < Count; i++)
            {
                _color_array[i] = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            }
        }
        #endregion /Public methods

        public override bool IsCompatible(FractalAssociationParametrs FAP)
        {
            if (FAP == null) throw new ArgumentNullException("Нельзя передавать значение null в данный метод!");
            return FAP.Is2D && (FAP.GetUniqueParameter() is FractalCloudPoints);
        }
    }
}
