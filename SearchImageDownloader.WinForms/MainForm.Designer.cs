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
        groupBox1 = new GroupBox();
        groupBox2 = new GroupBox();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
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
        cmbSize.Items.AddRange(new object[] { "Any size", "Large", "Medium", "Icon" });
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
        lvImages.CheckBoxes = true;
        lvImages.Dock = DockStyle.Fill;
        lvImages.LargeImageList = imageListLarge;
        lvImages.Location = new Point(3, 19);
        lvImages.Name = "lvImages";
        lvImages.Size = new Size(1017, 306);
        lvImages.TabIndex = 10;
        lvImages.UseCompatibleStateImageBehavior = false;
        // 
        // imageListLarge
        // 
        imageListLarge.ColorDepth = ColorDepth.Depth32Bit;
        imageListLarge.ImageSize = new Size(128, 128);
        imageListLarge.TransparentColor = Color.Transparent;
        // 
        // btnDownload
        // 
        btnDownload.Location = new Point(12, 516);
        btnDownload.Name = "btnDownload";
        btnDownload.Size = new Size(214, 30);
        btnDownload.TabIndex = 11;
        btnDownload.Text = "Download Selected";
        // 
        // progressBar
        // 
        progressBar.Location = new Point(18, 552);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(1020, 20);
        progressBar.TabIndex = 1;
        // 
        // lblStatus
        // 
        lblStatus.Location = new Point(12, 415);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(214, 98);
        lblStatus.TabIndex = 2;
        lblStatus.Text = "Ready";
        // 
        // txtLog
        // 
        txtLog.Location = new Point(232, 416);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Size = new Size(800, 130);
        txtLog.TabIndex = 0;
        txtLog.WordWrap = false;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(txtSearch);
        groupBox1.Controls.Add(cmbTime);
        groupBox1.Controls.Add(cmbType);
        groupBox1.Controls.Add(cmbUsageRights);
        groupBox1.Controls.Add(cmbColor);
        groupBox1.Controls.Add(btnSearch);
        groupBox1.Controls.Add(cmbSize);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(830, 64);
        groupBox1.TabIndex = 12;
        groupBox1.TabStop = false;
        groupBox1.Text = "Search Filters";
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(lvImages);
        groupBox2.Location = new Point(12, 82);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(1023, 328);
        groupBox2.TabIndex = 13;
        groupBox2.TabStop = false;
        groupBox2.Text = "Results";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1047, 583);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Controls.Add(txtLog);
        Controls.Add(progressBar);
        Controls.Add(lblStatus);
        Controls.Add(btnDownload);
        Name = "MainForm";
        Text = "Google Image Downloader";
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox2.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
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
    private GroupBox groupBox1;
    private GroupBox groupBox2;
}
