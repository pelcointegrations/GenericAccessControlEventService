using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace XMLUtilities
{
    class CryptoSvc
    {
        private static byte[] Entropy = Encoding.Unicode.GetBytes("{DF001A6B-DB26-4642-ABED-AD2A849B2A12}");
        private static string StartEncodedString = "!!!"; // signifies string has been encoded by this crypto service

        public CryptoSvc()
        { }

        public static bool IsEncoded(string input)
        {
            if (input.StartsWith(StartEncodedString))
                return true;
            return false;
        }

        public static string Encrypt(string input)
        {
            string data = string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                System.Diagnostics.Trace.WriteLine("Unable to Encrypt, input is not valid");
                return data;
            }

            try
            {
                byte[] encryptedData = ProtectedData.Protect(
                    Encoding.Unicode.GetBytes(input),
                    Entropy, DataProtectionScope.LocalMachine);
                data = Convert.ToBase64String(encryptedData);
                data = StartEncodedString + data;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e, "Threw while Encrypt");
            }
            return data;
        }

        public static string Decrypt(string input)
        {
            string data = null;
            if (string.IsNullOrWhiteSpace(input))
            {
                System.Diagnostics.Trace.WriteLine("Unable to Decrypt, input is not valid");
                return data;
            }
            if (!input.StartsWith(StartEncodedString))
            {
                System.Diagnostics.Trace.WriteLine("Unable to Decrypt, string has not been encoded correctly");
                return data;
            }
            Regex r = new Regex(StartEncodedString, RegexOptions.IgnoreCase);
            data = r.Replace(input, "", 1);

            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(
                    Convert.FromBase64String(data),
                    Entropy,
                    DataProtectionScope.LocalMachine);
                data = Encoding.Unicode.GetString(decryptedData);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e, "Threw while Decrypt");
            }
            return data;
        }
    }
}
