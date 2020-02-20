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
    /// Interaction logic for NewProject.xaml
    /// </summary>
    public partial class NewProject : UserControl
    {
        private ProjectManager project_manager;
        private NewProjectViewModel _new_project_view_model;

        public ProjectManager ProjectManager {
            get { return project_manager; }
            set { project_manager = value; }
        }
        public NewProjectViewModel NewProjectViewModel {
            get { return _new_project_view_model; }
            set { _new_project_view_model = value; }
        }

        public NewProject(ProjectManager project_manager)
        {
            InitializeComponent();
            NewProjectViewModel = new NewProjectViewModel(this);

            this.DataContext = _new_project_view_model;
            this.ProjectManager = project_manager;
        }
    }
}
