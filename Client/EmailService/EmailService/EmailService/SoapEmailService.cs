using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Web;
using System.Collections;
using System.Net;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace EmailService
{
    class SoapEmailService : IEmailService
    {
        public XmlDocument Soap { get; private set; }
        public XmlElement Header { get; private set; }
        public XmlElement Body { get; private set; }
        public bool Debug { get; set; } = true;

        public SoapEmailService()
        {
            Init();
        }

        public char Post(string url)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "Post";
            
            var stream = req.GetRequestStream();
            Soap.Save(stream);
            stream.Close();
            var response = req.GetResponse();
            var res = ReadResult(response.GetResponseStream());


            if (Debug)
            {
                Print();
            }

            return res;
        }
        public void AddMethod(string methodName, string[][] args)
        {
            string apiUri = "http://emailservice/";
            var method = Soap.CreateElement("api:" + methodName, apiUri);

            //method.Prefix = "api";

            foreach (var arg in args)
            {
                var ele = Soap.CreateElement(arg[0]);

                ele.InnerText = arg[1];
                method.AppendChild(ele);
            }
            Body.AppendChild(method);
        }
        public void Print()
        {
            //var writer = new XmlTextWriter(Console.Out);
            //Soap.WriteTo(writer);
            Soap.Save(Console.Out);

        }

        private void Init()
        {
            Soap = new XmlDocument();
            Soap.CreateXmlDeclaration("1.0", "UTF-8", null);

            XmlElement root = Soap.CreateElement("soap", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            //root.SetAttribute("xmlns:api","http://webservice/");

            Soap.AppendChild(root);
            Header = Soap.CreateElement("soap", "Header", "http://schemas.xmlsoap.org/soap/envelope/");
            root.AppendChild(Header);

            Body = Soap.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            root.AppendChild(Body);
        }

        private char ReadResult(Stream stream)
        {
            var sr = new StreamReader(stream, Encoding.UTF8);
            var xml = sr.ReadToEnd();
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var res = doc.GetElementsByTagName("return").Item(0);
            return (char)Convert.ToInt32(res.InnerText);
        }

        public char SendEmail(string url, string payload)
        {
            //throw new NotImplementedException();
            var methodName = "sendEmail";
            var args = new string[][]
            {
                new string[]{"arg0",url },
                new string[]{"arg1",payload}
            };
            AddMethod(methodName, args);

            var res = Post(SoapConfig.SoapUrl);
            return res;
        }

        public char SendEmailBatch(string[] url, string payload)
        {
            //throw new NotImplementedException();
            var methodName = "sendEmailBatch";
            
            var args = new List<string[]>();
            foreach(var arg0 in url)
            {
                args.Add(new string[] { "arg0", arg0 });
            }
            args.Add(new string[] { "arg1", payload });

            AddMethod(methodName, args.ToArray());

            var res = Post(SoapConfig.SoapUrl);
            return res;
        }

        public char ValidateEmailAddress(string url)
        {
            //throw new NotImplementedException();
            var methodName = "validateEmailAddress";
            var args = new string[][]
            {
                new string[]{"arg0",url }
                
            };
            AddMethod(methodName, args);

            var res = Post(SoapConfig.SoapUrl);
            return res;
        }
    }


    

}
