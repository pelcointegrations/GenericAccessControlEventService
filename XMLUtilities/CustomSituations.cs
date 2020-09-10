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
    public class CustomSituations
    {
        [XmlElement("CustomSituation")]
        public CustomSituation[] customSituations;
    }

    [XmlRoot()]
    public class CustomSituation
    {
        private string situationType = string.Empty;
        private string sourceDeviceId = string.Empty;
        private string name = string.Empty;
        private int severity = 1;
        private bool log = false;
        private bool notify = false;
        private bool displayBanner = false;
        private bool expandBanner = false;
        private bool audible = false;
        private bool ackNeeded = false;
        private int autoAck = 0;
        private string snoozeIntervals = string.Empty;

        public string SituationType
        {
            get { return situationType; }
            set { situationType = value; }
        }

        public string SourceDeviceId
        {
            get { return sourceDeviceId; }
            set { sourceDeviceId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Severity
        {
            get { return severity; }
            set { severity = value; }
        }

        public bool Log
        {
            get { return log; }
            set { log = value; }
        }

        public bool Notify
        {
            get { return notify; }
            set { notify = value; }
        }

        public bool DisplayBanner
        {
            get { return displayBanner; }
            set { displayBanner = value; }
        }

        public bool ExpandBanner
        {
            get { return expandBanner; }
            set { expandBanner = value; }
        }

        public bool Audible
        {
            get { return audible; }
            set { audible = value; }
        }

        public bool AckNeeded
        {
            get { return ackNeeded; }
            set { ackNeeded = value; }
        }

        public int AutoAcknowledge
        {
            get { return autoAck; }
            set { autoAck = value; }
        }

        public string SnoozeIntervals
        {
            get { return snoozeIntervals; }
            set { snoozeIntervals = value; }
        }

        public List<int> GetSnoozeIntervals()
        {
            List<int> snoozeIntList = new List<int>();
            try
            {
                if (! string.IsNullOrEmpty(SnoozeIntervals))
                {
                    string[] snoozeArray = SnoozeIntervals.Split(',');
                    foreach (string snooze in snoozeArray)
                    {
                        snoozeIntList.Add(Convert.ToInt32(snooze));
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Trace.WriteLine("Exception parsing snooze intervals: " + e.Message);
            }
            return snoozeIntList;
        }

        public bool CompareSnoozeIntervals(List<int> snoozeInts)
        {
            bool same = true;
            List<int> ourSnoozeInts = GetSnoozeIntervals();
            if (ourSnoozeInts.Count != snoozeInts.Count)
                return false;
            foreach(int snoozeInt in snoozeInts)
            {
                if (! ourSnoozeInts.Contains(snoozeInt))
                    return false;
            }
            return same;
        }

        public void SetSnoozeIntervals(List<int> snoozeInts)
        {
            snoozeIntervals = string.Empty;
            int count = snoozeInts.Count;
            foreach(int snoozeInt in snoozeInts)
            {
                snoozeIntervals += Convert.ToString(snoozeInt);
                count--;
                if (count > 0)
                    snoozeIntervals += ',';
            }
        }
    }
}
