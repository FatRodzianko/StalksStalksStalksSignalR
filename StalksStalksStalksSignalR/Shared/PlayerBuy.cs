using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class PlayerBuy
    {
        public string StalkName { get; set; }
        public int TotalStalks { get; set; }
        public int PricePerShare { get; set; }
        public string BuyOrSell { get; set; }


        public PlayerBuy(string stalkname, int totalstalks, int pricepershare, string buyorsell)
        {
            StalkName = stalkname;
            TotalStalks = totalstalks;
            PricePerShare = pricepershare;
            BuyOrSell = buyorsell;
        }



    }
}
