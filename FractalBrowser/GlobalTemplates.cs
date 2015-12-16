using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace FractalBrowser
{
    public static class GlobalTemplates
    {
        public static void Initializate()
        {
            DefaultColor = Color.Black;
            DefaultFont = new Font("Microsoft sans serif", 12.85f);
            globalTemplates = new List<globalTemplate>();
            defaulttemplates = new List<globalTemplate>();
        }
        public static void Initializate(string FileName)
        {
            DefaultColor = Color.Black;
            DefaultFont = new Font(FontFamily.GenericSansSerif, 12.85f);
            try
            {
                BinaryFormatter bf=new BinaryFormatter();
                FileStream fs=new FileStream(FileName,FileMode.Open);
                globalTemplates = (List<globalTemplate>)bf.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                Initializate();
            }
            defaulttemplates = new List<globalTemplate>();
        }
        public static void ToDefault()
        {
            globalTemplates.Clear();
            globalTemplates.AddRange(defaulttemplates);
        }
        public static void SaveTemplates(string FileName)
        {
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, globalTemplates);
                fs.Close();
            }
            catch
            {

            }
        }
        public static void AddTemplate(string Name,Color ForeColor,Font Font)
        {
            globalTemplate gt = new globalTemplate();
            gt.Name = Name;
            gt.ForeColor = ForeColor;
            gt.Font = Font;
            if (globalTemplates.FindIndex((temp) => temp.Name == Name) < 0) globalTemplates.Add(gt);
        }
        public static void AddDefaultTemplate(string Name, Color ForeColor, Font Font)
        {
            globalTemplate gt = new globalTemplate();
            gt.Name = Name;
            gt.ForeColor = ForeColor;
            gt.Font = Font;
            if (defaulttemplates.FindIndex((temp) => temp.Name == Name) < 0) defaulttemplates.Add(gt);
            AddTemplate(Name, ForeColor, Font);
        }
        public static void ChangeTemplate(string Name,Color NeoColor,Font NeoFont)
        {
            globalTemplate gt = globalTemplates.Find((t) => t.Name == Name);
            if(gt==null)
            {
                AddTemplate(Name, NeoColor, NeoFont);
            }
            else
            {
                gt.ForeColor = NeoColor;
                gt.Font = NeoFont;
            }
        }
        public static void SetRecTemplate(Control control,string Name)
        {
            SetTemplate(control, Name);
            foreach (Control reccontrol in control.Controls) SetRecTemplate(reccontrol, Name);
        }
        public static void SetTemplate(Control control,string Name)
        {
            globalTemplate temp = globalTemplates.Find((gt)=>gt.Name==Name);
            if (temp == null) AddTemplate(Name, DefaultColor, DefaultFont);
            temp = globalTemplates.Find((gt) => gt.Name == Name);
            control.ForeColor = temp.ForeColor;
            control.Font = temp.Font;
        }
        public static Color GetTemplateForeColor(string Name)
        {
            globalTemplate gt = globalTemplates.Find((temp) => temp.Name == Name);
            if (gt != null) return gt.ForeColor;
            else return DefaultColor;
        }
        public static Font GetTemplateFont(string Name)
        {
            globalTemplate gt = globalTemplates.Find((temp) => temp.Name == Name);
            if (gt != null) return gt.Font;
            else return DefaultFont;
        }
        public static string[] GetAllNamesOfTemplates()
        {
            List<string> result = new List<string>();
            foreach(globalTemplate gt in globalTemplates)
            {
                result.Add(gt.Name);
            }
            return result.ToArray();
        }
        public static Color DefaultColor;
        public static Font DefaultFont;
        private static List<globalTemplate> globalTemplates,defaulttemplates;
       [Serializable]
       private class globalTemplate
       {
           public string Name;
           public Color ForeColor;
           public Font Font;
       }
    }
}
