using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Files;
using SkyElevator.src.view_models.commands;
using CoreApp = Core.Application;

namespace SkyElevator.src.view_models
{
    public class RecentProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public static List<RecentProjectModel> GetItems()
        {
            return new List<RecentProjectModel>()
        {
            new RecentProjectModel()
            {
                Id = 1,
                Name = "Item 1",
                Description = "Item 1 Description",
                Price = 120.00
            },
            new RecentProjectModel()
            {
                Id = 2,
                Name = "Item 2",
                Description = "Item 2 Description",
                Price = 360.00
            },
            new RecentProjectModel()
            {
                Id = 3,
                Name = "Item 3",
                Description = "Item 3 Description",
                Price = 590.00
            }
        };
        }

        //public string DisplayName { get; set; }
        //public string Path { get; set; }

        //private List<ProgrameData.ProjectViewData> _recent_project_datas = new List<ProgrameData.ProjectViewData>();

        //public List<ProgrameData.ProjectViewData> RecentProjectDatas {
        //    get { return CoreApp.getSingleton().getRecentProjects(); }
        //    //set { _recent_project_datas = value; }
        //}

        //int index = 0;
        //public string Name
        //{
        //    get
        //    {
        //        var list = Core.Application.getSingleton().getRecentProjects();
        //        return (list.Count > 0) ? list[index++].name : "";
        //    }
        //}

        //    public RecentProjectModel() {
        //    //    foreach (ProgrameData.ProjectViewData project_view_data in CoreApp.getSingleton().getRecentProjects()) {
        //    //    RecentProjectDatas.Add(project_view_data);
        //    //    DisplayName = string.Format("{0} ({1})", project_view_data.name, project_view_data.client_name);
        //    //    Path = project_view_data.path;
        //    //}
        //}
    }

    public class OpenProjectViewModel: INotifyPropertyChanged
    {
        //public RelayCommand SelectCommand { get; set; }
        ////public RecentProjectModel RecentProjectModel { get; set; }
        //public bool NoRecentProjectsVisibility { get; set; }
        //public int SelectedIndex { get; set; }

        ////private List<ProgrameData.ProjectViewData> _recent_project_datas = new List<ProgrameData.ProjectViewData>();

        ////public List<ProgrameData.ProjectViewData> RecentProjectDatas
        ////{
        ////    get { return CoreApp.getSingleton().getRecentProjects(); }
        ////    //set { _recent_project_datas = value; }
        ////}

        //public List<RecentProjectModel> RecentProjectDatas = new List<RecentProjectModel>() { new RecentProjectModel() { DisplayName = "asd" }, new RecentProjectModel() { DisplayName = "asd" } };

        public RelayCommand ItemSelectedCommand { get; private set; }

        public int SelectedIndex { get; set; }

        public List<RecentProjectModel> Items { get; private set; }

        public OpenProjectViewModel() {
            //RecentProjectModel = new RecentProjectModel();
            //if (RecentProjectModel.RecentProjectDatas.Count == 0) NoRecentProjectsVisibility = true;
            //else NoRecentProjectsVisibility = false;
            ItemSelectedCommand = new RelayCommand(testFunction);
            Items = RecentProjectModel.GetItems();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void testFunction(object parameter) {
            Console.WriteLine("Hello Azeem");
            Console.WriteLine(SelectedIndex);
        }

        private void onPropertyRaised(string prop)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
