using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace VxEvents
{
    #region Fake Test Classes
    class FakeUser
    {
        public string Name;
        public string UserId;
        public int CardNumber;
        public Image UserImage; 
    }

    class FakeAlarm
    {
        public string Name;
        public int Number;
        public string AlarmId;
        public string Status;
    }

    class FakeDoor
    {
        public string Name;
        public int Number;
        public string DoorId;
        public string Status;
    }

    class FakeACSEvent
    {
        public string EventType;
        public FakeUser User;
        public FakeDoor Door;
        public FakeAlarm Alarm;
    }
    #endregion Fake Test Classes

    class ACSWrapper
    {
        List<FakeAlarm> _alarms = new List<FakeAlarm>();
        List<FakeUser> _users = new List<FakeUser>();
        List<FakeDoor> _doors = new List<FakeDoor>();

        public bool LoggedIn
        {
            get { return true; }
        }

        public ACSWrapper(string hostUrl)
        {
            // populate test data
            FakeUser user1001 = new FakeUser { Name = "Carol Jensen", UserId = "611C22EE-9568-495C-B55F-C0E4DAC826CD", CardNumber = 1001, UserImage = VxEventServer.Properties.Resources.user};
            _users.Add(user1001);

            FakeUser user1002 = new FakeUser { Name = "Steve Smith", UserId = "0881E65B-F79A-4E00-818D-4AC3AF831ACB", CardNumber = 1002, UserImage = VxEventServer.Properties.Resources.user1};
            _users.Add(user1002);

            FakeUser user1008 = new FakeUser { Name = "Dave Cook", UserId = "8C935BB5-05A9-4183-9072-16176C2045A1", CardNumber = 1008, UserImage = VxEventServer.Properties.Resources.user2};
            _users.Add(user1008);

            FakeDoor door1 = new FakeDoor { Name = "Front Lobby Door", Number = 1, DoorId = "2EEB3E9F-7E75-438C-A2A0-901EC18E019A", Status = "Unlocked" };
            _doors.Add(door1);

            FakeDoor door2 = new FakeDoor { Name = "Back Patio Door", Number = 2, DoorId = "DE629756-5D01-4363-B7E7-6CFFAD106947", Status = "Locked" };
            _doors.Add(door2);

            FakeDoor door3 = new FakeDoor { Name = "Side Door", Number = 3, DoorId = "1E17DEF5-F038-4C6F-A89F-11743C20F81A", Status = "Locked" };
            _doors.Add(door3);

            FakeAlarm alarm1 = new FakeAlarm { Name = "Front Lobby Alarm", Number = 1, AlarmId = "574450B0-7BE5-449E-AFC7-DF4384A6B259", Status = "Off" };
            _alarms.Add(alarm1);

            FakeAlarm alarm2 = new FakeAlarm { Name = "Back Door Alarm", Number = 2, AlarmId = "C7CC3AD2-D1B0-4914-A551-C323455B114E", Status = "On" };
            _alarms.Add(alarm2);

            FakeAlarm alarm3 = new FakeAlarm { Name = "Parking Lot Alarm", Number = 3, AlarmId = "78B837C8-D6C8-4D63-B0A1-C9632AF90769", Status = "Faulted" };
            _alarms.Add(alarm3);

        }

        public string GetSysInfo()
        {
            string info = "ServerId: Generic ACS";
            info += "\r\nVersion: 1.0.0.0";
            info += "\r\nServer Time: " + GetServerTime().ToString();
			return info;
        }

        public bool Login(string userName, string passWord)
        {
            return LoggedIn;
        }

        public DateTime GetServerTime()
        {
            return DateTime.UtcNow;
        }

        public List<string> GetEventList()
        {
            List<string> eventList = new List<string>();
            eventList.Add("Door Open");
            eventList.Add("Door Closed");
            eventList.Add("Door Locked");
            eventList.Add("Door Unlocked");
            eventList.Add("Access Denied");
            eventList.Add("Access Granted");
            eventList.Add("Motion Detected");
            eventList.Add("Device Offline");
            eventList.Add("Alarm On");
            eventList.Add("Alarm Off");
            eventList.Add("Alarm Faulted");
            eventList.Add("Alarm Unknown");
            return eventList;
        }

        public List<string> GetUserNames()
        {
            List<string> userList = new List<string>();
            foreach(var user in _users)
            {
                userList.Add(user.Name);
            }
            return userList;
        }

        public List<FakeUser> GetUsers()
        {
            List<FakeUser> userList = new List<FakeUser>();
            foreach (var user in _users)
            {
                userList.Add(user);
            }
            return userList;
        }

        public Image GetUserImage(string userId)
        {
            foreach (var user in _users)
            {
                if (user.UserId == userId)
                    return user.UserImage;
            }
            return null;
        }

        public int GetCardAssignment(string userName)
        {
            int cardNumber = 0;
            var user = _users.FirstOrDefault(x => x.Name.ToUpper().Contains(userName.ToUpper()));
            if (user != null)
            {
                cardNumber = user.CardNumber;
            }
            return cardNumber;
        }

        public List<string> GetDoorNames()
        {
            List<string> controllerList = new List<string>();
            foreach (var door in _doors)
            {
                controllerList.Add(door.Name);
            }
            return controllerList;
        }

        public List<FakeAlarm> GetAlarms()
        {
            List<FakeAlarm> alarmList = new List<FakeAlarm>();
            foreach (var alarm in _alarms)
            {
                alarmList.Add(alarm);
            }
            return alarmList;
        }

        public string GetAlarmStatus(string alarmName)
        {
            string status = "Unknown";
            var alarm = _alarms.FirstOrDefault(x => x.Name.ToUpper().Contains(alarmName.ToUpper()));
            if (alarm != null)
            {
                status = alarm.Status;
            }
            return status;
        }

        public void SetAlarmStatus(string alarmName, string status)
        {
            var alarm = _alarms.FirstOrDefault(x => x.Name.ToUpper().Contains(alarmName.ToUpper()));
            if (alarm != null)
            {
                if (status.ToUpper().Contains("OF"))
                    alarm.Status = "Off";
                else if (status.ToUpper().Contains("ON"))
                    alarm.Status = "On";
                else if (status.ToUpper().Contains("FA"))
                    alarm.Status = "Faulted";
                else alarm.Status = "Unknown";
            }
        }

        public void SetAlarmStatusById(string alarmId, Pelco.AccessControlIPC.ACSTypes.ACSAlarmState status)
        {
            var alarm = _alarms.FirstOrDefault(x => x.AlarmId.ToUpper() == alarmId.ToUpper());
            if (alarm != null)
            {
                alarm.Status = status.ToString();
            }
        }


        public List<FakeDoor> GetDoors()
        {
            List<FakeDoor> controllerList = new List<FakeDoor>();
            foreach (var door in _doors)
            {
                controllerList.Add(door);
            }
            return controllerList;
        }

        public string GetDoorStatus(string doorName)
        {
            string status = "Unknown";
            var door = _doors.FirstOrDefault(x => x.Name.ToUpper().Contains(doorName.ToUpper()));
            if (door != null)
            {
                status = door.Status;
            }
            return status;
        }

        public int GetDoorNumber(string doorName)
        {
            int number = 0;
            var door = _doors.FirstOrDefault(x => x.Name.ToUpper().Contains(doorName.ToUpper()));
            if (door != null)
            {
                number = door.Number;
            }
            return number;
        }

        public void SetDoorStatus(string doorName, string status)
        {
            var door = _doors.FirstOrDefault(x => x.Name.ToUpper().Contains(doorName.ToUpper()));
            if (door != null)
            {
                if (status.ToUpper().Contains("UNL"))
                    door.Status = "Unlocked";
                else if (status.ToUpper().Contains("LOCK"))
                    door.Status = "Locked";
                else if (status.ToUpper().Contains("OPE"))
                    door.Status = "Open";
                else if (status.ToUpper().Contains("CL"))
                    door.Status = "Closed";
                else if (status.ToUpper().Contains("FA"))
                        door.Status = "Faulted";
                else if (status.ToUpper().Contains("FO"))
                    door.Status = "Forced";
                else if (status.ToUpper().Contains("PR"))
                    door.Status = "Propped";
                else door.Status = "Unknown";
            }
        }
        #region Fake Test Methods
        public FakeACSEvent CreateFakeEvent()
        {
            FakeACSEvent fakeEvent = new FakeACSEvent();

            Random random = new Random();
            var eventTypes = GetEventList();
            int eventNumber = random.Next(eventTypes.Count - 1);
            fakeEvent.EventType = eventTypes[eventNumber];
            int userNumber = random.Next(_users.Count - 1);
            fakeEvent.User = _users[userNumber];
            int doorNumber = random.Next(_doors.Count - 1);
            fakeEvent.Door = _doors[doorNumber];
            Trace.WriteLine("Fake ACS Event Created");
            Trace.WriteLine("    EventType: " + fakeEvent.EventType);
            Trace.WriteLine("    User: " + fakeEvent.User.Name);
            Trace.WriteLine("    Door: " + fakeEvent.Door.Name);
            return fakeEvent;
        }

        public FakeACSEvent CreateFakeDoorEvent(string doorId, string doorEvent)
        {
            FakeACSEvent fakeEvent = new FakeACSEvent();
            var eventTypes = GetEventList();
            fakeEvent.EventType = eventTypes.FirstOrDefault(x => x.ToUpper().Contains(doorEvent.ToUpper()));
            fakeEvent.User = null;
            fakeEvent.Door = _doors.FirstOrDefault(x => x.DoorId == doorId);
            // seems like the id is the name sometimes.
            if (fakeEvent.Door == null)
            {
                fakeEvent.Door = _doors.FirstOrDefault(x => x.Name == doorId);
            }
            Trace.WriteLine("Fake ACS Door Event Created");
            Trace.WriteLine("    EventType: " + fakeEvent.EventType);
            Trace.WriteLine("    Door: " + fakeEvent.Door.Name);
            return fakeEvent;
        }

        public FakeACSEvent CreateFakeAlarmEvent(string alarmId, string alarmEvent)
        {
            FakeACSEvent fakeEvent = new FakeACSEvent();
            var eventTypes = GetEventList();
            fakeEvent.EventType = eventTypes.FirstOrDefault(x => x.ToUpper().Contains(alarmEvent.ToUpper()));
            fakeEvent.User = null;
            fakeEvent.Alarm = _alarms.FirstOrDefault(x => x.AlarmId == alarmId);
            // seems like the id is the name sometimes.
            if (fakeEvent.Alarm == null)
            {
                fakeEvent.Alarm = _alarms.FirstOrDefault(x => x.Name == alarmId);
            }
            Trace.WriteLine("Fake ACS Alarm Event Created");
            Trace.WriteLine("    EventType: " + fakeEvent.EventType);
            Trace.WriteLine("    Alarm: " + fakeEvent.Alarm.Name);
            return fakeEvent;
        }
        #endregion Fake Test Methods
    }
}
