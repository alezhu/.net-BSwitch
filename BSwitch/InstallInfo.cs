using System;
using Microsoft.Win32;

namespace BSwitch
{
    public class InstallInfo
    {
        private const string HideIconsCommandKeyName = "HideIconsCommand";
        public string HideIconsCommand = "";

        private const string ReinstallCommandKeyName = "ReinstallCommand";
        public string ReinstallCommand = "";

        private const string ShowIconsCommandKeyName = "ShowIconsCommand";
        public string ShowIconsCommand = "";

        private const string IconsVisibleKeyName = "IconsVisible";
        public bool IconsVisible = true;

        private const string DefaultKeyName = "";
        public string Default = "";



        public static InstallInfo Load(RegistryKey key)
        {
            InstallInfo res = new InstallInfo();
            if (key != null)
            {
                res.Default = (string)key.GetValue(DefaultKeyName);
                res.HideIconsCommand = (string)key.GetValue(HideIconsCommandKeyName);
                res.ReinstallCommand = (string)key.GetValue(ReinstallCommandKeyName);
                res.ShowIconsCommand = (string)key.GetValue(ShowIconsCommandKeyName);
                Int32 dw = (Int32)key.GetValue(IconsVisibleKeyName, (Int32)0);
                res.IconsVisible = dw != 0;
            }
            return res;
        }

        public void Save(RegistryKey key)
        {
            Utilities.RegSetValue(key, DefaultKeyName, this.Default);
            Utilities.RegSetValue(key, HideIconsCommandKeyName, this.HideIconsCommand, RegistryValueKind.ExpandString);
            Utilities.RegSetValue(key, ReinstallCommandKeyName, this.ReinstallCommand, RegistryValueKind.ExpandString);
            Utilities.RegSetValue(key, ShowIconsCommandKeyName, this.ShowIconsCommand, RegistryValueKind.ExpandString);
            UInt32 dw = (UInt32)((this.IconsVisible) ? 1 : 0);
            Utilities.RegSetValue(key, IconsVisibleKeyName, dw, RegistryValueKind.DWord);
        }
    }
}
