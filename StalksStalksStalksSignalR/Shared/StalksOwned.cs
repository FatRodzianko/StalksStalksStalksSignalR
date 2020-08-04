using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class StalksOwned
    {
        public string PlayerName { get; set; }
        public string StalkName { get; set; }
        public int TotalStalks { get; set; }


        public StalksOwned(string playername, string stalkname, int totalstalks)
        {
            PlayerName = playername;
            StalkName = stalkname;
            TotalStalks = totalstalks;
        }



    }
}
