using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BSwitch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                string Action = ExecuteRules.GetActionForQuery(args[0]);
                if (Action == null)
                {
                    // выполняем действие по умолчанию

                    Dictionary<string, object> state = Registrations.SaveCurrentState();
                    try
                    {
                        Registrations.RestoreBackupedValues();

                        // защита от зацикливания - проверяем  ен запускаем ли мы сами себя
                        string ExeString = Utilities.FindExecutableString(args[0]);
                        if (ExeString.IndexOf(System.IO.Path.GetFileName(Application.ExecutablePath)) < 0)
                        {
                            Utilities.ShellExecute(IntPtr.Zero, null, args[0], String.Join(" ", args, 1, args.Length - 1).Trim(), null, Utilities.ShowCommands.SW_NORMAL);
                            //System.Threading.Thread.Sleep(1000);

                        }

                    }
                    finally
                    {
                        Registrations.RestoreCurrentState(state);
                    }
                }
                else
                {
                    //выполняем назначенное действие 
                    Utilities.ShellExecute(IntPtr.Zero, null, Action, String.Join(" ", args), null, Utilities.ShowCommands.SW_NORMAL);
                }
            }

        }
    }
}
