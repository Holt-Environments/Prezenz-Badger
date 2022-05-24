using System;
using System.ComponentModel;
using HoltEvironments;

namespace HoltEnvironments
{
    public class PrezenzBadger : INotifyPropertyChanged
    {

    #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

    #endregion INotifyPropertyChanged

    #region Private Attributes

        private BarcodeScanner _barcodeScanner;
        private string _serialPort = "";
        private int _baudRate = 0;

        private string _firstName = "";
        private string _lastName = "";
        private string _title = "";
        private string _company = "";
        private string _email = "";
        private string _phone = "";

    #endregion Private Attributes

    #region Properties

        public string SerialPortName
        {
            get { return _serialPort; }
            set
            {
                if (_serialPort != value)
                {
                    _serialPort = value;
                    NotifyPropertyChanged("SerialPortName");
                }
            }
        }

        public int BaudRate
        {
            get { return _baudRate; }
            set
            {
                if (_baudRate != value)
                {
                    _baudRate = value;
                    NotifyPropertyChanged("BaudRate");
                }
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string Company
        {
            get { return _company; }
            set
            {
                if (_company != value)
                {
                    _company = value;
                    NotifyPropertyChanged("Company");
                }
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    NotifyPropertyChanged("Phone");
                }
            }
        }

        #endregion Properties

        #region Events

        //public delegate void DataRetrievedFromScanHandler(object sender, LeadInfo e);
        //public event DataRetrievedFromScanHandler DataRetrievedFromScan;

        public event EventHandler DataRetrievedFromScan;

    #endregion Events

    #region Public methods

        public void Connect()
        {
            _barcodeScanner = new BarcodeScanner
            {
                SerialPortName = SerialPortName,
                BaudRate = BaudRate
            };

            _barcodeScanner.CodeScanned += HandleCodeScanned;
            _barcodeScanner.StatusMessage += HandleStatusMessage;

            _barcodeScanner.OpenConnection();
        }

        public void Disconnect()
        {
            _barcodeScanner.CloseConnection();
        }

    #endregion Public methods

    #region Private methods
        private void HandleCodeScanned(object sender, SerialStatusMessageEventArgs e)
        {
            IBadgerTranscoder _bad_trans = new ScanToClassConverter();
            ApiResponse _trigger_return_data = _bad_trans.GetBadgeUserData(e.Message);
            LeadInfo _data = _trigger_return_data.LeadInfo;

            //DataRetrievedFromScan(this, data);

            FirstName = _data.FirstName;
            LastName = _data.LastName;
            Title = _data.Title;
            Company = _data.Company;
            Email = _data.Email;
            Phone = _data.Phone;

            DataRetrievedFromScan(this, null);
        }

        private void HandleStatusMessage(object sender, EventArgs e)
        {
            // e.Message
        }

    #endregion Private methods
    }
}
