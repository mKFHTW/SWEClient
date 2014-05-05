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
    /// Interaktionslogik für AddRechnung.xaml
    /// </summary>
    public partial class AddRechnung : Window
    {
        ViewModels.RechnungViewModel viewModel;
        public AddRechnung(Models.Rechnung param)
        {
            viewModel = new ViewModels.RechnungViewModel(param);
            this.DataContext = viewModel;

            InitializeComponent();
        }
    }
}
