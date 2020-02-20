using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Files;

namespace SkyElevator.src.view_models
{
    public class OpenProjectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ProgrameData.ProjectViewData> RecentProjectDatas { 
            get { return Core.Application.getSingleton().getRecentProjects(); }
        }

        int index = 0;
        public string Name {
            get {
                var list = Core.Application.getSingleton().getRecentProjects();
                return (list.Count>0)?list[index++].name:""; 
            }
        }

        public OpenProjectViewModel()
        {
            
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
