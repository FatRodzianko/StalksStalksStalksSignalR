using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class stalk
    {
        public string Name { get; set; }
        public int PricePerShare { get; set; }
        public int YearlyChange { get; set; }
        public int maxChangeBear { get; set; }
        public int maxChangeBull { get; set; }
        public int BearDividend { get; set; }
        public int BullDividend { get; set; }

        public stalk(string name, int pricepershare, int yearlychange, int maxchangebear, int maxchangebull, int beardividend, int bulldividend)
        {
            Name = name;
            PricePerShare = pricepershare;
            YearlyChange = yearlychange;
            maxChangeBear = maxchangebear;
            maxChangeBull = maxchangebull;
            BearDividend = beardividend;
            BullDividend = bulldividend;
        }


    }
}
