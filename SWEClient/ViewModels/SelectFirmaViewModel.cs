using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SWEClient.ViewModels
{
    class SelectFirmaViewModel : INotifyPropertyChanged
    {
        List<Models.Firma> FirmenList;
        Models.Firma Firma;

        public SelectFirmaViewModel()
        {
            Firma = new Models.Firma();
            FirmenList = new List<Models.Firma>();
            FirmenList = Proxy.Instance.FirmenList;
            FirmaCollection = new ObservableCollection<Models.Firma>();
        }

        ObservableCollection<Models.Firma> FirmaCollection;

        public ObservableCollection<Models.Firma> Firmen
        {
            get
            {
                return FirmaCollection;
            }
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

        public Models.Firma SelectedFirma { get { return Firma; } set { Firma = value; ItemSelected(); } }

        public string Name
        {
            get { return Firma.Name; }
            set { Firma.Name = value; RaisePropertyChanged("Name"); }
        }

        public string Suche { get; set; }

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
                case "Select":
                    ItemSelected();
                    break;
                case "Changed":
                    SearchList();
                    break;
                case "Closing":
                    Proxy.Instance.Closed = true;
                    
                    break;
                default:
                    break;
            }
        }

        public void SearchList()
        {
            Firmen.Clear();
            foreach (Models.Firma item in FirmenList)
            {
                if (item.Name.Contains(Suche))
                {
                    Firmen.Add(item);
                }
            }
        }

        public void ItemSelected()
        {
            Proxy.Instance.Selected = SelectedFirma;
             
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }
        }
        #endregion
    }
}
