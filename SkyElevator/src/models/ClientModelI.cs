using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Models;
using Core.Data;
using Core;

namespace SkyElevator.src.models
{
    public class ClientModelI : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ClientModel _client_model = new ClientModel();

        public ClientModelI(string name) {
            _client_model.name.value = name;
        }

        public ClientModelI() {

        }
        public ClientModel ClientModel { get { return _client_model; } }

        public string ClientName {
            get { return _client_model.name.value; }
            set { _client_model.name.value = value; onPropertyRaised("ClientName"); }
        }
        public string ClientAddress {
            get { return _client_model.address.value; }
            set { _client_model.address.value = value; onPropertyRaised("ClientAddress"); }
        }
        public string ClientCompany {
            get { return _client_model.company.value; }
            set { _client_model.company.value = value; onPropertyRaised("ClientCompany"); }
        }
        public string ClientEmail {
            get { return _client_model.email.value; }
            set { _client_model.email.value = value; onPropertyRaised("ClientEmail"); }
        }
        public string ClientPosition {
            get { return _client_model.position.value; }
            set { _client_model.position.value = value;onPropertyRaised("ClientPosition"); }
        }
        public string ClientTelephoneNumber {
            get { return _client_model.telephone.value; }
            set { _client_model.telephone.value = value; onPropertyRaised("ClientTelephoneNumber"); }
        }
        public string ClientNIC {
            get { return _client_model.nic.value; }
            set { _client_model.nic.value = value; onPropertyRaised("ClientNIC"); }
        }
        public string ClientWebsite {
            get { return _client_model.website.value; }
            set { _client_model.website.value = value; onPropertyRaised("ClientWebsite"); }
        }

        public override string ToString()
        {
            return _client_model.name.value;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
