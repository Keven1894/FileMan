using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileMan
{
    public partial class mainForm : Form
    {
        FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
        {
            Location = new Point(23, 230),
            Size = new Size(330, 280),
            BorderStyle = BorderStyle.FixedSingle,
            Name = "FlowLayoutPanel",
            TabIndex = 0
        };
        public mainForm()
        {
            InitializeComponent();
            ScrollBar vScrollBar1 = new VScrollBar();
            vScrollBar1.Dock = DockStyle.Right;
            vScrollBar1.Scroll += (sender, e) => { flowLayoutPanel.VerticalScroll.Value = vScrollBar1.Value; };
            flowLayoutPanel.Controls.Add(vScrollBar1);
            this.Controls.Add(flowLayoutPanel);
        }

        private void cueTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private DateTime GetDateTimeConsolidation(DateTimePicker datePicker, DateTimePicker timePicker)
        {
            DateTime consolidatedDateTime = datePicker.Value.Date + timePicker.Value.TimeOfDay;
            return consolidatedDateTime;
        }

        private void SetSearchCondtionRichTextBoxFormat(RichTextBox searchConditionRichTextBox)
        {
            searchConditionRichTextBox.Select(searchConditionRichTextBox.Text.Length, -1);
            searchConditionRichTextBox.SelectionColor = Color.Red;
            searchConditionRichTextBox.Enabled = false;
            searchConditionRichTextBox.ReadOnly = true;
            searchConditionRichTextBox.RightMargin = 0;
            searchConditionRichTextBox.SelectAll();
            searchConditionRichTextBox.SelectionAlignment = HorizontalAlignment.Center;
            //set up the richtext height and width
            using (Graphics g = CreateGraphics())
            {
                searchConditionRichTextBox.Height = (int)g.MeasureString(searchConditionRichTextBox.Text,
                    searchConditionRichTextBox.Font, searchConditionRichTextBox.Width).Height + 7;
                searchConditionRichTextBox.Width = (int)g.MeasureString(searchConditionRichTextBox.Text,
                    searchConditionRichTextBox.Font, searchConditionRichTextBox.Width).Width;
            }
        }

        private void Searchbutton_Click(object sender, EventArgs e)
        {
            //to remove all RichTextBoxs
            List<Control> listControls = flowLayoutPanel.Controls.Cast<Control>().ToList();

            foreach (Control control in listControls)
            {
                flowLayoutPanel.Controls.Remove(control);
                control.Dispose();
            }
            RichTextBox searchCueTextBoxRichTextBox = new RichTextBox
            {
                Text = "File name contains key word:" + searchCueTextBox.Text + "         X"
            };
            SetSearchCondtionRichTextBoxFormat(searchCueTextBoxRichTextBox);


            if (searchCueTextBox.Text != "")
            {
                warningLabel.Text = "";
                flowLayoutPanel.Controls.Add(searchCueTextBoxRichTextBox);
            }
            else
            {
                warningLabel.Text = "File name filter condition is empty.";
                warningLabel.ForeColor = Color.Red;
            }
            //get from datetime
            DateTime fromDateTime = GetDateTimeConsolidation(fromDatePortionDateTimePicker, fromTimePortionDateTimePicker);
            RichTextBox searchFromDateTime = new RichTextBox
            {
                Text = "File modified from:" + fromDateTime.ToString() + "         X"
            };
            SetSearchCondtionRichTextBoxFormat(searchFromDateTime);
            flowLayoutPanel.Controls.Add(searchFromDateTime);
            //get to datetime
            DateTime toDateTime = GetDateTimeConsolidation(toDatePorttionDateTimePicker, toTimePortionDateTimePicker);
            RichTextBox searchToDateTime = new RichTextBox
            {
                Text = "File modified to:" + toDateTime.ToString() + "         X"
            };
            SetSearchCondtionRichTextBoxFormat(searchToDateTime);
            flowLayoutPanel.Controls.Add(searchToDateTime);
        }

        private void mainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
