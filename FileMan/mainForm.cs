using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using System.Diagnostics;

namespace FileMan
{
    public partial class mainForm : Form
    {
        //[PL0218]Used for selected file.
        string sSelectedFile;
        string newFileName = "SectionNo_SRxx_StudyType_Location_MPxx_xx.suffix";
        //[PL0219]New file to save
        DOTFile dOTFileNewSave = new DOTFile();
        //BackgroundWorker backgroundWorker = new BackgroundWorker();

        //[PL0221]Add the flag to check if the menu is showing
        bool isMenuShowing = false;

        public mainForm()
        {
            InitializeComponent();
            initComponentsSetup();

            //backgroundWorker.DoWork += BackgroundWorker_DoWork;
            //backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            //this.Size = new Size(816, 854);
            this.Size = new Size(1000, 954);
            this.BackColor = Color.White;
            //this.ForeColor = ColorTranslator.FromHtml("#ce2b2f");
            //this.FormElement.TitleBar.ForeColor = ColorTranslator.FromHtml("#ce2b2f");
            ScrollBar vScrollBar1 = new VScrollBar();
            vScrollBar1.Dock = DockStyle.Right;
            //vScrollBar1.Scroll += (sender, e) => { flowLayoutPanel.VerticalScroll.Value = vScrollBar1.Value; };
            //flowLayoutPanel.Controls.Add(vScrollBar1);
            //this.Controls.Add(flowLayoutPanel);
            //obtain the input file and check if it is the latest one
            Boolean inputFileStatus = CheckTheInputFileStatus();
            showDynamicInfo();
            this.Click += MainForm_Click;
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            if (isMenuShowing)
            {
                closeMenu();
            }
        }

        private void closeMenu()
        {
            Button buttonMenuSearch = (Button)Controls["panelMenu"].Controls["buttonMenuSearch"];
            Button buttonMenuAddFile = (Button)Controls["panelMenu"].Controls["buttonMenuAddFile"];
            Label labelRedRectangle = (Label)Controls["panelMenu"].Controls["labelRedRectangle"];
            buttonMenuSearch.Visible = false;
            buttonMenuAddFile.Visible = false;
            labelRedRectangle.Visible = false;
            Panel panelMenu = (Panel)Controls["panelMenu"];
            panelMenu.Size = new Size(200, 50);
            panelMenu.BackColor = Color.White;
            isMenuShowing = false;
        }

        private void initComponentsSetup()
        {
            //[PL0217]Add panel for menu
            Panel panelMenu = new Panel()
            {
                Location = new Point(23, 15),
                Size = new Size(200, 150),
                BorderStyle = BorderStyle.None,
                Name = "panelMenu",
                TabIndex = 0
            };

            //[PL0221]Change the radio buttons to buttons into a panel
            Button buttonMenuIcon = new Button()
            {
                Name = "buttonMenuIcon",
                Location = new Point(0, 0),
                Font = new Font("Segoe UI Semilight", 24),
                AutoSize = true,
                Text = "☰",
                FlatStyle = FlatStyle.Flat
            };
            buttonMenuIcon.Click += ButtonMenuIcon_Click;
            buttonMenuIcon.FlatAppearance.BorderSize = 0;
            buttonMenuIcon.FlatAppearance.BorderColor = Color.White;

            Label labelRedRectangle = new Label()
            {
                Name = "labelRedRectangle",
                Text = "▮",
                Font = new Font("Segoe UI Semilight", 17),
                AutoSize = true,
                Location = new Point(0, 58),
                ForeColor = ColorTranslator.FromHtml("#ce2b2f")
            };
            panelMenu.Controls.Add(labelRedRectangle);

            Button buttonMenuSearch = new Button()
            {
                Name = "buttonMenuSearch",
                Text = "Search",
                Location = new Point(50, 55),
                Font = new Font("Segoe UI Semilight", 15),
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTranslator.FromHtml("#e5e5e5")

            };
            buttonMenuSearch.FlatAppearance.BorderSize = 0;
            buttonMenuSearch.FlatAppearance.BorderColor = Color.White;
            buttonMenuSearch.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#e5e5e5");
            buttonMenuSearch.Click += ButtonMenuSearch_Click;
            buttonMenuSearch.MouseMove += ButtonMenuSearch_MouseMove;
            Button buttonMenuAddFile = new Button()
            {
                Name = "buttonMenuAddFile",
                Text = "Add File",
                Location = new Point(50, 100),
                Font = new Font("Segoe UI Semilight", 15),
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTranslator.FromHtml("#e5e5e5")
            };
            buttonMenuAddFile.Click += ButtonMenuAddFile_Click;
            buttonMenuAddFile.MouseMove += ButtonMenuAddFile_MouseMove;
            buttonMenuAddFile.FlatAppearance.BorderSize = 0;
            buttonMenuAddFile.FlatAppearance.BorderColor = Color.White;
            buttonMenuAddFile.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#e5e5e5");

            panelMenu.Controls.Add(buttonMenuIcon);
            panelMenu.Controls.Add(buttonMenuSearch);
            panelMenu.Controls.Add(buttonMenuAddFile);
            panelMenu.Visible = true;
            buttonMenuSearch.Visible = false;
            buttonMenuAddFile.Visible = false;
            labelRedRectangle.Visible = false;
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

        private void ButtonMenuSearch_MouseMove(object sender, MouseEventArgs e)
        {
            Label labelRedRectangle = (Label)Controls["panelMenu"].Controls["labelRedRectangle"];
            labelRedRectangle.Location = new Point(0, 58);
        }

        private void ButtonMenuAddFile_MouseMove(object sender, MouseEventArgs e)
        {
            Label labelRedRectangle = (Label)Controls["panelMenu"].Controls["labelRedRectangle"];
            labelRedRectangle.Location = new Point(0, 102);
        }

        private void ButtonMenuAddFile_Click(object sender, EventArgs e)
        {
            Panel panelSearchPage = (Panel)Controls["panelSearchPage"];
            Panel panelAddFilePage = (Panel)Controls["panelAddFilePage"];
            panelSearchPage.Visible = false;
            panelAddFilePage.Visible = true;

            closeMenu();

            Button buttonMenuIcon = (Button)Controls["panelMenu"].Controls["buttonMenuIcon"];
            buttonMenuIcon.Text = "☰  Add File";
        }

        private void ButtonMenuSearch_Click(object sender, EventArgs e)
        {
            Panel panelSearchPage = (Panel)Controls["panelSearchPage"];
            Panel panelAddFilePage = (Panel)Controls["panelAddFilePage"];
            panelAddFilePage.Visible = false;
            panelSearchPage.Visible = true;

            closeMenu();

            Button buttonMenuIcon = (Button)Controls["panelMenu"].Controls["buttonMenuIcon"];
            buttonMenuIcon.Text = "☰  Search";

        }

        private void ButtonMenuIcon_Click(object sender, EventArgs e)
        {
            Button buttonMenuSearch = (Button)Controls["panelMenu"].Controls["buttonMenuSearch"];
            Button buttonMenuAddFile = (Button)Controls["panelMenu"].Controls["buttonMenuAddFile"];
            Label labelRedRectangle = (Label)Controls["panelMenu"].Controls["labelRedRectangle"];
            Panel panelMenu = (Panel)Controls["panelMenu"];
            if (!isMenuShowing)
            {
                buttonMenuSearch.Visible = true;
                buttonMenuAddFile.Visible = true;
                labelRedRectangle.Visible = true;
                panelMenu.Size = new Size(200, 150);
                panelMenu.BackColor = ColorTranslator.FromHtml("#e5e5e5");
                isMenuShowing = true;
            }
            else
            {
                buttonMenuSearch.Visible = false;
                buttonMenuAddFile.Visible = false;
                labelRedRectangle.Visible = false;
                panelMenu.Size = new Size(200, 50);
                panelMenu.BackColor = Color.White;
                isMenuShowing = false;
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
            String recentChangeStatistic = "⬛  " + dOTQueriedFilesRecentOneDay.Count.ToString() + " Files, " + distinctFolderCountOneDay.ToString() + " Folders changed.";
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
                Size = new Size(999, 800),
                BorderStyle = BorderStyle.None
            };
            return panel;
        }

        private void initSearchPageComponentsSetup()
        {
            //[PL0217]Create a big panel for all "Search page" components.
            Panel panelSearchPage = createPagePanel("panelSearchPage");

            panelSearchPage.Click += PanelSearchPage_Click;

            //[PL0217]Add button for advanced search.
            Button buttonAdvanced = new Button()
            {
                Text = "Advanced ▾",
                Font = new Font("Segoe UI", 12),
                Location = new Point(200, 25),
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
                Location = new Point(307, 28),
                Size = new Size(435, 30),
                TabIndex = 0
            };
            panelSearchPage.Controls.Add(searchCueTextBox);

            //[PL0217]Add warning label to avoid empty search, that will return all files, performance low.
            Label warningLabel = new Label()
            {
                Name = "warningLabel",
                Font = new Font("Segoe UI", 12),
                Location = new Point(305, 60),
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
                Location = new Point(710, 30),
                Size = new Size(28, 30),
                FlatStyle = FlatStyle.Flat
            };
            Searchbutton.FlatAppearance.BorderColor = Color.White;
            Searchbutton.Click += new EventHandler(Searchbutton_Click);
            panelSearchPage.Controls.Add(Searchbutton);
            Searchbutton.BringToFront();

            //[PL0217]Add lable for past time condition.
            Label labelChangesInPast = new Label()
            {
                Text = "Changes in the past:",
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                Location = new Point(200, 85),
                AutoSize = true

            };
            panelSearchPage.Controls.Add(labelChangesInPast);

            //[PL0217]Add comboBox for past time condition
            ColoredCombo comboBoxChangesInPast = new ColoredCombo()
            {
                Name = "comboBoxChangesInPast",
                Location = new Point(425, 85),
                Font = new Font("Segoe UI", 15),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            comboBoxChangesInPast.Items.Add("24 Hours");
            comboBoxChangesInPast.Items.Add("72 Hours");
            comboBoxChangesInPast.Items.Add("Week");
            comboBoxChangesInPast.Items.Add("Month");
            comboBoxChangesInPast.Items.Add("Year");
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
                Location = new Point(200, 150),
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
                Location = new Point(200, 190),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = "⬛  Recent search term: "
            };
            panelSearchPage.Controls.Add(recentSearchLabel);

            TextBox recentSearchTextBox = new TextBox()
            {
                Name = "recentSearchTextBox",
                ReadOnly = true,
                BorderStyle = 0,
                Location = new Point(421, 190),
                BackColor = Color.White,
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                TabStop = false
            };
            recentSearchTextBox.TextChanged += RecentSearchTextBox_TextChanged;
            panelSearchPage.Controls.Add(recentSearchTextBox);
            recentSearchTextBox.BringToFront();

            Label recentFilesLabel = new Label()
            {
                Name = "recentFilesLabel",
                Location = new Point(200, 230),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = "⬛  Recent Files..."
            };
            panelSearchPage.Controls.Add(recentFilesLabel);

            Label totalFileAndFolderInfoLabel = new Label()
            {
                Name = "totalFileAndFolderInfoLabel",
                Location = new Point(10, 716),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = "There are"
            };
            panelSearchPage.Controls.Add(totalFileAndFolderInfoLabel);

            //[PL0218]Add horizontal line
            Panel useAsHorizontalLine = new Panel()
            {
                Location = new Point(13, 745),
                Size = new Size(900, 1),
                BorderStyle = BorderStyle.FixedSingle,
                Name = "useAsHorizontalLine"
            };
            panelSearchPage.Controls.Add(useAsHorizontalLine);

            //[PL0218]Add file types label
            LinkLabel linkLabelFileType1 = new LinkLabel()
            {
                Name = "linkLabelFileType1",
                AutoSize = true,
                Location = new Point(13, 757),
                Font = new Font("Segoe UI", 12),
                Text = ""
            };
            linkLabelFileType1.LinkClicked += LinkLabelFileType1_LinkClicked;
            panelSearchPage.Controls.Add(linkLabelFileType1);

            LinkLabel linkLabelFileType2 = new LinkLabel()
            {
                Name = "linkLabelFileType2",
                AutoSize = true,
                Location = new Point(13 + 200, 757),
                Font = new Font("Segoe UI", 12),
                Text = ""
            };
            linkLabelFileType2.LinkClicked += LinkLabelFileType2_LinkClicked;
            panelSearchPage.Controls.Add(linkLabelFileType2);

            LinkLabel linkLabelFileType3 = new LinkLabel()
            {
                Name = "linkLabelFileType3",
                AutoSize = true,
                Location = new Point(13 + 370, 757),
                Font = new Font("Segoe UI", 12),
                Text = ""
            };
            linkLabelFileType3.LinkClicked += LinkLabelFileType3_LinkClicked;
            panelSearchPage.Controls.Add(linkLabelFileType3);

            LinkLabel linkLabelFileType4 = new LinkLabel()
            {
                Name = "linkLabelFileType4",
                AutoSize = true,
                Location = new Point(13 + 540, 757),
                Font = new Font("Segoe UI", 12),
                Text = ""
            };
            linkLabelFileType4.LinkClicked += LinkLabelFileType4_LinkClicked;
            panelSearchPage.Controls.Add(linkLabelFileType4);

            DataGridView dataGridView1 = new DataGridView()
            {
                Name = "dataGridView1",
                Size = new Size(900, 435),
                Location = new Point(25, 280),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Font = new Font("Segoe UI", 10),
                ScrollBars = ScrollBars.Both,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false
            };
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12);
            dataGridView1.CellClick += DataGridView1_CellClick;
            dataGridView1.RowTemplate.Height = 30;
            panelSearchPage.Controls.Add(dataGridView1);


            //ProgressBar progressBar = new ProgressBar()
            //{
            //    Name = "progressBar",
            //    Size = new Size(300, 50),
            //    Location = new Point(15, 280),
            //    Font = new Font("Segoe UI", 10),
            //    Value = 0,
            //    Maximum = 1000
            //};
            //panelSearchPage.Controls.Add(progressBar);

            PictureBox pictureBoxLoadingIcon = new PictureBox()
            {
                Name = "pictureBoxLoadingIcon",
                Size = new Size(40, 40),
                Location = new Point(350, 500),
                Image = Image.FromFile(@".\assets\img\loading.gif"),

            };
            panelSearchPage.Controls.Add(pictureBoxLoadingIcon);
            pictureBoxLoadingIcon.BringToFront();

            //[PL0217]Add panel for advanced search
            Panel panelAdvancedSearch = new Panel()
            {
                Location = new Point(200, 76),
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

        private void PanelSearchPage_Click(object sender, EventArgs e)
        {
            closeMenu();
        }

        private void RecentSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox recentSearchTextBox = (TextBox)Controls["panelSearchPage"].Controls["recentSearchTextBox"];
            Size size = TextRenderer.MeasureText(recentSearchTextBox.Text, recentSearchTextBox.Font);
            recentSearchTextBox.Width = size.Width;
            recentSearchTextBox.Height = size.Height;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView1 = (DataGridView)Controls["panelSearchPage"].Controls["dataGridView1"];
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (e.ColumnIndex == 0)
            {
                try
                {
                    Process.Start(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].FormattedValue.ToString());
                }
                catch
                {
                    MessageBox.Show("The folder does not exist.", "Error");
                }
            }
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //[PL0220]This function can be used to check the content when data binding finished.
            //DataGridView dataGridView1 = (DataGridView)Controls["panelSearchPage"].Controls["dataGridView1"];
            //foreach (DataGridViewRow r in dataGridView1.Rows)
            //{
            //        r.Cells["File Name"] = new DataGridViewTextBoxCell();                
            //}
        }

        //private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    Button Searchbutton = (Button)Controls["panelSearchPage"].Controls["Searchbutton"];
        //    Searchbutton.Enabled = true;
        //    PictureBox pictureBoxLoadingIcon = (PictureBox)Controls["panelSearchPage"].Controls["pictureBoxLoadingIcon"];
        //    pictureBoxLoadingIcon.Hide();
        //}

        //private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        //{

        //dataGridView1.ScrollBars = ScrollBars.None;
        //query the xml input file based on the search conditions
        //FileQueryConditions fileQueryConditions = new FileQueryConditions()
        //{
        //    FileName = (string)e.Argument,
        //    DateCreatedTimeFrom = DateTime.Now.AddDays(1),
        //    DateCreatedTimeTo = DateTime.Now
        //};
        //var dOTQueriedFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
        //fillDataGridView(dOTQueriedFiles);
        // }

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
            PictureBox pictureBoxLoadingIcon = (PictureBox)Controls["panelSearchPage"].Controls["pictureBoxLoadingIcon"];
            pictureBoxLoadingIcon.Show();
            pictureBoxLoadingIcon.Update();
            List<DOTFile> dOTOtherFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            //fill out the query results table
            fillDataGridView(dOTOtherFiles);
            pictureBoxLoadingIcon.Hide();
        }

        private void LinkLabelFileType3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileQueryConditions fileQueryConditions = new FileQueryConditions()
            {
                FileName = ".jpg",
                DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                DateCreatedTimeTo = DateTime.Now
            };
            PictureBox pictureBoxLoadingIcon = (PictureBox)Controls["panelSearchPage"].Controls["pictureBoxLoadingIcon"];
            pictureBoxLoadingIcon.Show();
            pictureBoxLoadingIcon.Update();
            List<DOTFile> dOTJPGFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            //fill out the query results table
            fillDataGridView(dOTJPGFiles);
            pictureBoxLoadingIcon.Hide();
        }

        private void LinkLabelFileType2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileQueryConditions fileQueryConditions = new FileQueryConditions()
            {
                FileName = ".tiff",
                DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                DateCreatedTimeTo = DateTime.Now
            };
            PictureBox pictureBoxLoadingIcon = (PictureBox)Controls["panelSearchPage"].Controls["pictureBoxLoadingIcon"];
            pictureBoxLoadingIcon.Show();
            pictureBoxLoadingIcon.Update();
            List<DOTFile> dOTTIFFFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            //fill out the query results table
            fillDataGridView(dOTTIFFFiles);
            pictureBoxLoadingIcon.Hide();

        }

        private void LinkLabelFileType1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileQueryConditions fileQueryConditions = new FileQueryConditions()
            {
                FileName = ".pdf",
                DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                DateCreatedTimeTo = DateTime.Now
            };
            PictureBox pictureBoxLoadingIcon = (PictureBox)Controls["panelSearchPage"].Controls["pictureBoxLoadingIcon"];
            pictureBoxLoadingIcon.Show();
            pictureBoxLoadingIcon.Update();
            List<DOTFile> dOTPDFFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
            //fill out the query results table
            fillDataGridView(dOTPDFFiles);
            pictureBoxLoadingIcon.Hide();
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
                    showPastDaysChanges(DateTime.Now.AddDays(-3));
                    break;
                case 2:
                    showPastDaysChanges(DateTime.Now.AddDays(-7));
                    break;
                case 3:
                    showPastDaysChanges(DateTime.Now.AddDays(-30));
                    break;
                case 4:
                    showPastDaysChanges(DateTime.Now.AddDays(-365));
                    break;
                default:
                    break;
            }
        }

        private void createSingleSetAdditionalItem(String title, String textBoxName, String warning, Point location, bool isMandatory, String textBoxType)
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
            if (isMandatory)
            {
                Label labelRedStar = new Label()
                {
                    Name = title + "RedStar",
                    Font = new Font("Segoe UI", 15),
                    Location = new Point(location.X + labelTitle.Size.Width - 5, location.Y),
                    AutoSize = true,
                    Text = "*",
                    ForeColor = Color.Red
                };
                Controls["panelAddFilePage"].Controls.Add(labelRedStar);
            }
            Label labelWarning = new Label()
            {
                Name = warning,
                Font = new Font("Seravek", 9, FontStyle.Italic),
                AutoSize = true,
                Text = warning
            };

            Controls["panelAddFilePage"].Controls.Add(labelWarning);
            if (textBoxType == "TextBox")
            {
                TextBox textBoxContent = new TextBox()
                {
                    Name = textBoxName,
                    Location = new Point(location.X, location.Y + 35),
                    Font = new Font("Segoe UI", 15),
                    Size = new Size(380, 20),
                    AutoSize = true,
                    Text = ""
                };
                Controls["panelAddFilePage"].Controls.Add(textBoxContent);
                labelWarning.Location = new Point(location.X - 2, textBoxContent.Location.Y + 45);
            }
            if (textBoxType == "ComboBox")
            {
                ColoredCombo textBoxContent = new ColoredCombo()
                {
                    Name = textBoxName,
                    Location = new Point(location.X, location.Y + 35),
                    Font = new Font("Segoe UI", 15),
                    Size = new Size(380, 20),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                Controls["panelAddFilePage"].Controls.Add(textBoxContent);
                labelWarning.Location = new Point(location.X - 2, textBoxContent.Location.Y + 45);
            }

        }

        private void initAddFilePageComponentsSetup()
        {
            //[PL0217]Create a big panel for all "Add File" components.
            Panel panelAddFilePage = createPagePanel("panelAddFilePage");
            panelAddFilePage.Click += PanelAddFilePage_Click;

            int startingXPoint = 50;
            int startingYPoint = 0;
            int textBoxRightMove = 450;
            //[PL0217]Add button for advanced search.
            Button buttonSelectFile = new Button()
            {
                Text = "Select File",
                Font = new Font("Segoe UI", 15),
                Location = new Point(startingXPoint, startingYPoint),
                Size = new Size(120, 40),
                AutoSize = false,
                BackColor = Color.FromArgb(0xcccccc)
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
            createSingleSetAdditionalItem("Section Number", "textBoxSectionNumber", "Please make sure to enter the eight digits section number.", Location = new Point(startingXPoint, startingYPoint + 60 + 30), true, "TextBox");
            TextBox textBoxSectionNumber = (TextBox)Controls["panelAddFilePage"].Controls["textBoxSectionNumber"];
            textBoxSectionNumber.MaxLength = 8;
            textBoxSectionNumber.TextChanged += TextBoxSectionNumber_TextChanged;
            textBoxSectionNumber.KeyPress += TextBoxSectionNumber_KeyPress;
            createSingleSetAdditionalItem("State Road (SR)", "textBoxSR", "Type only the number.", Location = new Point(startingXPoint + textBoxRightMove, startingYPoint + 60 + 30), true, "TextBox");
            TextBox textBoxSR = (TextBox)Controls["panelAddFilePage"].Controls["textBoxSR"];
            textBoxSR.TextChanged += TextBoxSR_TextChanged;
            textBoxSR.KeyPress += TextBoxSR_KeyPress;

            createSingleSetAdditionalItem("Study Type", "comboBoxStudyType", "Select one study type.", Location = new Point(startingXPoint, startingYPoint + 200), true, "ComboBox");
            ColoredCombo comboBoxStudyType = (ColoredCombo)Controls["panelAddFilePage"].Controls["comboBoxStudyType"];
            comboBoxStudyType.Items.Add("Select");
            comboBoxStudyType.Items.Add("Qualitative Assessments");
            comboBoxStudyType.Items.Add("Signal Warrant Analysis");
            comboBoxStudyType.Items.Add("Intersection Analysis");
            comboBoxStudyType.Items.Add("Arterial Analysis");
            comboBoxStudyType.Items.Add("Left Turn Please Warrant Analysis");
            comboBoxStudyType.Items.Add("Composite Studies");
            comboBoxStudyType.Items.Add("Other traffic engineering related studies");
            comboBoxStudyType.Items.Add("Public Involvement");
            comboBoxStudyType.Items.Add("Fatal Crash Reviews");
            comboBoxStudyType.Items.Add("Speed Zone Studies");
            comboBoxStudyType.Items.Add("Technical Memo");
            comboBoxStudyType.SelectedIndex = 0;

            createSingleSetAdditionalItem("Location", "comboBoxLocation", "Select one location type.", Location = new Point(startingXPoint + textBoxRightMove, startingYPoint + 200), true, "ComboBox");
            ColoredCombo comboBoxLocation = (ColoredCombo)Controls["panelAddFilePage"].Controls["comboBoxLocation"];
            comboBoxLocation.Items.Add("Select");
            comboBoxLocation.Items.Add("Intersection");
            comboBoxLocation.Items.Add("Segment");
            comboBoxLocation.SelectedIndex = 0;

            createSingleSetAdditionalItem("Beginning Milepost", "textBoxBeginningMilepost", "Enter only Milepost digits and double the numbers to avoid errors" + Environment.NewLine + "into the system.", Location = new Point(startingXPoint, startingYPoint + 310), true, "TextBox");
            TextBox textBoxBeginningMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxBeginningMilepost"];
            textBoxBeginningMilepost.Size = new Size(170, 20);
            textBoxBeginningMilepost.MaxLength = 6;
            textBoxBeginningMilepost.TextChanged += TextBoxBeginningMilepost_TextChanged;
            textBoxBeginningMilepost.KeyPress += TextBoxBeginningMilepost_KeyPress;

            createSingleSetAdditionalItem("Ending Milepost", "textBoxEndingMilepost", "", Location = new Point(startingXPoint + 210, startingYPoint + 310), true, "TextBox");
            TextBox textBoxEndingMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxEndingMilepost"];
            textBoxEndingMilepost.Size = new Size(170, 20);
            textBoxEndingMilepost.MaxLength = 6;
            textBoxEndingMilepost.TextChanged += TextBoxEndingMilepost_TextChanged;
            textBoxEndingMilepost.KeyPress += TextBoxEndingMilepost_KeyPress;

            createSingleSetAdditionalItem("FM Number", "textBoxFMNumber", "Enter the number, including \"-\".", Location = new Point(startingXPoint + textBoxRightMove, startingYPoint + 310), true, "TextBox");
            TextBox textBoxFMNumber = (TextBox)Controls["panelAddFilePage"].Controls["textBoxFMNumber"];
            textBoxFMNumber.KeyPress += TextBoxFMNumber_KeyPress;

            createSingleSetAdditionalItem("Author", "textBoxAuthor", "Add the author of the report. For instance, consultant company name.", Location = new Point(startingXPoint, startingYPoint + 430), true, "TextBox");
            createSingleSetAdditionalItem("Key words", "textBoxKeyWords", "Add key words improve results in document searches.", Location = new Point(startingXPoint + textBoxRightMove, startingYPoint + 430), false, "TextBox");



            Label labelFileName = new Label()
            {
                Name = "labelFileName",
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                Location = new Point(startingXPoint, startingYPoint + 540),
                AutoSize = false,
                BackColor = ColorTranslator.FromHtml("#f0bfc0"),
                Size = new Size(830, 30),
                Text = "File Name: " + newFileName
            };
            panelAddFilePage.Controls.Add(labelFileName);

            LinkLabel linkLabelFileNameStructure = new LinkLabel()
            {
                Name = "linkLabelFileNameStructure",
                Font = new Font("Seravek", 8, FontStyle.Italic),
                Location = new Point(startingXPoint + 701, startingYPoint + 575),
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

            Label labelFileInfoContent = new Label()
            {
                Name = "labelFileInfoContent",
                Font = new Font("Segoe UI", 11),
                Location = new Point(startingXPoint, startingYPoint + 620),
                AutoSize = false,
                BackColor = ColorTranslator.FromHtml("#f2f2f2"),
                Size = new Size(380, 130),
                Text = "File Info Details..."
            };
            panelAddFilePage.Controls.Add(labelFileInfoContent);

            createSingleSetAdditionalItem("Comments", "textBoxComments", "Add important comments related to the study.", Location = new Point(startingXPoint + textBoxRightMove, startingYPoint + 585), false, "TextBox");
            TextBox textBoxComments = (TextBox)Controls["panelAddFilePage"].Controls["textBoxComments"];
            textBoxComments.Multiline = true;
            textBoxComments.Size = new Size(380, 80);
            Label labelComments = (Label)Controls["panelAddFilePage"].Controls["Add important comments related to the study."];
            labelComments.Location = new Point(textBoxComments.Location.X - 2, textBoxComments.Location.Y + 95);

            Label labelRequiredFieldStar = new Label()
            {
                Name = "labelRequiredFieldStar",
                Font = new Font("Segoe UI", 12),
                Location = new Point(startingXPoint, startingYPoint + 760),
                AutoSize = true,
                ForeColor = Color.Red,
                Text = "*"
            };
            panelAddFilePage.Controls.Add(labelRequiredFieldStar);

            Label labelRequiredField = new Label()
            {
                Name = "labelRequiredField",
                Font = new Font("Segoe UI", 12),
                Location = new Point(startingXPoint + 13, startingYPoint + 760),
                AutoSize = true,
                Text = "Required Field"
            };
            panelAddFilePage.Controls.Add(labelRequiredField);

            Button buttonSave = new Button()
            {
                Name = "buttonSave",
                Font = new Font("Segoe UI", 15),
                Location = new Point(startingXPoint + 712, startingYPoint + 740),
                Size = new Size(120, 40),
                AutoSize = false,
                Text = "Save",
                BackColor = Color.FromArgb(0xcccccc)
            };
            buttonSave.Click += ButtonSave_Click;
            panelAddFilePage.Controls.Add(buttonSave);
        }

        private void PanelAddFilePage_Click(object sender, EventArgs e)
        {
            closeMenu();
        }

        private void TextBoxEndingMilepost_KeyPress(object sender, KeyPressEventArgs e)
        {
            Label labelMilepost = (Label)Controls["panelAddFilePage"].Controls["Enter only Milepost digits and double the numbers to avoid errors" + Environment.NewLine + "into the system."];
            //[0219]Only allow numbers to keyin
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                labelMilepost.ForeColor = Color.Red;
            }
            //// only allow two decimal point
            //else if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -2))
            //{
            //    e.Handled = true;
            //}
            else
            {
                labelMilepost.ForeColor = Color.Black;
            }
        }

        private void TextBoxEndingMilepost_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxEndingMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxEndingMilepost"];

            Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];
            string textBoxEndingMilepostText = textBoxEndingMilepost.Text;

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ":"
                + labelFileName.Text.Split(':')[1].Split('_')[0]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[1]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[2]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[3]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[4]
                + "_" + textBoxEndingMilepostText.Replace(".", "");
        }

        private void TextBoxBeginningMilepost_KeyPress(object sender, KeyPressEventArgs e)
        {
            Label labelMilepost = (Label)Controls["panelAddFilePage"].Controls["Enter only Milepost digits and double the numbers to avoid errors" + Environment.NewLine + "into the system."];
            //[0219]Only allow numbers to keyin
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                labelMilepost.ForeColor = Color.Red;
            }
            //// only allow two decimal point
            //else if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -2))
            //{
            //    e.Handled = true;
            //}
            else
            {
                labelMilepost.ForeColor = Color.Black;
            }
        }

        private void TextBoxBeginningMilepost_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxBeginningMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxBeginningMilepost"];

            Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];
            string textBoxBeginningMilepostText = textBoxBeginningMilepost.Text;

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ":"
                + labelFileName.Text.Split(':')[1].Split('_')[0]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[1]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[2]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[3]
                + "_" + "MP" + textBoxBeginningMilepostText.Replace(".", "")
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[5];
        }

        private void TextBoxFMNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            Label labelFM = (Label)Controls["panelAddFilePage"].Controls["Enter the number, including \"-\"."];
            //[0219]Only allow numbers to keyin
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-'))
            {
                e.Handled = true;
                labelFM.ForeColor = Color.Red;
            }
            else
            {
                labelFM.ForeColor = Color.Black;
            }
        }

        private void TextBoxSR_KeyPress(object sender, KeyPressEventArgs e)
        {

            Label labelSR = (Label)Controls["panelAddFilePage"].Controls["Type only the number."];
            //[0219]Only allow numbers to keyin
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                labelSR.ForeColor = Color.Red;
            }
            else
            {
                labelSR.ForeColor = Color.Black;
            }
        }




        private void TextBoxSR_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxSR = (TextBox)Controls["panelAddFilePage"].Controls["textBoxSR"];

            Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ":"
                + labelFileName.Text.Split(':')[1].Split('_')[0]
                + "_" + "SR" + textBoxSR.Text
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[2]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[3]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[4]
                + "_" + labelFileName.Text.Split(':')[1].Split('_')[5];
        }

        private void TextBoxSectionNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            Label labelSectionNumber = (Label)Controls["panelAddFilePage"].Controls["Please make sure to enter the eight digits section number."];
            //[0219]Only allow numbers to keyin
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                labelSectionNumber.ForeColor = Color.Red;
            }
            else
            {
                labelSectionNumber.ForeColor = Color.Black;
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

        private string getContentFromTextBoxInOnePanel(String panelName, String textBoxName)
        {
            TextBox foundTextBoxName = (TextBox)Controls[panelName].Controls[textBoxName];
            return foundTextBoxName.Text;
        }

        private bool checkIfMandatoryFieldsFilledOut()
        {
            TextBox textBoxSectionNumber = (TextBox)Controls["panelAddFilePage"].Controls["textBoxSectionNumber"];
            TextBox textBoxSR = (TextBox)Controls["panelAddFilePage"].Controls["textBoxSR"];
            TextBox comboBoxStudyType = (TextBox)Controls["panelAddFilePage"].Controls["comboBoxStudyType"];
            TextBox comboBoxLocation = (TextBox)Controls["panelAddFilePage"].Controls["comboBoxLocation"];
            TextBox textBoxBeginningMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxBeginningMilepost"];
            TextBox textBoxEndingMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxEndingMilepost"];
            TextBox textBoxFM = (TextBox)Controls["panelAddFilePage"].Controls["textBoxFM"];
            TextBox textBoxAuthor = (TextBox)Controls["panelAddFilePage"].Controls["textBoxAuthor"];
            if (textBoxSectionNumber.Text == "")
            {
                MessageBox.Show("Section Number is a mandatory field, please fill it out before save.", "Warning");
                return false;
            }
            else if (textBoxSR.Text == "")
            {
                MessageBox.Show("State Road (SR) is a mandatory field, please fill it out before save.", "Warning");
                return false;
            }
            else if (comboBoxStudyType.Text == "")
            {
                MessageBox.Show("Study Type is a mandatory field, please fill it out before save.", "Warning");
                return false;
            }
            else if (comboBoxLocation.Text == "")
            {
                MessageBox.Show("Location is a mandatory field, please fill it out before save.", "Warning");
                return false;
            }
            else if (textBoxBeginningMilepost.Text == "")
            {
                MessageBox.Show("Beginning Milepost is a mandatory field, please fill it out before save.", "Warning");
                return false;
            }
            else if (textBoxEndingMilepost.Text == "")
            {
                MessageBox.Show("Ending Milepost is a mandatory field, please fill it out before save.", "Warning");
                return false;
            }
            else if (textBoxFM.Text == "")
            {
                MessageBox.Show("FM Number is a mandatory field, please fill it out before save.", "Warning");
                return false;
            }
            else if (textBoxAuthor.Text == "")
            {
                MessageBox.Show("Author is a mandatory field, please fill it out before save.", "Warning");
                return false;
            }
            return true;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //[PL0218]Destination folder should read from config file
            string fileToCopy = sSelectedFile;
            string destinationDirectory = ".\\dummyDest\\";

            LinkLabel linkLabelPath = (LinkLabel)Controls["panelAddFilePage"].Controls["linkLabelPath"];
            if (linkLabelPath.Text.Length == 6)
            {
                MessageBox.Show("Please select a file before save.", "Warning");
            }
            else
            {
                //[PL0219]Check if the mandatory fields filled out.
                if (checkIfMandatoryFieldsFilledOut())
                {
                    //File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                    Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];
                    string fileNewName = labelFileName.Text.Split(':')[1].Trim();
                    File.Copy(fileToCopy, destinationDirectory + fileNewName);

                    //[PL0219]Get file additional properties.
                    dOTFileNewSave.SectionNumber = getContentFromTextBoxInOnePanel("panelAddFilePage", "textBoxSectionNumber");
                    dOTFileNewSave.SR = getContentFromTextBoxInOnePanel("panelAddFilePage", "textBoxSR");
                    dOTFileNewSave.StudyType = getContentFromTextBoxInOnePanel("panelAddFilePage", "comboBoxStudyType");
                    dOTFileNewSave.Location = getContentFromTextBoxInOnePanel("panelAddFilePage", "comboBoxLocation");
                    dOTFileNewSave.BeginningMilepost = getContentFromTextBoxInOnePanel("panelAddFilePage", "textBoxBeginningMilepost");
                    dOTFileNewSave.EndingMilepost = getContentFromTextBoxInOnePanel("panelAddFilePage", "textBoxEndingMilepost");
                    dOTFileNewSave.FM = getContentFromTextBoxInOnePanel("panelAddFilePage", "textBoxFMNumber");
                    dOTFileNewSave.Author = getContentFromTextBoxInOnePanel("panelAddFilePage", "textBoxAuthor");
                    dOTFileNewSave.KeyWords = getContentFromTextBoxInOnePanel("panelAddFilePage", "textBoxKeyWords");
                    dOTFileNewSave.Comments = getContentFromTextBoxInOnePanel("panelAddFilePage", "textBoxComments");

                    String XMLInputFile = "./inputFile/outputxml.xml";
                    if (File.Exists(XMLInputFile))
                    {
                        var doc = XDocument.Load(XMLInputFile);

                        var newFileElement = new XElement("row",
                            new XElement("var", new XAttribute("name", "FilePathAndName"), new XAttribute("value", dOTFileNewSave.FilePathAndName)),
                            new XElement("var", new XAttribute("name", "ParentFolder"), new XAttribute("value", dOTFileNewSave.ParentFolder)),
                            new XElement("var", new XAttribute("name", "Name"), new XAttribute("value", fileNewName)),
                            new XElement("var", new XAttribute("name", "DateCreated"), new XAttribute("value", new DateTime())),
                            new XElement("var", new XAttribute("name", "DateLastAccessed"), new XAttribute("value", dOTFileNewSave.DateLastAccessed)),
                            new XElement("var", new XAttribute("name", "DateLastModified"), new XAttribute("value", dOTFileNewSave.DateLastModified)),
                            new XElement("var", new XAttribute("name", "Size"), new XAttribute("value", dOTFileNewSave.Size)),
                            new XElement("var", new XAttribute("name", "Type"), new XAttribute("value", dOTFileNewSave.Type)),
                            new XElement("var", new XAttribute("name", "Suffix"), new XAttribute("value", dOTFileNewSave.Suffix)),
                            new XElement("var", new XAttribute("name", "Owner"), new XAttribute("value", dOTFileNewSave.Owner)),
                            new XElement("var", new XAttribute("name", "SectionNumber"), new XAttribute("value", dOTFileNewSave.SectionNumber)),
                            new XElement("var", new XAttribute("name", "SR"), new XAttribute("value", dOTFileNewSave.SR)),
                            new XElement("var", new XAttribute("name", "StudyType"), new XAttribute("value", dOTFileNewSave.StudyType)),
                            new XElement("var", new XAttribute("name", "Location"), new XAttribute("value", dOTFileNewSave.Location)),
                            new XElement("var", new XAttribute("name", "BeginningMilepost"), new XAttribute("value", dOTFileNewSave.BeginningMilepost)),
                            new XElement("var", new XAttribute("name", "EndingMilepost"), new XAttribute("value", dOTFileNewSave.EndingMilepost)),
                            new XElement("var", new XAttribute("name", "FM"), new XAttribute("value", dOTFileNewSave.FM)),
                            new XElement("var", new XAttribute("name", "Author"), new XAttribute("value", dOTFileNewSave.Author)),
                            new XElement("var", new XAttribute("name", "KeyWords"), new XAttribute("value", dOTFileNewSave.KeyWords)),
                            new XElement("var", new XAttribute("name", "Comments"), new XAttribute("value", dOTFileNewSave.Comments))
                            );
                        doc.Element("root").Add(newFileElement);
                        doc.Save(XMLInputFile);

                        MessageBox.Show("File \"" + fileNewName + "\" is successfully saved.", "Info");
                    }
                    else
                    {
                        //XML does not exist.
                        ///TODO
                    }

                }
            }
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
                string fileSuffix = Path.GetFileName(sSelectedFile).Split('.').Last();
                string numberOfPages = "";
                if (fileSuffix.ToUpper() == "PDF")
                {
                    PdfReader pdfReader = new PdfReader(sSelectedFile);
                    numberOfPages = pdfReader.NumberOfPages.ToString();
                }
                if (fileSuffix.ToUpper() == "DOCX")
                {
                    //// Open a doc file.
                    //var application = new Application();
                    //var document = application.Documents.Open(@"C:\Users\MyName\Documents\word.docx");

                    //// Get the page count.
                    //var numberOfPages = document.ComputeStatistics(WdStatistic.wdStatisticPages, false);

                    //// Close word.
                    //application.Quit();
                }

                labelFileInfoContent.Text = "Date Created: " + fileInfo.CreationTime
                    + Environment.NewLine
                    + "Date Modified: " + fileInfo.LastWriteTime
                    + Environment.NewLine
                    + "Page: " + numberOfPages
                    + Environment.NewLine
                    + "Size: " + getFileSizeHumanReadable(fileInfo.Length)
                    + Environment.NewLine
                    + "Type: " + fileSuffix;

                Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];

                labelFileName.Text = labelFileName.Text.Replace("suffix", fileSuffix);

                ///TODO, [PL0219]Here needs to save the new file path, instead of the old one.
                dOTFileNewSave.FilePathAndName = sSelectedFile;
                dOTFileNewSave.ParentFolder = Path.GetFullPath(sSelectedFile);
                ///TODO, [PL0219]Only get the old fileinfo
                dOTFileNewSave.DateLastAccessed = fileInfo.LastAccessTime.ToString();
                dOTFileNewSave.DateLastModified = fileInfo.LastWriteTime.ToString();
                dOTFileNewSave.Size = getFileSizeHumanReadable(fileInfo.Length);
                dOTFileNewSave.Type = fileInfo.GetType().ToString();
                dOTFileNewSave.Suffix = Path.GetFileName(sSelectedFile).Split('.').Last();
                dOTFileNewSave.Owner = File.GetAccessControl(sSelectedFile).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

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
            Button Searchbutton = (Button)Controls["panelSearchPage"].Controls["Searchbutton"];
            Searchbutton.Enabled = false;

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
                PictureBox pictureBoxLoadingIcon = (PictureBox)Controls["panelSearchPage"].Controls["pictureBoxLoadingIcon"];
                pictureBoxLoadingIcon.Show();
                pictureBoxLoadingIcon.Update();
                warningLabel.Text = "";
                //fill out the query results table
                //fillDataGridView(dOTQueriedFiles);
                //[PL0217]Fill the recent search label.
                //[PL0220]Change it to textbox, otherwise it cannot be selected.
                TextBox recentSearchTextBox = (TextBox)Controls["panelSearchPage"].Controls["recentSearchTextBox"];
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
                    recentSearchTextBox.Text = "";
                    recentSearchTextBox.Text = recentSearchTextBox.Text + "\"" + keywordFirst + "\", \"" + keywordSecond + "\", \"" + keywordThird + "\".";
                }
                else if (keywordFirst != "" && keywordSecond != "")
                {
                    recentSearchTextBox.Text = "";
                    recentSearchTextBox.Text = recentSearchTextBox.Text + "\"" + keywordFirst + "\", \"" + keywordSecond + "\".";
                }
                else if (keywordFirst != "")
                {
                    recentSearchTextBox.Text = "";
                    recentSearchTextBox.Text = recentSearchTextBox.Text + "\"" + keywordFirst + "\".";
                }
                numberOfSearchTerm++;
                DataGridView dataGridView1 = (DataGridView)Controls["panelSearchPage"].Controls["dataGridView1"];
                dataGridView1.DataSource = null;
                //backgroundWorker.RunWorkerAsync(searchCueTextBox.Text);
                FileQueryConditions fileQueryConditions = new FileQueryConditions()
                {
                    FileName = searchCueTextBox.Text,
                    DateCreatedTimeFrom = DateTime.Now.AddDays(1),
                    DateCreatedTimeTo = DateTime.Now
                };
                var dOTQueriedFiles = queryInfoFromXMLInputFile("./inputFile/outputxml.xml", fileQueryConditions);
                fillDataGridView(dOTQueriedFiles);
                dataGridView1.Refresh();
                Searchbutton.Enabled = true;

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
                //pictureBoxLoadingIcon.Visible = false;
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
                //pictureBoxLoadingIcon.Visible = false;
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
            dOTFile.SectionNumber = queryInfoForFileAttribute(fileAttributes, "SectionNumber");
            dOTFile.SR = queryInfoForFileAttribute(fileAttributes, "SR");
            dOTFile.StudyType = queryInfoForFileAttribute(fileAttributes, "StudyType");
            dOTFile.Location = queryInfoForFileAttribute(fileAttributes, "Location");
            dOTFile.BeginningMilepost = queryInfoForFileAttribute(fileAttributes, "BeginningMilepost");
            dOTFile.EndingMilepost = queryInfoForFileAttribute(fileAttributes, "EndingMilepost");
            dOTFile.FM = queryInfoForFileAttribute(fileAttributes, "FM");
            dOTFile.Author = queryInfoForFileAttribute(fileAttributes, "Author");
            dOTFile.KeyWords = queryInfoForFileAttribute(fileAttributes, "KeyWords");
            dOTFile.Comments = queryInfoForFileAttribute(fileAttributes, "Comments");
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
            PictureBox pictureBoxLoadingIcon = (PictureBox)Controls["panelSearchPage"].Controls["pictureBoxLoadingIcon"];
            PictureBox.CheckForIllegalCrossThreadCalls = false;
            pictureBoxLoadingIcon.Visible = true;
            //pictureBoxLoadingIcon.Update();
            var queriedDataSource = dOTQueriedFiles.Select(i => new
            {
                i.Name,
                i.ParentFolder,
                i.Location,
                i.StudyType,
                i.Comments,
                i.KeyWords,
                i.FM,
                i.SR,
                i.Author,
                Ext = (String)i.Name.Split('.').Last()
            }).ToArray();
            DataGridView dataGridView1 = (DataGridView)Controls["panelSearchPage"].Controls["dataGridView1"];
            dataGridView1.Refresh();
            dataGridView1.DataSource = null;

            DataGridViewLinkColumn col = new DataGridViewLinkColumn()
            {
                DataPropertyName = "Name",
                Name = "fileName",
                HeaderText = "File Name",
                TrackVisitedState = true,
                ActiveLinkColor = Color.White,
                VisitedLinkColor = Color.YellowGreen
            };

            dataGridView1.Columns.Add(col);

            //dataGridView1.DataSource = null;
            dataGridView1.DataSource = queriedDataSource;
            if (queriedDataSource.Length > 0)
            {
                dataGridView1.Columns[0].Width = 120;
                dataGridView1.Columns[1].Width = 120;
                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[3].Width = 60;
                dataGridView1.Columns[4].Width = 60;
                dataGridView1.Columns[5].Width = 60;
                dataGridView1.Columns[6].Width = 60;
                dataGridView1.Columns[7].Width = 40;
                dataGridView1.Columns[8].Width = 50;
                dataGridView1.Columns[9].Width = 30;

                dataGridView1.Rows[0].Cells[0].Selected = false;
            }
            //dataGridView1.Columns[0].Visible = false;
            //dataGridView1.DataBind();
            dataGridView1.Refresh();
            pictureBoxLoadingIcon.Visible = false;

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
