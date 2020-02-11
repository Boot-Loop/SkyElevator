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
using SkyElevator.src.ui.project_manager;

namespace SkyElevator.src.ui.project_manager.sub_views
{
    /// <summary>
    /// Interaction logic for NewProject.xaml
    /// </summary>
    public partial class NewProject : UserControl
    {
        private ProjectManager project_manager;
        public NewProject(ProjectManager project_manager) {
            InitializeComponent();
            this.project_manager = project_manager;
        }
        
        private void nextButtonClick(object sender, RoutedEventArgs e) {
            project_manager.nextButtonPressed();
        }

    }
}
