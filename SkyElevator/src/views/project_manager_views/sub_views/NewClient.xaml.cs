using SkyElevator.src.models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkyElevator.src.views.project_manager_views.sub_views
{
    /// <summary>
    /// Interaction logic for NewClient.xaml
    /// </summary>
    public partial class NewClient : UserControl
    {
        private ProjectManager project_manager;
        private NewClientViewModel _new_client_view_model = new NewClientViewModel();
        public NewClient(ProjectManager project_manager)
        {
            InitializeComponent();
            DataContext = new ClientModelI();
            this.project_manager = project_manager;
            this.DataContext = _new_client_view_model;
        }

        private void backButtonClick(object sender, RoutedEventArgs e)
        {
            project_manager.backButtonPressed();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _new_client_view_model.createNewClient();
        }
    }
}
