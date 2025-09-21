using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PayrollSystem
{
    public partial class frmReportDTR : Form
    {
        private Panel pnlTop;
        private Panel pnlMain;
        private Panel pnlFilters;
        private GroupBox grpFilters;
        private Label lblDateFrom;
        private DateTimePicker dtpDateFrom;
        private Label lblDateTo;
        private DateTimePicker dtpDateTo;
        private Label lblEmployee;
        private ComboBox cmbEmployee;
        private Label lblDepartment;
        private ComboBox cmbDepartment;
        private Label lblReportType;
        private ComboBox cmbReportType;
        private CheckBox chkIncludeAbsent;
        private CheckBox chkIncludeLate;
        private CheckBox chkIncludeOvertime;
        private Button btnGenerate;
        private Button btnExport;
        private Button btnPrint;
        private Button btnClose;
        private ReportViewer reportViewer;
        private ProgressBar progressBar;
        private Label lblStatus;

        public frmReportDTR()
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
            this.lblDateFrom = new Label();
            this.dtpDateFrom = new DateTimePicker();
            this.lblDateTo = new Label();
            this.dtpDateTo = new DateTimePicker();
            this.lblEmployee = new Label();
            this.cmbEmployee = new ComboBox();
            this.lblDepartment = new Label();
            this.cmbDepartment = new ComboBox();
            this.lblReportType = new Label();
            this.cmbReportType = new ComboBox();
            this.chkIncludeAbsent = new CheckBox();
            this.chkIncludeLate = new CheckBox();
            this.chkIncludeOvertime = new CheckBox();
            this.btnGenerate = new Button();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.btnClose = new Button();
            this.reportViewer = new ReportViewer();
            this.progressBar = new ProgressBar();
            this.lblStatus = new Label();

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
            this.grpFilters.Controls.Add(this.chkIncludeOvertime);
            this.grpFilters.Controls.Add(this.chkIncludeLate);
            this.grpFilters.Controls.Add(this.chkIncludeAbsent);
            this.grpFilters.Controls.Add(this.cmbReportType);
            this.grpFilters.Controls.Add(this.lblReportType);
            this.grpFilters.Controls.Add(this.cmbDepartment);
            this.grpFilters.Controls.Add(this.lblDepartment);
            this.grpFilters.Controls.Add(this.cmbEmployee);
            this.grpFilters.Controls.Add(this.lblEmployee);
            this.grpFilters.Controls.Add(this.dtpDateTo);
            this.grpFilters.Controls.Add(this.lblDateTo);
            this.grpFilters.Controls.Add(this.dtpDateFrom);
            this.grpFilters.Controls.Add(this.lblDateFrom);
            this.grpFilters.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpFilters.Location = new Point(12, 10);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Size = new Size(1176, 120);
            this.grpFilters.TabIndex = 0;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "Report Filters";

            // lblDateFrom
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblDateFrom.Location = new Point(15, 30);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new Size(70, 15);
            this.lblDateFrom.TabIndex = 0;
            this.lblDateFrom.Text = "Date From:";

            // dtpDateFrom
            this.dtpDateFrom.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.dtpDateFrom.Format = DateTimePickerFormat.Short;
            this.dtpDateFrom.Location = new Point(15, 50);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new Size(120, 23);
            this.dtpDateFrom.TabIndex = 1;

            // lblDateTo
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblDateTo.Location = new Point(150, 30);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new Size(54, 15);
            this.lblDateTo.TabIndex = 2;
            this.lblDateTo.Text = "Date To:";

            // dtpDateTo
            this.dtpDateTo.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.dtpDateTo.Format = DateTimePickerFormat.Short;
            this.dtpDateTo.Location = new Point(150, 50);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new Size(120, 23);
            this.dtpDateTo.TabIndex = 3;

            // lblEmployee
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblEmployee.Location = new Point(285, 30);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new Size(65, 15);
            this.lblEmployee.TabIndex = 4;
            this.lblEmployee.Text = "Employee:";

            // cmbEmployee
            this.cmbEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEmployee.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbEmployee.FormattingEnabled = true;
            this.cmbEmployee.Location = new Point(285, 50);
            this.cmbEmployee.Name = "cmbEmployee";
            this.cmbEmployee.Size = new Size(200, 23);
            this.cmbEmployee.TabIndex = 5;

            // lblDepartment
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblDepartment.Location = new Point(500, 30);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new Size(73, 15);
            this.lblDepartment.TabIndex = 6;
            this.lblDepartment.Text = "Department:";

            // cmbDepartment
            this.cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDepartment.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new Point(500, 50);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new Size(150, 23);
            this.cmbDepartment.TabIndex = 7;
            this.cmbDepartment.SelectedIndexChanged += new EventHandler(this.cmbDepartment_SelectedIndexChanged);

            // lblReportType
            this.lblReportType.AutoSize = true;
            this.lblReportType.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblReportType.Location = new Point(665, 30);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new Size(77, 15);
            this.lblReportType.TabIndex = 8;
            this.lblReportType.Text = "Report Type:";

            // cmbReportType
            this.cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbReportType.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Location = new Point(665, 50);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new Size(150, 23);
            this.cmbReportType.TabIndex = 9;

            // chkIncludeAbsent
            this.chkIncludeAbsent.AutoSize = true;
            this.chkIncludeAbsent.Checked = true;
            this.chkIncludeAbsent.CheckState = CheckState.Checked;
            this.chkIncludeAbsent.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkIncludeAbsent.Location = new Point(15, 85);
            this.chkIncludeAbsent.Name = "chkIncludeAbsent";
            this.chkIncludeAbsent.Size = new Size(113, 19);
            this.chkIncludeAbsent.TabIndex = 10;
            this.chkIncludeAbsent.Text = "Include Absences";
            this.chkIncludeAbsent.UseVisualStyleBackColor = true;

            // chkIncludeLate
            this.chkIncludeLate.AutoSize = true;
            this.chkIncludeLate.Checked = true;
            this.chkIncludeLate.CheckState = CheckState.Checked;
            this.chkIncludeLate.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkIncludeLate.Location = new Point(150, 85);
            this.chkIncludeLate.Name = "chkIncludeLate";
            this.chkIncludeLate.Size = new Size(125, 19);
            this.chkIncludeLate.TabIndex = 11;
            this.chkIncludeLate.Text = "Include Late/Tardy";
            this.chkIncludeLate.UseVisualStyleBackColor = true;

            // chkIncludeOvertime
            this.chkIncludeOvertime.AutoSize = true;
            this.chkIncludeOvertime.Checked = true;
            this.chkIncludeOvertime.CheckState = CheckState.Checked;
            this.chkIncludeOvertime.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.chkIncludeOvertime.Location = new Point(285, 85);
            this.chkIncludeOvertime.Name = "chkIncludeOvertime";
            this.chkIncludeOvertime.Size = new Size(118, 19);
            this.chkIncludeOvertime.TabIndex = 12;
            this.chkIncludeOvertime.Text = "Include Overtime";
            this.chkIncludeOvertime.UseVisualStyleBackColor = true;

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
            this.pnlMain.Controls.Add(this.lblStatus);
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

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblStatus.Location = new Point(500, 230);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(109, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Generating report...";
            this.lblStatus.Visible = false;

            // frmReportDTR
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlTop);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.Name = "frmReportDTR";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Daily Time Record Report";
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(this.frmReportDTR_Load);

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
            // Set default date range (current month)
            dtpDateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDateTo.Value = DateTime.Now;

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
                else if (control is ComboBox || control is DateTimePicker)
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

        private void frmReportDTR_Load(object sender, EventArgs e)
        {
            LoadDepartments();
            LoadEmployees();
            LoadReportTypes();
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
            cmbReportType.Items.Add(new { Text = "Attendance Only", Value = "Attendance" });
            cmbReportType.Items.Add(new { Text = "Overtime Report", Value = "Overtime" });
            cmbReportType.Items.Add(new { Text = "Late/Tardy Report", Value = "Late" });
            
            cmbReportType.DisplayMember = "Text";
            cmbReportType.ValueMember = "Value";
            cmbReportType.SelectedIndex = 0;
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
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
            //BUG needed to be fix
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
                    dtr.dtr_id,
                    dtr.dtr_date,
                    CONCAT(e.first_name, ' ', e.last_name) as employee_name,
                    e.employee_number,
                    d.department_name,
                    dtr.time_in,
                    dtr.time_out,
                    dtr.break_out,
                    dtr.break_in,
                    dtr.total_hours,
                    dtr.regular_hours,
                    dtr.overtime_hours,
                    dtr.late_minutes,
                    dtr.undertime_minutes,
                    dtr.status,
                    dtr.remarks
                FROM dtr 
                INNER JOIN employees e ON dtr.employee_id = e.employee_id
                INNER JOIN departments d ON e.department_id = d.department_id
                WHERE dtr.dtr_date BETWEEN @dateFrom AND @dateTo";

            // Add filters based on checkboxes
            if (!chkIncludeAbsent.Checked)
            {
                query += " AND dtr.status != 'Absent'";
            }

            if (!chkIncludeLate.Checked)
            {
                query += " AND dtr.late_minutes = 0";
            }

            if (!chkIncludeOvertime.Checked)
            {
                query += " AND dtr.overtime_hours = 0";
            }

            // Add filters
            var selectedEmployee = (dynamic)cmbEmployee.SelectedItem;
            if (!string.IsNullOrEmpty(selectedEmployee?.Value?.ToString()))
            {
                query += " AND dtr.employee_id = @employeeId";
            }

            var selectedDepartment = (dynamic)cmbDepartment.SelectedItem;
            if (!string.IsNullOrEmpty(selectedDepartment?.Value?.ToString()))
            {
                query += " AND e.department_id = @departmentId";
            }

            // Modify query based on report type
            switch (reportType)
            {
                case "Overtime":
                    query += " AND dtr.overtime_hours > 0";
                    break;
                case "Late":
                    query += " AND dtr.late_minutes > 0";
                    break;
                case "Attendance":
                    query += " AND dtr.status IN ('Present', 'Late', 'Overtime')";
                    break;
            }

            query += " ORDER BY dtr.dtr_date DESC, e.first_name, e.last_name";

            return query;
        }

        private void LoadReportData(DataTable data)
        {
            try
            {
                var selectedReportType = (dynamic)cmbReportType.SelectedItem;
                string reportType = selectedReportType?.Value?.ToString() ?? "Summary";
                
                string reportPath = "rptDTR.rdlc";
                switch (reportType)
                {
                    case "Summary":
                        reportPath = "rptDTRSummary.rdlc";
                        break;
                    case "Detailed":
                        reportPath = "rptDTRDetailed.rdlc";
                        break;
                    case "Overtime":
                        reportPath = "rptDTROvertime.rdlc";
                        break;
                    case "Late":
                        reportPath = "rptDTRLate.rdlc";
                        break;
                    default:
                        reportPath = "rptDTR.rdlc";
                        break;
                }
                
                reportViewer.LocalReport.ReportPath = reportPath;
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DTRDataSet", data));
                
                // Set report parameters
                var parameters = new ReportParameter[]
                {
                    new ReportParameter("DateFrom", dtpDateFrom.Value.ToString("MM/dd/yyyy")),
                    new ReportParameter("DateTo", dtpDateTo.Value.ToString("MM/dd/yyyy")),
                    new ReportParameter("ReportType", ((dynamic)cmbReportType.SelectedItem).Text.ToString()),
                    new ReportParameter("CompanyName", GlobalVariables.CompanyName),
                    new ReportParameter("CompanyAddress", GlobalVariables.CompanyAddress),
                    new ReportParameter("GeneratedBy", GlobalVariables.CurrentUser),
                    new ReportParameter("GeneratedDate", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"))
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
            lblStatus.Visible = show;
            reportViewer.Visible = !show;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf|Excel Files (*.xlsx)|*.xlsx";
                saveDialog.FileName = $"DTRReport_{DateTime.Now:yyyyMMdd}";
                
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
