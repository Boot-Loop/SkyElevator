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
using PdfiumViewer;
using System.IO;
using TesterUI.Views.UserControls;

namespace TesterUI.Views.Windows
{
    /// <summary>
    /// Interaction logic for PDFViewerWindow.xaml
    /// </summary>
    public partial class PDFViewerWindow : Window
    {

        PdfiumViewer.PdfViewer pdf;
        public PDFViewerWindow()
        {
            InitializeComponent();
            var uc = new UserControl();
            uc.Background = Brushes.Red;
            pdff.Content = uc;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pdf = new PdfViewer();
            openfile(@"C:\Users\Lap.lk\Desktop\Hello1.pdf");

        }

        public void openfile(string path)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            var stream = new MemoryStream(bytes);
            PdfDocument pdfDocument = PdfDocument.Load(stream);
            pdf.Document = pdfDocument; 
            pdff.Content = pdf;
            Console.WriteLine( pdf.Document.ToString() );
        }
    }
}
