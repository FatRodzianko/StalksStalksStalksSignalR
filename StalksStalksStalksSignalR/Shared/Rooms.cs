using System;
using System.Collections.Generic;
using System.Text;
using StalksStalksStalksSignalR.Shared;

namespace StalksStalksStalksSignalR.Shared
{
    public class Rooms
    {
        public string RoomName { get; set; }
        public List<CCPlayer> Players { get; set; }

        public Rooms(string roomname, List<CCPlayer> players)
        {
            RoomName = roomname;
            Players = players;
        }
    }
}
