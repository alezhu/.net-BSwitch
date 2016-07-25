using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
namespace BSwitch
{
    [Serializable]
    public class ExecuteRules : ArrayList
    {
        public new ExecuteRule this[int index]
        {
            get { return (ExecuteRule)base[index]; }
            set { base[index] = value; }
        }


        static private ExecuteRules _rules = null;
        static public ExecuteRules Rules
        {
            get
            {
                if (_rules == null)
                {
                    _rules = ExecuteRules.Load();
                }
                return _rules;
            }
        }

        static private ExecuteRules Load()
        {
            ExecuteRules res = null;
            IFormatter formatter = new BinaryFormatter();
            using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("bswitch rules.dat", FileMode.OpenOrCreate,
                IsolatedStorageFile.GetUserStoreForDomain()))
            {
                try
                {
                    res = (ExecuteRules)formatter.Deserialize(stream);
                }
                catch (SerializationException)
                {
                    res = new ExecuteRules();
                }

            }
            return res;
        }


        private bool _modified = false;
        private bool Modified
        {
            get
            {
                if (_modified)
                {
                    return true;
                }
                foreach (ExecuteRule rule in this)
                {
                    if (rule.Modified)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        ~ExecuteRules()
        {
            if (Modified)
            {
                Save();

            }
        }

        private void Save()
        {
            IFormatter formatter = new BinaryFormatter();
            using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("bswitch rules.dat", FileMode.OpenOrCreate,
                IsolatedStorageFile.GetUserStoreForDomain()))
            {
                formatter.Serialize(stream, this);
            }
        }

        public static string GetActionForQuery(string query)
        {
            foreach (ExecuteRule item in Rules)
            {
                if (Regex.IsMatch(query, item.TestString))
                {
                    // нашли
                    return item.Action;
                }
            }
            return null;
        }

        public int Add(ExecuteRule Rule)
        {
            _modified = true;
            return base.Add(Rule);
        }


        public void Remove(ExecuteRule Rule)
        {
            _modified = true;
            base.Remove(Rule);
        }

        public void Swap(int Index1, int Index2)
        {
            ExecuteRule temp = this[Index1];
            this[Index1] = this[Index2];
            this[Index2] = temp;
            _modified = true;
        }


        public void Swap(ExecuteRule Rule1, ExecuteRule Rule2)
        {
            this.Swap(this.IndexOf(Rule1),this.IndexOf(Rule2));
        }


    }
}
