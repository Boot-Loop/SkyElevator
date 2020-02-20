using SkyElevator.src.view_models;
using SkyElevator.src.views.project_manager_views.sub_views;
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



namespace SkyElevator.src.views.project_manager_views
{
    /// <summary>
    /// Interaction logic for ProjectManager.xaml
    /// </summary>
    public partial class ProjectManager : Window
    {
        private ProjectManagerViewModel _project_manager_view_model;
        private NewProject _new_project;

        public ProjectManagerViewModel ProjectManagerViewModel {
            get { return _project_manager_view_model; }
            set { _project_manager_view_model = value; }
        }
        public NewProject NewProject {
            get { return _new_project; }
            set { _new_project = value; }
        }

        public ProjectManager()
        {
            InitializeComponent();
            this.ProjectManagerViewModel = new ProjectManagerViewModel(this);

            Core.Application.getSingleton().initialize();

            this.DataContext = _project_manager_view_model;

            new_client_content_control.Content = new NewClient(this);
            new_project_content_control.Content = new NewProject(this);
            this.NewProject = new_project_content_control.Content as NewProject;
            open_project_content_control.Content = new OpenProject();
            Console.WriteLine(this.NewProject.ToString());
        }

        private void tabBarButtonClick(object sender, RoutedEventArgs e)
        {
            Button selected_button = (Button)sender;
            var selected_button_style = FindResource("TabButtonProjectMangerS") as Style;
            var unselected_button_style = FindResource("TabButtonProjectMangerU") as Style;

            foreach (Button unselected_button in stack_panel.Children)
            {
                unselected_button.Style = unselected_button_style;
            }
            tab_control.SelectedIndex = stack_panel.Children.IndexOf(selected_button);

            selected_button.Style = selected_button_style;

            tab_cursor.Margin = new Thickness(tab_control.SelectedIndex * 120, 40, 0, 0);
        }

        private void mainGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
