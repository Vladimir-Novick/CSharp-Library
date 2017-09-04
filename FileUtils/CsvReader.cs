using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SGcombo.FileUtils
{

////////////////////////////////////////////////////////////////////////////
//	Copyright 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//        
//             https://github.com/Vladimir-Novick/CSharp-Utility-Classes
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////

    public class CsvReader
    {

        public static bool IsNumber(String value)
        {
            if (value == null || value == "") return false;
            String pattern = @"-?\d*(\.\d+)?";
            bool ret =   Regex.IsMatch(value, pattern);
            return ret;
       
        }


        public  string GetValue(String key)
        {
            int index= -1;
            _dictionary.TryGetValue(key, out index);
            if (index == -1 )
            {
                throw new Exception($"invalid field name <{key}> on file {_fileName}");
            }
            return _row[index];
        }

        private String[] _row;

        private Dictionary<String, int> _dictionary;

        private string _fileName;

        public void SetRow(string line)
        {
            _row = Regex.Split(line, seporator);
	   
        }


        public String seporator = @"[|]";

        public  void SetSchema(string filepath, String _seporator = @"[|]")
        {
            _fileName = filepath;
             seporator = _seporator;
            int count = 1;
           
            int index = 0;
            Dictionary<String, int> dictionary = new Dictionary<string, int>();


            using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = reader.ReadLine();
                    string[] fieldsName = Regex.Split(line, seporator);

                    for ( int i = 0; i < fieldsName.Length; i ++ )
                    {
                        dictionary.Add(fieldsName[i], i);
                    }


                }
            }

            _dictionary =  dictionary;




        }



         


    }
}
