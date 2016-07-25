using System;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.AccessControl;
using System.Security.Principal;


namespace BSwitch
{
    public class Utilities
    {

        public static RegistryKey OpenSubKeyForReadWrite(RegistryKey Parent, string Name)
        {
            NTAccount account = new NTAccount(Environment.UserName);
            SecurityIdentifier si = (SecurityIdentifier)account.Translate(typeof(SecurityIdentifier));
            RegistrySecurity rs = new RegistrySecurity();
            RegistryRights rr = RegistryRights.ChangePermissions | RegistryRights.Delete | 
                   RegistryRights.QueryValues | RegistryRights.ReadKey | 
                   RegistryRights.ReadPermissions | RegistryRights.SetValue | RegistryRights.WriteKey;
            rs.AddAccessRule(new RegistryAccessRule(account, rr, AccessControlType.Allow));

            using (RegistryKey key = Parent.OpenSubKey(Name, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions))
            {
                ClearPermissionToSetValue(key);
            }
            
            RegistryKey res = Parent.CreateSubKey(Name, RegistryKeyPermissionCheck.Default, rs);            
            return res;
        }

        public static void RegSetValue(RegistryKey key, string Name, object value)
        {
            RegSetValue(key, Name, value, RegistryValueKind.String);
        }

        public static void RegSetValue(RegistryKey key, string Name, object value, RegistryValueKind DefKind)
        {
            if (value == null)
                return;
            RegistryValueKind kind = DefKind;
            try
            {
                kind = key.GetValueKind(Name);
            }
            catch (IOException) { }
            finally
            {
                try
                {
                    key.SetValue(Name, value, kind);
                }
                catch (UnauthorizedAccessException)
                {
                    ClearPermissionToSetValue(key);
                    key.SetValue(Name, value, kind);
                }
            }

        }

        static void ClearPermissionToSetValue(RegistryKey key)
        {
            NTAccount account = new NTAccount(Environment.UserName);
            SecurityIdentifier si = (SecurityIdentifier)account.Translate(typeof(SecurityIdentifier));
            RegistrySecurity rs = key.GetAccessControl();
            foreach (RegistryAccessRule rule in rs.GetAccessRules(true, false, si.GetType()))
            {
                if ((rule.AccessControlType == AccessControlType.Deny) &&
                    ((rule.RegistryRights & RegistryRights.SetValue) != 0))
                    rs.RemoveAccessRule(rule);
            }
            key.SetAccessControl(rs);
        }

        public static string FindExecutableString(string Extension)
        {
            string res = AssocQueryString((AssocF)0, AssocStr.Command, Extension, null);
            return res;
        }



        [DllImport("Shlwapi.dll", CharSet=CharSet.Auto, SetLastError = true)]
        private static extern IntPtr AssocQueryString(AssocF flags, AssocStr str,
            string Assoc,
            string Extra,
            [Out] StringBuilder Out,
            [In, Out] ref uint CountOut);

        public static string AssocQueryString(AssocF flags, AssocStr str, string Assoc, string Extra)
        {
            uint Size = 0;
            AssocQueryString(flags | AssocF.NoTruncate, str, Assoc, Extra, null, ref Size);
            StringBuilder sb = new StringBuilder((int)Size);
            AssocQueryString(flags, str, Assoc, Extra, sb, ref Size);
            return sb.ToString();
        }
        [Flags]
        public enum AssocF
        {
            Init_NoRemapCLSID = 0x1,
            Init_ByExeName = 0x2,
            Open_ByExeName = 0x2,
            Init_DefaultToStar = 0x4,
            Init_DefaultToFolder = 0x8,
            NoUserSettings = 0x10,
            NoTruncate = 0x20,
            Verify = 0x40,
            RemapRunDll = 0x80,
            NoFixUps = 0x100,
            IgnoreBaseClass = 0x200
        }

        public enum AssocStr
        {
            Command = 1,
            Executable,
            FriendlyDocName,
            FriendlyAppName,
            NoOpen,
            ShellNewValue,
            DDECommand,
            DDEIfExec,
            DDEApplication,
            DDETopic
        }

        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            ShowCommands nShowCmd);

    }
}
