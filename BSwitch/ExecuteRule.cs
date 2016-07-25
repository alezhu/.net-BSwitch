using System;
using System.Collections.Generic;
using System.Text;

namespace BSwitch
{
    [Serializable]
    public class ExecuteRule
    {
        public ExecuteRule(string testString, string action) {
            this._testString = testString;
            this._action = action;
            this._modified = false;
        }

        [NonSerialized]
        private bool _modified = false;
        public bool Modified {
            get {return _modified;}
        }
        private string _testString = null;
        public string TestString {
            get {return _testString;}
            set {
                _modified |= value == _testString;
                _testString = value;
            }
        }

        private string _action = null;
        public string Action
        {
            get { return _action; }
            set
            {
                _modified |= value == _action;
                _action = value;
            }
        }
    }
}
