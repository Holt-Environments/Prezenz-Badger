using System;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Windows.Threading;

namespace Prezenz_Badger
{
    public class Prezenz_Badger : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        /**
         * Variable for the device id, as well as the object used to 
         * get the variable value and set/update it
         */
        private String device_id = "";
        public String DeviceId
        {
            get
            {
                return device_id;
            }
            set
            {
                if (device_id != value)
                {
                    device_id = value;
                    NotifyPropertyChanged("DeviceId");
                }
            }
        }

        private String token = "";
        public String Token
        {
            get
            {
                return token;
            }
            set
            {
                if (token != value)
                {
                    token = value;
                    NotifyPropertyChanged("Token");
                }
            }
        }

        private String scan_code = "";
        public String ScanCode
        {
            get
            {
                return scan_code;
            }
            set
            {
                if (scan_code != value)
                {
                    // if the last char of the scancode is \u00f1 we need to cut it off
                    if (value.EndsWith("\u001f"))
                    {
                        scan_code = value.Remove(value.Length - 1, 1);
                    }
                    else
                    {
                        scan_code = value;
                    }

                    NotifyPropertyChanged("ScanCode");
                }
            }
        }

        private String response_badge_id = "";
        public String ResponseBadgeId
        {
            get
            {
                return response_badge_id;
            }
            set
            {
                if (response_badge_id != value)
                {
                    response_badge_id = value;
                    NotifyPropertyChanged("ResponseBadgeId");
                }
            }
        }

        private String response_member_id = "";
        public String ResponseMemberId
        {
            get
            {
                return response_member_id;
            }
            set
            {
                if (response_member_id != value)
                {
                    response_member_id = value;
                    NotifyPropertyChanged("ResponseMemberId");
                }
            }
        }


        private String response_name_first = "";
        public String ResponseNameFirst
        {
            get
            {
                return response_name_first;
            }
            set
            {
                if (response_name_first != value)
                {
                    response_name_first = value;
                    NotifyPropertyChanged("ResponseNameFirst");
                }
            }
        }

        private String response_title = "";
        public String ResponseTitle
        {
            get
            {
                return response_title;
            }
            set
            {
                if (response_title != value)
                {
                    response_title = value;
                    NotifyPropertyChanged("ResponseTitle");
                }
            }
        }

        private String response_company_name = "";
        public String ResponseCompanyName
        {
            get
            {
                return response_company_name;
            }
            set
            {
                if (response_company_name != value)
                {
                    response_company_name = value;
                    NotifyPropertyChanged("ResponseCompanyName");
                }
            }
        }

        private String response_address_1 = "";
        public String ResponseAddress1
        {
            get
            {
                return response_address_1;
            }
            set
            {
                if (response_address_1 != value)
                {
                    response_address_1 = value;
                    NotifyPropertyChanged("ResponseAddress1");
                }
            }
        }

        private String response_address_2 = "";
        public String ResponseAddress2
        {
            get
            {
                return response_address_2;
            }
            set
            {
                if (response_address_2 != value)
                {
                    response_address_2 = value;
                    NotifyPropertyChanged("ResponseAddress2");
                }
            }
        }

        private String response_address_city = "";
        public String ResponseAddressCity
        {
            get
            {
                return response_address_city;
            }
            set
            {
                if (response_address_city != value)
                {
                    response_address_city = value;
                    NotifyPropertyChanged("ResponseAddressCity");
                }
            }
        }

        private String response_address_state = "";
        public String ResponseAddressState
        {
            get
            {
                return response_address_state;
            }
            set
            {
                if (response_address_state != value)
                {
                    response_address_state = value;
                    NotifyPropertyChanged("ResponseAddressState");
                }
            }
        }

        private String response_address_postal = "";
        public String ResponseAddressPostal
        {
            get
            {
                return response_address_postal;
            }
            set
            {
                if (response_address_postal != value)
                {
                    response_address_postal = value;
                    NotifyPropertyChanged("ResponseAddressPostal");
                }
            }
        }

        private String response_address_country = "";
        public String ResponseAddressCountry
        {
            get
            {
                return response_address_country;
            }
            set
            {
                if (response_address_country != value)
                {
                    response_address_country = value;
                    NotifyPropertyChanged("ResponseAddressCountry");
                }
            }
        }

        private String response_phone = "";
        public String ResponsePhone
        {
            get
            {
                return response_phone;
            }
            set
            {
                if (response_phone != value)
                {
                    response_phone = value;
                    NotifyPropertyChanged("ResponsePhone");
                }
            }
        }

        private String response_email = "";
        public String ResponseEmail
        {
            get
            {
                return response_email;
            }
            set
            {
                if (response_email != value)
                {
                    response_email = value;
                    NotifyPropertyChanged("ResponseEmail");
                }
            }
        }

        private String response_registration_type = "";
        public String ResponseRegistrationType
        {
            get
            {
                return response_registration_type;
            }
            set
            {
                if (response_registration_type != value)
                {
                    response_registration_type = value;
                    NotifyPropertyChanged("ResponseRegistrationType");
                }
            }
        }

        private String response_name_last = "";
        public String ResponseNameLast
        {
            get
            {
                return response_name_last;
            }
            set
            {
                if (response_name_last != value)
                {
                    response_name_last = value;
                    NotifyPropertyChanged("ResponseNameLast");
                }
            }
        }

        private String response_error_msg = "";
        public String ResponseErrorMsg
        {
            get
            {
                return response_error_msg;
            }
            set
            {
                if (response_error_msg != value)
                {
                    response_error_msg = value;
                    NotifyPropertyChanged("ResponseErrorMsg");
                }
            }
        }


        private String response_error = "";
        public String ResponseError
        {
            get
            {
                return response_error;
            }
            set
            {
                if (response_error != value)
                {
                    response_error = value;
                    NotifyPropertyChanged("ResponseError");
                }
            }
        }

        private static bool is_running = false;

        public event EventHandler RequestComplete;
        public event EventHandler AlertWait;
        public event EventHandler RequestError;

        public static void DelayAction(int millisecond, Action action)
        {
            var timer = new DispatcherTimer();
            timer.Tick += delegate

            {
                action.Invoke();
                timer.Stop();
            };

            timer.Interval = TimeSpan.FromMilliseconds(millisecond);
            timer.Start();
        }

        public void DoGetRequest(string _scan_code)
        {
            if (is_running)
            {
                AlertWait(this, null);
                return;
            }

            is_running = true;

            if (_scan_code.EndsWith("\u001f"))
            {
                _scan_code = _scan_code.Remove(_scan_code.Length - 1, 1);
            }

            // https://ws.expoleads.net/scan?deviceId=95BBF370ED1A80B193AF522C556C752B&token=E0591C28-ECCC-47F2-841A-025181DBED58&scanCode=59278445
            String uri = "https://ws.expoleads.net/scan?deviceId=" + DeviceId + "&token=" + Token + "&scanCode=" + _scan_code;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                String read_text = reader.ReadToEnd();
                QuickType.ExpoleadsApiResponse expoleadsApiResponse = QuickType.ExpoleadsApiResponse.FromJson(read_text);
                try
                {
                    ResponseBadgeId = expoleadsApiResponse.Data.Attendee.BadgeId.ToString();
                    ResponseNameFirst = expoleadsApiResponse.Data.Attendee.NameFirst.ToString();
                    ResponseMemberId = expoleadsApiResponse.Data.Attendee.MemberId.ToString();
                    ResponseNameLast = expoleadsApiResponse.Data.Attendee.NameLast.ToString();
                    ResponseTitle = expoleadsApiResponse.Data.Attendee.Title.ToString();
                    ResponseCompanyName = expoleadsApiResponse.Data.Attendee.CompanyName.ToString();
                    ResponseAddress1 = expoleadsApiResponse.Data.Attendee.Address1.ToString();
                    ResponseAddress2 = expoleadsApiResponse.Data.Attendee.Address2.ToString();
                    ResponseAddressCity = expoleadsApiResponse.Data.Attendee.AddressCity.ToString();
                    ResponseAddressState = expoleadsApiResponse.Data.Attendee.AddressState.ToString();
                    ResponseAddressPostal = expoleadsApiResponse.Data.Attendee.AddressPostal.ToString();
                    ResponseAddressCountry = expoleadsApiResponse.Data.Attendee.AddressCountry.ToString();
                    ResponsePhone = expoleadsApiResponse.Data.Attendee.Phone.ToString();
                    ResponseEmail = expoleadsApiResponse.Data.Attendee.Email.ToString();
                    ResponseRegistrationType = expoleadsApiResponse.Data.Attendee.RegistrationType.ToString();
                    ResponseErrorMsg = "";
                    ResponseError = "";
                    RequestComplete(this, null);
                }
                catch (Exception e)
                {
                    ResponseBadgeId = "";
                    ResponseNameFirst = "";
                    ResponseMemberId = "";
                    ResponseNameLast = "";
                    ResponseTitle = "";
                    ResponseCompanyName = "";
                    ResponseAddress1 = "";
                    ResponseAddress2 = "";
                    ResponseAddressCity = "";
                    ResponseAddressState = "";
                    ResponseAddressPostal = "";
                    ResponseAddressCountry = "";
                    ResponsePhone = "";
                    ResponseEmail = "";
                    ResponseRegistrationType = "";
                    ResponseErrorMsg = expoleadsApiResponse.ErrorText.ToString();
                    ResponseError = expoleadsApiResponse.ErrorCode.ToString();
                    RequestError(this, null);
                }
            }

            DelayAction(5000, () => { is_running = false; });
        }
    }
}
