using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Web.Script.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using StalksStalksStalksSignalR.Shared;
using Microsoft.AspNetCore.Builder;
using StalksStalksStalksSignalR.Client.Pages;

namespace StalksStalksStalksSignalR.Server.Hubs
{
    public class Game : Hub
    {
        // For getting all connected userS?
        public override Task OnConnectedAsync()
        {
            //ConnectedUser.Ids.Add(Context.ConnectionId);
            CreatePlayer("", Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            //ConnectedUser.Ids.Remove(Context.ConnectionId);
            playerList.RemoveAll(x => x.ConnectionId == Context.ConnectionId);
            bool anyRemainingPlayers = playerList.Any(x => x.Ready == true);
            if (!anyRemainingPlayers)
            {
                GameAlready = false;
                EndTheGame = false;
                year = 0;
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task GetUsers()
        { 
            await Clients.All.SendAsync("Returned users", JsonConvert.SerializeObject(playerList));
        }

        public static List<stalk> stalkList = new List<stalk>();
        public static List<player> playerList = new List<player>();
        public static List<StalksOwned> stalksOwned = new List<StalksOwned>();
        public static List<BearEvent> BearEvents = new List<BearEvent>();
        public static List<BullEvent> BullEvents = new List<BullEvent>();
        public PlayerBuy playerBuy = new PlayerBuy("", 0, 0, "");
        public static bool GameAlready = false;
        public static int year = 0;
        public bool EndTheGame = false;

        public async Task ReadyUp(string username)
        {
            CreatePlayer(username, Context.ConnectionId);
            bool allPlayersReady = CheckIfPlayersReady();
            player currentPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            await Clients.All.SendAsync("Readied User", JsonConvert.SerializeObject(playerList), allPlayersReady, JsonConvert.SerializeObject(currentPlayer));
        }

        public async Task EndGame()
        { 
            player currentPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            currentPlayer.EndGame = true;
            bool EndTheGame = CheckEndGame();
            await Clients.All.SendAsync("Player Wants To Quit", JsonConvert.SerializeObject(playerList), EndTheGame);
        }
        public bool CheckEndGame()
        {
            if (playerList.Any(x => x.EndGame == false))
            {
                return false;
            }
            else { return true; }
        }

        public bool CheckReadyYear()
        {
            if (playerList.Any(x => x.StartNewYear == false))
            {
                return false;
            }
            else { return true; }
        }

        public bool CheckIfPlayersReady()
        {
            if (playerList.Any(x => x.Ready == false))
            {
                return false;
            }
            else { return true; }
        }
        
               
        public async Task InitializeGame(bool gameStarted, string playerName)
        {
            if (!gameStarted)
            {
                gameStarted = true;            
                    
            }

            //CreatePlayer(playerName, Context.ConnectionId);

            if (stalkList.Count > 0)
            {
                stalkList.Clear();
            }
            if (stalksOwned.Count > 0)
            {
                stalksOwned.Clear();
            }

            //add the stalks
            stalkList.Add(new stalk("strYker", 100, 0, 65, 65, 0, 0));
            stalkList.Add(new stalk("Soprano's HUD Scam", 100, 0, 10, 20, 5, 10));
            stalkList.Add(new stalk("BERRY BONDS FROM DIE HARD", 100, 0, 0, 0, 5, 5));
            stalkList.Add(new stalk("Teamsters Pension Fund", 100, 0, 80, 25, 0, 25));
            stalkList.Add(new stalk("Springfield Nuclear Power",100, 0, 30, 25, 5, 10));
            stalkList.Add(new stalk("North Haverbrook MONORAIL", 100, 0, 50, 40, 10, 15));
            stalkList.Add(new stalk("PG&Enron", 100, 0, 25, 15, 0, 15));
            stalkList.Add(new stalk("My Mutuals and Me Inc.", 100, 0, 35, 30, 10, 20));
            stalkList.Add(new stalk("Unionized Submissives LLC", 100, 0, 45, 35, 20, 5));
            stalkList.Add(new stalk("My Pillow, Your Pillow, We're All Pillows!", 100, 0, 30, 30, 15, 5));

            //add the yearly events

            BearEvents.Add(new BearEvent("strYker", "I've abandoned my child...I'VE ABANDONED MY CHILD. I'VE ABANDONED MY BOY!", -20));
            BullEvents.Add(new BullEvent("strYker","I drink your milkshake. Sluuurrrrp! I drink it up!", 15));
            BearEvents.Add(new BearEvent( "Soprano's HUD Scam", "\"Of all the girls in Jersey, you had to **** this one?\" Tony Soprano beats you with a belt.", -10));
            BullEvents.Add(new BullEvent("Soprano's HUD Scam", "This Old House has a lot of copper pipes. Bada-bing!", 10));
            BearEvents.Add(new BearEvent("Teamsters Pension Fund", "Pension fund now managed by Goldman Sachs & Co. and Northern Trust Global Advisors fiduciaries. no more corruption!", -15));
            BullEvents.Add(new BullEvent("Teamsters Pension Fund", "Ah Marone another hotel in Las Vegas needs our help. We're getting in on the ground floor!", 10));
            BearEvents.Add(new BearEvent("Springfield Nuclear Power", "Danke, sir, we're from Germany and here to make your power plant more efficient.", -25));
            BullEvents.Add(new BullEvent("Springfield Nuclear Power", "Mr Urbns, time for your sponge bath!", 10));
            BearEvents.Add(new BearEvent("North Haverbrook MONORAIL", "After an extensive three week training session, EHMOR ISPMOSN is now a monorail conductor!", -20));
            BullEvents.Add(new BullEvent("North Haverbrook MONORAIL", "There's nothing on this Earth like genuine, bonafide, electrified, six car MONORAIL!", 20));
            BearEvents.Add(new BearEvent("PG&Enron", "Ah crap we forgot about maintainence for the 50th year in a row!", -25));
            BullEvents.Add(new BullEvent("PG&Enron", "Planned blackouts going just as planned. AHHNOLD is sure to win the presidency now!", 10));
            BearEvents.Add(new BearEvent("My Mutuals and Me Inc.", "You're not my mutual...", -5));
            BullEvents.Add(new BullEvent("My Mutuals and Me Inc.", "Hey, we're mutuals!", 10));
            BearEvents.Add(new BearEvent("Unionized Submissives LLC", "If I could save the Union without ungagging any subs, I would do it!", -10));
            BullEvents.Add(new BullEvent("Unionized Submissives LLC", "All the armies of Europe and Asia combined, could not by force, take a drink from our Golden Streams!", 15));
            BearEvents.Add(new BearEvent("My Pillow, Your Pillow, We're All Pillows!", "My pillow, my pillow, what have ye done!", -5));
            BullEvents.Add(new BullEvent("My Pillow, Your Pillow, We're All Pillows!", "Your pillow is looking mighty fine today!", 5));

            foreach (player player in playerList)
            {
                foreach (stalk stalk in stalkList)
                {
                    stalksOwned.Add(new StalksOwned(player.ConnectionId, stalk.Name, 0));
                }
            }
            GetNetWorth();

            var stalksListToJson = JsonConvert.SerializeObject(stalkList);
            //var serializer = new JavaScriptSerializer();
            //string stalksListToJson = serializer.Serialize(stalkList);

            GameAlready = true;
            player currentPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            await Clients.All.SendAsync("Start Game", gameStarted, stalksListToJson, year, JsonConvert.SerializeObject(currentPlayer), JsonConvert.SerializeObject(playerList), JsonConvert.SerializeObject(stalksOwned));
        }
        private static readonly string[] BearOrBull = new[]
        {
            "bear...", "BULL BULL BULL"
        };

        public void CreatePlayer(string playerName, string connectionid)
        {
            bool exists = playerList.Any(x => x.ConnectionId == connectionid);
            bool nameExists = playerList.Any(x => x.Name == playerName);

            if (exists && !nameExists)
            {
                foreach (player player in playerList.Where(w => w.ConnectionId == connectionid))
                {
                    player.Name = playerName;
                    if (!GameAlready)
                    {
                        player.Ready = true;
                    }
                }
            }
            else if (!exists)
            {
                playerList.Add(new player(playerName, connectionid, 10000, 0, false, false, false));
            }

        }

        

        public string BullBear()
        {
            var rng = new Random();
            string BearBull = BearOrBull[rng.Next(BearOrBull.Length)];
            return BearBull;
        }

        public YearEvent GetYearEvent(string bearOrBull)
        {
            YearEvent thisYear = new YearEvent("", "", "", 0);
            var rng = new Random();

            if (string.Equals(bearOrBull, "bear..."))
            {
                BearEvent newBearEvent = BearEvents[rng.Next(BearEvents.Count - 1)];
                thisYear = new YearEvent("bear...", newBearEvent.StalkName, newBearEvent.Description, newBearEvent.PriceChange);

                foreach (stalk stalk in stalkList)
                {
                    if (string.Equals(stalk.Name, thisYear.StalkName))
                    {
                        stalk.PricePerShare += thisYear.PriceChange;
                    }
                }
            }
            else
            {
                BullEvent newBullEvent = BullEvents[rng.Next(BullEvents.Count - 1)];
                thisYear = new YearEvent("BULL BULL BULL", newBullEvent.StalkName, newBullEvent.Description, newBullEvent.PriceChange);

                foreach (stalk stalk in stalkList)
                {
                    if (string.Equals(stalk.Name, thisYear.StalkName))
                    {
                        stalk.PricePerShare += thisYear.PriceChange;
                    }
                }
            }

            return thisYear;
        }

        public async Task GetNewYear(string bullBear)
        {
            player player = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            player.StartNewYear = true;
            YearEvent thisYear = new YearEvent("", "", "", 0);

            bool isEveryoneReady = CheckReadyYear();
            if (isEveryoneReady || EndTheGame)
            {
                //stalkList = JsonConvert.DeserializeAnonymousType(stalksListToJson, stalkList);
                year++;
                bullBear = BullBear();
                adjustStalks(bullBear);
                CheckBankruptcy();
                thisYear = GetYearEvent(bullBear);
                PayDividend(bullBear);
                GetNetWorth();
                foreach (player otherplayer in playerList)
                {
                    otherplayer.StartNewYear = false;
                    otherplayer.EndGame = false;
                }

            }

            await Clients.All.SendAsync("New Year", year, bullBear, JsonConvert.SerializeObject(stalkList), JsonConvert.SerializeObject(stalksOwned), JsonConvert.SerializeObject(playerList), JsonConvert.SerializeObject(thisYear));
        }

        public bool CanBuy(int numberOfStalks, int stalkPrice, player currentPlayer)
        {
            int price = numberOfStalks * stalkPrice;

            if (currentPlayer.CashOnHand < price)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task BuyStalks(string playerBuyJson)
        {
            playerBuy = JsonConvert.DeserializeAnonymousType(playerBuyJson, playerBuy);
            player currentPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);

            int absoluteTotalStalks = Math.Abs(playerBuy.TotalStalks);
            foreach (StalksOwned owned in stalksOwned)
            {
                if (string.Equals(owned.StalkName, playerBuy.StalkName) && string.Equals(owned.PlayerName, currentPlayer.ConnectionId))
                {

                    if (string.Equals(playerBuy.BuyOrSell, "Buy"))
                    {
                        int stalkPrice = GetStalkPrice(owned.StalkName);
                        if (CanBuy(absoluteTotalStalks, stalkPrice, currentPlayer))
                        {
                            owned.TotalStalks += absoluteTotalStalks;
                            currentPlayer.CashOnHand -= (absoluteTotalStalks * stalkPrice);
                        }
                        else
                        {
                            Console.Write("too poor!");
                        }
                    }
                    else if (string.Equals(playerBuy.BuyOrSell, "Sell"))
                    {
                        // Check if player owns enough stalks to sell this much
                        if (absoluteTotalStalks <= owned.TotalStalks)
                        {
                            int stalkPrice = GetStalkPrice(owned.StalkName);
                            owned.TotalStalks -= absoluteTotalStalks;
                            currentPlayer.CashOnHand += (absoluteTotalStalks * stalkPrice);
                        }
                    }

                }
            }

            await Clients.All.SendAsync("Bought Stalks", JsonConvert.SerializeObject(playerList), JsonConvert.SerializeObject(stalksOwned));
        }

        public void GetNetWorth()
        {
            foreach (player player in playerList)
            {
                int totalStalkValue = 0;
                int netWorthValue = 0;
                foreach (StalksOwned owned in stalksOwned)
                {
                    if (string.Equals(owned.PlayerName, player.ConnectionId))
                    {
                        int pricePerStalk = GetStalkPrice(owned.StalkName);
                        totalStalkValue = (pricePerStalk * owned.TotalStalks);
                        netWorthValue += totalStalkValue;
                    }
                }
                netWorthValue += player.CashOnHand;
                player.NetWorth = netWorthValue;
            }
        }

        public void PayDividend(string bullBear)
        {
            foreach (player player in playerList)
            {
                foreach (StalksOwned owned in stalksOwned)
                {
                    if (string.Equals(owned.PlayerName, player.ConnectionId))
                    {
                        int dividendAmount = 0;
                        foreach (stalk stalk in stalkList)
                        {
                            if (string.Equals(stalk.Name, owned.StalkName))
                            {
                                if (string.Equals(bullBear, "bear..."))
                                {
                                    dividendAmount = stalk.BearDividend;
                                }
                                else
                                {
                                    dividendAmount = stalk.BullDividend;
                                }
                            }
                        }
                        int dividenPayment = (owned.TotalStalks * dividendAmount);
                        player.CashOnHand += dividenPayment;
                    }
                }
            }
        }

        public int GetStalkPrice(string stalkName)
        {
            int stalkPrice = 0;
            foreach (stalk stalk in stalkList)
            {
                if (string.Equals(stalk.Name, stalkName))
                {
                    stalkPrice = stalk.PricePerShare;
                }
            }
            return stalkPrice;
        }
        void CheckBankruptcy()
        {
            for (int i = stalkList.Count - 1; i >= 0; i--)
            {
                if (stalkList[i].PricePerShare <= 0)
                {
                    stalksOwned.RemoveAll(x => x.StalkName == stalkList[i].Name);
                    stalkList.RemoveAt(i);
                }
            }
        }

        public void adjustStalks(string bullBear)
        {
            var rng = new Random();

            if (string.Equals(bullBear, "bear..."))
            {
                foreach (stalk stalk in stalkList)
                {
                    if (stalk.Name != "BERRY BONDS FROM DIE HARD")
                    {
                        stalk.YearlyChange = rng.Next((stalk.maxChangeBear * -1), 10);
                        stalk.PricePerShare += stalk.YearlyChange;
                    }
                }
            }
            else
            {
                foreach (stalk stalk in stalkList)
                {
                    if (stalk.Name != "BERRY BONDS FROM DIE HARD")
                    {
                        stalk.YearlyChange = rng.Next(-15, stalk.maxChangeBull);
                        stalk.PricePerShare += stalk.YearlyChange;
                    }
                }
            }

        }
    }



}
