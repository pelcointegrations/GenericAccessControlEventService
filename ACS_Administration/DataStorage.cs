using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _VX_ACS_Administration
{
    // these classes are used to store local data read from Feenics Keep
    public class PeripheralData
    {
        public string ControllerCommonName;
        public string ControllerKey;
        public string CommonName;
        public string Key;
    }

    public class EventData
    {
        public string ApplicationCommonName;
        public string ApplicationKey;
        public string CommonName;
        public string Key;
    }
}
