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
        public mainForm()
        {
            InitializeComponent();
            SearchConditionsPanel.BorderStyle = BorderStyle.FixedSingle;
        }

        private void cueTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Searchbutton_Click(object sender, EventArgs e)
        {
            //to remove all RichTextBoxs
            foreach (Control item in SearchConditionsPanel.Controls.OfType<RichTextBox>())
            {
                SearchConditionsPanel.Controls.Remove(item);
            }
            RichTextBox searchCueTextBoxRichTextBox = new RichTextBox
            {
                Text = "File name contains key word:" + searchCueTextBox.Text + "         X" 
            };
            searchCueTextBoxRichTextBox.Select(searchCueTextBoxRichTextBox.Text.Length, -1);
            searchCueTextBoxRichTextBox.SelectionColor = Color.Red;
            searchCueTextBoxRichTextBox.Enabled = false;
            searchCueTextBoxRichTextBox.ReadOnly = true;
            searchCueTextBoxRichTextBox.RightMargin = 0;
            //searchCueTextBoxRichTextBox.Width = 60;
            searchCueTextBoxRichTextBox.SelectAll();
            searchCueTextBoxRichTextBox.SelectionAlignment = HorizontalAlignment.Right;
            //set up the richtext height
            using (Graphics g = CreateGraphics())
            {
                searchCueTextBoxRichTextBox.Height = (int)g.MeasureString(searchCueTextBoxRichTextBox.Text,
                    searchCueTextBoxRichTextBox.Font, searchCueTextBoxRichTextBox.Width).Height + 7;
            }
            if (searchCueTextBox.Text != "")
            {
                SearchConditionsPanel.Controls.Add(searchCueTextBoxRichTextBox);
            }
        }
    }
}
