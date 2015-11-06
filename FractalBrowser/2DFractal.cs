using System;
using System.Numerics;
using System.Collections.Generic;
namespace FractalBrowser
{
    public abstract class _2DFractal : Fractal
    {
        /*________________________________________________Главные_данные_о_фрактале____________________________________________________________*/
        #region Protected atribytes
        /// <summary>
        /// Левая граница, координата, укаживающая откуда на псевдореальной оси началась постройка фрактала(Ось абцисс в декартовой системе).
        /// </summary>
        protected double _2df_left_edge;
        /// <summary>
        /// Правая граница, координата, укаживающая до куда на псевдореальной оси строиться фрактал(Ось абцисс в декартовой системе).
        /// </summary>
        protected double _2df_right_edge;
        /// <summary>
        /// Верхняя граница, координата, укаживающая откуда, по псевдореальной оси ординат в декартовой системе координат, строиться фрактал.
        /// </summary>
        protected double _2df_top_edge;
        /// <summary>
        /// Верхняя граница, координата, укаживающая до куда, по псевдореальной оси ординат в декартовой системе координат, строиться фрактал.
        /// </summary>
        protected double _2df_bottom_edge;
        /// <summary>
        /// Хранит мнимую ширину фрактала.
        /// </summary>
        protected BigInteger _2df_imagine_width;
        /// <summary>
        /// Хранит мнимую высоту фрактала.
        /// </summary>
        protected BigInteger _2df_imagine_height;

        protected BigInteger _2df_imagine_left;

        protected BigInteger _2df_imagine_top;

        protected Stack<BigInteger[]> _2df_back_stack;
        #endregion /Protected atribytes

        /*________________________________________________Защищённые_утилиты_класса____________________________________________________________*/
        #region Protected utilities of class
        protected void _2df_push_fractal_state()
        {
            if (_2df_back_stack == null) _2df_back_stack = new Stack<BigInteger[]>();
            BigInteger[] new_state = new BigInteger[] { _2df_imagine_left, _2df_imagine_top, _2df_imagine_width, _2df_imagine_height };
            if (_2df_back_stack.Count > 0) { 
                BigInteger[] pass_state=_2df_back_stack.Peek();
                int eq=0;
                for (int i = 0; i < 4 && pass_state[i] == new_state[i]; i++) eq++;
                if (eq == 4) return;
            }
            _2df_back_stack.Push(new_state);

        }
        protected void _2df_pop_fractal_state()
        {
            if (_2df_back_stack == null) return;
            if (_2df_back_stack.Count < 1) return;
            BigInteger[] pass_state = _2df_back_stack.Pop();
            if (pass_state.Length != 4) return;
            _2df_imagine_left = pass_state[0];
            _2df_imagine_top = pass_state[1];
            _2df_imagine_width = pass_state[2];
            _2df_imagine_height = pass_state[3];
        }
        protected void _2df_set_scale(int _Width, int _Height, int n_left, int n_top, int width, int height)
        {
            if (_2df_imagine_width == null)
            {
                _2df_imagine_width = new BigInteger(width);
                _2df_imagine_height = new BigInteger(height);
                _2df_imagine_left = BigInteger.Zero;
                _2df_imagine_top = BigInteger.Zero;
                return;
            }
            if (_Width == width && _Height == height && n_left == 0 && n_top == 0) return;
            _2df_push_fractal_state();
            //BigInteger width_dif=_2df_imagine_width/width,height_dif=_2df_imagine_height/height;
            BigInteger width_dif = _Width / width, height_dif = _Height / height;
            _2df_imagine_left = BigInteger.Multiply(_2df_imagine_left + n_left, width_dif);
            _2df_imagine_top = BigInteger.Multiply(_2df_imagine_top + n_top, height_dif);
            _2df_imagine_width *= width_dif;
            _2df_imagine_height *= height_dif;

        }
        protected void _2df_safe_set_scale(int _Width, int _Height, int n_left, int n_top, int width, int height)
        {
            if (_2df_imagine_width == null)
            {
                _2df_imagine_width = new BigInteger(width);
                _2df_imagine_height = new BigInteger(height);
                _2df_imagine_left = BigInteger.Zero;
                _2df_imagine_top = BigInteger.Zero;
                return;
            }
            if (_Width == width && _Height == height && n_left == 0 && n_top == 0) return;
            _2df_push_fractal_state();
            BigInteger width_dif = _Width / width, height_dif = _Height / height;
            BigInteger safe_dif = width_dif > height_dif ? height_dif : width_dif;
            _2df_imagine_left = BigInteger.Multiply(_2df_imagine_left + n_left, safe_dif);
            _2df_imagine_top = BigInteger.Multiply(_2df_imagine_top + n_top, safe_dif);
            _2df_imagine_width *= safe_dif;
            _2df_imagine_height *= safe_dif;

        }
        protected void _2df_reset_scale(int width, int height)
        {
            _2df_back_stack = null;
            _2df_imagine_width = new BigInteger(width);
            _2df_imagine_height = new BigInteger(height);
            _2df_imagine_left = BigInteger.Zero;
            _2df_imagine_top = BigInteger.Zero;
        }
        protected double _2df_get_double_abciss_interval_length()
        {
            return (_2df_right_edge - _2df_left_edge) / ((long)_2df_imagine_width);
        }
        protected double _2df_get_double_abciss_start()
        {
            return _2df_left_edge + (((_2df_right_edge - _2df_left_edge) / ((long)_2df_imagine_width)) * ((long)_2df_imagine_left));
        }
        protected double _2df_get_double_ordinate_interval_length()
        {
            return (_2df_bottom_edge - _2df_top_edge) / ((long)_2df_imagine_height);
        }
        protected double _2df_get_double_ordinate_start()
        {
            return _2df_top_edge + (((_2df_bottom_edge - _2df_top_edge) / ((long)_2df_imagine_height)) * ((long)_2df_imagine_top));
        }
        protected BigRational _2df_get_br_abciss_interval_length()
        {
            return BigRational.Divide(BigRational.ConvertToBigRational(_2df_right_edge) - BigRational.ConvertToBigRational(_2df_left_edge), new BigRational(_2df_imagine_width));
        }
        protected BigRational _2df_get_br_abciss_start()
        {
            BigRational br_left_edge = BigRational.ConvertToBigRational(_2df_left_edge);
            BigRational br_interval_length = BigRational.Divide(BigRational.ConvertToBigRational(_2df_right_edge) - br_left_edge, new BigRational(_2df_imagine_width));
            return br_left_edge + br_interval_length * (new BigRational(_2df_imagine_left));
        }
        protected BigRational _2df_get_br_ordinate_interval_length()
        {
            return BigRational.Divide(BigRational.ConvertToBigRational(_2df_bottom_edge) - BigRational.ConvertToBigRational(_2df_top_edge), new BigRational(_2df_imagine_height));
        }
        protected BigRational _2df_get_br_ordinate_start()
        {
            BigRational br_top_edge = BigRational.ConvertToBigRational(_2df_top_edge);
            BigRational br_interval_length = BigRational.Divide(BigRational.ConvertToBigRational(_2df_bottom_edge) - br_top_edge, new BigRational(_2df_imagine_height));
            return br_top_edge + br_interval_length * (new BigRational(_2df_imagine_top));
        }
        /// <summary>
        /// Вызывает событие FractalCreatedParallel, и подготавливает для него соответсвующий результат.
        /// </summary>
        /// <param name="result">Матрица итераций.</param>
        /// <param name="width">Ширина матрицы</param>
        /// <param name="height">Высота матрицы</param>
        /// <param name="start_time">Время начала построения фрактала.</param>
        protected void _2df_double_send_result(ulong[][] result, int width, int height, DateTime start_time)
        {
            double vertical = _2df_get_double_abciss_start(), horizontal = _2df_get_double_ordinate_start();
            f_activate_ParallelFractalCreatingFinished(new FractalAssociationParametrs(result, DateTime.Now - start_time, f_iterations_count, vertical, vertical + width * _2df_get_double_abciss_interval_length(),
                horizontal, horizontal + height * _2df_get_double_ordinate_interval_length(), GetFractalType()));
        }
        /// <summary>
        /// Вызывает событие FractalCreatedParallel, и подготавливает для него соответсвующий результат (Версия для вычислений с BigRational).
        /// </summary>
        /// <param name="result">Матрица итераций.</param>
        /// <param name="width">Ширина матрицы</param>
        /// <param name="height">Высота матрицы</param>
        /// <param name="start_time">Время начала построения фрактала.</param>
        protected void _2df_br_send_result(ulong[][] result, int width, int height, DateTime start_time)
        {
            BigRational horizontal = _2df_get_br_abciss_start(), vertical = _2df_get_br_ordinate_start();
            f_activate_ParallelFractalCreatingFinished(new FractalAssociationParametrs(result, DateTime.Now, DateTime.Now - start_time, f_iterations_count, horizontal, horizontal + new BigRational(width, 1) * _2df_get_br_abciss_interval_length(),
                vertical, vertical + new BigRational(height, 1) * _2df_get_br_ordinate_interval_length(), GetFractalType()));
        }
        #endregion /Protected utilities of class

        /*_________________________________________________Общедоступные_свойства______________________________________________________________*/
        #region Public propertyes
        public BigInteger ImagineWidth
        {
            get { return _2df_imagine_width; }
        }

        #endregion /Public propertyes

        /*________________________________________________Делегаты_и_события_класса____________________________________________________________*/
        #region Delegates and events



        #endregion /Delegates and events

        /*______________________________________________Защищенные_активаторы_событий__________________________________________________________*/
        #region Protected activators


        #endregion

        /*___________________________________________________Защищённые_классы_________________________________________________________________*/
        #region Protected classes
        protected class _2DFractalHelper
        {
            /*_______________________________________________Конструкторы_класса____________________________________________________________*/
            #region Constructors of class
            private _2DFractalHelper() { }
            public _2DFractalHelper(_2DFractal Fractal, int Width, int Height)
            {
                if (Width < 1 || Height < 1)
                    throw new ArgumentException("Ширина и высота матрицы не могут быть меньше единицы" + (Width < 1 ? ", ошибочная ширина = " + Width : "") + (Height < 1 ? ", ошибочная высота = " + Height : "") + "!");
                _start_time = DateTime.Now;
                _is_process_parallel = Fractal.f_parallel_isbusy;
                _fractal = Fractal;
                _width = Width;
                _height = Height;
                double future_percent_length = _width / (double)_fractal.f_max_percent;
                _curent_percent = _percent_length = (int)(future_percent_length + (future_percent_length % 1d > 0 ? 1 : 0));
                _iterations_count = Fractal.f_iterations_count;
                _abciss_interval_length = Fractal._2df_get_double_abciss_interval_length();
                _ordinate_interval_length = Fractal._2df_get_double_ordinate_interval_length();
                _left_edge = Fractal._2df_get_double_abciss_start();
                _right_edge = _left_edge + (Width - 1) * _abciss_interval_length;
                _top_edge = Fractal._2df_get_double_ordinate_start();
                _bottom_edge = _top_edge + (Height - 1) * _ordinate_interval_length;
                _result_matrix = new ulong[_width][];
                for (int i = 0; i < _width; i++)
                {
                    _result_matrix[i] = new ulong[_height];
                }
                _abciss_real_values_vector = _create_abciss_real_values_vector();
                _ordinate_real_values_vector = _create_ordinate_real_values_vector();
                _aoh = new AbcissOrdinateHandler(Fractal, Width, Height);
                if (!_fractal.f_parallel_isbusy)
                {
                    _aoh.Disconnect();
                }
            }
            #endregion /Constructors of class

            /*_____________________________________________Общедоступные_атрибуты___________________________________________________________*/
            #region Public Atribytes
            public bool IsIterCountConst;

            #endregion /Public Atribytes

            /*_____________________________________________Частные_атрибуты_класса__________________________________________________________*/
            #region Private atribytes
            private _2DFractal _fractal;
            /// <summary>
            /// Выполняется ли процесс в отдельном потоке, true если процесс выпоняется в отдельном потоке, в противном случае false.
            /// <para>
            /// Следить за тем, нужно ли вызывать событие f_new_percent_in_parallel.
            /// </para>
            /// </summary>
            private bool _is_process_parallel;
            private int _width, _height, _percent_length, _curent_percent;
            private AbcissOrdinateHandler _aoh;
            private ulong _iterations_count;
            private ulong[][] _result_matrix;
            private double _left_edge, _right_edge, _top_edge, _bottom_edge, _abciss_interval_length, _ordinate_interval_length;
            private double[] _abciss_real_values_vector, _ordinate_real_values_vector;
            DateTime _start_time;
            private object _unique=null;
            #endregion /Private atribytes

            /*______________________________________________Частные_утилиты_класса__________________________________________________________*/
            #region Private utilities of class
            private double[] _create_abciss_real_values_vector()
            {
                double[] result = new double[_width];
                int swidth = _width - 1;
                for (int i = 1; i < swidth; ++i)
                {
                    result[i] = _left_edge + i * _abciss_interval_length;
                }
                result[0] = _left_edge;
                result[swidth] = _right_edge;
                return result;
            }
            private double[] _create_ordinate_real_values_vector()
            {
                double[] result = new double[_height];
                int sheight = _height - 1;
                for (int i = 1; i < sheight; ++i)
                {
                    result[i] = _top_edge + i * _ordinate_interval_length;
                }
                result[0] = _top_edge;
                result[sheight] = _bottom_edge;
                return result;
            }

            #endregion /Private utilities of class

            /*_______________________________________________Общедоступные_методы___________________________________________________________*/
            #region Public methods
            public void GiveUnique(object Unique)
            {
                _unique = Unique;
            }
            public void GetComplex(Complex outarg)
            {
                outarg.Real = _abciss_real_values_vector[_aoh.abciss];
                outarg.Imagine = _ordinate_real_values_vector[_aoh.ordinate];
            }
            public Complex GetComplex()
            {
                return new Complex(_abciss_real_values_vector[_aoh.abciss], _ordinate_real_values_vector[_aoh.ordinate]);
            }
            public void GiveIterCount(ulong IterCount)
            {
                _result_matrix[_aoh.abciss][_aoh.ordinate] = IterCount;
                if ((++_aoh.ordinate) >= _height)
                {
                    _aoh.ordinate = 0;
                    if (_aoh.abciss < _width)
                    {
                        _aoh.abciss++;
                        if (_is_process_parallel)
                            if ((--_curent_percent) == 0)
                            {
                                _curent_percent = _percent_length;
                                _fractal.f_new_percent_in_parallel_activate();
                            }

                    }
                }

            }

            public void Reset()
            {
                _aoh.abciss = 0;
                _aoh.ordinate = 0;
            }
            public FractalAssociationParametrs GetResult()
            {
                return new FractalAssociationParametrs(_result_matrix, DateTime.Now - _start_time, _iterations_count, _left_edge, _right_edge, _top_edge, _bottom_edge, _fractal.GetFractalType(), null);
            }
            public FractalAssociationParametrs GetResult(object Unique)
            {
                return new FractalAssociationParametrs(_result_matrix, DateTime.Now - _start_time, _iterations_count, _left_edge, _right_edge, _top_edge, _bottom_edge, _fractal.GetFractalType(), Unique);
            }
            public void SendResult()
            {
                if (!_fractal.f_parallel_must_cancel)
                {
                    _fractal.f_activate_progresschanged(_fractal.f_max_percent);
                    _fractal.f_activate_ParallelFractalCreatingFinished(new FractalAssociationParametrs(_result_matrix, DateTime.Now - _start_time, _iterations_count, _left_edge, _right_edge, _top_edge, _bottom_edge, _fractal.GetFractalType(), _unique,_fractal.GetResumeData()));
                }
            }
            public void SendResult(object Unique)
            {
                if (!_fractal.f_parallel_must_cancel)
                {
                    _fractal.f_activate_progresschanged(_fractal.f_max_percent);
                    _fractal.f_activate_ParallelFractalCreatingFinished(new FractalAssociationParametrs(_result_matrix, DateTime.Now - _start_time, _iterations_count, _left_edge, _right_edge, _top_edge, _bottom_edge, _fractal.GetFractalType(), Unique, _fractal.GetResumeData()));
                }
            }
            #endregion /Public methods

            /*__________________________________________Общедоступные_свойства_класса_______________________________________________________*/
            #region Public properties
            public bool IsReady
            {
                get { return _aoh.abciss >= _width; }
            }
            public bool IsnotReady
            {
                get { return _aoh.abciss < _width; }
            }
            public ulong[][] CommonMatrix
            {
                get { return _result_matrix; }
            }
            public double[] AbcissRealValues
            {
                get { return _abciss_real_values_vector; }
            }
            public double[] OrdinateRealValues
            {
                get { return _ordinate_real_values_vector; }
            }
            public int PercentLength
            {
                get { return _percent_length; }
            }
            public AbcissOrdinateHandler AOH
            {
                get { return _aoh; }
            }
            #endregion /Public properties
        }
        protected class AbcissOrdinateHandler
        {
            /*_________________________________________Конструкторы_класса_________________________________________________________*/
            #region Constructors
            public AbcissOrdinateHandler(_2DFractal fractal, int Width, int Height)
            {
                _end_of_abciss = Width-1;
                _end_of_ordinate = Height-1;
                _fractal = fractal;
                fractal.f_parallel_canceled += parallel_cancel;
            }

            #endregion /Constructors

            /*____________________________________________Данные_класса____________________________________________________________*/
            #region Data
            public int abciss, ordinate;
            private _2DFractal _fractal;
            private int _end_of_abciss, _end_of_ordinate;
            #endregion /Data

            /*____________________________________________Методы_класса____________________________________________________________*/
            #region Methods
            private void parallel_cancel()
            {
                abciss = _end_of_abciss;
                ordinate = _end_of_ordinate;
            }
            public void Disconnect()
            {
                _fractal.f_parallel_canceled -= parallel_cancel;
            }
            #endregion /Methods
        }
        #endregion /Protected classes

        /*_____________________________________________Реализация_абстрактных_методов__________________________________________________________*/
        #region Realization abstract methods
        public override void GetBack()
        {
            _2df_pop_fractal_state();
        }


        #endregion /Realization abstract methods
    }
}