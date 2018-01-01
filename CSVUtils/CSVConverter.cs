using BeProCore.ILDS.Converter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CSV.Utils
{
	////////////////////////////////////////////////////////////////////////////
//	Copyright 2018 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////

    public class CsvConverter
    {
        private class MetodataItem
        {
            public int Column { get; set; }
            public String Field { get; set; }
        }




        /// <summary>
        ///     Convert Csv file by metadata description 
        ///           use only properties ( get; set; ) field
        ///     
        ///    Metadata File as :
        ///        [
        ///          {
        ///         "Column":0,
        ///         "Field":"FL_ID"
        ///             },
        ///         {
        ///         "Column":1,
        ///         "Field":"FL_Dep_Hour"
        ///             },
        ///         {
        ///         "Column":2,
        ///         "Field":"FL_Arrv_Hour"
        ///             }
        ///         ]
        /// 
        ///    run as :
        ///            CsvConverter convert = new CsvConverter();
        ///            List<FlightDetail> flight = convert.CsvConvert<FlightDetail>(@"FlList.txt", @"FlList.json");
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CsvFileName"></param>
        /// <param name="jsonMetoDataFileName"></param>
        /// <returns></returns>
        public List<T> CsvConvert<T>(String CsvFileName,String jsonMetoDataFileName)
        {
            String jsonString = File.ReadAllText(jsonMetoDataFileName, Encoding.UTF8);
            dynamic metoData = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString,typeof( List<MetodataItem>));
            List<T> ret = new List<T>();


            var bindingFlags = BindingFlags.Public |
                            BindingFlags.Instance;
            var fields = typeof(T).GetFields(bindingFlags);

            var properties = typeof(T).GetProperties();

            T t = (T)Activator.CreateInstance(typeof(T), new object[] { });

            String line = "";

            using (var fs = new FileStream(CsvFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader fileReader = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8, false))
                {
                    while ((line = fileReader.ReadLine()) != null)
                    {
                        String[] fieldsItem = line.Split(";");
                        T Obj = (T)Activator.CreateInstance(typeof(T), new object[] { });

                        if (properties.Length > 0)
                        {


                            foreach (var item in metoData)
                            {

                                if (item.Column < fieldsItem.Length)
                                {
                                    String value = fieldsItem[item.Column];
                                    PropertyInfo propertyInfo = Obj.GetType().GetProperty(item.Field);
                                    if (propertyInfo != null)
                                    {
                                        try
                                        {
                                            object safeValue = MyConvert.ChangeType(value, propertyInfo.PropertyType);
                                            propertyInfo.SetValue(Obj, safeValue, null);
                                        }
                                        catch (Exception ex)
                                        {
                                            // TODO - Exception - check field type
                                           MyLogger.LogDebug($"Error: File - {CsvFileName} {item} field - {propertyInfo.Name} {ex.Message} ");
                                        }
                                    }

                                }
                            }
                        }


                        ret.Add(Obj);
                    }
                }
            }

            



            return ret;
        }
    }
}
