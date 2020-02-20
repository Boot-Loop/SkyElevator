using System.Windows;

using SkyElevator.src.views.home_views;
using SkyElevator.src.views.project_manager_views;

namespace SkyElevator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void applicationStartup(object sender, StartupEventArgs e) {
            ProjectManager project_manager = new ProjectManager();
            project_manager.Show();
        }

        public static void openHome() {
            Home home = new Home();
            home.Show();
        }
    }
}
