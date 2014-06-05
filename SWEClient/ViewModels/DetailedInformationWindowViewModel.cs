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
using System.Windows.Media;

namespace SWEClient.ViewModels
{
    class DetailedInformationWindowViewModel : INotifyPropertyChanged
    {
        Models.Firma Firma;
        Models.Person Person;
        bool isPerson = false;
        List<Models.Firma> Firmen;
        int amountFirmen;
        //Models.Firma ParamFirma;
        //Models.Person ParamPerson;

        public DetailedInformationWindowViewModel()
        {
            Firma = new Models.Firma();
            Person = new Models.Person();
        }

        public DetailedInformationWindowViewModel(Models.Kontakt param)
        {
            Color = Brushes.LightGreen;
            RaisePropertyChanged("Color");

            Firmen = new List<Models.Firma>();
            GetFirmen();
            amountFirmen = 0;

            //ParamFirma = new Models.Firma();
            //ParamPerson = new Models.Person();

            Firma = new Models.Firma();
            Person = new Models.Person();            

            if (param is Models.Firma)
            {
                //ParamFirma = (Models.Firma)param;
                Firma = (Models.Firma)param.Clone();             
            }
            else
            {
                //ParamPerson = (Models.Person)param;
                Person = (Models.Person)param.Clone();
                isPerson = true;
            }
        }

        #region PropertyChangedLogic
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

        private void NotifyStateChanged()
        {
            RaisePropertyChanged("IsFirma");
            RaisePropertyChanged("CanEditPerson");
            RaisePropertyChanged("CanEditFirma");           
        }

        public bool? IsFirma
        {
            get
            {                
                if (string.IsNullOrWhiteSpace(Person.Nachname) && string.IsNullOrWhiteSpace(Person.Vorname) && string.IsNullOrWhiteSpace(Firma.Name)) return null;
                return !string.IsNullOrWhiteSpace(Firma.Name);
            }
        }

        public bool CanEditPerson
        {
            get
            {
                return IsFirma == null || IsFirma == false;
            }
        }

        public bool CanEditFirma
        {
            get
            {
                return IsFirma == null || IsFirma == true;
            }
        }

        public Brush Color { get; set; }

        public string Name
        {
            get { return Firma.Name; }
            set { Firma.Name = value; RaisePropertyChanged("Name"); NotifyStateChanged(); }
        }

        public string UID
        {
            get { return Firma.UID; }
            set { Firma.UID = value; RaisePropertyChanged("UID"); NotifyStateChanged(); }
        }

        public string Titel
        {
            get { return Person.Titel; }
            set { Person.Titel = value; RaisePropertyChanged("Titel"); NotifyStateChanged(); }
        }

        public string Suffix
        {
            get { return Person.Suffix; }
            set { Person.Suffix = value; RaisePropertyChanged("Suffix"); NotifyStateChanged(); }
        }

        public string Vorname
        {
            get { return Person.Vorname; }
            set { Person.Vorname = value; RaisePropertyChanged("Vorname"); NotifyStateChanged(); }
        }

        public string Nachname
        {
            get { return Person.Nachname; }
            set { Person.Nachname = value; RaisePropertyChanged("Nachname"); NotifyStateChanged(); }
        }

        public DateTime GebDatum
        {
            get { return Person.GebDatum; }
            set { Person.GebDatum = value; RaisePropertyChanged("GebDatum"); NotifyStateChanged(); }
        }

        public string Firm
        {
            get { return Person.Firm; }
            set { Person.Firm = value; RaisePropertyChanged("Firm"); NotifyStateChanged(); }
        }

        public string Adresse
        {
            get 
            {
                if (isPerson)
                    return Person.Adresse;
                else
                    return Firma.Adresse;
            }
            set 
            {
                if (isPerson)
                    Person.Adresse = value;
                else
                    Firma.Adresse = value;
                
                RaisePropertyChanged("Adresse"); NotifyStateChanged(); 
            }
        }

        public string Ort
        {
            get
            {
                if (isPerson)
                    return Person.Ort;
                else
                    return Firma.Ort;
            }
            set
            {
                if (isPerson)
                    Person.Ort = value;
                else
                    Firma.Ort = value;

                RaisePropertyChanged("Ort"); NotifyStateChanged();
            }
        }

        public string PLZ
        {
            get
            {
                if (isPerson)
                    return Person.PLZ;
                else
                    return Firma.PLZ;
            }
            set
            {
                if (isPerson)
                    Person.PLZ = value;
                else
                    Firma.PLZ = value;

                RaisePropertyChanged("PLZ"); NotifyStateChanged();
            }
        }

        public string RechnungsAdresse
        {
            get
            {
                if (isPerson)
                    return Person.RechnungsAdresse;
                else
                    return Firma.RechnungsAdresse;
            }
            set
            {
                if (isPerson)
                    Person.RechnungsAdresse = value;
                else
                    Firma.RechnungsAdresse = value;

                RaisePropertyChanged("RechnungsAdresse"); NotifyStateChanged();
            }
        }

        public string RechnungsOrt
        {
            get
            {
                if (isPerson)
                    return Person.RechnungsOrt;
                else
                    return Firma.RechnungsOrt;
            }
            set
            {
                if (isPerson)
                    Person.RechnungsOrt = value;
                else
                    Firma.RechnungsOrt = value;

                RaisePropertyChanged("RechnungsOrt"); NotifyStateChanged();
            }
        }

        public string RechnungsPLZ
        {
            get
            {
                if (isPerson)
                    return Person.RechnungsPLZ;
                else
                    return Firma.RechnungsPLZ;
            }
            set
            {
                if (isPerson)
                    Person.RechnungsPLZ = value;
                else
                    Firma.RechnungsPLZ = value;

                RaisePropertyChanged("RechnungsPLZ"); NotifyStateChanged();
            }
        }

        public string LieferAdresse
        {
            get
            {
                if (isPerson)
                    return Person.LieferAdresse;
                else
                    return Firma.LieferAdresse;
            }
            set
            {
                if (isPerson)
                    Person.LieferAdresse = value;
                else
                    Firma.LieferAdresse = value;

                RaisePropertyChanged("LieferAdresse"); NotifyStateChanged();
            }
        }

        public string LieferOrt
        {
            get
            {
                if (isPerson)
                    return Person.LieferOrt;
                else
                    return Firma.LieferOrt;
            }
            set
            {
                if (isPerson)
                    Person.LieferOrt = value;
                else
                    Firma.LieferOrt = value;

                RaisePropertyChanged("LieferOrt"); NotifyStateChanged();
            }
        }

        public string LieferPLZ
        {
            get
            {
                if (isPerson)
                    return Person.LieferPLZ;
                else
                    return Firma.LieferPLZ;
            }
            set
            {
                if (isPerson)
                    Person.LieferPLZ = value;
                else
                    Firma.LieferPLZ = value;

                RaisePropertyChanged("LieferPLZ"); NotifyStateChanged();
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
                case "Changed":
                    if (ConnectToFirma())
                    {
                        Color = Brushes.LightGreen;
                        RaisePropertyChanged("Color");
                    }
                    else
                    {
                        Color = Brushes.Red;
                        RaisePropertyChanged("Color");
                    }
                    break;
                case "ConnectFirma":
                    if (ConnectToFirma())
                    {
                        Color = Brushes.LightGreen;
                        RaisePropertyChanged("Color");
                    }
                    else
                    {
                        Color = Brushes.Red;
                        RaisePropertyChanged("Color");
                        Proxy.Instance.FirmenList = Firmen;
                        Proxy.Instance.Closed = false;                        
                        
                        SelectFirma window = new SelectFirma();

                        if (window.ShowDialog() == true)
                        {
                            
                        }                        
                        Firm = Proxy.Instance.Selected.Name;
                        Person.FirmaID = Proxy.Instance.Selected.ID;
                        Color = Brushes.LightGreen;
                        RaisePropertyChanged("Color");
                    }
                    break;

                case "Update":
                    UpdateInsertTarget();
                    break;
                case "RemoveFirmaConnect":
                    RemoveFirmaConnect();
                    break;
                default:
                    break;
            }
        }        
        #endregion

        public void RemoveFirmaConnect()
        {
            if(string.IsNullOrWhiteSpace(Person.ID))
            {
                Person.FirmaID = "";
                Person.Firm = "";
                RaisePropertyChanged("Firm"); NotifyStateChanged();
            }
            else
            {
                XElement xml = null;
                xml =
                new XElement("Update",
                    new XElement("PersonFirma",
                        new XElement("ID", Person.ID)              
                        )
                        );
                byte[] data = Encoding.UTF8.GetBytes(xml.ToString());
                Proxy.Instance.Send(data);
                Person.FirmaID = "";
                Person.Firm = "";
                RaisePropertyChanged("Firm"); NotifyStateChanged();
            }
        }

        private void UpdateInsertTarget()
        {
            XElement xml = null;

            if(isPerson && string.IsNullOrWhiteSpace(Person.ID))
            {
                xml =
                new XElement("Insert",
                    new XElement("Person",
                        new XElement("ID", Person.ID),
                        new XElement("Vorname", Person.Vorname),
                        new XElement("Nachname", Person.Nachname),
                        new XElement("Titel", Person.Titel),
                        new XElement("Suffix", Person.Suffix),
                        new XElement("GebDatum", Person.GebDatum),
                        new XElement("FirmaID", Person.FirmaID)
                        )
                        );
            }

            else if (!isPerson && string.IsNullOrWhiteSpace(Firma.ID))
            {
                xml =
                new XElement("Insert",
                    new XElement("Firma",
                        new XElement("ID", Firma.ID),
                        new XElement("Firmenname", Firma.Name),
                        new XElement("UID", Firma.UID)
                        )
                        );
            }
            else
            {
                switch (isPerson)
                {

                    case true:
                        xml =
                    new XElement("Update",
                        new XElement("Person",
                            new XElement("ID", Person.ID),
                            new XElement("Vorname", Person.Vorname),
                            new XElement("Nachname", Person.Nachname),
                            new XElement("Titel", Person.Titel),
                            new XElement("Suffix", Person.Suffix),
                            new XElement("GebDatum", Person.GebDatum),
                            new XElement("FirmaID", Person.FirmaID)
                            )
                            );
                        break;

                    case false:
                        xml =
                    new XElement("Update",
                        new XElement("Firma",
                            new XElement("ID", Firma.ID),
                            new XElement("Firmenname", Firma.Name),
                            new XElement("UID", Firma.UID)
                            )
                            );
                        break;
                    default:
                        break;
                }
            }

            byte[] data = Encoding.UTF8.GetBytes(xml.ToString());
            Proxy.Instance.Send(data);

            /*if (isPerson)
                ParamPerson = Person;

            else
                ParamFirma = Firma;*/

            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }
        }

        public bool ConnectToFirma()
        {
            foreach (Models.Firma item in Firmen)
            {
                if (item.Name == Firm)
                    amountFirmen++;
                if (amountFirmen == 2)
                {
                    Color = Brushes.Red;
                    RaisePropertyChanged("Color");
                    return false;
                }
            }
            if (amountFirmen == 1)
                return true;
            else
            {
                Color = Brushes.Red;
                RaisePropertyChanged("Color");
                return false;
            }
        }

        public void GetFirmen()
        {
            XElement xml;            
                xml =
                new XElement("Search",
                    new XElement("Firmen")                        
                        );
            byte[] data = Encoding.UTF8.GetBytes(xml.ToString());
            Proxy.Instance.Send(data);
            Proxy.Instance.Receive();

            XmlDocument response = new XmlDocument();
            response.LoadXml(Proxy.Instance.Response);

            XmlElement root = response.DocumentElement;
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
                }
                Firmen.Add(firma);
            }
        }
    }
}
