﻿namespace Microsoft.Boogie.ModelViewer
{
  partial class Main
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
      if (disposing && (components != null)) {
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
      this.currentStateView = new System.Windows.Forms.ListView();
      this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.prevValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.stateViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.dummyItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.matchesList = new System.Windows.Forms.ListView();
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.linkLabel1 = new System.Windows.Forms.LinkLabel();
      this.label1 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.stateList = new System.Windows.Forms.ListView();
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.stateViewMenu.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.SuspendLayout();
      // 
      // currentStateView
      // 
      this.currentStateView.BackColor = System.Drawing.SystemColors.Window;
      this.currentStateView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.value,
            this.prevValue});
      this.currentStateView.ContextMenuStrip = this.stateViewMenu;
      this.currentStateView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.currentStateView.FullRowSelect = true;
      this.currentStateView.GridLines = true;
      this.currentStateView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.currentStateView.Location = new System.Drawing.Point(0, 0);
      this.currentStateView.MultiSelect = false;
      this.currentStateView.Name = "currentStateView";
      this.currentStateView.OwnerDraw = true;
      this.currentStateView.ShowItemToolTips = true;
      this.currentStateView.Size = new System.Drawing.Size(677, 558);
      this.currentStateView.TabIndex = 0;
      this.currentStateView.UseCompatibleStateImageBehavior = false;
      this.currentStateView.View = System.Windows.Forms.View.Details;
      this.currentStateView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView1_ColumnWidthChanged);
      this.currentStateView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView1_DrawColumnHeader);
      this.currentStateView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listView1_DrawItem);
      this.currentStateView.SelectedIndexChanged += new System.EventHandler(this.currentStateView_SelectedIndexChanged);
      this.currentStateView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
      this.currentStateView.Resize += new System.EventHandler(this.listView1_Resize);
      // 
      // name
      // 
      this.name.Text = "Name";
      this.name.Width = 174;
      // 
      // value
      // 
      this.value.Text = "Value";
      this.value.Width = 99;
      // 
      // prevValue
      // 
      this.prevValue.Text = "Previous";
      this.prevValue.Width = 147;
      // 
      // stateViewMenu
      // 
      this.stateViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyItemToolStripMenuItem});
      this.stateViewMenu.Name = "stateViewMenu";
      this.stateViewMenu.Size = new System.Drawing.Size(145, 26);
      this.stateViewMenu.Opening += new System.ComponentModel.CancelEventHandler(this.stateViewMenu_Opening);
      // 
      // dummyItemToolStripMenuItem
      // 
      this.dummyItemToolStripMenuItem.Name = "dummyItemToolStripMenuItem";
      this.dummyItemToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
      this.dummyItemToolStripMenuItem.Text = "Dummy item";
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.stateList);
      this.splitContainer1.Size = new System.Drawing.Size(915, 726);
      this.splitContainer1.SplitterDistance = 677;
      this.splitContainer1.TabIndex = 1;
      // 
      // splitContainer2
      // 
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.currentStateView);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.matchesList);
      this.splitContainer2.Panel2.Controls.Add(this.linkLabel1);
      this.splitContainer2.Panel2.Controls.Add(this.label1);
      this.splitContainer2.Panel2.Controls.Add(this.textBox1);
      this.splitContainer2.Size = new System.Drawing.Size(677, 726);
      this.splitContainer2.SplitterDistance = 558;
      this.splitContainer2.TabIndex = 1;
      // 
      // matchesList
      // 
      this.matchesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.matchesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
      this.matchesList.FullRowSelect = true;
      this.matchesList.GridLines = true;
      this.matchesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.matchesList.Location = new System.Drawing.Point(0, 29);
      this.matchesList.MultiSelect = false;
      this.matchesList.Name = "matchesList";
      this.matchesList.OwnerDraw = true;
      this.matchesList.ShowItemToolTips = true;
      this.matchesList.Size = new System.Drawing.Size(677, 135);
      this.matchesList.TabIndex = 4;
      this.matchesList.UseCompatibleStateImageBehavior = false;
      this.matchesList.View = System.Windows.Forms.View.Details;
      this.matchesList.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView1_ColumnWidthChanged);
      this.matchesList.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView1_DrawColumnHeader);
      this.matchesList.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listView1_DrawItem);
      this.matchesList.DoubleClick += new System.EventHandler(this.matchesList_DoubleClick);
      this.matchesList.Resize += new System.EventHandler(this.matchesList_Resize);
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Name";
      this.columnHeader4.Width = 300;
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "Value";
      this.columnHeader5.Width = 350;
      // 
      // linkLabel1
      // 
      this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Location = new System.Drawing.Point(617, 5);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new System.Drawing.Size(57, 13);
      this.linkLabel1.TabIndex = 3;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "Show All...";
      this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
      this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 5);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(44, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Search:";
      // 
      // textBox1
      // 
      this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox1.Location = new System.Drawing.Point(53, 3);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(558, 20);
      this.textBox1.TabIndex = 1;
      this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
      // 
      // stateList
      // 
      this.stateList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader1,
            this.columnHeader2});
      this.stateList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.stateList.FullRowSelect = true;
      this.stateList.GridLines = true;
      this.stateList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.stateList.HideSelection = false;
      this.stateList.Location = new System.Drawing.Point(0, 0);
      this.stateList.MultiSelect = false;
      this.stateList.Name = "stateList";
      this.stateList.ShowItemToolTips = true;
      this.stateList.Size = new System.Drawing.Size(234, 726);
      this.stateList.TabIndex = 0;
      this.stateList.UseCompatibleStateImageBehavior = false;
      this.stateList.View = System.Windows.Forms.View.Details;
      this.stateList.SelectedIndexChanged += new System.EventHandler(this.stateList_SelectedIndexChanged);
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "#";
      this.columnHeader3.Width = 22;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "State";
      this.columnHeader1.Width = 109;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Value";
      this.columnHeader2.Width = 116;
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(915, 726);
      this.Controls.Add(this.splitContainer1);
      this.Name = "Main";
      this.Text = "Boogie Verification Debugger";
      this.stateViewMenu.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView currentStateView;
    private System.Windows.Forms.ColumnHeader name;
    private System.Windows.Forms.ColumnHeader value;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.ListView stateList;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader prevValue;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.ListView matchesList;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.LinkLabel linkLabel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.ContextMenuStrip stateViewMenu;
    private System.Windows.Forms.ToolStripMenuItem dummyItemToolStripMenuItem;


  }
}

