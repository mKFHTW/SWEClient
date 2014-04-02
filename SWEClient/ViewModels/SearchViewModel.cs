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
using System.Collections.ObjectModel;

namespace SWEClient.ViewModels
{
    class SearchViewModel : INotifyPropertyChanged
    {
        Models.Firma Firma;
        Models.Person Person;
        ObservableCollection<Models.Firma> FirmaCollection;

        public ObservableCollection<Models.Firma> Firmen
        {
            get
            {
                return FirmaCollection;
            }
        }

        public SearchViewModel()
        {
            Firma = new Models.Firma();
            Person = new Models.Person();
            FirmaCollection = new ObservableCollection<Models.Firma>();
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
                new XElement("Do","Search",
                    new XElement("Type","Firma",
                        new XElement("Name", Firma.Name),
                        new XElement("UID", Firma.UID)
                        )
                        );

            byte[] data = Encoding.UTF8.GetBytes(xml.ToString());

            Proxy.Instance.Send(data);
            Proxy.Instance.Receive();
            Proceed();
        }

        public void SearchPerson()
        {
            MessageBox.Show("Person " + Person.Vorname + " " + Person.Nachname);
        }

        public void SearchRechnung()
        {
            MessageBox.Show("Rechnung");
        }

        public void Proceed()
        {
            XmlDocument xml = new XmlDocument();            
            xml.LoadXml(Proxy.Instance.Response);
            
            XmlNodeList xnList = xml.SelectNodes("/Type");

            foreach (XmlNode xn in xnList)
            {
                Models.Firma obj = new Models.Firma();

                obj.Name = xn["Name"].InnerText;
                obj.UID = xn["UID"].InnerText;
                FirmaCollection.Add(obj);
            }
        }
        #endregion
        #endregion             
    }
}
