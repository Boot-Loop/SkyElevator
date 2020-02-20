using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;

namespace SkyElevator.src.view_models
{
    public class TabItemViewModel
    {
        public TabItemViewModel(string header, UserControl content, Action<TabItemViewModel> onClose)
        {
            this.Header = header;
            this.Content = content;
            this.CloseCommand = new RelayCommand(x => onClose(this));
        }
        public string Header { get; set; }
        public UserControl Content { get; set; }

        public RelayCommand CloseCommand { get; set; }
    }
}
