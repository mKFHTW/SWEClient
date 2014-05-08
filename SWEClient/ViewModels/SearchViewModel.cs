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
        Models.SearchReceipt RechnungSearch;
        Models.Rechnung Rechnung;
        byte[] data;

        #region ObservableCollections
        ObservableCollection<Models.Firma> FirmaCollection;

        public ObservableCollection<Models.Firma> Firmen
        {
            get
            {
                return FirmaCollection;
            }
        }

        ObservableCollection<Models.Person> PersonCollection;

        public ObservableCollection<Models.Person> Personen
        {
            get
            {
                return PersonCollection;
            }
        }

        ObservableCollection<Models.Rechnung> RechnungCollection;

        public ObservableCollection<Models.Rechnung> Rechnungen
        {
            get
            {
                return RechnungCollection;
            }
        }
        #endregion

        public SearchViewModel()
        {
            Firma = new Models.Firma();
            Person = new Models.Person();
            RechnungSearch = new Models.SearchReceipt();
            Rechnung = new Models.Rechnung();
            FirmaCollection = new ObservableCollection<Models.Firma>();
            PersonCollection = new ObservableCollection<Models.Person>();
            RechnungCollection = new ObservableCollection<Models.Rechnung>();
        }

        #region PropertyChangedLogic
        public string BetragVon
        {
            get { return RechnungSearch.BetragVon.ToString(); }
            set { RechnungSearch.BetragVon = Convert.ToInt32(value); RaisePropertyChanged("BetragVon"); }
        }

        public string BetragBis
        {
            get { return RechnungSearch.BetragBis.ToString(); }
            set { RechnungSearch.BetragBis = Convert.ToInt32(value); RaisePropertyChanged("BetragBis"); }
        }

        public DateTime DateBis
        {
            get { return RechnungSearch.DateBis; }
            set { RechnungSearch.DateBis = value; RaisePropertyChanged("DateBis"); }
        }        

        public DateTime DateVon
        {
            get { return RechnungSearch.DateVon; }
            set { RechnungSearch.DateVon = value; RaisePropertyChanged("DateVon"); }
        }

        public string Rechnungsname
        {
            get { return RechnungSearch.Name; }
            set { RechnungSearch.Name = value; RaisePropertyChanged("Rechnungsname"); }
        }

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

        public string Datum
        {
            get { return Rechnung.Datum.ToShortDateString(); }
            set { Rechnung.Datum = Convert.ToDateTime(value); RaisePropertyChanged("Datum"); }
        }

        public string Due
        {
            get { return Rechnung.Due.ToShortDateString(); }
            set { Rechnung.Due = Convert.ToDateTime(value); RaisePropertyChanged("Due"); }
        }



        public Models.Firma SelectedFirma { set { Firma = value; } }
        public Models.Person SelectedPerson { set { Person = value; } }
        public Models.Rechnung SelectedRechnung { set { Rechnung = value; } }

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
                    Firmen.Clear();
                    SearchFirma();
                    Proxy.Instance.Send(data);
                    Proxy.Instance.Receive();
                    Proceed();
                    break;
                case "Person":
                    Personen.Clear();
                    SearchPerson();
                    Proxy.Instance.Send(data);
                    Proxy.Instance.Receive();
                    Proceed();
                    break;
                case "Rechnung":
                    Rechnungen.Clear();
                    SearchRechnung();
                    Proxy.Instance.Send(data);
                    Proxy.Instance.Receive();
                    Proceed();
                    break;
                case "ViewFirma":
                    lViewDoubleClickFirma();
                    break;
                case "ViewPerson":
                    lViewDoubleClickPerson();
                    break;
                case "AddFirma":
                    lViewDoubleClickFirma();
                    break;
                case "AddPerson":
                    lViewDoubleClickPerson();
                    break;
                case "AddRechnung":
                    lViewDoubleClickRechnung();
                    break;
                case "SearchRechnungen":
                    SearchRechnung();
                    Proxy.Instance.Send(data);
                    Proxy.Instance.Receive();
                    Proceed();
                    break;
                default:
                    //DetailedInformationWindow window = new DetailedInformationWindow();
                    //window.Show();
                    break;
            }            
        }               

        #region FunctionsToCall
        public void lViewDoubleClickRechnung()
        {
            AddRechnung window = new AddRechnung(Rechnung);
            window.Show();
        }

        public void lViewDoubleClickFirma()
        {
            DetailedInformationWindow window = new DetailedInformationWindow(Firma);
            window.Show();
        }

        public void lViewDoubleClickPerson()
        {
            DetailedInformationWindow window = new DetailedInformationWindow(Person);
            window.Show();
        }        

        public void SearchFirma()
        {
            XElement xml;
            if (string.IsNullOrWhiteSpace(Firma.Name))
            {
                xml =
                new XElement("Search",
                    new XElement("Firma",
                        new XElement("UID", Firma.UID)
                        )
                        );
            }
            else
            {
                xml =
                    new XElement("Search",
                        new XElement("Firma",
                            new XElement("Name", Firma.Name)
                            )
                            );
            }

            data = Encoding.UTF8.GetBytes(xml.ToString());           
            
        }

        public void SearchPerson()
        {
            XElement xml;
            if (string.IsNullOrWhiteSpace(Person.Vorname))
            {
                xml =
                new XElement("Search", 
                    new XElement("Person", 
                        new XElement("Nachname", Person.Nachname)
                        )
                        );
            }
            else if (string.IsNullOrWhiteSpace(Person.Nachname))
            {
                xml =
                new XElement("Search",
                    new XElement("Person",
                        new XElement("Vorname", Person.Vorname)
                        )
                        );
            }

            else 
            {
                xml =
                new XElement("Search",
                    new XElement("Person",
                        new XElement("Vorname", Person.Vorname),
                        new XElement("Nachname", Person.Nachname)                        
                        )
                        );
            }

            data = Encoding.UTF8.GetBytes(xml.ToString());
        }

        public void SearchRechnung()
        {
            XElement xml = null;
            if (!string.IsNullOrWhiteSpace(RechnungSearch.Name))
            {
                xml =
                new XElement("Search",
                    new XElement("Rechnung",
                        new XElement("Name", RechnungSearch.Name)
                        )
                        );
            }

            else if(RechnungSearch.BetragVon != 0 && RechnungSearch.BetragBis != 0)
            {
                xml =
                new XElement("Search",
                    new XElement("Rechnung",
                        new XElement("BetragVon", RechnungSearch.BetragVon),
                        new XElement("BetragBis", RechnungSearch.BetragBis)
                        )
                        );
            }

            else if (RechnungSearch.DateVon != null && RechnungSearch.DateBis != null)
            {
                xml =
                new XElement("Search",
                    new XElement("Rechnung",
                        new XElement("DateVon", RechnungSearch.DateVon),
                        new XElement("DateBis", RechnungSearch.DateBis)
                        )
                        );
            }

            data = Encoding.UTF8.GetBytes(xml.ToString());
        }

        #region Unpack Response
        public void Proceed()
        {
            XmlDocument xml = new XmlDocument();            
            xml.LoadXml(Proxy.Instance.Response);

            XmlElement root = xml.DocumentElement;

            switch (root.Name)
            {
                case "Personen":
                    UnpackPersonen(root);
                    break;
                case "Firmen":
                    UnpackFirmen(root);
                    break;
                case "Rechnungen":
                    UnpackRechnungen(root);
                    break;
                default:
                    break;
            }
        }

        public void UnpackPersonen(XmlElement root)
        {
            foreach (XmlNode element in root.ChildNodes)
            {
                Models.Person person = new Models.Person();

                foreach (XmlNode item in element.ChildNodes)
                {
                    if (item.Name == "ID")
                    {
                        person.ID = item.InnerText;
                    }
                    if (item.Name == "Vorname")
                    {
                        person.Vorname = item.InnerText;
                    }
                    if (item.Name == "Nachname")
                    {
                        person.Nachname = item.InnerText;
                    }
                    if (item.Name == "Suffix")
                    {
                        person.Suffix = item.InnerText;
                    }
                    if (item.Name == "Titel")
                    {
                        person.Titel = item.InnerText;
                    }
                    if (item.Name == "Adresse")
                    {
                        person.Adresse = item.InnerText;
                    }
                    if (item.Name == "Ort")
                    {
                        person.Ort = item.InnerText;
                    }
                    if (item.Name == "PLZ")
                    {
                        person.PLZ = item.InnerText;
                    }
                    if (item.Name == "RechnungsAdresse")
                    {
                        person.RechnungsAdresse = item.InnerText;
                    }
                    if (item.Name == "RechnungsOrt")
                    {
                        person.RechnungsOrt = item.InnerText;
                    }
                    if (item.Name == "RechnungsPLZ")
                    {
                        person.RechnungsPLZ = item.InnerText;
                    }
                    if (item.Name == "LieferAdresse")
                    {
                        person.LieferAdresse = item.InnerText;
                    }
                    if (item.Name == "LieferOrt")
                    {
                        person.LieferOrt = item.InnerText;
                    }
                    if (item.Name == "LieferPLZ")
                    {
                        person.LieferPLZ = item.InnerText;
                    }
                    if (item.Name == "Firma")
                    {
                        person.Firm = item.InnerText;
                    }
                    if (item.Name == "FirmaID")
                    {
                        person.FirmaID = item.InnerText;
                    }
                    if (item.Name == "Geburtsdatum")
                    {
                        person.GebDatum = Convert.ToDateTime(item.InnerText);
                    }   
                }
                Personen.Add(person);
            }
        }

        public void UnpackFirmen(XmlElement root)
        {
            foreach (XmlNode element in root.ChildNodes)
            {
                Models.Firma firma = new Models.Firma();

                foreach (XmlNode item in element.ChildNodes)
                {
                    if (item.Name == "ID")
                    {
                        firma.ID = item.InnerText;
                    }
                    if (item.Name == "Name")
                    {
                        firma.Name = item.InnerText;
                    }
                    if (item.Name == "UID")
                    {
                        firma.UID = item.InnerText;
                    }
                    if (item.Name == "Adresse")
                    {
                        firma.Adresse = item.InnerText;
                    }
                    if (item.Name == "Ort")
                    {
                        firma.Ort = item.InnerText;
                    }
                    if (item.Name == "PLZ")
                    {
                        firma.PLZ = item.InnerText;
                    }
                    if (item.Name == "RechnungsAdresse")
                    {
                        firma.RechnungsAdresse = item.InnerText;
                    }
                    if (item.Name == "RechnungsOrt")
                    {
                        firma.RechnungsOrt = item.InnerText;
                    }
                    if (item.Name == "RechnungsPLZ")
                    {
                        firma.RechnungsPLZ = item.InnerText;
                    }
                    if (item.Name == "LieferAdresse")
                    {
                        firma.LieferAdresse = item.InnerText;
                    }
                    if (item.Name == "LieferOrt")
                    {
                        firma.LieferOrt = item.InnerText;
                    }
                    if (item.Name == "LieferPLZ")
                    {
                        firma.LieferPLZ = item.InnerText;
                    }
                }
                Firmen.Add(firma);
            }
        }

        public void UnpackRechnungen(XmlElement root)
        {
            foreach (XmlNode element in root.ChildNodes)
            {
                Models.Rechnung rechnung = new Models.Rechnung();

                foreach (XmlNode item in element.ChildNodes)
                {
                    if (item.Name == "ID")
                    {
                        rechnung.ID = item.InnerText;
                    }
                    if (item.Name == "KundenID")
                    {
                        rechnung.KundenID = item.InnerText;
                    }
                    if (item.Name == "Kundenname")
                    {
                        rechnung.Kundenname = item.InnerText;
                    }
                    if (item.Name == "Kommentar")
                    {
                        rechnung.Kommentar = item.InnerText;
                    }
                    if (item.Name == "Nachricht")
                    {
                        rechnung.Nachricht = item.InnerText;
                    }
                    if (item.Name == "Date")
                    {
                        rechnung.Datum = Convert.ToDateTime(item.InnerText);
                    }
                    if (item.Name == "Due")
                    {
                        rechnung.Due = Convert.ToDateTime(item.InnerText);
                    }
                    if (item.Name == "Zeile")
                    {
                        Models.Rechnungszeile zeile = new Models.Rechnungszeile();

                        foreach (XmlNode line in item.ChildNodes)
                        {
                            if (line.Name == "Stk")
                            {
                                zeile.Stk = Convert.ToInt32(line.InnerText);
                            }
                            if (line.Name == "Artikel")
                            {
                                zeile.Artikel = line.InnerText;
                            }
                            if (line.Name == "Preis")
                            {
                                zeile.Preis = Convert.ToDouble(line.InnerText);
                            }                            
                        }
                        rechnung.Zeilen.Add(zeile);
                    }                    
                }    
                Rechnungen.Add(rechnung);
            }
        }
        #endregion
        #endregion
        #endregion
    }
}
