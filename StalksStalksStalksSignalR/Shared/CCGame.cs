using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class CCGame
    {
        public int RoundNumber { get; set; }
        public string Phase { get; set; }
        public List<Army> Armies { get; set; }
        public Rooms Room { get; set; }
        public string WinningPlayer { get; set; }

        public CCGame(int roundnumber, string phase, List<Army> armies, Rooms room, string winningplayer)
        {
            RoundNumber = roundnumber;
            Phase = phase;
            Armies = armies;
            Room = room;
            WinningPlayer = winningplayer;
        }
    }
}
