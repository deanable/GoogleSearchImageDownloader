namespace GoogleImageDownloader.WinForms;

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
        this.components = new System.ComponentModel.Container();
        this.txtSearch = new System.Windows.Forms.TextBox();
        this.btnSearch = new System.Windows.Forms.Button();
        this.cmbSize = new System.Windows.Forms.ComboBox();
        this.cmbColor = new System.Windows.Forms.ComboBox();
        this.cmbUsageRights = new System.Windows.Forms.ComboBox();
        this.cmbType = new System.Windows.Forms.ComboBox();
        this.cmbTime = new System.Windows.Forms.ComboBox();
        this.lvImages = new System.Windows.Forms.ListView();
        this.btnDownload = new System.Windows.Forms.Button();
        this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.lblStatus = new System.Windows.Forms.Label();
        this.txtLog = new System.Windows.Forms.TextBox();
        this.SuspendLayout();
        // 
        // txtSearch
        // 
        this.txtSearch.Location = new System.Drawing.Point(12, 12);
        this.txtSearch.Size = new System.Drawing.Size(250, 23);
        // 
        // btnSearch
        // 
        this.btnSearch.Location = new System.Drawing.Point(270, 12);
        this.btnSearch.Size = new System.Drawing.Size(75, 23);
        this.btnSearch.Text = "Search";
        // 
        // cmbSize
        // 
        this.cmbSize.Location = new System.Drawing.Point(360, 12);
        this.cmbSize.Size = new System.Drawing.Size(80, 23);
        this.cmbSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbSize.Items.AddRange(new object[] {"Any size", "Large", "Medium", "Icon"});
        // 
        // cmbColor
        // 
        this.cmbColor.Location = new System.Drawing.Point(450, 12);
        this.cmbColor.Size = new System.Drawing.Size(80, 23);
        this.cmbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbColor.Items.AddRange(new object[] {"Any color", "Color", "Black and white", "Transparent"});
        // 
        // cmbUsageRights
        // 
        this.cmbUsageRights.Location = new System.Drawing.Point(540, 12);
        this.cmbUsageRights.Size = new System.Drawing.Size(100, 23);
        this.cmbUsageRights.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbUsageRights.Items.AddRange(new object[] {"Any rights", "Labeled for reuse"});
        // 
        // cmbType
        // 
        this.cmbType.Location = new System.Drawing.Point(650, 12);
        this.cmbType.Size = new System.Drawing.Size(80, 23);
        this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbType.Items.AddRange(new object[] {"Any type", "Photo", "Clipart", "Lineart"});
        // 
        // cmbTime
        // 
        this.cmbTime.Location = new System.Drawing.Point(740, 12);
        this.cmbTime.Size = new System.Drawing.Size(80, 23);
        this.cmbTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbTime.Items.AddRange(new object[] {"Any time", "Past 24 hours", "Past week"});
        // 
        // lvImages
        // 
        this.lvImages.Location = new System.Drawing.Point(12, 50);
        this.lvImages.Size = new System.Drawing.Size(760, 330);
        this.lvImages.View = System.Windows.Forms.View.LargeIcon;
        this.lvImages.CheckBoxes = true;
        this.lvImages.LargeImageList = this.imageListLarge;
        // 
        // btnDownload
        // 
        this.btnDownload.Location = new System.Drawing.Point(12, 440);
        this.btnDownload.Size = new System.Drawing.Size(120, 30);
        this.btnDownload.Text = "Download Selected";
        // 
        // imageListLarge
        // 
        this.imageListLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
        this.imageListLarge.ImageSize = new System.Drawing.Size(128, 128);
        // 
        // progressBar
        // 
        this.progressBar.Location = new System.Drawing.Point(12, 390);
        this.progressBar.Size = new System.Drawing.Size(760, 20);
        this.progressBar.Minimum = 0;
        this.progressBar.Maximum = 100;
        this.progressBar.Value = 0;
        // 
        // lblStatus
        // 
        this.lblStatus.Location = new System.Drawing.Point(12, 415);
        this.lblStatus.Size = new System.Drawing.Size(760, 20);
        this.lblStatus.Text = "Ready";
        // 
        // txtLog
        // 
        this.txtLog.Location = new System.Drawing.Point(12, 480);
        this.txtLog.Size = new System.Drawing.Size(800, 100);
        this.txtLog.Multiline = true;
        this.txtLog.ReadOnly = true;
        this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtLog.WordWrap = false;
        // 
        // Form1
        // 
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(840, 600);
        this.Controls.Add(this.txtLog);
        this.Controls.Add(this.progressBar);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.txtSearch);
        this.Controls.Add(this.btnSearch);
        this.Controls.Add(this.cmbSize);
        this.Controls.Add(this.cmbColor);
        this.Controls.Add(this.cmbUsageRights);
        this.Controls.Add(this.cmbType);
        this.Controls.Add(this.cmbTime);
        this.Controls.Add(this.lvImages);
        this.Controls.Add(this.btnDownload);
        this.Text = "Google Image Downloader";
        this.ResumeLayout(false);
        this.PerformLayout();
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
}
