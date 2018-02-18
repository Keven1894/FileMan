
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.fromDatePortionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromTimePortionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toDatePorttionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toTimePortionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromLabel = new System.Windows.Forms.Label();
            this.toLabel = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // fromDatePortionDateTimePicker
            // 
            this.fromDatePortionDateTimePicker.Location = new System.Drawing.Point(633, 166);
            this.fromDatePortionDateTimePicker.Name = "fromDatePortionDateTimePicker";
            this.fromDatePortionDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.fromDatePortionDateTimePicker.TabIndex = 2;
            // 
            // fromTimePortionDateTimePicker
            // 
            this.fromTimePortionDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.fromTimePortionDateTimePicker.Location = new System.Drawing.Point(857, 142);
            this.fromTimePortionDateTimePicker.Name = "fromTimePortionDateTimePicker";
            this.fromTimePortionDateTimePicker.ShowUpDown = true;
            this.fromTimePortionDateTimePicker.Size = new System.Drawing.Size(101, 20);
            this.fromTimePortionDateTimePicker.TabIndex = 3;
            // 
            // toDatePorttionDateTimePicker
            // 
            this.toDatePorttionDateTimePicker.Location = new System.Drawing.Point(633, 217);
            this.toDatePorttionDateTimePicker.Name = "toDatePorttionDateTimePicker";
            this.toDatePorttionDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.toDatePorttionDateTimePicker.TabIndex = 4;
            // 
            // toTimePortionDateTimePicker
            // 
            this.toTimePortionDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.toTimePortionDateTimePicker.Location = new System.Drawing.Point(857, 193);
            this.toTimePortionDateTimePicker.Name = "toTimePortionDateTimePicker";
            this.toTimePortionDateTimePicker.ShowUpDown = true;
            this.toTimePortionDateTimePicker.Size = new System.Drawing.Size(101, 20);
            this.toTimePortionDateTimePicker.TabIndex = 5;
            // 
            // fromLabel
            // 
            this.fromLabel.AutoSize = true;
            this.fromLabel.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromLabel.Location = new System.Drawing.Point(633, 150);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.Size = new System.Drawing.Size(31, 13);
            this.fromLabel.TabIndex = 6;
            this.fromLabel.Text = "From";
            // 
            // toLabel
            // 
            this.toLabel.AutoSize = true;
            this.toLabel.Location = new System.Drawing.Point(633, 201);
            this.toLabel.Name = "toLabel";
            this.toLabel.Size = new System.Drawing.Size(20, 13);
            this.toLabel.TabIndex = 7;
            this.toLabel.Text = "To";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(41, 362);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(728, 397);
            this.dataGridView1.TabIndex = 14;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 815);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toLabel);
            this.Controls.Add(this.fromLabel);
            this.Controls.Add(this.toTimePortionDateTimePicker);
            this.Controls.Add(this.toDatePorttionDateTimePicker);
            this.Controls.Add(this.fromTimePortionDateTimePicker);
            this.Controls.Add(this.fromDatePortionDateTimePicker);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "mainForm";
            this.Text = "File Management Tool";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.mainForm_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker fromDatePortionDateTimePicker;
        private System.Windows.Forms.DateTimePicker fromTimePortionDateTimePicker;
        private System.Windows.Forms.DateTimePicker toDatePorttionDateTimePicker;
        private System.Windows.Forms.DateTimePicker toTimePortionDateTimePicker;
        private System.Windows.Forms.Label fromLabel;
        private System.Windows.Forms.Label toLabel;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

