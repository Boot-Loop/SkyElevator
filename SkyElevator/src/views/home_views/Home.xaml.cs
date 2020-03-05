using System.Windows;
using System.Windows.Controls;

using SkyElevator.src.view_models;
using SkyElevator.src.views.inquiry_sheet_views;
using SkyElevator.src.views.progress_tracker_views.client;
using SkyElevator.src.views.progress_tracker_views.supplier;

using Core;

namespace SkyElevator.src.views.home_views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        //private MainViewModel mvm = new MainViewModel();
        public Home()
        {
            InitializeComponent();
            //this.DataContext = mvm;
            this.DataContext = ProjectManager.singleton.project_file.data.dirs.items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProjectManager.singleton.project_file.data.dirs.addFile("new file");
            DataContext = null;
            this.DataContext = ProjectManager.singleton.project_file.data.dirs.items;
            //Button but = (Button)sender;
            //if (but.Content.ToString() == "Inquiry Sheet") { mvm.AddItem(new InquirySheet()); }
            //else if (but.Content.ToString() == "Client") { mvm.AddItem(new ProgressTrackerClient()); }
            //else if (but.Content.ToString() == "Supplier") { mvm.AddItem(new ProgressTrackerSupplier()); }
            //mvm.AddItem(new InquirySheet());
        }
    }
}
