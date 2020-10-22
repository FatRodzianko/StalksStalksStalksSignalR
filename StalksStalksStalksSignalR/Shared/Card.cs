using System;
using System.Collections.Generic;
using System.Text;

namespace StalksStalksStalksSignalR.Shared
{
    public class Card
    {
        public string CardName { get; set; }
        public int Power { get; set; }
        public int AttackValue { get; set; }
        public int DefenseValue { get; set; }
        public string AsciiArt { get; set; }

        public Card(string cardname, int power, int attackvalue, int defensevalue, string asciiart)
        {
            CardName = cardname;
            Power = power;
            AttackValue = attackvalue;
            DefenseValue = defensevalue;
            AsciiArt = asciiart;
        }

    }
}
