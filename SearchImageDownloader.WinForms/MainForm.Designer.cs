namespace SearchImageDownloader.WinForms;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        txtSearch = new TextBox();
        btnSearch = new Button();
        cmbSize = new ComboBox();
        cmbColor = new ComboBox();
        cmbUsageRights = new ComboBox();
        cmbType = new ComboBox();
        cmbTime = new ComboBox();
        lvImages = new ListView();
        imageListLarge = new ImageList(components);
        btnDownload = new Button();
        progressBar = new ProgressBar();
        lblStatus = new Label();
        txtLog = new TextBox();
        gbFilters = new GroupBox();
        gbResults = new GroupBox();
        gbStatus = new GroupBox();
        gbFilters.SuspendLayout();
        gbResults.SuspendLayout();
        gbStatus.SuspendLayout();
        SuspendLayout();
        // 
        // txtSearch
        // 
        txtSearch.Location = new Point(6, 22);
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(250, 23);
        txtSearch.TabIndex = 3;
        // 
        // btnSearch
        // 
        btnSearch.Location = new Point(264, 22);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(75, 23);
        btnSearch.TabIndex = 4;
        btnSearch.Text = "Search";
        // 
        // cmbSize
        // 
        cmbSize.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSize.Items.Clear();
        cmbSize.Items.AddRange(new object[] { "Any size", "Icon", "Small", "Medium", "Large", "X-Large", "XX-Large", "Huge" });
        cmbSize.Location = new Point(354, 22);
        cmbSize.Name = "cmbSize";
        cmbSize.Size = new Size(80, 23);
        cmbSize.TabIndex = 5;
        // 
        // cmbColor
        // 
        cmbColor.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbColor.Items.AddRange(new object[] { "Any color", "Color", "Black and white", "Transparent" });
        cmbColor.Location = new Point(444, 22);
        cmbColor.Name = "cmbColor";
        cmbColor.Size = new Size(80, 23);
        cmbColor.TabIndex = 6;
        // 
        // cmbUsageRights
        // 
        cmbUsageRights.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbUsageRights.Items.AddRange(new object[] { "Any rights", "Labeled for reuse" });
        cmbUsageRights.Location = new Point(534, 22);
        cmbUsageRights.Name = "cmbUsageRights";
        cmbUsageRights.Size = new Size(100, 23);
        cmbUsageRights.TabIndex = 7;
        // 
        // cmbType
        // 
        cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbType.Items.AddRange(new object[] { "Any type", "Photo", "Clipart", "Lineart" });
        cmbType.Location = new Point(644, 22);
        cmbType.Name = "cmbType";
        cmbType.Size = new Size(80, 23);
        cmbType.TabIndex = 8;
        // 
        // cmbTime
        // 
        cmbTime.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTime.Items.AddRange(new object[] { "Any time", "Past 24 hours", "Past week" });
        cmbTime.Location = new Point(734, 22);
        cmbTime.Name = "cmbTime";
        cmbTime.Size = new Size(80, 23);
        cmbTime.TabIndex = 9;
        // 
        // lvImages
        // 
        lvImages.CheckBoxes = false;
        lvImages.Dock = DockStyle.Fill;
        lvImages.LargeImageList = imageListLarge;
        lvImages.Location = new Point(3, 19);
        lvImages.Name = "lvImages";
        lvImages.Size = new Size(1041, 497);
        lvImages.TabIndex = 10;
        lvImages.UseCompatibleStateImageBehavior = false;
        lvImages.MultiSelect = true;
        // 
        // imageListLarge
        // 
        imageListLarge.ColorDepth = ColorDepth.Depth32Bit;
        imageListLarge.ImageSize = new Size(128, 128);
        imageListLarge.TransparentColor = Color.Transparent;
        // 
        // btnDownload
        // 
        btnDownload.Location = new Point(857, 22);
        btnDownload.Name = "btnDownload";
        btnDownload.Size = new Size(155, 23);
        btnDownload.TabIndex = 11;
        btnDownload.Text = "Download Selected";
        // 
        // progressBar
        // 
        progressBar.Location = new Point(6, 121);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(1006, 20);
        progressBar.TabIndex = 1;
        // 
        // lblStatus
        // 
        lblStatus.Location = new Point(6, 22);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(1006, 28);
        lblStatus.TabIndex = 2;
        lblStatus.Text = "Ready";
        // 
        // txtLog
        // 
        txtLog.Location = new Point(6, 53);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Size = new Size(1006, 63);
        txtLog.TabIndex = 0;
        txtLog.WordWrap = false;
        // 
        // gbFilters
        // 
        gbFilters.Controls.Add(txtSearch);
        gbFilters.Controls.Add(cmbTime);
        gbFilters.Controls.Add(cmbType);
        gbFilters.Controls.Add(cmbUsageRights);
        gbFilters.Controls.Add(cmbColor);
        gbFilters.Controls.Add(btnSearch);
        gbFilters.Controls.Add(cmbSize);
        gbFilters.Controls.Add(btnDownload);
        gbFilters.Dock = DockStyle.Top;
        gbFilters.Location = new Point(0, 0);
        gbFilters.Name = "gbFilters";
        gbFilters.Size = new Size(1047, 64);
        gbFilters.TabIndex = 12;
        gbFilters.TabStop = false;
        gbFilters.Text = "Search Filters";
        // Instantiate controls
        numMinFileSize = new NumericUpDown();
        lblMinFileSize = new Label();
        // lblMinFileSize
        lblMinFileSize.Location = new Point(860, 22);
        lblMinFileSize.Size = new Size(90, 23);
        lblMinFileSize.Text = "Min Size (KB):";
        lblMinFileSize.TextAlign = ContentAlignment.MiddleRight;
        lblMinFileSize.Name = "lblMinFileSize";
        // numMinFileSize
        numMinFileSize.Location = new Point(950, 22);
        numMinFileSize.Minimum = 0;
        numMinFileSize.Maximum = 10000;
        numMinFileSize.Value = 200;
        numMinFileSize.Size = new Size(70, 23);
        numMinFileSize.Name = "numMinFileSize";
        numMinFileSize.TabIndex = 12;
        // Add to group box
        gbFilters.Controls.Add(lblMinFileSize);
        gbFilters.Controls.Add(numMinFileSize);
        // 
        // gbResults
        // 
        gbResults.Controls.Add(lvImages);
        gbResults.Dock = DockStyle.Fill;
        gbResults.Location = new Point(0, 64);
        gbResults.Name = "gbResults";
        gbResults.Size = new Size(1047, 519);
        gbResults.TabIndex = 13;
        gbResults.TabStop = false;
        gbResults.Text = "Results";
        // 
        // gbStatus
        // 
        gbStatus.Controls.Add(lblStatus);
        gbStatus.Controls.Add(progressBar);
        gbStatus.Controls.Add(txtLog);
        gbStatus.Dock = DockStyle.Bottom;
        gbStatus.Location = new Point(0, 428);
        gbStatus.Name = "gbStatus";
        gbStatus.Size = new Size(1047, 155);
        gbStatus.TabIndex = 14;
        gbStatus.TabStop = false;
        gbStatus.Text = "Status";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1047, 583);
        Controls.Add(gbStatus);
        Controls.Add(gbResults);
        Controls.Add(gbFilters);
        Name = "MainForm";
        Text = "Google Image Downloader";
        gbFilters.ResumeLayout(false);
        gbFilters.PerformLayout();
        gbResults.ResumeLayout(false);
        gbStatus.ResumeLayout(false);
        gbStatus.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Button btnSearch;
    private System.Windows.Forms.ComboBox cmbSize;
    private System.Windows.Forms.ComboBox cmbColor;
    private System.Windows.Forms.ComboBox cmbUsageRights;
    private System.Windows.Forms.ComboBox cmbType;
    private System.Windows.Forms.ComboBox cmbTime;
    private System.Windows.Forms.ListView lvImages;
    private System.Windows.Forms.Button btnDownload;
    private System.Windows.Forms.ImageList imageListLarge;
    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.TextBox txtLog;
    private GroupBox gbFilters;
    private GroupBox gbResults;
    private GroupBox gbStatus;
    private System.Windows.Forms.NumericUpDown numMinFileSize;
    private System.Windows.Forms.Label lblMinFileSize;
}
