using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging
{
    public class AutoSmptEmailMessage : IAutoMailMessage<MailMessage>
    {
        private const string EXCEPTION_INVALID_EMAIL_FORMAT = "Invalid Email Format";

        public MailMessage MailMessage { get; set; }

        public String Subject { get
        {
            return MailMessage.Subject;
        }
            set { MailMessage.Subject = value; }
        }
        public String Body {
            get {
                return MailMessage.Body;
            }
            set {
                MailMessage.Body = value;
            }
        }

        public AutoSmptEmailMessage()
        {
            this.MailMessage = new MailMessage();
            this.MailMessage.IsBodyHtml = true;
        }

        public void SetFromAccount(string displayName, string emailAddress)
        {
            if (Commons.Ultility.IsValidEmailAddress(emailAddress))
            {
                MailAddress mailAddress = new MailAddress(emailAddress, displayName);
                MailMessage.From = mailAddress;
            }
            else
                throw new Exception(EXCEPTION_INVALID_EMAIL_FORMAT);
        }

        public void AddToEmailAddress(string name, string emailAddress)
        {
            if (Commons.Ultility.IsValidEmailAddress(emailAddress))
            {
                MailAddress mailAddress = new MailAddress(emailAddress, name);
                MailMessage.To.Add(mailAddress);
            }
            else
                throw new Exception(EXCEPTION_INVALID_EMAIL_FORMAT);
        }

        public void AddToEmailAddress(string emailAddress)
        {
            this.AddToEmailAddress(emailAddress, emailAddress);
        }

        public void AddCcEmailAddress(string name, string emailAddress)
        {
            if (Commons.Ultility.IsValidEmailAddress(emailAddress))
            {
                MailAddress mailAddress = new MailAddress(emailAddress, name);
                MailMessage.CC.Add(mailAddress);
            }
            else
                throw new Exception(EXCEPTION_INVALID_EMAIL_FORMAT);

        }

        public void AddCcEmailAddress(string emailAddress)
        {
            this.AddCcEmailAddress(emailAddress, emailAddress);
        }

        public void AddBccEmailAddress(string name, string emailAddress)
        {
            if (Commons.Ultility.IsValidEmailAddress(emailAddress))
            {
                MailAddress mailAddress = new MailAddress(emailAddress, name);
                MailMessage.Bcc.Add(mailAddress);
            }
            else
                throw new Exception(EXCEPTION_INVALID_EMAIL_FORMAT);
        }

        public void AddBccEmailAddress(string emailAddress)
        {
            this.AddBccEmailAddress(emailAddress, emailAddress);
        }
    }
}
