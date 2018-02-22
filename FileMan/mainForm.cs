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

        //[PL0222]Set global window width
        int winformWidth = 1160;

        int buttonAdvancedPostionX = 240;
        int buttonAdvancedPostionY = 28;

        //[PL0221]For auto complete in search textbox
        AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
        public mainForm()
        {
            InitializeComponent();
            initComponentsSetup();

            //backgroundWorker.DoWork += BackgroundWorker_DoWork;
            //backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            //this.Size = new Size(816, 854);
            this.Size = new Size(winformWidth, 954);
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
                Location = new Point(10, 14),
                Size = new Size(16, 16),
                BackgroundImage = Image.FromFile(@".\assets\img\menu.png"),
                BackgroundImageLayout = ImageLayout.Stretch,
                //Text = "☰",
                FlatStyle = FlatStyle.Flat
            };
            buttonMenuIcon.Click += ButtonMenuIcon_Click;
            buttonMenuIcon.FlatAppearance.BorderSize = 0;
            buttonMenuIcon.FlatAppearance.BorderColor = Color.White;

            Label labelPageTitle = new Label()
            {
                Name = "labelPageTitle",
                Location = new Point(60, 5),
                Font = new Font("Segoe UI Semilight", 18),
                AutoSize = true,
                Text = "Search"
            };
            panelMenu.Controls.Add(labelPageTitle);

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

            int iconLength = 20;

            Label labelZoomIcon = new Label()
            {
                Name = "labelZoomIcon",
                Size = new Size(iconLength, iconLength),
                BackgroundImage = Image.FromFile(@".\assets\img\menuZoom.png"),
                Location = new Point(50, 68),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            panelMenu.Controls.Add(labelZoomIcon);

            Label labelAddFileIcon = new Label()
            {
                Name = "labelAddFileIcon",
                Size = new Size(iconLength, iconLength),
                BackgroundImage = Image.FromFile(@".\assets\img\menuAddFile.png"),
                Location = new Point(50, 110),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            panelMenu.Controls.Add(labelAddFileIcon);

            Button buttonMenuSearch = new Button()
            {
                Name = "buttonMenuSearch",
                Text = "Search",
                Location = new Point(80, 55),
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
                Location = new Point(80, 100),
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
            labelAddFileIcon.Visible = false;
            labelZoomIcon.Visible = false;

            this.Controls.Add(panelMenu);

            initSearchPageComponentsSetup();
            initAddFilePageComponentsSetup();

            //[PL0217]Setup footer
            Label labelFooter = new Label()
            {
                Text = "About the File Management System | Copyright 2017 \u00A9 FDOT Traffic Operations, District Six",
                Location = new Point(23 + 13, 890),
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

            Label labelPageTitle = (Label)Controls["panelMenu"].Controls["labelPageTitle"];
            labelPageTitle.Text = "Add File";
        }

        private void ButtonMenuSearch_Click(object sender, EventArgs e)
        {
            Panel panelSearchPage = (Panel)Controls["panelSearchPage"];
            Panel panelAddFilePage = (Panel)Controls["panelAddFilePage"];
            panelAddFilePage.Visible = false;
            panelSearchPage.Visible = true;

            closeMenu();

            Label labelPageTitle = (Label)Controls["panelMenu"].Controls["labelPageTitle"];
            labelPageTitle.Text = "Search";

        }

        private void ButtonMenuIcon_Click(object sender, EventArgs e)
        {
            Button buttonMenuSearch = (Button)Controls["panelMenu"].Controls["buttonMenuSearch"];
            Button buttonMenuAddFile = (Button)Controls["panelMenu"].Controls["buttonMenuAddFile"];
            Label labelRedRectangle = (Label)Controls["panelMenu"].Controls["labelRedRectangle"];
            Label labelAddFileIcon = (Label)Controls["panelMenu"].Controls["labelAddFileIcon"];
            Label labelZoomIcon = (Label)Controls["panelMenu"].Controls["labelZoomIcon"];
            Panel panelMenu = (Panel)Controls["panelMenu"];
            if (!isMenuShowing)
            {
                buttonMenuSearch.Visible = true;
                buttonMenuAddFile.Visible = true;
                labelRedRectangle.Visible = true;
                labelAddFileIcon.Visible = true;
                labelZoomIcon.Visible = true;
                panelMenu.Size = new Size(200, 150);
                panelMenu.BackColor = ColorTranslator.FromHtml("#e5e5e5");
                isMenuShowing = true;
            }
            else
            {
                buttonMenuSearch.Visible = false;
                buttonMenuAddFile.Visible = false;
                labelRedRectangle.Visible = false;
                labelAddFileIcon.Visible = false;
                labelZoomIcon.Visible = false;
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
                Size = new Size(winformWidth - 1, 800),
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
                Location = new Point(buttonAdvancedPostionX, buttonAdvancedPostionY),
                Size = new Size(93, 30),
                AutoSize = true,
                FlatStyle = FlatStyle.Flat
            };
            buttonAdvanced.Click += ButtonAdvanced_Click;
            panelSearchPage.Controls.Add(buttonAdvanced);

            //[PL0217]Add cueText search.
            CueTextBox searchCueTextBox = new CueTextBox()
            {
                Name = "searchCueTextBox",
                Cue = "Search",
                Font = new Font("Segoe UI", 15),
                Location = new Point(370, buttonAdvancedPostionY),
                Size = new Size(460, 40),
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
                Location = new Point(searchCueTextBox.Location.X + 430, 30),
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
                Location = new Point(buttonAdvancedPostionX, buttonAdvancedPostionY + 75),
                AutoSize = true

            };
            panelSearchPage.Controls.Add(labelChangesInPast);

            //[PL0217]Add comboBox for past time condition
            ColoredCombo comboBoxChangesInPast = new ColoredCombo()
            {
                Name = "comboBoxChangesInPast",
                Location = new Point(buttonAdvancedPostionX + 220, buttonAdvancedPostionY + 75),
                Font = new Font("Segoe UI", 15),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(135, 40)
            };
            comboBoxChangesInPast.Items.Add("24 Hours");
            comboBoxChangesInPast.Items.Add("72 Hours");
            comboBoxChangesInPast.Items.Add("Week");
            comboBoxChangesInPast.Items.Add("Month");
            comboBoxChangesInPast.Items.Add("Year");
            comboBoxChangesInPast.SelectedIndex = 0;
            comboBoxChangesInPast.SelectedIndexChanged += ComboBoxChangesInPast_SelectedIndexChanged;
            panelSearchPage.Controls.Add(comboBoxChangesInPast);

            int bulletItemStartingPointY = buttonAdvancedPostionY + 146;

            //[PL0217]Add text info for number of files and folders changed.
            Label recentChangeStatisticLabel = new Label()
            {
                Name = "recentChangeStatisticLabel",
                Location = new Point(buttonAdvancedPostionX, bulletItemStartingPointY),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = " "
            };
            panelSearchPage.Controls.Add(recentChangeStatisticLabel);

            Label recentSearchLabel = new Label()
            {
                Name = "recentSearchLabel",
                Location = new Point(buttonAdvancedPostionX, bulletItemStartingPointY + 42),
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
                Location = new Point(buttonAdvancedPostionX + 220, bulletItemStartingPointY + 42),
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
                Location = new Point(buttonAdvancedPostionX, bulletItemStartingPointY + 84),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = "⬛  Recent Files..."
            };
            panelSearchPage.Controls.Add(recentFilesLabel);

            Label totalFileAndFolderInfoLabel = new Label()
            {
                Name = "totalFileAndFolderInfoLabel",
                Location = new Point(10, 700),
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                AutoSize = true,
                Text = "There are"
            };
            panelSearchPage.Controls.Add(totalFileAndFolderInfoLabel);

            //[PL0218]Add horizontal line
            Panel useAsHorizontalLine = new Panel()
            {
                Location = new Point(13, 738),
                Size = new Size(winformWidth - 100, 1),
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
                Size = new Size(winformWidth - 90, 337),
                Location = new Point(13, 340),
                //AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
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
            dataGridView1.RowTemplate.Height = 39;
            panelSearchPage.Controls.Add(dataGridView1);

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }


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
                Location = new Point(500, 500),
                Image = Image.FromFile(@".\assets\img\loading.gif"),

            };
            panelSearchPage.Controls.Add(pictureBoxLoadingIcon);
            pictureBoxLoadingIcon.BringToFront();

            //[PL0217]Add panel for advanced search
            Panel panelAdvancedSearch = new Panel()
            {
                Location = new Point(buttonAdvancedPostionX, 76),
                Size = new Size(590, 400),
                BorderStyle = BorderStyle.FixedSingle,        
                Name = "panelAdvancedSearch",
                BackColor = ColorTranslator.FromHtml("#e3e3e3")

            };
            panelAdvancedSearch.Visible = false;

            int advancedLeftItemPositionX = 25;

            //[PL0218]Add components to advanced search panel
            Label documentTitleLabel = new Label()
            {
                Name = "documentTitleLabel",
                Location = new Point(advancedLeftItemPositionX, 20),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Document Title"
            };
            panelAdvancedSearch.Controls.Add(documentTitleLabel);
            TextBox documentTitleTextBox = new TextBox()
            {
                Name = "documentTitleTextBox",
                Location = new Point(advancedLeftItemPositionX + 4, 40),
                Font = new Font("Segoe UI", 12),
                Size = new Size(520, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(documentTitleTextBox);

            Label fMLabel = new Label()
            {
                Name = "fMLabel",
                Location = new Point(advancedLeftItemPositionX, 80),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Project FM Number (FM)"
            };
            panelAdvancedSearch.Controls.Add(fMLabel);
            TextBox fMTextBox = new TextBox()
            {
                Name = "fMTextBox",
                Location = new Point(advancedLeftItemPositionX + 4, 100),
                Font = new Font("Segoe UI", 12),
                Size = new Size(230, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(fMTextBox);

            Label sRLabel = new Label()
            {
                Name = "sRLabel",
                Location = new Point(315, 80),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "State Road (SR)"
            };
            panelAdvancedSearch.Controls.Add(sRLabel);
            TextBox sRTextBox = new TextBox()
            {
                Name = "sRTextBox",
                Location = new Point(315 + 4, 100),
                Font = new Font("Segoe UI", 12),
                Size = new Size(230, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(sRTextBox);

            Label studyTypeLabel = new Label()
            {
                Name = "studyTypeLabel",
                Location = new Point(advancedLeftItemPositionX, 140),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Study Type"
            };
            panelAdvancedSearch.Controls.Add(studyTypeLabel);
            TextBox studyTypeTextBox = new TextBox()
            {
                Name = "studyType",
                Location = new Point(advancedLeftItemPositionX + 4, 160),
                Font = new Font("Segoe UI", 12),
                Size = new Size(520, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(studyTypeTextBox);

            Label descriptionLabel = new Label()
            {
                Name = "descriptionLabel",
                Location = new Point(advancedLeftItemPositionX, 200),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Description"
            };
            panelAdvancedSearch.Controls.Add(descriptionLabel);
            TextBox descriptionTextBox = new TextBox()
            {
                Name = "descriptionTextBox",
                Location = new Point(advancedLeftItemPositionX + 4, 220),
                Font = new Font("Segoe UI", 12),
                Size = new Size(520, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(descriptionTextBox);

            Label authorLabel = new Label()
            {
                Name = "authorLabel",
                Location = new Point(advancedLeftItemPositionX, 260),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "Author"
            };
            panelAdvancedSearch.Controls.Add(authorLabel);
            TextBox authorTextBox = new TextBox()
            {
                Name = "authorTextBox",
                Location = new Point(advancedLeftItemPositionX + 4, 280),
                Font = new Font("Segoe UI", 12),
                Size = new Size(520, 20),
                AutoSize = true,
                Text = ""
            };
            panelAdvancedSearch.Controls.Add(authorTextBox);

            Label createdTimeFromLabel = new Label()
            {
                Name = "createdTimeFromLabel",
                Location = new Point(advancedLeftItemPositionX, 320),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "From"
            };
            panelAdvancedSearch.Controls.Add(createdTimeFromLabel);

            DateTimePicker createdTimeFromDateTimePicker = new DateTimePicker()
            {
                Name = "createdTimeFromDateTimePicker",
                Location = new Point(advancedLeftItemPositionX + 4, 340),
                Font = new Font("Segoe UI", 12),
                Size = new Size(200, 20)

            };
            createdTimeFromDateTimePicker.Format = DateTimePickerFormat.Custom;
            createdTimeFromDateTimePicker.CustomFormat = "MM/dd/yyyy";
            panelAdvancedSearch.Controls.Add(createdTimeFromDateTimePicker);

            Label createdTimeToLabel = new Label()
            {
                Name = "createdTimeToLabel",
                Location = new Point(345, 320),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Text = "To"
            };
            panelAdvancedSearch.Controls.Add(createdTimeToLabel);
            DateTimePicker createdTimeToDateTimePicker = new DateTimePicker()
            {
                Name = "createdTimeToDateTimePicker",
                Location = new Point(345 + 4, 340),
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
            //[PL0218]Hide advanced search if it is visible.
            Panel panelAdvancedSearch = (Panel)Controls["panelSearchPage"].Controls["panelAdvancedSearch"];
            if (panelAdvancedSearch.Visible == true)
                panelAdvancedSearch.Visible = false;
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
            if (e.RowIndex == -1)  // ignore header row
                return;
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
                Font = new Font("Seravek", 10, FontStyle.Italic),
                AutoSize = true,
                Text = warning
            };

            Controls["panelAddFilePage"].Controls.Add(labelWarning);
            if (textBoxType == "TextBox")
            {
                TextBox textBoxContent = new TextBox()
                {
                    Name = textBoxName,
                    Location = new Point(location.X + 5, location.Y + 35),
                    Font = new Font("Segoe UI", 15),
                    Size = new Size(420, 20),
                    AutoSize = true,
                    Text = ""
                };
                Controls["panelAddFilePage"].Controls.Add(textBoxContent);
                labelWarning.Location = new Point(location.X, textBoxContent.Location.Y + 45);
            }
            if (textBoxType == "ComboBox")
            {
                ColoredCombo textBoxContent = new ColoredCombo()
                {
                    Name = textBoxName,
                    Location = new Point(location.X + 5, location.Y + 35),
                    Font = new Font("Segoe UI", 15),
                    Size = new Size(420, 20),
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

            int startingXPoint = 80;
            int startingYPoint = 0;
            int textBoxRightMove = 520;
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
            buttonSelectFile.FlatAppearance.BorderSize = 0;
            buttonSelectFile.FlatAppearance.BorderColor = this.BackColor;

            buttonSelectFile.Click += ButtonSelectFile_Click;
            panelAddFilePage.Controls.Add(buttonSelectFile);

            LinkLabel linkLabelPath = new LinkLabel()
            {
                Name = "linkLabelPath",
                AutoSize = true,
                Location = new Point(startingXPoint + 180, startingYPoint + 5),
                Font = new Font("Segoe UI", 12),
                Text = "Path: ",
                MaximumSize = new Size(780, 0)

            };
            linkLabelPath.LinkClicked += LinkLabelPath_LinkClicked;
            linkLabelPath.LinkArea = new LinkArea(0,0);
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
            comboBoxStudyType.SelectedIndexChanged += ComboBoxStudyType_SelectedIndexChanged;

            createSingleSetAdditionalItem("Location", "comboBoxLocation", "Select one location type.", Location = new Point(startingXPoint + textBoxRightMove, startingYPoint + 200), true, "ComboBox");
            ColoredCombo comboBoxLocation = (ColoredCombo)Controls["panelAddFilePage"].Controls["comboBoxLocation"];
            comboBoxLocation.Items.Add("Select");
            comboBoxLocation.Items.Add("Intersection");
            comboBoxLocation.Items.Add("Segment");
            comboBoxLocation.SelectedIndex = 0;
            comboBoxLocation.SelectedIndexChanged += ComboBoxLocation_SelectedIndexChanged;

            createSingleSetAdditionalItem("Beginning Milepost", "textBoxBeginningMilepost", "Enter only Milepost digits and double the numbers to avoid errors" + Environment.NewLine + "into the system.", Location = new Point(startingXPoint, startingYPoint + 310), true, "TextBox");
            TextBox textBoxBeginningMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxBeginningMilepost"];
            textBoxBeginningMilepost.Size = new Size(180, 20);
            textBoxBeginningMilepost.MaxLength = 6;
            textBoxBeginningMilepost.TextChanged += TextBoxBeginningMilepost_TextChanged;
            textBoxBeginningMilepost.KeyPress += TextBoxBeginningMilepost_KeyPress;

            createSingleSetAdditionalItem("Ending Milepost", "textBoxEndingMilepost", "", Location = new Point(startingXPoint + 240, startingYPoint + 310), true, "TextBox");
            TextBox textBoxEndingMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxEndingMilepost"];
            textBoxEndingMilepost.Size = new Size(180, 20);
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
                Location = new Point(startingXPoint, startingYPoint + 546),
                AutoSize = false,
                BackColor = ColorTranslator.FromHtml("#f0bfc0"),
                Size = new Size(946, 40),
                Text = "  File Name: " + newFileName,
                TextAlign = ContentAlignment.MiddleLeft
            };
            panelAddFilePage.Controls.Add(labelFileName);

            LinkLabel linkLabelFileNameStructure = new LinkLabel()
            {
                Name = "linkLabelFileNameStructure",
                Font = new Font("Seravek", 8, FontStyle.Italic),
                Location = new Point(startingXPoint + 816, startingYPoint + 589),
                AutoSize = true,
                ForeColor = Color.Red,
                LinkColor = Color.Red,
                Text = "About File Name Structure."
            };
            panelAddFilePage.Controls.Add(linkLabelFileNameStructure);

            Label labelFileInfo = new Label()
            {
                Name = "labelFileInfo",
                Font = new Font("Segoe UI", 15),
                Location = new Point(startingXPoint, startingYPoint + 600),
                AutoSize = true,
                Text = "File Info"
            };
            panelAddFilePage.Controls.Add(labelFileInfo);

            Label labelFileInfoContent = new Label()
            {
                Name = "labelFileInfoContent",
                Font = new Font("Segoe UI", 11),
                Location = new Point(startingXPoint, labelFileInfo.Location.Y + 35),
                AutoSize = false,
                BackColor = ColorTranslator.FromHtml("#f2f2f2"),
                Size = new Size(420, 130),
                Text = " File Info Details..."
            };
            panelAddFilePage.Controls.Add(labelFileInfoContent);

            createSingleSetAdditionalItem("Comments", "textBoxComments", "Add important comments related to the study.", Location = new Point(startingXPoint + textBoxRightMove, startingYPoint + 600), false, "TextBox");
            TextBox textBoxComments = (TextBox)Controls["panelAddFilePage"].Controls["textBoxComments"];
            textBoxComments.Multiline = true;
            textBoxComments.Size = new Size(420, 90);
            Label labelComments = (Label)Controls["panelAddFilePage"].Controls["Add important comments related to the study."];
            labelComments.Location = new Point(textBoxComments.Location.X - 2, textBoxComments.Location.Y + 95);

            Label labelRequiredFieldStar = new Label()
            {
                Name = "labelRequiredFieldStar",
                Font = new Font("Segoe UI", 12),
                Location = new Point(startingXPoint, labelFileInfo.Location.Y + 175),
                AutoSize = true,
                ForeColor = Color.Red,
                Text = "*"
            };
            panelAddFilePage.Controls.Add(labelRequiredFieldStar);

            Label labelRequiredField = new Label()
            {
                Name = "labelRequiredField",
                Font = new Font("Segoe UI", 12),
                Location = new Point(startingXPoint + 13, labelFileInfo.Location.Y + 175),
                AutoSize = true,
                Text = "Required Field"
            };
            panelAddFilePage.Controls.Add(labelRequiredField);

            Button buttonSave = new Button()
            {
                Name = "buttonSave",
                Font = new Font("Segoe UI", 15),
                Location = new Point(startingXPoint + 712 + 114, startingYPoint + 755),
                Size = new Size(120, 40),
                AutoSize = false,
                Text = "Save",
                BackColor = Color.FromArgb(0xcccccc)
            };
            buttonSave.FlatAppearance.BorderSize = 0;
            buttonSave.Click += ButtonSave_Click;
            panelAddFilePage.Controls.Add(buttonSave);
        }

        private void ComboBoxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];
            ColoredCombo ComboBoxLocation = (ColoredCombo)Controls["panelAddFilePage"].Controls["ComboBoxLocation"];
            String location = "";
            switch (ComboBoxLocation.SelectedIndex)
            {
                case 0:
                    location = "";
                    break;
                case 1:
                    location = "Intersection";
                    break;
                case 2:
                    location = "Segment";
                    break;
                default:
                    break;
            }

            newFileName = newFileName.Split('_')[0]
                + "_" + newFileName.Split('_')[1]
                + "_" + newFileName.Split('_')[2]
                + "_" + location
                + "_" + newFileName.Split('_')[4]
                + "_" + newFileName.Split('_')[5];

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ":" + newFileName.Replace(" ", "_");
        }

        private void ComboBoxStudyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label labelFileName = (Label)Controls["panelAddFilePage"].Controls["labelFileName"];
            ColoredCombo ComboBoxStudyType = (ColoredCombo)Controls["panelAddFilePage"].Controls["ComboBoxStudyType"];
            String studyType = "";
            switch (ComboBoxStudyType.SelectedIndex)
            {
                case 0:
                    studyType = "";
                    break;
                case 1:
                    studyType = "Qualitative Assessments";
                    break;
                case 2:
                    studyType = "Signal Warrant Analysis";
                    break;
                case 3:
                    studyType = "Intersection Analysis";
                    break;
                case 4:
                    studyType = "Arterial Analysis";
                    break;
                case 5:
                    studyType = "Left Turn Please Warrant Analysis";
                    break;
                case 6:
                    studyType = "Composite Studies";
                    break;
                case 7:
                    studyType = "Other traffic engineering related_studies";
                    break;
                case 8:
                    studyType = "Public Involvement";
                    break;
                case 9:
                    studyType = "Fatal Crash Reviews";
                    break;
                case 10:
                    studyType = "Speed Zone Studies";
                    break;
                case 11:
                    studyType = "Technical Memo";
                    break;
                default:
                    break;
            }

            newFileName = newFileName.Split('_')[0]
                + "_" + newFileName.Split('_')[1]
                + "_" + studyType
                + "_" + newFileName.Split('_')[3]
                + "_" + newFileName.Split('_')[4]
                + "_" + newFileName.Split('_')[5];

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ":" + newFileName.Replace(" ", "_");
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

            newFileName = newFileName.Split('.')[0].Split('_')[0]
                        + "_" + newFileName.Split('.')[0].Split('_')[1]
                        + "_" + newFileName.Split('.')[0].Split('_')[2]
                        + "_" + newFileName.Split('.')[0].Split('_')[3]
                        + "_" + newFileName.Split('.')[0].Split('_')[4]
                        + "_" + textBoxEndingMilepostText.Replace(".", "")
                        + "." + newFileName.Split('.')[1];

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ":" + newFileName;
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

            newFileName = newFileName.Split('_')[0]
            + "_" + newFileName.Split('_')[1]
            + "_" + newFileName.Split('_')[2]
            + "_" + newFileName.Split('_')[3]
            + "_" + "MP" + textBoxBeginningMilepostText.Replace(".", "")
            + "_" + newFileName.Split('_')[5];

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ":" + newFileName;
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

            newFileName = newFileName.Split('_')[0]
                        + "_" + "SR" + textBoxSR.Text
                        + "_" + newFileName.Split('_')[2]
                        + "_" + newFileName.Split('_')[3]
                        + "_" + newFileName.Split('_')[4]
                        + "_" + newFileName.Split('_')[5];

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ":" + newFileName;

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

            newFileName = textBoxSectionNumber.Text
                        + "_" + newFileName.Split('_')[1]
                        + "_" + newFileName.Split('_')[2]
                        + "_" + newFileName.Split('_')[3]
                        + "_" + newFileName.Split('_')[4]
                        + "_" + newFileName.Split('_')[5];

            labelFileName.Text = labelFileName.Text.Split(':')[0] + ":" + newFileName;
        }

        private string getContentFromTextBoxInOnePanel(String panelName, String textBoxName)
        {
            TextBox foundTextBoxName = (TextBox)Controls[panelName].Controls[textBoxName];
            return foundTextBoxName.Text;
        }

        private string getContentFromComboBoxInOnePanel(String panelName, String comboBoxName)
        {
            ColoredCombo foundComboBoxName = (ColoredCombo)Controls[panelName].Controls[comboBoxName];
            return foundComboBoxName.Text;
        }

        private bool checkIfMandatoryFieldsFilledOut()
        {
            TextBox textBoxSectionNumber = (TextBox)Controls["panelAddFilePage"].Controls["textBoxSectionNumber"];
            TextBox textBoxSR = (TextBox)Controls["panelAddFilePage"].Controls["textBoxSR"];
            ColoredCombo comboBoxStudyType = (ColoredCombo)Controls["panelAddFilePage"].Controls["comboBoxStudyType"];
            ColoredCombo comboBoxLocation = (ColoredCombo)Controls["panelAddFilePage"].Controls["comboBoxLocation"];
            TextBox textBoxBeginningMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxBeginningMilepost"];
            TextBox textBoxEndingMilepost = (TextBox)Controls["panelAddFilePage"].Controls["textBoxEndingMilepost"];
            TextBox textBoxFMNumber = (TextBox)Controls["panelAddFilePage"].Controls["textBoxFMNumber"];
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
            else if (comboBoxStudyType.Text == "Select")
            {
                MessageBox.Show("Study Type is a mandatory field, please select before save.", "Warning");
                return false;
            }
            else if (comboBoxLocation.Text == "Select")
            {
                MessageBox.Show("Location is a mandatory field, please select before save.", "Warning");
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
            else if (textBoxFMNumber.Text == "")
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
                    dOTFileNewSave.StudyType = getContentFromComboBoxInOnePanel("panelAddFilePage", "comboBoxStudyType");
                    dOTFileNewSave.Location = getContentFromComboBoxInOnePanel("panelAddFilePage", "comboBoxLocation");
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
            LinkLabel linkLabelPath = (LinkLabel)Controls["panelAddFilePage"].Controls["linkLabelPath"];
            Process.Start(Path.GetDirectoryName(linkLabelPath.Text.ToString().Substring(6)));
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
                linkLabelPath.LinkArea = new LinkArea(6, linkLabelPath.Text.Length - 1);
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

                newFileName = newFileName.Replace("suffix", fileSuffix);
                labelFileName.Text = labelFileName.Text.Split(':')[0] + ":" + newFileName;


                ///TODO, [PL0219]Here needs to save the new file path, instead of the old one.
                dOTFileNewSave.FilePathAndName = sSelectedFile;
                dOTFileNewSave.ParentFolder = Path.GetDirectoryName(sSelectedFile);
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
                Searchbutton.Enabled = false;
                PictureBox pictureBoxLoadingIcon = (PictureBox)Controls["panelSearchPage"].Controls["pictureBoxLoadingIcon"];
                pictureBoxLoadingIcon.Show();
                pictureBoxLoadingIcon.Update();
                warningLabel.Text = "";

                autoComplete.Add(searchCueTextBox.Text);

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
                    Comments = searchCueTextBox.Text,
                    Keywords = searchCueTextBox.Text,
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
                                            where ((string)cli.Attribute("name").Value == "Name" && cli.Attribute("value").Value.ToUpper().Contains(fileNameKeyWord.ToUpper()) == true)
                                            || ((string)cli.Attribute("name").Value == "Comments" && cli.Attribute("value").Value.ToUpper().Contains(fileNameKeyWord.ToUpper()) == true)
                                            || ((string)cli.Attribute("name").Value == "KeyWords" && cli.Attribute("value").Value.ToUpper().Contains(fileNameKeyWord.ToUpper()) == true)
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
                               where ((string)cli.Attribute("name").Value == "Name" && cli.Attribute("value").Value.ToUpper().Contains(fileNameKeyWord.ToUpper()) == true)
                                    || ((string)cli.Attribute("name").Value == "Comments" && cli.Attribute("value").Value.ToUpper().Contains(fileNameKeyWord.ToUpper()) == true)
                                    || ((string)cli.Attribute("name").Value == "KeyWords" && cli.Attribute("value").Value.ToUpper().Contains(fileNameKeyWord.ToUpper()) == true)
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
            dOTFile.Edit = queryInfoForFileAttribute(fileAttributes, "Edit");
            dOTFile.MP = queryInfoForFileAttribute(fileAttributes, "MP");
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
                i.BeginningMilepost,
                i.FM,
                i.SR,
                i.Author,
                Ext = (String)i.Name.Split('.').Last(),
                i.DateLastModified,
                i.Edit
            }).ToArray();
            DataGridView dataGridView1 = (DataGridView)Controls["panelSearchPage"].Controls["dataGridView1"];
            dataGridView1.Refresh();
            dataGridView1.DataSource = null;

            DataGridViewLinkColumn colLinkName = new DataGridViewLinkColumn()
            {
                DataPropertyName = "Name",
                Name = "fileName",
                HeaderText = "File Name",
                TrackVisitedState = true,
                ActiveLinkColor = Color.White,
                VisitedLinkColor = Color.YellowGreen
            };

            dataGridView1.Columns.Add(colLinkName);

            DataGridViewLinkColumn colEdit = new DataGridViewLinkColumn()
            {
                DataPropertyName = "Edit",
                Name = "colEdit",
                HeaderText = "EDIT",
                TrackVisitedState = true,
                ActiveLinkColor = Color.White,
                VisitedLinkColor = Color.YellowGreen
            };
            dataGridView1.Columns.Add(colEdit);



            dataGridView1.DataSource = queriedDataSource;
            dataGridView1.Columns["colEdit"].DisplayIndex = 12;
            //DataGridViewLinkColumn colEDIT = new DataGridViewLinkColumn()
            //{
            //    Name = "colEDIT",
            //    HeaderText = "EDIT",
            //    TrackVisitedState = true,
            //    ActiveLinkColor = Color.White,
            //    VisitedLinkColor = Color.YellowGreen
            //};

            //dataGridView1.Columns.Add(colEDIT);

            dataGridView1.Columns[1].HeaderText = "Path";
            dataGridView1.Columns[3].HeaderText = "StudyType";
            dataGridView1.Columns[6].HeaderText = "MP";
            dataGridView1.Columns[7].HeaderText = "FM No.";
            dataGridView1.Columns[6].HeaderText = "MP";
            dataGridView1.Columns[11].HeaderText = "DateModified";

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            //dataGridView1.AutoResizeColumns();


            if (queriedDataSource.Length > 0)
            {
                dataGridView1.Columns[0].Width = 100;
                dataGridView1.Columns[1].Width = 100;
                //dataGridView1.Columns[2].Width = 20;
                //dataGridView1.Columns[3].Width = 20;
                //dataGridView1.Columns[4].Width = 20;
                //dataGridView1.Columns[5].Width = 20;
                //dataGridView1.Columns[6].Width = 20;
                //dataGridView1.Columns[7].Width = 20;
                //dataGridView1.Columns[8].Width = 20;
                //dataGridView1.Columns[9].Width = 20;
                dataGridView1.Columns[10].Width = 35;
                //dataGridView1.Columns[11].Width = 20;

                dataGridView1.Rows[0].Cells[0].Selected = false;
            }
            //dataGridView1.Columns[0].Visible = false;
            //dataGridView1.DataBind();
            dataGridView1.Refresh();
            pictureBoxLoadingIcon.Visible = false;

        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            //[PL0221]For auto fill the text in search box
            CueTextBox searchCueTextBox = (CueTextBox)Controls["panelSearchPage"].Controls["searchCueTextBox"];
            searchCueTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            searchCueTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            searchCueTextBox.AutoCompleteCustomSource = autoComplete;
        }

        private void mainForm_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
