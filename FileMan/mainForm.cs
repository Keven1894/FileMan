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
using System.Collections;

namespace FileMan
{
    public partial class mainForm : Form
    {
        //FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
        //{
        //    Location = new Point(23, 230),
        //    Size = new Size(300, 300),
        //    BorderStyle = BorderStyle.FixedSingle,
        //    Name = "FlowLayoutPanel",
        //    TabIndex = 0
        //};
        public mainForm()
        {
            InitializeComponent();
            initComponentsSetup();
            //only show the mainForm search part.
            this.Size = new Size(816, 854);
            ScrollBar vScrollBar1 = new VScrollBar();
            vScrollBar1.Dock = DockStyle.Right;
            //vScrollBar1.Scroll += (sender, e) => { flowLayoutPanel.VerticalScroll.Value = vScrollBar1.Value; };
            //flowLayoutPanel.Controls.Add(vScrollBar1);
            //this.Controls.Add(flowLayoutPanel);
            //obtain the input file and check if it is the latest one
            Boolean inputFileStatus = CheckTheInputFileStatus();
        }

        private void initComponentsSetup()
        {
            //[PL0217]Add panel for menu
            Panel panelMenu = new Panel()
            {
                Location = new Point(23, 15),
                Size = new Size(140, 90),
                BorderStyle = BorderStyle.None,
                Name = "FlowLayoutPanel",
                TabIndex = 0
            };
            //[PL0217]Radio buttons for menu
            RadioButton radioButtonSearch = new RadioButton()
            {
                Text = "Search",
                Location = new Point(5, 0),
                Font = new Font("Segoe UI Semilight", 24),
                AutoSize = true
            };
            RadioButton radioButtonAddFile = new RadioButton()
            {
                Text = "Add File",
                Location = new Point(5, 45),
                Font = new Font("Segoe UI Semilight", 24),
                AutoSize = true
            };
            panelMenu.Controls.Add(radioButtonSearch);
            panelMenu.Controls.Add(radioButtonAddFile);
            panelMenu.Visible = true;
            this.Controls.Add(panelMenu);

            initSearchPageComponentsSetup();
            initAddFilePageComponentsSetup();

            //[PL0217]Setup footer
            Label labelFooter = new Label()
            {
                Text = "About the File Management System | Copyright 2017 \u00A9 FDOT Traffic Operations, District Six",
                Location = new Point(23, 790),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(labelFooter);
        }

        private Panel createPagePanel(String panelName)
        {
            Panel panel = new Panel()
            {
                Name = panelName,
                Location = new Point(23, 80),
                Size = new Size(760, 700),
                BorderStyle = BorderStyle.FixedSingle,
                TabIndex = 0
            };

            return panel;
        }

        private void initSearchPageComponentsSetup()
        {
            //[PL0217]Create a big panel for all "Search page" components.
            Panel panelSearchPage = createPagePanel("panelSearchPage");

            //[PL0217]Add button for advanced search.
            Button buttonAdvanced = new Button()
            {
                Text = "Advanced",
                Font = new Font("Segoe UI", 12),
                Location = new Point(100, 25),
                Size = new Size(93, 40),
                AutoSize = true
            };
            panelSearchPage.Controls.Add(buttonAdvanced);

            //[PL0217]Add cueText search.
            CueTextBox searchCueTextBox = new CueTextBox()
            {
                Name = "searchCueTextBox",
                Cue = "Search",
                Font = new Font("Segoe UI", 15),
                Location = new Point(207, 28),
                Size = new Size(435, 30)
            };
            panelSearchPage.Controls.Add(searchCueTextBox);

            //[PL0217]Add warning label to avoid empty search, that will return all files, performance low.
            Label warningLabel = new Label()
            {
                Name = "warningLabel",
                Font = new Font("Segoe UI", 12),
                Location = new Point(205, 60),
                AutoSize = true
            };
            warningLabel.Visible = false;
            panelSearchPage.Controls.Add(warningLabel);

            //[PL0217]Add zoom search button.
            Button Searchbutton = new Button()
            {
                Name = "Searchbutton",
                BackgroundImage = Image.FromFile(@".\assets\img\zoom.png"),
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(610, 30),
                Size = new Size(28, 30)
            };
            Searchbutton.Click += new EventHandler(Searchbutton_Click);            
            panelSearchPage.Controls.Add(Searchbutton);
            Searchbutton.BringToFront();

            //[PL0217]Add lable for past time condition.
            Label labelChangesInPast = new Label()
            {
                Text = "Changes in the past:",
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                Location = new Point(100, 85),
                AutoSize = true

            };
            panelSearchPage.Controls.Add(labelChangesInPast);

            //[PL0217]Add comboBox for past time condition
            ComboBox comboBoxChangesInPast = new ComboBox()
            {                
                Location = new Point(325, 85),
                Font = new Font("Segoe UI", 15)
            };
            comboBoxChangesInPast.Items.Add("24 Hours");
            comboBoxChangesInPast.Items.Add("2 Days");
            comboBoxChangesInPast.Items.Add("3 Days");
            comboBoxChangesInPast.SelectedIndex = 0;
            panelSearchPage.Controls.Add(comboBoxChangesInPast);

            //[PL0217]Add text info for number of files and folders changed.
            //Button updatedFilesNumberButton = new Button()
            //{
            //    Name = "updatedFilesNumberButton",
            //    Location = new Point(100, 150),
            //    Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold)
            //};
            //setButtonOutlookAsLabel(updatedFilesNumberButton);
            //panelSearchPage.Controls.Add(updatedFilesNumberButton);
            Label recentChangeStatisticLabel = new Label()
            {
                Name = "recentChangeStatisticLabel",
                Location = new Point(100, 150),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = " "
            };
            panelSearchPage.Controls.Add(recentChangeStatisticLabel);
            //Button updatedFolderNumberButton = new Button()
            //{
            //    Name = "updatedFolderNumberButton",
            //    Location = new Point(206, 150),
            //    Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold)
            //};
            //setButtonOutlookAsLabel(updatedFolderNumberButton);
            //panelSearchPage.Controls.Add(updatedFolderNumberButton);

            Label recentSearchLabel = new Label()
            {
                Name = "recentSearchLabel",
                Location = new Point(100, 190),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = "Recent search term: "
            };
            panelSearchPage.Controls.Add(recentSearchLabel);

            Label recentFilesLabel = new Label()
            {
                Name = "recentFilesLabel",
                Location = new Point(100, 230),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = "Recent Files..."
            };
            panelSearchPage.Controls.Add(recentFilesLabel);

            //[PL0217]Add panel for advanced search
            Panel panelAdvancedSearch = new Panel()
            {
                Location = new Point(23, 230),
                Size = new Size(300, 300),
                BorderStyle = BorderStyle.FixedSingle,
                Name = "panelAdvancedSearch",
                TabIndex = 0
            };
            panelAdvancedSearch.Visible = false;
            panelSearchPage.Controls.Add(panelAdvancedSearch);
            this.Controls.Add(panelSearchPage);





        }

        private void initAddFilePageComponentsSetup()
        {
            //[PL0217]Create a big panel for all "Add File" components.
            Panel panelSearchPage = createPagePanel("panelSearchPage");


        }

        private void setButtonOutlookAsLabel(Button button)
        {
            button.TabStop = false;
            button.FlatAppearance.BorderSize = 0;
            button.AutoSize = true;
            button.BackColor = Color.Transparent;
        }

        private Boolean CheckTheInputFileStatus()
        {
            string csv = File.ReadAllText("./inputFile/FileInfoSummary.csv");
            XDocument doc = ConverstorCsvToXml.ConvertCsvToXML(csv, new[] { "\",\"" });
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

        int numberOfSearchTerm = 0;
        String keywordFirst = "";
        String keywordSecond = "";
        String keywordThird = "";
        private void Searchbutton_Click(object sender, EventArgs e)
        {
            //[PL0217]Refer to the dynamic created component
            CueTextBox searchCueTextBox = (CueTextBox)Controls["panelSearchPage"].Controls["searchCueTextBox"];
            Label warningLabel = (Label)Controls["panelSearchPage"].Controls["warningLabel"];
            warningLabel.Visible = false;
            //check the search key word
            if (searchCueTextBox.Text != "")
            {
                warningLabel.Text = "";
                //get from datetime
                DateTime fromDateTime = GetDateTimeConsolidation(fromDatePortionDateTimePicker, fromTimePortionDateTimePicker);
                
                //get to datetime
                DateTime toDateTime = GetDateTimeConsolidation(toDatePorttionDateTimePicker, toTimePortionDateTimePicker);

                //query the xml input file based on the search conditions
                List<DOTFile> dOTQueriedFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", searchCueTextBox.Text, fromDateTime, toDateTime);
                var distinctFolderCount = dOTQueriedFiles.Select(x => x.ParentFolder).Distinct().Count();
                String recentChangeStatistic = dOTQueriedFiles.Count.ToString() + " Files, " + distinctFolderCount.ToString() + " Folders changed.";
                Label recentChangeStatisticLabel = (Label)Controls["panelSearchPage"].Controls["recentChangeStatisticLabel"];
                recentChangeStatisticLabel.Text = recentChangeStatistic;

                //fill out the query results table
                fillDataGridView(dOTQueriedFiles);

                //[PL0217]Fill the recent search label.
                
                Label recentSearchLabel = (Label)Controls["panelSearchPage"].Controls["recentSearchLabel"];

                switch (numberOfSearchTerm % 3)
                {
                    case 0:
                        keywordFirst = searchCueTextBox.Text;
                        break;
                    case 1:
                        keywordSecond = searchCueTextBox.Text;
                        break;
                    case 2:
                        keywordThird = searchCueTextBox.Text;
                        break;
                    default:
                        break;
                };
                if (keywordFirst != "" && keywordSecond != "" && keywordThird != "")
                {
                    recentSearchLabel.Text = "Recent search term: ";
                    recentSearchLabel.Text = recentSearchLabel.Text + "\"" + keywordFirst + "\", \"" + keywordSecond + "\", \"" + keywordThird + "\".";
                }
                else if (keywordFirst != "" && keywordSecond != "")
                {
                    recentSearchLabel.Text = "Recent search term: ";
                    recentSearchLabel.Text = recentSearchLabel.Text + "\"" + keywordFirst + "\", \"" + keywordSecond + "\".";
                }
                else if (keywordFirst != "") {
                    recentSearchLabel.Text = "Recent search term: ";
                    recentSearchLabel.Text = recentSearchLabel.Text + "\"" + keywordFirst + "\".";
                }
                
                numberOfSearchTerm++;

                //ArrayList keywordArray = new ArrayList();
                //keywordArray.Add(searchCueTextBox.Text);

                //foreach (String singleKeyword in keywordArray)
                //{
                //    recentSearchLabel.Text += singleKeyword;
                //}

            }
            else
            {
                warningLabel.Text = "Please specify the keyword to search.";
                warningLabel.Visible = true;
                warningLabel.ForeColor = Color.Red;
            }
            searchCueTextBox.Text = "";
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

        private void fillDataGridView(List<DOTFile> dOTQueriedFiles)
        {
            var fileNameAndPath = dOTQueriedFiles.Select(i => new { i.Name, i.ParentFolder }).ToArray();
            dataGridView1.DataSource = fileNameAndPath;
            //dataGridView1.DataBind();


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

        private void searchResultGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void descriptionLabel3_Click(object sender, EventArgs e)
        {

        }
    }
}
