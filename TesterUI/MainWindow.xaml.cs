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
using TesterUI.Views.Windows;
using TesterUI.Views.UserControls;
using System.IO;
using TesterUI.Models.TreeView;

using System.Timers;
using System.Xml;

namespace TesterUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DateTime dclick_time = new DateTime();

        public MainWindow()
        {
            InitializeComponent();
            
            

            var itemProvider = new ItemProvider();

            var items = itemProvider.GetItems(@"C:\Users\Lap.lk\Desktop\dev\C#\SkyElevator\SkyElevator\res\dir.xml");

            DataContext = items;
        }

        private void NewProjectClick(object sender, RoutedEventArgs e)
        {
            NewProjectWindow NPW = new NewProjectWindow();
            NPW.Owner = this;
            
            NPW.ShowDialog();
        }
        List<object> TabPath = new List<object>();
        private void StackPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DateTime.Now.Subtract(dclick_time).TotalSeconds < .5) {                 
                StackPanel sp = (StackPanel)sender;
                Console.WriteLine(sp.Tag.ToString());
                var Tb = sp.Children[1] as TextBlock;
                if (TabPath.Contains(sp.Tag)) { 
                    MainTab.SelectedIndex = TabPath.IndexOf(sp.Tag);
                }
                else
                {AddTab(Tb.Text.ToString(), sp.Tag, Tb.ToolTip.ToString());
                TabPath.Add(sp.Tag);
                    MainTab.SelectedIndex = TabPath.IndexOf(sp.Tag);
                }

             
            }
            dclick_time = DateTime.Now;

        }

        private void StackPanel_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            StackPanel SP = (StackPanel)sender;

            if (SP.ContextMenu.Items.Count == 0)
            {
                ContextMenu menu = new ContextMenu();
                List<string> ContextMenuItem = new List<string>() { "Copy", "Paste","Open" };
                for (int i = 0; i < ContextMenuItem.Count; i++)
                {
                    SP.ContextMenu = menu;
                    MenuItem item = new MenuItem();
                    item.Header = ContextMenuItem[i];

                    item.Click += ContextMenuClick;

                    menu.Items.Add(item);
                }
            }
            else { }
            
        }

           

        private void TextBlock_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
                TextBlock TB = (TextBlock)sender;
            
            if ((TB.Text.ToString() == "client" && TB.ContextMenu.Items.Count == 0) || (TB.Text.ToString() == "suppliers" && TB.ContextMenu.Items.Count == 0))
            {
                    ContextMenu menu = new ContextMenu();
                TB.ContextMenu.Visibility = Visibility.Visible;
                //TB.ContextMenu.Visibility = Visibility.Visible;
                List<string> ContextMenuItem = new List<string>() { "Add","Import"};
                for(int i=0; i<ContextMenuItem.Count; i++)
                {

                    TB.ContextMenu = menu;
                    MenuItem item = new MenuItem();
                    item.Header = ContextMenuItem[i];
                    item.Tag = TB.Tag;
                    item.Click += ContextMenuClick;
                    menu.Items.Add(item);
                }
                

            }
            else {
            }
                Console.WriteLine(TB.Tag.ToString());
        }



        //private void ContextMenu()
        //{
        //    ContextMenu CM = new ContextMenu();
        //    CM.Items.Add("Add Item");
        //    CM.Items.Add("Import Item");

        //}
        private void ContextMenuClick(object sender, RoutedEventArgs e)
        {
            MenuItem MI = (MenuItem)sender;
            Console.WriteLine(MI.Header);
            if(MI.Header.ToString() == "Add")
            {
                FileAddWindow FAW = new FileAddWindow();
                FAW.Owner = this;
                FAW.ShowDialog();
            }
        }


        private void AddTab(string header, object tag, string path)
        {
            StackPanel sp = new StackPanel();
            Button But = new Button();
            Label lbl = new Label();            
            TabItem TI = new TabItem();

            But.Width = 10;
            But.Height = 10;
            sp.Tag = tag;
            sp.Orientation = Orientation.Horizontal;
            lbl.Content = header;
            But.Click += DeleteTabButton;
            But.Tag = tag;
            
            sp.Children.Add(lbl);
            sp.Children.Add(But);

            TI.Header = sp;

            TI.ToolTip = path;
            InquirySheetWord ISW = new InquirySheetWord();
            TI.Content = ISW;
            

            MainTab.Items.Add(TI);
           
        }

        private void DeleteTabButton(object sender, RoutedEventArgs e)
        {
            Button But = (Button)sender;
            DeleteTab(But.Tag);
            
            
        }

        public void DeleteTab(object tag)
        {
              
            foreach(TabItem tb in MainTab.Items)
            {
                StackPanel sp = tb.Header as StackPanel;
                if(sp.Tag == tag) {                    
                    MainTab.Items.Remove(tb);
                    break;
                }
                else { }
            }

            TabPath.Remove(tag);
            
        }

        


    }
}
