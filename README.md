# BleemSync
BleemSync is a relatively safe way to add games to your PlayStation Classic.

## Features
1. Overmounts portions of the PSC's filesystem to safely allow modifications
1. Modifies the stock UI to show added games
1. Supports multi-disc games

## Installation
1. Download the ZIP file from the [release page](https://github.com/pathartl/BleemSync/releases/latest)
1. Extract the contents to the root of your FAT32 or ext4 formatted USB flash drive
1. Name your flash drive `SONY`. This is a requirement.

## Game Configuration
On the root of your flash drive, create a `Games` folder. In this folder, create a folder for each game you would like to add to the system. The folders must be numbered sequentially. Each game folder must contain a `GameData` folder which contains a `Game.ini`, cover art image, `pcsx.cfg`, and the game's `bin` and `cue` files. A template of the `Games` folder can be found in the release ZIP.

A proper folder structure looks something like this:
```
Games/
    1/
        GameData/
            Game.ini
            pcsx.cfg
            SLUS-01066.bin
            SLUS-01066.cue
            SLUS-01066.png
    2/
        ...
    3/
        ...
```
It is recommended, though not necessary that the game's filenames use the appropriate disc ID.

The `pcsx.cfg` file can be copied without modification as it's the same config that is shipped with the system's games.

For each game, the `Game.ini` must be customized in order to be displayed in the menu correctly. The `Discs` value should be the name of the `.cue` file without the file extension. Sample:
```
[Game]
Discs=SLUS-01066
Title=Tony Hawk's Pro Skater 2
Publisher=Activision
Players=2
Year=2000
```

### Multi-disc games
For multi-disc games, add both `bin` and `cue` files for each disc into the game's numbered folder and make a config that looks like:
```
[Game]
Discs=SLUS-00665,SLUS-00667
Title=Command & Conquer - Red Alert
Publisher=Westwood Studios
Players=1
Year=1998
```

## Synchronization
Once all the games are configured, go into the `BleemSync` directory and run `BleemSync.exe`. This will generate a `System` folder containing the newly generated database and a script to safely mount the games. Insert the flash drive into your PlayStation Classic and then turn it on. Your newly added games should be on display.

## Frequently Asked Questions

### My controller isn't working
Make sure your `pcsx.cfg` is in the game directory

### I'm getting a UI error upon boot
Make sure that you unplug the power on your system before you remove or add your flash drive.

### Are my original games intact?
Yes. Remove the flash drive and the stock games should be available

### Can I add games alongside the pre-included ones?
Not currently.

### What size does the cover art have to be?
The cover art should be square (1:1 aspect ratio). If the cover art is not square, it will be squashed by the UI as square anyway so visually it's best to pre-crop it. The system uses 226x226px images for the pre-loaded games.

### My controller still isn't working
Your user data might have gotten messed up. Try going to Settings and then Initialize Console. **This will wipe out any saves and save states, proceed with caution!**

### When I boot, I get a 001-001 error!
Make sure you have run BleemSync on your PC. This usually means the game database which BleemSync builds cannot be found.

### The Sony logo appears, but then I am kicked to an error screen and that says "An error has occurred. Turn off the console, unplug the AC adapter, then turn on the console again.
On your USB drive, go to `System/Preferences` and remove `regional.pre`. If this doesn't work, start up your PlayStation Classic without your USB drive inserted, select Battle Arena Toshinden, and then unplug the machine. Plug it back in along with your USB drive and it should now boot into the menu.

## Known Issues/Limitations
* Games have to be in sequential order. This is a limit imposed by BleemSync. Further testing has to be done to see if this is a system limitation
* Games have to be in `bin`/`cue` format. This is a limit imposed by the system's UI
* Other regions may not be working at this time

## Credits
* [madmonkey](https://github.com/madmonkey1907) - created lolhack which makes all of this possible
* [DanTheMan827](https://github.com/DanTheMan827) - overmounting lessons
* [yifanlu](https://github.com/yifanlu) - why not

## Contributors
[CompCom](https://github.com/compcom), [Maku](https://github.com/justMaku)
