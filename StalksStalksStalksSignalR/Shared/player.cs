using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class player
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public int CashOnHand { get; set; }
        public int NetWorth { get; set; }
        public bool Ready { get; set; }
        public bool StartNewYear { get; set; }
        public bool EndGame { get; set; }
        public bool HasLoan { get; set; }

        public player(string name, string connectionid, int cashonhand, int networth, bool ready, bool startnewyear, bool endgame, bool hasloan)
        {
            Name = name;
            ConnectionId = connectionid;
            CashOnHand = cashonhand;
            NetWorth = networth;
            Ready = ready;
            StartNewYear = startnewyear;
            EndGame = endgame;
            HasLoan = hasloan;
        }


    }
}
