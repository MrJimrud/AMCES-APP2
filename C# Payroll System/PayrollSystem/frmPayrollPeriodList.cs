using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PayrollSystem
{
    public partial class frmPayrollPeriodList : Form
    {
        // DatabaseManager is static - no instance needed
        private DataTable periodData;
        
        // Controls
        private DataGridView dgvPeriods;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnActivate;
        private Button btnDeactivate;
        private Button btnRefresh;
        private Button btnClose;
        private TextBox txtSearch;
        private ComboBox cmbStatus;
        private ComboBox cmbYear;
        private Label lblTotalPeriods;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        
        public frmPayrollPeriodList()
        {
            InitializeComponent();
            LoadInitialData();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Payroll Period Management";
            this.Size = new Size(1000, 600);
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
            txtSearch = new TextBox();
            txtSearch.SetPlaceholderText("Search period name...");
            cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbYear = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            
            // Data grid
            dgvPeriods = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            
            // Buttons
            btnAdd = new Button { Text = "Add Period", Size = new Size(100, 30) };
            btnEdit = new Button { Text = "Edit", Size = new Size(80, 30) };
            btnDelete = new Button { Text = "Delete", Size = new Size(80, 30) };
            btnActivate = new Button { Text = "Activate", Size = new Size(80, 30) };
            btnDeactivate = new Button { Text = "Deactivate", Size = new Size(90, 30) };
            btnRefresh = new Button { Text = "Refresh", Size = new Size(80, 30) };
            btnClose = new Button { Text = "Close", Size = new Size(80, 30) };
            
            // Summary label
            lblTotalPeriods = new Label { Text = "Total Periods: 0", AutoSize = true };
            
            // Status strip
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Ready");
            statusStrip.Items.Add(statusLabel);
        }
        
        private void LayoutControls()
        {
            // Filter panel
            var filterPanel = new Panel { Height = 50, Dock = DockStyle.Top };
            
            var lblSearch = new Label { Text = "Search:", Location = new Point(10, 15), AutoSize = true };
            txtSearch.Location = new Point(60, 12);
            txtSearch.Size = new Size(200, 25);
            
            var lblStatus = new Label { Text = "Status:", Location = new Point(280, 15), AutoSize = true };
            cmbStatus.Location = new Point(325, 12);
            cmbStatus.Size = new Size(120, 25);
            
            var lblYear = new Label { Text = "Year:", Location = new Point(460, 15), AutoSize = true };
            cmbYear.Location = new Point(495, 12);
            cmbYear.Size = new Size(80, 25);
            
            btnRefresh.Location = new Point(590, 12);
            
            lblTotalPeriods.Location = new Point(700, 15);
            
            filterPanel.Controls.AddRange(new Control[] {
                lblSearch, txtSearch, lblStatus, cmbStatus, lblYear, cmbYear, btnRefresh, lblTotalPeriods
            });
            
            // Button panel
            var buttonPanel = new Panel { Height = 50, Dock = DockStyle.Bottom };
            
            btnAdd.Location = new Point(10, 10);
            btnEdit.Location = new Point(120, 10);
            btnDelete.Location = new Point(210, 10);
            btnActivate.Location = new Point(300, 10);
            btnDeactivate.Location = new Point(390, 10);
            btnClose.Location = new Point(490, 10);
            
            buttonPanel.Controls.AddRange(new Control[] {
                btnAdd, btnEdit, btnDelete, btnActivate, btnDeactivate, btnClose
            });
            
            // Main panel for data grid
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            dgvPeriods.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(dgvPeriods);
            
            // Add all panels to form
            this.Controls.Add(mainPanel);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(filterPanel);
            this.Controls.Add(statusStrip);
        }
        
        private void AttachEvents()
        {
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnActivate.Click += BtnActivate_Click;
            btnDeactivate.Click += BtnDeactivate_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnClose.Click += BtnClose_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            cmbStatus.SelectedIndexChanged += FilterChanged;
            cmbYear.SelectedIndexChanged += FilterChanged;
            dgvPeriods.SelectionChanged += DgvPeriods_SelectionChanged;
            dgvPeriods.CellDoubleClick += DgvPeriods_CellDoubleClick;
        }
        
        private void LoadInitialData()
        {
            try
            {
                LoadStatusOptions();
                LoadYearOptions();
                LoadPeriodData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading initial data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadStatusOptions()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new string[] { "All", "Active", "Inactive", "Closed" });
            cmbStatus.SelectedIndex = 0;
        }
        
        private void LoadYearOptions()
        {
            cmbYear.Items.Clear();
            cmbYear.Items.Add("All Years");
            
            var currentYear = DateTime.Now.Year;
            for (int year = currentYear - 2; year <= currentYear + 1; year++)
            {
                cmbYear.Items.Add(year.ToString());
            }
            cmbYear.SelectedIndex = 0;
        }
        
        private void LoadPeriodData()
        {
            try
            {
                statusLabel.Text = "Loading payroll periods...";
                
                var query = @"
                    SELECT period_id, period_name, start_date, 
                           end_date, pay_date, status, 
                           is_active, created_at, created_by, 
                           description, DATEDIFF(day, start_date, end_date) + 1 AS DaysCount
                    FROM payroll_periods
                    ORDER BY start_date DESC";
                
                periodData = DatabaseManager.GetDataTable(query);
                ApplyFilters();
                
                statusLabel.Text = "Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading period data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error loading data";
            }
        }
        
        private void ApplyFilters()
        {
            if (periodData == null) return;
            
            var filteredData = periodData.AsEnumerable();
            
            // Apply status filter
            if (cmbStatus.SelectedIndex > 0)
            {
                var selectedStatus = cmbStatus.SelectedItem.ToString();
                filteredData = filteredData.Where(row => row.Field<string>("Status") == selectedStatus);
            }
            
            // Apply year filter
            if (cmbYear.SelectedIndex > 0)
            {
                var selectedYear = int.Parse(cmbYear.SelectedItem.ToString());
                filteredData = filteredData.Where(row => 
                    row.Field<DateTime>("StartDate").Year == selectedYear);
            }
            
            // Apply search filter
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchText = txtSearch.Text.ToLower();
                filteredData = filteredData.Where(row => 
                    row.Field<string>("period_name").ToLower().Contains(searchText));
            }
            
            var filteredTable = filteredData.Any() ? filteredData.CopyToDataTable() : periodData.Clone();
            dgvPeriods.DataSource = filteredTable;
            
            UpdateSummary(filteredTable);
            FormatDataGrid();
            UpdateButtonStates();
        }
        
        private void UpdateSummary(DataTable data)
        {
            lblTotalPeriods.Text = $"Total Periods: {data.Rows.Count}";
        }
        
        private void FormatDataGrid()
        {
            if (dgvPeriods.Columns.Count == 0) return;
            
            dgvPeriods.Columns["period_id"].Visible = false;
            dgvPeriods.Columns["period_name"].HeaderText = "Period Name";
            dgvPeriods.Columns["start_date"].HeaderText = "Start Date";
            dgvPeriods.Columns["end_date"].HeaderText = "End Date";
            dgvPeriods.Columns["pay_date"].HeaderText = "Pay Date";
            dgvPeriods.Columns["status"].HeaderText = "Status";
            dgvPeriods.Columns["is_active"].HeaderText = "Active";
            dgvPeriods.Columns["created_at"].HeaderText = "Created Date";
            dgvPeriods.Columns["created_by"].HeaderText = "Created By";
            dgvPeriods.Columns["description"].HeaderText = "Notes";
            dgvPeriods.Columns["DaysCount"].HeaderText = "Days";
            
            // Format date columns
            var dateColumns = new[] { "StartDate", "EndDate", "PayDate", "CreatedDate" };
            foreach (var colName in dateColumns)
            {
                if (dgvPeriods.Columns[colName] != null)
                {
                    dgvPeriods.Columns[colName].DefaultCellStyle.Format = "MM/dd/yyyy";
                }
            }
            
            // Format boolean column
            if (dgvPeriods.Columns["is_active"] != null)
            {
                dgvPeriods.Columns["is_active"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            
            // Set column widths
            dgvPeriods.Columns["period_name"].Width = 150;
            dgvPeriods.Columns["start_date"].Width = 100;
            dgvPeriods.Columns["end_date"].Width = 100;
            dgvPeriods.Columns["pay_date"].Width = 100;
            dgvPeriods.Columns["status"].Width = 80;
            dgvPeriods.Columns["is_active"].Width = 60;
            dgvPeriods.Columns["DaysCount"].Width = 60;
        }
        
        private void UpdateButtonStates()
        {
            var hasSelection = dgvPeriods.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnActivate.Enabled = hasSelection;
            btnDeactivate.Enabled = hasSelection;
        }
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var frmPeriod = new frmPayrollPeriodEdit();
            if (frmPeriod.ShowDialog() == DialogResult.OK)
            {
                LoadPeriodData();
            }
        }
        
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPeriods.SelectedRows.Count == 0) return;
            
            var periodId = Convert.ToInt32(dgvPeriods.SelectedRows[0].Cells["period_id"].Value);
            var frmPeriod = new frmPayrollPeriodEdit(periodId);
            if (frmPeriod.ShowDialog() == DialogResult.OK)
            {
                LoadPeriodData();
            }
        }
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPeriods.SelectedRows.Count == 0) return;
            
            var periodName = dgvPeriods.SelectedRows[0].Cells["period_name"].Value.ToString();
            
            if (MessageBox.Show($"Are you sure you want to delete the period '{periodName}'?\n\nThis action cannot be undone.", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DeletePeriod();
            }
        }
        
        private void DeletePeriod()
        {
            try
            {
                var periodId = Convert.ToInt32(dgvPeriods.SelectedRows[0].Cells["PeriodID"].Value);
                
                // Check if period has associated payroll records
                var checkQuery = "SELECT COUNT(*) FROM Payroll WHERE PeriodID = @PeriodID";
                var checkParams = new[] { DatabaseManager.CreateParameter("@PeriodID", periodId) };
                var payrollCount = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkQuery, checkParams));
                
                if (payrollCount > 0)
                {
                    MessageBox.Show("Cannot delete this period because it has associated payroll records.", 
                        "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                var deleteQuery = "DELETE FROM payroll_periods WHERE period_id = @PeriodID";
                var deleteParams = new[] { DatabaseManager.CreateParameter("@PeriodID", periodId) };
                
                DatabaseManager.ExecuteNonQuery(deleteQuery, deleteParams);
                
                MessageBox.Show("Period deleted successfully.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPeriodData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting period: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnActivate_Click(object sender, EventArgs e)
        {
            if (dgvPeriods.SelectedRows.Count == 0) return;
            
            UpdatePeriodStatus(true);
        }
        
        private void BtnDeactivate_Click(object sender, EventArgs e)
        {
            if (dgvPeriods.SelectedRows.Count == 0) return;
            
            UpdatePeriodStatus(false);
        }
        
        private void UpdatePeriodStatus(bool isActive)
        {
            try
            {
                var periodId = Convert.ToInt32(dgvPeriods.SelectedRows[0].Cells["period_id"].Value);
                var periodName = dgvPeriods.SelectedRows[0].Cells["period_name"].Value.ToString();
                
                var action = isActive ? "activate" : "deactivate";
                if (MessageBox.Show($"Are you sure you want to {action} the period '{periodName}'?", 
                    $"Confirm {action.Substring(0, 1).ToUpper() + action.Substring(1)}", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var updateQuery = @"
                        UPDATE payroll_periods 
                        SET is_active = @IsActive,
                            status = CASE WHEN @IsActive = 1 THEN 'Active' ELSE 'Inactive' END
                        WHERE period_id = @PeriodID";
                    
                    var parameters = new[]
                    {
                        DatabaseManager.CreateParameter("@IsActive", isActive),
                        DatabaseManager.CreateParameter("@PeriodID", periodId)
                    };
                    
                    DatabaseManager.ExecuteNonQuery(updateQuery, parameters);
                    
                    MessageBox.Show($"Period {action}d successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPeriodData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating period status: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadPeriodData();
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
        
        private void DgvPeriods_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }
        
        private void DgvPeriods_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEdit_Click(sender, e);
            }
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // DatabaseManager is static - no disposal needed
            base.OnFormClosing(e);
        }
    }
    
    // Helper form for editing payroll periods
    public partial class frmPayrollPeriodEdit : Form
    {
        // DatabaseManager is static - no instance needed
        private int? periodId;
        
        // Controls
        private TextBox txtPeriodName;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private DateTimePicker dtpPayDate;
        private ComboBox cmbStatus;
        private CheckBox chkIsActive;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnCancel;
        
        public frmPayrollPeriodEdit(int? periodId = null)
        {
            this.periodId = periodId;
            InitializeComponent();
            
            if (periodId.HasValue)
            {
                LoadPeriodData();
            }
            else
            {
                SetDefaultValues();
            }
        }
        
        private void InitializeComponent()
        {
            this.Text = periodId.HasValue ? "Edit Payroll Period" : "Add Payroll Period";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            CreateControls();
            LayoutControls();
            AttachEvents();
        }
        
        private void CreateControls()
        {
            txtPeriodName = new TextBox();
            dtpStartDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            dtpEndDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            dtpPayDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            chkIsActive = new CheckBox { Text = "Active", Checked = true };
            txtNotes = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical };
            btnSave = new Button { Text = "Save", DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel };
            
            cmbStatus.Items.AddRange(new string[] { "Active", "Inactive", "Closed" });
            cmbStatus.SelectedIndex = 0;
        }
        
        private void LayoutControls()
        {
            var lblPeriodName = new Label { Text = "Period Name:", Location = new Point(20, 20), AutoSize = true };
            txtPeriodName.Location = new Point(120, 17);
            txtPeriodName.Size = new Size(280, 25);
            
            var lblStartDate = new Label { Text = "Start Date:", Location = new Point(20, 55), AutoSize = true };
            dtpStartDate.Location = new Point(120, 52);
            dtpStartDate.Size = new Size(120, 25);
            
            var lblEndDate = new Label { Text = "End Date:", Location = new Point(260, 55), AutoSize = true };
            dtpEndDate.Location = new Point(320, 52);
            dtpEndDate.Size = new Size(120, 25);
            
            var lblPayDate = new Label { Text = "Pay Date:", Location = new Point(20, 90), AutoSize = true };
            dtpPayDate.Location = new Point(120, 87);
            dtpPayDate.Size = new Size(120, 25);
            
            var lblStatus = new Label { Text = "Status:", Location = new Point(260, 90), AutoSize = true };
            cmbStatus.Location = new Point(320, 87);
            cmbStatus.Size = new Size(120, 25);
            
            chkIsActive.Location = new Point(120, 125);
            
            var lblNotes = new Label { Text = "Notes:", Location = new Point(20, 160), AutoSize = true };
            txtNotes.Location = new Point(120, 157);
            txtNotes.Size = new Size(280, 80);
            
            btnSave.Location = new Point(245, 260);
            btnSave.Size = new Size(75, 30);
            
            btnCancel.Location = new Point(325, 260);
            btnCancel.Size = new Size(75, 30);
            
            this.Controls.AddRange(new Control[] {
                lblPeriodName, txtPeriodName, lblStartDate, dtpStartDate, lblEndDate, dtpEndDate,
                lblPayDate, dtpPayDate, lblStatus, cmbStatus, chkIsActive, lblNotes, txtNotes,
                btnSave, btnCancel
            });
        }
        
        private void AttachEvents()
        {
            btnSave.Click += BtnSave_Click;
            dtpStartDate.ValueChanged += DtpStartDate_ValueChanged;
        }
        
        private void SetDefaultValues()
        {
            var now = DateTime.Now;
            dtpStartDate.Value = new DateTime(now.Year, now.Month, 1);
            dtpEndDate.Value = dtpStartDate.Value.AddMonths(1).AddDays(-1);
            dtpPayDate.Value = dtpEndDate.Value.AddDays(5);
            txtPeriodName.Text = $"{now:MMMM yyyy} Payroll";
        }
        
        private void LoadPeriodData()
        {
            try
            {
                var query = "SELECT * FROM payroll_periods WHERE period_id = @period_id";
                var parameters = new[] { DatabaseManager.CreateParameter("@period_id", periodId.Value) };
                var data = DatabaseManager.GetDataTable(query, parameters);
                
                if (data.Rows.Count > 0)
                {
                    var row = data.Rows[0];
                    txtPeriodName.Text = row["period_name"].ToString();
                    dtpStartDate.Value = Convert.ToDateTime(row["start_date"]);
                    dtpEndDate.Value = Convert.ToDateTime(row["end_date"]);
                    dtpPayDate.Value = Convert.ToDateTime(row["pay_date"]);
                    cmbStatus.SelectedItem = row["status"].ToString();
                     chkIsActive.Checked = Convert.ToBoolean(row["is_active"]);
                    txtNotes.Text = row["notes"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading period data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void DtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            // Auto-calculate end date (end of month)
            var startDate = dtpStartDate.Value;
            dtpEndDate.Value = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
            dtpPayDate.Value = dtpEndDate.Value.AddDays(5);
            
            // Auto-generate period name
            if (!periodId.HasValue)
            {
                txtPeriodName.Text = $"{startDate:MMMM yyyy} Payroll";
            }
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SavePeriod();
            }
        }
        
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtPeriodName.Text))
            {
                MessageBox.Show("Please enter a period name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPeriodName.Focus();
                return false;
            }
            
            if (dtpStartDate.Value >= dtpEndDate.Value)
            {
                MessageBox.Show("End date must be after start date.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndDate.Focus();
                return false;
            }
            
            if (dtpPayDate.Value < dtpEndDate.Value)
            {
                MessageBox.Show("Pay date should be after or equal to end date.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpPayDate.Focus();
                return false;
            }
            
            return true;
        }
        
        private void SavePeriod()
        {
            try
            {
                string query;
                if (periodId.HasValue)
                {
                    query = @"
                        UPDATE payroll_periods 
                        SET period_name = @period_name, start_date = @start_date, end_date = @end_date,
                            pay_date = @pay_date, status = @status, is_active = @is_active, description = @notes
                        WHERE period_id = @period_id";
                }
                else
                {
                    query = @"
                        INSERT INTO payroll_periods (period_name, start_date, end_date, pay_date, status, is_active, description, created_at, created_by)
                        VALUES (@period_name, @start_date, @end_date, @pay_date, @status, @is_active, @notes, NOW(), 'System')";
                }
                
                var parameters = new[]
                {
                    DatabaseManager.CreateParameter("@period_name", txtPeriodName.Text.Trim()),
                    DatabaseManager.CreateParameter("@start_date", dtpStartDate.Value.Date),
                    DatabaseManager.CreateParameter("@end_date", dtpEndDate.Value.Date),
                    DatabaseManager.CreateParameter("@pay_date", dtpPayDate.Value.Date),
                    DatabaseManager.CreateParameter("@status", cmbStatus.SelectedItem.ToString()),
                    DatabaseManager.CreateParameter("@is_active", chkIsActive.Checked),
                    DatabaseManager.CreateParameter("@notes", txtNotes.Text.Trim())
                };
                
                if (periodId.HasValue)
                {
                    Array.Resize(ref parameters, parameters.Length + 1);
                    parameters[parameters.Length - 1] = DatabaseManager.CreateParameter("@period_id", periodId.Value);
                }
                
                DatabaseManager.ExecuteNonQuery(query, parameters);
                
                MessageBox.Show("Payroll period saved successfully.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving period: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // DatabaseManager is static - no disposal needed
            base.OnFormClosing(e);
        }
    }
}
