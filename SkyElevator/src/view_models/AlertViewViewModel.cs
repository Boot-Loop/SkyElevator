using System;
using System.ComponentModel;
using SkyElevator.src.view_models.commands;

namespace SkyElevator.src.view_models
{
    public class AlertViewViewModel: INotifyPropertyChanged
    {
        public class Button
        {
            public bool visible = true;
            public string name;
            public Action button_action;
        }
        
        public enum AlertViewType { INFO, OK, WARNING, ERROR }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private string _title;
        private string _message;
        private string _display_image;
        private Button _button_1 = new Button();
        private Button _button_2 = new Button();
        private Button _button_3 = new Button();
        private AlertViewType _type;
        private AlertViewRelayCommand _close_command;
        private AlertViewRelayCommand _button_1_command;
        private AlertViewRelayCommand _button_2_command;
        private AlertViewRelayCommand _button_3_command;

        public string Title {
            get { return _title; }
            set { _title = value; onPropertyRaised("Title"); }
        }
        public string Message {
            get { return _message; }
            set { _message = value; onPropertyRaised("Message"); }
        }
        public string DisplayedImage {
            get { return _display_image; }
            set { _display_image = value; }
        }
        public bool ButtonTwoVisibility {
            get { return _button_2.visible; }
            set { _button_2.visible = value; }
        }
        public bool ButtonThreeVisibility {
            get { return _button_3.visible; }
            set { _button_3.visible = value; }
        }
        public string ButtonOneName {
            get { return _button_1.name; }
            set { _button_1.name = value; }
        }
        public string ButtonTwoName {
            get { return _button_2.name; }
            set { _button_2.name = value; }
        }
        public string ButtonThreeName {
            get { return _button_3.name; }
            set { _button_3.name = value; }
        }
        public AlertViewRelayCommand CloseCommand {
            get { return _close_command; }
            set { _close_command = value; }
        }
        public AlertViewRelayCommand ButtonOneCommand {
            get { return _button_1_command; }
            set { _button_1_command = value; }
        }
        public AlertViewRelayCommand ButtonTwoCommand {
            get { return _button_2_command; }
            set { _button_2_command = value; }
        }
        public AlertViewRelayCommand ButtonThreeCommand {
            get { return _button_3_command; }
            set { _button_3_command = value; }
        }

        public AlertViewViewModel(string title, string message, AlertViewType type, Button button1, Action close, Button button2 = null, Button button3 = null) {
            this._title     = title;
            this._message   = message;
            this._type      = type;
            this._button_1  = button1;
            this._button_2  = button2;
            this._button_3  = button3;
            switch (_type) {
                case AlertViewType.INFO: {
                        DisplayedImage = @"/res/images/info.png";
                        break;
                    }
                case AlertViewType.OK: {
                        DisplayedImage = @"/res/images/ok.png";
                        break;
                    }
                case AlertViewType.WARNING: {
                        DisplayedImage = @"/res/images/warning.png";
                        break;
                    }
                case AlertViewType.ERROR: {
                        DisplayedImage = @"/res/images/error.png";
                        break;
                    }
            }
            if (button2 == null && button3 == null) {
                _button_2 = new Button();
                _button_3 = new Button();
                ButtonTwoVisibility = false;
                ButtonThreeVisibility = false;
            }
            else if (button3 == null) {
                _button_3 = new Button();
                ButtonThreeVisibility = false;
            }
            else if (button2 == null) {
                _button_2 = new Button();
                ButtonTwoVisibility = false;
            }

            this.CloseCommand = new AlertViewRelayCommand(() => close());

            if (_button_1.button_action == null) this.ButtonOneCommand = new AlertViewRelayCommand(() => close());
            else this.ButtonOneCommand = new AlertViewRelayCommand(() => _button_1.button_action());

            if (_button_2.button_action == null) this.ButtonTwoCommand = new AlertViewRelayCommand(() => close());
            else this.ButtonTwoCommand = new AlertViewRelayCommand(() => _button_2.button_action());

            if (_button_3.button_action == null) this.ButtonThreeCommand = new AlertViewRelayCommand(() => close());
            else this.ButtonThreeCommand = new AlertViewRelayCommand(() => _button_3.button_action());
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
