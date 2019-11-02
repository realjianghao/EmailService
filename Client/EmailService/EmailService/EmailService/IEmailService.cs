using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public interface IEmailService
    {
        char SendEmail(string url, string payload);
        char SendEmailBatch(string[] url, string payload);
        char ValidateEmailAddress(string url);
    }
}
