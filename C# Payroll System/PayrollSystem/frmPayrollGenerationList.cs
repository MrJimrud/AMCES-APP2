using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PayrollSystem
{
    public partial class frmPayrollGenerationList : Form
    {
        // DatabaseManager is static - no instance needed
        private DataTable generationData;
        
        // Controls
        private ComboBox cmbPeriod;
        private ComboBox cmbDepartment;
        private ComboBox cmbStatus;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private DataGridView dgvGenerations;
        private Button btnGenerate;
        private Button btnRegenerate;
        private Button btnView;
        private Button btnDelete;
        private Button btnExport;
        private Button btnRefresh;
        private Button btnClose;
        private TextBox txtSearch;
        private Label lblTotalGenerations;
        private Label lblTotalEmployees;
        private Label lblTotalAmount;
        private ProgressBar progressBar;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolStripProgressBar statusProgress;
        
        public frmPayrollGenerationList()
        {
            InitializeComponent();
            LoadInitialData();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Payroll Generation Management";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            
            CreateControls();
            LayoutControls();
            AttachEvents();
        }
        
        private void CreateControls()
        {
            // Filter controls
            cmbPeriod = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbDepartment = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            dtpFromDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            dtpToDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            txtSearch = new TextBox { Size = new Size(200, 23) };
            txtSearch.SetPlaceholderText("Search generation...");
            
            // Data grid
            dgvGenerations = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            
            // Buttons
            btnGenerate = new Button { Text = "Generate New", Size = new Size(120, 30) };
            btnRegenerate = new Button { Text = "Regenerate", Size = new Size(100, 30) };
            btnView = new Button { Text = "View Details", Size = new Size(100, 30) };
            btnDelete = new Button { Text = "Delete", Size = new Size(80, 30) };
            btnExport = new Button { Text = "Export", Size = new Size(80, 30) };
            btnRefresh = new Button { Text = "Refresh", Size = new Size(80, 30) };
            btnClose = new Button { Text = "Close", Size = new Size(80, 30) };
            
            // Summary labels
            lblTotalGenerations = new Label { Text = "Total Generations: 0", AutoSize = true };
            lblTotalEmployees = new Label { Text = "Total Employees: 0", AutoSize = true };
            lblTotalAmount = new Label { Text = "Total Amount: ₱0.00", AutoSize = true };
            
            // Progress bar
            progressBar = new ProgressBar { Visible = false };
            
            // Status strip
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Ready");
            statusProgress = new ToolStripProgressBar { Visible = false };
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, statusProgress });
        }
        
        private void LayoutControls()
        {
            // Filter panel
            var filterPanel = new Panel { Height = 80, Dock = DockStyle.Top };
            
            var lblPeriod = new Label { Text = "Period:", Location = new Point(10, 15), AutoSize = true };
            cmbPeriod.Location = new Point(60, 12);
            cmbPeriod.Size = new Size(200, 25);
            
            var lblDept = new Label { Text = "Department:", Location = new Point(270, 15), AutoSize = true };
            cmbDepartment.Location = new Point(340, 12);
            cmbDepartment.Size = new Size(150, 25);
            
            var lblStatus = new Label { Text = "Status:", Location = new Point(500, 15), AutoSize = true };
            cmbStatus.Location = new Point(540, 12);
            cmbStatus.Size = new Size(120, 25);
            
            var lblFrom = new Label { Text = "From:", Location = new Point(10, 45), AutoSize = true };
            dtpFromDate.Location = new Point(50, 42);
            dtpFromDate.Size = new Size(100, 25);
            
            var lblTo = new Label { Text = "To:", Location = new Point(160, 45), AutoSize = true };
            dtpToDate.Location = new Point(180, 42);
            dtpToDate.Size = new Size(100, 25);
            
            var lblSearch = new Label { Text = "Search:", Location = new Point(290, 45), AutoSize = true };
            txtSearch.Location = new Point(340, 42);
            txtSearch.Size = new Size(200, 25);
            
            btnRefresh.Location = new Point(550, 42);
            
            filterPanel.Controls.AddRange(new Control[] {
                lblPeriod, cmbPeriod, lblDept, cmbDepartment, lblStatus, cmbStatus,
                lblFrom, dtpFromDate, lblTo, dtpToDate, lblSearch, txtSearch, btnRefresh
            });
            
            // Button panel
            var buttonPanel = new Panel { Height = 50, Dock = DockStyle.Bottom };
            
            btnGenerate.Location = new Point(10, 10);
            btnRegenerate.Location = new Point(140, 10);
            btnView.Location = new Point(250, 10);
            btnDelete.Location = new Point(360, 10);
            btnExport.Location = new Point(450, 10);
            btnClose.Location = new Point(540, 10);
            
            lblTotalGenerations.Location = new Point(650, 10);
            lblTotalEmployees.Location = new Point(650, 25);
            lblTotalAmount.Location = new Point(800, 15);
            
            buttonPanel.Controls.AddRange(new Control[] {
                btnGenerate, btnRegenerate, btnView, btnDelete, btnExport, btnClose,
                lblTotalGenerations, lblTotalEmployees, lblTotalAmount
            });
            
            // Progress panel
            var progressPanel = new Panel { Height = 30, Dock = DockStyle.Bottom };
            progressBar.Dock = DockStyle.Fill;
            progressPanel.Controls.Add(progressBar);
            
            // Main panel for data grid
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            dgvGenerations.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(dgvGenerations);
            
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
            btnRegenerate.Click += BtnRegenerate_Click;
            btnView.Click += BtnView_Click;
            btnDelete.Click += BtnDelete_Click;
            btnExport.Click += BtnExport_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnClose.Click += BtnClose_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            cmbPeriod.SelectedIndexChanged += FilterChanged;
            cmbDepartment.SelectedIndexChanged += FilterChanged;
            cmbStatus.SelectedIndexChanged += FilterChanged;
            dtpFromDate.ValueChanged += FilterChanged;
            dtpToDate.ValueChanged += FilterChanged;
            dgvGenerations.SelectionChanged += DgvGenerations_SelectionChanged;
            dgvGenerations.CellDoubleClick += DgvGenerations_CellDoubleClick;
        }
        
        private void LoadInitialData()
        {
            try
            {
                LoadPeriods();
                LoadDepartments();
                LoadStatusOptions();
                SetDefaultDates();
                LoadGenerationData();
                
                // Ensure data is displayed immediately after form load
                if (generationData != null)
                {
                    dgvGenerations.DataSource = generationData;
                    FormatDataGrid();
                    UpdateSummary(generationData);
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading initial data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadPeriods()
        {
            var periods = DatabaseManager.GetPayrollPeriods();
            cmbPeriod.Items.Clear();
            cmbPeriod.Items.Add("All Periods");
            foreach (DataRow row in periods.Rows)
            {
                cmbPeriod.Items.Add($"{row["period_name"]} ({row["start_date"]:MM/dd/yyyy} - {row["end_date"]:MM/dd/yyyy})");
            }
            cmbPeriod.SelectedIndex = 0;
        }
        
        private void LoadDepartments()
        {
            var departments = DatabaseManager.GetDepartments();
            cmbDepartment.DataSource = null;
            cmbDepartment.Items.Clear();
            
            // Create a new DataTable with an "All Departments" option
            DataTable dtDepartments = new DataTable();
            dtDepartments.Columns.Add("department_id", typeof(int));
            dtDepartments.Columns.Add("department_name", typeof(string));
            
            // Add "All Departments" as the first option
            dtDepartments.Rows.Add(0, "All Departments");
            
            // Add all departments from the database
            foreach (DataRow row in departments.Rows)
            {
                dtDepartments.Rows.Add(row["department_id"], row["department_name"]);
            }
            
            // Set up the ComboBox with the DataTable
            cmbDepartment.DataSource = dtDepartments;
            cmbDepartment.DisplayMember = "department_name";
            cmbDepartment.ValueMember = "department_id";
            cmbDepartment.SelectedIndex = 0;
        }
        
        private void LoadStatusOptions()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new string[] { "All", "In Progress", "Completed", "Failed", "Cancelled" });
            cmbStatus.SelectedIndex = 0;
        }
        
        private void SetDefaultDates()
        {
            var now = DateTime.Now;
            dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
            dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }
        
        private void LoadGenerationData()
        {
            try
            {
                statusLabel.Text = "Loading payroll generations...";
                
                var query = @"
                    SELECT 
                        pd.period_id as GenerationID, 
                        pd.created_at as GenerationName, 
                        pd.period_id as PeriodID, 
                        pp.period_name,
                        e.department_id as DepartmentID, 
                        d.department_name, 
                        pd.created_at as GenerationDate, 
                        pd.status as Status,
                        COUNT(pd.employee_id) as TotalEmployees, 
                        SUM(pd.net_pay) as TotalAmount, 
                        'System' as CreatedBy, 
                        pd.created_at as CreatedDate,
                        pd.created_at as CompletedDate, 
                        '' as ErrorMessage, 
                        '' as Notes
                    FROM payroll_details pd
                    LEFT JOIN payroll_periods pp ON pd.period_id = pp.period_id
                    LEFT JOIN employees e ON pd.employee_id = e.employee_id
                    LEFT JOIN departments d ON e.department_id = d.department_id
                    GROUP BY pd.period_id, DATE(pd.created_at), pd.status
                    ORDER BY pd.created_at DESC";
                
                var parameters = new[]
                {
                    DatabaseManager.CreateParameter("@FromDate", dtpFromDate.Value.Date),
                    DatabaseManager.CreateParameter("@ToDate", dtpToDate.Value.Date.AddDays(1).AddSeconds(-1))
                };
                
                generationData = DatabaseManager.GetDataTable(query, parameters);
                ApplyFilters();
                
                statusLabel.Text = "Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading generation data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error loading data";
            }
        }
        
        private void ApplyFilters()
        {
            if (generationData == null) return;
            
            var filteredData = generationData.AsEnumerable();
            
            // Apply period filter
            if (cmbPeriod.SelectedIndex > 0)
            {
                var selectedPeriod = cmbPeriod.SelectedItem.ToString();
                var periodName = selectedPeriod.Split('(')[0].Trim();
                filteredData = filteredData.Where(row => 
                {
                    var rowPeriodName = row.Field<string>("period_name");
                    return rowPeriodName != null && rowPeriodName.Equals(periodName, StringComparison.OrdinalIgnoreCase);
                });
            }
            
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
                    row.Field<DateTime>("GenerationName").ToString().ToLower().Contains(searchText) ||
                    row.Field<string>("period_name")?.ToLower().Contains(searchText) == true);
            }
            
            var filteredTable = filteredData.Any() ? filteredData.CopyToDataTable() : generationData.Clone();
            dgvGenerations.DataSource = filteredTable;
            
            UpdateSummary(filteredTable);
            FormatDataGrid();
            UpdateButtonStates();
        }
        
        private void UpdateSummary(DataTable data)
        {
            var totalGenerations = data.Rows.Count;
            var totalEmployees = data.AsEnumerable().Sum(row => 
            {
                // Handle different possible types for TotalEmployees
                if (row["TotalEmployees"] == DBNull.Value) return 0;
                if (row["TotalEmployees"] is long longVal) return (int)longVal;
                if (row["TotalEmployees"] is int intVal) return intVal;
                if (row["TotalEmployees"] is decimal decVal) return (int)decVal;
                if (row["TotalEmployees"] is double dblVal) return (int)dblVal;
                // Try to convert to string and parse
                return int.TryParse(row["TotalEmployees"].ToString(), out int result) ? result : 0;
            });
            var totalAmount = data.AsEnumerable().Sum(row => row.Field<decimal?>("TotalAmount") ?? 0);
            
            lblTotalGenerations.Text = $"Total Generations: {totalGenerations}";
            lblTotalEmployees.Text = $"Total Employees: {totalEmployees}";
            lblTotalAmount.Text = $"Total Amount: ₱{totalAmount:N2}";
        }
        
        private void FormatDataGrid()
        {
            if (dgvGenerations.Columns.Count == 0) return;
            
            dgvGenerations.Columns["GenerationID"].Visible = false;
            dgvGenerations.Columns["PeriodID"].Visible = false;
            dgvGenerations.Columns["DepartmentID"].Visible = false;
            dgvGenerations.Columns["GenerationName"].HeaderText = "Generation Name";
            dgvGenerations.Columns["period_name"].HeaderText = "Period";
            dgvGenerations.Columns["department_name"].HeaderText = "Department";
            dgvGenerations.Columns["GenerationDate"].HeaderText = "Generation Date";
            dgvGenerations.Columns["Status"].HeaderText = "Status";
            dgvGenerations.Columns["TotalEmployees"].HeaderText = "Employees";
            dgvGenerations.Columns["TotalAmount"].HeaderText = "Total Amount";
            dgvGenerations.Columns["CreatedBy"].HeaderText = "Created By";
            dgvGenerations.Columns["CreatedDate"].HeaderText = "Created Date";
            dgvGenerations.Columns["CompletedDate"].HeaderText = "Completed Date";
            dgvGenerations.Columns["ErrorMessage"].HeaderText = "Error Message";
            dgvGenerations.Columns["Notes"].HeaderText = "Notes";
            
            // Format currency columns
            if (dgvGenerations.Columns["TotalAmount"] != null)
            {
                dgvGenerations.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
                dgvGenerations.Columns["TotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            // Format date columns
            var dateColumns = new[] { "GenerationDate", "CreatedDate", "CompletedDate" };
            foreach (var colName in dateColumns)
            {
                if (dgvGenerations.Columns[colName] != null)
                {
                    dgvGenerations.Columns[colName].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm";
                }
            }
            
            // Format status column with colors
            if (dgvGenerations.Columns["Status"] != null)
            {
                foreach (DataGridViewRow row in dgvGenerations.Rows)
                {
                    var status = row.Cells["Status"].Value?.ToString();
                    switch (status)
                    {
                        case "Completed":
                            row.Cells["Status"].Style.BackColor = Color.LightGreen;
                            break;
                        case "Failed":
                            row.Cells["Status"].Style.BackColor = Color.LightCoral;
                            break;
                        case "In Progress":
                            row.Cells["Status"].Style.BackColor = Color.LightYellow;
                            break;
                        case "Cancelled":
                            row.Cells["Status"].Style.BackColor = Color.LightGray;
                            break;
                    }
                }
            }
        }
        
        private void UpdateButtonStates()
        {
            var hasSelection = dgvGenerations.SelectedRows.Count > 0;
            var selectedStatus = hasSelection ? dgvGenerations.SelectedRows[0].Cells["Status"].Value?.ToString() : "";
            
            btnRegenerate.Enabled = hasSelection && (selectedStatus == "Failed" || selectedStatus == "Cancelled");
            btnView.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection && selectedStatus != "In Progress";
            btnExport.Enabled = hasSelection && selectedStatus == "Completed";
        }
        
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            var frmGenerate = new frmPayrollGenerateNew();
            if (frmGenerate.ShowDialog() == DialogResult.OK)
            {
                LoadGenerationData();
            }
        }
        
        private void BtnRegenerate_Click(object sender, EventArgs e)
        {
            if (dgvGenerations.SelectedRows.Count == 0) return;
            
            var generationId = Convert.ToInt32(dgvGenerations.SelectedRows[0].Cells["GenerationID"].Value);
            var generationName = dgvGenerations.SelectedRows[0].Cells["GenerationName"].Value.ToString();
            
            if (MessageBox.Show($"Are you sure you want to regenerate '{generationName}'?\n\nThis will recreate all payroll records for this generation.", 
                "Confirm Regeneration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RegeneratePayroll(generationId);
            }
        }
        
        private async void RegeneratePayroll(int generationId)
        {
            try
            {
                progressBar.Visible = true;
                statusProgress.Visible = true;
                statusLabel.Text = "Regenerating payroll...";
                
                // Update status to In Progress
                var updateQuery = "UPDATE payroll_generations SET status = 'In Progress', error_message = NULL WHERE generation_id = @GenerationID";
                var updateParams = new[] { DatabaseManager.CreateParameter("@GenerationID", generationId) };
                DatabaseManager.ExecuteNonQuery(updateQuery, updateParams);
                
                // Delete existing payroll records for this generation
                var deleteQuery = "DELETE FROM payroll_details WHERE period_id = @GenerationID";
                var deleteParams = new[] { DatabaseManager.CreateParameter("@GenerationID", generationId) };
                DatabaseManager.ExecuteNonQuery(deleteQuery, deleteParams);
                
                // Regenerate payroll
                await Task.Run(() => GeneratePayrollRecords(generationId));
                
                progressBar.Visible = false;
                statusProgress.Visible = false;
                statusLabel.Text = "Ready";
                
                MessageBox.Show("Payroll regenerated successfully.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGenerationData();
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                statusProgress.Visible = false;
                statusLabel.Text = "Error";
                
                // Update generation status to Failed
                var errorQuery = "UPDATE payroll_generations SET status = 'Failed', error_message = @ErrorMessage WHERE generation_id = @GenerationID";
                var errorParams = new[]
                {
                    DatabaseManager.CreateParameter("@ErrorMessage", ex.Message),
                    DatabaseManager.CreateParameter("@GenerationID", generationId)
                };
                DatabaseManager.ExecuteNonQuery(errorQuery, errorParams);
                
                MessageBox.Show($"Error regenerating payroll: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadGenerationData();
            }
        }
        
        private void GeneratePayrollRecords(int generationId)
        {
            // Implementation for generating payroll records
            // This would include complex payroll calculation logic
            
            // Get generation details
            var genQuery = "SELECT * FROM payroll_details WHERE payroll_id = @GenerationID LIMIT 1";
            var genParams = new[] { DatabaseManager.CreateParameter("@GenerationID", generationId) };
            var genData = DatabaseManager.GetDataTable(genQuery, genParams);
            
            if (genData.Rows.Count == 0) return;
            
            var genRow = genData.Rows[0];
            var periodId = Convert.ToInt32(genRow["period_id"]);
            var departmentId = genRow["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(genRow["department_id"]);
            
            // Get employees to process
            var empQuery = departmentId.HasValue ? 
                "SELECT * FROM employees WHERE department_id = @DepartmentID AND is_active = 1" :
                "SELECT * FROM employees WHERE is_active = 1";
            
            var empParams = departmentId.HasValue ? 
                new[] { DatabaseManager.CreateParameter("@DepartmentID", departmentId.Value) } : 
                new MySqlConnector.MySqlParameter[0];
            
            var employees = DatabaseManager.GetDataTable(empQuery, empParams);
            
            var totalEmployees = employees.Rows.Count;
            var processedEmployees = 0;
            var totalAmount = 0m;
            
            foreach (DataRow emp in employees.Rows)
            {
                var employeeId = Convert.ToInt32(emp["EmployeeID"]);
                var basicSalary = Convert.ToDecimal(emp["BasicSalary"]);
                
                // Calculate payroll (simplified)
                var overtime = CalculateOvertime(employeeId, periodId);
                var allowances = CalculateAllowances(employeeId);
                var deductions = CalculateDeductions(employeeId);
                var netPay = basicSalary + overtime + allowances - deductions;
                
                // Insert payroll record
                var insertQuery = @"
                    INSERT INTO payroll_details (employee_id, period_id, basic_pay, overtime_pay, 
                                        Allowances, Deductions, NetPay, PayrollDate, Status)
                    VALUES (@EmployeeID, @PeriodID, @GenerationID, @BasicSalary, @Overtime, 
                           @Allowances, @Deductions, @NetPay, GETDATE(), 'Generated')";
                
                var insertParams = new[]
                {
                    DatabaseManager.CreateParameter("@EmployeeID", employeeId),
                    DatabaseManager.CreateParameter("@PeriodID", periodId),
                    DatabaseManager.CreateParameter("@GenerationID", generationId),
                    DatabaseManager.CreateParameter("@BasicSalary", basicSalary),
                    DatabaseManager.CreateParameter("@Overtime", overtime),
                    DatabaseManager.CreateParameter("@Allowances", allowances),
                    DatabaseManager.CreateParameter("@Deductions", deductions),
                    DatabaseManager.CreateParameter("@NetPay", netPay)
                };
                
                DatabaseManager.ExecuteNonQuery(insertQuery, insertParams);
                
                totalAmount += netPay;
                processedEmployees++;
                
                // Update progress
                var progress = (int)((double)processedEmployees / totalEmployees * 100);
                statusProgress.Value = progress;
            }
            
            // Update generation status
            var completeQuery = @"
                UPDATE payroll_generations 
                SET Status = 'Completed', TotalEmployees = @TotalEmployees, TotalAmount = @TotalAmount, 
                    CompletedDate = GETDATE()
                WHERE GenerationID = @GenerationID";
            
            var completeParams = new[]
            {
                DatabaseManager.CreateParameter("@TotalEmployees", processedEmployees),
                DatabaseManager.CreateParameter("@TotalAmount", totalAmount),
                DatabaseManager.CreateParameter("@GenerationID", generationId)
            };
            
            DatabaseManager.ExecuteNonQuery(completeQuery, completeParams);
        }
        
        private decimal CalculateOvertime(int employeeId, int periodId)
        {
            // Simplified overtime calculation
            var query = @"
                SELECT SUM(CASE WHEN DATEDIFF(minute, TimeIn, TimeOut) > 480 
                               THEN (DATEDIFF(minute, TimeIn, TimeOut) - 480) * 0.02 
                               ELSE 0 END) AS OvertimeAmount
                FROM DTR 
                WHERE EmployeeID = @EmployeeID AND DTRDate BETWEEN 
                      (SELECT start_date FROM payroll_periods WHERE period_id = @PeriodID) AND 
                      (SELECT end_date FROM payroll_periods WHERE period_id = @PeriodID)";
            
            var parameters = new[]
            {
                DatabaseManager.CreateParameter("@EmployeeID", employeeId),
                DatabaseManager.CreateParameter("@PeriodID", periodId)
            };
            
            var result = DatabaseManager.ExecuteScalar(query, parameters);
            return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
        }
        
        private decimal CalculateAllowances(int employeeId)
        {
            // Simplified allowances calculation
            return 1000; // Fixed allowance for demo
        }
        
        private decimal CalculateDeductions(int employeeId)
        {
            // Simplified deductions calculation
            return 500; // Fixed deduction for demo
        }
        
        private void BtnView_Click(object sender, EventArgs e)
        {
            if (dgvGenerations.SelectedRows.Count == 0) return;
            
            var generationId = Convert.ToInt32(dgvGenerations.SelectedRows[0].Cells["GenerationID"].Value);
            var frmDetails = new frmPayrollGenerationDetails(generationId);
            frmDetails.ShowDialog();
        }
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvGenerations.SelectedRows.Count == 0) return;
            
            var generationName = dgvGenerations.SelectedRows[0].Cells["GenerationName"].Value.ToString();
            
            if (MessageBox.Show($"Are you sure you want to delete the generation '{generationName}'?\n\nThis will also delete all associated payroll records.\n\nThis action cannot be undone.", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DeleteGeneration();
            }
        }
        
        private void DeleteGeneration()
        {
            try
            {
                var generationId = Convert.ToInt32(dgvGenerations.SelectedRows[0].Cells["GenerationID"].Value);
                
                // Delete payroll records first
                var deletePayrollQuery = "DELETE FROM payroll_details WHERE period_id = @GenerationID";
                var payrollParams = new[] { DatabaseManager.CreateParameter("@GenerationID", generationId) };
                DatabaseManager.ExecuteNonQuery(deletePayrollQuery, payrollParams);
                
                // No need to delete from payroll_generations as we're using payroll_details directly
                
                MessageBox.Show("Generation deleted successfully.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGenerationData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting generation: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvGenerations.SelectedRows.Count == 0) return;
            
            try
            {
                var generationId = Convert.ToInt32(dgvGenerations.SelectedRows[0].Cells["GenerationID"].Value);
                var generationName = dgvGenerations.SelectedRows[0].Cells["GenerationName"].Value.ToString();
                
                var saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    DefaultExt = "csv",
                    FileName = $"{generationName}_{DateTime.Now:yyyyMMdd}.csv"
                };
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportGenerationToCSV(generationId, saveDialog.FileName);
                    MessageBox.Show("Generation data exported successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ExportGenerationToCSV(int generationId, string fileName)
        {
            var query = @"
                SELECT e.EmployeeID, e.FirstName + ' ' + e.LastName AS EmployeeName,
                       d.DepartmentName, jt.JobTitleName, p.BasicSalary, p.Overtime, 
                       p.Allowances, p.Deductions, p.NetPay, p.PayrollDate
                FROM payroll_details p
                INNER JOIN Employees e ON p.EmployeeID = e.EmployeeID
                INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
                INNER JOIN JobTitles jt ON e.JobTitleID = jt.JobTitleID
                WHERE p.GenerationID = @GenerationID
                ORDER BY e.LastName, e.FirstName";
            
            var parameters = new[] { DatabaseManager.CreateParameter("@GenerationID", generationId) };
            var data = DatabaseManager.GetDataTable(query, parameters);
            
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
            LoadGenerationData();
            
            // Ensure data is displayed after refresh
            if (generationData != null)
            {
                dgvGenerations.DataSource = generationData;
                FormatDataGrid();
                UpdateSummary(generationData);
                UpdateButtonStates();
            }
        }
        
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Only apply filters if we have data loaded
            if (generationData != null)
            {
                ApplyFilters();
            }
        }
        
        private void FilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        
        private void DgvGenerations_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }
        
        private void DgvGenerations_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnView_Click(sender, e);
            }
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // DatabaseManager is static, no disposal needed
            base.OnFormClosing(e);
        }
    }
    
    // Helper forms (simplified implementations)
    public partial class frmPayrollGenerateNew : Form
    {
        public frmPayrollGenerateNew()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Generate New Payroll";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            
            var btnOK = new Button { Text = "Generate", DialogResult = DialogResult.OK, Location = new Point(200, 200) };
            var btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(280, 200) };
            
            this.Controls.AddRange(new Control[] { btnOK, btnCancel });
        }
    }
    
    public partial class frmPayrollGenerationDetails : Form
    {
        private int generationId;
        
        public frmPayrollGenerationDetails(int generationId)
        {
            this.generationId = generationId;
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Payroll Generation Details";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            
            var btnClose = new Button { Text = "Close", DialogResult = DialogResult.OK, Location = new Point(700, 500) };
            this.Controls.Add(btnClose);
        }
    }
}
