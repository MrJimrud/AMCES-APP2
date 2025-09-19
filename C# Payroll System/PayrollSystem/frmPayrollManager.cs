using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace PayrollSystem
{
    public partial class frmPayrollManager : Form
    {
        // DatabaseManager is static, no instance needed
        private DataTable payrollData;
        
        // Controls
        private ComboBox cmbPayrollPeriod;
        private ComboBox cmbDepartment;
        private ComboBox cmbStatus;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private DataGridView dgvPayroll;
        private Button btnGenerate;
        private Button btnCalculate;
        private Button btnApprove;
        private Button btnReject;
        private Button btnExport;
        private Button btnRefresh;
        private Button btnClose;
        private TextBox txtSearch;
        private TextBox txtRemarks;
        private Label lblTotalEmployees;
        private Label lblTotalAmount;
        private ProgressBar progressBar;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        
        public frmPayrollManager()
        {
            InitializeComponent();
            
            // Initialize remarks textbox
            txtRemarks = new TextBox
            {
                Location = new Point(10, 45),
                Size = new Size(200, 25),
                Name = "txtRemarks",
                Text = ""
            };
            this.Controls.Add(txtRemarks);
            
            LoadInitialData();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Payroll Manager";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            
            // Create controls
            CreateControls();
            LayoutControls();
            AttachEvents();
        }
        
        private void CreateControls()
        {
            // Filter controls
            cmbPayrollPeriod = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbDepartment = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            dtpFromDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            dtpToDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            txtSearch = new TextBox { Size = new Size(200, 23) };
            txtSearch.SetPlaceholderText("Search employee...");
            
            // Data grid
            dgvPayroll = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            
            // Buttons
            btnGenerate = new Button { Text = "Generate Payroll", Size = new Size(120, 30) };
            btnCalculate = new Button { Text = "Calculate", Size = new Size(100, 30) };
            btnApprove = new Button { Text = "Approve", Size = new Size(100, 30) };
            btnReject = new Button { Text = "Reject", Size = new Size(100, 30) };
            btnExport = new Button { Text = "Export", Size = new Size(100, 30) };
            btnRefresh = new Button { Text = "Refresh", Size = new Size(100, 30) };
            btnClose = new Button { Text = "Close", Size = new Size(100, 30) };
            
            // Summary labels
            lblTotalEmployees = new Label { Text = "Total Employees: 0", AutoSize = true };
            lblTotalAmount = new Label { Text = "Total Amount: ₱0.00", AutoSize = true };
            
            // Progress bar
            progressBar = new ProgressBar { Visible = false };
            
            // Status strip
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Ready");
            statusStrip.Items.Add(statusLabel);
        }
        
        private void LayoutControls()
        {
            // Filter panel
            var filterPanel = new Panel { Height = 80, Dock = DockStyle.Top };
            
            var lblPeriod = new Label { Text = "Period:", Location = new Point(10, 15), AutoSize = true };
            cmbPayrollPeriod.Location = new Point(60, 12);
            cmbPayrollPeriod.Size = new Size(150, 25);
            
            var lblDept = new Label { Text = "Department:", Location = new Point(220, 15), AutoSize = true };
            cmbDepartment.Location = new Point(290, 12);
            cmbDepartment.Size = new Size(150, 25);
            
            var lblStatus = new Label { Text = "Status:", Location = new Point(450, 15), AutoSize = true };
            cmbStatus.Location = new Point(490, 12);
            cmbStatus.Size = new Size(120, 25);
            
            var lblFrom = new Label { Text = "From:", Location = new Point(620, 15), AutoSize = true };
            dtpFromDate.Location = new Point(655, 12);
            dtpFromDate.Size = new Size(100, 25);
            
            var lblTo = new Label { Text = "To:", Location = new Point(765, 15), AutoSize = true };
            dtpToDate.Location = new Point(785, 12);
            dtpToDate.Size = new Size(100, 25);
            
            var lblSearch = new Label { Text = "Search:", Location = new Point(10, 45), AutoSize = true };
            txtSearch.Location = new Point(60, 42);
            txtSearch.Size = new Size(200, 25);
            
            btnRefresh.Location = new Point(270, 42);
            
            filterPanel.Controls.AddRange(new Control[] {
                lblPeriod, cmbPayrollPeriod, lblDept, cmbDepartment, lblStatus, cmbStatus,
                lblFrom, dtpFromDate, lblTo, dtpToDate, lblSearch, txtSearch, btnRefresh
            });
            
            // Button panel
            var buttonPanel = new Panel { Height = 50, Dock = DockStyle.Bottom };
            
            btnGenerate.Location = new Point(10, 10);
            btnCalculate.Location = new Point(140, 10);
            btnApprove.Location = new Point(250, 10);
            btnReject.Location = new Point(360, 10);
            btnExport.Location = new Point(470, 10);
            btnClose.Location = new Point(580, 10);
            
            lblTotalEmployees.Location = new Point(700, 15);
            lblTotalAmount.Location = new Point(850, 15);
            
            buttonPanel.Controls.AddRange(new Control[] {
                btnGenerate, btnCalculate, btnApprove, btnReject, btnExport, btnClose,
                lblTotalEmployees, lblTotalAmount
            });
            
            // Progress panel
            var progressPanel = new Panel { Height = 30, Dock = DockStyle.Bottom };
            progressBar.Dock = DockStyle.Fill;
            progressPanel.Controls.Add(progressBar);
            
            // Main panel for data grid
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            dgvPayroll.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(dgvPayroll);
            
            // Add all panels to form
            this.Controls.Add(mainPanel);
            this.Controls.Add(progressPanel);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(filterPanel);
            this.Controls.Add(statusStrip);
        }
        
        private void AttachEvents()
        {
            btnGenerate.Click += BtnGenerate_Click;
            btnCalculate.Click += BtnCalculate_Click;
            btnApprove.Click += BtnApprove_Click;
            btnReject.Click += BtnReject_Click;
            btnExport.Click += BtnExport_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnClose.Click += BtnClose_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            cmbPayrollPeriod.SelectedIndexChanged += FilterChanged;
            cmbDepartment.SelectedIndexChanged += FilterChanged;
            cmbStatus.SelectedIndexChanged += FilterChanged;
            dtpFromDate.ValueChanged += FilterChanged;
            dtpToDate.ValueChanged += FilterChanged;
        }
        
        private void LoadInitialData()
        {
            try
            {
                LoadPayrollPeriods();
                LoadDepartments();
                LoadStatusOptions();
                LoadPayrollData();
                SetDefaultDates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading initial data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadPayrollPeriods()
        {
            var periods = DatabaseManager.GetPayrollPeriods();
            cmbPayrollPeriod.Items.Clear();
            cmbPayrollPeriod.Items.Add("All Periods");
            foreach (DataRow row in periods.Rows)
            {
                cmbPayrollPeriod.Items.Add($"{row["period_name"]} ({row["start_date"]:MM/dd/yyyy} - {row["end_date"]:MM/dd/yyyy})");
            }
            cmbPayrollPeriod.SelectedIndex = 0;
        }
        
        private void LoadDepartments()
        {
            var departments = DatabaseManager.GetDepartments();
            cmbDepartment.Items.Clear();
            cmbDepartment.Items.Add("All Departments");
            foreach (DataRow row in departments.Rows)
            {
                cmbDepartment.Items.Add(row["department_name"].ToString());
            }
            cmbDepartment.SelectedIndex = 0;
        }
        
        private void LoadStatusOptions()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new string[] { "All", "Pending", "Calculated", "Approved", "Rejected", "Paid" });
            cmbStatus.SelectedIndex = 0;
        }
        
        private void SetDefaultDates()
        {
            var now = DateTime.Now;
            dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
            dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }
        
        private void LoadPayrollData()
        {
            try
            {
                statusLabel.Text = "Loading payroll data...";
                
                var query = @"
                    SELECT p.payroll_id as PayrollID, e.employee_id as EmployeeID, e.first_name + ' ' + e.last_name AS EmployeeName,
                           d.department_name, jt.job_title_name as JobTitleName, p.basic_salary as BasicSalary, p.overtime as Overtime, p.allowances as Allowances,
                           p.deductions as Deductions, p.net_pay as NetPay, p.payroll_date as PayrollDate, p.status as Status, p.period_start as PeriodStart, p.period_end as PeriodEnd
                    FROM payroll_details p
                    INNER JOIN employees e ON p.employee_id = e.employee_id
                     INNER JOIN departments d ON e.department_id = d.department_id
                     LEFT JOIN job_titles jt ON e.job_title_id = jt.job_title_id
                    WHERE p.payroll_date BETWEEN @FromDate AND @ToDate
                     ORDER BY p.payroll_date DESC, e.last_name, e.first_name";
                
                var parameters = new[]
                {
                    DatabaseManager.CreateParameter("@FromDate", dtpFromDate.Value.Date),
                    DatabaseManager.CreateParameter("@ToDate", dtpToDate.Value.Date.AddDays(1).AddSeconds(-1))
                };
                
                payrollData = DatabaseManager.GetDataTable(query, parameters);
                ApplyFilters();
                
                statusLabel.Text = "Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payroll data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error loading data";
            }
        }
        
        private void ApplyFilters()
        {
            if (payrollData == null) return;
            
            var filteredData = payrollData.AsEnumerable();
            
            // Apply department filter
            if (cmbDepartment.SelectedIndex > 0)
            {
                var selectedDept = cmbDepartment.SelectedItem.ToString();
                filteredData = filteredData.Where(row => row.Field<string>("department_name") == selectedDept);
            }
            
            // Apply status filter
            if (cmbStatus.SelectedIndex > 0)
            {
                var selectedStatus = cmbStatus.SelectedItem.ToString();
                filteredData = filteredData.Where(row => row.Field<string>("Status") == selectedStatus);
            }
            
            // Apply search filter
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchText = txtSearch.Text.ToLower();
                filteredData = filteredData.Where(row => 
                    row.Field<string>("EmployeeName").ToLower().Contains(searchText) ||
                    row.Field<string>("EmployeeID").ToString().Contains(searchText));
            }
            
            var filteredTable = filteredData.Any() ? filteredData.CopyToDataTable() : payrollData.Clone();
            dgvPayroll.DataSource = filteredTable;
            
            UpdateSummary(filteredTable);
            FormatDataGrid();
        }
        
        private void UpdateSummary(DataTable data)
        {
            var totalEmployees = data.Rows.Count;
            var totalAmount = data.AsEnumerable().Sum(row => row.Field<decimal?>("NetPay") ?? 0);
            
            lblTotalEmployees.Text = $"Total Employees: {totalEmployees}";
            lblTotalAmount.Text = $"Total Amount: ₱{totalAmount:N2}";
        }
        
        private void FormatDataGrid()
        {
            if (dgvPayroll.Columns.Count == 0) return;
            
            dgvPayroll.Columns["PayrollID"].Visible = false;
            dgvPayroll.Columns["EmployeeID"].HeaderText = "Employee ID";
            dgvPayroll.Columns["EmployeeName"].HeaderText = "Employee Name";
            dgvPayroll.Columns["department_name"].HeaderText = "Department";
            dgvPayroll.Columns["JobTitleName"].HeaderText = "Job Title";
            dgvPayroll.Columns["BasicSalary"].HeaderText = "Basic Salary";
            dgvPayroll.Columns["Overtime"].HeaderText = "Overtime";
            dgvPayroll.Columns["Allowances"].HeaderText = "Allowances";
            dgvPayroll.Columns["Deductions"].HeaderText = "Deductions";
            dgvPayroll.Columns["NetPay"].HeaderText = "Net Pay";
            dgvPayroll.Columns["PayrollDate"].HeaderText = "Payroll Date";
            dgvPayroll.Columns["Status"].HeaderText = "Status";
            dgvPayroll.Columns["PeriodStart"].HeaderText = "Period Start";
            dgvPayroll.Columns["PeriodEnd"].HeaderText = "Period End";
            
            // Format currency columns
            var currencyColumns = new[] { "BasicSalary", "Overtime", "Allowances", "Deductions", "NetPay" };
            foreach (var colName in currencyColumns)
            {
                if (dgvPayroll.Columns[colName] != null)
                {
                    dgvPayroll.Columns[colName].DefaultCellStyle.Format = "C2";
                    dgvPayroll.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            
            // Format date columns
            var dateColumns = new[] { "PayrollDate", "PeriodStart", "PeriodEnd" };
            foreach (var colName in dateColumns)
            {
                if (dgvPayroll.Columns[colName] != null)
                {
                    dgvPayroll.Columns[colName].DefaultCellStyle.Format = "MM/dd/yyyy";
                }
            }
        }
        
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            var frmGenerate = new frmPayrollGenerationList();
            if (frmGenerate.ShowDialog() == DialogResult.OK)
            {
                LoadPayrollData();
            }
        }
        
        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            if (dgvPayroll.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select payroll records to calculate.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (MessageBox.Show("Are you sure you want to calculate the selected payroll records?", 
                "Confirm Calculation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CalculateSelectedPayroll();
            }
        }
        
        private void CalculateSelectedPayroll()
        {
            try
            {
                progressBar.Visible = true;
                progressBar.Maximum = dgvPayroll.SelectedRows.Count;
                progressBar.Value = 0;
                
                foreach (DataGridViewRow row in dgvPayroll.SelectedRows)
                {
                    var payrollId = Convert.ToInt32(row.Cells["PayrollID"].Value);
                    CalculatePayroll(payrollId);
                    progressBar.Value++;
                }
                
                progressBar.Visible = false;
                MessageBox.Show("Payroll calculation completed successfully.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPayrollData();
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                MessageBox.Show($"Error calculating payroll: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void CalculatePayroll(int payrollId)
        {
            // Implementation for payroll calculation logic
            var updateQuery = @"
                UPDATE Payroll 
                SET Status = 'Calculated', 
                    NetPay = BasicSalary + Overtime + Allowances - Deductions,
                    CalculatedDate = GETDATE()
                WHERE PayrollID = @PayrollID";
            
            var parameters = new[] { DatabaseManager.CreateParameter("@PayrollID", payrollId) };
            DatabaseManager.ExecuteNonQuery(updateQuery, parameters);
        }
        
        private void BtnApprove_Click(object sender, EventArgs e)
        {
            if (dgvPayroll.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select payroll records to approve.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (MessageBox.Show("Are you sure you want to approve the selected payroll records?", 
                "Confirm Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UpdatePayrollStatus("Approved");
            }
        }
        
        private void BtnReject_Click(object sender, EventArgs e)
        {
            if (dgvPayroll.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select payroll records to reject.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (MessageBox.Show("Are you sure you want to reject the selected payroll records?", 
                "Confirm Rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UpdatePayrollStatus("Rejected");
            }
        }
        
        private async void UpdatePayrollStatus(string status)
        {
            try
            {
                foreach (DataGridViewRow row in dgvPayroll.SelectedRows)
                {
                    var payrollId = Convert.ToInt32(row.Cells["PayrollID"].Value);
                    var currentStatus = row.Cells["Status"].Value.ToString();
                    
                    // Validate status transition
                    if (!IsValidStatusTransition(currentStatus, status))
                    {
                        MessageBox.Show($"Invalid status transition from {currentStatus} to {status}", 
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    var updateQuery = @"
                        UPDATE payroll_details 
                        SET Status = @Status,
                            calculated_by = CASE WHEN @Status = 'Calculated' THEN @UserId ELSE calculated_by END,
                            calculated_date = CASE WHEN @Status = 'Calculated' THEN NOW() ELSE calculated_date END,
                            approved_by = CASE WHEN @Status = 'Approved' THEN @UserId ELSE approved_by END,
                            approved_date = CASE WHEN @Status = 'Approved' THEN NOW() ELSE approved_date END,
                            paid_by = CASE WHEN @Status = 'Paid' THEN @UserId ELSE paid_by END,
                            paid_date = CASE WHEN @Status = 'Paid' THEN NOW() ELSE paid_date END,
                            remarks = @Remarks
                        WHERE PayrollID = @PayrollID";
                    
                    var parameters = new[]
                    {
                        DatabaseManager.CreateParameter("@Status", status),
                        DatabaseManager.CreateParameter("@PayrollID", payrollId),
                        DatabaseManager.CreateParameter("@UserId", GlobalVariables.CurrentUserId),
                        DatabaseManager.CreateParameter("@Remarks", txtRemarks.Text.Trim())
                    };
                    
                    DatabaseManager.ExecuteNonQuery(updateQuery, parameters);

                    // Send email notification if enabled
                    await SendStatusUpdateEmail(payrollId, status);
                }
                
                MessageBox.Show($"Payroll records {status.ToLower()} successfully.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPayrollData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating payroll status: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
            // Define valid status transitions
            var validTransitions = new Dictionary<string, string[]>
            {
                { "Draft", new[] { "Calculated" } },
                { "Calculated", new[] { "Approved", "Draft" } },
                { "Approved", new[] { "Paid", "Calculated" } },
                { "Paid", new[] { "Approved" } }
            };

            return validTransitions.ContainsKey(currentStatus) && 
                   validTransitions[currentStatus].Contains(newStatus);
        }

        private async Task SendStatusUpdateEmail(int payrollId, string status)
        {
            try
            {
                // Get employee email and payroll details
                string query = @"
                    SELECT e.email, e.first_name, e.last_name, pp.period_name,
                           pd.basic_pay, pd.net_pay
                    FROM payroll_details pd
                    JOIN employees e ON pd.employee_id = e.employee_id
                    JOIN payroll_periods pp ON pd.period_id = pp.period_id
                    WHERE pd.payroll_id = @PayrollID";

                var parameters = new[] { DatabaseManager.CreateParameter("@PayrollID", payrollId) };
                var dt = DatabaseManager.GetDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    string employeeEmail = row["email"].ToString();
                    string employeeName = $"{row["first_name"]} {row["last_name"]}".Trim();
                    string periodName = row["period_name"].ToString();
                    decimal netPay = Convert.ToDecimal(row["net_pay"]);

                    // Send email notification
                    await EmailManager.SendPayslipEmailAsync(
                        employeeEmail,
                        employeeName,
                        periodName,
                        null // No attachment for status updates
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending status update email: {ex.Message}");
                // Don't throw the error as this is a non-critical operation
            }
        }
          
          private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    DefaultExt = "csv",
                    FileName = $"PayrollReport_{DateTime.Now:yyyyMMdd}.csv"
                };
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(saveDialog.FileName);
                    MessageBox.Show("Payroll data exported successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ExportToCSV(string fileName)
        {
            var data = (DataTable)dgvPayroll.DataSource;
            if (data == null || data.Rows.Count == 0) return;
            
            using (var writer = new System.IO.StreamWriter(fileName))
            {
                // Write headers
                var headers = data.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                writer.WriteLine(string.Join(",", headers));
                
                // Write data
                foreach (DataRow row in data.Rows)
                {
                    var fields = row.ItemArray.Select(field => $"\"{field}\"");
                    writer.WriteLine(string.Join(",", fields));
                }
            }
        }
        
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadPayrollData();
        }
        
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        
        private void FilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // DatabaseManager is static, no disposal needed
            base.OnFormClosing(e);
        }
    }
}
