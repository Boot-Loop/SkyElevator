using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SkyElevator.src.view_models;
using SkyElevator.src.views.project_manager_views.sub_views;

namespace SkyElevator.src.views.project_manager_views
{
    /// <summary>
    /// Interaction logic for ProjectManager.xaml
    /// </summary>
    public partial class ProjectManager : Window
    {
        private ProjectManagerViewModel _project_manager_view_model;
        private NewProject _new_project;
        private bool _project_created;

        public ProjectManagerViewModel ProjectManagerViewModel {
            get { return _project_manager_view_model; }
            set { _project_manager_view_model = value; }
        }
        public NewProject NewProject {
            get { return _new_project; }
            set { _new_project = value; }
        }
        public bool ProjectCreated {
            get { return _project_created; }
            set { _project_created = value; }
        }

        public ProjectManager()
        {
            InitializeComponent();
            this.ProjectManagerViewModel = new ProjectManagerViewModel(this);

            Core.Application.singleton.initialize();

            this.DataContext = _project_manager_view_model;

            new_client_content_control.Content = new NewClient(this);
            new_project_content_control.Content = new NewProject(this);
            this.NewProject = new_project_content_control.Content as NewProject;
            open_project_content_control.Content = new OpenProject(this);
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

        public void closeWindow() {
            App.openHome();
            this.Close();
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
