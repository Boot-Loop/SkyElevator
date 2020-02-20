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

        public ModelAPI<ClientModel> api { get; } = new ModelAPI<ClientModel>(null, ModelApiMode.MODE_CREATE);

        public ClientModelI(string name) {
            api.model.name.value = name;
        }

        public ClientModelI() {

        }
        public ClientModel ClientModel { get { return api.model; } }

        public string ClientName {
            get { return api.model.name.value; }
            set { api.model.name.value = value; onPropertyRaised("ClientName"); }
        }
        public string ClientAddress {
            get { return api.model.address.value; }
            set { api.model.address.value = value; onPropertyRaised("ClientAddress"); }
        }
        public string ClientCompany {
            get { return api.model.company.value; }
            set { api.model.company.value = value; onPropertyRaised("ClientCompany"); }
        }
        public string ClientEmail {
            get { return api.model.email.value; }
            set { api.model.email.value = value; onPropertyRaised("ClientEmail"); }
        }
        public string ClientPosition {
            get { return api.model.position.value; }
            set { api.model.position.value = value;onPropertyRaised("ClientPosition"); }
        }
        public string ClientTelephoneNumber {
            get { return api.model.telephone.value; }
            set { api.model.telephone.value = value; onPropertyRaised("ClientTelephoneNumber"); }
        }
        public string ClientNIC {
            get { return api.model.nic.value; }
            set { api.model.nic.value = value; onPropertyRaised("ClientNIC"); }
        }
        public string ClientWebsite {
            get { return api.model.website.value; }
            set { api.model.website.value = value; onPropertyRaised("ClientWebsite"); }
        }

        public override string ToString()
        {
            return api.model.name.value;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
