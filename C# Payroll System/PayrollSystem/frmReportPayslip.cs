using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using MySqlConnector;
using Microsoft.CSharp.RuntimeBinder;

namespace PayrollSystem
{
    public partial class frmReportPayslip : Form
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
        private Label lblPayslipType;
        private ComboBox cmbPayslipType;
        private CheckBox chkShowDeductions;
        private CheckBox chkShowAllowances;
        private CheckBox chkShowTax;
        private CheckBox chkShowContributions;
        private Button btnGenerate;
        private Button btnExport;
        private Button btnPrint;
        private Button btnEmail;
        private Button btnClose;
        private ReportViewer reportViewer;
        private ProgressBar progressBar;
        private Label lblStatusMsg;

        public frmReportPayslip()
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
            this.lblPayslipType = new Label();
            this.cmbPayslipType = new ComboBox();
            this.chkShowDeductions = new CheckBox();
            this.chkShowAllowances = new CheckBox();
            this.chkShowTax = new CheckBox();
            this.chkShowContributions = new CheckBox();
            this.btnGenerate = new Button();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.btnEmail = new Button();
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
            this.pnlTop.Controls.Add(this.btnEmail);
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
            this.pnlFilters.Size = new Size(1200, 120);
            this.pnlFilters.TabIndex = 1;

            // grpFilters
            this.grpFilters.Controls.Add(this.chkShowContributions);
            this.grpFilters.Controls.Add(this.chkShowTax);
            this.grpFilters.Controls.Add(this.chkShowAllowances);
            this.grpFilters.Controls.Add(this.chkShowDeductions);
            this.grpFilters.Controls.Add(this.cmbPayslipType);
            this.grpFilters.Controls.Add(this.lblPayslipType);
            this.grpFilters.Controls.Add(this.cmbDepartment);
            this.grpFilters.Controls.Add(this.lblDepartment);
            this.grpFilters.Controls.Add(this.cmbEmployee);
            this.grpFilters.Controls.Add(this.lblEmployee);
            this.grpFilters.Controls.Add(this.cmbPayrollPeriod);
            this.grpFilters.Controls.Add(this.lblPayrollPeriod);
            this.grpFilters.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpFilters.Location = new Point(12, 10);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Size = new Size(1176, 100);
            this.grpFilters.TabIndex = 0;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "Payslip Filters";

            // lblPayrollPeriod
            this.lblPayrollPeriod.AutoSize = true;
            this.lblPayrollPeriod.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblPayrollPeriod.Location = new Point(15, 25);
            this.lblPayrollPeriod.Name = "lblPayrollPeriod";
            this.lblPayrollPeriod.Size = new Size(87, 15);
            this.lblPayrollPeriod.TabIndex = 0;
            this.lblPayrollPeriod.Text = "Payroll Period:";

            // cmbPayrollPeriod
            this.cmbPayrollPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPayrollPeriod.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbPayrollPeriod.FormattingEnabled = true;
            this.cmbPayrollPeriod.Location = new Point(15, 45);
            this.cmbPayrollPeriod.Name = "cmbPayrollPeriod";
            this.cmbPayrollPeriod.Size = new Size(200, 23);
            this.cmbPayrollPeriod.TabIndex = 1;

            // lblEmployee
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblEmployee.Location = new Point(230, 25);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new Size(65, 15);
            this.lblEmployee.TabIndex = 2;
            this.lblEmployee.Text = "Employee:";

            // cmbEmployee
            this.cmbEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEmployee.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbEmployee.FormattingEnabled = true;
            this.cmbEmployee.Location = new Point(230, 45);
            this.cmbEmployee.Name = "cmbEmployee";
            this.cmbEmployee.Size = new Size(200, 23);
            this.cmbEmployee.TabIndex = 3;

            // lblDepartment
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblDepartment.Location = new Point(445, 25);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new Size(73, 15);
            this.lblDepartment.TabIndex = 4;
            this.lblDepartment.Text = "Department:";

            // cmbDepartment
            this.cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDepartment.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new Point(445, 45);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new Size(150, 23);
            this.cmbDepartment.TabIndex = 5;
            this.cmbDepartment.SelectedIndexChanged += new EventHandler(this.cmbDepartment_SelectedIndexChanged);

            // lblPayslipType
            this.lblPayslipType.AutoSize = true;
            this.lblPayslipType.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblPayslipType.Location = new Point(610, 25);
            this.lblPayslipType.Name = "lblPayslipType";
            this.lblPayslipType.Size = new Size(78, 15);
            this.lblPayslipType.TabIndex = 6;
            this.lblPayslipType.Text = "Payslip Type:";

            // cmbPayslipType
            this.cmbPayslipType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPayslipType.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbPayslipType.FormattingEnabled = true;
            this.cmbPayslipType.Location = new Point(610, 45);
            this.cmbPayslipType.Name = "cmbPayslipType";
            this.cmbPayslipType.Size = new Size(150, 23);
            this.cmbPayslipType.TabIndex = 7;

            // chkShowDeductions
            this.chkShowDeductions.AutoSize = true;
            this.chkShowDeductions.Checked = true;
            this.chkShowDeductions.CheckState = CheckState.Checked;
            this.chkShowDeductions.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkShowDeductions.Location = new Point(780, 25);
            this.chkShowDeductions.Name = "chkShowDeductions";
            this.chkShowDeductions.Size = new Size(118, 19);
            this.chkShowDeductions.TabIndex = 8;
            this.chkShowDeductions.Text = "Show Deductions";
            this.chkShowDeductions.UseVisualStyleBackColor = true;

            // chkShowAllowances
            this.chkShowAllowances.AutoSize = true;
            this.chkShowAllowances.Checked = true;
            this.chkShowAllowances.CheckState = CheckState.Checked;
            this.chkShowAllowances.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkShowAllowances.Location = new Point(780, 50);
            this.chkShowAllowances.Name = "chkShowAllowances";
            this.chkShowAllowances.Size = new Size(117, 19);
            this.chkShowAllowances.TabIndex = 9;
            this.chkShowAllowances.Text = "Show Allowances";
            this.chkShowAllowances.UseVisualStyleBackColor = true;

            // chkShowTax
            this.chkShowTax.AutoSize = true;
            this.chkShowTax.Checked = true;
            this.chkShowTax.CheckState = CheckState.Checked;
            this.chkShowTax.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkShowTax.Location = new Point(920, 25);
            this.chkShowTax.Name = "chkShowTax";
            this.chkShowTax.Size = new Size(78, 19);
            this.chkShowTax.TabIndex = 10;
            this.chkShowTax.Text = "Show Tax";
            this.chkShowTax.UseVisualStyleBackColor = true;

            // chkShowContributions
            this.chkShowContributions.AutoSize = true;
            this.chkShowContributions.Checked = true;
            this.chkShowContributions.CheckState = CheckState.Checked;
            this.chkShowContributions.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkShowContributions.Location = new Point(920, 50);
            this.chkShowContributions.Name = "chkShowContributions";
            this.chkShowContributions.Size = new Size(133, 19);
            this.chkShowContributions.TabIndex = 11;
            this.chkShowContributions.Text = "Show Contributions";
            this.chkShowContributions.UseVisualStyleBackColor = true;

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

            // btnEmail
            this.btnEmail.BackColor = Color.FromArgb(241, 196, 15);
            this.btnEmail.Enabled = false;
            this.btnEmail.FlatAppearance.BorderSize = 0;
            this.btnEmail.FlatStyle = FlatStyle.Flat;
            this.btnEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnEmail.ForeColor = Color.White;
            this.btnEmail.Location = new Point(351, 15);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new Size(100, 30);
            this.btnEmail.TabIndex = 3;
            this.btnEmail.Text = "Email";
            this.btnEmail.UseVisualStyleBackColor = false;
            this.btnEmail.Click += new EventHandler(this.btnEmail_Click);

            // btnClose
            this.btnClose.BackColor = Color.FromArgb(231, 76, 60);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(1088, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(100, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // pnlMain
            this.pnlMain.Controls.Add(this.lblStatusMsg);
            this.pnlMain.Controls.Add(this.progressBar);
            this.pnlMain.Controls.Add(this.reportViewer);
            this.pnlMain.Dock = DockStyle.Fill;
            this.pnlMain.Location = new Point(0, 180);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new Size(1200, 520);
            this.pnlMain.TabIndex = 2;

            // reportViewer
            this.reportViewer.Dock = DockStyle.Fill;
            this.reportViewer.Location = new Point(0, 0);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ServerReport.BearerToken = null;
            this.reportViewer.Size = new Size(1200, 520);
            this.reportViewer.TabIndex = 0;

            // progressBar
            this.progressBar.Location = new Point(500, 260);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(200, 23);
            this.progressBar.Style = ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 1;
            this.progressBar.Visible = false;

            // lblStatusMsg
            this.lblStatusMsg.AutoSize = true;
            this.lblStatusMsg.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblStatusMsg.Location = new Point(500, 240);
            this.lblStatusMsg.Name = "lblStatusMsg";
            this.lblStatusMsg.Size = new Size(115, 15);
            this.lblStatusMsg.TabIndex = 2;
            this.lblStatusMsg.Text = "Generating payslip...";
            this.lblStatusMsg.Visible = false;

            // frmReportPayslip
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlTop);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.Name = "frmReportPayslip";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Payslip Report";
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(this.frmReportPayslip_Load);

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

        private void frmReportPayslip_Load(object sender, EventArgs e)
        {
            LoadPayrollPeriods();
            LoadDepartments();
            LoadEmployees();
            LoadPayslipTypes();
        }

        private void LoadPayrollPeriods()
        {
            try
            {
                cmbPayrollPeriod.Items.Clear();
                cmbPayrollPeriod.Items.Add(new { Text = "Select Period", Value = "" });
                
                string query = @"SELECT period_id, 
                                CONCAT(period_name, ' (', DATE_FORMAT(date_from, '%m/%d/%Y'), ' - ', DATE_FORMAT(date_to, '%m/%d/%Y'), ')') as period_display,
                                date_from, date_to
                                FROM payroll_periods 
                                WHERE status = 'Active' 
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
                
                string query = "SELECT department_id, department_name FROM departments WHERE status = 'Active' ORDER BY department_name";
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
                                WHERE e.status = 'Active' ";
                
                var selectedDepartment = (dynamic)cmbDepartment.SelectedItem;
                if (!string.IsNullOrEmpty(selectedDepartment?.Value?.ToString()))
                {
                    query += " AND e.department_id = @departmentId";
                }
                
                query += " ORDER BY e.first_name, e.last_name";
                
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

        private void LoadPayslipTypes()
        {
            cmbPayslipType.Items.Clear();
            cmbPayslipType.Items.Add(new { Text = "Standard Payslip", Value = "Standard" });
            cmbPayslipType.Items.Add(new { Text = "Detailed Payslip", Value = "Detailed" });
            cmbPayslipType.Items.Add(new { Text = "Summary Payslip", Value = "Summary" });
            cmbPayslipType.Items.Add(new { Text = "Tax Certificate", Value = "TaxCert" });
            cmbPayslipType.Items.Add(new { Text = "13th Month Pay", Value = "13thMonth" });
            
            cmbPayslipType.DisplayMember = "Text";
            cmbPayslipType.ValueMember = "Value";
            cmbPayslipType.SelectedIndex = 0;
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
                
                await GeneratePayslip();
                
                btnExport.Enabled = true;
                btnPrint.Enabled = true;
                btnEmail.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating payslip: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowProgress(false);
                btnGenerate.Enabled = true;
            }
        }

        private async System.Threading.Tasks.Task GeneratePayslip()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    string query = BuildQuery();
                    DataTable payslipData = UtilityHelper.GetDataTable(query);
                    
                    this.Invoke(new Action(() =>
                    {
                        LoadPayslipData(payslipData);
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
            string query = @"
                SELECT 
                    p.payroll_id,
                    pp.period_name,
                    pp.date_from,
                    pp.date_to,
                    e.employee_id,
                    e.employee_number,
                    CONCAT(e.first_name, ' ', e.last_name) as employee_name,
                    e.first_name,
                    e.last_name,
                    e.middle_name,
                    e.email,
                    e.phone,
                    e.address,
                    e.hire_date,
                    d.department_name,
                    jt.job_title,
                    p.basic_salary,
                    p.overtime_pay,
                    p.holiday_pay,
                    p.night_differential,
                    p.allowances,
                    p.bonus,
                    p.gross_pay,
                    p.sss_contribution,
                    p.philhealth_contribution,
                    p.pagibig_contribution,
                    p.tax_withheld,
                    p.loan_deduction,
                    p.other_deductions,
                    p.total_deductions,
                    p.net_pay,
                    p.status,
                    p.processed_date,
                    p.processed_by
                FROM payroll p
                INNER JOIN employees e ON p.employee_id = e.employee_id
                INNER JOIN departments d ON e.department_id = d.department_id
                INNER JOIN job_titles jt ON e.job_title_id = jt.job_title_id
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

            query += " ORDER BY e.first_name, e.last_name";

            return query;
        }

        private void LoadPayslipData(DataTable data)
        {
            try
            {
                var selectedPayslipType = (dynamic)cmbPayslipType.SelectedItem;
                string payslipType = selectedPayslipType?.Value?.ToString() ?? "Standard";
                
                string reportPath = "rptPayslip.rdlc";
                switch (payslipType)
                {
                    case "Standard":
                        reportPath = "rptPayslipStandard.rdlc";
                        break;
                    case "Detailed":
                        reportPath = "rptPayslipDetailed.rdlc";
                        break;
                    case "Summary":
                        reportPath = "rptPayslipSummary.rdlc";
                        break;
                    case "TaxCert":
                        reportPath = "rptTaxCertificate.rdlc";
                        break;
                    case "13thMonth":
                        reportPath = "rpt13thMonthPay.rdlc";
                        break;
                    default:
                        reportPath = "rptPayslip.rdlc";
                        break;
                }
                
                reportViewer.LocalReport.ReportPath = reportPath;
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PayslipDataSet", data));
                
                // Set report parameters
                var selectedPeriod = (dynamic)cmbPayrollPeriod.SelectedItem;
                var parameters = new ReportParameter[]
                {
                    new ReportParameter("PayrollPeriod", selectedPeriod?.Text?.ToString() ?? "All Periods"),
                    new ReportParameter("PayslipType", ((dynamic)cmbPayslipType.SelectedItem).Text.ToString()),
                    new ReportParameter("CompanyName", GlobalVariables.CompanyName),
                    new ReportParameter("CompanyAddress", GlobalVariables.CompanyAddress),
                    new ReportParameter("CompanyPhone", GlobalVariables.CompanyPhone),
                    new ReportParameter("CompanyEmail", GlobalVariables.CompanyEmail),
                    new ReportParameter("GeneratedBy", GlobalVariables.CurrentUser),
                    new ReportParameter("GeneratedDate", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")),
                    new ReportParameter("ShowDeductions", chkShowDeductions.Checked.ToString()),
                    new ReportParameter("ShowAllowances", chkShowAllowances.Checked.ToString()),
                    new ReportParameter("ShowTax", chkShowTax.Checked.ToString()),
                    new ReportParameter("ShowContributions", chkShowContributions.Checked.ToString())
                };
                
                reportViewer.LocalReport.SetParameters(parameters);
                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payslip data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                saveDialog.FileName = $"Payslip_{DateTime.Now:yyyyMMdd}";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string format = saveDialog.FilterIndex == 1 ? "PDF" : "EXCELOPENXML";
                    
                    byte[] bytes = reportViewer.LocalReport.Render(format);
                    System.IO.File.WriteAllBytes(saveDialog.FileName, bytes);
                    
                    MessageBox.Show("Payslip exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting payslip: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Error printing payslip: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEmail_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedEmployee = (dynamic)cmbEmployee.SelectedItem;
                if (string.IsNullOrEmpty(selectedEmployee?.Value?.ToString()))
                {
                    MessageBox.Show("Please select a specific employee to email payslip.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Generate PDF for email
                byte[] pdfBytes = reportViewer.LocalReport.Render("PDF");
                string tempFile = System.IO.Path.GetTempFileName() + ".pdf";
                System.IO.File.WriteAllBytes(tempFile, pdfBytes);

                // Get employee email
                string query = "SELECT email FROM employees WHERE employee_id = @employeeId";
                MySqlParameter[] parameters = new MySqlParameter[] { DatabaseManager.CreateParameter("@employeeId", selectedEmployee.Value.ToString()) };
                string employeeEmail = DatabaseManager.ExecuteScalar(query, parameters)?.ToString();

                if (string.IsNullOrEmpty(employeeEmail))
                {
                    MessageBox.Show("Employee email not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    // Load email settings if not already loaded
                    EmailManager.LoadEmailSettings();

                    // Send email with payslip
                    await EmailManager.SendPayslipEmailAsync(
                        employeeEmail,
                        selectedEmployee.Text,
                        ((dynamic)cmbPayrollPeriod.SelectedItem).Text,
                        tempFile
                    );

                    MessageBox.Show($"Payslip sent successfully to {employeeEmail}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception emailEx)
                {
                    MessageBox.Show($"Error sending email: {emailEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Clean up temporary file
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
