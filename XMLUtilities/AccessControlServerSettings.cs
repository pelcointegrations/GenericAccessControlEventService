using System.Xml.Serialization;

namespace XMLUtilities
{
    [XmlRoot()]
    public class AccessControlServerSettings
    {
        private string _acsAddress = string.Empty;
        private int _acsPort = 9999;
        private bool _useSSL = false;
        private string _certificate = string.Empty;

        public string Address
        {
            get { return _acsAddress; }
            set 
            { 
                if (value != string.Empty) 
                    _acsAddress = value; 
            }
        }

        public int Port
        {
            get { return _acsPort; }
            set { _acsPort = value; }
        }

        public bool UseSSL
        {
            get { return _useSSL; }
            set
            {
                _useSSL = value;
            }
        }

        public string Certificate
        {
            get { return _certificate; }
            set 
            { 
                if (value != string.Empty) 
                    _certificate = value; 
            }
        }
    }
}
