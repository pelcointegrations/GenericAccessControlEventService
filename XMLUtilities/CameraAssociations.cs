using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace XMLUtilities
{

    [XmlRoot()]
    public class CameraAssociations
    {
        [XmlElement("CameraAssociation")]
        public CameraAssociation[] CameraAssociation;
    }

    [XmlRoot()]
    public class CameraAssociation
    {
        private string _cameraId;
        private string _acsPeripheral;
        private string _acsEvent;

        public string CameraId
        {
            get { return _cameraId; }
            set { _cameraId = value; }
        }

        public string ACSPeripheral
        {
            get { return _acsPeripheral; }
            set { _acsPeripheral = value; }
        }

        public string ACSEvent
        {
            get { return _acsEvent; }
            set { _acsEvent = value; }
        }
    }
}
