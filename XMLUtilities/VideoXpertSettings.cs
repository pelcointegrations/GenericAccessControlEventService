using System;
using System.Text;
using System.Xml.Serialization;

namespace XMLUtilities
{
    [XmlRoot()]
    public class VideoXpertSettings
    {
        private int _debugLevel = 1;
        private string _vxCoreAddress = string.Empty;
        private int _vxCorePort = 443;
        private string _vxUsername = string.Empty;
        private string _vxPassword = string.Empty;
        private string _integrationId = "E024457E-B2A2-49B5-AE62-20816418650F"; // default IntegrationId
        private bool _changed = false;

        public int DebugLevel
        {
            get { return _debugLevel; }
            set { _debugLevel = value; }
        }

        public string VxCoreAddress
        {
            get { return _vxCoreAddress; }
            set 
            { 
                if (value != string.Empty) 
                    _vxCoreAddress = value; 
            }
        }

        public int VxCorePort
        {
            get { return _vxCorePort; }
            set { _vxCorePort = value; }
        }

        public string VxUsername
        {
            get { return _vxUsername; }
            set
            {
                if (value != string.Empty)
                {
                    if (CryptoSvc.IsEncoded(value))
                    {
                        _vxUsername = value;
                    }
                    else  // not yet encoded
                    {
                        try
                        {
                            byte[] decodedBytes = Convert.FromBase64String(value);
                            _vxUsername = Encoding.UTF8.GetString(decodedBytes);
                            _vxUsername = _vxUsername.Replace("\r", "");
                            _vxUsername = _vxUsername.Replace("\n", "");
                            _vxUsername = _vxUsername.Trim();
                            // now encrypt
                            _vxUsername = CryptoSvc.Encrypt(_vxUsername);
                            _changed = true;
                        }
                        catch
                        {
                            System.Diagnostics.Trace.WriteLine("Exception: Unable to convert username from Base64, defaulted to " + _vxUsername);
                        }
                    }
                }
            }
        }

        public string VxPassword
        {
            get { return _vxPassword; }
            set
            {
                if (value != string.Empty)
                {
                    if (CryptoSvc.IsEncoded(value))
                    {
                        _vxPassword = value;
                    }
                    else  // not yet encoded
                    {
                        try
                        {
                            // decode from base64
                            byte[] decodedBytes = Convert.FromBase64String(value);
                            _vxPassword = Encoding.UTF8.GetString(decodedBytes);
                            _vxPassword = _vxPassword.Replace("\r", "");
                            _vxPassword = _vxPassword.Replace("\n", "");
                            _vxPassword = _vxPassword.Trim();
                            // now encrypt
                            _vxPassword = CryptoSvc.Encrypt(_vxPassword);
                            _changed = true;
                        }
                        catch
                        {
                            System.Diagnostics.Trace.WriteLine("Exception: Unable to convert password from Base64, defaulted to " + _vxPassword);
                        }
                    }
                }
            }
        }
        public string IntegrationId
        {
            get { return _integrationId; }
            set
            {
                if (value != string.Empty)
                    _integrationId = value;
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
            return CryptoSvc.Decrypt(_vxUsername);
        }

        /// <summary>
        /// Gets the Password
        /// </summary>
        /// <returns>Decrypted Password</returns>
        public string GetPassword()
        {
            return CryptoSvc.Decrypt(_vxPassword);
        }

        /// <summary>
        /// Encrypts Username and Password from plain text
        /// </summary>
        /// <param name="username">username to encrypt</param>
        /// <param name="password">password to encrypt</param>
        public void EncodeAndSaveUserNamePassword(string username, string password)
        {
            byte[] userBytes = Encoding.UTF8.GetBytes(username);
            string encodedStr = Convert.ToBase64String(userBytes);
            VxUsername = encodedStr;
            byte[] passBytes = Encoding.UTF8.GetBytes(password);
            encodedStr = Convert.ToBase64String(passBytes);
            VxPassword = encodedStr;
        }
    }
}
