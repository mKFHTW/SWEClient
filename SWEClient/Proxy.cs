using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;

namespace SWEClient
{
    public class Proxy
    {
        private static Proxy instance;
        private WebRequest request;
        private Stream dataStream;

        public Models.Firma Selected { get; set; }
        List<Models.Firma> Firmen;

        public List<Models.Firma> FirmenList { get { return Firmen; } set { Firmen = value; } }
        public bool Closed { get; set; }

        public string Response { get; set; }

        private Proxy()
        {
            Firmen = new List<Models.Firma>();            
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
            request = WebRequest.Create(ConfigurationSettings.AppSettings["Server"]);
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = data.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();
            //Receive();
        }

        public void Receive()
        {
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            Response = reader.ReadToEnd();
            MessageBox.Show(Response);
            reader.Close();
            dataStream.Close();
            response.Close();
            
        }
    }
}
