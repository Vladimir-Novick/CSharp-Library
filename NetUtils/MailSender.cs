using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SGcombo.NetUtils
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

    public class MailSender
    {
	// <summary>Send eMail message</summary> 
	//
        //  <param name="strSMTPServer" > -  "smtp.smtpmail.com"</param>
        //  <param name="from" >  - "your mail@smtpmail.com"</param>
        //  <param name="to" > to - "my_recipient@mail.com"</param>
        //  <param name="subject" > - "subject message"</param>
        //  <param name="body" >body - "mail body "</param>
        //  <param name="isBodyHTML" >  true/false</param>
        //  <param name="attachmentFileName" > - "c:/attacments/datafile.pdf"</param>
        //  <param name="credentionalEMAIL" > - "your mail@smtpmail.com"</param>
        //  <param name="credentionalPassword" > -"your email password"</param>
        //  <param name="port" >port</para>
        // 
        public void SendMail(
                         string strSMTPServer,
                         string from,
                         string to,
                         string subject,
                         string body,
                         bool isBodyHTML, 
                         string attachmentFileName ,
                         string credentionalEMAIL,
                         string credentionalPassword,
                         int port   
                         )
        {
            MailMessage mail = new MailMessage();
         
            mail.From = new MailAddress(from);


            String[] mails = to.Split(';');
            foreach (String m in mails)
            {
                try
                {
                    mail.To.Add(m);
                }
                catch (Exception ex)
                {
                }
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isBodyHTML;


            if (attachmentFileName != null)
            {
                Attachment attachment;
                attachment = new Attachment(attachmentFileName);



                mail.Attachments.Add(attachment);
            }


            SmtpClient smtpClient = new SmtpClient(strSMTPServer);
            smtpClient.Port = port;

            smtpClient.Credentials = new System.Net.NetworkCredential(credentionalEMAIL, credentionalPassword);
            smtpClient.EnableSsl = false;

            smtpClient.Send(mail);

        }
    }
}
