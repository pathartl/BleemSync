#!/bin/sh
NumberOfGamesToT

# Extract system files to avoid crashing
mkdir -p /media/System
mkdir -p /media/System/Bios
mkdir -p /media/System/Preferences
mkdir -p /media/System/Databases
mkdir -p /media/System/Region
mkdir -p /media/System/UI

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

# Allow access to the PCSX menu via Select + Triangle
export PCSX_ESC_KEY=2

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

# Overmount some files
#mount -o bind /media/System/sonyapp-copy /usr/sony/bin/sonyapp-copy
#mount -o bind /media/System/sonyapp-copylink /usr/sony/bin/sonyapp-copylink
mount -o bind /media/System/bin /usr/sony/bin

# Overmount some folders
mount -o bind /media/System/Bios /gaadata/system/bios
mount -o bind /media/System/Preferences /gaadata/preferences
mount -o bind /media/System/Databases /gaadata/databases
mount -o bind /media/System/Region /gaadata/geninfo
mount -o bind /media/System/UI /data/sony/ui
#mount -o bind /media/UserData /data/AppData

# The pcsx.cfg file needs to be copied into the user data folder or controllers may not work.
cd /media/Games
find * -type d -maxdepth 1 -exec mkdir -p /media/UserData/{} \;
find * -type d -maxdepth 1 -exec mount -o bind /media/UserData/{} /data/AppData/sony/pcsx/{} \;
find * -type d -maxdepth 1 -exec mkdir -p /data/AppData/sony/pcsx/{}/.pcsx \;
find * -type d -maxdepth 1 -exec cp /media/Games/{}/pcsx.cfg /data/AppData/sony/pcsx/{}/.pcsx/pcsx.cfg \;
cd -

mkdir -p /data/AppData/sony/pcsx/bios
cp /media/System/Bios/* /data/AppData/sony/pcsx/bios

mkdir -p /data/AppData/sony/pcsx/plugins
cp /usr/sony/bin/plugins/* /data/AppData/sony/pcsx/plugins

/usr/sony/bin/sonyapp-copy

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