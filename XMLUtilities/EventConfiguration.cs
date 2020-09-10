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
    public class EventConfiguration
    {
        [XmlElement("EventMap")]
        public EventMap[] EventMap;
    }

    [XmlRoot()]
    public class EventMap
    {
        private string _direction;
        private string _acsEvent;
        private VxSituation _vxSituation;
        private string _runScripts;
        private string _ackScripts;

        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public string ACSEvent
        {
            get { return _acsEvent; }
            set { _acsEvent = value; }
        }

        public VxSituation VxSituation
        {
            get { return _vxSituation; }
            set { _vxSituation = value; }
        }

        public string RunScripts
        {
            get { return _runScripts; }
            set { _runScripts = value; }
        }

        public string AckScripts
        {
            get { return _ackScripts; }
            set { _ackScripts = value; }
        }
    }

    [XmlRoot()]
    public class VxSituation
    {
        private string ackState = string.Empty;
        private string type = string.Empty;

        [XmlElement("Property")]
        public VxSituationProperty[] properties;

        public string AckState
        {
            get { return ackState; }
            set { ackState = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }

    public class VxSituationProperty
    {
        private string key;
        private string val;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public string Value
        {
            get { return val; }
            set { val = value; }
        }
    }
}
