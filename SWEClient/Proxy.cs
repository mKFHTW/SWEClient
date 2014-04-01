using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient
{
    class Proxy
    {
        private static Proxy instance;
        private WebRequest request;
        private Stream dataStream;

        public string Response { get; set; }

        private Proxy()
        {
            request = WebRequest.Create("http://localhost:8080");
            request.Method = "POST";
            request.ContentType = "text/xml";
        }

        public static Proxy Instance
        {
            get
            {
                if (instance == null)
                    instance = new Proxy();

                return instance;
            }
        }

        public void Send(byte[] data)
        {
            request.ContentLength = data.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();
        }

        public void Receive()
        {
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            Response = reader.ReadToEnd();
            //MessageBox.Show(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
        }
    }
}
