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

### Special Events
Every year there is a `Special Event` that will affect one Stalk. There will be some flavor text, and then the Stalk price will recieve an additional increase or decrease to its price depending on the event. In Year 0, there is no special event and instead it just wishes you luck in being the best stalk investor.

![](https://github.com/FatRodzianko/StalksStalksStalksSignalR/blob/master/How-To-Play/9-special-events.JPG?raw=true)
