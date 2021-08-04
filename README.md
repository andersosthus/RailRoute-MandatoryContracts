# Supported version
Tested with Rail Route 1.0.18

# Description
Adds another type of contracts to the game, Mandatory contracts.
These are contracts that you cannot reject, and have the following properties:
- They will only include stations you have bought (so make sure you are ready to connect a new station when you buy it)
- They have the potential of giving a bit higher reward that regular contracts
- They will be auto-accepted 1 hour after showing up in your contract log, so you can't just leave them there
- They will continue to be generated even though you are at your regular max contracts (5/5)
- Each mandatory contract has a random number of mandatory times it needs to be completed. When this number is reached, the contract will go away

# Known issues
Saving is not implemented for the mod yet, so if you save and re-load your game, all existing mandatory contracts will be regular contracts.

# Future plans
- Implement saving so that mandatory contracts are kept between loads
- Add randomization to the created trains with regards to train length and max speed

# Installation

Copy all the files to:

Windows:
%UserProfile%\AppData\LocalLow\bitrich\Rail Route\mods\MandatoryContracts

Simply put %UserProfile%\AppData\LocalLow\bitrich\Rail Route\ into your explorer location and hit enter
You may need to create the folder mods

Linux (untested):
~/.config/unity3d/bitrich/Rail Route/mods/MandatoryContracts

Mac (untested):
~/Library/Application Support/Rail Route/mods/MandatoryContracts