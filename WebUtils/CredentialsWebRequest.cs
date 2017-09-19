using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace SGcombo.WebUtils
{

////////////////////////////////////////////////////////////////////////////
//	Copyright 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//        
//             https://github.com/Vladimir-Novick/CSharp-Library
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////
/// <summary>
///    
/// </summary>
    public class CredentialsWebRequest
    {

        public int Timeout = 1700000;
        public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";
        public string ContentType = "application/x-www-form-urlencoded";
        public string Method = "GET";
        public bool KeepAlive = true;
        public bool AllowAutoRedirect = true;
        public CookieContainer CookieContainer = new CookieContainer();


        public string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

        public string baseDir = "";
        internal int Sleep = 0;

        private String userName = null;
        private String password = null;
        private String domain = null;

        public void SetAsJson()
        {
            Accept = "application/json;q=0.9,*/*;q=0.8";
        }

        ///
        /// <summary> Create Domain Credentional </summary> 
        /// 
        /// <param name="UserName">Domain Used Name</param>
        /// <param name="Password">Domain user name Password</param>
        /// <param name="Domain">Domain Name</param>
        ///

        public CredentialsWebRequest(string UserName, string Password, string Domain = null)
        {
            userName = UserName;
            password = Password;
            domain = Domain;
        }

        ///
        /// <summary> Create Simple Credentional </summary> 
        /// 
        /// <param name="UserName">Domain Used Name</param>
        /// <param name="Password">Domain user name Password</param>
        ///
        public CredentialsWebRequest(string UserName, string Password)
        {
            userName = UserName;
            password = Password;

        }

        /// <summary> Create standard network Credentional </summary> 
        public CredentialsWebRequest()
        {


        }

        /// <summary> Request with repeat </summary> 
        /// 
        /// <param name="url">http request - string </param>
        /// <param name="counter">max repeated counter</param>
        /// <param name="deltaTime">time by count ( microsecond )</param>/// 
        public string RepeatRequest(string url, int counter = 20, int deltaTime = 20000)
        {
            bool repeat = true;
            int CountWait = 0;
            String ret = ""; ;
            while (repeat)
            {
                try
                {
                    ret = GetWebRequestReg(url);
                    repeat = false;
                }
                catch (Exception)
                {
                    CountWait++;
                    Console.WriteLine($"Wait {deltaTime * CountWait}");
                    Thread.Sleep(deltaTime * CountWait);
                    if (CountWait > counter) break;
                }
            }

            return ret;
        }


        private HttpWebRequest loHttp = null;

        private CredentialCache myCredentialCache = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="UserName"></param>
        /// <param name="SecurelyStoredPassword"></param>
        /// <param name="Domain"></param>
        public void setCredentionalCache(string strURL, string UserName, string SecurelyStoredPassword, string Domain = null)
        {
            myCredentialCache = new CredentialCache();
            myCredentialCache.Add(new Uri(strURL), "Basic", new NetworkCredential(UserName, SecurelyStoredPassword));
            if (Domain != null)
            {
                myCredentialCache.Add(new Uri(strURL), "Digest", new NetworkCredential(UserName, SecurelyStoredPassword, Domain));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUrl"></param>
        /// <returns></returns>
        public string GetWebRequestReg(string sUrl)
        {

            string lcHtml = string.Empty;

            loHttp = (HttpWebRequest)WebRequest.Create(sUrl);
            if (myCredentialCache != null)
            {
                loHttp.Credentials = myCredentialCache;
             }
            else
            {
                if (userName == null)
                {
                    loHttp.UseDefaultCredentials = true;
                    loHttp.PreAuthenticate = true;
                    loHttp.Credentials = CredentialCache.DefaultCredentials;

                }
                else
                {
                    loHttp.UseDefaultCredentials = false;
                    loHttp.PreAuthenticate = true;
                    if (domain == null)
                    {
                        loHttp.Credentials = new System.Net.NetworkCredential(userName, password);
                    }
                    else
                    {
                        loHttp.Credentials = new System.Net.NetworkCredential(userName, password, domain);
                    }
                }
            }

            loHttp.KeepAlive = KeepAlive;
            loHttp.AllowAutoRedirect = AllowAutoRedirect;
            loHttp.CookieContainer = CookieContainer;
            loHttp.Accept = Accept;

            // *** Set properties

            loHttp.UserAgent = UserAgent;

            loHttp.Method = Method;
            loHttp.Timeout = Timeout;
            loHttp.ContinueTimeout = Timeout;
            loHttp.ReadWriteTimeout = Timeout;


            loHttp.ContentType = ContentType;

            //      var taskWrt = loPostData.WriteAsync(lbPostBuffer, 0, lbPostBuffer.Length);
            //      taskWrt.Wait(Timeout);

            // *** Retrieve request info headers
            using (var task2 = loHttp.GetResponseAsync())
            {
                task2.Wait(Timeout);

                using (WebResponse response = task2.Result)
                {
                  
                    Encoding enc = Encoding.UTF8;

                    using (Stream stream = response.GetResponseStream())
                    {

                        using (StreamReader requestReader = new StreamReader(stream, enc))
                        {
                            using (var retTask = requestReader.ReadToEndAsync())
                            {
                                retTask.Wait(Timeout);
                                lcHtml = retTask.Result;
                            }
                        }
                    }
                }

            }

            return lcHtml;
        }
    }
}

