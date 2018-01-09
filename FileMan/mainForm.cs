using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Drawing.Drawing2D;

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
            //only show the mainForm search part.
            this.Size = new Size(392, 562);
            ScrollBar vScrollBar1 = new VScrollBar();
            vScrollBar1.Dock = DockStyle.Right;
            vScrollBar1.Scroll += (sender, e) => { flowLayoutPanel.VerticalScroll.Value = vScrollBar1.Value; };
            flowLayoutPanel.Controls.Add(vScrollBar1);
            this.Controls.Add(flowLayoutPanel);
            //obtain the input file and check if it is the latest one
            Boolean inputFileStatus = CheckTheInputFileStatus();

            setButtonOutlookAsLabel(updatedFilesNumberButton);
            setButtonOutlookAsLabel(updatedFolderNumberButton);
        }

        private void setButtonOutlookAsLabel(Button button)
        {
            button.TabStop = false;
            button.FlatAppearance.BorderSize = 0;
            button.AutoSize = true;
        }

        private Boolean CheckTheInputFileStatus()
        {
            string csv = File.ReadAllText("./inputFile/FileInfoSummary.csv");
            XDocument doc = ConverstorCsvToXml.ConvertCsvToXML(csv, new[] { "|" });
            doc.Save("./inputFile/outputxml.xml");


            return true;
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

            //show the mainForm search part and result part.
            this.Size = new Size(1220, 562);

            //to clear search result tablelayoutPanel
            searchResultTableLayoutPanel.Controls.Clear();
            searchResultTableLayoutPanel.RowStyles.Clear();

            //to set the autoscroll for tablelayoutPanel, need to set autoscroll as false firstly, otherwise table cannot shrink
            searchResultTableLayoutPanel.AutoScroll = false;
            searchResultTableLayoutPanel.Size = new Size(723, 317);
            searchResultTableLayoutPanel.MaximumSize = new Size(723, 317);
            searchResultTableLayoutPanel.AutoScroll = true;

            //check the search key word
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

            //query the xml input file based on the search conditions
            List<DOTFile> dOTQueriedFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", searchCueTextBox.Text, fromDateTime, toDateTime);

            //fill out the button of how many files updated for the search conditions
            updatedFilesNumberButton.Text = dOTQueriedFiles.Count.ToString();

            //change the size of the button of how many files according to the count
            updatedFilesNumberButton.Width = updatedFilesNumberButton.Text.Length;

            //fill out the button of how many folders updated for the search conditions
            var distinctFolderCount = dOTQueriedFiles.Select(x => x.ParentFolder).Distinct().Count();
            updatedFolderNumberButton.Text = distinctFolderCount.ToString();

            //change the size of the button of how many folders according to the count
            updatedFolderNumberButton.Width = updatedFolderNumberButton.Text.Length;

            //set up the following content positions
            DescriptionLabel2.Location = new Point(updatedFilesNumberButton.Location.X + updatedFilesNumberButton.Width,
                                                   DescriptionLabel2.Location.Y);
            updatedFolderNumberButton.Location = new Point(DescriptionLabel2.Location.X + DescriptionLabel2.Width,
                                                           updatedFolderNumberButton.Location.Y);
            descriptionLabel3.Location = new Point(updatedFolderNumberButton.Location.X + updatedFolderNumberButton.Width,
                                                   descriptionLabel3.Location.Y);



            //fill out the query results table
            fillTableLayoutPanel(dOTQueriedFiles);
        }

        private List<DOTFile> queryInfoFromXMLInputFile(String XMLInputFileName, String fileNameKeyWord, DateTime fromDateTime, DateTime toDateTime)
        {
            List<DOTFile> dOTFiles = new List<DOTFile>();
            XElement root = XElement.Load(XMLInputFileName);
            var selected = from cli in root.Elements("row").Elements("var")
                           where (string)cli.Attribute("name").Value == "Name" && cli.Attribute("value").Value.Contains(fileNameKeyWord) == true
                           select cli.Parent;

            foreach (var d in selected)
            {
                DOTFile dOTFile = queryInfoForOneFile(d);
                dOTFiles.Add(dOTFile);
            }
            return dOTFiles;
        }

        private DOTFile queryInfoForOneFile(dynamic oneFileXMLContent)
        {
            DOTFile dOTFile = new DOTFile();
            XElement fileAttributes = XElement.Parse(oneFileXMLContent.ToString());
            dOTFile.FilePathAndName = queryInfoForFileAttribute(fileAttributes, "FilePathAndName");
            dOTFile.ParentFolder = queryInfoForFileAttribute(fileAttributes, "ParentFolder");
            dOTFile.Name = queryInfoForFileAttribute(fileAttributes, "Name");
            dOTFile.DateCreated = queryInfoForFileAttribute(fileAttributes, "DateCreated");
            dOTFile.DateLastAccessed = queryInfoForFileAttribute(fileAttributes, "DateLastAccessed");
            dOTFile.DateLastModified = queryInfoForFileAttribute(fileAttributes, "DateLastModified");
            dOTFile.Size = queryInfoForFileAttribute(fileAttributes, "Size");
            dOTFile.Type = queryInfoForFileAttribute(fileAttributes, "Type");
            dOTFile.Suffix = queryInfoForFileAttribute(fileAttributes, "Suffix");
            dOTFile.Owner = queryInfoForFileAttribute(fileAttributes, "Owner");
            return dOTFile;
        }

        private String queryInfoForFileAttribute(XElement fileAttributes, String fileAttribute)
        {
            var fileAttributeContent = from c in fileAttributes.Elements("var")
                                       where c.Attribute("name").Value == fileAttribute
                                       select c.Attribute("value").Value;
            foreach (var theFileAttributeContent in fileAttributeContent)
            { 
                return theFileAttributeContent.ToString();
            }
            return "";
        }

        private void fillTableLayoutPanel(List<DOTFile> dOTQueriedFiles)
        {
            searchResultTableLayoutPanel.ColumnCount = 2; 
            searchResultTableLayoutPanel.RowCount = 1;
            searchResultTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            searchResultTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            searchResultTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            searchResultTableLayoutPanel.Controls.Add(new Label() { Text = "Modified File's Name", AutoSize = true }, 0, 0);
            searchResultTableLayoutPanel.Controls.Add(new Label() { Text = "The Located Folder", AutoSize = true }, 1, 0);
            foreach (var dOTQueriedFile in dOTQueriedFiles)
            {
                searchResultTableLayoutPanel.RowCount = searchResultTableLayoutPanel.RowCount + 1;
                searchResultTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                //MessageBox.Show(dOTQueriedFile.Name);
                //AutoSize property has to be true, otherwise the content got cut as unexpected.
                searchResultTableLayoutPanel.Controls.Add(new Label() { Text = dOTQueriedFile.Name, AutoSize = true }, 0, searchResultTableLayoutPanel.RowCount - 1);
                searchResultTableLayoutPanel.Controls.Add(new Label() { Text = dOTQueriedFile.ParentFolder, AutoSize = true }, 1, searchResultTableLayoutPanel.RowCount - 1);
            }
        }

        //below function is used to change the tableLayoutPanel border color.
        private void searchResultTableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
        //    Graphics g = e.Graphics;
        //    Rectangle r = e.CellBounds;

        //    using (Pen pen = new Pen(Color.CornflowerBlue, 0 /*1px width despite of page scale, dpi, page units*/ ))
        //    {
        //        pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
        //        // define border style
        //        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

        //        // decrease border rectangle height/width by pen's width for last row/column cell
        //        if (e.Row == (searchResultTableLayoutPanel.RowCount - 1))
        //        {
        //            r.Height -= 1;
        //        }

        //        if (e.Column == (searchResultTableLayoutPanel.ColumnCount - 1))
        //        {
        //            r.Width -= 1;
        //        }

        //        // use graphics mehtods to draw cell's border
        //        e.Graphics.DrawRectangle(pen, r);
        //    }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {

        }

        private void DescriptionLabel2_Click(object sender, EventArgs e)
        {

        }

        private void mainForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void searchResultGroupBox_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
            e.Graphics.DrawLine(pen, 17, 60, 718, 60);
        }

        private void searchResultTableLayoutPanel_MouseClick(object sender, MouseEventArgs e)
        {
            //the code is not working when having scrolls
            //int row = 0;
            //int verticalOffset = 0;
            //foreach (int h in searchResultTableLayoutPanel.GetRowHeights())
            //{
            //    int column = 0;
            //    int horizontalOffset = 0;
            //    foreach (int w in searchResultTableLayoutPanel.GetColumnWidths())
            //    {
            //        Rectangle rectangle = new Rectangle(horizontalOffset, verticalOffset, w, h);
            //        if (rectangle.Contains(e.Location))
            //        {
            //            MessageBox.Show(String.Format("row {0}, column {1} was clicked", row, column));
            //            return;
            //        }
            //        horizontalOffset += w;
            //        column++;
            //    }
            //    verticalOffset += h;
            //    row++;
            //}
        }
    }
}
