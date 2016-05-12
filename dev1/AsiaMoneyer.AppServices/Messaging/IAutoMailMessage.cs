using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging
{
    public interface IAutoMailMessage<T>
    {
        T MailMessage { get; set; }

        String Subject { get; set; }
        String Body { get; set; }

        void SetFromAccount(string displayName, string emailAddress);

        void AddToEmailAddress(string name, string emailAddress);
        void AddToEmailAddress(string emailAddress);
        void AddCcEmailAddress(string name, string emailAddress);
        void AddCcEmailAddress(string emailAddress);
        void AddBccEmailAddress(string name, string emailAddress);
        void AddBccEmailAddress(string emailAddress);
    }
}
