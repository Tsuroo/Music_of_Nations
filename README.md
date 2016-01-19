# Rise of Music
Rise of Music is an application which plays the background music for Rise of Nations (RoN), instead of using the current buggy audio system in game.  It does so with the use of an executable you run before a round of Rise of Nations and a Big Huge Script (BHS) that is running while you are playing the game.

Full documentation can be found here: https://docs.google.com/document/d/1MfRT_oydPE7pFMdP7FUKANnfytTHogrJg0K5QlUn1Es/pub

## Modifications in this fork:
* A quick (and likely dirty?) fix for WASAPI / DirectSound not initializing

## Using the fix in this fork:
* IF YOU DO NOT EXPERIENCE THIS -SPECIFIC- ISSUE, THIS FORK IS UNLIKELY TO SOLVE YOUR ISSUE.
* If you know how to, you can just download and build it, then take the newly-built exe and replace the old one.
* If you don't know how to do that, then poke me for a download link with the modified .exe

## Other known issues:
If your Rise of Nations installation is somewhere other than these two locations:
* C:\Program Files (x86)\Steam\SteamApps\common\Rise of Nations\sounds\tracks\
* C:\Program Files\Steam\SteamApps\common\Rise of Nations\sounds\tracks\


then you'll get a "file not found" exception. I have not fixed this, but a workaround is to copy the tracks folder from where your RoN is installed and copy that folder into those file paths.
