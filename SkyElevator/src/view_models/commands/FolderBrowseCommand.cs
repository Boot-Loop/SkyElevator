using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace SkyElevator.src.view_models.commands
{
    public class FolderBrowseCommand : ICommand, INotifyPropertyChanged
    {
        private string _folder_path;

        public event PropertyChangedEventHandler PropertyChanged;

        public string FolderPath {
            get { return _folder_path; }
            set { _folder_path = value; onPropertyRaised("FolderPath"); }
        }

        public FolderBrowseCommand() {
            FolderPath = @"C:\";
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter) {
            return true;
        }
        public void Execute(object parameter) {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK) FolderPath = folderBrowserDialog.SelectedPath;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
