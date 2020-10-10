# StalksStalksStalksSignalR
 Creating a game to learn Blazor or something

## Installation
On Ubuntu:

Install dotnet sdk and framework: https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#2004-

I then had to copy `System.Web.Extensions.dll` from my local system to the linux server.

Then, in the csproj files under both "Client" and "Server" change:
```<HintPath>..\..\..\..\..\..\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Web.Extensions.dll</HintPath>```
So that it points to the location of `System.Web.Extensions.dll` on your linux box

Then, `dotnet run --urls http://<IP.IP.IP.IP>:<PORT>`

## Where to Play
Instead of building the game yourself, you can always just player it here:

http://stalks.fatrodzianko.com/Stalks

Only one "game" can be played at once. Once everyone who is on the `/Stalks` page readies up with a user name, the game begins and will go until everyone decides to quit or they all leave the page. No new games can be started while a game is in progress. There is no limit to the number of players in a single game as far as I can tell. I'm not very knowledgable about how Blazor/SignalR works so I might be wrong about that.

Example of a game in progress
![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/example-stalks-game.PNG?raw=true)

## How to Play
### Starting the game
First, everyone will need to click on `Play Stalks`

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/1-click-play-stalks.JPG?raw=true)

Enter your player name and click on `Ready Up`. Your player name cannot be blank.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/2-enter-player-name.JPG?raw=true)

All players at http://stalks.fatrodzianko.com/Stalks will need to `Ready Up` before the game can begin. Clicking `Ready Up` or `Get Users` will show the list of players conencted to the /Stalks page. If the player's last value is set to `true`, they are ready to play. If it is `false`, that player still needs to click `Ready Up`.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/3-waiting-for-ready-up.JPG?raw=true)

Once all players have their ready status at `true`, anyone can click `Start the mother loving game!!!` to begin.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/4-start-the-game.JPG?raw=true)

### Year 0
After the game is started, everything initializes at Year 0. The StalksStalksStalks theme song will play. To begin the game for real, all players must click on `Start a new year`

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/5-start-year-1.JPG?raw=true)

Next to the `Start a new year` button is a list of players who still need to click on `Start a new year`. After all players have clicked `Start a new Year`, a new year will advance.

### Interface components
Before describing how to play after Year 0, here is what all the different components on the interface are telling you. These will be described in the order they appear from top to bottom.

#### Can I get a yeehaw
StalksStalksStalks allows any user to 'request' a yeehaw by clicking the `Can I get a yeehaw?` button.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/6-can-i-get-a-yeehaw.JPG?raw=true)

After a player clicks `Can I get a yeehaw?`, all players will be alerted that someone has requested a yeehaw.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/7-requesting-a-yeehaw.JPG?raw=true)

Any *other* player can send a yeehaw to the requesting player by clicking `Give <player name> a Yeehaw!`. You *cannot* give yourself a yeehaw. Unfortunately, this means single player games cannot participate in yeehaws. If a player tries to give themselves a yeehaw, the yeehaw request is cancelled.

After `Give <player name> a Yeehaw!` is clicked, the yeehaw sound plays for all players. Everyone will be informed of who graciously sent the yeehaw, and the `Can I get a yeehaw?` button will return for future requests.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/8-yeehaw-sent.JPG?raw=true)

### Start a new year
After you are doing taking your actions, you can click on `Start a new year` to advance the year. This works the same as described above in the Year 0 section. After all players have clicked on `Start a new year`, the year will advance. During the game, this will cause stalks' prices to change and dividends to be paid, which will be covered later.

### Special Events
Every year there is a `Special Event` that will affect one Stalk. There will be some flavor text, and then the Stalk price will recieve an additional increase or decrease to its price depending on the event. In Year 0, there is no special event and instead it just wishes you luck in being the best stalk investor.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/9-special-events.JPG?raw=true)

### Year status
The "Year Status" is the number of the current year and whether it is a `Bear` or `Bull` year (there is no `Bull` or `Bear` status for Year 0). Generally speaking, a `Bear` year means stalk prices go down. A `Bull` year means stalk prices will go up. In any given `Bear` year, there is a 10% chance that an individual stalk's price will actually go up by a small amount. Similarly, in any given `Bull` year, there is a 10% chance that an individual stalk's price will actually go down by a small amount.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/10-bear-or-bull-year.JPG?raw=true)

By default, the likelyhood of a `Bull` year occuring is 60%, and a `Bear` year 40%.

### Stalk Board
The Stalk Board displays all available stalks and their properties.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/11-stalk-board.JPG?raw=true)

**Stalk Name** The name of the stalk / company

**Price Per Share** The price of 1 stalk. This will be used to calculate whether you can afford to buy a stalk, how much money you receive when selling a stalk, and your current networth for the stalks you own. This value changes with each new year (except for `BERRY BOND FROM DIE HARD` whose Price Per Share never changes). All stalks start with a Price Per Share value of `100` in Year 0.

*If the Price Per Share reaches 0*, the stalk is declared `Bankrupt` and is removed from the game. If you owned any stalk that went bankrupt, you lose all money you invested in that stalk.

*If the Price Per Share is greater than 150*, the stalk will `split` 2-for-1. This means that every player that owned the split stalk will have their total owned stalks doubled, and the stalk price will be halved. Example: In Year 1 you buy `10` stalks of `strYker`. In Year 2, the price of `strYker` increases to 160. The price of strYker is then divided by 2 and set at `80`, and your total owned stalks in doubled to `20`.

**Dividend Per Share (bull)** This is the `dividend` paid to players at the start of each new year if they own the stalk. If you bought 1 stalk of `Soprano's HUD Scam` in Year 1, and in Year 2 it was a `Bull` year, you would be paid a $3 dividend for owning that 1 stalk. If you had bought 2 stalks, you would have been paid $6. Your dividend payment is `Number of stalks owned` multiplied by `Dividend per share (bull)`. This value remains static throughout the game.

**Dividend Per Share (bear)** This is the `dividend` paid to players during `bear` years. It follows the same rules as the `bull` year dividends. This value remains static through the game.

**Yearly Change** This is the amount the price of the stalk has changed from the previous year. This will be either a positive or negative value.

### Player Stats
The `Player Stats` section displays your `CashOnHand`, or how much space money you have to buy stalks with, and your `NetWorth`, or the total value of your `CashOnHand` and all the stalks you own. Your `Networth` is calculated at the beginning of each turn. If the value of the stalks you own goes up, your `NetWorth` goes up. If the value of the stalks you own goes down, your `NetWorth` goes down. If you are paid a dividend, your `CashOnHand` goes up, which causes your `NetWorth` to go up. Buying or selling stalks within the same year does not immediately effect your `NetWorth`.

**NetWorth is used to determine the winner of the game.** The player with the highest NetWorth at the end of the game is declared the winner.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/12-player-stats.JPG?raw=true)

### Owned Stalks
After Year 0, you will be able to buy and sell stalks. The stalks you own will be displayed to you, providing you the name of the stalk and the number of stalks you own.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/16-stalks-you-own.JPG?raw=true)

### Scoreboard
The scoreboard displays all player names and their respective `NetWorth`. This can help you keep track of who is winning and losing, who are your rivals and who is a chump. If you're not winning, you gotta pump those numbers up, rookie!

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/13-score-board.JPG?raw=true)

### End the game
At the bottom of the page is the `End it all now?` button. When you click `End it all now?`, you vote to end the game. Once all players within the same year click on `End it all now?`, the game will end.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/14-end-the-game.JPG?raw=true)

After you click on `End it all now?`, your player name will be displayed below the button. This informs other players that you want to end the game. Hopefully players can all agree when to end the game!

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/15-voted-to-end.JPG)

When all players agree to end the game, a new year is started. This means the following:
* the current year number increases by 1
* A new `bear` or `bull` year is declared
* stalks are adjusted
* dividends are paid to players

As an example: On Year 10, all players buy and sell stalks as usual. They then all click on `End it all now?`. The year advances to Year 11. Stalks prices are adjusted, dividends are paid, and the player with the highest `NetWorth` in Year 11 is delcared the winner.

## Playing the Game
When all palyers click on `Start a new year` in Year 0, the game will officially start. The first Year will be Year 1. The year will be randomly choosen to be a `Bear` or `Bull` year. Stalks will adjust their prices. Most importantly, players will now be able to buy and sell stalks.

### Buying and selling stalks
Below the `Stalk Board` you will see the option to `Make a purchase?`. 

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/17-make-a-purchase.JPG?raw=true)

The first dropdown lets you select either `BUY BUY BUY` or `SELL SELL SELL`. As the options suggest, this is where you choose whether you want to buy or sell a stalk.

The second dropdown is a list of all available stalks in the game. Select here the name of the stalk you wish to buy or sell.

The third input field is the number of stalks you wiish to buy or sell. If you want to buy 10 stalks, you enter the number `10`. If you want to sell 192 stalks, you enter the number `192`. The value of the stalks you buy can not exceed your `CashOnHand` value, and the number of stalks you sell can not exceed the number of stalks you own.

Finally, hit the `Submit` button to make your purchase or sale.

Below is an example of a player buying 70 stalks of `strYker`.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/18-buy-stryker.JPG?raw=true)

Now, in their `Owned Stalks` list, the player will see 70 stalks of `strYker`.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/19-strYker-bought.JPG?raw=true)

In the next example, the player already owns 25 stalks of `Teamsters Pension Fund`. They are going to sell 20 stalks of `Teamsters Pension Fund`.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/20-own-25-teamsters-2.JPG?raw=true)

After the player clicks on `Submit`, their number of owned `Teamsters Pension Fund` stalks decreases from 25 to 5, and their `CashOnHand` value increases.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/21-teamsters-sold-20.JPG?raw=true)

### Example (Short) Game
Both `MyPlayerName` and `PlayerNumber2` decide to start the game and begin Year 1. The following `Special Event` and `Year Status` occur.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/22-year-1-new-status-bear-bull.JPG?raw=true)

The `Special Event` results in `My Mutuals and Me Inc.`'s Price Per Share increasing an additional $5 on top of its normal yearly change. The year advanced to Year 1, and the year is a `Bull` year.

The new `Stalk Board` is:

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/23-year-1-stalk-board.JPG)

Note that `My Mutuals and Me Inc.`'s `yearly Change` is 0, but its Price Per Share is 105. This is because the `Special Event` changed `My Mutuals and Me Inc.` by 5.

`MyPlayerName` decides to buy 50 stalks of `strYker`.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/24-myplayername-buy-50-stryker.JPG)

`PlayerNumber2` decides to buy 90 stalks of `My Mutuals and Me Inc.`.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/25-playernumber2-buy-mutuals.JPG)

Both players then click on `Start a new year`. Year 2 is a `Bull` Year. The `Special Event` increases `PG&Enron` by 10. Below is the new `Stalk Board`.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/26-year-2-board.JPG)

`strYker` had a large increase and `Split`. All other stalks have more modest increases.

For `MyPlayerName`, their total owned stalks for `strYker` doubles from 50 to 100 due to the split. Their `NetWorth` is calculated at $11,500. This comes from 100 stalks of `strYker` at 83 per stalk (100 * 83 = 8300) plus their `CashOnHand` of $3200. $8300 + $3200 = $11500.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/27-year-2-myplayername.JPG)

`PlayerNumber` owned 90 stalks of `My Mutuals and Me Inc.` It was a `Bull` year, so they received a dividen of $3 per stalk, or 90 * 3 = $270. Their `CashOnHand` at the end of Year 1 was `550`. 550 + 270 = $820 for their new `CashOnHand`. `My Mutuals and Me Inc.` is at 109 per stalk. 109 * 90 = $9,810. Adding the `CashOnHand` of $820, $9,810 + $820 = $10630.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/28-year2-player2.JPG)

Both players decide they have had enough, and choose to `End it all now?` on Year 2. The year advances to Year 3. It is another `Bull` year. The `Special Event` increases `Soprano's HUD Scam` by 10. `MyPlayerName`'s stalks in `strYker` increase yet again, resulting in `MyPlayerName` declared as the winner.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/29-myplayername-winner-stryker-baby.JPG)

The scoreboard shows that `MyPlayerName` beat `PlayerNumber2` by almost $4,000. That's the power of `strYker` baby!

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/30-year-3-scoreboard.JPG)
