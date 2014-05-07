using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SWEClient
{
    /// <summary>
    /// Interaktionslogik für DetailedInformationWindow.xaml
    /// </summary>
    public partial class DetailedInformationWindow : Window
    {
        ViewModels.DetailedInformationWindowViewModel viewModel;

        public DetailedInformationWindow(Models.Kontakt param)
        {            
            InitializeComponent();

            viewModel = new ViewModels.DetailedInformationWindowViewModel(param);
            base.DataContext = viewModel;
        }

        public DetailedInformationWindow()
        {
            InitializeComponent();

            viewModel = new ViewModels.DetailedInformationWindowViewModel();
            base.DataContext = viewModel;
        }   
    }
}
