using System;
using System.Text;
using System.Xml.Serialization;

namespace XMLUtilities
{
    [XmlRoot()]
    public class ACSSettings
    {
        private string _hostUrl = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _changed = false;

        public string HostUrl
        {
            get { return _hostUrl; }
            set 
            { 
                if (value != string.Empty) 
                    _hostUrl = value; 
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (value != string.Empty)
                {
                    if (CryptoSvc.IsEncoded(value))
                    {
                        _username = value;
                    }
                    else  // not yet encoded
                    {
                        if (value != string.Empty)
                        {
                            try
                            {
                                byte[] decodedBytes = Convert.FromBase64String(value);
                                _username = Encoding.UTF8.GetString(decodedBytes);
                                _username = _username.Replace("\r", "");
                                _username = _username.Replace("\n", "");
                                _username = _username.Trim();
                                // now encrypt
                                _username = CryptoSvc.Encrypt(_username);
                                _changed = true;
                            }
                            catch
                            {
                                System.Diagnostics.Trace.WriteLine("Exception: Unable to convert username from Base64.");
                            }
                        }
                    }
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (CryptoSvc.IsEncoded(value))
                {
                    _password = value;
                }
                else  // not yet encoded
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        try
                        {
                            byte[] decodedBytes = Convert.FromBase64String(value);
                            _password = Encoding.UTF8.GetString(decodedBytes);
                            _password = _password.Replace("\r", "");
                            _password = _password.Replace("\n", "");
                            _password = _password.Trim();
                            // now encrypt
                            _password = CryptoSvc.Encrypt(_password);
                            _changed = true;
                        }
                        catch
                        {
                            System.Diagnostics.Trace.WriteLine("Exception: Unable to convert password from Base64.");
                        }
                    }
                }
            }
        }

        public bool IsChanged()
        {
            return _changed;
        }

        public void SetChanged(bool changed)
        {
            _changed = changed;
        }

        /// <summary>
        /// Gets the Username
        /// </summary>
        /// <returns>Decrypted Username</returns>
        public string GetUsername()
        {
            return CryptoSvc.Decrypt(_username);
        }

        /// <summary>
        /// Gets the Password
        /// </summary>
        /// <returns>Decrypted Password</returns>
        public string GetPassword()
        {
            return CryptoSvc.Decrypt(_password);
        }

        public void EncodeAndSaveUserNamePassword(string username, string password)
        {
            byte[] userBytes = Encoding.UTF8.GetBytes(username); 
            _username = Convert.ToBase64String(userBytes); 
            byte[] passBytes = Encoding.UTF8.GetBytes(password); 
            _password = Convert.ToBase64String(passBytes);
        }
    }
}
