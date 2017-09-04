using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SGcombo.WebUtils
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
        //  <para>strSMTPServer -  "smtp.smtpmail.com"</para>
        //  <para> from - "your mail@smtpmail.com"</para>
        //  <para> to - "my_recipient@mail.com"</para>
        //  <para>subject - "subject message"</para>
        //  <para>body - "mail body "</para>
        //  <para>isBodyHTML  true/false</para>
        //  <para>attachmentFileName - "c:/attacments/datafile.pdf"</para>
        //  <para>credentionalEMAIL - "your mail@smtpmail.com"</para>
        //  <para>credentionalPassword -"your email password"</para>
        //  <para>port</para>
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
