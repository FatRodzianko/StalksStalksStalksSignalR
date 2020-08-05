﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class BearEvent
    {
        
        public string StalkName { get; set; }
        public string Description { get; set; }
        public int PriceChange { get; set; }

        public BearEvent(string stalkname, string description, int pricechange)
        {
            StalkName = stalkname;
            Description = description;
            PriceChange = pricechange;
        }
    }
}

