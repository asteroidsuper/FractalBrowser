using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace FractalBrowser
{
    [Serializable]
    public class FractalTemplates:IList<FractalTemplate>
    {
        /*___________________________________________________________Конструкторы_класса______________________________________________________*/
        #region Constructors
        public FractalTemplates()
        {
            _templates = new List<FractalTemplate>();
        }
        public FractalTemplates(string FileName)
        {
            try
            {
                _save = new FileStream(FileName, FileMode.OpenOrCreate);
                AutoSave = true;
            }
            catch
            {
                _save = null;
            }
            _templates = new List<FractalTemplate>();
            
        }
        #endregion /Constructors

        /*_________________________________________________________Частные_атрибуты_класса____________________________________________________*/
        #region Private atribytes
        private List<FractalTemplate> _templates;
        [NonSerialized]
        private Stream _save;
        [NonSerialized]
        private bool _auto_save;
        private ulong lastindex;
        #endregion /Private atribytes

        /*____________________________________________________________Делегаты_и_эвенты________________________________________________________*/
        #region Delegates and events
        public delegate void DataChangedHandler(FractalTemplates sender);
        public event DataChangedHandler DataChanged;
        public delegate void DataSavedHandler(FractalTemplates sender, Stream Destinator);
        public event DataSavedHandler DataSaved;
        #endregion /Delegates and events

        /*_______________________________________________________Общедоступные_методы_класса___________________________________________________*/
        #region Public methods of class
        public void SaveDataToFile()
        {
            if (_save == null) throw new InvalidOperationException("Поток для записи отсуствует! Дайте поток и повторите операцию.");
            if (!_save.CanWrite) throw new InvalidOperationException("Поток не пригоден для записи! Дайте другой поток.");
                BinaryFormatter bf = new BinaryFormatter();
                _save.Position = 0;
                bf.Serialize(_save, this);
                if (DataSaved != null) DataSaved(this, _save);
        }
        /*public void Add(FractalTemplate FractalTemplate)
        {
            FractalTemplate.index=lastindex++;
            _templates.Add(FractalTemplate);
            if (DataChanged != null) DataChanged(this);
        }
        public void Remove(FractalTemplate fractalTemplate)
        {
            for(int i=0;i<_templates.Count;i++)
            {
                if(_templates[i].index==fractalTemplate.index)
                {
                    _templates.RemoveAt(i);
                    if (DataChanged != null) DataChanged(this);
                    break;
                }
            }
        }*/
        #endregion Public methods of class

        /*__________________________________________________________Частные_методы_класса______________________________________________________*/
        #region Private methods
        private void _save_data(FractalTemplates sender)
        {
            SaveDataToFile();
        }
        #endregion /Private methods

        /*________________________________________________________Общедоступные_поля_класса____________________________________________________*/
        #region Public fields
        public bool AutoSave
        {
            get { return _auto_save; }
            set
            {
                if (value == _auto_save) return;
                _auto_save=value;
                if (value)
                {
                    this.DataChanged += _save_data;
                }
                else this.DataChanged -= _save_data;
            }
        }
        public Stream Stream
        {
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (!value.CanWrite) throw new ArgumentException();
                if (_save != null) _save.Close();
                _save = value;
            }
        }
        public FractalTemplate[] Templates
        {
            get { return _templates.ToArray(); }
        }
        #endregion /Public fields

        /*____________________________________________________Общедоступные_статические_методы_________________________________________________*/
        #region Public static methods
        public static FractalTemplates LoadFromFile(string FileName)
        {
            FileStream stream = null; ;
            try
            {
                stream = new FileStream(FileName, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                FractalTemplates result = (FractalTemplates)bf.Deserialize(stream);
                result.Stream = stream;
                result.AutoSave = true;
                return result;
            }
            catch
            {
                if (stream != null) stream.Close();
                return new FractalTemplates(FileName);
            }
            
        }
        #endregion /Public static methods

        /*________________________________________________________Реализация_интерфейсов_______________________________________________________*/
        #region Realization of interface
        public int IndexOf(FractalTemplate item)
        {
            return _templates.FindIndex(arg => arg.index == item.index);
        }
        public void Insert(int index, FractalTemplate item)
        {
            _templates.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _templates.RemoveAt(index);
        }

        public FractalTemplate this[int index]
        {
            get
            {
                return _templates[index];
            }
            set
            {
                _templates[index] = value;
            }
        }

        public void Add(FractalTemplate item)
        {
            item.index = lastindex++;
            _templates.Add(item);
            if (DataChanged != null) DataChanged(this);
        }

        public void Clear()
        {
            _templates.Clear();
        }

        public bool Contains(FractalTemplate item)
        {
            return _templates.Find(arg => arg.index == item.index)!=null;
        }

        public void CopyTo(FractalTemplate[] array, int arrayIndex)
        {
            _templates.CopyTo(array, arrayIndex);
        }
        public void CopyTo(FractalTemplate[] array)
        {
            _templates.CopyTo(array);
        }
        public int Count
        {
            get { return _templates.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(FractalTemplate item)
        {
            for (int i = 0; i < _templates.Count; i++)
            {
                if (_templates[i].index == item.index)
                {
                    _templates.RemoveAt(i);
                    if (DataChanged != null) DataChanged(this);
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<FractalTemplate> GetEnumerator()
        {
            foreach(FractalTemplate template in _templates)
            {
                yield return template;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion /Realization of interface

        

        
    }
}
