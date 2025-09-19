namespace PayrollSystem
{
    partial class frmPayrollGeneration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelPeriod = new System.Windows.Forms.Panel();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblPeriodInfo = new System.Windows.Forms.Label();
            this.panelEmployees = new System.Windows.Forms.Panel();
            this.lblEmployeesTitle = new System.Windows.Forms.Label();
            this.dgvEmployees = new System.Windows.Forms.DataGridView();
            this.panelPayroll = new System.Windows.Forms.Panel();
            this.lblPayrollTitle = new System.Windows.Forms.Label();
            this.dgvPayroll = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblSummaryTitle = new System.Windows.Forms.Label();
            this.lblTotalEmployees = new System.Windows.Forms.Label();
            this.lblTotalGross = new System.Windows.Forms.Label();
            this.lblTotalDeductions = new System.Windows.Forms.Label();
            this.lblTotalNet = new System.Windows.Forms.Label();

            this.panelHeader.SuspendLayout();
            this.panelPeriod.SuspendLayout();
            this.panelEmployees.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).BeginInit();
            this.panelPayroll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayroll)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.SuspendLayout();

            // Form settings
            this.Text = "Payroll Generation";
            this.Size = new System.Drawing.Size(1200, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // Panel Header
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 50;
            this.panelHeader.BackColor = System.Drawing.Color.SteelBlue;
            
            // Title Label
            this.lblTitle.Text = "PAYROLL GENERATION";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // Add title to header panel
            this.panelHeader.Controls.Add(this.lblTitle);

            // Controls setup
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.panelHeader,
                this.panelPeriod,
                this.panelEmployees,
                this.panelPayroll,
                this.panelSummary,
                this.panelButtons,
                this.progressBar,
                this.lblProgress
            });
            
            // Panel Period
            this.panelPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPeriod.Location = new System.Drawing.Point(0, 50);
            this.panelPeriod.Size = new System.Drawing.Size(1184, 70);
            this.panelPeriod.Padding = new System.Windows.Forms.Padding(10);
            
            // Period Label and ComboBox
            this.lblPeriod.Text = "Select Payroll Period:";
            this.lblPeriod.Location = new System.Drawing.Point(15, 15);
            this.lblPeriod.AutoSize = true;
            
            this.cmbPeriod.Location = new System.Drawing.Point(150, 12);
            this.cmbPeriod.Size = new System.Drawing.Size(400, 25);
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            
            this.lblPeriodInfo.Location = new System.Drawing.Point(15, 40);
            this.lblPeriodInfo.AutoSize = true;
            this.lblPeriodInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            
            // Add controls to period panel
            this.panelPeriod.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblPeriod,
                this.cmbPeriod,
                this.lblPeriodInfo
            });
            
            // Panel Employees
            this.panelEmployees.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEmployees.Location = new System.Drawing.Point(0, 120);
            this.panelEmployees.Size = new System.Drawing.Size(1184, 200);
            this.panelEmployees.Padding = new System.Windows.Forms.Padding(10);
            
            // Employees Title
            this.lblEmployeesTitle.Text = "Active Employees";
            this.lblEmployeesTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEmployeesTitle.Location = new System.Drawing.Point(15, 5);
            this.lblEmployeesTitle.AutoSize = true;
            
            // Employees DataGridView
            this.dgvEmployees.Location = new System.Drawing.Point(15, 30);
            this.dgvEmployees.Size = new System.Drawing.Size(1154, 160);
            this.dgvEmployees.AllowUserToAddRows = false;
            this.dgvEmployees.AllowUserToDeleteRows = false;
            this.dgvEmployees.ReadOnly = true;
            this.dgvEmployees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEmployees.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            
            // Add controls to employees panel
            this.panelEmployees.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblEmployeesTitle,
                this.dgvEmployees
            });

            // Panel Payroll
            this.panelPayroll.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPayroll.Location = new System.Drawing.Point(0, 320);
            this.panelPayroll.Size = new System.Drawing.Size(1184, 200);
            this.panelPayroll.Padding = new System.Windows.Forms.Padding(10);
            
            // Payroll Title
            this.lblPayrollTitle.Text = "Payroll Details";
            this.lblPayrollTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPayrollTitle.Location = new System.Drawing.Point(15, 5);
            this.lblPayrollTitle.AutoSize = true;
            
            // Payroll DataGridView
            this.dgvPayroll.Location = new System.Drawing.Point(15, 30);
            this.dgvPayroll.Size = new System.Drawing.Size(1154, 160);
            this.dgvPayroll.AllowUserToAddRows = false;
            this.dgvPayroll.AllowUserToDeleteRows = false;
            this.dgvPayroll.ReadOnly = true;
            this.dgvPayroll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPayroll.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            
            // Add controls to payroll panel
            this.panelPayroll.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblPayrollTitle,
                this.dgvPayroll
            });
            
            // Panel Summary
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 520);
            this.panelSummary.Size = new System.Drawing.Size(1184, 80);
            this.panelSummary.Padding = new System.Windows.Forms.Padding(10);
            
            // Summary Title
            this.lblSummaryTitle.Text = "Summary";
            this.lblSummaryTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummaryTitle.Location = new System.Drawing.Point(15, 5);
            this.lblSummaryTitle.AutoSize = true;
            
            // Summary Labels
            this.lblTotalEmployees.Text = "Total Employees: 0";
            this.lblTotalEmployees.Location = new System.Drawing.Point(15, 30);
            this.lblTotalEmployees.AutoSize = true;
            
            this.lblTotalGross.Text = "Total Gross: ₱0.00";
            this.lblTotalGross.Location = new System.Drawing.Point(15, 50);
            this.lblTotalGross.AutoSize = true;
            
            this.lblTotalDeductions.Text = "Total Deductions: ₱0.00";
            this.lblTotalDeductions.Location = new System.Drawing.Point(300, 30);
            this.lblTotalDeductions.AutoSize = true;
            
            this.lblTotalNet.Text = "Total Net: ₱0.00";
            this.lblTotalNet.Location = new System.Drawing.Point(300, 50);
            this.lblTotalNet.AutoSize = true;
            
            // Add controls to summary panel
            this.panelSummary.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblSummaryTitle,
                this.lblTotalEmployees,
                this.lblTotalGross,
                this.lblTotalDeductions,
                this.lblTotalNet
            });
            
            // Panel Buttons
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 600);
            this.panelButtons.Size = new System.Drawing.Size(1184, 60);
            this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
            
            // Buttons
            this.btnGenerate.Text = "Generate Payroll";
            this.btnGenerate.Location = new System.Drawing.Point(15, 15);
            this.btnGenerate.Size = new System.Drawing.Size(150, 30);
            
            this.btnCalculate.Text = "Recalculate";
            this.btnCalculate.Location = new System.Drawing.Point(175, 15);
            this.btnCalculate.Size = new System.Drawing.Size(150, 30);
            
            this.btnPreview.Text = "Preview";
            this.btnPreview.Location = new System.Drawing.Point(335, 15);
            this.btnPreview.Size = new System.Drawing.Size(150, 30);
            
            this.btnExport.Text = "Export to Excel";
            this.btnExport.Location = new System.Drawing.Point(495, 15);
            this.btnExport.Size = new System.Drawing.Size(150, 30);
            
            this.btnClose.Text = "Close";
            this.btnClose.Location = new System.Drawing.Point(1019, 15);
            this.btnClose.Size = new System.Drawing.Size(150, 30);
            
            // Progress Bar and Label
            this.progressBar.Location = new System.Drawing.Point(655, 15);
            this.progressBar.Size = new System.Drawing.Size(350, 30);
            this.progressBar.Visible = false;
            
            this.lblProgress.Location = new System.Drawing.Point(655, 45);
            this.lblProgress.AutoSize = true;
            this.lblProgress.Text = "";
            
            // Add controls to buttons panel
            this.panelButtons.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnGenerate,
                this.btnCalculate,
                this.btnPreview,
                this.btnExport,
                this.btnClose,
                this.progressBar,
                this.lblProgress
            });
            
            // Event handlers
            this.Load += new System.EventHandler(this.FrmPayrollGeneration_Load);
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.CmbPeriod_SelectedIndexChanged);
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            this.btnCalculate.Click += new System.EventHandler(this.BtnCalculate_Click);
            this.btnPreview.Click += new System.EventHandler(this.BtnPreview_Click);
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelPeriod.ResumeLayout(false);
            this.panelPeriod.PerformLayout();
            this.panelEmployees.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).EndInit();
            this.panelPayroll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayroll)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.Label lblPeriodInfo;
        private System.Windows.Forms.Panel panelEmployees;
        private System.Windows.Forms.Label lblEmployeesTitle;
        private System.Windows.Forms.DataGridView dgvEmployees;
        private System.Windows.Forms.Panel panelPayroll;
        private System.Windows.Forms.Label lblPayrollTitle;
        private System.Windows.Forms.DataGridView dgvPayroll;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblSummaryTitle;
        private System.Windows.Forms.Label lblTotalEmployees;
        private System.Windows.Forms.Label lblTotalGross;
        private System.Windows.Forms.Label lblTotalDeductions;
        private System.Windows.Forms.Label lblTotalNet;
    }
}