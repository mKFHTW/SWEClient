using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Web;
using System.IO;

namespace SWEClient.ViewModels
{
    class SearchViewModel : INotifyPropertyChanged
    {
        Models.Firma Firma;
        Models.Person Person;
        WebRequest request;
        Stream dataStream;

        public SearchViewModel()
        {
            Firma = new Models.Firma();
            Person = new Models.Person();

            request = WebRequest.Create("http://localhost:8080");            
            request.Method = "POST";
            request.ContentType = "text/xml";
        }
        #region PropertyChangedLogic
        public string Name 
        {
            get { return Firma.Name; }
            set { Firma.Name = value; RaisePropertyChanged("Name"); }
        }

        public string UID
        {
            get { return Firma.UID; }
            set { Firma.UID = value; RaisePropertyChanged("UID"); }
        }

        public string Vorname
        {
            get { return Person.Vorname; }
            set { Person.Vorname = value; RaisePropertyChanged("Vorname"); }
        }

        public string Nachname
        {
            get { return Person.Nachname; }
            set { Person.Nachname = value; RaisePropertyChanged("Nachname"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region ButtonICommandImplementation
        private ICommand _generalCommand;
        public ICommand GeneralCommand
        {
            get
            {
                if (this._generalCommand == null)
                {
                    this._generalCommand = new RelayCommand(GeneralCommandMethod);
                }
                return this._generalCommand;
            }
        }

        private void GeneralCommandMethod(object param)
        {
            switch (param as string)
            {
                case "Firma":
                    SearchFirma();
                    break;
                case "Person":
                    SearchPerson();
                    break;
                case "Rechnung":
                    SearchRechnung();
                    break;
            }
        }

        #region FunctionsToCall
        public void SearchFirma()
        {
            //MessageBox.Show("Firma " + Firma.Name + " " + Firma.UID);
            XElement xml =
                new XElement("Search",
                    new XElement("Firma",
                        new XElement("Name", Firma.Name),
                        new XElement("UID", Firma.UID)
                        )
                        );

            byte[] data = Encoding.UTF8.GetBytes(xml.ToString());
            Send(data);
            Receive();
        }

        public void SearchPerson()
        {
            MessageBox.Show("Person " + Person.Vorname + " " + Person.Nachname);
        }

        public void SearchRechnung()
        {
            MessageBox.Show("Rechnung");
        }
        #endregion
        #endregion

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
            string responseFromServer = reader.ReadToEnd();
            MessageBox.Show(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
        }
    }
}
