using System;
using System.IO;
using System.Numerics;
namespace FractalBrowser
{
    /// <summary>
    /// Представляет фрактал, данный абстрактный класс хранит данные для фрактала и работы с ним.
    /// </summary>
    [Serializable]
    public abstract class Fractal
    {
        /*_____________________________________________Данные_для_отрисовки,_построения_и_работы_с_фракталом____________________________________________*/
        #region Protected atribytes
        /// <summary>
        /// Число итераций необходимые для подтверждения точки.
        /// </summary>
        protected ulong f_iterations_count;
        /// <summary>
        /// Булеан необходимый для работы с много поточностью, хранить true если в данный момент в отдельном потоке строиться FractalAssociationParameters, в противной случае хранить false.
        /// </summary>
        protected bool f_parallel_isbusy;
        /// <summary>
        /// Процент завершённости фрактала в паралели, нужно чтобы объединят в себе несколько потоков.
        /// </summary>
        protected int f_parallel_percent;
        /// <summary>
        /// Переменная для работы с распаралеливанием, если хранит значение true параллельное построение фрактала должно прерваться.
        /// </summary>
        protected bool f_parallel_must_cancel;
        /// <summary>
        /// Количество тредов, на которое будет распаралелено построение фрактала.
        /// </summary>
        protected int f_number_of_using_threads_for_parallel;
        /// <summary>
        /// Максимальный процент, используеться для распаралеливания.
        /// </summary>
        protected int f_max_percent = 100;

        protected int f_max_count_of_saved_states=120;
        protected FractalAssociationParametrs f_fap;
        #endregion /Protected atribytes

        /*___________________________________________________________Общедоступные_данные_______________________________________________________________*/
        #region Public atribytes

        public FractalColorMode fractalColorMode;
        #endregion /Public atribytes

        /*__________________________________________________________Делегаты_и_эвенты_класса____________________________________________________________*/
        #region Delegates and events
        /// <summary>
        /// Делегат хранить данные для обработки события изменения прогресса.
        /// </summary>
        /// <param name="Sender">Обьект Fractal который вызвал событие</param>
        /// <param name="Percent">Текущий прогресс</param>
        public delegate void _f_progress_changed_delegate(Fractal Sender, int Percent);
        /// <summary>
        /// Вызываеться когда был изменён прогресс построения экзнмпляра FractalAssociationParameters для этого фрактала.
        /// </summary>
        public event _f_progress_changed_delegate ProgressChanged;
        /// <summary>
        /// Делегат для события изменения количество итераций.
        /// </summary>
        /// <param name="NewValue">Новое количество итeраций</param>
        protected delegate void _f_iterations_count_changing_delegate(ulong NewValue);
        /// <summary>
        /// Происходить когда меняеться количество итераций.
        /// </summary>
        protected event _f_iterations_count_changing_delegate f_event_iterations_count_changing;
        /// <summary>
        /// Происходить когда построение фрактала в отдельном потоке завершено.
        /// </summary>
        /// <param name="Sender">Экземпляр фрактала, который вызвал событие</param>
        /// <param name="FAP">Экземпляр готового фрактала.</param>
        public delegate void _f_parallel_fractal_creating_finished_delegate(Fractal Sender, FractalAssociationParametrs FAP);
        /// <summary>
        /// Событие завершение построения фрактала в отдельном потоке.
        /// </summary>
        public event _f_parallel_fractal_creating_finished_delegate ParallelFractalCreatingFinished;

        protected delegate void _f_newpercent_in_parallel_delegate();
        protected event _f_newpercent_in_parallel_delegate f_new_percent_in_parallel;

        protected delegate void _f_parallel_canceled_delegate();
        protected event _f_parallel_canceled_delegate f_parallel_canceled;
        #endregion /Delegates and events

        /*__________________________________________________________Общедоступные_поля_класса___________________________________________________________*/
        #region Public propertyes
        /// <summary>
        /// Получает или задает количество итераций, используемых для построения фрактала.
        /// </summary>
        public ulong Iterations
        {
            get { return f_iterations_count; }
            set
            {
                if (f_event_iterations_count_changing != null)
                    f_event_iterations_count_changing(value);
            }
        }
        /// <summary>
        /// Получает информацию создаеться ли FractalAssociationParameters этого фрактала в отдельном потоке, true если строиться, иначе false.
        /// </summary>
        public bool IsBusy
        {
            get { return f_parallel_isbusy; }
        }
        /// <summary>
        /// Была ли прерванно паралельное построение фрактала, возвращает true если паралельное построение было прервано, в противной случае false.
        /// </summary>
        public bool IsCanceled
        {
            get { return f_parallel_must_cancel; }
        }
        /// <summary>
        /// Максимальный процент, завершонность процесса измеряется в шкале от 0 до MaxPercent (по умолчаниею 100).
        /// </summary>
        public int MaxPercent
        {
            get { return f_max_percent; }
            set
            {
                if (value < 1) throw new ArgumentException("Значение процента не может быть меньше единицы (переданное значение " + value + ")!");
                f_max_percent = value;
            }
        }
        #endregion /Public propertyes

        /*_____________________________________________________Защищённые_методы_активации_событий______________________________________________________*/
        #region Protected activators
        /// <summary>
        /// Активирует событие ProgressChanged.
        /// </summary>
        /// <param name="Percent">Процент укаживающий завершенность процесса.</param>
        protected void f_activate_progresschanged(int Percent)
        {
            if (ProgressChanged != null) ProgressChanged(this, Percent);
        }
        /// <summary>
        /// Вызывает событие ParallelFractalCreatingFinished.
        /// </summary>
        /// <param name="FAP">Экземпляр готового фрактала.</param>
        protected void f_activate_ParallelFractalCreatingFinished(FractalAssociationParametrs FAP)
        {
            if (ParallelFractalCreatingFinished != null) ParallelFractalCreatingFinished(this, FAP);
        }
        /// <summary>
        /// Вызывает событие f_new_percent_in_parallel.
        /// </summary>
        protected void f_new_percent_in_parallel_activate()
        {
            if (f_new_percent_in_parallel != null)
                f_new_percent_in_parallel();
        }




        #endregion /Protected activators

        /*______________________________________________________Общедоступные_абстрактные_методы________________________________________________________*/
        #region Public abstract methods
        public abstract FractalAssociationParametrs CreateFractal(int Width, int Height);

        public abstract void CreateParallelFractal(int Width, int Height);

        public abstract void CreateParallelFractal(int Width, int Height, int HorizontalStar, int VerticalStart, int SelectedWidth, int SelectedHeight, bool safe = false);

        public abstract FractalType GetFractalType();

        public abstract bool GetBack();

        public abstract Fractal GetClone();

        public abstract bool CanBack();

        public abstract System.Numerics.BigInteger[] FindFirstInserts(BigInteger Width, BigInteger Height, ulong MinIters);

        public abstract double[] GetDoubleFirstInserts(BigInteger[] args);

        public abstract void AlignBy(BigInteger Width, BigInteger Height, ulong Iters);
        #endregion /Public abstract methods

        /*______________________________________________________Защищённые_абстрактные_методы___________________________________________________________*/
        #region Protected abstract methods
        #endregion /Protected abstract methods

        /*________________________________________________________Общедоступные_методы_класса___________________________________________________________*/
        #region Public methods
        /// <summary>
        /// Сообщает фракталу что нужно прервать паралельное построение фрактала.
        /// </summary>
        public void CancelParallelCreating()
        {
            if (f_parallel_isbusy && f_parallel_canceled != null) f_parallel_canceled();
        }

        public void ConnectFractalToPictureBox(FractalPictureBox fpb)
        {
            
        }

        #endregion /Public methods

        /*_________________________________________________________Защищённые_утилиты_класса____________________________________________________________*/
        #region Protected utilities of class
        /// <summary>
        /// Подготавливает текущий экземпляр класса Fractal к паралельному построению фрактала.
        /// <para>
        /// Рекомендуется вызывать данный метод в начале процесса построение фрактала в отдельном потоке,
        /// </para>
        /// </summary>
        protected void f_begin_parallel_process()
        {
            f_parallel_percent = 0;
            f_parallel_must_cancel = false;
            f_parallel_isbusy = true;
            f_new_percent_in_parallel += f_new_percent_handler;
            f_parallel_canceled += f_parallel_canceled_handler;
        }
        protected void f_end_parallel_process()
        {
            if (f_parallel_must_cancel) return;
            f_parallel_isbusy = false;
            f_new_percent_in_parallel -= f_new_percent_handler;
            f_parallel_canceled -= f_parallel_canceled_handler;
            GC.Collect();
        }
        protected void f_new_percent_handler()
        {
            f_activate_progresschanged(++f_parallel_percent);
        }

        protected void f_parallel_canceled_handler()
        {
            f_parallel_must_cancel = true;
            f_parallel_isbusy = false;
            f_new_percent_in_parallel -= f_new_percent_handler;
            f_parallel_canceled -= f_parallel_canceled_handler;
        }
        protected void f_event_iterations_count_changing_handler(ulong NewValue)
        {
            f_iterations_count = NewValue;
        }
        protected void f_allow_change_iterations_count()
        {
            f_event_iterations_count_changing += f_event_iterations_count_changing_handler;
        }
        protected void f_dont_allow_change_iterations_count()
        {
            f_event_iterations_count_changing -= f_event_iterations_count_changing_handler;
        }
        #endregion /Protected utilities of class

        /*______________________________________________________Защищённые_виртуальные_методы___________________________________________________________*/
        #region Protected virtual methods
        protected virtual object GetResumeData()
        {
            return null;
        }

        #endregion /Protected underclasses

        /*__________________________________________________________Статические_методы__________________________________________________________________*/
        #region Static methods
        public static void CopyTo(Fractal Source,Fractal Destinator)
        {
            Destinator.f_iterations_count = Source.f_iterations_count;
            Destinator.f_max_count_of_saved_states = Source.f_max_count_of_saved_states;
            Destinator.f_max_percent = Source.f_max_percent;
            Destinator.f_number_of_using_threads_for_parallel = Source.f_number_of_using_threads_for_parallel;
        }
        public static void ClearParallelFractalCreatingFinishedEvents(Fractal Fractal)
        {
            Fractal.ParallelFractalCreatingFinished = null;
        }
        public static void ClearProgressChangedEvents(Fractal Fractal)
        {
            Fractal.ProgressChanged = null;
        }
        #endregion /Static methods

        /*______________________________________________________________Перечисления____________________________________________________________________*/
        #region Enums
        /// <summary>
        /// Режим вывода результата найденных вхождений во фрактал.
        /// </summary>
        public enum FindFirstInsertsModes{
            LeftTopAndRightBottomPoints,
            LeftRightTopBottom
        }
        #endregion /Enums
    }

}
