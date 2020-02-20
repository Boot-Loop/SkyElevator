using SkyElevator.src.views.project_manager_views;
using SkyElevator.src.views.project_manager_views.sub_views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SkyElevator.src.view_models
{
    public class ProjectManagerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _new_client_content_control_visibility;
        private bool _new_project_content_control_visibility;

        private ProjectManager _project_manager;

        public bool NewClientCCVisibility {
            get { return _new_client_content_control_visibility; }
            set { _new_client_content_control_visibility = value; onPropertyRaised("NewClientCCVisibility"); }
        }
        public bool NewProjectCCVisibility {
            get { return _new_project_content_control_visibility; }
            set { _new_project_content_control_visibility = value; onPropertyRaised("NewProjectCCVisibility"); }
        }

        public ProjectManagerViewModel(ProjectManager project_manager) {
            this.NewClientCCVisibility = false;
            this.NewProjectCCVisibility = true;
            this._project_manager = project_manager;
        }

        public void nextButtonPressed() {
            NewClientCCVisibility = true;
            NewProjectCCVisibility = false;
        }

        public void backButtonPressed() {
            NewClientCCVisibility = false;
            NewProjectCCVisibility = true;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
