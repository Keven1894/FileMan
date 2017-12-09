namespace FileMan
{
    partial class mainForm
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
            this.Searchbutton = new System.Windows.Forms.Button();
            this.fromDatePortionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromTimePortionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toDatePorttionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toTimePortionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromLabel = new System.Windows.Forms.Label();
            this.toLabel = new System.Windows.Forms.Label();
            this.searchConditionsPanelLabel = new System.Windows.Forms.Label();
            this.searchCueTextBox = new CueTextBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Searchbutton
            // 
            this.Searchbutton.Location = new System.Drawing.Point(23, 173);
            this.Searchbutton.Name = "Searchbutton";
            this.Searchbutton.Size = new System.Drawing.Size(75, 23);
            this.Searchbutton.TabIndex = 1;
            this.Searchbutton.Text = "Search";
            this.Searchbutton.UseVisualStyleBackColor = true;
            this.Searchbutton.Click += new System.EventHandler(this.Searchbutton_Click);
            // 
            // fromDatePortionDateTimePicker
            // 
            this.fromDatePortionDateTimePicker.Location = new System.Drawing.Point(23, 64);
            this.fromDatePortionDateTimePicker.Name = "fromDatePortionDateTimePicker";
            this.fromDatePortionDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.fromDatePortionDateTimePicker.TabIndex = 2;
            // 
            // fromTimePortionDateTimePicker
            // 
            this.fromTimePortionDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.fromTimePortionDateTimePicker.Location = new System.Drawing.Point(247, 64);
            this.fromTimePortionDateTimePicker.Name = "fromTimePortionDateTimePicker";
            this.fromTimePortionDateTimePicker.ShowUpDown = true;
            this.fromTimePortionDateTimePicker.Size = new System.Drawing.Size(101, 20);
            this.fromTimePortionDateTimePicker.TabIndex = 3;
            // 
            // toDatePorttionDateTimePicker
            // 
            this.toDatePorttionDateTimePicker.Location = new System.Drawing.Point(23, 115);
            this.toDatePorttionDateTimePicker.Name = "toDatePorttionDateTimePicker";
            this.toDatePorttionDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.toDatePorttionDateTimePicker.TabIndex = 4;
            // 
            // toTimePortionDateTimePicker
            // 
            this.toTimePortionDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.toTimePortionDateTimePicker.Location = new System.Drawing.Point(247, 115);
            this.toTimePortionDateTimePicker.Name = "toTimePortionDateTimePicker";
            this.toTimePortionDateTimePicker.ShowUpDown = true;
            this.toTimePortionDateTimePicker.Size = new System.Drawing.Size(101, 20);
            this.toTimePortionDateTimePicker.TabIndex = 5;
            // 
            // fromLabel
            // 
            this.fromLabel.AutoSize = true;
            this.fromLabel.Location = new System.Drawing.Point(23, 48);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.Size = new System.Drawing.Size(30, 13);
            this.fromLabel.TabIndex = 6;
            this.fromLabel.Text = "From";
            // 
            // toLabel
            // 
            this.toLabel.AutoSize = true;
            this.toLabel.Location = new System.Drawing.Point(23, 99);
            this.toLabel.Name = "toLabel";
            this.toLabel.Size = new System.Drawing.Size(20, 13);
            this.toLabel.TabIndex = 7;
            this.toLabel.Text = "To";
            // 
            // searchConditionsPanelLabel
            // 
            this.searchConditionsPanelLabel.AutoSize = true;
            this.searchConditionsPanelLabel.Location = new System.Drawing.Point(23, 209);
            this.searchConditionsPanelLabel.Name = "searchConditionsPanelLabel";
            this.searchConditionsPanelLabel.Size = new System.Drawing.Size(267, 13);
            this.searchConditionsPanelLabel.TabIndex = 9;
            this.searchConditionsPanelLabel.Text = "Search Conditions: (Click to Remove Search Condition)";
            // 
            // searchCueTextBox
            // 
            this.searchCueTextBox.Cue = "Search File";
            this.searchCueTextBox.Location = new System.Drawing.Point(23, 12);
            this.searchCueTextBox.Name = "searchCueTextBox";
            this.searchCueTextBox.Size = new System.Drawing.Size(211, 20);
            this.searchCueTextBox.TabIndex = 0;
            this.searchCueTextBox.TextChanged += new System.EventHandler(this.cueTextBox1_TextChanged);
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Location = new System.Drawing.Point(23, 148);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(0, 13);
            this.warningLabel.TabIndex = 10;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 523);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.searchConditionsPanelLabel);
            this.Controls.Add(this.toLabel);
            this.Controls.Add(this.fromLabel);
            this.Controls.Add(this.toTimePortionDateTimePicker);
            this.Controls.Add(this.toDatePorttionDateTimePicker);
            this.Controls.Add(this.fromTimePortionDateTimePicker);
            this.Controls.Add(this.fromDatePortionDateTimePicker);
            this.Controls.Add(this.Searchbutton);
            this.Controls.Add(this.searchCueTextBox);
            this.Name = "mainForm";
            this.Text = "File Management Tool";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CueTextBox searchCueTextBox;
        private System.Windows.Forms.Button Searchbutton;
        private System.Windows.Forms.DateTimePicker fromDatePortionDateTimePicker;
        private System.Windows.Forms.DateTimePicker fromTimePortionDateTimePicker;
        private System.Windows.Forms.DateTimePicker toDatePorttionDateTimePicker;
        private System.Windows.Forms.DateTimePicker toTimePortionDateTimePicker;
        private System.Windows.Forms.Label fromLabel;
        private System.Windows.Forms.Label toLabel;
        private System.Windows.Forms.Label searchConditionsPanelLabel;
        private System.Windows.Forms.Label warningLabel;
    }
}

