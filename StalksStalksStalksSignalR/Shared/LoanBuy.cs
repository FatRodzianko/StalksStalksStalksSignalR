using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class LoanPurchaseOrPayment
    {
        public string PlayerName { get; set; }
        public string ConnectionId { get; set; }
        public string PurchaseOrPayment { get; set; }
        public int DollarAmount { get; set; }

        public LoanPurchaseOrPayment(string playername, string connectionid, string purchaseorpayment, int dollaramount)
        {
            PlayerName = playername;
            ConnectionId = connectionid;
            PurchaseOrPayment = purchaseorpayment;
            DollarAmount = dollaramount;

        }

    }
}
