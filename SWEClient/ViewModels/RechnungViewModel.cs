using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SWEClient.ViewModels
{
    class RechnungViewModel : INotifyPropertyChanged
    {
        Models.Rechnung Rechnung;
        Models.InputRechnung Input;

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
            Input = new Models.InputRechnung();
            LineCollection = new ObservableCollection<Models.Rechnungszeile>();
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
            get { return Input.Kundenname; }
            set { Input.Kundenname = value; RaisePropertyChanged("Kundenname"); }
        }
        public string Kommentar
        {
            get { return Input.Kommentar; }
            set { Input.Kommentar = value; RaisePropertyChanged("Kommentar"); }
        }
        public string Nachricht
        {
            get { return Input.Nachricht; }
            set { Input.Nachricht = value; RaisePropertyChanged("Nachricht"); }
        }
        public DateTime Due
        {
            get { return Input.Due; }
            set { Input.Due = value; RaisePropertyChanged("Due"); }
        }
        public int Stk
        {
            get { return Input.Stk; }
            set { Input.Stk = Convert.ToInt32(value); RaisePropertyChanged("Stk"); }
        }
        public string Artikel
        {
            get { return Input.Artikel; }
            set { Input.Artikel = value; RaisePropertyChanged("Artikel"); }
        }
        public int Preis
        {
            get { return Input.Preis; }
            set { Input.Preis = Convert.ToInt32(value); RaisePropertyChanged("Preis"); }
        }

        public int SummeNetto
        {
            get
            {
                int Summe = 0;
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
                default:                    
                    break;
            }
        }
        #endregion

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
    }
}
