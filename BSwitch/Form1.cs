using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BSwitch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const string sDefAction = "(Действие по умолчанию)";


        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            ReloadSettings();
            //menuRules.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.добавитьПравилоToolStripMenuItem,
            //this.изменитьПравилоToolStripMenuItem,
            //this.удалитьПравилоToolStripMenuItem});
            Application.Idle += new EventHandler(OnApplicationIdle);
        }

        private void OnApplicationIdle(Object sender, EventArgs e)
        {
            bool select = lvRules.SelectedItems.Count > 0;
            изменитьПравилоToolStripMenuItem.Enabled = select;
            удалитьПравилоToolStripMenuItem.Enabled = select;
        }


        private void ReloadSettings()
        {
            lvRules.BeginUpdate();
            try
            {
                lvRules.Items.Clear();
                foreach (ExecuteRule rule in ExecuteRules.Rules)
                {
                    ListViewItem item = lvRules.Items.Add(rule.TestString);
                    item.SubItems.Add((rule.Action == null) ? sDefAction : rule.Action);
                    item.Tag = rule;
                }
            }
            finally
            {
                lvRules.EndUpdate();
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void регистрацияВСистемеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Registrations.AddToStartMenuList();
            Registrations.AddToRegisteredApplication();
            Registrations.InstallExtensions();
            Registrations.InstallProtocols();
        }

        private void отменаРегистрацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Registrations.UnInstallExtensions();
            Registrations.InstallProtocols();
        }

        private void добавитьПравилоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormAddEditAction form = new FormAddEditAction())
            {
                form.TestString = ".*";
                form.Action = sDefAction;
                if (form.ShowDialog(this as IWin32Window) == DialogResult.OK)
                {
                    ExecuteRule rule = new ExecuteRule(form.TestString, (form.Action == sDefAction) ? null : form.Action);
                    ExecuteRules.Rules.Add(rule);

                    ListViewItem item = lvRules.Items.Add(rule.TestString);
                    item.SubItems.Add(form.Action);
                    item.Tag = rule;
                }
            }
        }

        private void изменитьПравилоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.lvRules.SelectedItems;
            if (items.Count > 0)
            {
                ListViewItem item = items[0];
                ExecuteRule rule = (ExecuteRule)item.Tag;
                using (FormAddEditAction form = new FormAddEditAction())
                {
                    form.TestString = rule.TestString;
                    form.Action = (rule.Action == null) ? sDefAction : rule.Action;
                    if (form.ShowDialog(this as IWin32Window) == DialogResult.OK)
                    {
                        rule.TestString = form.TestString;
                        rule.Action = (form.Action == String.Empty | form.Action == sDefAction) ? null : form.Action;
                        if (rule.Modified)
                        {
                            item.Text = rule.TestString;
                            item.SubItems[1].Text = (rule.Action == null) ? sDefAction : rule.Action;
                        }

                    }
                }

            }
        }

        private void удалитьПравилоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.lvRules.SelectedItems;
            if (items.Count > 0)
            {
                ListViewItem item = items[0];
                int index = item.Index;
                item.Remove();
                if (index >= lvRules.Items.Count)
                {
                    index = lvRules.Items.Count - 1;
                }
                lvRules.Items[index].Selected = true;
                ExecuteRule rule = (ExecuteRule)item.Tag;
                ExecuteRules.Rules.Remove(rule);
            }

        }

        private void lvRules_DoubleClick(object sender, EventArgs e)
        {
            изменитьПравилоToolStripMenuItem_Click(sender, e);
        }

        private ListViewItem Dragged = null;
        private ListViewItem DropedOn = null;
        private Rectangle DragBox = Rectangle.Empty;
        private Point screenOffset = Point.Empty;

        private void lvRules_MouseDown(object sender, MouseEventArgs e)
        {
            Dragged = lvRules.GetItemAt(e.X, e.Y);

            if (Dragged != null)
            {

                Size dragSize = SystemInformation.DragSize;
                DragBox = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                             e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                DragBox = Rectangle.Empty;

        }

        private void lvRules_MouseUp(object sender, MouseEventArgs e)
        {
            DragBox = Rectangle.Empty;
        }

        private void lvRules_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {

                if (DragBox != Rectangle.Empty && !DragBox.Contains(e.X, e.Y))
                {

                    screenOffset = SystemInformation.WorkingArea.Location;

                    DragDropEffects dropEffect = lvRules.DoDragDrop(Dragged, DragDropEffects.All);
                }
            }

        }

        private void lvRules_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(ListViewItem)))
            {

                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = DragDropEffects.Copy;

            DropedOn = lvRules.GetItemAt(e.X, e.Y);

       }

        private void lvRules_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv != null)
            {

                Form f = lv.FindForm();

                // Cancel the drag if the mouse moves off the form. The screenOffset
                // takes into account any desktop bands that may be at the top or left
                // side of the screen.
                if (((Control.MousePosition.X - screenOffset.X) < f.DesktopBounds.Left) ||
                    ((Control.MousePosition.X - screenOffset.X) > f.DesktopBounds.Right) ||
                    ((Control.MousePosition.Y - screenOffset.Y) < f.DesktopBounds.Top) ||
                    ((Control.MousePosition.Y - screenOffset.Y) > f.DesktopBounds.Bottom))
                {

                    e.Action = DragAction.Cancel;
                }
            }

        }

        private void lvRules_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = lvRules.PointToClient(new Point(e.X, e.Y));
            ExecuteRule rule = (ExecuteRule)Dragged.Tag;
            lvRules.Items.Remove(Dragged);
            ExecuteRules.Rules.Remove(rule);

            DropedOn = lvRules.GetItemAt(clientPoint.X, clientPoint.Y);
            if (DropedOn != null)
            {
                lvRules.Items.Insert(DropedOn.Index, Dragged);
                ExecuteRules.Rules.Insert(ExecuteRules.Rules.IndexOf((ExecuteRule)DropedOn.Tag), rule);    
            }
            else
            {
                lvRules.Items.Add(Dragged);
                ExecuteRules.Rules.Add(rule);
            }
        }

        private void lvRules_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = true;
        }


    }
}
