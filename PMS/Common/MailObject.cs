using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace PMS.Common
{
    class MailObject
    {

        private class SmtpConfiguration
        {

            public string mailServerConfiguration;
            public string userId;
            public string password;
            public string smtpPort;
            public string EnableSSL;

            public SmtpConfiguration()
            {
                this.mailServerConfiguration = ConfigurationManager.AppSettings["mailserver"].ToString();
                this.userId = ConfigurationManager.AppSettings["id"].ToString();
                this.password = ConfigurationManager.AppSettings["password"].ToString();
                this.smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
                this.EnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString();
            }
        }

        #region Private Members
        private MailMessage objMailMessage = null;
        private SmtpConfiguration objSmtpConfiguration = null;
        #endregion

        #region Constructors
        public MailObject(string fromAdd, string toAdd, string subject, string txt)
        {
            objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress(fromAdd);
            objMailMessage.To.Add(toAdd);
            objMailMessage.Subject = subject;
            objMailMessage.Body = txt;
            objMailMessage.IsBodyHtml = true;           
            objSmtpConfiguration = new SmtpConfiguration();
        }
        public MailObject(string fromAdd, string toAdd, string subject, string txt, byte[] bytes, string fileName)
        {
            objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress(fromAdd);
            objMailMessage.To.Add(toAdd);
            objMailMessage.Subject = subject;
            objMailMessage.Body = txt;
            objMailMessage.Attachments.Add(new Attachment(new MemoryStream(bytes), fileName));
            objMailMessage.IsBodyHtml = true;
            objSmtpConfiguration = new SmtpConfiguration();
        }
        #endregion

        #region Public Methods
        public void SendMail()
        {
            SmtpClient objSmtpServer = new SmtpClient(objSmtpConfiguration.mailServerConfiguration);
            objSmtpServer.Credentials = new System.Net.NetworkCredential(objSmtpConfiguration.userId, objSmtpConfiguration.password);
            objSmtpServer.Port = Convert.ToInt32(objSmtpConfiguration.smtpPort); // smptPort is added to send mails from AmericanCare mail server            
            if (objSmtpConfiguration.EnableSSL == "true")
                objSmtpServer.EnableSsl = true;
            objSmtpServer.Send(objMailMessage);
        }


        public void SendMailWithAttachment()
        {
            SmtpClient objSmtpServer = new SmtpClient(objSmtpConfiguration.mailServerConfiguration);
            objSmtpServer.Credentials = new System.Net.NetworkCredential(objSmtpConfiguration.userId, objSmtpConfiguration.password);
            objSmtpServer.Port = Convert.ToInt32(objSmtpConfiguration.smtpPort); // smptPort is added to send mails from AmericanCare mail server            
            if (objSmtpConfiguration.EnableSSL == "true")
                objSmtpServer.EnableSsl = true;
            objSmtpServer.Send(objMailMessage);
        }

        public void addCCRecipient(string EmailAddress)
        {
            objMailMessage.CC.Add(EmailAddress);
        }
        #endregion
    }
}
