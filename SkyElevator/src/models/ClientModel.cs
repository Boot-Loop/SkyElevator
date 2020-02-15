using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data;
using Core;

namespace SkyElevator.src.models
{

    public interface IClientModelBuilder
    {
        void setClientName(string client_name);
        void setClientAddress(string client_address);
        void setClientCompany(string client_company);
        void setClientEmail(string client_email);
        void setClientPosition(string client_position);
        void setClientTelephoneNumber(string client_telephone_number);
        void setClientNIC(string client_nic);
        void setClientWebsite(string client_website);
        ClientModel GetClinetModel();
    }

    public class ClinetModelBuilder : IClientModelBuilder
    {
        private ClientModel _client_model = new ClientModel();

        public void setClientName(string client_name) {
            this._client_model.ClientName = client_name;
        }

        public void setClientAddress(string client_address) {
            this._client_model.ClientAddress = client_address;
        }

        public void setClientCompany(string client_company) {
            this._client_model.ClientCompany = client_company;
        }

        public void setClientEmail(string client_email) {
            this._client_model.ClientEmail = client_email;
        }

        public void setClientPosition(string client_position) {
            this._client_model.ClientPosition = client_position;
        }

        public void setClientTelephoneNumber(string client_telephone_number) {
            this._client_model.ClientTelephoneNumber = client_telephone_number;
        }

        public void setClientNIC(string client_nic) {
            this._client_model.ClientNIC = client_nic;
        }

        public void setClientWebsite(string client_website) {
            this._client_model.ClientWebsite = client_website;
        }

        public ClientModel GetClinetModel() {
            return _client_model;
        }    
    }
    public class ClientModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TextField _client_name                      = new TextField();
        private TextField _client_address                   = new TextField();
        private TextField _client_company                   = new TextField();
        private EmailField _client_email                    = new EmailField();
        private TextField _client_position                  = new TextField();
        private PhoneNumberField _client_telephone_number   = new PhoneNumberField();
        private NICField _client_nic                        = new NICField();
        private WebSiteField _client_website                = new WebSiteField();

        private string _client_email_error = "";

        public ClientModel() {

        }

        public string ClientName {
            get { return _client_name.value; }
            set { _client_name.value = value; onPropertyRaised("ClientName"); }
        }
        public string ClientAddress {
            get { return _client_address.value; }
            set { _client_address.value = value; onPropertyRaised("ClientAddress"); }
        }
        public string ClientCompany {
            get { return _client_company.value; }
            set { _client_company.value = value; onPropertyRaised("ClientCompany"); }
        }
        public string ClientEmail {
            get { return _client_email.value; }
            set {
                try { _client_email.value = value; _client_email_error = ""; onPropertyRaised("ClientEmail"); onPropertyRaised("ClientEmailError"); }
                catch (ValidationError err) { _client_email_error = "The email you set is error"; onPropertyRaised("ClientEmailError"); }
            }
        }
        public string ClientPosition {
            get { return _client_position.value; }
            set { _client_position.value = value;onPropertyRaised("ClientPosition"); }
        }
        public string ClientTelephoneNumber {
            get { return _client_telephone_number.value; }
            set { _client_telephone_number.value = value; onPropertyRaised("ClientTelephoneNumber"); }
        }
        public string ClientNIC {
            get { return _client_nic.value; }
            set { _client_nic.value = value; onPropertyRaised("ClientNIC"); }
        }
        public string ClientWebsite {
            get { return _client_website.value; }
            set { _client_website.value = value; onPropertyRaised("ClientWebsite"); }
        }

        public string ClientEmailError {
            get { return _client_email_error; }
            set { _client_email_error = value; onPropertyRaised("ClientEmailError"); }
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
