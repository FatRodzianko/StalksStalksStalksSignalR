using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class YearEvent
    {
        public string BearOrBullYear { get; set; }
        public string StalkName { get; set; }
        public string Description { get; set; }
        public int PriceChange { get; set; }

        public YearEvent(string bearorbull, string stalkname, string description, int pricechange)
        {
            BearOrBullYear = bearorbull;
            StalkName = stalkname;
            Description = description;
            PriceChange = pricechange;
        }
    }
}
