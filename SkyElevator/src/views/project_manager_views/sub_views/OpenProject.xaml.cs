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
    /// Interaction logic for OpenProject.xaml
    /// </summary>
    public partial class OpenProject : UserControl
    {
        public ProjectManager ProjectManager { get; set; }

        public OpenProject(ProjectManager project_manager)
        {
            InitializeComponent();
            this.ProjectManager = project_manager;
            this.DataContext = new OpenProjectViewModel(this);
        }
    }
}
