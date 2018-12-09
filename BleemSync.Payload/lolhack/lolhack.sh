#!/bin/sh
NumberOfGamesToT

# Extract system files to avoid crashing
mkdir -p /media/System
mkdir -p /media/System/Bios
mkdir -p /media/System/Preferences
mkdir -p /media/System/Databases
mkdir -p /media/System/Region

killall ui_menu

if [ ! -d /media/UserData ]
then
		killall sonylogo
		mkdir -p /media/UserData
		cp /data/AppData/* /media/UserData
fi

if [ ! -f /media/System/Bios/romw.bin ]
then
		cp -r /gaadata/system/bios/* /media/System/Bios
fi

if [ ! -f /media/System/Preferences/regional.pre ]
then
		cp /gaadata/preferences/* /media/System/Preferences
fi

if [ ! -f /media/System/Region/GENINFO ]
then
		cp /gaadata/geninfo/* /media/System/Region
fi

killall ui_menu

# Safely overmount the games and user data folder
source /media/System/MountGames.sh
# find /media/Games -maxdepth 1 -type d -printf "%f\n" > /media/System/payload.log
# for game in `find /media/Games/* -maxdepth 0 -type d -printf "%f\n"`
# do
# 	if [ ! -z "$game" ]
# 	then
# 		echo Mounting $game >> /media/System/payload.log
# 		mount -o bind /media/Games/$game /gaadata/$game
# 	fi
# done

# mount -o bind /media/System/Bios /gaadata/system/bios
# mount -o bind /media/System/Preferences /gaadata/preferences
mount -o bind /media/System/Databases /gaadata/databases
# mount -o bind /media/System/Region /gaadata/geninfo
# mount -o bind /media/UserData /data/AppData

killall ui_menu

sync

sed -i "s/iUiUserSettingLastSelectGameCursorPos.*/iUiUserSettingLastSelectGameCursorPos=1/" /media/data/AppData/sony/ui/user.pre

sync

find / > /media/filelist.log

/usr/sony/bin/ui_menu

red_led "12" "0.3"

function MountGame() {
	echo Mounting $1 >> /media/System/payload.log
	mount -o bind /media/Games/$1 /gaadata/$1
}