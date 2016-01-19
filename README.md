# Rise of Music
Rise of Music is an application which plays the background music for Rise of Nations (RoN), instead of using the current buggy audio system in game.  It does so with the use of an executable you run before a round of Rise of Nations and a Big Huge Script (BHS) that is running while you are playing the game.

Full documentation can be found here: https://docs.google.com/document/d/1MfRT_oydPE7pFMdP7FUKANnfytTHogrJg0K5QlUn1Es/pub

## Modifications in this fork:
* A quick (and likely dirty?) fix for WASAPI / DirectSound not initializing

## Other known issues:
* If your Rise of Nations installation is somewhere other than the usual C:\Program Files (x86)\Steam\SteamApps\common\Rise of Nations\sounds\tracks\ or C:\Program Files\Steam\SteamApps\common\Rise of Nations\sounds\tracks\ then you'll get a "file not found" exception. I have not fixed this, but a workaround is to copy the tracks folder from your RoN IS installed into those file paths.