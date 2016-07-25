using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Security.AccessControl;


namespace BSwitch
{
    public class Registrations
    {
        public static readonly string[] FileExtensions = { ".htm", ".html", ".url", ".shtml", ".xml", ".mht", ".mhtml", ".xhtml" };
        public static readonly string[] Protocols = { "http", "ftp", "https"};

        public static string GetQuotetedAppPath()
        {
            return "\"" + Application.ExecutablePath + "\"";
        }


        const string RegBackupKeyName = "BSwitch backup";
        static object GetSetValueWithBackup(RegistryKey key, string Name, object Value)
        {
            // сохраняет значение в реестр, а старое заносит в поле RegBackupKeyName и возвращает
            object res = key.GetValue(Name);
            Utilities.RegSetValue(key, Name, Value);
            object backup = key.GetValue(RegBackupKeyName);
            if (backup == null)
            {
                Utilities.RegSetValue(key, RegBackupKeyName, res);
            }
            else
            {
                string sBack = (string)backup;
                if ((sBack == GetUrlType() || sBack == GetFileType()) && res != GetFileType() && res != GetUrlType())
                {
                    Utilities.RegSetValue(key, RegBackupKeyName, res);
                }
            }
            return res;
        }


        // возвращает значение из бэкапа и записывает его в поле Name
        static object RestoreValueFromBackup(RegistryKey Key, string Name)
        {
            object res = Key.GetValue(RegBackupKeyName);
            if (res != null && res != GetFileType() && res != GetUrlType())
            {
                Key.DeleteValue(RegBackupKeyName);
                Utilities.RegSetValue(Key, Name, res);
            }
            return res;
        }


        const string RegFileExt = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\{0}\UserChoice";

        static public void InstallExtensions()
        {
            foreach (string extension in FileExtensions)
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, false))
                {
                    if (key != null)
                    {
                        string Type = (string)key.GetValue(String.Empty);
                        if (Type != String.Empty)
                        {
                            InstallCommand(Type);
                        }
                    }
                }

                //key = Registry.CurrentUser.OpenSubKey(String.Format(RegFileExt, extension),RegistryKeyPermissionCheck.ReadWriteSubTree,);
                using (RegistryKey key = Utilities.OpenSubKeyForReadWrite(Registry.CurrentUser, String.Format(RegFileExt, extension)))
                {
                    if (key != null)
                    {
                        string FileType = GetFileType();
                        GetSetValueWithBackup(key, sProgid, FileType);
                    }
                }
            }
        }

        static public void UnInstallExtensions()
        {
            foreach (string extension in FileExtensions)
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, false))
                {
                    if (key != null)
                    {
                        string Type = (string)key.GetValue(String.Empty);
                        if (Type != String.Empty)
                        {
                            UnInstallCommand(Type);
                        }
                    }
                }
                using (RegistryKey key = Utilities.OpenSubKeyForReadWrite(Registry.CurrentUser, String.Format(RegFileExt, extension)))
                {
                    if (key != null)
                    {
                        RestoreValueFromBackup(key, sProgid);
                    }
                }
            }
        }


        static void InstallCommand(string Protocol)
        {
            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(Protocol + @"\shell\open\command"))
            {
                GetSetValueWithBackup(key, String.Empty, GetQuotetedAppPath() + " \"%1\"");
            }
        }

        static void UnInstallCommand(string Protocol)
        {
            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(Protocol + @"\shell\open\command"))
            {
                RestoreValueFromBackup(key, String.Empty);
            }
        }

        const string RegProtocol = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\{0}\UserChoice";

        static public void InstallProtocols()
        {
            foreach (string Protocol in Protocols)
            {
                InstallCommand(Protocol);
                using (RegistryKey key = Utilities.OpenSubKeyForReadWrite(Registry.CurrentUser, String.Format(RegProtocol, Protocol)))
                {
                    string Url = GetUrlType();
                    GetSetValueWithBackup(key, sProgid, Url);
                }
            }
        }

        static public void UnInstallProtocols()
        {
            foreach (string Protocol in Protocols)
            {
                UnInstallCommand(Protocol);
                using (RegistryKey key = Utilities.OpenSubKeyForReadWrite(Registry.CurrentUser, String.Format(RegProtocol, Protocol)))
                {
                    if (key != null)
                    {
                        RestoreValueFromBackup(key, sProgid);
                    }
                }
            }
        }

        static public void AddToStartMenuList()
        {
            // Добавляет это приложение в список выбора Браузера по умолчанию в меню Пуск
            StartMenuInternetItems items = StartMenuInternetItems.LoadFromRegistry();
            StartMenuInternetItem newitem = new StartMenuInternetItem();
            newitem.Name = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            newitem.Description = "Browser Switch";
            newitem.DefaultIcon = GetIconPath();
            StartMenuInternetItem.ShellCommand command = new StartMenuInternetItem.ShellCommand();
            command.Command = GetQuotetedAppPath();
            newitem.ShellCommands.Add("open", command);
            newitem.InstallInfo.HideIconsCommand = command.Command;
            newitem.InstallInfo.ReinstallCommand = command.Command;
            newitem.InstallInfo.ShowIconsCommand = command.Command;
            newitem.InstallInfo.IconsVisible = true;

            string FileType = AddFileType();
            foreach (string item in FileExtensions)
            {
                newitem.Capabilities.FileAssociation[item] = FileType;
            }

            newitem.Capabilities.ApplicationDescription = "Select browser according to URL";
            newitem.Capabilities.ApplicationIcon = newitem.DefaultIcon;
            newitem.Capabilities.ApplicationName = Application.ProductName;

            string UrlType = AddUrlType();
            foreach (string item in Protocols)
            {
                newitem.Capabilities.URLAssociations[item] = UrlType;
            }

            newitem.Capabilities.StartMenu["StartMenuInternet"] = newitem.Name;
            items.Add(newitem);
            items.SaveToRegistry();
        }

        static public string GetIconPath()
        {
            return "\"" + Application.ExecutablePath + "\",0";
        }


        static public void AddToRegisteredApplication()
        {
            // добавляет в список зарегистрированных приложений, (в Висте для выбора программ по умолчанию)

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\RegisteredApplications", true))
            {
                string Cap = @"SOFTWARE\Clients\StartMenuInternet\" + Path.GetFileNameWithoutExtension(Application.ExecutablePath) + @"\Capabilities";
                Utilities.RegSetValue(key, Application.ProductName, Cap, RegistryValueKind.String);
            }
        }

        static string GetFileType()
        {
            return Application.ProductName + ".File";
        }

        static public string AddFileType()
        {
            string res = GetFileType();
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\" + res, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                const string HTMLFile = "HTML Document";
                Utilities.RegSetValue(key, "", HTMLFile);
                Utilities.RegSetValue(key, "FriendlyTypeName", HTMLFile);
                RegistryKey subkey = key.CreateSubKey("DefaultIcon", RegistryKeyPermissionCheck.ReadWriteSubTree);
                Utilities.RegSetValue(subkey, "", GetIconPath());

                using (subkey = key.CreateSubKey(@"shell\open\command", RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    Utilities.RegSetValue(subkey, "", "\"" + Application.ExecutablePath + "\" \"%1\"");
                }
            }
            return res;

        }

        static string GetUrlType()
        {
            return Application.ProductName + ".Url";
        }

        static public string AddUrlType()
        {
            string res = GetUrlType();
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\" + res, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                const string URL = "HTTP Protocol";
                Utilities.RegSetValue(key, "", URL, RegistryValueKind.String);
                Utilities.RegSetValue(key, "FriendlyTypeName", URL);
                Utilities.RegSetValue(key, "EditFlags", 2, RegistryValueKind.DWord);
                Utilities.RegSetValue(key, "URL Protocol", "", RegistryValueKind.String);
                RegistryKey subkey = key.CreateSubKey("DefaultIcon", RegistryKeyPermissionCheck.ReadWriteSubTree);
                Utilities.RegSetValue(subkey, "", GetIconPath());

                using (subkey = key.CreateSubKey(@"shell\open\command", RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    Utilities.RegSetValue(subkey, "", "\"" + Application.ExecutablePath + "\" \"%1\"");
                }
            }
            return res;
        }

        const string sProgid = "Progid";
        static public Dictionary<string, object> SaveCurrentState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            #region SaveFileExtensions
            foreach (string extension in FileExtensions)
            {
                using (RegistryKey keyExt = Registry.ClassesRoot.OpenSubKey(extension, false))
                {
                    if (keyExt != null)
                    {
                        string Type = (string)keyExt.GetValue(String.Empty);
                        if (Type != String.Empty)
                        {
                            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(Type))
                            {
                                if (key != null)
                                {
                                    using (RegistryKey subkey = key.OpenSubKey(@"shell\open\command"))
                                    {
                                        if (subkey != null)
                                            try
                                            {
                                                state.Add(subkey.Name + @"\", subkey.GetValue(String.Empty));
                                                state.Add(subkey.Name + @"\" + RegBackupKeyName, subkey.GetValue(RegBackupKeyName));
                                            }
                                            catch (ArgumentException)
                                            {
                                                // такой элемент уже сохранен - пропускаем
                                            }
                                    }
                                }
                            }
                        }
                    }
                }

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(String.Format(RegFileExt, extension)))
                {
                    if (key != null)
                    {
                        state.Add(key.Name + @"\" + sProgid, key.GetValue(sProgid));
                        state.Add(key.Name + @"\" + RegBackupKeyName, key.GetValue(RegBackupKeyName));
                    }
                }
            }

            #endregion


            #region SaveProtocols
            foreach (string Protocol in Protocols)
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(Protocol, false))
                {
                    if (key != null)
                    {
                        using (RegistryKey subkey = key.OpenSubKey(@"shell\open\command"))
                        {
                            if (subkey != null)
                                state.Add(subkey.Name + @"\", subkey.GetValue(String.Empty));
                                state.Add(subkey.Name + @"\" + RegBackupKeyName, subkey.GetValue(RegBackupKeyName));

                        }
                    }
                }
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(String.Format(RegProtocol, Protocol), true))
                {
                    if (key != null)
                    {
                        state.Add(key.Name + @"\" + sProgid, key.GetValue(sProgid));
                        state.Add(key.Name + @"\" + RegBackupKeyName, key.GetValue(RegBackupKeyName));
                    }
                }
            }
            #endregion

            return state;
        }

        static public void RestoreBackupedValues()
        {
            #region RestoreFileExtensions
            foreach (string extension in FileExtensions)
            {
                using (RegistryKey keyExt = Registry.ClassesRoot.OpenSubKey(extension, false))
                {
                    if (keyExt != null)
                    {
                        string Type = (string)keyExt.GetValue(String.Empty);
                        if (Type != String.Empty)
                        {
                            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(Type))
                            {
                                if (key != null)
                                {
                                    using (RegistryKey subkey = key.OpenSubKey(@"shell\open\command", true))
                                    {
                                        if (subkey != null)
                                            RestoreValueFromBackup(subkey, String.Empty);
                                    }
                                }
                            }
                        }
                    }
                }


                string Path = String.Format(RegFileExt, extension);
                using (RegistryKey key = Utilities.OpenSubKeyForReadWrite(Registry.CurrentUser, Path))
                {
                    if (key != null)
                    {
                        RestoreValueFromBackup(key, sProgid);
                    }
                }

            }

            #endregion


            #region RestoreProtocols
            foreach (string Protocol in Protocols)
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(Protocol + @"\shell\open\command", true))
                {
                    if (key != null)
                    {
                        RestoreValueFromBackup(key, String.Empty);
                    }
                }
                string Path = String.Format(RegProtocol, Protocol);
                using (RegistryKey key = Utilities.OpenSubKeyForReadWrite(Registry.CurrentUser, Path))
                {
                    if (key != null)
                    {
                        RestoreValueFromBackup(key, sProgid);
                    }
                }
            }
            #endregion
        }

        static public void RestoreCurrentState(Dictionary<string, object> state)
        {
            foreach (KeyValuePair<string, object> item in state)
            {
                int iPos = item.Key.LastIndexOf(@"\");
                string Path = item.Key.Substring(0, iPos);
                string Name = item.Key.Substring(iPos + 1);
                try
                {
                    Registry.SetValue(Path, Name, item.Value);
                }
                catch (UnauthorizedAccessException)
                {

                    using (RegistryKey key = GetRegistryKeyFromString(Path))
                    {
                        Utilities.RegSetValue(key, Name, item.Value);
                    }
                }
                catch (ArgumentNullException)
                {
                    // нечего сохранять
                };
            }
        }

        static RegistryKey GetRegistryKeyFromString(string Path)
        {

            int iPos = Path.IndexOf(@"\");
            string sTop = Path.Substring(0, iPos).ToUpper();
            string[] sKeys = { "HKEY_CLASSES_ROOT", "HKEY_CURRENT_USER", "HKEY_LOCAL_MACHINE", "HKEY_USERS", "HKEY_CURRENT_CONFIG", "HKEY_PERFORMANCE_DATA", "HKEY_DYN_DATA" };
            RegistryKey[] Keys = { Registry.ClassesRoot, Registry.CurrentUser, Registry.LocalMachine, Registry.Users, Registry.CurrentConfig, Registry.PerformanceData, Registry.DynData };
            int index = 0;
            foreach (string item in sKeys)
            {
                if (item == sTop)
                {
                    return Keys[index].OpenSubKey(Path.Substring(iPos + 1));
                }
                index++;
            }
            return null;
        }



        //static void SetValueProgId(RegistryKey key, object Value)
        //{
        //    if (key != null)
        //    {
        //        Dictionary<string, object> values = new Dictionary<string, object>();
        //        foreach (string item in key.GetValueNames())
        //        {
        //            values.Add(item, key.GetValue(item));
        //        }
        //        values[sProgid] = Value;

        //        int iLastPos = key.Name.LastIndexOf(@"\");
        //        int iFirstPos = key.Name.IndexOf(@"\");
        //        string KeyName = key.Name.Substring(iLastPos + 1);
        //        string Parent = key.Name.Substring(iFirstPos + 1, iLastPos - iFirstPos - 1);
        //        key = Registry.CurrentUser.OpenSubKey(Parent, RegistryKeyPermissionCheck.ReadWriteSubTree);
        //        key.DeleteSubKey(KeyName);
        //        key = key.CreateSubKey(KeyName);

        //        foreach (KeyValuePair<string, object> item in values)
        //        {
        //            key.SetValue(item.Key, item.Value);
        //        }
        //    }
        //}
    }
}
