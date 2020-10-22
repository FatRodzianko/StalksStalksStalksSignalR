using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using StalksStalksStalksSignalR.Shared;

namespace StalksStalksStalksSignalR.Server.Hubs
{
    public class CardConquestHub : Hub
    {
        // The cards
        public Card power5 = new Card("Power 5", 5, 2, 2, @" ---------- 
| POWER: 5 |
| ATT:   2 |
| DEF:   2 |
 ---------- 
");
        public Card power4 = new Card("Power 4", 4, 2, 1, @" ---------- 
| POWER: 4 |
| ATT:   2 |
| DEF:   1 |
 ---------- 
");
        public Card power3 = new Card("Power 3", 3, 3, 0, @" ---------- 
| POWER: 3 |
| ATT:   3 |
| DEF:   0 |
 ---------- 
");
        public Card power2 = new Card("Power 2", 2, 0, 2, @" ---------- 
| POWER: 2 |
| ATT:   0 |
| DEF:   2 |
 ---------- 
");
        public Card power1 = new Card("Power 1", 1, 1, 1, @" ---------- 
| POWER: 1 |
| ATT:   1 |
| DEF:   1 |
 ---------- 
");


        public static List<CCPlayer> playerList = new List<CCPlayer>();
        public static List<Rooms> roomList = new List<Rooms>();

        public override Task OnConnectedAsync()
        {
            CreatePlayer("", Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Remove the player from any game rooms
            CCPlayer RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            Rooms roomJoined = new Rooms(null, null);
            try {
                roomJoined = roomList.First(x => x.RoomName == RequestingPlayer.RoomName);
            }
            catch (Exception e) { }
            if (!String.IsNullOrEmpty(roomJoined.RoomName))
            {
                //Rooms roomJoined = roomList.First(x => x.RoomName == RequestingPlayer.RoomName);
                CCPlayer playerToRemove = new CCPlayer(null, null, false, null, 0, 0, 0, 0, null, null, 0);
                try
                {
                    playerToRemove = roomJoined.Players.Where(x => x.PlayerName == RequestingPlayer.PlayerName).First();
                }
                catch (Exception e) { }
                roomJoined.Players.Remove(playerToRemove);
                bool isRoomEmpty = IsRoomEmpty(RequestingPlayer.RoomName);
                if (isRoomEmpty)
                {
                    roomList.RemoveAll(x => x.RoomName == RequestingPlayer.RoomName);
                }
                if (!String.IsNullOrEmpty(RequestingPlayer.RoomName))
                {
                    LeaveRoom(RequestingPlayer.RoomName);
                }
            }
            
            // remove the player from the player list
            playerList.RemoveAll(x => x.ConnectionId == Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        public bool IsRoomEmpty(string roomName)
        {
            Rooms roomToCheck = roomList.First(x => x.RoomName == roomName);
            if (roomToCheck.Players.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void JoinRoom(string roomName)
        {
            bool roomExist = roomList.Any(x => x.RoomName == roomName);
            CCPlayer RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);

            if (!roomExist)
            {
                Console.WriteLine("creating new room");
                List<CCPlayer> roomPlayers = new List<CCPlayer>();
                Rooms newRoom = new Rooms(roomName, roomPlayers);
                newRoom.Players.Add(RequestingPlayer);
                roomList.Add(newRoom);
                RequestingPlayer.RoomName = roomName;
                /*
                Rooms newRoom = roomList.First(x => x.RoomName == roomName);
                newRoom.Players.Add(RequestingPlayer);
                RequestingPlayer.RoomName = roomName;
                */
            }
            else
            {
                Console.WriteLine("Joining existing room");
                Rooms roomToJoin = roomList.First(x => x.RoomName == roomName);
                if (roomToJoin.Players.Count < 2)
                {
                    List<CCPlayer> roomPlayers = (List<CCPlayer>)roomToJoin.Players;
                    bool playerAlreadyJoined = roomPlayers.Any(x => x.PlayerName == RequestingPlayer.PlayerName);
                    if (!playerAlreadyJoined)
                    {
                        roomToJoin.Players.Add(RequestingPlayer);
                        RequestingPlayer.RoomName = roomName;
                    }
                }

            }
            Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public void LeaveRoom(string roomName)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
        public async Task CreateOrJoinRoom(string roomName)
        {
            if (!String.IsNullOrEmpty(roomName))
            {
                JoinRoom(roomName);
            }
            CCPlayer RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            await Clients.Caller.SendAsync("Room Joined", JsonConvert.SerializeObject(playerList), JsonConvert.SerializeObject(RequestingPlayer), JsonConvert.SerializeObject(roomList));
        }

        public void CreatePlayer(string playername, string connectionid)
        {
            List<Card> newCards = new List<Card> { power5, power4, power3, power2, power1 };
            List<Card> emptyCards = new List<Card>();
            playerList.Add(new CCPlayer(playername, connectionid, false, "", 4, 6, 0, 0, newCards, emptyCards, 0));
        }

        public async Task CreatePlayerName(string playername)
        {
            bool userNameTaken = playerList.Any(x => x.PlayerName == playername);
            CCPlayer RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            if (!userNameTaken)
            {
                RequestingPlayer.PlayerName = playername;
            }
            else if (userNameTaken)
            {
                RequestingPlayer.PlayerName = "";
            }

            await Clients.Caller.SendAsync("Player Name Added", JsonConvert.SerializeObject(playerList), JsonConvert.SerializeObject(RequestingPlayer), JsonConvert.SerializeObject(roomList));
        }
        public async Task GetUsers()
        {
            await Clients.All.SendAsync("Returned users", JsonConvert.SerializeObject(playerList));
        }
        public async Task GetRooms()
        {
            await Clients.All.SendAsync("Returned rooms", JsonConvert.SerializeObject(roomList));
        }
        public async Task ReadyUp()
        {
            CCPlayer RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            Rooms playerRoom = roomList.First(x => x.RoomName == RequestingPlayer.RoomName);
            RequestingPlayer.Ready = true;
            playerRoom.Players.Where(x => x.PlayerName == RequestingPlayer.PlayerName).Select(x => { x.Ready = true; return x; });
            bool arePlayersReady = CheckIfPlayersReady();
            CCGame playerGame = new CCGame(1, "", null, null, "");

            // setup the game if players are all ready
            if (arePlayersReady)
            {
                playerGame = InitializeGame(playerRoom, RequestingPlayer);
            }

            await Clients.Group(RequestingPlayer.RoomName).SendAsync("User Readied", JsonConvert.SerializeObject(playerList), arePlayersReady, JsonConvert.SerializeObject(playerRoom.Players), JsonConvert.SerializeObject(playerGame));
        }
        public bool CheckIfPlayersReady()
        {
            CCPlayer RequestingPlayer = playerList.First(x => x.ConnectionId == Context.ConnectionId);
            Rooms playerRoom = roomList.First(x => x.RoomName == RequestingPlayer.RoomName);
            if (playerRoom.Players.Count == 2)
            {
                if (playerRoom.Players.Any(x => x.Ready == false))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public CCGame InitializeGame(Rooms playerRoom, CCPlayer RequestingPlayer)
        {
            // Get info for the other player
            CCPlayer player2 = playerRoom.Players.First(x => x.PlayerName != RequestingPlayer.PlayerName);

            //Intialize the armies
            List<Army> playerArmies = new List<Army>();
            Army army1 = new Army(RequestingPlayer.PlayerName, 0, 0, null);
            Army army2 = new Army(player2.PlayerName, 0, 0, null);
            playerArmies.Add(army1);
            playerArmies.Add(army2);

            // Create the game
            CCGame playerGame = new CCGame(1, "Unit Selection", playerArmies, playerRoom, "");
            //playerGame.Room.Players.Where(x => x.PlayerName == RequestingPlayer.PlayerName).Select(x => { x.Ready = false; return x; });
            //playerGame.Room.Players.Where(x => x.PlayerName == player2.PlayerName).Select(x => { x.Ready = false; return x; });

            foreach (CCPlayer player in playerGame.Room.Players)
            {
                player.Ready = false;
            }
            return playerGame;
        }
        public async Task SubmitUnitsToGroup(string playerGameToJson)
        {
            CCGame playerGame = new CCGame(0, null, null, null, "");
            playerGame = JsonConvert.DeserializeAnonymousType(playerGameToJson, playerGame);

            CCPlayer RequestingPlayer = playerGame.Room.Players.First(x => x.ConnectionId == Context.ConnectionId);
            Army RequestingArmy = playerGame.Armies.First(x => x.PlayerName == RequestingPlayer.PlayerName);

            foreach (CCPlayer player in playerGame.Room.Players)
            {
                if (player.PlayerName == RequestingPlayer.PlayerName)
                {
                    player.AvailableInf -= RequestingArmy.Infantry;
                    player.AvailableTanks -= RequestingArmy.Tanks;
                }
            }

            foreach (CCPlayer player in playerGame.Room.Players)
            {
                Console.WriteLine("Submitted units " + player.PlayerName + " : " + player.Ready.ToString());
            }
            if (playerGame.Room.Players.All(x => x.Ready == true))
            {
                playerGame.Phase = "Card Selection";
                foreach (CCPlayer player in playerGame.Room.Players)
                {
                    player.Ready = false;
                }
            }

            await Clients.Group(playerGame.Room.RoomName).SendAsync("Units Submitted To Group", JsonConvert.SerializeObject(playerGame));
        }
        public async Task SubmitCardToGroup(string playerGameToJson)
        {
            CCGame playerGame = new CCGame(0, null, null, null, "");
            playerGame = JsonConvert.DeserializeAnonymousType(playerGameToJson, playerGame);
            foreach (CCPlayer player in playerGame.Room.Players)
            {
                Console.WriteLine("Submitted card " + player.PlayerName + " : " + player.Ready.ToString());
            }
            if (playerGame.Room.Players.All(x => x.Ready == true))
            {
                playerGame = WinnerOfBattle(playerGame);
                playerGame.Phase = "Battle Resolution";
                foreach (CCPlayer player in playerGame.Room.Players)
                {
                    player.Ready = false;
                }
            }
            await Clients.Group(playerGame.Room.RoomName).SendAsync("Card Submitted To Group", JsonConvert.SerializeObject(playerGame));
        }
        public CCGame WinnerOfBattle(CCGame playerGame)
        {
            Console.WriteLine("Chosing a winning player");
            CCPlayer RequestingPlayer = playerGame.Room.Players.First(x => x.ConnectionId == Context.ConnectionId);

            Army RequestingPlayerArmy = playerGame.Armies.First(x => x.PlayerName == RequestingPlayer.PlayerName);
            Army OtherPlayerArmy = playerGame.Armies.First(x => x.PlayerName != RequestingPlayer.PlayerName);

            int RequestingPlayerTotalPower = (RequestingPlayerArmy.Tanks * 2) + RequestingPlayerArmy.Infantry + RequestingPlayerArmy.PlayerCard.Power;
            int OtherPlayerTotalPower = (OtherPlayerArmy.Tanks * 2) + OtherPlayerArmy.Infantry + OtherPlayerArmy.PlayerCard.Power;

            if (RequestingPlayerTotalPower > OtherPlayerTotalPower)
            {
                playerGame.WinningPlayer = RequestingPlayerArmy.PlayerName;
            }
            else if (RequestingPlayerTotalPower == OtherPlayerTotalPower)
            {
                if (RequestingPlayerArmy.PlayerCard.Power > OtherPlayerArmy.PlayerCard.Power)
                {
                    playerGame.WinningPlayer = RequestingPlayerArmy.PlayerName;
                }
                else if (RequestingPlayerArmy.PlayerCard.Power < OtherPlayerArmy.PlayerCard.Power)
                {
                    playerGame.WinningPlayer = OtherPlayerArmy.PlayerName;
                }
                else if (RequestingPlayerArmy.PlayerCard.Power == OtherPlayerArmy.PlayerCard.Power)
                {
                    if (RequestingPlayerArmy.Infantry > OtherPlayerArmy.Infantry)
                    {
                        playerGame.WinningPlayer = RequestingPlayerArmy.PlayerName;
                    }
                    else if (RequestingPlayerArmy.Infantry < OtherPlayerArmy.Infantry)
                    {
                        playerGame.WinningPlayer = OtherPlayerArmy.PlayerName;
                    }
                    else if (RequestingPlayerArmy.Infantry == OtherPlayerArmy.Infantry)
                    {
                        playerGame.WinningPlayer = "tie";
                    }
                }
            }
            else if (RequestingPlayerTotalPower < OtherPlayerTotalPower)
            {
                playerGame.WinningPlayer = OtherPlayerArmy.PlayerName;
            }
            return playerGame;
        }
        public async Task BattleCleanUpToGroup(string playerGameToJson)
        {
            Console.WriteLine("called BattleCleanUpToGroup");
            //Game battleResults = new Game(0, null, null, null, "");
            CCGame playerGame = new CCGame(0, null, null, null, "");
            playerGame = JsonConvert.DeserializeAnonymousType(playerGameToJson, playerGame);
            Console.WriteLine("CChecking if all players are ready");
            bool isGameOver = false;
            bool isLastRound = false;
            string noUnitsError = null;

            foreach (CCPlayer player in playerGame.Room.Players)
            {
                Console.WriteLine(player.PlayerName + " : " + player.Ready.ToString());
            }
            if (playerGame.Room.Players.All(x => x.Ready == true))
            {
                Console.WriteLine("initiating battle cleanup");
                //Move disabled units into available units
                Console.WriteLine("moving disabled units to available");
                playerGame = MakeUnavailableAvailable(playerGame);
                /*foreach (Player player in playerGame.Room.Players)
                {
                    player.AvailableInf += player.UnavailableInf;
                    player.AvailableTanks += player.UnavailableTanks;
                    player.UnavailableInf = 0;
                    player.UnavailableTanks = 0;
                    // Increase player's win total, if they won
                    if (player.PlayerName == playerGame.WinningPlayer)
                    {
                        player.TotalWins++;
                    }
                }*/
                // Resolve the Attack / Defense effects of the cards
                playerGame = ResolveAttackDefense(playerGame);
                // Move remaining armies into the players "unavailable" units and discard cards
                Console.WriteLine("moving army into disabled units");
                foreach (CCPlayer player in playerGame.Room.Players)
                {
                    foreach (Army army in playerGame.Armies)
                    {
                        if (army.PlayerName == player.PlayerName)
                        {
                            player.UnavailableInf += army.Infantry;
                            player.UnavailableTanks += army.Tanks;
                            army.Infantry = 0;
                            army.Tanks = 0;
                            Console.WriteLine("Discarding played card");
                            player.DiscardPile.Add(army.PlayerCard);
                            player.Hand.RemoveAll(x => x.CardName == army.PlayerCard.CardName);
                            if (player.Hand.Count == 0)
                            {
                                Console.WriteLine("Hand depleted. Resetting the player's hand");
                                foreach (Card card in player.DiscardPile)
                                {
                                    player.Hand.Add(card);
                                }
                                player.DiscardPile.Clear();
                            }
                        }
                    }
                }
                // Increase winning player's win total
                foreach (CCPlayer player in playerGame.Room.Players)
                {
                    if (player.PlayerName == playerGame.WinningPlayer)
                    {
                        player.TotalWins++;
                    }
                }
                // check if any players have no units available
                if (playerGame.Room.Players.Any(x => x.AvailableInf == 0 && x.AvailableTanks == 0))
                {
                    // check if both players have no available units
                    if (playerGame.Room.Players.All(x => x.AvailableInf == 0 && x.AvailableTanks == 0))
                    {
                        playerGame.RoundNumber++;
                        noUnitsError = "Both players had no available units. Round " + playerGame.RoundNumber.ToString() + " was forfeited by both and skipped";
                    }
                    else if (playerGame.Room.Players.Any(x => x.AvailableInf == 0 && x.AvailableTanks == 0 && x.UnavailableInf == 0 && x.UnavailableTanks == 0))
                    {
                        // Check if both players are completely out of units. I don't think this is possible?
                        if (playerGame.Room.Players.All(x => x.AvailableInf == 0 && x.AvailableTanks == 0 && x.UnavailableInf == 0 && x.UnavailableTanks == 0))
                        {
                            noUnitsError = "Mutually assured destruction. All units in the game are destroyed. Game over.";
                            playerGame.RoundNumber = 10;
                        }
                        else
                        {
                            string playerWithNoAvilalbeUnits = playerGame.Room.Players.First(x => x.AvailableInf == 0 && x.AvailableTanks == 0 && x.UnavailableInf == 0 && x.UnavailableTanks == 0).PlayerName;
                            noUnitsError = playerWithNoAvilalbeUnits + " has lost all units. They foreit all remaining rounds.";
                            int currentRoundNumber = playerGame.RoundNumber;
                            for (int i = currentRoundNumber; i < 10; i++)
                            {
                                playerGame.RoundNumber++;
                                foreach (CCPlayer player in playerGame.Room.Players)
                                {
                                    if (player.PlayerName != playerWithNoAvilalbeUnits)
                                    {
                                        player.TotalWins++;
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        string playerWithNoAvilalbeUnits = playerGame.Room.Players.First(x => x.AvailableInf == 0 && x.AvailableTanks == 0).PlayerName;
                        playerGame.RoundNumber++;
                        noUnitsError = playerWithNoAvilalbeUnits + " had no units. They forfeited round " + playerGame.RoundNumber.ToString();
                        foreach (CCPlayer player in playerGame.Room.Players)
                        {
                            if (player.PlayerName != playerWithNoAvilalbeUnits)
                            {
                                player.TotalWins++;
                            }
                        }
                    }

                    playerGame = MakeUnavailableAvailable(playerGame);

                }
                // Reset the winning player
                playerGame.WinningPlayer = null;
                // Increase the round number

                if (playerGame.RoundNumber == 10)
                {
                    isGameOver = true;
                    isLastRound = false;
                }
                else if (playerGame.RoundNumber == 9)
                {
                    isLastRound = true;
                }
                // Increase the round number
                playerGame.RoundNumber++;
                // reset the phase to Card Selection
                playerGame.Phase = "Unit Selection";
                // reset player status
                foreach (CCPlayer player in playerGame.Room.Players)
                {
                    player.Ready = false;
                }
            }
            Console.WriteLine("returning player game from battle cleanup");
            await Clients.Group(playerGame.Room.RoomName).SendAsync("Battle Completed", JsonConvert.SerializeObject(playerGame), isGameOver, isLastRound, noUnitsError);
        }

        public CCGame ResolveAttackDefense(CCGame playerGame)
        {
            Console.WriteLine("resolving attack / defense");
            if (playerGame.WinningPlayer != "tie")
            {
                CCPlayer WinningPlayer = playerGame.Room.Players.First(x => x.PlayerName == playerGame.WinningPlayer);
                CCPlayer LosingPlayer = playerGame.Room.Players.First(x => x.PlayerName != playerGame.WinningPlayer);

                Army WinningArmy = playerGame.Armies.First(x => x.PlayerName == playerGame.WinningPlayer);
                Army LosingArmy = playerGame.Armies.First(x => x.PlayerName != playerGame.WinningPlayer);

                if (WinningArmy.PlayerCard.AttackValue > 0)
                {
                    int TotalDamage = WinningArmy.PlayerCard.AttackValue - LosingArmy.PlayerCard.DefenseValue;
                    if (TotalDamage > 0)
                    {

                        //remove tanks first
                        LosingArmy.Tanks -= TotalDamage;
                        if (LosingArmy.Tanks < 0)
                        {
                            TotalDamage = Math.Abs(LosingArmy.Tanks);
                            LosingArmy.Tanks = 0;
                        }
                        else
                        {
                            TotalDamage = 0;
                        }
                        LosingArmy.Infantry -= TotalDamage;
                        if (LosingArmy.Infantry < 0)
                        {
                            LosingArmy.Infantry = 0;
                        }

                        foreach (Army army in playerGame.Armies)
                        {
                            if (army.PlayerName == LosingPlayer.PlayerName)
                            {
                                army.Tanks = LosingArmy.Tanks;
                                army.Infantry = LosingArmy.Infantry;
                            }
                        }
                    }
                }
            }
            return playerGame;
        }
        public CCGame MakeUnavailableAvailable(CCGame playerGame)
        {
            foreach (CCPlayer player in playerGame.Room.Players)
            {
                player.AvailableInf += player.UnavailableInf;
                player.AvailableTanks += player.UnavailableTanks;
                player.UnavailableInf = 0;
                player.UnavailableTanks = 0;
                // Increase player's win total, if they won
            }
            return playerGame;
        }

    }
}
