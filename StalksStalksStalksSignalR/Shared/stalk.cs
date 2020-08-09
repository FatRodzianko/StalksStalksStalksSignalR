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
        public int minChangeBear { get; set; }
        public int maxChangeBear { get; set; }
        public int minChangeBull { get; set; }
        public int maxChangeBull { get; set; }
        public int BearDividend { get; set; }
        public int BullDividend { get; set; }
        public bool Split { get; set; }

        public stalk(string name, int pricepershare, int yearlychange, int minchangebear, int maxchangebear, int minchangebull, int maxchangebull, int beardividend, int bulldividend, bool split)
        {
            Name = name;
            PricePerShare = pricepershare;
            YearlyChange = yearlychange;
            minChangeBear = minchangebear;
            maxChangeBear = maxchangebear;
            minChangeBull = minchangebull;
            maxChangeBull = maxchangebull;
            BearDividend = beardividend;
            BullDividend = bulldividend;
            Split = split;
        }


    }
}
