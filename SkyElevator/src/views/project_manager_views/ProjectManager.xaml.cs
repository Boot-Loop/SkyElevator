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
        public ProjectManager()
        {
            InitializeComponent();
            new_client_content_control.Content = new NewClient(this);
            new_project_content_control.Content = new NewProject(this);
            open_project_content_control.Content = new OpenProject();
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

        private void main_grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void nextButtonPressed()
        {
            new_client_content_control.Visibility = Visibility.Visible;
            new_project_content_control.Visibility = Visibility.Hidden;
        }

        public void backButtonPressed()
        {
            new_client_content_control.Visibility = Visibility.Hidden;
            new_project_content_control.Visibility = Visibility.Visible;
        }
    }
}
