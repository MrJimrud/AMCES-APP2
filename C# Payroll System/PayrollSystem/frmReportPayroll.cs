using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PayrollSystem
{
    public partial class frmReportPayroll : Form
    {
        private Panel pnlTop;
        private Panel pnlMain;
        private Panel pnlFilters;
        private GroupBox grpFilters;
        private Label lblPayrollPeriod;
        private ComboBox cmbPayrollPeriod;
        private Label lblEmployee;
        private ComboBox cmbEmployee;
        private Label lblDepartment;
        private ComboBox cmbDepartment;
        private Label lblReportType;
        private ComboBox cmbReportType;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private CheckBox chkIncludeDeductions;
        private CheckBox chkIncludeAllowances;
        private CheckBox chkIncludeTax;
        private Button btnGenerate;
        private Button btnExport;
        private Button btnPrint;
        private Button btnClose;
        private ReportViewer reportViewer;
        private ProgressBar progressBar;
        private Label lblStatusMsg;

        public frmReportPayroll()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeComponent()
        {
            this.pnlTop = new Panel();
            this.pnlMain = new Panel();
            this.pnlFilters = new Panel();
            this.grpFilters = new GroupBox();
            this.lblPayrollPeriod = new Label();
            this.cmbPayrollPeriod = new ComboBox();
            this.lblEmployee = new Label();
            this.cmbEmployee = new ComboBox();
            this.lblDepartment = new Label();
            this.cmbDepartment = new ComboBox();
            this.lblReportType = new Label();
            this.cmbReportType = new ComboBox();
            this.lblStatus = new Label();
            this.cmbStatus = new ComboBox();
            this.chkIncludeDeductions = new CheckBox();
            this.chkIncludeAllowances = new CheckBox();
            this.chkIncludeTax = new CheckBox();
            this.btnGenerate = new Button();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.btnClose = new Button();
            this.reportViewer = new ReportViewer();
            this.progressBar = new ProgressBar();
            this.lblStatusMsg = new Label();

            this.pnlTop.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.grpFilters.SuspendLayout();
            this.SuspendLayout();

            // pnlTop
            this.pnlTop.BackColor = Color.FromArgb(52, 73, 94);
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Controls.Add(this.btnPrint);
            this.pnlTop.Controls.Add(this.btnExport);
            this.pnlTop.Controls.Add(this.btnGenerate);
            this.pnlTop.Dock = DockStyle.Top;
            this.pnlTop.Location = new Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new Size(1200, 60);
            this.pnlTop.TabIndex = 0;

            // pnlFilters
            this.pnlFilters.BackColor = Color.White;
            this.pnlFilters.Controls.Add(this.grpFilters);
            this.pnlFilters.Dock = DockStyle.Top;
            this.pnlFilters.Location = new Point(0, 60);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new Size(1200, 140);
            this.pnlFilters.TabIndex = 1;

            // grpFilters
            this.grpFilters.Controls.Add(this.chkIncludeTax);
            this.grpFilters.Controls.Add(this.chkIncludeAllowances);
            this.grpFilters.Controls.Add(this.chkIncludeDeductions);
            this.grpFilters.Controls.Add(this.cmbStatus);
            this.grpFilters.Controls.Add(this.lblStatus);
            this.grpFilters.Controls.Add(this.cmbReportType);
            this.grpFilters.Controls.Add(this.lblReportType);
            this.grpFilters.Controls.Add(this.cmbDepartment);
            this.grpFilters.Controls.Add(this.lblDepartment);
            this.grpFilters.Controls.Add(this.cmbEmployee);
            this.grpFilters.Controls.Add(this.lblEmployee);
            this.grpFilters.Controls.Add(this.cmbPayrollPeriod);
            this.grpFilters.Controls.Add(this.lblPayrollPeriod);
            this.grpFilters.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpFilters.Location = new Point(12, 10);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Size = new Size(1176, 120);
            this.grpFilters.TabIndex = 0;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "Report Filters";

            // lblPayrollPeriod
            this.lblPayrollPeriod.AutoSize = true;
            this.lblPayrollPeriod.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblPayrollPeriod.Location = new Point(15, 30);
            this.lblPayrollPeriod.Name = "lblPayrollPeriod";
            this.lblPayrollPeriod.Size = new Size(87, 15);
            this.lblPayrollPeriod.TabIndex = 0;
            this.lblPayrollPeriod.Text = "Payroll Period:";

            // cmbPayrollPeriod
            this.cmbPayrollPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPayrollPeriod.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbPayrollPeriod.FormattingEnabled = true;
            this.cmbPayrollPeriod.Location = new Point(15, 50);
            this.cmbPayrollPeriod.Name = "cmbPayrollPeriod";
            this.cmbPayrollPeriod.Size = new Size(200, 23);
            this.cmbPayrollPeriod.TabIndex = 1;

            // lblEmployee
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblEmployee.Location = new Point(230, 30);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new Size(65, 15);
            this.lblEmployee.TabIndex = 2;
            this.lblEmployee.Text = "Employee:";

            // cmbEmployee
            this.cmbEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEmployee.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbEmployee.FormattingEnabled = true;
            this.cmbEmployee.Location = new Point(230, 50);
            this.cmbEmployee.Name = "cmbEmployee";
            this.cmbEmployee.Size = new Size(200, 23);
            this.cmbEmployee.TabIndex = 3;

            // lblDepartment
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblDepartment.Location = new Point(445, 30);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new Size(73, 15);
            this.lblDepartment.TabIndex = 4;
            this.lblDepartment.Text = "Department:";

            // cmbDepartment
            this.cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDepartment.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new Point(445, 50);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new Size(150, 23);
            this.cmbDepartment.TabIndex = 5;
            this.cmbDepartment.SelectedIndexChanged += new EventHandler(this.cmbDepartment_SelectedIndexChanged);

            // lblReportType
            this.lblReportType.AutoSize = true;
            this.lblReportType.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblReportType.Location = new Point(610, 30);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new Size(77, 15);
            this.lblReportType.TabIndex = 6;
            this.lblReportType.Text = "Report Type:";

            // cmbReportType
            this.cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbReportType.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Location = new Point(610, 50);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new Size(150, 23);
            this.cmbReportType.TabIndex = 7;

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblStatus.Location = new Point(775, 30);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(42, 15);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Status:";

            // cmbStatus
            this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new Point(775, 50);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new Size(120, 23);
            this.cmbStatus.TabIndex = 9;

            // chkIncludeDeductions
            this.chkIncludeDeductions.AutoSize = true;
            this.chkIncludeDeductions.Checked = true;
            this.chkIncludeDeductions.CheckState = CheckState.Checked;
            this.chkIncludeDeductions.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkIncludeDeductions.Location = new Point(15, 85);
            this.chkIncludeDeductions.Name = "chkIncludeDeductions";
            this.chkIncludeDeductions.Size = new Size(128, 19);
            this.chkIncludeDeductions.TabIndex = 10;
            this.chkIncludeDeductions.Text = "Include Deductions";
            this.chkIncludeDeductions.UseVisualStyleBackColor = true;

            // chkIncludeAllowances
            this.chkIncludeAllowances.AutoSize = true;
            this.chkIncludeAllowances.Checked = true;
            this.chkIncludeAllowances.CheckState = CheckState.Checked;
            this.chkIncludeAllowances.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkIncludeAllowances.Location = new Point(160, 85);
            this.chkIncludeAllowances.Name = "chkIncludeAllowances";
            this.chkIncludeAllowances.Size = new Size(127, 19);
            this.chkIncludeAllowances.TabIndex = 11;
            this.chkIncludeAllowances.Text = "Include Allowances";
            this.chkIncludeAllowances.UseVisualStyleBackColor = true;

            // chkIncludeTax
            this.chkIncludeTax.AutoSize = true;
            this.chkIncludeTax.Checked = true;
            this.chkIncludeTax.CheckState = CheckState.Checked;
            this.chkIncludeTax.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkIncludeTax.Location = new Point(305, 85);
            this.chkIncludeTax.Name = "chkIncludeTax";
            this.chkIncludeTax.Size = new Size(88, 19);
            this.chkIncludeTax.TabIndex = 12;
            this.chkIncludeTax.Text = "Include Tax";
            this.chkIncludeTax.UseVisualStyleBackColor = true;

            // btnGenerate
            this.btnGenerate.BackColor = Color.FromArgb(46, 204, 113);
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.FlatStyle = FlatStyle.Flat;
            this.btnGenerate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnGenerate.ForeColor = Color.White;
            this.btnGenerate.Location = new Point(12, 15);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new Size(100, 30);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new EventHandler(this.btnGenerate_Click);

            // btnExport
            this.btnExport.BackColor = Color.FromArgb(52, 152, 219);
            this.btnExport.Enabled = false;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.Location = new Point(125, 15);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(100, 30);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);

            // btnPrint
            this.btnPrint.BackColor = Color.FromArgb(155, 89, 182);
            this.btnPrint.Enabled = false;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.Location = new Point(238, 15);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(100, 30);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);

            // btnClose
            this.btnClose.BackColor = Color.FromArgb(231, 76, 60);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(1088, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(100, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // pnlMain
            this.pnlMain.Controls.Add(this.lblStatusMsg);
            this.pnlMain.Controls.Add(this.progressBar);
            this.pnlMain.Controls.Add(this.reportViewer);
            this.pnlMain.Dock = DockStyle.Fill;
            this.pnlMain.Location = new Point(0, 200);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new Size(1200, 500);
            this.pnlMain.TabIndex = 2;

            // reportViewer
            this.reportViewer.Dock = DockStyle.Fill;
            this.reportViewer.Location = new Point(0, 0);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ServerReport.BearerToken = null;
            this.reportViewer.Size = new Size(1200, 500);
            this.reportViewer.TabIndex = 0;

            // progressBar
            this.progressBar.Location = new Point(500, 250);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(200, 23);
            this.progressBar.Style = ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 1;
            this.progressBar.Visible = false;

            // lblStatusMsg
            this.lblStatusMsg.AutoSize = true;
            this.lblStatusMsg.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblStatusMsg.Location = new Point(500, 230);
            this.lblStatusMsg.Name = "lblStatusMsg";
            this.lblStatusMsg.Size = new Size(109, 15);
            this.lblStatusMsg.TabIndex = 2;
            this.lblStatusMsg.Text = "Generating report...";
            this.lblStatusMsg.Visible = false;

            // frmReportPayroll
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlTop);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.Name = "frmReportPayroll";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Payroll Report";
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(this.frmReportPayroll_Load);

            this.pnlTop.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlFilters.ResumeLayout(false);
            this.grpFilters.ResumeLayout(false);
            this.grpFilters.PerformLayout();
            this.ResumeLayout(false);
        }

        private void InitializeForm()
        {
            // Apply dark theme if enabled
            if (GlobalVariables.IsDarkMode)
            {
                ApplyDarkTheme();
            }
        }

        private void ApplyDarkTheme()
        {
            this.BackColor = Color.FromArgb(45, 45, 48);
            pnlFilters.BackColor = Color.FromArgb(37, 37, 38);
            grpFilters.ForeColor = Color.White;
            
            foreach (Control control in grpFilters.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = Color.White;
                }
                else if (control is ComboBox)
                {
                    control.BackColor = Color.FromArgb(60, 60, 60);
                    control.ForeColor = Color.White;
                }
                else if (control is CheckBox)
                {
                    control.ForeColor = Color.White;
                }
            }
        }

        private void frmReportPayroll_Load(object sender, EventArgs e)
        {
            LoadPayrollPeriods();
            LoadDepartments();
            LoadEmployees();
            LoadReportTypes();
            LoadStatus();
        }

        private void LoadPayrollPeriods()
        {
            try
            {
                cmbPayrollPeriod.Items.Clear();
                cmbPayrollPeriod.Items.Add(new { Text = "All Periods", Value = "" });
                
                string query = @"SELECT period_id, 
                                CONCAT(period_name, ' (', DATE_FORMAT(date_from, '%m/%d/%Y'), ' - ', DATE_FORMAT(date_to, '%m/%d/%Y'), ')') as period_display,
                                date_from, date_to
                                FROM payroll_periods 
                                WHERE is_active  = 'Active' 
                                ORDER BY date_from DESC";
                
                DataTable dt = UtilityHelper.GetDataTable(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbPayrollPeriod.Items.Add(new { 
                        Text = row["period_display"].ToString(), 
                        Value = row["period_id"].ToString() 
                    });
                }
                
                cmbPayrollPeriod.DisplayMember = "Text";
                cmbPayrollPeriod.ValueMember = "Value";
                cmbPayrollPeriod.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payroll periods: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                cmbDepartment.Items.Clear();
                cmbDepartment.Items.Add(new { Text = "All Departments", Value = "" });
                
                string query = "SELECT department_id, department_name FROM departments WHERE is_active = 'Active' ORDER BY department_name";
                DataTable dt = UtilityHelper.GetDataTable(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbDepartment.Items.Add(new { 
                        Text = row["department_name"].ToString(), 
                        Value = row["department_id"].ToString() 
                    });
                }
                
                cmbDepartment.DisplayMember = "Text";
                cmbDepartment.ValueMember = "Value";
                cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEmployees()
        {
            try
            {
                cmbEmployee.Items.Clear();
                cmbEmployee.Items.Add(new { Text = "All Employees", Value = "" });
                
                string query = @"SELECT e.employee_id, CONCAT(e.first_name, ' ', e.last_name) as full_name 
                                FROM employees e 
                                WHERE e.status = 'Active' 
                                ORDER BY e.first_name, e.last_name";
                
                DataTable dt = UtilityHelper.GetDataTable(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbEmployee.Items.Add(new { 
                        Text = row["full_name"].ToString(), 
                        Value = row["employee_id"].ToString() 
                    });
                }
                
                cmbEmployee.DisplayMember = "Text";
                cmbEmployee.ValueMember = "Value";
                cmbEmployee.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReportTypes()
        {
            cmbReportType.Items.Clear();
            cmbReportType.Items.Add(new { Text = "Summary Report", Value = "Summary" });
            cmbReportType.Items.Add(new { Text = "Detailed Report", Value = "Detailed" });
            cmbReportType.Items.Add(new { Text = "Payslip Report", Value = "Payslip" });
            cmbReportType.Items.Add(new { Text = "Tax Report", Value = "Tax" });
            cmbReportType.Items.Add(new { Text = "Deductions Report", Value = "Deductions" });
            cmbReportType.Items.Add(new { Text = "Allowances Report", Value = "Allowances" });
            
            cmbReportType.DisplayMember = "Text";
            cmbReportType.ValueMember = "Value";
            cmbReportType.SelectedIndex = 0;
        }

        private void LoadStatus()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add(new { Text = "All Status", Value = "" });
            cmbStatus.Items.Add(new { Text = "Draft", Value = "Draft" });
            cmbStatus.Items.Add(new { Text = "Processed", Value = "Processed" });
            cmbStatus.Items.Add(new { Text = "Approved", Value = "Approved" });
            cmbStatus.Items.Add(new { Text = "Paid", Value = "Paid" });
            
            cmbStatus.DisplayMember = "Text";
            cmbStatus.ValueMember = "Value";
            cmbStatus.SelectedIndex = 0;
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPayrollPeriod.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select a payroll period.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ShowProgress(true);
                btnGenerate.Enabled = false;
                
                await GenerateReport();
                
                btnExport.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowProgress(false);
                btnGenerate.Enabled = true;
            }
        }

        private async System.Threading.Tasks.Task GenerateReport()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    string query = BuildQuery();
                    DataTable reportData = UtilityHelper.GetDataTable(query);
                    
                    this.Invoke(new Action(() =>
                    {
                        LoadReportData(reportData);
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        throw ex;
                    }));
                }
            });
        }

        private string BuildQuery()
        {
            var selectedReportType = (dynamic)cmbReportType.SelectedItem;
            string reportType = selectedReportType?.Value?.ToString() ?? "Summary";

            string query = @"
                SELECT 
                    p.payroll_id,
                    pp.period_name,
                    pp.date_from,
                    pp.date_to,
                    CONCAT(e.first_name, ' ', e.last_name) as employee_name,
                    e.employee_number,
                    d.department_name,
                    p.basic_salary,
                    p.overtime_pay,
                    p.allowances,
                    p.gross_pay,
                    p.sss_contribution,
                    p.philhealth_contribution,
                    p.pagibig_contribution,
                    p.tax_withheld,
                    p.other_deductions,
                    p.total_deductions,
                    p.net_pay,
                    p.status,
                    p.processed_date
                FROM payroll p
                INNER JOIN employees e ON p.employee_id = e.employee_id
                INNER JOIN departments d ON e.department_id = d.department_id
                INNER JOIN payroll_periods pp ON p.period_id = pp.period_id
                WHERE 1=1";

            // Add filters
            var selectedPeriod = (dynamic)cmbPayrollPeriod.SelectedItem;
            if (!string.IsNullOrEmpty(selectedPeriod?.Value?.ToString()))
            {
                query += " AND p.period_id = @periodId";
            }

            var selectedEmployee = (dynamic)cmbEmployee.SelectedItem;
            if (!string.IsNullOrEmpty(selectedEmployee?.Value?.ToString()))
            {
                query += " AND p.employee_id = @employeeId";
            }

            var selectedDepartment = (dynamic)cmbDepartment.SelectedItem;
            if (!string.IsNullOrEmpty(selectedDepartment?.Value?.ToString()))
            {
                query += " AND e.department_id = @departmentId";
            }

            var selectedStatus = (dynamic)cmbStatus.SelectedItem;
            if (!string.IsNullOrEmpty(selectedStatus?.Value?.ToString()))
            {
                query += " AND p.status = @status";
            }

            query += " ORDER BY pp.date_from DESC, e.first_name, e.last_name";

            return query;
        }

        private void LoadReportData(DataTable data)
        {
            try
            {
                var selectedReportType = (dynamic)cmbReportType.SelectedItem;
                string reportType = selectedReportType?.Value?.ToString() ?? "Summary";
                
                string reportPath = "rptPayroll.rdlc";
                switch (reportType)
                {
                    case "Summary":
                        reportPath = "rptPayrollSummary.rdlc";
                        break;
                    case "Detailed":
                        reportPath = "rptPayrollDetailed.rdlc";
                        break;
                    case "Payslip":
                        reportPath = "rptPayslip.rdlc";
                        break;
                    case "Tax":
                        reportPath = "rptPayrollTax.rdlc";
                        break;
                    case "Deductions":
                        reportPath = "rptPayrollDeductions.rdlc";
                        break;
                    case "Allowances":
                        reportPath = "rptPayrollAllowances.rdlc";
                        break;
                    default:
                        reportPath = "rptPayroll.rdlc";
                        break;
                }
                
                reportViewer.LocalReport.ReportPath = reportPath;
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PayrollDataSet", data));
                
                // Set report parameters
                var selectedPeriod = (dynamic)cmbPayrollPeriod.SelectedItem;
                var parameters = new ReportParameter[]
                {
                    new ReportParameter("PayrollPeriod", selectedPeriod?.Text?.ToString() ?? "All Periods"),
                    new ReportParameter("ReportType", ((dynamic)cmbReportType.SelectedItem).Text.ToString()),
                    new ReportParameter("CompanyName", GlobalVariables.CompanyName),
                    new ReportParameter("CompanyAddress", GlobalVariables.CompanyAddress),
                    new ReportParameter("GeneratedBy", GlobalVariables.CurrentUser),
                    new ReportParameter("GeneratedDate", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")),
                    new ReportParameter("IncludeDeductions", chkIncludeDeductions.Checked.ToString()),
                    new ReportParameter("IncludeAllowances", chkIncludeAllowances.Checked.ToString()),
                    new ReportParameter("IncludeTax", chkIncludeTax.Checked.ToString())
                };
                
                reportViewer.LocalReport.SetParameters(parameters);
                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowProgress(bool show)
        {
            progressBar.Visible = show;
            lblStatusMsg.Visible = show;
            reportViewer.Visible = !show;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf|Excel Files (*.xlsx)|*.xlsx";
                saveDialog.FileName = $"PayrollReport_{DateTime.Now:yyyyMMdd}";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string format = saveDialog.FilterIndex == 1 ? "PDF" : "EXCELOPENXML";
                    
                    byte[] bytes = reportViewer.LocalReport.Render(format);
                    System.IO.File.WriteAllBytes(saveDialog.FileName, bytes);
                    
                    MessageBox.Show("Report exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                reportViewer.PrintDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
