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

namespace SkyElevator.src.ui.progress_tracker.client
{
    /// <summary>
    /// Interaction logic for Progress_tracker_client.xaml
    /// </summary>
    public partial class ProgressTrackerClient : UserControl
    {

        //List<dynamic> payment_tracker = new List<dynamic>() {"Hello", "First", "Second", "Third" };
        public ProgressTrackerClient()
        {
            InitializeComponent();
            PaymentTracker PT = new PaymentTracker();
            PT.Amount = "1000";
            PaymentTracker PT2 = new PaymentTracker();
            PT2.Date = "12/23/42";
            payment_tracker.Items.Add(PT);
            payment_tracker.Items.Add(PT2); payment_tracker.Items.Add(PT);
            payment_tracker.Items.Add(PT2); payment_tracker.Items.Add(PT);
            payment_tracker.Items.Add(PT2); payment_tracker.Items.Add(PT);
            payment_tracker.Items.Add(PT2); payment_tracker.Items.Add(PT);
            payment_tracker.Items.Add(PT2); payment_tracker.Items.Add(PT);
            payment_tracker.Items.Add(PT2);

        }


        public class PaymentTracker
        {
            public string Date { get; set; }
            public string Amount { get; set; }
            public string Payment_Method { get; set; }
            public string Total { get; set; }
        }
    }
}
