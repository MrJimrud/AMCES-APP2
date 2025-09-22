using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PayrollSystem
{
    public partial class frmReportCA : Form
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
        private Label lblStatus;
        private ComboBox cmbStatus;
        private Button btnGenerate;
        private Button btnExport;
        private Button btnPrint;
        private Button btnClose;
        private ReportViewer reportViewer;
        private ProgressBar progressBar;
        private Label lblStatus2;

        public frmReportCA()
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
            this.lblStatus = new Label();
            this.cmbStatus = new ComboBox();
            this.btnGenerate = new Button();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.btnClose = new Button();
            this.reportViewer = new ReportViewer();
            this.progressBar = new ProgressBar();
            this.lblStatus2 = new Label();

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
            this.pnlFilters.Size = new Size(1200, 120);
            this.pnlFilters.TabIndex = 1;

            // grpFilters
            this.grpFilters.Controls.Add(this.cmbStatus);
            this.grpFilters.Controls.Add(this.lblStatus);
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
            this.grpFilters.Size = new Size(1176, 100);
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

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblStatus.Location = new Point(665, 30);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(42, 15);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Status:";

            // cmbStatus
            this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new Point(665, 50);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new Size(120, 23);
            this.cmbStatus.TabIndex = 9;

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
            this.pnlMain.Controls.Add(this.lblStatus2);
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
            this.progressBar.Location = new Point(500, 250);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(200, 23);
            this.progressBar.Style = ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 1;
            this.progressBar.Visible = false;

            // lblStatus2
            this.lblStatus2.AutoSize = true;
            this.lblStatus2.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblStatus2.Location = new Point(500, 230);
            this.lblStatus2.Name = "lblStatus2";
            this.lblStatus2.Size = new Size(109, 15);
            this.lblStatus2.TabIndex = 2;
            this.lblStatus2.Text = "Generating report...";
            this.lblStatus2.Visible = false;

            // frmReportCA
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlTop);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.Name = "frmReportCA";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Cash Advance Report";
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(this.frmReportCA_Load);

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
            }
        }

        private void frmReportCA_Load(object sender, EventArgs e)
        {
            LoadDepartments();
            LoadEmployees();
            LoadStatus();
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

        private void LoadStatus()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add(new { Text = "All Status", Value = "" });
            cmbStatus.Items.Add(new { Text = "Pending", Value = "Pending" });
            cmbStatus.Items.Add(new { Text = "Approved", Value = "Approved" });
            cmbStatus.Items.Add(new { Text = "Rejected", Value = "Rejected" });
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
            string query = @"
                SELECT 
                    ca.advance_id,
                    ca.request_date,
                    CONCAT(e.first_name, ' ', e.last_name) as employee_name,
                    e.employee_number,
                    d.department_name,
                    ca.amount,
                    ca.reason as purpose,
                    ca.status,
                    ca.approved_by,
                    ca.approved_date,
                    ca.remarks
                FROM cash_advances ca
                INNER JOIN employees e ON ca.employee_id = e.employee_id
                INNER JOIN departments d ON e.department_id = d.department_id
                WHERE ca.request_date BETWEEN @dateFrom AND @dateTo";

            // Add filters
            var selectedEmployee = (dynamic)cmbEmployee.SelectedItem;
            if (!string.IsNullOrEmpty(selectedEmployee?.Value?.ToString()))
            {
                query += " AND ca.employee_id = @employeeId";
            }

            var selectedDepartment = (dynamic)cmbDepartment.SelectedItem;
            if (!string.IsNullOrEmpty(selectedDepartment?.Value?.ToString()))
            {
                query += " AND e.department_id = @departmentId";
            }

            var selectedStatus = (dynamic)cmbStatus.SelectedItem;
            if (!string.IsNullOrEmpty(selectedStatus?.Value?.ToString()))
            {
                query += " AND ca.status = @status";
            }

            query += " ORDER BY ca.ca_date DESC, e.first_name, e.last_name";

            return query;
        }

        private void LoadReportData(DataTable data)
        {
            try
            {
                reportViewer.LocalReport.ReportPath = "rptCA.rdlc";
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CashAdvanceDataSet", data));
                
                // Set report parameters
                var parameters = new ReportParameter[]
                {
                    new ReportParameter("DateFrom", dtpDateFrom.Value.ToString("MM/dd/yyyy")),
                    new ReportParameter("DateTo", dtpDateTo.Value.ToString("MM/dd/yyyy")),
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
            lblStatus2.Visible = show;
            reportViewer.Visible = !show;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf|Excel Files (*.xlsx)|*.xlsx";
                saveDialog.FileName = $"CashAdvanceReport_{DateTime.Now:yyyyMMdd}";
                
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
