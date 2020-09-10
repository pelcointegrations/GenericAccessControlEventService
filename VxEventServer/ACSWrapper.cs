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
    }
    #endregion Fake Test Classes

    class ACSWrapper
    {
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
        #endregion Fake Test Methods
    }
}
