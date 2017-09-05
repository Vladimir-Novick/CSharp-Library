using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGcombo.Utils
{


////////////////////////////////////////////////////////////////////////////
//	Copyright 2004 - 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//        
//             https://github.com/Vladimir-Novick/CSharp-Library
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////
    public class HexConverter
    {

        // HexConverter.ByteArrayToString
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string HexadecimalToString(string Data)
        {
            string Data1 = "";
            string sData = "";

            while (Data.Length > 0)
            {
                Data1 = System.Convert.ToChar(System.Convert.ToUInt32(Data.Substring(0, 2), 16)).ToString();
                sData = sData + Data1;
                Data = Data.Substring(2, Data.Length - 2);
            }
            return sData;
        }
        public static string StringToHexadecimal(string Data)
        {
            string sValue;
            string sHex = "";
            foreach (char c in Data.ToCharArray())
            {
                sValue = String.Format("{0:X}", Convert.ToUInt32(c));
                sHex = sHex + sValue;
            }
            return sHex;
        }
    }
}
