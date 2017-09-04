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

    public class PostWebRequest
    {

	public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";

 	public int Timeout = 12000;
 	public bool KeepAlive = true;
	public string ContentType = "application/x-www-form-urlencoded";

        public  string SendHttpReq(string sUrl, string sQuery, bool repeat = true)
        {
            String ret = "";
            try
            {
                ret = SendHttpReq_(sUrl, sQuery);
                if (string.IsNullOrEmpty(ret))
                {
		    if (repeat) {	
                       Thread.Sleep(Timeout);
		    }
                    ret = SendHttpReq_(sUrl, sQuery);
                }
             } catch (Exception ex)
            {
		if (repeat) {
                  Thread.Sleep(Timeout);
                  ret = SendHttpReq_(sUrl, sQuery);
                }
               
            }
            return ret;
        }

	

        private  string SendHttpReq_(string lcUrl, string sQuery)
        {
          
                     
            string lcHtml = string.Empty;


            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcUrl);

            // *** Set properties
            loHttp.ContinueTimeout = Timeout;
            loHttp.UserAgent = UserAgent;
            loHttp.Method = "POST";

            string lcPostData = sQuery;
           
            byte[] lbPostBuffer = System.Text.Encoding.UTF8.GetBytes(lcPostData);
            loHttp.ContentLength = lbPostBuffer.Length;
            loHttp.KeepAlive = KeepAlive;
            loHttp.ContentType = ContentType;

            var task = loHttp.GetRequestStreamAsync();
            task.Wait(Timeout);

            using (Stream loPostData = task.Result) {
               loPostData.Write(lbPostBuffer, 0, lbPostBuffer.Length);
            }

            // *** Retrieve request info headers

            var task2 = loHttp.GetResponseAsync();

            task2.Wait(Timeout);

            using (WebResponse response = task2.Result) {
                Encoding enc = Encoding.UTF8;
                using ( StreamReader requestReader = new StreamReader(response.GetResponseStream(), enc)) {
                   lcHtml = requestReader.ReadToEnd();
                }
	    }

            return lcHtml;
        }
    }
}
