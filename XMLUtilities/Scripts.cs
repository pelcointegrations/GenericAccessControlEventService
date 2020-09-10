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
    public class Scripts
    {
        [XmlElement("Script")]
        public Script[] scriptArray;
    }

    [XmlRoot()]
    public class Script
    {
        //public string[] ActionNames = {"SetLayout", "DisplayCamera", "DisconnectCamera", "GotoPreset", "RunPattern", "BookMark"};
        //public string[] Layouts = {"1x1","1x2","2x1","2x2","2x3","3x2","3x3","4x3","4x4","5x5","1plus12","2plus8","3plus4","1plus5","1plus7","12plus1","8plus2","1plus1plus4","1plus4tall","1plus4wide"};
        private string _name;
        private string _number;

        [XmlElement("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlElement("Number")]
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        [XmlElement("Action")]
        public Action[] Actions;
    }

    public class Action
    {
        private string number;
        private string name;
        private string monitor;
        private string cell;
        private string camera;
        private string preset;
        private string pattern;
        private string layout;
        private string previousSeconds;
        private string description;

        [XmlElement("Number")]
        public string Number
        {
            get { return number; }
            set { number = value; }
        }

        [XmlElement("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlElement("Monitor")]
        public string Monitor
        {
            get { return monitor; }
            set { monitor = value; }
        }

        [XmlElement("Cell")]
        public string Cell
        {
            get { return cell; }
            set { cell = value; }
        }

        [XmlElement("Camera")]
        public string Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        [XmlElement("Preset")]
        public string Preset
        {
            get { return preset; }
            set { preset = value; }
        }

        [XmlElement("Pattern")]
        public string Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        [XmlElement("Layout")]
        public string Layout
        {
            get { return layout; }
            set { layout = value; }
        }
        
        [XmlElement("PreviousSeconds")]
        public string PreviousSeconds
        {
            get { return previousSeconds; }
            set { previousSeconds = value; }
        }

        [XmlElement("Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
