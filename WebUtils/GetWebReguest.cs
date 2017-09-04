using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace SGCombo.WebUtils
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

    public class GetWebReguest
    {
        private static String buffer = null;
        private static ManualResetEvent oSignalEvent = new ManualResetEvent(false);

        public static int Timeout = 12000;
 	public static bool KeepAlive = true;

        public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";

        private static void FinishWebRequest(IAsyncResult result)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)(result.AsyncState as HttpWebRequest).EndGetResponse(result);
                using (StreamReader SR = new StreamReader(response.GetResponseStream()))
                {
                    buffer = SR.ReadToEnd();
                }
            }
            finally
            {
                oSignalEvent.Set();
            }

        }


        public static String GetContent(string url,string _ContentType = "text/xml; encoding='utf-8'")
        {
          
                oSignalEvent.Set();
                buffer = String.Empty;

		try {

                    using (HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url)){

                    // *** Set properties
                       request.ContinueTimeout = Timeout;
                       request.UserAgent = UserAgent;
                       request.Method = "GET";
                       request.ContentType = _ContentType;
               
                       request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);

                       oSignalEvent.Reset();
                       oSignalEvent.WaitOne();
                    }
                 } finally {
                     oSignalEvent.Set();
                 }



            return buffer;

        }


    }
}
