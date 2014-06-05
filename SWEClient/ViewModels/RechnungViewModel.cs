using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace SWEClient.ViewModels
{
    class RechnungViewModel : INotifyPropertyChanged
    {
        Models.Rechnung Rechnung;        
        Models.Rechnungszeile Rechnungszeile;
        Models.Person Person;
        DateTime date;
        bool Editable;

        ObservableCollection<Models.Rechnungszeile> LineCollection;

        public ObservableCollection<Models.Rechnungszeile> Lines
        {
            get
            {
                return LineCollection;
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

        public RechnungViewModel(Models.Rechnung param) 
        {
            Rechnung = new Models.Rechnung();
            Rechnungszeile = new Models.Rechnungszeile();
            Person = new Models.Person();
            
            LineCollection = new ObservableCollection<Models.Rechnungszeile>();
            PersonCollection = new ObservableCollection<Models.Person>();

            Rechnung = param;
            GetPersonen(); 
            Editable = true;

            if (!string.IsNullOrWhiteSpace(Rechnung.ID))
            { 
                GetLines(); Editable = false;
                foreach (Models.Person item in Personen)
                {
                    if (item.ID == Rechnung.KundenID)
                        SelectedPerson = item;
                }
                RaisePropertyChanged("Kundenname");
            }            
        }

        #region PropertyChanged
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

        public Models.Person SelectedPerson { set { Person = value; Name = Person.Nachname; } }

        public bool Exists { get { return Editable; } }
        public string Kundenname
        {
            get { return Rechnung.Kundenname; }
            set { Rechnung.Kundenname = value; RaisePropertyChanged("Kundenname"); }
        }
        public string Name { get; set; }
        public string Kommentar
        {
            get { return Rechnung.Kommentar; }
            set { Rechnung.Kommentar = value; RaisePropertyChanged("Kommentar"); }
        }
        public string Nachricht
        {
            get { return Rechnung.Nachricht; }
            set { Rechnung.Nachricht = value; RaisePropertyChanged("Nachricht"); }
        }
        public DateTime Datum
        {
            get
            {
                if (Rechnung.Datum == DateTime.MinValue)
                    return DateTime.Now;

                return Rechnung.Datum;
            }
            set { Rechnung.Datum = value; RaisePropertyChanged("Datum"); }
        }
        public DateTime Due
        {
            get 
            {
                if (Rechnung.Due == DateTime.MinValue)
                    return DateTime.Now;
                return Rechnung.Due; 
            }
            set { Rechnung.Due = value; RaisePropertyChanged("Due"); }
        }
        public string Stk
        {
            get { return Rechnungszeile.Stk.ToString(); }
            set { Rechnungszeile.Stk = Convert.ToInt32(value); RaisePropertyChanged("Stk"); }
        }
        public string Artikel
        {
            get { return Rechnungszeile.Artikel; }
            set { Rechnungszeile.Artikel = value; RaisePropertyChanged("Artikel"); }
        }
        public string Preis
        {
            get { return Rechnungszeile.Preis.ToString(); }
            set { Rechnungszeile.Preis = Convert.ToDouble(value); RaisePropertyChanged("Preis"); }
        }

        public double SummeNetto
        {
            get
            {
                double Summe = 0;
                foreach (Models.Rechnungszeile item in Lines)
                {
                    Summe += item.Stk * item.Preis;
                }
                //RaisePropertyChanged("SummeBrutto");
                return Summe;
            }
        }
        public double SummeBrutto
        {
            get
            {
                return SummeNetto * 1.2;
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
                case "AddLine":
                    Add();
                    break;
                case "Print":
                    Print();
                    break;  
                case "Changed":
                    UpdateComboBox();
                    break;
                case "Add":
                    InsertRechnung();
                    break;
                default:                    
                    break;
            }
        }

        public void UpdateComboBox()
        {
            Personen.Clear();            

            foreach (Models.Person item in Personen)
            {
                if (item.Vorname.Contains(Kundenname) || item.Nachname.Contains(Kundenname))
                    Personen.Add(item);
            }            
        }

        private void Print()
        {
            PdfCreator.Write(Rechnung);
        }

        public void Add()
        {
            Models.Rechnungszeile Line = new Models.Rechnungszeile();
            Line.Artikel = Artikel;
            Line.Preis = Convert.ToDouble(Preis);
            Line.Stk = Convert.ToInt32(Stk);
            Rechnung.Zeilen.Add(Line);
            Lines.Add(Line);

            RaisePropertyChanged("SummeBrutto");
            RaisePropertyChanged("SummeNetto");
        }

        public void InsertRechnung()
        {
            if (Person != null)
            {
                XElement xml = null;
                new XElement("Insert",
                    new XElement("Rechnung",
                        new XElement("ID", Person.ID),
                        new XElement("Date", Rechnung.Datum),
                        new XElement("Due", Rechnung.Due),
                        new XElement("Kommentar", Rechnung.Kommentar),
                        new XElement("Nachricht", Rechnung.Nachricht)
                        )
                        );

                foreach (Models.Rechnungszeile item in Rechnung.Zeilen)
                {
                    XElement line = new XElement("Zeile",
                        new XElement("Artikel", item.Artikel),
                        new XElement("Menge", item.Stk),
                        new XElement("Preis", item.Preis)
                        );
                    xml.Add(line);
                }

                byte[] data = Encoding.UTF8.GetBytes(xml.ToString());
                Proxy.Instance.Send(data);
                Proxy.Instance.Receive();
            }
        }

        #endregion

        public void GetPersonen()
        {
            XElement xml = null;

            //if (string.IsNullOrWhiteSpace(Rechnung.ID))
            //{
                xml =
                new XElement("Search",
                    new XElement("Personen"                        
                        )
                        );
                byte[] data = Encoding.UTF8.GetBytes(xml.ToString());
                Proxy.Instance.Send(data);
                Proxy.Instance.Receive();
                ProceedPersonen();
            //}
        }

        public void GetLines()
        {
            XElement xml = null;

            if (!string.IsNullOrWhiteSpace(Rechnung.ID))
            {
                xml =
                new XElement("Search",
                    new XElement("Rechnungszeilen",
                        new XElement("ID", Rechnung.ID)
                        )
                        );
                byte[] data = Encoding.UTF8.GetBytes(xml.ToString());
                Proxy.Instance.Send(data);
                Proxy.Instance.Receive();
                ProceedLines();
            }
        }

        public void ProceedPersonen()
        {
            Personen.Clear();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Proxy.Instance.Response);

            XmlElement root = xml.DocumentElement;

            foreach (XmlNode item in root.ChildNodes)
            {
                if (item.Name == "Person")
                {
                    Models.Person person = new Models.Person();

                    foreach (XmlNode line in item.ChildNodes)
                    {
                        if (line.Name == "ID")
                        {
                            person.ID = line.InnerText;
                        }
                        if (line.Name == "Vorname")
                        {
                            person.Vorname = line.InnerText;
                        }
                        if (line.Name == "Nachname")
                        {                            
                            person.Nachname = line.InnerText;
                        }
                    }
                    
                    Personen.Add(person);
                }
            }
        }

        public void ProceedLines()
        {
            Lines.Clear();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Proxy.Instance.Response);

            XmlElement root = xml.DocumentElement;

            foreach (XmlNode item in root.ChildNodes)
            {                
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
                            line.InnerText = line.InnerText.Replace('.', ',');
                            zeile.Preis = Convert.ToDouble(line.InnerText);
                        }
                    }
                    Rechnung.Zeilen.Add(zeile);
                    Lines.Add(zeile);
                }
            }
            RaisePropertyChanged("SummeBrutto");
            RaisePropertyChanged("SummeNetto");
        }
        
        public void Close()
        {
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }
        }             
    }
}
