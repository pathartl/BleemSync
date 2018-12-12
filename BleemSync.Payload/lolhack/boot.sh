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
mount -t tmpfs tmpfs "/gaadata" || MOUNT_FAIL=1 
mount -t tmpfs tmpfs "/data" || MOUNT_FAIL=1 
[ $MOUNT_FAIL -eq 1 ] && reboot && exit

# Create gaadata tmpfs
mkdir -p /gaadata/system/
ln -s /media/System/Databases /gaadata/databases
ln -s /media/System/Region /gaadata/geninfo
ln -s /media/System/Bios /gaadata/system/bios
ln -s /media/System/Preferences/System /gaadata/preferences
ls /media/Games | grep '^[0-9]\+$' | xargs -I % sh -c "ln -s /media/Games/%/GameData /gaadata/% && mkdir -p /media/Games/%/.pcsx && cp /media/Games/%/GameData/pcsx.cfg /media/Games/%/.pcsx"

# Create data tmpfs
mkdir -p /data/sony/sgmo /data/AppData/sony
ln -s /tmp/diag /data/sony/sgmo/diag
ln -s /dev/shm/power /data/power
ln -s /media/System/UI /data/sony/ui
ln -s /media/System/Preferences/User /data/AppData/sony/ui
ln -s /media/System/Preferences/AutoDimmer /data/AppData/sony/auto_dimmer
cp -r /usr/sony/share/recovery/AppData/sony/pcsx /data/AppData/sony/pcsx
ls /media/Games | grep '^[0-9]\+$' | xargs -I % sh -c "rm -rf /data/AppData/sony/pcsx/% && ln -s /media/Games/% /data/AppData/sony/pcsx/%"
ln -s /media/System/Bios /data/AppData/sony/pcsx/bios
ln -s /usr/sony/bin/plugins /data/AppData/sony/pcsx/plugins

# Fix for last selected game issue. If not in place user may experience UI issue
sed -i "s/iUiUserSettingLastSelectGameCursorPos.*/iUiUserSettingLastSelectGameCursorPos=0/" /data/AppData/sony/ui/user.pre

# Fix for line endings. BAD WINDOWS
find /media -name *.cfg -exec sed -i 's/\r//g' {} \;
find /media -name *.pre -exec sed -i 's/\r//g' {} \;

cd /data/AppData/sony/pcsx
export PCSX_ESC_KEY=2
/usr/sony/bin/ui_menu --power-off-enable &> /media/System/Logs/ui_menu.log
sync
sync
reboot

