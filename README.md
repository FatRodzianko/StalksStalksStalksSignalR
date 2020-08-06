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
