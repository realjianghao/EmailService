using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    class SoapConfig
    {
        public static string SoapUrl 
            = "http://localhost:8080/EmailService?wsdl";
    }

    class TcpConfig
    {
            
    }
    
    class RestConfig
    {
        public static string RestUrl = "http://127.0.0.1:8081/service/";
    }
}
