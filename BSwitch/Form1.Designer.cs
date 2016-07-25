namespace BSwitch
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.регистрацияВСистемеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отменаРегистрацииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.правилаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьПравилоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьПравилоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьПравилоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvRules = new System.Windows.Forms.ListView();
            this.columnTestString = new System.Windows.Forms.ColumnHeader();
            this.columnAction = new System.Windows.Forms.ColumnHeader();
            this.menuRules = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.menuRules.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.правилаToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(662, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.регистрацияВСистемеToolStripMenuItem,
            this.отменаРегистрацииToolStripMenuItem,
            this.toolStripMenuItem1,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // регистрацияВСистемеToolStripMenuItem
            // 
            this.регистрацияВСистемеToolStripMenuItem.Name = "регистрацияВСистемеToolStripMenuItem";
            this.регистрацияВСистемеToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.регистрацияВСистемеToolStripMenuItem.Text = "Регистрация в системе";
            this.регистрацияВСистемеToolStripMenuItem.Click += new System.EventHandler(this.регистрацияВСистемеToolStripMenuItem_Click);
            // 
            // отменаРегистрацииToolStripMenuItem
            // 
            this.отменаРегистрацииToolStripMenuItem.Name = "отменаРегистрацииToolStripMenuItem";
            this.отменаРегистрацииToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.отменаРегистрацииToolStripMenuItem.Text = "Отмена регистрации";
            this.отменаРегистрацииToolStripMenuItem.Click += new System.EventHandler(this.отменаРегистрацииToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(197, 6);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // правилаToolStripMenuItem
            // 
            this.правилаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьПравилоToolStripMenuItem,
            this.изменитьПравилоToolStripMenuItem,
            this.удалитьПравилоToolStripMenuItem});
            this.правилаToolStripMenuItem.Name = "правилаToolStripMenuItem";
            this.правилаToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.правилаToolStripMenuItem.Text = "Правила";
            // 
            // добавитьПравилоToolStripMenuItem
            // 
            this.добавитьПравилоToolStripMenuItem.Name = "добавитьПравилоToolStripMenuItem";
            this.добавитьПравилоToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.добавитьПравилоToolStripMenuItem.Text = "Добавить правило";
            this.добавитьПравилоToolStripMenuItem.Click += new System.EventHandler(this.добавитьПравилоToolStripMenuItem_Click);
            // 
            // изменитьПравилоToolStripMenuItem
            // 
            this.изменитьПравилоToolStripMenuItem.Name = "изменитьПравилоToolStripMenuItem";
            this.изменитьПравилоToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.изменитьПравилоToolStripMenuItem.Text = "Изменить правило";
            this.изменитьПравилоToolStripMenuItem.Click += new System.EventHandler(this.изменитьПравилоToolStripMenuItem_Click);
            // 
            // удалитьПравилоToolStripMenuItem
            // 
            this.удалитьПравилоToolStripMenuItem.Name = "удалитьПравилоToolStripMenuItem";
            this.удалитьПравилоToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.удалитьПравилоToolStripMenuItem.Text = "Удалить правило";
            this.удалитьПравилоToolStripMenuItem.Click += new System.EventHandler(this.удалитьПравилоToolStripMenuItem_Click);
            // 
            // lvRules
            // 
            this.lvRules.AllowDrop = true;
            this.lvRules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTestString,
            this.columnAction});
            this.lvRules.ContextMenuStrip = this.menuRules;
            this.lvRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRules.FullRowSelect = true;
            this.lvRules.GridLines = true;
            this.lvRules.LabelEdit = true;
            this.lvRules.LabelWrap = false;
            this.lvRules.Location = new System.Drawing.Point(0, 24);
            this.lvRules.MultiSelect = false;
            this.lvRules.Name = "lvRules";
            this.lvRules.Size = new System.Drawing.Size(662, 388);
            this.lvRules.TabIndex = 1;
            this.lvRules.UseCompatibleStateImageBehavior = false;
            this.lvRules.View = System.Windows.Forms.View.Details;
            this.lvRules.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.lvRules_QueryContinueDrag);
            this.lvRules.DoubleClick += new System.EventHandler(this.lvRules_DoubleClick);
            this.lvRules.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvRules_MouseUp);
            this.lvRules.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvRules_DragDrop);
            this.lvRules.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lvRules_MouseMove);
            this.lvRules.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvRules_MouseDown);
            this.lvRules.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.lvRules_GiveFeedback);
            this.lvRules.DragOver += new System.Windows.Forms.DragEventHandler(this.lvRules_DragOver);
            // 
            // columnTestString
            // 
            this.columnTestString.Text = "Строка поиска";
            this.columnTestString.Width = global::BSwitch.Properties.Settings.Default.ColumnTestStringWidth;
            // 
            // columnAction
            // 
            this.columnAction.Text = "Программа";
            this.columnAction.Width = global::BSwitch.Properties.Settings.Default.ColumnActionWidth;
            // 
            // menuRules
            // 
            this.menuRules.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьToolStripMenuItem,
            this.изменитьToolStripMenuItem,
            this.удалитьToolStripMenuItem});
            this.menuRules.Name = "menuRules";
            this.menuRules.Size = new System.Drawing.Size(129, 70);
            // 
            // добавитьToolStripMenuItem
            // 
            this.добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
            this.добавитьToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.добавитьToolStripMenuItem.Text = "Добавить";
            this.добавитьToolStripMenuItem.Click += new System.EventHandler(this.добавитьПравилоToolStripMenuItem_Click);
            // 
            // изменитьToolStripMenuItem
            // 
            this.изменитьToolStripMenuItem.Name = "изменитьToolStripMenuItem";
            this.изменитьToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.изменитьToolStripMenuItem.Text = "Изменить";
            this.изменитьToolStripMenuItem.Click += new System.EventHandler(this.изменитьПравилоToolStripMenuItem_Click);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьПравилоToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 412);
            this.Controls.Add(this.lvRules);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.Name = "Form1";
            this.Text = "\"\"";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.menuRules.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem регистрацияВСистемеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отменаРегистрацииToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ListView lvRules;
        private System.Windows.Forms.ColumnHeader columnTestString;
        private System.Windows.Forms.ColumnHeader columnAction;
        private System.Windows.Forms.ToolStripMenuItem правилаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьПравилоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьПравилоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьПравилоToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuRules;
        private System.Windows.Forms.ToolStripMenuItem добавитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
    }
}

