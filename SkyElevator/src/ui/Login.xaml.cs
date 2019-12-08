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

namespace SkyElevator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            SetQuoteAndAuthor();
        }

        private void SetQuoteAndAuthor()
        {
            string[,] QuotesAndAuthor = new string[5, 2] { {"There are no secrets to success. It is the result of preparation, hard work, and learning from failure.", "Colin Powell" }, 
                { "If people like you, they'll listen to you, but if they trust you, they'll do business with you.", "Zig Ziglar" }, 
                { "Good business leaders create a vision, articulate the vision, passionately own the vision, and relentlessly drive it to completion.", "Jack Welch" }, 
                { "The first responsibility of a leader is to define reality. The last is to say thank you. In between, the leader is a servant.", "Max de Pree" }, 
                { "The competitor to be feared is one who never bothers about you at all, but goes on making his own business better all the time.", "Henry Ford" } };
            Random random = new Random();
            int number = random.Next(0, 5);
            QuoteLabel.Text = QuotesAndAuthor[number, 0];
            AuthorLabel.Content = QuotesAndAuthor[number, 1];
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
