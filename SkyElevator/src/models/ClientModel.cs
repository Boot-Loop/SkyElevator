using Core.src.documents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyElevator.src.models
{

    public interface IClientModelBuilder
    {
        void setClientName(TextField client_name);
        void setClientAddress(TextField client_address);
        void setClientCompany(TextField client_company);
        void setClientEmail(TextField client_email);
        void setClientPosition(TextField client_position);
        void setClientTelephoneNumber(TextField client_telephone_number);
        void setClientNIC(TextField client_nic);
        void setClientWebsite(TextField client_website);
        ClientModel GetClinetModel();
    }

    public class ClinetModelBuilder : IClientModelBuilder
    {
        private ClientModel _client_model = new ClientModel();

        public void setClientName(TextField client_name) {
            this._client_model.ClientName = client_name;
        }

        public void setClientAddress(TextField client_address) {
            this._client_model.ClientAddress = client_address;
        }

        public void setClientCompany(TextField client_company) {
            this._client_model.ClientCompany = client_company;
        }

        public void setClientEmail(TextField client_email) {
            this._client_model.ClientEmail = client_email;
        }

        public void setClientPosition(TextField client_position) {
            this._client_model.ClientPosition = client_position;
        }

        public void setClientTelephoneNumber(TextField client_telephone_number) {
            this._client_model.ClientTelephoneNumber = client_telephone_number;
        }

        public void setClientNIC(TextField client_nic) {
            this._client_model.ClientNIC = client_nic;
        }

        public void setClientWebsite(TextField client_website) {
            this._client_model.ClientWebsite = client_website;
        }

        public ClientModel GetClinetModel() {
            return _client_model;
        }    
    }
    public class ClientModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TextField _client_name              = new TextField();
        private TextField _client_address           = new TextField();
        private TextField _client_company           = new TextField();
        private TextField _client_email             = new TextField();
        private TextField _client_position          = new TextField();
        private TextField _client_telephone_number  = new TextField();
        private TextField _client_nic               = new TextField();
        private TextField _client_website           = new TextField();

        public TextField ClientName {
            get { return _client_name; }
            set { _client_name = value; onPropertyRaised("ClientName"); }
        }
        public TextField ClientAddress {
            get { return _client_address; }
            set { _client_address = value; onPropertyRaised("ClientAddress"); }
        }
        public TextField ClientCompany {
            get { return _client_company; }
            set { _client_company = value; onPropertyRaised("ClientCompany"); }
        }
        public TextField ClientEmail {
            get { return _client_email; }
            set { _client_email = value; onPropertyRaised("ClientEmail"); }
        }
        public TextField ClientPosition {
            get { return _client_position; }
            set { _client_position = value; onPropertyRaised("ClientPosition"); }
        }
        public TextField ClientTelephoneNumber {
            get { return _client_telephone_number; }
            set { _client_telephone_number = value; onPropertyRaised("ClientTelephoneNumber"); }
        }
        public TextField ClientNIC {
            get { return _client_nic; }
            set { _client_nic = value; onPropertyRaised("ClientNIC"); }
        }
        public TextField ClientWebsite {
            get { return _client_website; }
            set { _client_website = value; onPropertyRaised("ClientWebsite"); }
        }
        public ClientModel() {

        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
