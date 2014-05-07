using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SWEClient.ViewModels
{
    class DetailedInformationWindowViewModel : INotifyPropertyChanged
    {
        Models.Firma Firma;
        Models.Person Person;
        bool isPerson = false;

        public DetailedInformationWindowViewModel()
        {
            Firma = new Models.Firma();
            Person = new Models.Person();
        }

        public DetailedInformationWindowViewModel(Models.Kontakt param)
        {
            Firma = new Models.Firma();
            Person = new Models.Person();

            if (param is Models.Firma)
            {
                Firma = (Models.Firma)param;                
            }
            else
            { 
                Person = (Models.Person)param;
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
                
            }
        }
        #endregion
    }
}
