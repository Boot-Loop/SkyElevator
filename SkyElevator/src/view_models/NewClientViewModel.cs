using SkyElevator.src.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyElevator.src.view_models
{
    public class NewClientViewModel :INotifyPropertyChanged
    {
        private ClientModelI _client_model_i = new ClientModelI();

        public event PropertyChangedEventHandler PropertyChanged;

        public ClientModelI ClientModelI {
            get { return _client_model_i; }
            set { _client_model_i = value; onPropertyRaised("ClientModelI"); }
        }

        public void createNewClient() {
            Core.Application.getSingleton().addClient(_client_model_i.ClientModel);
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
