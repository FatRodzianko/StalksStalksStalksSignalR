using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class Loan
    {
        public string PlayerName { get; set; }
        public string ConnectionId { get; set; }
        public int LoanBalance { get; set; }
        public int MinPayment { get; set; }
        public int YearsRemaining { get; set; }
        public bool PaidThisYear { get; set; }
        public int MissedPayments { get; set; }

        public Loan(string playername, string connectionid, int loanbalance, int minpayment, int yearsremaining, bool paidthisyear, int missedpayments)
        {
            PlayerName = playername;
            ConnectionId = connectionid;
            LoanBalance = loanbalance;
            MinPayment = minpayment;
            YearsRemaining = yearsremaining;
            PaidThisYear = paidthisyear;
            MissedPayments = missedpayments;
        }
    }
}
