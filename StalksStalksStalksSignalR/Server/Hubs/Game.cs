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
using System.Security.Cryptography.X509Certificates;

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
                EndTheGameCount = 0;
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
        public static List<Loan> AllLoans = new List<Loan>();
        
        public static List<string> stalksBankrupt = new List<string>();
        
        public static List<BearEvent> BearEvents = new List<BearEvent>();
        public static List<BullEvent> BullEvents = new List<BullEvent>();
        public PlayerBuy playerBuy = new PlayerBuy("", 0, 0, "");
        public static bool GameAlready = false;
        public static int year = 0;
        public static bool EndTheGame = false;
        public int EndTheGameCount = 0;

        public async Task RequestYeehaw()
        {
            player currentPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            await Clients.All.SendAsync("Yeehaw Requested", currentPlayer.Name);
        }

        public async Task GiveYeehaw()
        {
            player currentPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            await Clients.All.SendAsync("Yeehaw Sent", currentPlayer.Name);
        }

        public async Task ReadyUp(string username)
        {
            //CreatePlayer(username, Context.ConnectionId);
            player RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            if (!GameAlready)
            {
                RequestingPlayer.Ready = true;
            }
            
            bool allPlayersReady = CheckIfPlayersReady();
            player currentPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            await Clients.Caller.SendAsync("Readied User", JsonConvert.SerializeObject(playerList), allPlayersReady, JsonConvert.SerializeObject(currentPlayer));
        }
        public async Task CreatePlayerName(string playername)
        {
            bool userNameTaken = playerList.Any(x => x.Name == playername);
            player RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            if (!userNameTaken)
            {
                RequestingPlayer.Name = playername;
            }
            else if (userNameTaken)
            {
                RequestingPlayer.Name = "";
            }

            await Clients.Caller.SendAsync("Player Name Added", JsonConvert.SerializeObject(playerList), JsonConvert.SerializeObject(RequestingPlayer));
        }

        public async Task EndGame()
        { 
            player currentPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            currentPlayer.EndGame = true;
            EndTheGame = CheckEndGame();
            await Clients.All.SendAsync("Player Wants To Quit", JsonConvert.SerializeObject(playerList), EndTheGame);
        }
        public bool CheckEndGame()
        {
            if (playerList.Any(x => x.EndGame == false && x.Ready != false))
            {
                return false;
            }
            else { return true; }
        }

        public bool CheckReadyYear()
        {
            if (playerList.Any(x => x.StartNewYear == false && x.Ready != false))
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
            if (stalksBankrupt.Count > 0)
            {
                stalksBankrupt.Clear();
            }
            if (stalksBankrupt.Count > 0)
            {
                stalksBankrupt.Clear();
            }
            if (AllLoans.Count > 0)
            {
                AllLoans.Clear();
            }

            //add the stalks
            stalkList.Add(new stalk("strYker", 100, 0, 10, 50, 15, 45, 0, 0, false));
            stalkList.Add(new stalk("Soprano's HUD Scam", 100, 0, 5, 25, 5, 15, 1, 3, false));
            stalkList.Add(new stalk("BERRY BONDS FROM DIE HARD", 100, 0, 0, 0, 0, 0, 3, 3, false));
            stalkList.Add(new stalk("Teamsters Pension Fund", 100, 0, 15, 40, 0, 15, 0, 8, false));
            stalkList.Add(new stalk("Springfield Nuclear Power",100, 0, 10, 20, 0, 15, 0, 5, false));
            stalkList.Add(new stalk("North Haverbrook MONORAIL", 100, 0, 5, 30, 0, 20, 1, 4, false));
            stalkList.Add(new stalk("PG&Enron", 100, 0, 10, 35, 5, 15, 0, 4, false));
            stalkList.Add(new stalk("My Mutuals and Me Inc.", 100, 0, 0, 10, 0, 5, 2, 3, false));
            stalkList.Add(new stalk("Unionized Submissives LLC", 100, 0, 0, 25, 5, 20, 0, 2, false));
            stalkList.Add(new stalk("My Pillow, Your Pillow, We're All Pillows!", 100, 0, 5, 25, 0, 15, 1, 1, false));

            //add the yearly events
            //Bull events
            BullEvents.Add(new BullEvent("strYker", "I drink your milkshake. Sluuurrrrp! I drink it up!", 15));
            BullEvents.Add(new BullEvent("strYker", "Papa John appointed as Administrator of the EPA.", 10));
            BullEvents.Add(new BullEvent("strYker", "New oil deposists found beneath sacred Indian site. ", 5));
            BullEvents.Add(new BullEvent("strYker", "Regulations on poisoning the water supply relaxed.", 10));
            BullEvents.Add(new BullEvent("strYker", "Coup in Venezuela successful. Chicago Boys on first plane to Caracas to take over the finance ministry.", 20));

            BullEvents.Add(new BullEvent("Soprano's HUD Scam", "This Old House has a lot of copper pipes. Bada-bing!", 10));
            BullEvents.Add(new BullEvent("Soprano's HUD Scam", "Great meeting in the Russian bathhouse. You weren't even the fattest one there!", 5));
            BullEvents.Add(new BullEvent("Soprano's HUD Scam", "City Council member shows up at your underground casino. You spike their drinks and loand them $50k. They lose it all.", 10));
            BullEvents.Add(new BullEvent("Soprano's HUD Scam", "After purchasing a block of dilapidated homes, Police Commissioner on the dole agrees to clear out the 'riffraff' living in the neighborhood.", 5));
            BullEvents.Add(new BullEvent("Soprano's HUD Scam", "Head of the Urban League receives bill for daughter's college tuition. They agree to sign off on your scam.", 15));

            BullEvents.Add(new BullEvent("Teamsters Pension Fund", "Ah Marone another hotel in Las Vegas needs our help. We're getting in on the ground floor!", 10));
            BullEvents.Add(new BullEvent("Teamsters Pension Fund", "Bullets left on the doorsteps of union member dissidents day before leadership elections.", 5));
            BullEvents.Add(new BullEvent("Teamsters Pension Fund", "Construction of casino complete. No one will ever find what's beneath the foundation. Vegas, baby!", 10));
            BullEvents.Add(new BullEvent("Teamsters Pension Fund", "New union contract signed with FedEx. Nation wide strike ends!", 5));
            BullEvents.Add(new BullEvent("Teamsters Pension Fund", "An epidemic of goods falling off the back of trucks and right into the hands of our 'friends.'", 15));

            BullEvents.Add(new BullEvent("Springfield Nuclear Power", "Mr Urbns, time for your sponge bath!", 10));
            BullEvents.Add(new BullEvent("Springfield Nuclear Power", "New union contract specifies donuts to be provided each morning to every employee. Productivity increases 10%", 15));
            BullEvents.Add(new BullEvent("Springfield Nuclear Power", "Reactor meltdown avoided after employee presses random button on the control panel.", 5));
            BullEvents.Add(new BullEvent("Springfield Nuclear Power", "New chairs with added lumbar support provided to all control operators.", 5));
            BullEvents.Add(new BullEvent("Springfield Nuclear Power", "Worst performing employee quits to follow their dream of working at the bowling alley. We'll never see them again!", 10));

            BullEvents.Add(new BullEvent("North Haverbrook MONORAIL", "There's nothing on this Earth like genuine, bonafide, electrified, six car MONORAIL!", 5));
            BullEvents.Add(new BullEvent("North Haverbrook MONORAIL", "An anchor was conviently found on the runaway monorail train.", 10));
            BullEvents.Add(new BullEvent("North Haverbrook MONORAIL", "The citizens of the town come to their good senses, cast away all doubts about the monorail project, and purchase the monorail.", 20));
            BullEvents.Add(new BullEvent("North Haverbrook MONORAIL", "No one in this one horse town has heard of, let alone seen, The Music Man.", 15));
            BullEvents.Add(new BullEvent("North Haverbrook MONORAIL", "Groundbreaking of the MONORAIL with William Shatner is broadcast on local news.", 10));

            BullEvents.Add(new BullEvent("PG&Enron", "Planned blackouts going just as planned. AHHNOLD is sure to win the presidency now!", 10));
            BullEvents.Add(new BullEvent("PG&Enron", "Another year of cooked books and paid off auditors. We can't lose!", 10));
            BullEvents.Add(new BullEvent("PG&Enron", "It rained ALL WINTER! No need to worry about wildfires this summer! Put off those repairs for another year, baby!", 5));
            BullEvents.Add(new BullEvent("PG&Enron", "State legislators approve our 100% price increases!", 5));
            BullEvents.Add(new BullEvent("PG&Enron", "Severe drought diverts attention away from us and toward the water companies and water subsidies provided to pistachio farmers.", 5));

            BullEvents.Add(new BullEvent("My Mutuals and Me Inc.", "Hey, we're mutuals!", 5));
            BullEvents.Add(new BullEvent("My Mutuals and Me Inc.", "New mutual fund created: The NON-STANDARD and RICH 500.", 5));
            BullEvents.Add(new BullEvent("My Mutuals and Me Inc.", "New mutual fund created: Small capital business with no slave labor in their supply chain.", 5));
            BullEvents.Add(new BullEvent("My Mutuals and Me Inc.", "New mutual fund created: Crypto currency index fund.", 5));
            BullEvents.Add(new BullEvent("My Mutuals and Me Inc.", "New mutual fund created: Target retirement fund for naive Zoomers who think they'll ever be able to retire.", 5));

            BullEvents.Add(new BullEvent("Unionized Submissives LLC", "All the armies of Europe and Asia combined, could not by force, take a drink from our Golden Streams!", 15));
            BullEvents.Add(new BullEvent("Unionized Submissives LLC", "All the armies of Europe and Asia combined, could not by force, take a drink from our Golden Streams!", 10));
            BullEvents.Add(new BullEvent("Unionized Submissives LLC", "All the armies of Europe and Asia combined, could not by force, take a drink from our Golden Streams!", 15));
            BullEvents.Add(new BullEvent("Unionized Submissives LLC", "All the armies of Europe and Asia combined, could not by force, take a drink from our Golden Streams!", 5));
            BullEvents.Add(new BullEvent("Unionized Submissives LLC", "All the armies of Europe and Asia combined, could not by force, take a drink from our Golden Streams!", 5));
            
            BullEvents.Add(new BullEvent("My Pillow, Your Pillow, We're All Pillows!", "'My Pillow, Your Pillow, We're All Pillows!', the last manufacturer left in the U.S., moves factories to Laos to save on labor costs.", 15));
            BullEvents.Add(new BullEvent("My Pillow, Your Pillow, We're All Pillows!", "New cost saving pillow filling discovered.", 5));
            BullEvents.Add(new BullEvent("My Pillow, Your Pillow, We're All Pillows!", "Following the pandemic cruiselines replace all former pillows with 'My Pillow, Your Pillow, We're All Pillows' after experiments show no living thing can survive more than five minutes on the pillow's surface.", 10));
            BullEvents.Add(new BullEvent("My Pillow, Your Pillow, We're All Pillows!", "Recovering meth addict and best friend of the founder and owner plants head lice on the pillows in every Motel 6 across the country.", 10));
            BullEvents.Add(new BullEvent("My Pillow, Your Pillow, We're All Pillows!", "'My Pillow, Your Pillow, We're All Pillows!' featured in latest flat earth documentry.", 5));

            //Bear Events
            BearEvents.Add(new BearEvent("strYker", "I've abandoned my child...I'VE ABANDONED MY CHILD. I'VE ABANDONED MY BOY!", -20));
            BearEvents.Add(new BearEvent("strYker", "Coup in Venezuela failed. strYker's hand picked \"legitimate\" leader Otto Von Mengele gained zero popular support and was laughed out of the country.", -10));
            BearEvents.Add(new BearEvent("strYker", "New regulations in the U.S. restrict dumping oil directly into rivers.", -15));
            BearEvents.Add(new BearEvent("strYker", "Hired former Head of Safety and Quality Assurance from BP to design new offshore oilrigs.", -25));
            BearEvents.Add(new BearEvent("strYker", "New policy enacted to allow oil tanker captains (1) cup of beer while on duty. All captains purchase gallon sized cup.", -15));

            BearEvents.Add(new BearEvent("Soprano's HUD Scam", "\"Of all the girls in Jersey, you had to **** this one?\" Tony Soprano beats you with a belt.", -10));
            BearEvents.Add(new BearEvent("Soprano's HUD Scam", "City Council member gets cold feet. You're forced to give them cement shoes. Scam on hold until you can blackmail another council member to play ball.", -20));
            BearEvents.Add(new BearEvent("Soprano's HUD Scam", "A bright eyed idealist at HUD actually read your load application. Denied.", -20));
            BearEvents.Add(new BearEvent("Soprano's HUD Scam", "After a hard day's night working (drinking and playing cards in the back of a strip club), your tummy grumbles and you need to poo. It's the first poo in a week, and you've been eating nothing but cured meats during that time.", -15));
            BearEvents.Add(new BearEvent("Soprano's HUD Scam", "\"What ever happened to the Gary Cooper types\" you say to yourself as you start to cry watching James Cagney beat his wife with a grapefruit in 'The Public enemy.'", -15));

            BearEvents.Add(new BearEvent("Teamsters Pension Fund", "Pension fund now managed by Goldman Sachs & Co. and Northern Trust Global Advisors fiduciaries. no more corruption!", -25));
            BearEvents.Add(new BearEvent("Teamsters Pension Fund", "Motor Carrier Act passes congress.", -20));
            BearEvents.Add(new BearEvent("Teamsters Pension Fund", "No one can find Jimmy.", -15));
            BearEvents.Add(new BearEvent("Teamsters Pension Fund", "Strike broken. The 'man' won. Driver pay cut by 20%.", -15));
            BearEvents.Add(new BearEvent("Teamsters Pension Fund", "Vegas casinos convinced themselves that adding theme parks to their hotels will finally allow them to take money directly from children. All parks shuttered in early 2000's.", -15));

            BearEvents.Add(new BearEvent("Springfield Nuclear Power", "Danke, sir, we're from Germany and here to make your power plant more efficient.", -5));
            BearEvents.Add(new BearEvent("Springfield Nuclear Power", "Three-eyed fish found in river downstream from the plant.", -10));
            BearEvents.Add(new BearEvent("Springfield Nuclear Power", "New, cheaper vending machines installed in the break room. Items get stuck in the machine more frequently compared to the older machines. Producivity plummets as an increasing number of employees get their arms stuck in the vending machines.", -15));
            BearEvents.Add(new BearEvent("Springfield Nuclear Power", "Your long lost, cherished childhood toy ObOb is found by an employee's child. You reluctantly agree to pay the family to feel something for the first time in a decade.", -5));
            BearEvents.Add(new BearEvent("Springfield Nuclear Power", "Department of Energy Inspectors arrive at the plant.", -10));

            BearEvents.Add(new BearEvent("North Haverbrook MONORAIL", "At the townhall, a busybody citizen asks pointed questions about the details of the monorail project.", -10));
            BearEvents.Add(new BearEvent("North Haverbrook MONORAIL", "An intrepid citizen visits neighboring town to see the results of their monorail project.", -15));
            BearEvents.Add(new BearEvent("North Haverbrook MONORAIL", "Townsfolk confused by the 'MONO = ONE RAIL = RAIL' presentation slide.", -5));
            BearEvents.Add(new BearEvent("North Haverbrook MONORAIL", "Monorail train cars installed with no brakes.", -10));
            BearEvents.Add(new BearEvent("North Haverbrook MONORAIL", "Designer of the monorail train cars blows the whistle on shoddy contruction and cheap materials used to build the North Haverbrook MONORAIL.", -10));

            BearEvents.Add(new BearEvent("PG&Enron", "Ah crap we forgot about maintainence for the 50th year in a row!", -15));
            BearEvents.Add(new BearEvent("PG&Enron", "Former lead signer of the Dead Kennedys siezes the state capitol and nationalizes the power grid.", -20));
            BearEvents.Add(new BearEvent("PG&Enron", "Accounts Payable fat fingered the bank account number for the new auditor. Our books are inspected for the first time in 20 years.", -20));
            BearEvents.Add(new BearEvent("PG&Enron", "Earthquake on the San Andreas fault sends half the coastline, and 90% of our customer base, into the Pacific Ocean.", -15));
            BearEvents.Add(new BearEvent("PG&Enron", "Another dry winter, another summer of wildfires sparked by faulty transformers. How could we have ever forseen this?", -25));

            BearEvents.Add(new BearEvent("My Mutuals and Me Inc.", "You're not my mutual...", -5));
            BearEvents.Add(new BearEvent("My Mutuals and Me Inc.", "All firms in the 'Small capital business with no slave labor in their supply chain' fund found to utilize slave labor. Fund dissolved.", -5));
            BearEvents.Add(new BearEvent("My Mutuals and Me Inc.", "The NON-STANDARD and RICH index fund found to be too pricey for small investors.", -5));
            BearEvents.Add(new BearEvent("My Mutuals and Me Inc.", "New York Teacher's Pension Fund pulls out of My Mutals and Me to put all their money in the Roger Stone Hedge Fund.", -5));
            BearEvents.Add(new BearEvent("My Mutuals and Me Inc.", "Robinhood pre-installed on all new iPhones. Americans lose interest in putting their retirement savings in Mutual Funds.", -5));
            
            BearEvents.Add(new BearEvent("Unionized Submissives LLC", "If I could save the Union without ungagging any subs, I would do it!", -10));
            BearEvents.Add(new BearEvent("Unionized Submissives LLC", "If I could save the Union without ungagging any subs, I would do it!", -10));
            BearEvents.Add(new BearEvent("Unionized Submissives LLC", "If I could save the Union without ungagging any subs, I would do it!", -10));
            BearEvents.Add(new BearEvent("Unionized Submissives LLC", "If I could save the Union without ungagging any subs, I would do it!", -10));
            BearEvents.Add(new BearEvent("Unionized Submissives LLC", "If I could save the Union without ungagging any subs, I would do it!", -10));

            BearEvents.Add(new BearEvent("My Pillow, Your Pillow, We're All Pillows!", "My pillow, my pillow, what have ye done!", -5));
            BearEvents.Add(new BearEvent("My Pillow, Your Pillow, We're All Pillows!", "Founder and owner convinced by current US president that they should run for Senate.", -10));
            BearEvents.Add(new BearEvent("My Pillow, Your Pillow, We're All Pillows!", "Class action lawsuit after its discovered that new cost saving pillow fillings combust with greater intensitiy than gasoline.", -15));
            BearEvents.Add(new BearEvent("My Pillow, Your Pillow, We're All Pillows!", "Small manufacturing defect results in hundreds of infants becoming swallowed and trapped within 'My Pillow, Your Pillow, We're All Pillows!'", -15));
            BearEvents.Add(new BearEvent("My Pillow, Your Pillow, We're All Pillows!", "Factory in Laos forgot to install suicide nets outside the windows of the worker dormitories.", -5));


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
            "bear...", "bear...","bear...","bear...","bear...","BULL BULL BULL","BULL BULL BULL","BULL BULL BULL","BULL BULL BULL","BULL BULL BULL","BULL BULL BULL","BULL BULL BULL"
        };

        public void CreatePlayer(string playerName, string connectionid)
        {
            playerList.Add(new player(playerName, connectionid, 10000, 0, false, false, false, false));
            /*
            bool exists = playerList.Any(x => x.ConnectionId == connectionid);
            bool nameExists = playerList.Any(x => x.Name == playerName);

            player RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);

            if (exists && !nameExists)
            {
                RequestingPlayer.Name = playerName;
            }
            else if (!exists)
            {
                playerList.Add(new player(playerName, connectionid, 10000, 0, false, false, false));
            }
            */

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
            if (isEveryoneReady || EndTheGame && EndTheGameCount < 0)
            {
                //stalkList = JsonConvert.DeserializeAnonymousType(stalksListToJson, stalkList);
                if (EndTheGame)
                {
                    EndTheGameCount++;
                }
                year++;
                bullBear = BullBear();
                adjustStalks(bullBear);
                thisYear = GetYearEvent(bullBear);
                CheckBankruptcy();
                splitStalks();
                PayDividend(bullBear);
                // add Loan adjustments for year end here
                if (AllLoans.Count > 0)
                {
                    AdjustLoans();
                }
                GetNetWorth();
                foreach (player otherplayer in playerList)
                {
                    otherplayer.StartNewYear = false;
                    otherplayer.EndGame = false;
                }

            }

            await Clients.All.SendAsync("New Year", year, bullBear, JsonConvert.SerializeObject(stalkList), JsonConvert.SerializeObject(stalksOwned), JsonConvert.SerializeObject(playerList), JsonConvert.SerializeObject(thisYear), JsonConvert.SerializeObject(stalksBankrupt), JsonConvert.SerializeObject(AllLoans));
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

            await Clients.Caller.SendAsync("Bought Stalks", JsonConvert.SerializeObject(playerList), JsonConvert.SerializeObject(stalksOwned));
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
                if (AllLoans.Count > 0)
                {
                    foreach (Loan loan in AllLoans)
                    {
                        if (loan.ConnectionId == player.ConnectionId)
                        {
                            netWorthValue -= loan.LoanBalance;
                            Console.WriteLine("Lowered " + loan.PlayerName + "'s network due to loan by " + loan.LoanBalance.ToString());
                        }
                    }
                }
                player.NetWorth = netWorthValue;
                /*if (player.NetWorth < 0)
                {
                    player.NetWorth = 0;
                }*/
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
            stalksBankrupt.Clear();
            for (int i = stalkList.Count - 1; i >= 0; i--)
            {
                if (stalkList[i].PricePerShare <= 0)
                {

                    stalksOwned.RemoveAll(x => x.StalkName == stalkList[i].Name);
                    BearEvents.RemoveAll(x => x.StalkName == stalkList[i].Name);
                    BullEvents.RemoveAll(x => x.StalkName == stalkList[i].Name);
                    stalksBankrupt.Add(stalkList[i].Name);
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
                    stalk.YearlyChange = rng.Next((stalk.maxChangeBear * -1), (stalk.minChangeBear * -1));
                    if (stalk.Name != "BERRY BONDS FROM DIE HARD")
                    {
                        // Adding random chance that the stalk can DECREASE by as much as 10 in a bull year
                        int randomChance = rng.Next(0, 10);
                        if (randomChance > 8)
                        {
                            stalk.YearlyChange = rng.Next(0, 6);
                        }

                    }
                    stalk.PricePerShare += stalk.YearlyChange;

                }
            }
            else
            {
                foreach (stalk stalk in stalkList)
                {
                    stalk.YearlyChange = rng.Next(stalk.minChangeBull, stalk.maxChangeBull);

                    if (stalk.Name != "BERRY BONDS FROM DIE HARD")
                    {
                        // Adding random chance that the stalk can DECREASE by as much as 10 in a bull year
                        int randomChance = rng.Next(0, 10);
                        if (randomChance > 8)
                        {
                            stalk.YearlyChange = rng.Next(-10,1);
                        }
                        
                    }
                    stalk.PricePerShare += stalk.YearlyChange;
                }
            }

        }

        // If a stalk price is >150, split the stalk 2-for-1. Divide stalk price by 2, then double all owned stalks
        public void splitStalks()
        {
            foreach (stalk stalk in stalkList)
            {
                if (stalk.PricePerShare > 150)
                {
                    stalk.PricePerShare /= 2;
                    stalk.Split = true;
                    foreach (StalksOwned owned in stalksOwned)
                    {
                        if (string.Equals(owned.StalkName, stalk.Name))
                        {
                            owned.TotalStalks *= 2;
                        }
                    }
                }
                else 
                {
                    stalk.Split = false;
                }
            }
        }
        public async Task GetLoanTerms()
        {
            player RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            int MaxLoanAmount = (int)(RequestingPlayer.NetWorth * 0.25);
            int MinPayment = LoanMinimumPayment(MaxLoanAmount, 10);
            await Clients.Caller.SendAsync("Loan Terms Sent", MaxLoanAmount, MinPayment);
        }

        public int LoanMinimumPayment(int LoanAmount, int YearsRemaining)
        {
            double firstHalf = Math.Pow(1.06, YearsRemaining);
            firstHalf -= 1;
            double secondHalf = Math.Pow(1.06, YearsRemaining);
            secondHalf = secondHalf * 0.06;

            double denominator = firstHalf / secondHalf;
            Console.WriteLine("Calculating the minimum payment. Loanbalance: " + LoanAmount + " Years remaining: " + YearsRemaining);
            int MinPayment = (int)(LoanAmount / denominator);
            return MinPayment;
        }
        public async Task PurchaseLoan(string loanPurchase)
        {
            player RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            string purchaseError = null;
            LoanPurchaseOrPayment playerPurchase = new LoanPurchaseOrPayment(null, null, null, 0);
            playerPurchase = JsonConvert.DeserializeAnonymousType(loanPurchase, playerPurchase);
            playerPurchase.DollarAmount = Math.Abs(playerPurchase.DollarAmount);

            // Check if the player is allowed to make the pruchase
            bool allowPurchase = true;
            int MaxLoanAmount = (int)(RequestingPlayer.NetWorth * 0.25);
            if (playerPurchase.DollarAmount > MaxLoanAmount)
            {
                allowPurchase = false;
                purchaseError = "Requested loan exceeds maximum loan amount.";
                Console.WriteLine(purchaseError);
            }
            else if (playerPurchase.DollarAmount < 100)
            {
                allowPurchase = false;
                purchaseError = "The minimum loan you can purchase is $100.";
                Console.WriteLine(purchaseError);
            }

            if (allowPurchase)
            {
                int MinPayment = LoanMinimumPayment(playerPurchase.DollarAmount, 10);
                Loan newLoan = new Loan(RequestingPlayer.Name, RequestingPlayer.ConnectionId, playerPurchase.DollarAmount, MinPayment, 11, true, 0);
                Loan newLoan2 = new Loan(RequestingPlayer.Name, RequestingPlayer.ConnectionId, playerPurchase.DollarAmount, MinPayment, 11, true, 0);
                RequestingPlayer.HasLoan = true;
                RequestingPlayer.CashOnHand += playerPurchase.DollarAmount;
                Console.WriteLine(newLoan.PlayerName + "'s balance is " + newLoan.LoanBalance.ToString() + " after purchasing their first loan");
                Console.WriteLine ("Adding new loan. Player: " + newLoan.PlayerName + " loan balance " + newLoan.LoanBalance.ToString() + " min payment " + newLoan.MinPayment.ToString());
                Console.WriteLine("playerPurchase.DollarAmount = " + playerPurchase.DollarAmount.ToString());
                AllLoans.Add(newLoan);
            }

            await Clients.Caller.SendAsync("Loan Purchased", purchaseError, JsonConvert.SerializeObject(playerList), JsonConvert.SerializeObject(AllLoans));
        }
        public void AdjustLoans()
        {
            foreach (Loan loan in AllLoans)
            {
                if (loan.YearsRemaining < 11)
                {
                    loan.YearsRemaining--;
                    if (!loan.PaidThisYear)
                    {
                        loan.MissedPayments++;
                        if (loan.MissedPayments == 1 && !EndTheGame)
                        {
                            Console.WriteLine(loan.PlayerName + "'s balance is " + loan.LoanBalance.ToString() + " before missed payment penalty 1");
                            loan.LoanBalance = (int)(loan.LoanBalance * 1.10);
                            Console.WriteLine(loan.PlayerName + "'s balance is " + loan.LoanBalance.ToString() + " after missed payment penalty 1");
                        }
                        else if (loan.MissedPayments == 2 && !EndTheGame)
                        {
                            Console.WriteLine(loan.PlayerName + "'s balance is " + loan.LoanBalance.ToString() + " before missed payment penalty 2");
                            loan.LoanBalance = (int)(loan.LoanBalance * 1.15);
                            Console.WriteLine(loan.PlayerName + "'s balance is " + loan.LoanBalance.ToString() + " after missed payment penalty 2");
                        }
                        else if (loan.MissedPayments >= 3 && !EndTheGame)
                        {
                            player BankruptPlayer = playerList.First(x => x.ConnectionId == loan.ConnectionId);
                            stalksOwned.RemoveAll(x => x.PlayerName == loan.ConnectionId);
                            BankruptPlayer.CashOnHand = 0;
                            BankruptPlayer.NetWorth = 0;
                            //BankruptPlayer.HasLoan = false;
                            loan.LoanBalance = 0;
                            loan.MinPayment = 0;
                        }
                        loan.MinPayment = LoanMinimumPayment(loan.LoanBalance, loan.YearsRemaining);
                    }
                    Console.WriteLine(loan.PlayerName + "'s balance is " + loan.LoanBalance.ToString() + " before interest");
                    loan.LoanBalance = (int)(loan.LoanBalance * 1.06);
                    Console.WriteLine(loan.PlayerName + "'s balance is " + loan.LoanBalance.ToString() + " after interest");
                    //loan.MinPayment = LoanMinimumPayment(loan.LoanBalance, loan.YearsRemaining);
                }
                else if (loan.YearsRemaining == 11)
                {
                    Console.WriteLine(loan.PlayerName + "'s balance is " + loan.LoanBalance.ToString() + " before interest");
                    loan.LoanBalance = (int)(loan.LoanBalance * 1.06);
                    Console.WriteLine(loan.PlayerName + "'s balance is " + loan.LoanBalance.ToString() + " after interest");
                    loan.YearsRemaining--;
                }
            }
        }
        public async Task PlayerDidntPayLoan(string playerLoanToJson)
        {
            Loan playerLoan = new Loan(null, null, 0, 0, 0, false, 0);
            playerLoan = JsonConvert.DeserializeAnonymousType(playerLoanToJson, playerLoan);
            foreach (Loan loan in AllLoans)
            {
                if (loan.ConnectionId == playerLoan.ConnectionId)
                {
                    loan.PaidThisYear = false;
                }
            }
            await Clients.Caller.SendAsync("Didn't Pay Loan", JsonConvert.SerializeObject(AllLoans));
        }
        public async Task PayLoan(string playerLoanPaymentToJson)
        {
            Console.WriteLine("Initiating loan payment");
            player RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            string paymentError = null;
            LoanPurchaseOrPayment playerPayment = new LoanPurchaseOrPayment(null, null, null, 0);
            playerPayment = JsonConvert.DeserializeAnonymousType(playerLoanPaymentToJson, playerPayment);
            Loan playerLoan = AllLoans.First(x => x.ConnectionId == RequestingPlayer.ConnectionId);
            playerPayment.DollarAmount = Math.Abs(playerPayment.DollarAmount);

            foreach (player player in playerList)
            {
                if (player.ConnectionId == RequestingPlayer.ConnectionId)
                {
                    if ((playerPayment.DollarAmount >= playerLoan.MinPayment) && (playerPayment.DollarAmount <= player.CashOnHand))
                    {
                        foreach (Loan loan in AllLoans)
                        {
                            if (loan.ConnectionId == RequestingPlayer.ConnectionId)
                            {
                                loan.PaidThisYear = true;
                                if (playerPayment.DollarAmount < loan.LoanBalance)
                                {
                                    Console.WriteLine(loan.PlayerName + " has a loan balance of " + loan.LoanBalance.ToString());
                                    loan.LoanBalance -= playerPayment.DollarAmount;
                                    player.CashOnHand -= playerPayment.DollarAmount;
                                    // Calculate new minimum payment?
                                    loan.MinPayment = LoanMinimumPayment(loan.LoanBalance, (loan.YearsRemaining - 1));
                                }
                                else if (playerPayment.DollarAmount == loan.LoanBalance)
                                {
                                    Console.WriteLine(loan.PlayerName + " has a loan balance of " + loan.LoanBalance.ToString());
                                    loan.LoanBalance -= playerPayment.DollarAmount;
                                    player.CashOnHand -= playerPayment.DollarAmount;
                                    loan.LoanBalance = 0;
                                    loan.MinPayment = 0;
                                    //AllLoans.RemoveAll(x => x.ConnectionId == player.ConnectionId);
                                    //player.HasLoan = false;
                                }
                                else if (playerPayment.DollarAmount > loan.LoanBalance)
                                {
                                    player.CashOnHand -= loan.LoanBalance;
                                    loan.LoanBalance = 0;
                                    loan.MinPayment = 0;
                                    //AllLoans.RemoveAll(x => x.ConnectionId == player.ConnectionId);
                                    //player.HasLoan = false;
                                }

                                Console.WriteLine("Calculating new minimum payment. Loan balance: " + loan.LoanBalance + " Years remaining: " + (loan.YearsRemaining - 1).ToString() + " New miniumum: " + loan.MinPayment);
                                Console.WriteLine(loan.PlayerName + " paid " + playerPayment.DollarAmount.ToString() + " toward their loan. New balance is " + loan.LoanBalance.ToString());
                            }
                        }
                        //playerLoan.PaidThisYear = true;
                        //playerLoan.LoanBalance -= playerPayment.DollarAmount;

                    }
                    else if (playerPayment.DollarAmount < playerLoan.MinPayment)
                    {
                        paymentError = "Payment is below the minimum payment.";
                        foreach (Loan loan in AllLoans)
                        {
                            if (loan.ConnectionId == RequestingPlayer.ConnectionId)
                            {
                                loan.PaidThisYear = false;
                            }
                        }
                        Console.WriteLine("Payment is below the minimum payment " + playerPayment.DollarAmount);
                    }
                    else if (playerPayment.DollarAmount > player.CashOnHand)
                    {
                        paymentError = "You do not have enough money to pay that amount";
                        foreach (Loan loan in AllLoans)
                        {
                            if (loan.ConnectionId == RequestingPlayer.ConnectionId)
                            {
                                loan.PaidThisYear = false;
                            }
                        }
                    }
                }
            }


            
            Console.WriteLine("Loan payment complete");

            await Clients.Caller.SendAsync("Loan Paid", paymentError, JsonConvert.SerializeObject(AllLoans), JsonConvert.SerializeObject(playerList));
        }
    }



}
