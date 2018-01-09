
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Searchbutton = new System.Windows.Forms.Button();
            this.fromDatePortionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromTimePortionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toDatePorttionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toTimePortionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromLabel = new System.Windows.Forms.Label();
            this.toLabel = new System.Windows.Forms.Label();
            this.searchConditionsPanelLabel = new System.Windows.Forms.Label();
            this.warningLabel = new System.Windows.Forms.Label();
            this.searchResultGroupBox = new System.Windows.Forms.GroupBox();
            this.descriptionLabel3 = new System.Windows.Forms.Label();
            this.updatedFolderNumberButton = new System.Windows.Forms.Button();
            this.DescriptionLabel2 = new System.Windows.Forms.Label();
            this.descriptionLabel1 = new System.Windows.Forms.Label();
            this.updatedFilesNumberButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.searchCueTextBox = new CueTextBox();
            this.searchResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Location = new System.Drawing.Point(23, 148);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(0, 13);
            this.warningLabel.TabIndex = 10;
            // 
            // searchResultGroupBox
            // 
            this.searchResultGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.searchResultGroupBox.Controls.Add(this.descriptionLabel3);
            this.searchResultGroupBox.Controls.Add(this.updatedFolderNumberButton);
            this.searchResultGroupBox.Controls.Add(this.DescriptionLabel2);
            this.searchResultGroupBox.Controls.Add(this.descriptionLabel1);
            this.searchResultGroupBox.Controls.Add(this.updatedFilesNumberButton);
            this.searchResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchResultGroupBox.Location = new System.Drawing.Point(420, 12);
            this.searchResultGroupBox.Name = "searchResultGroupBox";
            this.searchResultGroupBox.Size = new System.Drawing.Size(735, 499);
            this.searchResultGroupBox.TabIndex = 11;
            this.searchResultGroupBox.TabStop = false;
            this.searchResultGroupBox.Text = "Search Result";
            this.searchResultGroupBox.Paint += new System.Windows.Forms.PaintEventHandler(this.searchResultGroupBox_Paint);
            // 
            // descriptionLabel3
            // 
            this.descriptionLabel3.AutoSize = true;
            this.descriptionLabel3.Location = new System.Drawing.Point(263, 35);
            this.descriptionLabel3.Name = "descriptionLabel3";
            this.descriptionLabel3.Size = new System.Drawing.Size(252, 13);
            this.descriptionLabel3.TabIndex = 13;
            this.descriptionLabel3.Text = " folder(s) changed based on the search condition(s).";
            // 
            // updatedFolderNumberButton
            // 
            this.updatedFolderNumberButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updatedFolderNumberButton.Location = new System.Drawing.Point(182, 30);
            this.updatedFolderNumberButton.Name = "updatedFolderNumberButton";
            this.updatedFolderNumberButton.Size = new System.Drawing.Size(75, 23);
            this.updatedFolderNumberButton.TabIndex = 3;
            this.updatedFolderNumberButton.Text = "updatedFolderNumberButton";
            this.updatedFolderNumberButton.UseVisualStyleBackColor = true;
            // 
            // DescriptionLabel2
            // 
            this.DescriptionLabel2.AutoSize = true;
            this.DescriptionLabel2.Location = new System.Drawing.Point(154, 36);
            this.DescriptionLabel2.Name = "DescriptionLabel2";
            this.DescriptionLabel2.Size = new System.Drawing.Size(40, 13);
            this.DescriptionLabel2.TabIndex = 2;
            this.DescriptionLabel2.Text = " file(s), ";
            this.DescriptionLabel2.Click += new System.EventHandler(this.DescriptionLabel2_Click);
            // 
            // descriptionLabel1
            // 
            this.descriptionLabel1.AutoSize = true;
            this.descriptionLabel1.Location = new System.Drawing.Point(17, 35);
            this.descriptionLabel1.Name = "descriptionLabel1";
            this.descriptionLabel1.Size = new System.Drawing.Size(53, 13);
            this.descriptionLabel1.TabIndex = 1;
            this.descriptionLabel1.Text = "There are";
            // 
            // updatedFilesNumberButton
            // 
            this.updatedFilesNumberButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updatedFilesNumberButton.Location = new System.Drawing.Point(70, 30);
            this.updatedFilesNumberButton.Name = "updatedFilesNumberButton";
            this.updatedFilesNumberButton.Size = new System.Drawing.Size(75, 23);
            this.updatedFilesNumberButton.TabIndex = 0;
            this.updatedFilesNumberButton.Text = "updatedFilesNumberButton";
            this.updatedFilesNumberButton.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(440, 173);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(694, 332);
            this.dataGridView1.TabIndex = 14;
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
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 523);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.searchResultGroupBox);
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "mainForm";
            this.Text = "File Management Tool";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.mainForm_Paint);
            this.searchResultGroupBox.ResumeLayout(false);
            this.searchResultGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.GroupBox searchResultGroupBox;
        private System.Windows.Forms.Button updatedFilesNumberButton;
        private System.Windows.Forms.Label descriptionLabel1;
        private System.Windows.Forms.Button updatedFolderNumberButton;
        private System.Windows.Forms.Label DescriptionLabel2;
        private System.Windows.Forms.Label descriptionLabel3;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

