using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmDepartmentList : Form
    {
        // DatabaseManager is static, no instance needed

        // Controls
        private DataGridView dgvDepartments;
        private TextBox txtSearch;
        private ComboBox cmbFilterStatus;
        private Button btnSearch;
        private Button btnClear;
        private Button btnNew;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnDetails;
        private Button btnRefresh;
        private Button btnExport;
        private Label lblSearch;
        private Label lblFilter;
        private Label lblTotalRecords;
        private GroupBox grpFilters;
        private GroupBox grpActions;

        public frmDepartmentList()
        {
            InitializeComponent();
            LoadDepartments();
        }

        private void InitializeComponent()
        {
            this.Text = "Department List";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // Initialize controls
            grpFilters = new GroupBox();
            grpActions = new GroupBox();
            lblSearch = new Label();
            lblFilter = new Label();
            lblTotalRecords = new Label();
            txtSearch = new TextBox();
            cmbFilterStatus = new ComboBox();
            btnSearch = new Button();
            btnClear = new Button();
            dgvDepartments = new DataGridView();
            btnNew = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnDetails = new Button();
            btnRefresh = new Button();
            btnExport = new Button();

            // Filters Group
            grpFilters.Text = "Search & Filter";
            grpFilters.Location = new Point(10, 10);
            grpFilters.Size = new Size(960, 60);

            lblSearch.Text = "Search:";
            lblSearch.Location = new Point(15, 25);
            lblSearch.Size = new Size(50, 20);

            txtSearch.Location = new Point(70, 22);
            txtSearch.Size = new Size(200, 20);
            txtSearch.SetPlaceholderText("Department name, location, manager...");

            lblFilter.Text = "Status:";
            lblFilter.Location = new Point(290, 25);
            lblFilter.Size = new Size(50, 20);

            cmbFilterStatus.Location = new Point(345, 22);
            cmbFilterStatus.Size = new Size(100, 20);
            cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterStatus.Items.AddRange(new string[] { "All", "Active", "Inactive" });
            cmbFilterStatus.SelectedIndex = 0;

            btnSearch.Text = "Search";
            btnSearch.Location = new Point(460, 20);
            btnSearch.Size = new Size(70, 25);
            btnSearch.Click += BtnSearch_Click;

            btnClear.Text = "Clear";
            btnClear.Location = new Point(540, 20);
            btnClear.Size = new Size(70, 25);
            btnClear.Click += BtnClear_Click;

            lblTotalRecords.Text = "Total: 0 records";
            lblTotalRecords.Location = new Point(650, 25);
            lblTotalRecords.Size = new Size(150, 20);
            lblTotalRecords.ForeColor = Color.Blue;

            // DataGridView
            dgvDepartments.Location = new Point(10, 80);
            dgvDepartments.Size = new Size(960, 400);
            dgvDepartments.AllowUserToAddRows = false;
            dgvDepartments.AllowUserToDeleteRows = false;
            dgvDepartments.ReadOnly = true;
            dgvDepartments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDepartments.MultiSelect = false;
            dgvDepartments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDepartments.DoubleClick += DgvDepartments_DoubleClick;
            dgvDepartments.SelectionChanged += DgvDepartments_SelectionChanged;

            // Actions Group
            grpActions.Text = "Actions";
            grpActions.Location = new Point(10, 490);
            grpActions.Size = new Size(960, 60);

            btnNew.Text = "New Department";
            btnNew.Location = new Point(20, 20);
            btnNew.Size = new Size(120, 30);
            btnNew.Click += BtnNew_Click;

            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(150, 20);
            btnEdit.Size = new Size(80, 30);
            btnEdit.Click += BtnEdit_Click;
            btnEdit.Enabled = false;

            btnDetails.Text = "Details";
            btnDetails.Location = new Point(240, 20);
            btnDetails.Size = new Size(80, 30);
            btnDetails.Click += BtnDetails_Click;
            btnDetails.Enabled = false;

            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(330, 20);
            btnDelete.Size = new Size(80, 30);
            btnDelete.Click += BtnDelete_Click;
            btnDelete.Enabled = false;

            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(420, 20);
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.Click += BtnRefresh_Click;

            btnExport.Text = "Export CSV";
            btnExport.Location = new Point(510, 20);
            btnExport.Size = new Size(100, 30);
            btnExport.Click += BtnExport_Click;

            // Add controls to groups
            grpFilters.Controls.AddRange(new Control[] {
                lblSearch, txtSearch, lblFilter, cmbFilterStatus,
                btnSearch, btnClear, lblTotalRecords
            });

            grpActions.Controls.AddRange(new Control[] {
                btnNew, btnEdit, btnDetails, btnDelete, btnRefresh, btnExport
            });

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                grpFilters, dgvDepartments, grpActions
            });

            // Set up Enter key for search
            txtSearch.KeyPress += (s, e) => {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    BtnSearch_Click(s, e);
                    e.Handled = true;
                }
            };
        }

        private void LoadDepartments(string searchText = "", string statusFilter = "All")
        {
            try
            {
                string query = @"SELECT d.department_id as DepartmentId, d.department_name, d.description as Description, d.location as Location,
                               COALESCE(CONCAT(e.first_name, ' ', e.last_name), 'No Manager') AS ManagerName,
                               d.budget as Budget, 
                               (SELECT COUNT(*) FROM employees emp WHERE emp.department_id = d.department_id AND emp.is_active = 1) AS EmployeeCount,
                               CASE WHEN d.is_active = 1 THEN 'Active' ELSE 'Inactive' END AS Status,
                               d.created_date as CreatedDate, d.modified_date as ModifiedDate
                               FROM departments d
                               LEFT JOIN employees e ON d.manager_id = e.employee_id
                                WHERE 1=1";

                var parameters = new System.Collections.Generic.Dictionary<string, object>();

                // Add search filter
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query += " AND (d.department_name LIKE @SearchText OR d.location LIKE @SearchText OR d.description LIKE @SearchText OR CONCAT(e.first_name, ' ', e.last_name) LIKE @SearchText)";
                    parameters["@SearchText"] = $"%{searchText}%";
                }

                // Add status filter
                if (statusFilter != "All")
                {
                    bool isActive = statusFilter == "Active";
                    query += " AND d.is_active = @IsActive";
                    parameters["@IsActive"] = isActive;
                }

                query += " ORDER BY d.department_name";

                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                DataTable dt = DatabaseManager.GetDataTable(query, mysqlParams);
                dgvDepartments.DataSource = dt;

                // Hide DepartmentId column
                if (dgvDepartments.Columns["DepartmentId"] != null)
                    dgvDepartments.Columns["DepartmentId"].Visible = false;

                // Format columns
                if (dgvDepartments.Columns["Budget"] != null)
                {
                    dgvDepartments.Columns["Budget"].DefaultCellStyle.Format = "N2";
                    dgvDepartments.Columns["Budget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvDepartments.Columns["EmployeeCount"] != null)
                {
                    dgvDepartments.Columns["EmployeeCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvDepartments.Columns["EmployeeCount"].HeaderText = "Employees";
                }

                if (dgvDepartments.Columns["CreatedDate"] != null)
                    dgvDepartments.Columns["CreatedDate"].DefaultCellStyle.Format = "MM/dd/yyyy";

                if (dgvDepartments.Columns["ModifiedDate"] != null)
                    dgvDepartments.Columns["ModifiedDate"].DefaultCellStyle.Format = "MM/dd/yyyy";

                // Set column headers
                if (dgvDepartments.Columns["department_name"] != null)
                    dgvDepartments.Columns["department_name"].HeaderText = "Department Name";
                if (dgvDepartments.Columns["ManagerName"] != null)
                    dgvDepartments.Columns["ManagerName"].HeaderText = "Manager";
                if (dgvDepartments.Columns["CreatedDate"] != null)
                    dgvDepartments.Columns["CreatedDate"].HeaderText = "Created";
                if (dgvDepartments.Columns["ModifiedDate"] != null)
                    dgvDepartments.Columns["ModifiedDate"].HeaderText = "Modified";

                // Update record count
                lblTotalRecords.Text = $"Total: {dt.Rows.Count} records";

                // Color rows based on status
                foreach (DataGridViewRow row in dgvDepartments.Rows)
                {
                    if (row.Cells["Status"].Value.ToString() == "Inactive")
                    {
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDepartments_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dgvDepartments.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDetails.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private void DgvDepartments_DoubleClick(object sender, EventArgs e)
        {
            BtnDetails_Click(sender, e);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadDepartments(txtSearch.Text.Trim(), cmbFilterStatus.Text);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilterStatus.SelectedIndex = 0;
            LoadDepartments();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            using (var departmentForm = new frmDepartmentDetails())
            {
                if (departmentForm.ShowDialog() == DialogResult.OK)
                {
                    LoadDepartments(txtSearch.Text.Trim(), cmbFilterStatus.Text);
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count > 0)
            {
                int departmentId = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["DepartmentId"].Value);
                using (var departmentForm = new frmDepartmentDetails(departmentId))
                {
                    if (departmentForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadDepartments(txtSearch.Text.Trim(), cmbFilterStatus.Text);
                    }
                }
            }
        }

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count > 0)
            {
                int departmentId = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["DepartmentId"].Value);
                using (var departmentForm = new frmDepartmentDetails(departmentId))
                {
                    departmentForm.ShowDialog();
                    LoadDepartments(txtSearch.Text.Trim(), cmbFilterStatus.Text);
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count > 0)
            {
                string departmentName = dgvDepartments.SelectedRows[0].Cells["department_name"].Value.ToString();
                int employeeCount = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["EmployeeCount"].Value);

                if (employeeCount > 0)
                {
                    MessageBox.Show($"Cannot delete department '{departmentName}' because it has {employeeCount} employee(s). Please reassign or remove employees first.", 
                        "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to delete the department '{departmentName}'?\n\nThis action cannot be undone.", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteDepartment();
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadDepartments(txtSearch.Text.Trim(), cmbFilterStatus.Text);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            ExportToCSV();
        }

        private void DeleteDepartment()
        {
            try
            {
                int departmentId = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["DepartmentId"].Value);
                
                string query = "UPDATE Departments SET IsActive = 0, ModifiedDate = @ModifiedDate WHERE DepartmentId = @DepartmentId";
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@DepartmentId"] = departmentId,
                    ["@ModifiedDate"] = DateTime.Now
                };

                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int result = DatabaseManager.ExecuteNonQuery(query, mysqlParams);

                if (result > 0)
                {
                    MessageBox.Show("Department deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDepartments(txtSearch.Text.Trim(), cmbFilterStatus.Text);
                }
                else
                {
                    MessageBox.Show("Failed to delete department.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting department: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV()
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV files (*.csv)|*.csv";
                    saveDialog.Title = "Export Departments to CSV";
                    saveDialog.FileName = $"Departments_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var csv = new System.Text.StringBuilder();

                        // Add headers
                        var headers = new string[]
                        {
                            "Department Name", "Description", "Location", "Manager", 
                            "Budget", "Employee Count", "Status", "Created Date", "Modified Date"
                        };
                        csv.AppendLine(string.Join(",", headers));

                        // Add data rows
                        foreach (DataGridViewRow row in dgvDepartments.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                var values = new string[]
                                {
                                    EscapeCSVField(row.Cells["department_name"].Value?.ToString() ?? ""),
                                    EscapeCSVField(row.Cells["Description"].Value?.ToString() ?? ""),
                                    EscapeCSVField(row.Cells["Location"].Value?.ToString() ?? ""),
                                    EscapeCSVField(row.Cells["ManagerName"].Value?.ToString() ?? ""),
                                    row.Cells["Budget"].Value?.ToString() ?? "0",
                                    row.Cells["EmployeeCount"].Value?.ToString() ?? "0",
                                    row.Cells["Status"].Value?.ToString() ?? "",
                                    Convert.ToDateTime(row.Cells["CreatedDate"].Value).ToString("MM/dd/yyyy"),
                                    Convert.ToDateTime(row.Cells["ModifiedDate"].Value).ToString("MM/dd/yyyy")
                                };
                                csv.AppendLine(string.Join(",", values));
                            }
                        }

                        System.IO.File.WriteAllText(saveDialog.FileName, csv.ToString());
                        MessageBox.Show($"Departments exported successfully to:\n{saveDialog.FileName}", "Export Complete", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Ask if user wants to open the file
                        if (MessageBox.Show("Would you like to open the exported file?", "Open File", 
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting departments: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string EscapeCSVField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            // If field contains comma, newline, or quote, wrap in quotes and escape internal quotes
            if (field.Contains(",") || field.Contains("\n") || field.Contains("\""))
            {
                return $"\"{field.Replace("\"", "\"\"")}\";";
            }

            return field;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // DatabaseManager is static - no disposal needed
                // dbManager?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
