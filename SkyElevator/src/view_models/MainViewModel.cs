using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using SkyElevator.src.views.inquiry_sheet_views;
using SkyElevator.src.views.progress_tracker_views.supplier;

namespace SkyElevator.src.view_models
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<TabItemViewModel>();

            this.AddCommand = new RelayCommand(AddItem);
        }

        public ObservableCollection<TabItemViewModel> Items { get; private set; }

        public TabItemViewModel SelectedItem { get; set; }

        public RelayCommand AddCommand { get; set; }

        public void AddItem()
        {
            ProgressTrackerSupplier PTS = new ProgressTrackerSupplier();
            var nextIndex = this.Items.Count + 1;
            var newItem = new TabItemViewModel("Added tab item " + nextIndex, PTS, this.CloseItem);
            this.Items.Add(newItem);
            this.SelectedItem = newItem;
            RaisePropertyChanged("SelectedItem");
        }

        private void CloseItem(TabItemViewModel vm)
        {
            this.Items.Remove(vm);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
