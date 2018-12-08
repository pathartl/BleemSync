# BleemSync
BleemSync is a relatively safe way to add games to your PlayStation Classic.

## Features
1. Overmounts portions of the PSC's filesystem to safely allow modifications
1. Modifies the stock UI to show added games
1. Supports multi-disc games

## Installation
1. Download the ZIP file from the [release page](https://github.com/pathartl/BleemSync/releases/tag/0.1.0)
1. Extract the contents to the root of your FAT32 or ext4 formatted USB flash drive

## Game Configuration
On the root of your flash drive, create a `Games` folder. In this folder, create a folder for each game you would like to add to the system. The folders must be numbered sequentially. Each game folder must contain a `Game.ini`, cover art image, `pcsx.cfg`, and the game's `bin` and `cue` files. A template of the `Game.ini` and `pcsx.cfg` can also be found in the release ZIP.

A proper folder structure looks something like this:
```
Games/
    1/
        Game.ini
        pcsx.cfg
        SLUS-01066.bin
        SLUS-01066.cue
        SLUS-01066.cfg
    2/
        ...
    3/
        ...
```
It is recommended, though not necessary that the game's filenames use the appropriate disc ID.

The `pcsx.cfg` file can be copied without modification as it's the same config that is shipped with the system's games.

For each game, the `Game.ini` must be customized in order to be displayed in the menu correctly. Sample:
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

## Syncronization
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
