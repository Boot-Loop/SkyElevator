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

namespace SkyElevator.src.ui.sub_views
{
    /// <summary>
    /// Interaction logic for TrackerClient.xaml
    /// </summary>
    public partial class TrackerClient : UserControl
    {
        List<dynamic> Paymenttracker = new List<dynamic>(); 
        public TrackerClient()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            DataAddingGrid.Height = 40;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataAddingGrid.Height = 0;
            PaymentTracker PT = new PaymentTracker();
            PT.Date = date.Text;
            PT.Amount = amount.Text;
            PT.Payment_Method = paymentmethod.Text;
            PT.Total = total.Text;

            PaymentTracker.Items.Add(PT);
        }
          
    }

    public class PaymentTracker
    {
        public string Date { get; set; }
        public string Amount { get; set; }
        public string Payment_Method { get; set; }
        public string Total { get; set; }
    }
}
