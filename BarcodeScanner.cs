using System;
using System.Text;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows.Forms;

namespace HoltEnvironments
{
    //Create the Event handler & arguments class to notify the Status of the Serial port object
    public delegate void SerialStatusMessageEventArgsHandler(object sender, SerialStatusMessageEventArgs e);

    public class SerialStatusMessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public class BarcodeScanner : INotifyPropertyChanged, IDisposable
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        private SerialPort pri_SerialPort = new SerialPort();

        public enum DataMode { Text, Hex };
        //Used with our current device: a BarcodeScanner
        private DataMode CurrentDataMode = DataMode.Text;

        //.ifd file
        //Presentation Name | Variable Name | type

        protected string pro_SerialPortName = "COM3";
        //Serial Port | PortName | string
        public string SerialPortName
        {
            get { return pro_SerialPortName; }
            set
            {
                string refOldValue = pro_SerialPortName;
                if (pro_SerialPortName != value)
                {
                    pro_SerialPortName = value;
                    NotifyPropertyChanged("SerialPortName");
                }
            }
        }
        protected int pro_BaudRate = 115200;
        //Baud Rate | BaudRate | integer  //Note: Changed from lowercase 'Baud rate'
        public int BaudRate
        {
            get { return pro_BaudRate; }
            set
            {
                int refOldValue = pro_BaudRate;
                if (pro_BaudRate != value)
                {
                    pro_BaudRate = value;
                    NotifyPropertyChanged("BaudRate");
                }
            }
        }

        //Product Id | PID | integer
        public int ProductId { get; set; }

        //Vendor Id | VID | integer //Note: the 'Id' text in the presentation name is inconsistent between this and Product Id, changed from 'ID'
        public int VendorId { get; set; }

        //Driver | Driver | string
        public string Driver { get; set; }

        //Data Bits | DataBits | integer //Note: Changed from lowercase 'Data bits'
        public int DataBits { get; set; }

        //Parity | Parity | integer
        public int Parity { get; set; }

        //the following are both read only?
        //Activity Log | OutputLog | string
        public string ActivityLog { get; set; }

        //serialList | serialList | string
        public string SerialList { get; set; }

        //trigger
        //codeScanned | Raised when a barcode has been scanned
        //public event EventHandler CodeScanned;
        public event SerialStatusMessageEventArgsHandler StatusMessage;
        private void RaiseStatusMessage(string msg_)
        {
            if (StatusMessage != null)
            {
                SerialStatusMessageEventArgs args = new SerialStatusMessageEventArgs() { Message = msg_ };
                StatusMessage(this, args);
            }
        }

        public event SerialStatusMessageEventArgsHandler CodeScanned;
        private void RaiseCodeScanned(string msg_)
        {
            if (CodeScanned != null)
            {
                SerialStatusMessageEventArgs args = new SerialStatusMessageEventArgs() { Message = msg_ };
                CodeScanned(this, args);
            }
        }

        public BarcodeScanner()
        {
            // When data is recieved through the port, call this method
            pri_SerialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            //error simple management
            pri_SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(comport_ErrorReceived);
        }

        public void OpenConnection()
        {
            bool error = false;

            // If the port is open, close it.
            if (pri_SerialPort.IsOpen) pri_SerialPort.Close();
            else
            {
                // Set the port's settings
                pri_SerialPort.BaudRate = this.BaudRate;
                pri_SerialPort.PortName = this.SerialPortName;

                try
                {
                    // Open the port
                    pri_SerialPort.Open();
                }
                catch (Exception) { error = true; }

                if (error) MessageBox.Show("Could not open the COM port.  Most likely it is already in use, has been removed, or is unavailable.", "COM Port Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

            if (pri_SerialPort.IsOpen)
            {
                //Notify success
                RaiseStatusMessage("Port " + pri_SerialPort.PortName + " opened with success");
            }
        }
        public void CloseConnection()
        {
            // If the port is open, close it.
            if (pri_SerialPort != null && pri_SerialPort.IsOpen) pri_SerialPort.Close();
        }

        #region callbacks
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // If the com port has been closed, do nothing
            if (!pri_SerialPort.IsOpen) return;

            // This method will be called when there is data waiting in the port's buffer

            // Determain which mode (string or binary) the user is in
            if (CurrentDataMode == DataMode.Text)
            {
                // Read all the data waiting in the buffer
                string data = pri_SerialPort.ReadExisting();

                // Display the text to the user in the terminal
                //Log(LogMsgType.Incoming, data);                
                //RaiseStatusMessage("Incoming: " + data);
                RaiseCodeScanned(data);
            }
            else
            {
                // Obtain the number of bytes waiting in the port's buffer
                int bytes = pri_SerialPort.BytesToRead;

                // Create a byte array buffer to hold the incoming data
                byte[] buffer = new byte[bytes];

                // Read the data from the port and store it in our buffer
                pri_SerialPort.Read(buffer, 0, bytes);

                // Show the user the incoming data in hex format
                //Log(LogMsgType.Incoming, ByteArrayToHexString(buffer));
                //RaiseStatusMessage("Incoming : " + ByteArrayToHexString(buffer));
                RaiseCodeScanned(ByteArrayToHexString(buffer));

            }
        }

        void comport_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            RaiseStatusMessage("Serial Port Error: " + e.EventType);
        }

        #endregion
        #region Converters

        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }

        public void Dispose()
        {
            this.CloseConnection();
        }

        #endregion
    }
}
