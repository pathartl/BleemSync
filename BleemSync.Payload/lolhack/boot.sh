#!/bin/sh

killall -s KILL sonyapp showLogo ui_menu

# Extract system files to avoid crashing
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
[ ! -f /media/System/Bios/romw.bin ] && cp -r /gaadata/system/bios/* /media/System/Bios
# Copy the regional.pre to USB
# This contains settings for the UI region
[ ! -f /media/System/Preferences/System/regional.pre ] && cp /gaadata/preferences/* /media/System/Preferences/System
# Copy out the user.pre to USB
# This contains things like language setting
[ ! -f /media/System/Preferences/User/user.pre ] && cp /data/AppData/sony/ui/* /media/System/Preferences/User
# Copy out the auto dimming config to USB
[ ! -f /media/System/Preferences/AutoDimmer/config.cnf ] && cp /data/AppData/sony/auto_dimmer/* /media/System/Preferences/AutoDimmer
# Copy out the region info
[ ! -f /media/System/Region/REGION ] && cp /gaadata/geninfo/* /media/System/Region
# Copy ui error log
[ ! -f /media/System/UI/error.log ] && cp /data/sony/ui/* /media/System/UI
# Init the ui_menu.log
[ ! -f /media/System/Logs/ui_menu.log ] && touch /media/System/Logs/ui_menu.log
sync

# Unmount partitons and create tmpfs - Shut system down on failure
MOUNT_FAIL=0
umount /data || MOUNT_FAIL=1 
umount /gaadata || MOUNT_FAIL=1 
# Create gaadata and data folders in tmp then mount over original folders
mkdir -p /tmp/gaadatatmp /tmp/datatmp
mount -o bind /tmp/gaadatatmp /gaadata || MOUNT_FAIL=1 
mount -o bind /tmp/datatmp /data || MOUNT_FAIL=1 
[ $MOUNT_FAIL -eq 1 ] && reboot && exit

# Create gaadata on tmpfs
mkdir -p /tmp/gaadatatmp/system/
ln -s /media/System/Databases /tmp/gaadatatmp/databases
ln -s /media/System/Region /tmp/gaadatatmp/geninfo
ln -s /media/System/Bios /tmp/gaadatatmp/system/bios
ln -s /media/System/Preferences/System /tmp/gaadatatmp/preferences
ls /media/Games | grep '^[0-9]\+$' | xargs -I % sh -c "ln -s /media/Games/%/GameData /tmp/gaadatatmp/% && mkdir -p /media/Games/%/.pcsx && cp /media/Games/%/GameData/pcsx.cfg /media/Games/%/.pcsx"

# Create data on tmpfs
mkdir -p /tmp/datatmp/sony/sgmo /tmp/datatmp/AppData/sony
ln -s /tmp/diag /tmp/datatmp/sony/sgmo/diag
ln -s /dev/shm/power /tmp/datatmp/power
ln -s /media/System/UI /tmp/datatmp/sony/ui
ln -s /media/System/Preferences/User /tmp/datatmp/AppData/sony/ui
ln -s /media/System/Preferences/AutoDimmer /tmp/datatmp/AppData/sony/auto_dimmer
cp -r /usr/sony/share/recovery/AppData/sony/pcsx /tmp/datatmp/AppData/sony/pcsx
ls /media/Games | grep '^[0-9]\+$' | xargs -I % sh -c "rm -rf /tmp/datatmp/AppData/sony/pcsx/% && ln -s /media/Games/% /tmp/datatmp/AppData/sony/pcsx/%"
ln -s /media/System/Bios /tmp/datatmp/AppData/sony/pcsx/bios
ln -s /usr/sony/bin/plugins /tmp/datatmp/AppData/sony/pcsx/plugins

# Fix for last selected game issue. If not in place user may experience UI issue
sed -i "s/iUiUserSettingLastSelectGameCursorPos.*/iUiUserSettingLastSelectGameCursorPos=0/" /tmp/datatmp/AppData/sony/ui/user.pre

# Fix for line endings. BAD WINDOWS
find /media -name *.cfg -exec sed -i 's/\r//g' {} \;
find /media -name *.pre -exec sed -i 's/\r//g' {} \;

cd /data/AppData/sony/pcsx
export PCSX_ESC_KEY=2
/usr/sony/bin/ui_menu --power-off-enable &> /media/System/Logs/ui_menu.log
sync
sync
reboot

