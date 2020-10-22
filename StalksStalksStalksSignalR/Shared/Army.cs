using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class Army
    {
        public string PlayerName { get; set; }
        public int Tanks { get; set; }
        public int Infantry { get; set; }
        public Card PlayerCard { get; set; }

        public Army(string playername, int tanks, int infantry, Card playercard)
        {
            PlayerName = playername;
            Tanks = tanks;
            Infantry = infantry;
            PlayerCard = playercard;
        }
    }
}
