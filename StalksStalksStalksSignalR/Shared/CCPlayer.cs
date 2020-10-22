using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class CCPlayer
    {
        public string PlayerName { get; set; }
        public string ConnectionId { get; set; }
        public bool Ready { get; set; }
        public string RoomName { get; set; }
        public int AvailableTanks { get; set; }
        public int AvailableInf { get; set; }
        public int UnavailableTanks { get; set; }
        public int UnavailableInf { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> DiscardPile { get; set; }
        public int TotalWins { get; set; }

        public CCPlayer(string playername, string connectionid, bool ready, string roomname, int availabletanks, int availableinf, int unavailabletanks, int unavailableinf, List<Card> hand, List<Card>discardpile, int totalwins)
        {
            PlayerName = playername;
            ConnectionId = connectionid;
            Ready = ready;
            RoomName = roomname;
            AvailableTanks = availabletanks;
            AvailableInf = availableinf;
            UnavailableTanks = unavailabletanks;
            UnavailableInf = unavailableinf;
            Hand = hand;
            DiscardPile = discardpile;
            TotalWins = totalwins;
        }
    }
}
