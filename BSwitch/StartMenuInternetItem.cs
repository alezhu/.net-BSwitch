using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections;


namespace BSwitch
{
    public class StartMenuInternetItem
    {

        public string Name = "";

        private const string DescriptionKeyName = "";
        public string Description = "";

        private const string LocalizedStringKeyName = "LocalizedString";
        public string LocalizedString = null;

        private const string DefaultIconKeyName = "DefaultIcon";
        public string DefaultIcon = "";

        private const string InstallInfoKeyName = "InstallInfo";
        public InstallInfo InstallInfo = new InstallInfo();

        private const string CapabilitiesKeyName = "Capabilities";
        public Capabilities Capabilities = new Capabilities();

        private const string ShellKeyName = "shell";

        public class ShellCommand
        {
            public string Description = "";
            public string Command = "";
        }

        public Dictionary<string, ShellCommand> ShellCommands = new Dictionary<string, ShellCommand>();



        public static StartMenuInternetItem Load(RegistryKey key)
        {
            StartMenuInternetItem res = new StartMenuInternetItem();
            res.Name = key.Name.Substring(key.Name.LastIndexOf('\\') + 1);
            res.Description = (string)key.GetValue(DescriptionKeyName);
            res.LocalizedString = (string)key.GetValue(LocalizedStringKeyName);

            RegistryKey subkey = key.OpenSubKey(InstallInfoKeyName, false);
            res.InstallInfo = InstallInfo.Load(subkey);

            subkey = key.OpenSubKey(DefaultIconKeyName, false);
            res.DefaultIcon = (string)subkey.GetValue(String.Empty);


            subkey = key.OpenSubKey(CapabilitiesKeyName, false);
            res.Capabilities = Capabilities.Load(subkey);

            subkey = key.OpenSubKey(ShellKeyName, false);
            foreach (string nick in subkey.GetSubKeyNames())
            {
                RegistryKey NickKey = subkey.OpenSubKey(nick, false);
                RegistryKey CommandKey = NickKey.OpenSubKey("command", false);
                ShellCommand command = new ShellCommand();
                try
                {
                    command.Description = (string)NickKey.GetValue(String.Empty);
                    command.Command = (string)CommandKey.GetValue(String.Empty);
                    res.ShellCommands[nick] = command;
                }
                catch (NullReferenceException) { }
                finally
                {
                    // do nothing
                }
            }

            return res;
        }

        public void Save(RegistryKey ParentKey)
        {
            //ParentKey.DeleteSubKeyTree(Name);
            RegistryKey key = ParentKey.CreateSubKey(this.Name, RegistryKeyPermissionCheck.ReadWriteSubTree);
            Utilities.RegSetValue(key, DescriptionKeyName, this.Description);
            if (this.LocalizedString != null)
            {
                Utilities.RegSetValue(key, LocalizedStringKeyName, this.LocalizedString, RegistryValueKind.ExpandString);
            }


            RegistryKey subkey = key.CreateSubKey(InstallInfoKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            this.InstallInfo.Save(subkey);

            subkey = key.CreateSubKey(DefaultIconKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            Utilities.RegSetValue(subkey, String.Empty, this.DefaultIcon, RegistryValueKind.ExpandString);



            subkey = key.CreateSubKey(CapabilitiesKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            this.Capabilities.Save(subkey);

            subkey = key.CreateSubKey(ShellKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            foreach (KeyValuePair<string, ShellCommand> item in ShellCommands)
            {
                RegistryKey NickKey = subkey.CreateSubKey(item.Key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                Utilities.RegSetValue(NickKey, "", item.Value.Description);
                RegistryKey CommandKey = NickKey.CreateSubKey("command", RegistryKeyPermissionCheck.ReadWriteSubTree);
                Utilities.RegSetValue(CommandKey, "", item.Value.Command);
            }


        }
    }

    public class StartMenuInternetItems : ArrayList
    {
        public new StartMenuInternetItem this[int index]
        {
            get { return (StartMenuInternetItem)base[index]; }
            set { base[index] = value; }
        }

        private const string regKey = @"SOFTWARE\Clients\StartMenuInternet";

        public static StartMenuInternetItems LoadFromRegistry()
        {
            StartMenuInternetItems res = new StartMenuInternetItems();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(regKey);
            if (key != null)
            {
                foreach (string item in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(item);
                    res.Add(StartMenuInternetItem.Load(subkey));
                }
            }
            return res;
        }

        public void SaveToRegistry()
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey(regKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
            foreach (StartMenuInternetItem item in this)
            {
                item.Save(key);
            }

        }
    }
}