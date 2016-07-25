using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BSwitch
{
    public partial class FormAddEditAction : Form
    {
        public string TestString;
        public string Action;

        public FormAddEditAction()
        {
            InitializeComponent();
        }

        private void FormAddEditAction_Load(object sender, EventArgs e)
        {
            tbAction.Text = this.Action;
            tbTestString.Text = this.TestString;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Action = tbAction.Text;
            this.TestString = tbTestString.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string exename = tbAction.Text.Trim();
            string parameters = "";
            int iPos = exename.IndexOf('%');
            if (iPos > 0)
            {

                iPos = exename.LastIndexOf(' ',iPos );
                if (iPos > 0)
                {
                    parameters = exename.Substring(iPos).Trim();
                    exename = exename.Substring(0, iPos).Trim();
                }
            }
            ofdSelectExe.FileName = exename;
            if (ofdSelectExe.ShowDialog(this as IWin32Window) == DialogResult.OK)
            {
                tbAction.Text = ofdSelectExe.FileName + " " + parameters;
            }
        }


    }
}
