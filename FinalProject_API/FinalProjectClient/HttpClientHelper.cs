using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinalProjectClient
{
    public class HttpClientHelper
    {
        public static HttpClient client = new HttpClient();
        public static string baseUrl = "http://lb-finalprojectapi-1801528690.us-east-1.elb.amazonaws.com";
    }
}
