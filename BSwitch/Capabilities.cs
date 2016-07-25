using System;
using Microsoft.Win32;
using System.Collections.Generic;

namespace BSwitch
{
    public class Capabilities
    {


        private const string DefaultKeyName = "";
        public string Default = "";

        private const string ApplicationDescriptionKeyName = "ApplicationDescription";
        public string ApplicationDescription = "";

        private const string ApplicationIconKeyName = "ApplicationIcon";
        public string ApplicationIcon = "";

        private const string ApplicationNameKeyName = "ApplicationName";
        public string ApplicationName = "";

        private const string FileAssociationKeyName = "FileAssociations";
        public Dictionary<string, string> FileAssociation = new Dictionary<string, string>();

        private const string URLAssociationsKeyName = "URLAssociations";
        public Dictionary<string, string> URLAssociations = new Dictionary<string, string>();

        private const string StartMenuKeyName = "StartMenu";
        public Dictionary<string, string> StartMenu = new Dictionary<string, string>();


        public static Capabilities Load(RegistryKey key)
        {
            Capabilities res = new Capabilities();
            if (key != null)
            {

                res.Default = (string)key.GetValue(DefaultKeyName);
                res.ApplicationDescription = (string)key.GetValue(ApplicationDescriptionKeyName);
                res.ApplicationIcon = (string)key.GetValue(ApplicationIconKeyName);
                res.ApplicationName = (string)key.GetValue(ApplicationNameKeyName);
                RegistryKey subkey = key.OpenSubKey(FileAssociationKeyName, false);
                if (subkey != null)
                {
                    foreach (string Key in subkey.GetValueNames())
                    {
                        object o = subkey.GetValue(Key);
                        if (o != null)
                        {
                            res.FileAssociation.Add(Key, (string)o);
                        }
                    }
                }

                subkey = key.OpenSubKey(URLAssociationsKeyName, false);
                if (subkey != null)
                {
                    foreach (string Key in subkey.GetValueNames())
                    {
                        object o = subkey.GetValue(Key);
                        if (o != null)
                        {
                            res.URLAssociations.Add(Key, (string)o);
                        }
                    }
                }

                subkey = key.OpenSubKey(StartMenuKeyName, false);
                if (subkey != null)
                {
                    foreach (string Key in subkey.GetValueNames())
                    {
                        object o = subkey.GetValue(Key);
                        if (o != null)
                        {
                            res.StartMenu.Add(Key, (string)o);
                        }
                    }
                }
            }

            return res;
        }

        public void Save(RegistryKey key)
        {
            Utilities.RegSetValue(key, DefaultKeyName, this.Default);
            Utilities.RegSetValue(key, ApplicationDescriptionKeyName, this.ApplicationDescription);
            Utilities.RegSetValue(key, ApplicationIconKeyName, this.ApplicationIcon, RegistryValueKind.ExpandString);
            Utilities.RegSetValue(key, ApplicationNameKeyName, this.ApplicationName);

            RegistryKey subkey = key.CreateSubKey(FileAssociationKeyName);
            foreach (KeyValuePair<string, string> pair in this.FileAssociation)
            {
                if (pair.Key.Length == 0)
                {
                    continue;
                }
                Utilities.RegSetValue(subkey, pair.Key, pair.Value);
            }

            subkey = key.CreateSubKey(URLAssociationsKeyName);
            foreach (KeyValuePair<string, string> pair in this.URLAssociations)
            {
                if (pair.Key.Length == 0)
                {
                    continue;
                }
                Utilities.RegSetValue(subkey, pair.Key, pair.Value);
            }

            subkey = key.CreateSubKey(StartMenuKeyName);
            foreach (KeyValuePair<string, string> pair in this.StartMenu)
            {
                if (pair.Key.Length == 0)
                {
                    continue;
                }
                Utilities.RegSetValue(subkey, pair.Key, pair.Value);
            }

        }

    }
}
