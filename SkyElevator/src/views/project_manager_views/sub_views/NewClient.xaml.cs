using SkyElevator.src.models;
using SkyElevator.src.view_models;
using System.Windows;
using System.Windows.Controls;

namespace SkyElevator.src.views.project_manager_views.sub_views
{
    /// <summary>
    /// Interaction logic for NewClient.xaml
    /// </summary>
    public partial class NewClient : UserControl
    {
        private ProjectManager _project_manager;
        private NewClientViewModel _new_client_view_model;

        public ProjectManager ProjectManager {
            get { return _project_manager; }
            set { _project_manager = value; }
        }

        public NewClient(ProjectManager project_manager) {
            InitializeComponent();
            this._new_client_view_model = new NewClientViewModel(this);

            this.DataContext = _new_client_view_model;
            this.ProjectManager = project_manager;
        }
    }
}
