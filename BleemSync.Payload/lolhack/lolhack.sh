#!/bin/sh
# BleemSync Payload 0.2.2
killall -s KILL sonyapp ui_menu

PCSX_DIR="/data/AppData/sony/pcsx"

# Extract system files to avoid crashing
mkdir -p /media/System
mkdir -p /media/System/Bios
mkdir -p /media/System/Preferences
mkdir -p /media/System/Preferences/System
mkdir -p /media/System/Preferences/User
mkdir -p /media/System/Databases
mkdir -p /media/System/Region
mkdir -p /media/System/Logs
mkdir -p /media/System/UI

if [ ! -f /media/System/Bios/romw.bin ]
then
		cp -r /gaadata/system/bios/* /media/System/Bios
fi

if [ ! -f /media/System/Preferences/regional.pre ]
then
		cp /gaadata/preferences/regional.pre /media/System/Preferences/regional.pre
fi

if [ ! -f /media/System/Region/GENINFO ]
then
		cp /gaadata/geninfo/* /media/System/Region
fi

if [ ! -f /media/System/Logs/ui_menu.log ]
then
        touch /media/System/Logs/ui_menu.log
fi

# Overmount some folders
mount -o bind /media/System/Bios /gaadata/system/bios
mount -o bind /media/System/Preferences/System /gaadata/preferences
mount -o bind /media/System/Preferences/User /data/AppData/sony/ui
mount -o bind /media/System/Databases /gaadata/databases
mount -o bind /media/System/Region /gaadata/geninfo
mount -o bind /media/System/Logs/UI /data/sony/ui

killall ui_menu

# The pcsx.cfg file needs to be copied into the user data folder or controllers may not work.
cd /media/Games
find * -maxdepth 0 -type d -exec mount -o bind /media/Games/{}/GameData /gaadata/{} \;

find * -maxdepth 0 -type d -exec mkdir -p /media/Games/{}/UserData \;
find * -maxdepth 0 -type d -exec mount -o bind /media/Games/{}/UserData /data/AppData/sony/pcsx/{} \;

find * -maxdepth 0 -type d -exec mkdir -p /data/AppData/sony/pcsx/{}/.pcsx \;
find * -maxdepth 0 -type d -exec cp /media/Games/{}/GameData/pcsx.cfg /data/AppData/sony/pcsx/{}/.pcsx/pcsx.cfg \;

cd -

##
## Grabbed from sonyapp
mkdir -p $PCSX_DIR/.pcsx

BIOS_SRC=/gaadata/system/bios
if [ ! -e $BIOS_SRC ]; then
    BIOS_SRC=/usr/sony/bin/bios
fi

if [ ! -e $PCSX_DIR/bios/SCPH1001.BIN ]; then
    echo "bios file 0 not exist"
    cp $BIOS_SRC/SCPH1001.BIN $PCSX_DIR/bios/
elif [ "$(stat -c%s $PCSX_DIR/bios/SCPH1001.BIN)" -eq "0" ]; then
    echo "bios file 0 is zero length"
    cp $BIOS_SRC/SCPH1001.BIN $PCSX_DIR/bios/
fi

if [ ! -e $PCSX_DIR/bios/romJP.bin ]; then
    echo "bios file 1 not exist"
    cp $BIOS_SRC/romJP.bin $PCSX_DIR/bios/
elif [ "$(stat -c%s $PCSX_DIR/bios/romJP.bin)" -eq "0" ]; then
    echo "bios file 1 is zero length"
    cp $BIOS_SRC/romJP.bin $PCSX_DIR/bios/
fi

if [ ! -e $PCSX_DIR/bios/romw.bin ]; then
    echo "bios file 2 not exist"
    cp $BIOS_SRC/romw.bin $PCSX_DIR/bios/
elif [ "$(stat -c%s $PCSX_DIR/bios/romw.bin)" -eq "0" ]; then
    echo "bios file 2 is zero length"
    cp $BIOS_SRC/romw.bin $PCSX_DIR/bios/
fi

PLUGINS_SRC=/usr/sony/bin/plugins
if [ ! -e $PCSX_DIR/plugins ]; then
    echo "plugins directory not exist"
    mkdir -p $PCSX_DIR/plugins
fi

for f in `ls $PLUGINS_SRC`; do
    if [ ! -e $PCSX_DIR/plugins/$f ]; then
	echo "$f file not exist"
	cp $PLUGINS_SRC/$f $PCSX_DIR/plugins/
    elif [ "$(stat -c%s $PCSX_DIR/plugins/$f)" -eq "0" ]; then
        echo "$f file is zero length"
        cp $PLUGINS_SRC/$f $PCSX_DIR/plugins/
    fi
done
##
##

sed -i "s/iUiUserSettingLastSelectGameCursorPos.*/iUiUserSettingLastSelectGameCursorPos=0/" /data/AppData/sony/ui/user.pre

find / > /media/filelist.log

cd /data/AppData/sony/pcsx
/usr/sony/bin/ui_menu --power-off-enable > /media/System/ui_menu.log

sync

red_led "12" "0.3"