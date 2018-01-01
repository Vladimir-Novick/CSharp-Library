using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

////////////////////////////////////////////////////////////////////////////
//	Copyright 2018 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////

namespace CSV.Utils
{
    /// <summary>
    /// 
    ///    Load CSV file to C# class
    /// 
    ///    using:
    ///    
    ///      CSVAutoLoader autoLoader = new CSVAutoLoader();
    ///      List<RegionList> regionList = autoLoader.LoadFromFile<RegionList>(fileName);
    ///
    /// </summary>
    public class CSVAutoLoader
    {

        private class MetodataItem
        {
            public int Column { get; set; }
            public String Field { get; set; }

            public PropertyInfo propertyInfo { get; set; }
        }

        public String seporator = @"""?[|]""?";

        private Dictionary<string, MetodataItem> _dictionary { get; set; }
        private string[] _row { get; set; }

        private void SetCSVSchema(string filepath)
        {

            _dictionary = new Dictionary<string, MetodataItem>();

            using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = reader.ReadLine();
                    string[] fieldsName = Regex.Split(line, seporator);

                    for (int i = 0; i < fieldsName.Length; i++)
                    {
                        MetodataItem item = new MetodataItem()
                        {
                            Field = fieldsName[i],
                            Column = i,
                            propertyInfo = null
                        };
                        _dictionary.Add(item.Field, item);
                    }

                }
            }

        }

        private bool SetRow(string line)
        {
            _row = Regex.Split(line, seporator);
            if (_row.Length < _dictionary.Count)
            {
                return false;
            }
            return true;
        }

        public List<T> LoadFromFile<T>(String strFileName, string seporator = @"""?[|]""?")
        {
            List<T> objList = new List<T>();

            this.seporator = seporator;

            SetCSVSchema(strFileName);

            var bindingFlags = BindingFlags.Public |
                                BindingFlags.Instance;
            var fields = typeof(T).GetFields(bindingFlags);

            var properties = typeof(T).GetProperties();

            MetodataItem meta = null;

            foreach (var item in properties)
            {
                meta = null;
                bool ok = _dictionary.TryGetValue(item.Name, out meta);
                if (ok)
                {
                    meta.propertyInfo = item;
                }
            }

            String line;

            using (Stream stream = File.OpenRead(strFileName))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(stream))
                {
                    file.ReadLine();

                    while ((line = file.ReadLine()) != null)
                    {

                        while (!SetRow(line))
                        {
                            String line2 = file.ReadLine();
                            line = line + "\n" + line2;
                        }

                        T Obj = (T)Activator.CreateInstance(typeof(T), new object[] { });

                        foreach (var item in _dictionary.Values)
                        {

                            if (item.propertyInfo != null)
                            {
                                String value = _row[item.Column];
                                PropertyInfo propertyInfo = Obj.GetType().GetProperty(item.Field);

                                try
                                    {
                                        propertyInfo.SetValue(Obj, value, null);
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                            }
                        }

                        objList.Add(Obj);

                    }
                }
            }

            return objList;
        }

    }
}
