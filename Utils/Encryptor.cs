using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


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

    public class Encryptor
    {
        private static readonly string DEFAULT_KEY = "2bGdj754SKLA41BDF643935";
        private static HMACSHA1 s_objHmac = new HMACSHA1(ConvertStringToByteArray(DEFAULT_KEY));
        private static UnicodeEncoding s_objEncoding;

        private Encryptor()
        {
        }

        public static string CreatePassword(string sValue)
        {

            byte[] lo_arrHash = s_objHmac.ComputeHash(ConvertStringToByteArray(sValue));
            string l_sMAC = Convert.ToBase64String(lo_arrHash, 0, lo_arrHash.Length);
            return l_sMAC;
        }

        public static bool ValidateMAC(string sValue, string sMAC)
        {
            string l_sMAC = CreatePassword(sValue);
            return l_sMAC == sMAC;
        }

        private static Byte[] ConvertStringToByteArray(String s)
        {
            if (s_objEncoding == null)
                s_objEncoding = new UnicodeEncoding();
            Byte[] l_arr = s_objEncoding.GetBytes(s);
            return l_arr;
        }
    }
}
