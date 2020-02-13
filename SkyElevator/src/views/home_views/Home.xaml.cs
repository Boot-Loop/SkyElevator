using SkyElevator.src.view_models;
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

namespace SkyElevator.src.views.home_views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private MainViewModel mvm = new MainViewModel();
        public Home()
        {
            InitializeComponent();
            this.DataContext = mvm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mvm.AddItem();
        }
    }
}
