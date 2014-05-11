using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        Models.InputRechnung Input;
        Models.Rechnungszeile Rechnungszeile;

        ObservableCollection<Models.Rechnungszeile> LineCollection;

        public ObservableCollection<Models.Rechnungszeile> Lines
        {
            get
            {
                return LineCollection;
            }
        }

        public RechnungViewModel(Models.Rechnung param) 
        {
            Rechnung = new Models.Rechnung();
            Rechnungszeile = new Models.Rechnungszeile();
            Input = new Models.InputRechnung();
            LineCollection = new ObservableCollection<Models.Rechnungszeile>();
            Rechnung = param;

            if (!string.IsNullOrWhiteSpace(Rechnung.ID))
                GetLines();
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

        public string Kundenname
        {
            get { return Rechnung.Kundenname; }
            set { Rechnung.Kundenname = value; RaisePropertyChanged("Kundenname"); }
        }
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
        public DateTime Due
        {
            get { return Rechnung.Due; }
            set { Rechnung.Due = value; RaisePropertyChanged("Due"); }
        }
        public int Stk
        {
            get { return Rechnungszeile.Stk; }
            set { Rechnungszeile.Stk = Convert.ToInt32(value); RaisePropertyChanged("Stk"); }
        }
        public string Artikel
        {
            get { return Rechnungszeile.Artikel; }
            set { Rechnungszeile.Artikel = value; RaisePropertyChanged("Artikel"); }
        }
        public double Preis
        {
            get { return Rechnungszeile.Preis; }
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
                default:                    
                    break;
            }
        }

        private void Print()
        {
            throw new NotImplementedException();
        }

        public void Add()
        {
            Models.Rechnungszeile Line = new Models.Rechnungszeile();
            Line.Artikel = Input.Artikel;
            Line.Preis = Input.Preis;
            Line.Stk = Input.Stk;
            Rechnung.Zeilen.Add(Line);
            Lines.Add(Line);

            RaisePropertyChanged("SummeBrutto");
            RaisePropertyChanged("SummeNetto");
        }
        #endregion

        

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
                Proceed();
            }
        }

        public void Proceed()
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
    }
}
