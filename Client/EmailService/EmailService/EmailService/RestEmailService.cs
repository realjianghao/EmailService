using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    class RestEmailService : IEmailService
    {
        public char SendEmail(string url, string payload)
        {
            url = RestConfig.RestUrl + "sendemail?url=" + url + "&payload=" + payload;

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "Get";
            var response = request.GetResponse();
            var res = response.GetResponseStream().ReadByte();
            return (char)res;
            //throw new NotImplementedException();
        }

        public char SendEmailBatch(string[] url, string payload)
        {
            //throw new NotImplementedException();
            bool res = true;
            foreach (var i in url)
            {
                char ch = SendEmail(i, payload);
                res &= (ch == 'Y');
                if (!res)
                {
                    break;
                }
            }
            return res ? 'Y' : 'N';
        }

        public char ValidateEmailAddress(string url)
        {
            url = RestConfig.RestUrl + "validate?url=" + url;

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "Get";
            var response = request.GetResponse();
            var res = response.GetResponseStream().ReadByte();
            return (char)res;
            //throw new NotImplementedException();
        }
    }
}
