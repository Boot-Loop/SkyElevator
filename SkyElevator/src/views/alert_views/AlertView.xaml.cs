using SkyElevator.src.view_models;
using System.Windows;
using SystemApp = System.Windows.Application;

using static SkyElevator.src.view_models.AlertViewViewModel;

namespace SkyElevator.src.views.alert_views
{
    /// <summary>
    /// Interaction logic for AlertView.xaml
    /// </summary>
    public partial class AlertView : Window
    {
        private AlertViewViewModel _alert_view_view_model;

        public AlertView(string title, string message, AlertViewType type, AlertViewViewModel.Button button1, AlertViewViewModel.Button button2 = null, AlertViewViewModel.Button button3 = null)
        {
            InitializeComponent();
            _alert_view_view_model = new AlertViewViewModel(title, message, type, button1, this.closeCommand, button2, button3);
            this.DataContext = _alert_view_view_model;
            this.Owner = SystemApp.Current.MainWindow;
        }

        private void closeCommand() {
            this.Close();
        }
    }
}
