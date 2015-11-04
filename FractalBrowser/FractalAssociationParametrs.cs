using System;
using System.Numerics;

namespace FractalBrowser
{
    /// <summary>
    /// Класс для хранения информации и данных о фрактале, необходимых для его отображения.
    /// Также обладает неким инструментарием для работы с фракталом.
    /// FractalAssociationParametrs (FAP)
    /// </summary>
    [Serializable]
    public class FractalAssociationParametrs
    {
        /*____________________________________________________Конструкторы_класса_____________________________________________________________*/
        #region Public constructors
        /// <summary>
        /// Экземпляр класса FractalAssociationParametrs, который содержить данные о двухмерном фрактале.
        /// </summary>
        /// <param name="IterMatrix">Матрица итераций.</param>
        /// <param name="CreateDate">Дата когда был завершен фрактал.</param>
        /// <param name="CalculateTime">Время затраченное на вычисления фрактала.</param>
        /// <param name="IterCount">Количество итераций, при котором строитьс фрактал.</param>
        /// <param name="LeftEdge">Левая граница, наименьшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая откуда на псевдореальной оси началась постройка фрактала.</param>
        /// <param name="RightEdge">Правая граница, наибольшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая до куда по оси абцисс строиться фрактал.</param>
        /// <param name="TopEdge">Верхняя граница, наименьшая координат на псевдореальной оси ординат в декартовой системе координат, укаживающая откуда стоиться фрактал.</param>
        /// <param name="BottomEdge">Ближайшая граница, наименьшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая откуда строиться фрактал.</param>
        public FractalAssociationParametrs(ulong[][] IterMatrix, DateTime CreateDate, TimeSpan CalculateTime, ulong IterCount, double LeftEdge, double RightEdge, double TopEdge, double BottomEdge, FractalType FType, object Unique = null)
        {
            _fap_create_date = CreateDate;
            _fap_calculating_time = CalculateTime;
            _fap_iteration_count = IterCount;
            _fap_left_edge = convert_to_br(LeftEdge);
            _fap_right_edge = convert_to_br(RightEdge);
            _fap_top_edge = convert_to_br(TopEdge);
            _fap_bottom_edge = convert_to_br(BottomEdge);
            _fap_near_edge = BigRational.Zero;
            _fap_far_edge = BigRational.Zero;
            _fap_aplicate_intervals_count = 0;
            _fap_2d_iterations_matrix = IterMatrix;
            _fap_3d_segments_matrix = null;
            _type_of_the_fractal = FType;
            _unoque_parametr = Unique;
        }
        /// <summary>
        /// Экземпляр класса FractalAssociationParametrs, который содержить данные о двухмерном фрактале.
        /// </summary>
        /// <param name="IterMatrix">Матрица итераций.</param>
        /// <param name="CalculateTime">Врем вычисления фрактала.</param>
        /// <param name="IterCount">Количество итераций, при котором строитьс фрактал.</param>
        /// <param name="LeftEdge">Левая граница, наименьшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая откуда на псевдореальной оси началась постройка фрактала.</param>
        /// <param name="RightEdge">Правая граница, наибольшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая до куда по оси абцисс строиться фрактал.</param>
        /// <param name="TopEdge">Верхняя граница, наименьшая координат на псевдореальной оси ординат в декартовой системе координат, укаживающая откуда стоиться фрактал.</param>
        /// <param name="BottomEdge">Ближайшая граница, наименьшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая откуда строиться фрактал.</param>
        public FractalAssociationParametrs(ulong[][] IterMatrix, TimeSpan CalculateTime, ulong IterCount, double LeftEdge, double RightEdge, double TopEdge, double BottomEdge, FractalType FType, object Unique = null)
        {
            _fap_create_date = DateTime.Now;
            _fap_calculating_time = CalculateTime;
            _fap_iteration_count = IterCount;
            _fap_left_edge = convert_to_br(LeftEdge);
            _fap_right_edge = convert_to_br(RightEdge);
            _fap_top_edge = convert_to_br(TopEdge);
            _fap_bottom_edge = convert_to_br(BottomEdge);
            _fap_near_edge = BigRational.Zero;
            _fap_far_edge = BigRational.Zero;
            _fap_aplicate_intervals_count = 0;
            _fap_2d_iterations_matrix = IterMatrix;
            _fap_3d_segments_matrix = null;
            _type_of_the_fractal = FType;
            _unoque_parametr = Unique;
        }
        /// <summary>
        /// Экземпляр класса для хранения информации и данных о трёхмерном фрактале.
        /// </summary>
        /// <param name="_3DFractalAplicateSegmensMatrix">Матрица отрезков фрактала на оси апликат в декартовой системе координат.</param>
        /// <param name="CreateDate">Дата когда был завершен фрактал.</param>
        /// <param name="CalculateTime">Врем затраченное на вычисления фрактала.</param>
        /// <param name="IterCount">Количество итераций, при котором строитьс фрактал.</param>
        /// <param name="LeftEdge">Левая граница, наименьшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая откуда на псевдореальной оси началась постройка фрактала.</param>
        /// <param name="RightEdge">Правая граница, наибольшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая до куда по оси абцисс строиться фрактал.</param>
        /// <param name="TopEdge">Верхняя граница, наименьшая координат на псевдореальной оси ординат в декартовой системе координат, укаживающая откуда стоиться фрактал.</param>
        /// <param name="BottomEdge">Ближайшая граница, наименьшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая откуда строиться фрактал.</param>
        /// <param name="NearEdge">Ближайшая граница, наименьшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая откуда строиться фрактал.</param>
        /// <param name="FarEdge">Дальнейшая граница, наибольшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая до куда строиться фрактал.</param>
        /// <param name="AplicateIntervalCount">Количество равных интервалов, на которые разбиваем весь доступный отрезок на оси апликат.</param>
        public FractalAssociationParametrs(int[][][] _3DFractalAplicateSegmensMatrix, DateTime CreateDate, TimeSpan CalculateTime, ulong IterCount, double LeftEdge, double RightEdge, double TopEdge, double BottomEdge,
                                           double NearEdge, double FarEdge, int AplicateIntervalCount, FractalType FType, object Unique = null)
        {
            _fap_create_date = CreateDate;
            _fap_calculating_time = CalculateTime;
            _fap_iteration_count = IterCount;
            _fap_left_edge = convert_to_br(LeftEdge);
            _fap_right_edge = convert_to_br(RightEdge);
            _fap_top_edge = convert_to_br(TopEdge);
            _fap_bottom_edge = convert_to_br(BottomEdge);
            _fap_near_edge = convert_to_br(NearEdge);
            _fap_far_edge = convert_to_br(FarEdge);
            _fap_aplicate_intervals_count = AplicateIntervalCount;
            _fap_2d_iterations_matrix = null;
            _fap_3d_segments_matrix = _3DFractalAplicateSegmensMatrix;
            _type_of_the_fractal = FType;
            _unoque_parametr = Unique;
        }
        /// <summary>
        /// Экземпляр класса FractalAssociationParametrs, который содержить данные о двухмерном фрактале.
        /// </summary>
        /// <param name="IterMatrix">Матрица итераций.</param>
        /// <param name="CreateDate">Дата когда был завершен фрактал.</param>
        /// <param name="CalculateTime">Время затраченное на вычисления фрактала.</param>
        /// <param name="IterCount">Количество итераций, при котором строитьс фрактал.</param>
        /// <param name="LeftEdge">Левая граница, наименьшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая откуда на псевдореальной оси началась постройка фрактала.</param>
        /// <param name="RightEdge">Правая граница, наибольшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая до куда по оси абцисс строиться фрактал.</param>
        /// <param name="TopEdge">Верхняя граница, наименьшая координат на псевдореальной оси ординат в декартовой системе координат, укаживающая откуда стоиться фрактал.</param>
        /// <param name="BottomEdge">Ближайшая граница, наименьшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая откуда строиться фрактал.</param>
        public FractalAssociationParametrs(ulong[][] IterMatrix, DateTime CreateDate, TimeSpan CalculateTime, ulong IterCount, BigRational LeftEdge, BigRational RightEdge, BigRational TopEdge, BigRational BottomEdge, FractalType FType, object Unique = null)
        {
            _fap_create_date = CreateDate;
            _fap_calculating_time = CalculateTime;
            _fap_iteration_count = IterCount;
            _fap_left_edge = LeftEdge;
            _fap_right_edge = RightEdge;
            _fap_top_edge = TopEdge;
            _fap_bottom_edge = BottomEdge;
            _fap_near_edge = BigRational.Zero;
            _fap_far_edge = BigRational.Zero;
            _fap_aplicate_intervals_count = 0;
            _fap_2d_iterations_matrix = IterMatrix;
            _fap_3d_segments_matrix = null;
            _type_of_the_fractal = FType;
            _unoque_parametr = Unique;

        }
        /// <summary>
        /// Экземпляр класса для хранения информации и данных о трёхмерном фрактале.
        /// </summary>
        /// <param name="_3DFractalAplicateSegmensMatrix">Матрица отрезков фрактала на оси апликат в декартовой системе координат.</param>
        /// <param name="CreateDate">Дата когда был завершен фрактал.</param>
        /// <param name="CalculateTime">Врем затраченное на вычисления фрактала.</param>
        /// <param name="IterCount">Количество итераций, при котором строитьс фрактал.</param>
        /// <param name="LeftEdge">Левая граница, наименьшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая откуда на псевдореальной оси началась постройка фрактала.</param>
        /// <param name="RightEdge">Правая граница, наибольшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая до куда по оси абцисс строиться фрактал.</param>
        /// <param name="TopEdge">Верхняя граница, наименьшая координат на псевдореальной оси ординат в декартовой системе координат, укаживающая откуда стоиться фрактал.</param>
        /// <param name="BottomEdge">Ближайшая граница, наименьшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая откуда строиться фрактал.</param>
        /// <param name="NearEdge">Ближайшая граница, наименьшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая откуда строиться фрактал.</param>
        /// <param name="FarEdge">Дальнейшая граница, наибольшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая до куда строиться фрактал.</param>
        /// <param name="AplicateIntervalCount">Количество равных интервалов, на которые разбиваем весь доступный отрезок на оси апликат.</param>
        public FractalAssociationParametrs(int[][][] _3DFractalAplicateSegmensMatrix, DateTime CreateDate, TimeSpan CalculateTime, ulong IterCount, BigRational LeftEdge, BigRational RightEdge, BigRational TopEdge, BigRational BottomEdge,
                                           BigRational NearEdge, BigRational FarEdge, int AplicateIntervalCount, FractalType FType, object Unique = null)
        {
            _fap_create_date = CreateDate;
            _fap_calculating_time = CalculateTime;
            _fap_iteration_count = IterCount;
            _fap_left_edge = LeftEdge;
            _fap_right_edge = RightEdge;
            _fap_top_edge = TopEdge;
            _fap_bottom_edge = BottomEdge;
            _fap_near_edge = NearEdge;
            _fap_far_edge = FarEdge;
            _fap_aplicate_intervals_count = AplicateIntervalCount;
            _fap_2d_iterations_matrix = null;
            _fap_3d_segments_matrix = _3DFractalAplicateSegmensMatrix;
            _type_of_the_fractal = FType;
            _unoque_parametr = Unique;
        }
        #endregion /Public constructors

        /*___________________________________________________Информация_об_фрактале___________________________________________________________*/
        #region Information about fractal
        /// <summary>
        /// Количество итераций, при котором строилься фрактал.
        /// </summary>
        private readonly ulong _fap_iteration_count;
        /// <summary>
        /// Дата завершения создания фрактала.
        /// </summary>
        private readonly DateTime _fap_create_date;
        /// <summary>
        /// Время затраченное на построение фроактала.
        /// </summary>
        private readonly TimeSpan _fap_calculating_time;
        /// <summary>
        /// Левая граница, наименьшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая откуда на псевдореальной оси началась постройка фрактала.
        /// </summary>
        private readonly BigRational _fap_left_edge;
        /// <summary>
        /// Правая граница, наибольшая координата на псевдореальной оси абцисс в декартовой системе координат, укаживающая до куда по оси абцисс строиться фрактал.
        /// </summary>
        private readonly BigRational _fap_right_edge;
        /// <summary>
        /// Верхняя граница, наименьшая координат на псевдореальной оси ординат в декартовой системе координат, укаживающая откуда стоиться фрактал.
        /// </summary>
        private readonly BigRational _fap_top_edge;
        /// <summary>
        /// Нижняя граница, наибольшая координата на псевдореальной оси ординат в декартовой системе координат, укаживающая до куда строиться фрактал.
        /// </summary>
        private readonly BigRational _fap_bottom_edge;
        /// <summary>
        /// Ближайшая граница, наименьшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая откуда строиться фрактал.
        /// </summary>
        private readonly BigRational _fap_near_edge;
        /// <summary>
        /// Дальнейшая граница, наибольшая координата на псевдореальной оси апликат в декартовой системе координат, укаживающая до куда строиться фрактал.
        /// </summary>
        private readonly BigRational _fap_far_edge;

        private readonly FractalType _type_of_the_fractal;
        #endregion /Information about fractal

        /*_____________________________________________________Данные_о_фрактале______________________________________________________________*/
        #region Data of the fractal
        /// <summary>
        /// Каждый элемент данной матрицы хранит количество итераций, совершенное в отдельной позиции фрактала соответсвующей позиции элемента в матрице.
        /// </summary>
        private readonly ulong[][] _fap_2d_iterations_matrix;
        /// <summary>
        /// Каждый элемент данной матрицы хранит массив отрезков по оси апликат в декартовой системе координат, совокупность этих отрезков составляют матрицу трёхмерного фрактала 
        /// (Pos[i]-начало отрезка, Pos[i+1] Конец отрезка, где i принадлежит множеству чётных чисел, а Pos это вектор отрезков принадлежащей соответсвующей позиции на осях абцис и ординат в декартовой системе координат).
        /// </summary>
        private readonly int[][][] _fap_3d_segments_matrix;
        /// <summary>
        /// Количество равных интервалов, на которые разбиваем весь доступный отрезок на оси апликат.
        /// </summary>
        private readonly int _fap_aplicate_intervals_count;
        /// <summary>
        /// Уникальный параметр фрактала.(Зависить от самого фрактала)
        /// </summary>
        private readonly object _unoque_parametr;



        #endregion /Data of the fractal

        /*______________________________________________________Утилити_класса________________________________________________________________*/
        #region Protected utilites
        /// <summary>
        /// Предcтавляет число 10.
        /// </summary>
        protected static readonly BigInteger Ten = new BigInteger(10u);
        /// <summary>
        /// Конвертирует double в BigRational.
        /// </summary>
        /// <param name="Arg">число двойной точности для конвертации.</param>
        /// <returns>BigRational представляющее это число двойной точности.</returns>
        protected static BigRational convert_to_br(double Arg)
        {
            int degree = 0;
            while (Arg != Math.Round(Arg))
            {
                Arg *= 10;
                ++degree;
            }
            return new BigRational((long)Arg, BigInteger.Pow(Ten, degree));
        }
        /// <summary>
        /// Конвертирует BigRational в double 
        /// </summary>
        /// <param name="arg">BigRational для конвертации.</param>
        /// <returns>Число двойной точности представляющее BigRational.</returns>
        protected static double conver_to_double(BigRational arg)
        {
            return ((double)((long)arg.Numerator)) / (long)arg.Denominator;
        }

        #endregion /Protected utilites

        /*_________________________________________________Общедоступные_поля_класса__________________________________________________________*/
        #region Public fields
        /// <summary>
        /// Получает ширину фрактала.
        /// </summary>
        public int Width
        {
            get
            {
                return _fap_3d_segments_matrix == null ? _fap_2d_iterations_matrix.Length : _fap_3d_segments_matrix.Length;
            }
        }
        /// <summary>
        /// Получает высоту фрактала.
        /// </summary>
        public int Height
        {
            get
            {
                return _fap_3d_segments_matrix == null ? _fap_2d_iterations_matrix[0].Length : _fap_3d_segments_matrix[0].Length;
            }
        }
        /// <summary>
        /// Возвращает глубину фрактала (eсли есть).
        /// </summary>
        public int Depth
        {
            get
            {
                return _fap_3d_segments_matrix == null ? 0 : _fap_aplicate_intervals_count;
            }
        }
        /// <summary>
        /// Возвоащает true еси контейнер хранит трёхмерный фрактал, в противном случае false.
        /// </summary>
        public bool Is3D
        {
            get { return _fap_2d_iterations_matrix == null; }
        }
        /// <summary>
        /// Возвоащает true еси контейнер хранит двухмерный фрактал, в противном случае false.
        /// </summary>
        public bool Is2D
        {
            get { return _fap_3d_segments_matrix == null; }
        }
        /// <summary>
        /// Возвращает левую границу фратала, наименьшую координату по оси абцисс в декартовой системе координат.
        /// </summary>
        public double LeftEdge
        {
            get { return conver_to_double(_fap_left_edge); }
        }
        /// <summary>
        /// Возвращает левую границу фратала, наименьшую координату по оси абцисс в декартовой системе координат в виде экземпляра класса BigRational.
        /// </summary>
        public BigRational LeftEdgeAsBigRational
        {
            get
            {
                return _fap_left_edge;
            }
        }
        /// <summary>
        /// Возвращает правую границу фратала, наибольшую координату по оси абцисс в декартовой системе координат.
        /// </summary>
        public double RightEdge
        { get { return conver_to_double(_fap_right_edge); } }
        /// <summary>
        /// Возвращает правую границу фратала, наибольшую координату по оси абцисс в декартовой системе координат в виде экземпляра класса BigRational.
        /// </summary>
        public BigRational RightEdgeAsBigRational
        {
            get
            {
                return _fap_right_edge;
            }
        }
        /// <summary>
        /// Получает размер интервала по ис абцисс используемого во фрактале.
        /// </summary>
        public BigRational AbcissIntervalSizeAsBigRational
        {
            get
            {
                BigRational _abciss_length = _fap_right_edge - _fap_left_edge;
                int _count = _fap_2d_iterations_matrix == null ? _fap_3d_segments_matrix.Length : _fap_2d_iterations_matrix.Length;
                return BigRational.Divide(_abciss_length, new BigRational(_count, BigInteger.One));
            }
        }
        /// <summary>
        /// Возвращает верхнюю границу, наименьшая координата по оси ординат в декартовой системе координат.
        /// </summary>
        public double TopEdge
        {
            get
            {
                return conver_to_double(_fap_top_edge);
            }
        }
        /// <summary>
        /// Возвращает верхнюю границу, наименьшая координата по оси ординат в декартовой системе координат в виде BigRational.
        /// </summary>
        public BigRational TopEdgeAsBigRational
        {
            get { return _fap_top_edge; }
        }
        public double BottomEdge
        {
            get
            {
                return conver_to_double(_fap_bottom_edge);
            }
        }
        public BigRational BottomEdgeAsBigRational
        {
            get { return _fap_bottom_edge; }
        }
        public double NearEdge
        {
            get
            {
                return conver_to_double(_fap_near_edge);
            }
        }
        public BigRational NearEdgeAsBigRational
        {
            get { return _fap_near_edge; }
        }
        public double FarEdge
        {
            get { return conver_to_double(_fap_far_edge); }
        }
        public BigRational FarEdgeAsBigRational
        {
            get { return _fap_far_edge; }
        }
        public ulong ItersCount
        {
            get { return _fap_iteration_count; }
        }
        public FractalType FractalType
        {
            get { return _type_of_the_fractal; }
        }
        public ulong[][] _2DIterMatrix
        {
            get
            {
                if (_fap_2d_iterations_matrix == null) return null;
                ulong[][] copy = new ulong[_fap_2d_iterations_matrix.Length][];
                int ullen = _fap_2d_iterations_matrix[0].Length;
                for (int i = 0; i < _fap_2d_iterations_matrix.Length; i++)
                {
                    copy[i] = new ulong[ullen];
                    for (int j = 0; j < ullen; j++) copy[i][j] = _fap_2d_iterations_matrix[i][j];
                }
                return copy;
            }
        }
        #endregion /Public fields

        /*________________________________________________Общедоступные_методы_класса_________________________________________________________*/
        #region Public methods
        public object GetUniqueParameter()
        {
            return _unoque_parametr;
        }


        #endregion /Public methods
    }
}
