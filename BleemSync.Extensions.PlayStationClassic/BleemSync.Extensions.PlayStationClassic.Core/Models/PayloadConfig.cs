using BleemSync.Extensions.PlayStationClassic.Core.Attributes;
using SharpConfig;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BleemSync.Extensions.PlayStationClassic.Core.Models
{
    public class PayloadConfig : IniConfig
    {
        public PayloadConfig() { }
        public PayloadConfig(Configuration configuration) : base(configuration) { }

        [IniProperty(Name = "runtime_log")]
        [IniSection(Name = "exe_booleans")]
        [DefaultValue(true)]
        [Display(Name = "Use runtime logs")]
        public bool RuntimeLog { get; set; }

        [IniProperty(Name = "refresh_logs")]
        [IniSection(Name = "exe_booleans")]
        [DefaultValue(true)]
        [Display(Name = "If TRUE then clear down logs on each bootup")]
        public bool RefreshLogs { get; set; }

        [IniProperty(Name = "force_redump")]
        [IniSection(Name = "exe_booleans")]
        [DefaultValue(false)]
        [Display(Name = "If TRUE dumps will redump on boot regardless of flag")]
        public bool ForceRedump { get; set; }

        [IniProperty(Name = "link_EMMC_and_USB")]
        [IniSection(Name = "exe_booleans")]
        [DefaultValue(true)]
        [Display(Name = "If TRUE, dynamically links EMMC games ontop of USB games")]
        public bool LinkInternalStorage { get; set; }

        [IniProperty(Name = "link_alphabeticalise")]
        [IniSection(Name = "exe_booleans")]
        [DefaultValue(true)]
        [Display(Name = "If TRUE then auto alphabeticalise EMMC+USB games")]
        public bool LinkAlphabetize { get; set; }

        [IniProperty(Name = "launch_ra_from_stock_UI")]
        [IniSection(Name = "exe_booleans")]
        [DefaultValue(false)]
        [Display(Name = "If TRUE and enable_intercept also TRUE then RetroArch is the emulator for Stock UI")]
        public bool LaunchRetroArchFromStockUi { get; set; }

        [IniProperty(Name = "enable_intercept")]
        [IniSection(Name = "exe_booleans")]
        [DefaultValue(false)]
        [Display(Name = "If TRUE Stock UI will launch intercept script instead of pcsx directly")]
        public bool EnableIntercept { get; set; }

        [IniProperty(Name = "boot_splash")]
        [IniSection(Name = "exe_boot_booleans")]
        [DefaultValue(true)]
        [Display(Name = "Toggle BleemSync splashscreen ( & Custom splashscreen )")]
        public bool BootSplash { get; set; }

        [IniProperty(Name = "boot_quick")]
        [IniSection(Name = "exe_boot_booleans")]
        [DefaultValue(false)]
        [Display(Name = "Enables quick boot (disables initial sony anim splash)")]
        public bool BootQuick { get; set; }

        [IniProperty(Name = "boot_disable_health")]
        [IniSection(Name = "exe_boot_booleans")]
        [DefaultValue(false)]
        [Display(Name = "Disables health warning (left on by default for H+S)")]
        public bool BootDisableHealthWarning { get; set; }

        [IniProperty(Name = "boot_target_stock_UI")]
        [IniSection(Name = "exe_boot_booleans")]
        [DefaultValue(false)]
        [Display(Name = "Boots directly to the Stock UI")]
        public bool BootTargetStockUi { get; set; }

        [IniProperty(Name = "boot_target_stock_RA")]
        [IniSection(Name = "exe_boot_booleans")]
        [DefaultValue(false)]
        [Display(Name = "Boots directly to RetroArch")]
        public bool BootTargetStockRetroArch { get; set; }

        [IniProperty(Name = "boot_target_stock_BM")]
        [IniSection(Name = "exe_boot_booleans")]
        [DefaultValue(true)]
        [Display(Name = "Boots directly to the bootmenu selector")]
        public bool BootTargetStockBootMenu { get; set; }

        [IniProperty(Name = "mountpoint")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("/media")]
        [Display(Name = "Mount location of BleemSync")]
        public string Mountpoint { get; set; }

        [IniProperty(Name = "bleemsync_path")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("$mountpoint/bleemsync")]
        [Display(Name = "Location of BleemSync")]
        public string BleemSyncPath { get; set; }

        [IniProperty(Name = "retroarch_path")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("$bleemsync_path/opt/retroarch")]
        [Display(Name = "Location of RetroArch")]
        public string RetroArchPath { get; set; }

        [IniProperty(Name = "images_path")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("$bleemsync_path/etc/bleemsync/IMG")]
        [Display(Name = "Location of image files")]
        public string ImagesPath { get; set; }

        [IniProperty(Name = "themes_path")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("$bleemsync_path/etc/bleemsync/THEME")]
        [Display(Name = "Location of UI themes")]
        public string ThemesPath { get; set; }

        [IniProperty(Name = "sounds_path")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("$bleemsync_path/etc/bleemsync/SND")]
        [Display(Name = "Location of sound files")]
        public string SoundsPath { get; set; }

        [IniProperty(Name = "runtime_log_path")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("$mountpoint/logs")]
        [Display(Name = "Location of logs")]
        public string RuntimeLogPath { get; set; }

        [IniProperty(Name = "runtime_exe_path")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("$bleemsync_path/bin")]
        [Display(Name = "Location of exe child")]
        public string RuntimeExePath { get; set; }

        [IniProperty(Name = "runtime_flag_path")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("$bleemsync_path/flags")]
        [Display(Name = "Location of execution flags")]
        public string RuntimeFlagPath { get; set; }

        [IniProperty(Name = "dump_path")]
        [IniSection(Name = "exe_paths")]
        [DefaultValue("$mountpoint/dump")]
        [Display(Name = "Location for dumps")]
        public string DumpPath { get; set; }

        [IniProperty(Name = "selected_theme")]
        [IniSection(Name = "func_themes")]
        [DefaultValue("modmyclassic")]
        [Display(Name = "Theme name (STOCK uses standard theme)")]
        public string SelectedTheme { get; set; }

        [IniProperty(Name = "override_theme_music")]
        [IniSection(Name = "func_themes")]
        [DefaultValue(false)]
        [Display(Name = "Override the current themes music folder")]
        public bool OverrideThemeMusic { get; set; }

        [IniProperty(Name = "random_theme_onload")]
        [IniSection(Name = "func_themes")]
        [DefaultValue(false)]
        [Display(Name = "Select a random theme from the THEMES folder")]
        public bool RandomThemeOnLoad { get; set; }

        [IniProperty(Name = "boot_menu_music")]
        [IniSection(Name = "func_boot")]
        [DefaultValue(true)]
        [Display(Name = "If TRUE then play boot menu music WAV loop")]
        public bool BootMenuMusic { get; set; }
    }
}
