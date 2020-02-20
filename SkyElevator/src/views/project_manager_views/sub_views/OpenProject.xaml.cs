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

using CoreApp = Core.Application;

namespace SkyElevator.src.views.project_manager_views.sub_views
{
    /// <summary>
    /// Interaction logic for OpenProject.xaml
    /// </summary>
    public partial class OpenProject : UserControl
    {
        private OpenProjectViewModel _open_project_view_model;
        public OpenProject()
        {
            InitializeComponent();
            this._open_project_view_model = new OpenProjectViewModel();
            this.DataContext = _open_project_view_model;
            foreach (var p in CoreApp.getSingleton().getRecentProjects())
            {
                Console.WriteLine(String.Format("name : {0} client : {1} path : {2}", p.name, p.client_name, p.path));
            }
        }
    }
}
