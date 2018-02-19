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
        //[PL0218]Used for selected file.
        string sSelectedFile;
        string newFileName = "SectionNo_SRxx_StudyType_Location_MPxx_xx.suffix";
        public mainForm()
        {
            InitializeComponent();
            initComponentsSetup();
            //this.Size = new Size(816, 854);
            this.Size = new Size(816, 954);
            ScrollBar vScrollBar1 = new VScrollBar();
            vScrollBar1.Dock = DockStyle.Right;
            //vScrollBar1.Scroll += (sender, e) => { flowLayoutPanel.VerticalScroll.Value = vScrollBar1.Value; };
            //flowLayoutPanel.Controls.Add(vScrollBar1);
            //this.Controls.Add(flowLayoutPanel);
            //obtain the input file and check if it is the latest one
            Boolean inputFileStatus = CheckTheInputFileStatus();
            showDynamicInfo();
        }

        private void initComponentsSetup()
        {
            //[PL0217]Add panel for menu
            Panel panelMenu = new Panel()
            {
                Location = new Point(23, 15),
                Size = new Size(300, 45),
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
            radioButtonSearch.Checked = true;
            radioButtonSearch.CheckedChanged += RadioButtonSearch_CheckedChanged;
            RadioButton radioButtonAddFile = new RadioButton()
            {
                Text = "Add File",
                Location = new Point(150, 0),
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
                Location = new Point(23, 890),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(labelFooter);
        }

        private void RadioButtonSearch_CheckedChanged(object sender, EventArgs e)
        {
            Panel panelSearchPage = (Panel)Controls["panelSearchPage"];
            Panel panelAddFilePage = (Panel)Controls["panelAddFilePage"];
            if (panelSearchPage.Visible == true)
            {
                panelSearchPage.Visible = false;
            }
            else
            {
                panelSearchPage.Visible = true;
            }
        }

        private void showPastDaysChanges(DateTime fromDateTime)
        {
            //[PL0218]Show recent "fromDateTime" to "now" changes
            FileQueryConditions fileQueryConditions = new FileQueryConditions()
            {
                FileName = "",
                DateCreatedTimeFrom = fromDateTime,
                DateCreatedTimeTo = DateTime.Now
            };
            List<DOTFile> dOTQueriedFilesRecentOneDay = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            var distinctFolderCountOneDay = dOTQueriedFilesRecentOneDay.Select(x => x.ParentFolder).Distinct().Count();
            String recentChangeStatistic = dOTQueriedFilesRecentOneDay.Count.ToString() + " Files, " + distinctFolderCountOneDay.ToString() + " Folders changed.";
            Label recentChangeStatisticLabel = (Label)Controls["panelSearchPage"].Controls["recentChangeStatisticLabel"];
            recentChangeStatisticLabel.Text = recentChangeStatistic;
            //fill out the query results table
            fillDataGridView(dOTQueriedFilesRecentOneDay);
        }

        private void showDynamicInfo()
        {
            //[PL0218]Show past 24 hours to "now" changes
            showPastDaysChanges(DateTime.Now.AddDays(-1));

            //[PL0218]Query total file and foler info.
            FileQueryConditions fileQueryConditions = new FileQueryConditions()
            {
                FileName = "",
                DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                DateCreatedTimeTo = DateTime.Now
            };
            List<DOTFile> dOTQueriedFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            var distinctFolderCountAll = dOTQueriedFiles.Select(x => x.ParentFolder).Distinct().Count();
            String totalFileAndFolderInfo = "There are " + dOTQueriedFiles.Count.ToString() + " files and " + distinctFolderCountAll.ToString() + " folders into File Management System.";
            Label totalFileAndFolderInfoLabel = (Label)Controls["panelSearchPage"].Controls["totalFileAndFolderInfoLabel"];
            totalFileAndFolderInfoLabel.Text = totalFileAndFolderInfo;

            //[PL0218]Show counts of filter based on file types
            fileQueryConditions.FileName = ".pdf";
            List<DOTFile> dOTPDFFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            LinkLabel linkLabelFileType1 = (LinkLabel)Controls["panelSearchPage"].Controls["linkLabelFileType1"];
            linkLabelFileType1.Text = dOTPDFFiles.Count.ToString() + " PDF Files";
            linkLabelFileType1.LinkArea = new System.Windows.Forms.LinkArea(0, dOTPDFFiles.Count.ToString().Length);

            fileQueryConditions.FileName = ".tiff";
            List<DOTFile> dOTTIFFFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            LinkLabel linkLabelFileType2 = (LinkLabel)Controls["panelSearchPage"].Controls["linkLabelFileType2"];
            linkLabelFileType2.Text = dOTTIFFFiles.Count.ToString() + " TIFF Files";
            linkLabelFileType2.LinkArea = new System.Windows.Forms.LinkArea(0, dOTTIFFFiles.Count.ToString().Length);

            fileQueryConditions.FileName = ".jpg";
            List<DOTFile> dOTJPGFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            LinkLabel linkLabelFileType3 = (LinkLabel)Controls["panelSearchPage"].Controls["linkLabelFileType3"];
            linkLabelFileType3.Text = dOTJPGFiles.Count.ToString() + " JPG Files";
            linkLabelFileType3.LinkArea = new System.Windows.Forms.LinkArea(0, dOTJPGFiles.Count.ToString().Length);

            LinkLabel linkLabelFileType4 = (LinkLabel)Controls["panelSearchPage"].Controls["linkLabelFileType4"];
            int otherFileTypeCount = dOTQueriedFiles.Count - dOTPDFFiles.Count - dOTTIFFFiles.Count - dOTJPGFiles.Count;
            linkLabelFileType4.Text = otherFileTypeCount.ToString() + " Other Files";
            linkLabelFileType4.LinkArea = new System.Windows.Forms.LinkArea(0, otherFileTypeCount.ToString().Length);
        }

        private Panel createPagePanel(String panelName)
        {
            Panel panel = new Panel()
            {
                Name = panelName,
                Location = new Point(23, 80),
                Size = new Size(760, 800),
                BorderStyle = BorderStyle.None
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
            buttonAdvanced.Click += ButtonAdvanced_Click;
            panelSearchPage.Controls.Add(buttonAdvanced);

            //[PL0217]Add cueText search.
            CueTextBox searchCueTextBox = new CueTextBox()
            {
                Name = "searchCueTextBox",
                Cue = "Search",
                Font = new Font("Segoe UI", 15),
                Location = new Point(207, 28),
                Size = new Size(435, 30),
                TabIndex = 0
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
                Name = "comboBoxChangesInPast",
                Location = new Point(325, 85),
                Font = new Font("Segoe UI", 15)
            };
            comboBoxChangesInPast.Items.Add("24 Hours");
            comboBoxChangesInPast.Items.Add("2 Days");
            comboBoxChangesInPast.Items.Add("3 Days");
            comboBoxChangesInPast.SelectedIndex = 0;
            comboBoxChangesInPast.SelectedIndexChanged += ComboBoxChangesInPast_SelectedIndexChanged;
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

            Label totalFileAndFolderInfoLabel = new Label()
            {
                Name = "totalFileAndFolderInfoLabel",
                Location = new Point(10, 616),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = "There are"
            };
            panelSearchPage.Controls.Add(totalFileAndFolderInfoLabel);

            //[PL0218]Add horizontal line
            Panel useAsHorizontalLine = new Panel()
            {
                Location = new Point(13, 645),
                Size = new Size(735, 1),
                BorderStyle = BorderStyle.FixedSingle,
                Name = "useAsHorizontalLine"
            };
            panelSearchPage.Controls.Add(useAsHorizontalLine);

            //[PL0218]Add file types label
            LinkLabel linkLabelFileType1 = new LinkLabel()
            {
                Name = "linkLabelFileType1",
                AutoSize = true,
                Location = new Point(13, 657),
                Font = new Font("Segoe UI", 12),
                Text = ""
            };
            linkLabelFileType1.LinkClicked += LinkLabelFileType1_LinkClicked;
            panelSearchPage.Controls.Add(linkLabelFileType1);

            LinkLabel linkLabelFileType2 = new LinkLabel()
            {
                Name = "linkLabelFileType2",
                AutoSize = true,
                Location = new Point(150, 657),
                Font = new Font("Segoe UI", 12),
                Text = ""
            };
            linkLabelFileType2.LinkClicked += LinkLabelFileType2_LinkClicked;
            panelSearchPage.Controls.Add(linkLabelFileType2);

            LinkLabel linkLabelFileType3 = new LinkLabel()
            {
                Name = "linkLabelFileType3",
                AutoSize = true,
                Location = new Point(300, 657),
                Font = new Font("Segoe UI", 12),
                Text = ""
            };
            linkLabelFileType3.LinkClicked += LinkLabelFileType3_LinkClicked;
            panelSearchPage.Controls.Add(linkLabelFileType3);

            LinkLabel linkLabelFileType4 = new LinkLabel()
            {
                Name = "linkLabelFileType4",
                AutoSize = true,
                Location = new Point(450, 657),
                Font = new Font("Segoe UI", 12),
                Text = ""
            };
            linkLabelFileType4.LinkClicked += LinkLabelFileType4_LinkClicked;
            panelSearchPage.Controls.Add(linkLabelFileType4);

            DataGridView dataGridView1 = new DataGridView()
            {
                Name = "dataGridView1",
                Size = new Size(728, 335),
                Location = new Point(15, 280),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Font = new Font("Segoe UI", 10)
            };
            panelSearchPage.Controls.Add(dataGridView1);

            //[PL0217]Add panel for advanced search
            Panel panelAdvancedSearch = new Panel()
            {
                Location = new Point(100, 76),
                Size = new Size(545, 400),
                BorderStyle = BorderStyle.FixedSingle,
                Name = "panelAdvancedSearch",

            };
            panelAdvancedSearch.Visible = false;

            //[PL0218]Add components to advanced search panel
            Label documentTitleLabel = new Label()
            {
                Name = "documentTitleLabel",
                Location = new Point(10, 20),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Document Title"
            };
            panelAdvancedSearch.Controls.Add(documentTitleLabel);
            TextBox documentTitleTextBox = new TextBox()
            {
                Name = "documentTitleTextBox",
                Location = new Point(10, 40),
                Font = new Font("Segoe UI", 12),
                Size = new Size(520, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(documentTitleTextBox);

            Label fMLabel = new Label()
            {
                Name = "fMLabel",
                Location = new Point(10, 80),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Project FM Number (FM)"
            };
            panelAdvancedSearch.Controls.Add(fMLabel);
            TextBox fMTextBox = new TextBox()
            {
                Name = "fMTextBox",
                Location = new Point(10, 100),
                Font = new Font("Segoe UI", 12),
                Size = new Size(200, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(fMTextBox);

            Label sRLabel = new Label()
            {
                Name = "sRLabel",
                Location = new Point(328, 80),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "State Road (SR)"
            };
            panelAdvancedSearch.Controls.Add(sRLabel);
            TextBox sRTextBox = new TextBox()
            {
                Name = "sRTextBox",
                Location = new Point(328, 100),
                Font = new Font("Segoe UI", 12),
                Size = new Size(200, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(sRTextBox);

            Label studyTypeLabel = new Label()
            {
                Name = "studyTypeLabel",
                Location = new Point(10, 140),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Study Type"
            };
            panelAdvancedSearch.Controls.Add(studyTypeLabel);
            TextBox studyTypeTextBox = new TextBox()
            {
                Name = "studyType",
                Location = new Point(10, 160),
                Font = new Font("Segoe UI", 12),
                Size = new Size(520, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(studyTypeTextBox);

            Label descriptionLabel = new Label()
            {
                Name = "descriptionLabel",
                Location = new Point(10, 200),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Description"
            };
            panelAdvancedSearch.Controls.Add(descriptionLabel);
            TextBox descriptionTextBox = new TextBox()
            {
                Name = "descriptionTextBox",
                Location = new Point(10, 220),
                Font = new Font("Segoe UI", 12),
                Size = new Size(520, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(descriptionTextBox);

            Label authorLabel = new Label()
            {
                Name = "authorLabel",
                Location = new Point(10, 260),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Author"
            };
            panelAdvancedSearch.Controls.Add(authorLabel);
            TextBox authorTextBox = new TextBox()
            {
                Name = "authorTextBox",
                Location = new Point(10, 280),
                Font = new Font("Segoe UI", 12),
                Size = new Size(520, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(authorTextBox);

            Label createdTimeFromLabel = new Label()
            {
                Name = "createdTimeFromLabel",
                Location = new Point(10, 320),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "From"
            };
            panelAdvancedSearch.Controls.Add(createdTimeFromLabel);

            DateTimePicker createdTimeFromDateTimePicker = new DateTimePicker()
            {
                Name = "createdTimeFromDateTimePicker",
                Location = new Point(10, 340),
                Font = new Font("Segoe UI", 12),
                Size = new Size(200, 20)

            };
            createdTimeFromDateTimePicker.Format = DateTimePickerFormat.Custom;
            createdTimeFromDateTimePicker.CustomFormat = "MM/dd/yyyy";
            panelAdvancedSearch.Controls.Add(createdTimeFromDateTimePicker);

            Label createdTimeToLabel = new Label()
            {
                Name = "createdTimeToLabel",
                Location = new Point(328, 320),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "To"
            };
            panelAdvancedSearch.Controls.Add(createdTimeToLabel);
            DateTimePicker createdTimeToDateTimePicker = new DateTimePicker()
            {
                Name = "createdTimeToDateTimePicker",
                Location = new Point(328, 340),
                Font = new Font("Segoe UI", 12),
                Size = new Size(200, 20)
            };
            createdTimeToDateTimePicker.Format = DateTimePickerFormat.Custom;
            createdTimeToDateTimePicker.CustomFormat = "MM/dd/yyyy";
            panelAdvancedSearch.Controls.Add(createdTimeToDateTimePicker);


            panelSearchPage.Controls.Add(panelAdvancedSearch);
            panelAdvancedSearch.BringToFront();
            this.Controls.Add(panelSearchPage);





        }

        private void ButtonAdvanced_Click(object sender, EventArgs e)
        {
            Panel panelAdvancedSearch = (Panel)Controls["panelSearchPage"].Controls["panelAdvancedSearch"];
            if (panelAdvancedSearch.Visible == true)
            {
                panelAdvancedSearch.Visible = false;
            }
            else
            {
                panelAdvancedSearch.Visible = true;
            }
        }

        private void LinkLabelFileType4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //[PL0218]"Other" file type link is clicked
            FileQueryConditions fileQueryConditions = new FileQueryConditions()
            {
                FileNameNotContain = new String[] { ".jpg", ".tiff", ".pdf" },
                DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                DateCreatedTimeTo = DateTime.Now
            };
            List<DOTFile> dOTOtherFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            //fill out the query results table
            fillDataGridView(dOTOtherFiles);
        }

        private void LinkLabelFileType3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileQueryConditions fileQueryConditions = new FileQueryConditions()
            {
                FileName = ".jpg",
                DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                DateCreatedTimeTo = DateTime.Now
            };
            List<DOTFile> dOTJPGFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            //fill out the query results table
            fillDataGridView(dOTJPGFiles);
        }

        private void LinkLabelFileType2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileQueryConditions fileQueryConditions = new FileQueryConditions()
            {
                FileName = ".tiff",
                DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                DateCreatedTimeTo = DateTime.Now
            };
            List<DOTFile> dOTTIFFFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            //fill out the query results table
            fillDataGridView(dOTTIFFFiles);

        }

        private void LinkLabelFileType1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileQueryConditions fileQueryConditions = new FileQueryConditions()
            {
                FileName = ".pdf",
                DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                DateCreatedTimeTo = DateTime.Now
            };
            List<DOTFile> dOTPDFFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            //fill out the query results table
            fillDataGridView(dOTPDFFiles);
        }

        private void ComboBoxChangesInPast_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboBoxChangesInPast = (ComboBox)Controls["panelSearchPage"].Controls["ComboBoxChangesInPast"];
            switch (ComboBoxChangesInPast.SelectedIndex)
            {
                case 0:
                    showPastDaysChanges(DateTime.Now.AddDays(-1));
                    break;
                case 1:
                    showPastDaysChanges(DateTime.Now.AddDays(-2));
                    break;
                case 2:
                    showPastDaysChanges(DateTime.Now.AddDays(-3));
                    break;
                default:
                    break;
            }
        }

        private void createSingleSetAdditionalItem(String title, String textBoxName, String warning, Point location)
        {
            Label labelTitle = new Label()
            {
                Name = title,
                Font = new Font("Segoe UI", 15),
                Location = location,
                AutoSize = true,
                Text = title
            };
            Controls["panelAddFilePage"].Controls.Add(labelTitle);

            TextBox textBoxContent = new TextBox()
            {
                Name = textBoxName,
                Location = new Point(location.X, location.Y + 35),
                Font = new Font("Segoe UI", 15),
                Size = new Size(315, 20),
                AutoSize = true,
                Text = ""
            };
            Controls["panelAddFilePage"].Controls.Add(textBoxContent);
            Label labelWarning = new Label()
            {
                Name = warning,
                Font = new Font("Seravek", 8, FontStyle.Italic),
                Location = new Point(location.X - 2, textBoxContent.Location.Y + 45),
                AutoSize = true,
                Text = warning
            };
            Controls["panelAddFilePage"].Controls.Add(labelWarning);
        }

        private void initAddFilePageComponentsSetup()
        {
            //[PL0217]Create a big panel for all "Add File" components.
            Panel panelAddFilePage = createPagePanel("panelAddFilePage");

            int startingXPoint = 25;
            int startingYPoint = 0;
            //[PL0217]Add button for advanced search.
            Button buttonSelectFile = new Button()
            {
                Text = "Select File",
                Font = new Font("Segoe UI", 15),
                Location = new Point(startingXPoint, startingYPoint),
                Size = new Size(120, 40),
                AutoSize = false
            };
            buttonSelectFile.Click += ButtonSelectFile_Click;
            panelAddFilePage.Controls.Add(buttonSelectFile);

            LinkLabel linkLabelPath = new LinkLabel()
            {
                Name = "linkLabelPath",
                AutoSize = true,
                Location = new Point(startingXPoint + 180, startingYPoint + 5),
                Font = new Font("Segoe UI", 12),
                Text = "Path: ",
                MaximumSize = new Size(520, 0)


            };
            linkLabelPath.LinkClicked += LinkLabelPath_LinkClicked;
            panelAddFilePage.Controls.Add(linkLabelPath);

            Label labelInOrderTo = new Label()
            {
                Name = "labelInOrderTo",
                Font = new Font("Seravek", 10, FontStyle.Italic),
                Location = new Point(startingXPoint, startingYPoint + 60),
                AutoSize = true,
                Text = "In order to save the file please complete the following fields"
            };
            panelAddFilePage.Controls.Add(labelInOrderTo);

            this.Controls.Add(panelAddFilePage);
            createSingleSetAdditionalItem("Section Number*", "textBoxSectionNumber", "Please make sure to enter the eight digits section number.", Location = new Point(startingXPoint, startingYPoint + 60 + 30));
            TextBox textBoxSectionNumber = (TextBox)Controls["panelAddFilePage"].Controls["textBoxSectionNumber"];
            textBoxSectionNumber.MaxLength = 8;
            textBoxSectionNumber.TextChanged += TextBoxSectionNumber_TextChanged;
            textBoxSectionNumber.KeyPress += TextBoxSectionNumber_KeyPress;
            createSingleSetAdditionalItem("State Road (SR)*", "textBoxSR", "Type only the number.", Location = new Point(startingXPoint + 375, startingYPoint + 60 + 30));

            createSingleSetAdditionalItem("Study Type*", "comboBoxStudyType", "Select one study type.", Location = new Point(startingXPoint, startingYPoint + 200));
            createSingleSetAdditionalItem("Location*", "comboBoxLocation", "Select one location type.", Location = new Point(startingXPoint + 375, startingYPoint + 200));

            createSingleSetAdditionalItem("Beginning Milepost*", "textBoxBeginningMilepost", "Enter only Milepost digits and double the numbers to avoid errors" + Environment.NewLine + "into the system.", Location = new Point(startingXPoint, startingYPoint + 310));
            TextBox textBoxBeginningMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxBeginningMilepost"];
            textBoxBeginningMilepost.Size = new Size(120, 20);

            createSingleSetAdditionalItem("Ending Milepost*", "textBoxEndingMilepost", "", Location = new Point(startingXPoint + 195, startingYPoint + 310));
            TextBox textBoxEndingMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxEndingMilepost"];
            textBoxEndingMilepost.Size = new Size(120, 20);

            createSingleSetAdditionalItem("FM Number*", "textBoxFMNumber", "Enter the number, including \"-\".", Location = new Point(startingXPoint + 375, startingYPoint + 310));

            createSingleSetAdditionalItem("Author*", "textBoxAuthor", "Add the author of the report. For instance, consultant company name.", Location = new Point(startingXPoint, startingYPoint + 430));
            createSingleSetAdditionalItem("Key words", "textBoxKeyWords", "Add key words improve results in document searches.", Location = new Point(startingXPoint + 375, startingYPoint + 430));



            Label labelFileName = new Label()
            {
                Name = "labelFileName",
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                Location = new Point(startingXPoint, startingYPoint + 540),
                AutoSize = false,
                BackColor = Color.Pink,
                Size = new Size(690, 30),
                Text = "File Name: " + newFileName
            };
            panelAddFilePage.Controls.Add(labelFileName);

            LinkLabel linkLabelFileNameStructure = new LinkLabel()
            {
                Name = "linkLabelFileNameStructure",
                Font = new Font("Seravek", 8, FontStyle.Italic),
                Location = new Point(startingXPoint + 560, startingYPoint + 575),
                AutoSize = true,
                Text = "About File Name Structure."
            };
            panelAddFilePage.Controls.Add(linkLabelFileNameStructure);

            Label labelFileInfo = new Label()
            {
                Name = "labelFileInfo",
                Font = new Font("Segoe UI", 15),
                Location = new Point(startingXPoint, startingYPoint + 585),
                AutoSize = true,
                Text = "File Info"
            };
            panelAddFilePage.Controls.Add(labelFileInfo);

            //Label labelComments = new Label()
            //{
            //    Name = "labelComments",
            //    Font = new Font("Segoe UI", 15),
            //    Location = new Point(startingXPoint + 375, startingYPoint + 585),
            //    AutoSize = true,
            //    Text = "Comments"
            //};
            //panelAddFilePage.Controls.Add(labelComments);

            Label labelFileInfoContent = new Label()
            {
                Name = "labelFileInfoContent",
                Font = new Font("Segoe UI", 11),
                Location = new Point(startingXPoint, startingYPoint + 620),
                AutoSize = false,
                BackColor = Color.Gray,
                Size = new Size(315, 130),
                Text = "File Info Details..."
            };
            panelAddFilePage.Controls.Add(labelFileInfoContent);

            createSingleSetAdditionalItem("Comments", "textBoxComments", "Add important comments related to the study.", Location = new Point(startingXPoint + 375, startingYPoint + 585));
            TextBox textBoxComments = (TextBox)Controls["panelAddFilePage"].Controls["textBoxComments"];
            textBoxComments.Multiline = true;
            textBoxComments.Size = new Size(315, 80);
            Label labelComments = (Label)Controls["panelAddFilePage"].Controls["Add important comments related to the study."];
            labelComments.Location = new Point(textBoxComments.Location.X - 2, textBoxComments.Location.Y + 95);

            Label labelRequiredField = new Label()
            {
                Name = "labelRequiredField",
                Font = new Font("Segoe UI", 12),
                Location = new Point(startingXPoint, startingYPoint + 760),
                AutoSize = true,
                Text = "* Required Field"
            };
            panelAddFilePage.Controls.Add(labelRequiredField);

            Button buttonSave = new Button()
            {
                Name = "buttonSave",
                Font = new Font("Segoe UI", 15),
                Location = new Point(startingXPoint + 570, startingYPoint + 740),
                Size = new Size(120, 40),
                AutoSize = false,
                Text = "Save"
            };
            buttonSave.Click += ButtonSave_Click;
            panelAddFilePage.Controls.Add(buttonSave);
        }

        private void TextBoxSectionNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //[0219]Only allow numbers to keyin
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBoxSectionNumber_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxSectionNumber = (TextBox)Controls["panelAddFilePage"].Controls["textBoxSectionNumber"];

            Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ": " + textBoxSectionNumber.Text
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[1]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[2]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[3]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[4]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[5];
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //[PL0218]Destination folder should read from config file
            string fileToCopy = sSelectedFile;
            string destinationDirectory = ".\\dummyDest\\";

            //File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
            Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];
            File.Copy(fileToCopy, destinationDirectory + labelFileName.Text.Split(':')[1]);
        }

        private void LinkLabelPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private string getFileSizeHumanReadable(long fileLength)
        {
            string sLen = fileLength.ToString();
            if (fileLength >= (1 << 30))
                sLen = string.Format("{0}Gb", fileLength >> 30);
            else
            if (fileLength >= (1 << 20))
                sLen = string.Format("{0}Mb", fileLength >> 20);
            else
            if (fileLength >= (1 << 10))
                sLen = string.Format("{0}Kb", fileLength >> 10);
            else
                sLen = string.Format("{0}byte", fileLength);

            return sLen;
        }

        private void ButtonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = true;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                sSelectedFile = choofdlog.FileName;
                LinkLabel linkLabelPath = (LinkLabel)Controls["panelAddFilePage"].Controls["linkLabelPath"];
                linkLabelPath.Text += sSelectedFile;
                Label labelFileInfoContent = (Label)Controls["panelAddFilePage"].Controls["labelFileInfoContent"];
                FileInfo fileInfo = new FileInfo(sSelectedFile);

                labelFileInfoContent.Text = "Date Created: " + fileInfo.CreationTime
                    + Environment.NewLine
                    + "Date Modified: " + fileInfo.LastWriteTime
                    + Environment.NewLine
                    + "Size: " + getFileSizeHumanReadable(fileInfo.Length)
                    + Environment.NewLine
                    + "Type: " + Path.GetFileName(sSelectedFile).Split('.').Last();

                //newFileName = "." + sSelectedFile.Split('.').Last();
                //Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];
                //labelFileName.Text = "File Name: ";
                //labelFileName.Text += newFileName;
            }
            else
            {
                sSelectedFile = string.Empty;
            }
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
            String XMLInputFile = "./inputFile/outputxml.xml";
            if (!File.Exists(XMLInputFile))
            {

                string csv = File.ReadAllText("./inputFile/FileInfoSummary.csv");
                XDocument doc = ConverstorCsvToXml.ConvertCsvToXML(csv, new[] { "\",\"" });
                doc.Save(XMLInputFile);
            }
            else
            {
                //XML file exists already
                ///TODO
            }

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
            //[PL0218]Hide advanced search if it is visible.
            Panel panelAdvancedSearch = (Panel)Controls["panelSearchPage"].Controls["panelAdvancedSearch"];
            panelAdvancedSearch.Visible = false;

            //[PL0217]Refer to the dynamic created component
            CueTextBox searchCueTextBox = (CueTextBox)Controls["panelSearchPage"].Controls["searchCueTextBox"];
            Label warningLabel = (Label)Controls["panelSearchPage"].Controls["warningLabel"];
            warningLabel.Visible = false;
            //check the search key word
            if (searchCueTextBox.Text != "")
            {
                warningLabel.Text = "";
                //get from datetime
                //DateTime fromDateTime = GetDateTimeConsolidation(fromDatePortionDateTimePicker, fromTimePortionDateTimePicker);

                //get to datetime
                //DateTime toDateTime = GetDateTimeConsolidation(toDatePorttionDateTimePicker, toTimePortionDateTimePicker);

                //query the xml input file based on the search conditions
                FileQueryConditions fileQueryConditions = new FileQueryConditions()
                {
                    FileName = searchCueTextBox.Text,
                    DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                    DateCreatedTimeTo = DateTime.Now
                };
                List<DOTFile> dOTQueriedFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
                //var distinctFolderCount = dOTQueriedFiles.Select(x => x.ParentFolder).Distinct().Count();
                //String recentChangeStatistic = dOTQueriedFiles.Count.ToString() + " Files, " + distinctFolderCount.ToString() + " Folders changed.";
                //Label recentChangeStatisticLabel = (Label)Controls["panelSearchPage"].Controls["recentChangeStatisticLabel"];
                //recentChangeStatisticLabel.Text = recentChangeStatistic;

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
                else if (keywordFirst != "")
                {
                    recentSearchLabel.Text = "Recent search term: ";
                    recentSearchLabel.Text = recentSearchLabel.Text + "\"" + keywordFirst + "\".";
                }
                numberOfSearchTerm++;
            }
            else
            {
                warningLabel.Text = "Please specify the keyword to search.";
                warningLabel.Visible = true;
                warningLabel.ForeColor = Color.Red;
            }
            searchCueTextBox.Text = "";
        }

        //private List<DOTFile> queryInfoFromXMLInputFile(String XMLInputFileName, String fileNameKeyWord, DateTime fromDateTime, DateTime toDateTime)
        private List<DOTFile> queryInfoFromXMLInputFile(String XMLInputFileName, FileQueryConditions fileQueryConditions)
        {
            List<DOTFile> dOTFiles = new List<DOTFile>();
            XElement root = XElement.Load(XMLInputFileName);
            DateTime fromDateTime = fileQueryConditions.DateCreatedTimeFrom;
            DateTime toDateTime = fileQueryConditions.DateCreatedTimeTo;
            String fileNameKeyWord = fileQueryConditions.FileName;
            if (fromDateTime <= toDateTime)
            {
                var selectedForTextFilter = from cli in root.Elements("row").Elements("var")
                                            where (string)cli.Attribute("name").Value == "Name" && cli.Attribute("value").Value.ToUpper().Contains(fileNameKeyWord.ToUpper()) == true
                                            select cli.Parent;

                var selectedForTextFilterAndCreatedFromTime = from cli in selectedForTextFilter.Elements("var")
                                                              where (string)cli.Attribute("name").Value == "DateCreated" && DateTime.Parse(cli.Attribute("value").Value).CompareTo(fromDateTime) >= 0
                                                              select cli.Parent;
                var selectedForTextFilterAndCreatedFromAndToTime = from cli in selectedForTextFilterAndCreatedFromTime.Elements("var")
                                                                   where (string)cli.Attribute("name").Value == "DateCreated" && DateTime.Parse(cli.Attribute("value").Value).CompareTo(toDateTime) <= 0
                                                                   select cli.Parent;
                foreach (var d in selectedForTextFilterAndCreatedFromAndToTime)
                {
                    DOTFile dOTFile = queryInfoForOneFile(d);
                    dOTFiles.Add(dOTFile);
                }
                return dOTFiles;
            }
            else if (fileQueryConditions.FileNameNotContain != null)
            {
                var fullList = from cli in root.Elements("row").Elements("var")
                               select cli.Parent;
                //[PL0218]this is not a good way, need to dynamic generate the filter conditon, instead of hardcode.
                var selected = from cli in root.Elements("row").Elements("var")
                               where (string)cli.Attribute("name").Value == "Name" &&
                               (cli.Attribute("value").Value.ToUpper().Contains(fileQueryConditions.FileNameNotContain[0].ToUpper()) == true ||
                               cli.Attribute("value").Value.ToUpper().Contains(fileQueryConditions.FileNameNotContain[1].ToUpper()) == true ||
                               cli.Attribute("value").Value.ToUpper().Contains(fileQueryConditions.FileNameNotContain[2].ToUpper()) == true)
                               select cli.Parent;
                foreach (var d in fullList.Except(selected))
                {
                    DOTFile dOTFile = queryInfoForOneFile(d);
                    dOTFiles.Add(dOTFile);
                }
                return dOTFiles;
            }
            else
            {
                var selected = from cli in root.Elements("row").Elements("var")
                               where (string)cli.Attribute("name").Value == "Name" && cli.Attribute("value").Value.ToUpper().Contains(fileNameKeyWord.ToUpper()) == true
                               select cli.Parent;
                foreach (var d in selected)
                {
                    DOTFile dOTFile = queryInfoForOneFile(d);
                    dOTFiles.Add(dOTFile);
                }
                return dOTFiles;
            }


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
            var queriedDataSource = dOTQueriedFiles.Select(i => new { i.Name, i.ParentFolder, Ext = (String)i.Name.Split('.').Last() }).ToArray();
            DataGridView dataGridView1 = (DataGridView)Controls["panelSearchPage"].Controls["dataGridView1"];
            dataGridView1.DataSource = queriedDataSource;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
