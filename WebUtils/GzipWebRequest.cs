using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;

namespace SGcomboLib.WebUtils
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


    public class GzipWebRequest
    {

        
        private String userName = null;
        private String password = null;
        private String domain = null;
        
        
        ///
        /// <summary> Create Domain Credentional </summary> 
        /// 
        /// <param name="UserName">Domain Used Name</param>
        /// <param name="Password">Domain user name Password</param>
        /// <param name="Domain">Domain Name</param>
        ///

        public GzipWebRequest(string UserName, string Password, string Domain = null)
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
        public GzipWebRequest(string UserName, string Password)
        {
            userName = UserName;
            password = Password;

        }

		///
        /// <summary> Create Default Credentional </summary> 
        ///
        public GzipWebRequest()
        {


        }

        
        private System.Net.NetworkCredential GetCredentional(){
            
            
               System.Net.NetworkCredential credential = null;
            
              if ( userName == null ) {
                  return CredentialCache.DefaultCredentials;
              }
            
                if (domain == null)
                {
                    credential = new System.Net.NetworkCredential(userName, password);
                }
                else
                {
                    credential= new System.Net.NetworkCredential(userName, password, domain);
                }
            
            return credential;
            
            
        }
        
        
        
        public int Timeout = 170000;
        public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";
        public string ContentType = "application/x-www-form-urlencoded";
        public string Method = "POST";
        public bool KeepAlive = true;
        public bool AllowAutoRedirect = true;
        public CookieContainer CookieContainer = new CookieContainer();
        public string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

        public string baseDir = "";
        internal int Sleep = 0;

        public  void Decompress(Stream stream, string _origName)
        {

            string origName = baseDir + _origName;

            using (FileStream outFile = File.Create(origName))
            {
                using (GZipStream Decompress = new GZipStream(stream,
                        CompressionMode.Decompress))
                {
                    
                    Decompress.CopyTo(outFile);
                    Console.WriteLine("Decompressed: {0}", origName);

                }
            }
        }


        public void ReadGzipFile(string sUrl, string outFileName)
        {
            // http request

            if (Sleep > 0)
            {
                Thread.Sleep(Sleep);
            }

            string lcHtml = string.Empty;


            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(sUrl);
            loHttp.KeepAlive = KeepAlive;
            loHttp.AllowAutoRedirect = AllowAutoRedirect;
            loHttp.CookieContainer = CookieContainer;
            loHttp.Credentials = GetCredentional();
            loHttp.Accept = Accept;

            // *** Set properties

            loHttp.UserAgent = UserAgent;

            loHttp.Method = Method;
            loHttp.Timeout = Timeout;
            loHttp.ContinueTimeout = Timeout;
            loHttp.ReadWriteTimeout = Timeout;
         
            loHttp.ContentType = ContentType;

            try
            {
                using (WebResponse response = loHttp.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {

                        Decompress(stream, outFileName);
                    }
                }
            } catch ( Exception ex)
            {
                Console.WriteLine($"Error load Gzip File :{outFileName} . Message: {ex.Message}");
            }
            return;
        }

        public string MyWebRequestReg(string sUrl, string sQuery)
        {
            // http request

            string lcHtml = string.Empty;


            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(sUrl);
            loHttp.KeepAlive = KeepAlive;
            loHttp.AllowAutoRedirect = AllowAutoRedirect;
            loHttp.CookieContainer = CookieContainer;
            loHttp.Credentials = GetCredentional();
            loHttp.Accept = Accept;

            // *** Set properties

            loHttp.UserAgent = UserAgent;

            loHttp.Method = Method;
            loHttp.Timeout = Timeout;
            loHttp.ContinueTimeout = Timeout;
            loHttp.ReadWriteTimeout = Timeout;

            string lcPostData = sQuery;

            byte[] lbPostBuffer = System.Text.Encoding.UTF8.GetBytes(lcPostData);
            loHttp.ContentLength = lbPostBuffer.Length;

            loHttp.ContentType = ContentType;

            using (var task = loHttp.GetRequestStreamAsync())
            {
                task.Wait(Timeout);
                using (Stream loPostData = task.Result)
                {
                
                    var taskWrt = loPostData.WriteAsync(lbPostBuffer, 0, lbPostBuffer.Length);
                    taskWrt.Wait(Timeout);

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
                }
            }

        
            return lcHtml;
        }
    }
}
