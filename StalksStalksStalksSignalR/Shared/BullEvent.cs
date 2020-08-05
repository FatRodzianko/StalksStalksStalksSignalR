using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class BullEvent
    {
        public string StalkName { get; set; }
        public string Description { get; set; }
        public int PriceChange { get; set; }

        public BullEvent(string stalkname, string description, int pricechange)
        {
            StalkName = stalkname;
            Description = description;
            PriceChange = pricechange;
        }
    }
}
