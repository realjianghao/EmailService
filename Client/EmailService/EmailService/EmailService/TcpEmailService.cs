using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    class TcpEmailService:IEmailService
    {
        private TcpClient client;
        String ip;
        int port;

        public TcpEmailService(String ip, int port)
        {
            //client.connect(ip,port);
            this.ip = ip;
            this.port = port;
        }

        public TcpEmailService() : this(Dns.GetHostName(), 2222)
        {

        }

        
        private char sendMessage(String message)
        {
            var ep = new IPEndPoint(IPAddress.Parse(ip), port);
            var client = new TcpClient();
            client.Connect(ep);

            var output = new StreamWriter(client.GetStream());
            //StreamWriter writer = new StreamWriter(output);

            //output.NewLine = "\n";
            
            output.Write(message);
            output.Flush();
            //client.GetStream().Write(Encoding.UTF8.GetBytes(message), 0,message.Length);
            
            //output.Close();

            var input = new StreamReader(client.GetStream());
            char res = (char)input.Read();
            return res;
        }


        public char sendEmail(String url, String payload)
        {
            var msg = new StringBuilder();
            msg.Append("sendEmail").Append('\n');
            msg.Append(url).Append('\n');
            msg.Append(payload);
            try
            {
                char res = sendMessage(msg.ToString());
                return res;
            }
            catch (Exception e)
            {
                return 'N';
            }

        }

        public char sendEmailBatch(String[] url, String payload)
        {
            var msg = new StringBuilder();
            msg.Append("sendEmailBatch").Append('\n');
            msg.Append(String.Join(" ", url)).Append('\n');
            msg.Append(payload);
            try
            {
                char res = sendMessage(msg.ToString());
                return res;
            }
            catch (Exception e)
            {
                return 'N';
            }
        }

        public char validateEmailAddress(String url)
        {
            var msg = new StringBuilder();
            msg.Append("validateEmailAddress").Append('\n');
            msg.Append(url);

            try
            {
                char res = sendMessage(msg.ToString());
                return res;
            }
            catch (Exception e)
            {
                return 'N';
            }
        }

        public char SendEmail(string url, string payload)
        {
            throw new NotImplementedException();
        }

        public char SendEmailBatch(string[] url, string payload)
        {
            throw new NotImplementedException();
        }

        public char ValidateEmailAddress(string url)
        {
            throw new NotImplementedException();
        }
    }


}
