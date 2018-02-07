namespace TestApplication
{
    partial class TestForm
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
            System.Windows.Forms.Label lblFormHwndCaption;
            System.Windows.Forms.Label lblTextBoxHwndCaption;
            System.Windows.Forms.Label lblGroupBoxHwndCaption;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.textBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbFormHwnd = new System.Windows.Forms.TextBox();
            this.tbTextBoxHwnd = new System.Windows.Forms.TextBox();
            this.tbGroupBoxHwnd = new System.Windows.Forms.TextBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbButton1 = new System.Windows.Forms.ToolStripButton();
            this.tsbButton2 = new System.Windows.Forms.ToolStripButton();
            this.tssButton = new System.Windows.Forms.ToolStripSplitButton();
            this.item1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.item2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.item21ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.item22ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            lblFormHwndCaption = new System.Windows.Forms.Label();
            lblTextBoxHwndCaption = new System.Windows.Forms.Label();
            lblGroupBoxHwndCaption = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFormHwndCaption
            // 
            lblFormHwndCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            lblFormHwndCaption.AutoSize = true;
            lblFormHwndCaption.Location = new System.Drawing.Point(3, 1);
            lblFormHwndCaption.Name = "lblFormHwndCaption";
            lblFormHwndCaption.Padding = new System.Windows.Forms.Padding(5);
            lblFormHwndCaption.Size = new System.Drawing.Size(116, 23);
            lblFormHwndCaption.TabIndex = 0;
            lblFormHwndCaption.Text = "Form HWND";
            lblFormHwndCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTextBoxHwndCaption
            // 
            lblTextBoxHwndCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            lblTextBoxHwndCaption.AutoSize = true;
            lblTextBoxHwndCaption.Location = new System.Drawing.Point(3, 27);
            lblTextBoxHwndCaption.Name = "lblTextBoxHwndCaption";
            lblTextBoxHwndCaption.Padding = new System.Windows.Forms.Padding(5);
            lblTextBoxHwndCaption.Size = new System.Drawing.Size(116, 23);
            lblTextBoxHwndCaption.TabIndex = 1;
            lblTextBoxHwndCaption.Text = "TextBox HWND";
            lblTextBoxHwndCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGroupBoxHwndCaption
            // 
            lblGroupBoxHwndCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            lblGroupBoxHwndCaption.AutoSize = true;
            lblGroupBoxHwndCaption.Location = new System.Drawing.Point(3, 53);
            lblGroupBoxHwndCaption.Name = "lblGroupBoxHwndCaption";
            lblGroupBoxHwndCaption.Padding = new System.Windows.Forms.Padding(5);
            lblGroupBoxHwndCaption.Size = new System.Drawing.Size(116, 23);
            lblGroupBoxHwndCaption.TabIndex = 4;
            lblGroupBoxHwndCaption.Text = "GroupBox HWND";
            lblGroupBoxHwndCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(19, 19);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(238, 20);
            this.textBox.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(lblFormHwndCaption, 0, 0);
            this.tableLayoutPanel1.Controls.Add(lblTextBoxHwndCaption, 0, 1);
            this.tableLayoutPanel1.Controls.Add(lblGroupBoxHwndCaption, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbFormHwnd, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbTextBoxHwnd, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbGroupBoxHwnd, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(31, 179);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(408, 109);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tbFormHwnd
            // 
            this.tbFormHwnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFormHwnd.Location = new System.Drawing.Point(125, 3);
            this.tbFormHwnd.Name = "tbFormHwnd";
            this.tbFormHwnd.ReadOnly = true;
            this.tbFormHwnd.Size = new System.Drawing.Size(280, 20);
            this.tbFormHwnd.TabIndex = 5;
            // 
            // tbTextBoxHwnd
            // 
            this.tbTextBoxHwnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTextBoxHwnd.Location = new System.Drawing.Point(125, 29);
            this.tbTextBoxHwnd.Name = "tbTextBoxHwnd";
            this.tbTextBoxHwnd.ReadOnly = true;
            this.tbTextBoxHwnd.Size = new System.Drawing.Size(280, 20);
            this.tbTextBoxHwnd.TabIndex = 6;
            // 
            // tbGroupBoxHwnd
            // 
            this.tbGroupBoxHwnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGroupBoxHwnd.Location = new System.Drawing.Point(125, 55);
            this.tbGroupBoxHwnd.Name = "tbGroupBoxHwnd";
            this.tbGroupBoxHwnd.ReadOnly = true;
            this.tbGroupBoxHwnd.Size = new System.Drawing.Size(280, 20);
            this.tbGroupBoxHwnd.TabIndex = 7;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.textBox);
            this.groupBox.Location = new System.Drawing.Point(31, 59);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(370, 100);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "groupBox";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbButton1,
            this.tsbButton2,
            this.tssButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(466, 25);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbButton1
            // 
            this.tsbButton1.Image = ((System.Drawing.Image)(resources.GetObject("tsbButton1.Image")));
            this.tsbButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbButton1.Name = "tsbButton1";
            this.tsbButton1.Size = new System.Drawing.Size(72, 22);
            this.tsbButton1.Text = "Button 1";
            // 
            // tsbButton2
            // 
            this.tsbButton2.Image = ((System.Drawing.Image)(resources.GetObject("tsbButton2.Image")));
            this.tsbButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbButton2.Name = "tsbButton2";
            this.tsbButton2.Size = new System.Drawing.Size(79, 22);
            this.tsbButton2.Text = "Buttoon 2";
            // 
            // tssButton
            // 
            this.tssButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tssButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.item1ToolStripMenuItem,
            this.item2ToolStripMenuItem,
            this.toolStripTextBox1,
            this.toolStripComboBox1});
            this.tssButton.Image = ((System.Drawing.Image)(resources.GetObject("tssButton.Image")));
            this.tssButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssButton.Name = "tssButton";
            this.tssButton.Size = new System.Drawing.Size(32, 22);
            this.tssButton.Text = "Split button";
            // 
            // item1ToolStripMenuItem
            // 
            this.item1ToolStripMenuItem.Name = "item1ToolStripMenuItem";
            this.item1ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.item1ToolStripMenuItem.Text = "Item 1";
            // 
            // item2ToolStripMenuItem
            // 
            this.item2ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.item21ToolStripMenuItem,
            this.item22ToolStripMenuItem});
            this.item2ToolStripMenuItem.Name = "item2ToolStripMenuItem";
            this.item2ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.item2ToolStripMenuItem.Text = "Item 2";
            // 
            // item21ToolStripMenuItem
            // 
            this.item21ToolStripMenuItem.Name = "item21ToolStripMenuItem";
            this.item21ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.item21ToolStripMenuItem.Text = "Item 2.1";
            // 
            // item22ToolStripMenuItem
            // 
            this.item22ToolStripMenuItem.Name = "item22ToolStripMenuItem";
            this.item22ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.item22ToolStripMenuItem.Text = "Item 2.2";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.Text = "Test";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 23);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 317);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Location = new System.Drawing.Point(500, 500);
            this.Name = "TestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Test form";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.TextBox tbFormHwnd;
        private System.Windows.Forms.TextBox tbTextBoxHwnd;
        private System.Windows.Forms.TextBox tbGroupBoxHwnd;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbButton1;
        private System.Windows.Forms.ToolStripButton tsbButton2;
        private System.Windows.Forms.ToolStripSplitButton tssButton;
        private System.Windows.Forms.ToolStripMenuItem item1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem item2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem item21ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem item22ToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
    }
}

