#!/bin/sh
# BleemSync Payload 0.2.2
killall -s KILL sonyapp ui_menu

PCSX_DIR="/data/AppData/sony/pcsx"
TempUserData="/tmp/UserData"

# Extract system files to avoid crashing
mkdir -p /media/System
mkdir -p /media/System/Bios
mkdir -p /media/System/Preferences
mkdir -p /media/System/Preferences/System
mkdir -p /media/System/Preferences/User
mkdir -p /media/System/Preferences/AutoDimmer
mkdir -p /media/System/Databases
mkdir -p /media/System/Region
mkdir -p /media/System/Logs
mkdir -p /media/System/UI

# Copy the BIOS files to USB
if [ ! -f /media/System/Bios/romw.bin ]
then
		cp -r /gaadata/system/bios/* /media/System/Bios
fi

# Copy the regional.pre to USB
# This contains settings for the UI region
if [ ! -f /media/System/Preferences/System/regional.pre ]
then
		cp /gaadata/preferences/regional.pre /media/System/Preferences/regional.pre
fi

# Copy out the user.pre to USB
# This contains things like language setting
if [ ! -f /media/System/Preferences/User/user.pre ]
then
        cp /data/AppData/sony/ui/user.pre /media/System/Preferences/User/user.pre
fi

# Copy out the auto dimming config to USB
if [ ! -f /media/System/Preferences/AutoDimmer/config.cnf ]
then
        cp /data/AppData/sony/auto_dimmer/config.cnf
fi

# Copy out the region info
if [ ! -f /media/System/Region/GENINFO ]
then
		cp /gaadata/geninfo/* /media/System/Region
fi

# Init the ui_menu.log
if [ ! -f /media/System/Logs/ui_menu.log ]
then
        touch /media/System/Logs/ui_menu.log
fi

# Unmount current games partition, overmount it
umount /gaadata
mount -t tmpfs tmpfs /gaadata

mkdir -p /gaadata/system/bios
mkdir -p /gaadata/preferences
mkdir -p /gaadata/geninfo
mkdir -p /gaadata/databases

# Overmount some system folders
mount -o bind /media/System/Bios /gaadata/system/bios
mount -o bind /media/System/Preferences/System /gaadata/preferences
mount -o bind /media/System/Databases /gaadata/databases
mount -o bind /media/System/Region /gaadata/geninfo



# Now mount a tmpfs for the user data
umount /data
mount -t tmpfs tmpfs /data

sync

# Recreate some linked functionality
mkdir -p /data/sony/sgmo
mkdir -p /data/AppData/sony/pcsx/.pcsx
mkdir -p /data/sony/ui
mkdir -p /data/AppData/sony/auto_dimmer

touch /data/sony/ui/error.log

ln -s /dev/shm/power /data/power
ln -s /usr/sony/bin/plugins /data/AppData/sony/pcsx/plugins
ln -s /media/System/Bios /data/AppData/sony/pcsx/bios
ln -s /media/System/Preferences/User/user.pre /data/AppData/sony/ui/user.pre
ln -s /tmp/diag /data/sony/sgmo/diag

mount -o bind /media/System/Preferences/User /data/AppData/sony/ui
mount -o bind /media/System/Preferences/AutoDimmer /data/AppData/sony/auto_dimmer

killall ui_menu

cd /media/Games
# Do UserData first
# The pcsx.cfg file needs to be copied into the user data folder or controllers may not work.
find * -maxdepth 0 -type d -exec mkdir -p /data/AppData/sony/pcsx/{}/.pcsx \;
#find * -maxdepth 0 -type d -exec mount -o bind /media/Games/{}/UserData/.pcsx /data/AppData/sony/pcsx/{}/.pcsx \;
find * -maxdepth 0 -type d -exec cp /media/Games/{}/GameData/pcsx.cfg /data/AppData/sony/pcsx/{}/.pcsx \;

find * -maxdepth 0 -type d -exec mkdir -p /gaadata/{} \;
find * -maxdepth 0 -type d -exec mount -o bind /media/Games/{}/GameData /gaadata/{} \;

cd -

find /data > /media/data.log

##
## Grabbed from sonyapp
#####    mkdir -p $PCSX_DIR/.pcsx
#####    
#####    BIOS_SRC=/gaadata/system/bios
#####    if [ ! -e $BIOS_SRC ]; then
#####        BIOS_SRC=/usr/sony/bin/bios
#####    fi
#####    
#####    if [ ! -e $PCSX_DIR/bios/SCPH1001.BIN ]; then
#####        echo "bios file 0 not exist"
#####        cp $BIOS_SRC/SCPH1001.BIN $PCSX_DIR/bios/
#####    elif [ "$(stat -c%s $PCSX_DIR/bios/SCPH1001.BIN)" -eq "0" ]; then
#####        echo "bios file 0 is zero length"
#####        cp $BIOS_SRC/SCPH1001.BIN $PCSX_DIR/bios/
#####    fi
#####    
#####    if [ ! -e $PCSX_DIR/bios/romJP.bin ]; then
#####        echo "bios file 1 not exist"
#####        cp $BIOS_SRC/romJP.bin $PCSX_DIR/bios/
#####    elif [ "$(stat -c%s $PCSX_DIR/bios/romJP.bin)" -eq "0" ]; then
#####        echo "bios file 1 is zero length"
#####        cp $BIOS_SRC/romJP.bin $PCSX_DIR/bios/
#####    fi
#####    
#####    if [ ! -e $PCSX_DIR/bios/romw.bin ]; then
#####        echo "bios file 2 not exist"
#####        cp $BIOS_SRC/romw.bin $PCSX_DIR/bios/
#####    elif [ "$(stat -c%s $PCSX_DIR/bios/romw.bin)" -eq "0" ]; then
#####        echo "bios file 2 is zero length"
#####        cp $BIOS_SRC/romw.bin $PCSX_DIR/bios/
#####    fi
#####    
#####    PLUGINS_SRC=/usr/sony/bin/plugins
#####    if [ ! -e $PCSX_DIR/plugins ]; then
#####        echo "plugins directory not exist"
#####        mkdir -p $PCSX_DIR/plugins
#####    fi
#####    
#####    for f in `ls $PLUGINS_SRC`; do
#####        if [ ! -e $PCSX_DIR/plugins/$f ]; then
#####    	echo "$f file not exist"
#####    	cp $PLUGINS_SRC/$f $PCSX_DIR/plugins/
#####        elif [ "$(stat -c%s $PCSX_DIR/plugins/$f)" -eq "0" ]; then
#####            echo "$f file is zero length"
#####            cp $PLUGINS_SRC/$f $PCSX_DIR/plugins/
#####        fi
#####    done
##
##

sed -i "s/iUiUserSettingLastSelectGameCursorPos.*/iUiUserSettingLastSelectGameCursorPos=0/" /data/AppData/sony/ui/user.pre

cd /data/AppData/sony/pcsx
/usr/sony/bin/ui_menu --power-off-enable > /media/System/Logs/ui_menu.log

sync

red_led "12" "0.3"