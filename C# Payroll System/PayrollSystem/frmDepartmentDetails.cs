using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmDepartmentDetails : Form
    {
        // DatabaseManager is static - no instance needed
        private bool isEditMode = false;
        private int currentDepartmentId = 0;

        // Controls
        private TextBox txtDepartmentName;
        private TextBox txtDescription;
        private TextBox txtLocation;
        private ComboBox cmbManager;
        private TextBox txtBudget;
        private CheckBox chkIsActive;
        private DataGridView dgvEmployees;
        private Button btnSave;
        private Button btnCancel;
        private Button btnEdit;
        private Button btnNew;
        private Button btnAddEmployee;
        private Button btnRemoveEmployee;
        private GroupBox grpDepartmentInfo;
        private GroupBox grpEmployees;
        private Label lblDepartmentName;
        private Label lblDescription;
        private Label lblLocation;
        private Label lblManager;
        private Label lblBudget;
        private Label lblEmployeeCount;
        private Label lblTotalSalary;
        private TextBox txtEmployeeCount;
        private TextBox txtTotalSalary;

        public int DepartmentId { get; set; }

        public frmDepartmentDetails(int departmentId = 0)
        {
            DepartmentId = departmentId;
            InitializeComponent();
            LoadManagers();
            
            if (DepartmentId > 0)
            {
                LoadDepartmentDetails();
                LoadDepartmentEmployees();
            }
            else
            {
                isEditMode = true;
                btnSave.Enabled = true;
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Department Details";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // Initialize controls
            grpDepartmentInfo = new GroupBox();
            grpEmployees = new GroupBox();
            lblDepartmentName = new Label();
            lblDescription = new Label();
            lblLocation = new Label();
            lblManager = new Label();
            lblBudget = new Label();
            lblEmployeeCount = new Label();
            lblTotalSalary = new Label();
            txtDepartmentName = new TextBox();
            txtDescription = new TextBox();
            txtLocation = new TextBox();
            cmbManager = new ComboBox();
            txtBudget = new TextBox();
            txtEmployeeCount = new TextBox();
            txtTotalSalary = new TextBox();
            chkIsActive = new CheckBox();
            dgvEmployees = new DataGridView();
            btnSave = new Button();
            btnCancel = new Button();
            btnEdit = new Button();
            btnNew = new Button();
            btnAddEmployee = new Button();
            btnRemoveEmployee = new Button();

            // Department Info Group
            grpDepartmentInfo.Text = "Department Information";
            grpDepartmentInfo.Location = new Point(10, 10);
            grpDepartmentInfo.Size = new Size(760, 200);

            // Labels and Controls for Department Info
            lblDepartmentName.Text = "Department Name:";
            lblDepartmentName.Location = new Point(15, 25);
            lblDepartmentName.Size = new Size(120, 20);

            txtDepartmentName.Location = new Point(140, 22);
            txtDepartmentName.Size = new Size(200, 20);

            lblDescription.Text = "Description:";
            lblDescription.Location = new Point(15, 55);
            lblDescription.Size = new Size(120, 20);

            txtDescription.Location = new Point(140, 52);
            txtDescription.Size = new Size(300, 60);
            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;

            lblLocation.Text = "Location:";
            lblLocation.Location = new Point(15, 125);
            lblLocation.Size = new Size(120, 20);

            txtLocation.Location = new Point(140, 122);
            txtLocation.Size = new Size(200, 20);

            lblManager.Text = "Manager:";
            lblManager.Location = new Point(360, 125);
            lblManager.Size = new Size(80, 20);

            cmbManager.Location = new Point(450, 122);
            cmbManager.Size = new Size(200, 20);
            cmbManager.DropDownStyle = ComboBoxStyle.DropDownList;

            lblBudget.Text = "Budget:";
            lblBudget.Location = new Point(15, 155);
            lblBudget.Size = new Size(120, 20);

            txtBudget.Location = new Point(140, 152);
            txtBudget.Size = new Size(150, 20);
            txtBudget.TextAlign = HorizontalAlignment.Right;

            lblEmployeeCount.Text = "Employee Count:";
            lblEmployeeCount.Location = new Point(360, 155);
            lblEmployeeCount.Size = new Size(100, 20);

            txtEmployeeCount.Location = new Point(470, 152);
            txtEmployeeCount.Size = new Size(80, 20);
            txtEmployeeCount.ReadOnly = true;
            txtEmployeeCount.BackColor = SystemColors.Control;

            lblTotalSalary.Text = "Total Salary:";
            lblTotalSalary.Location = new Point(560, 155);
            lblTotalSalary.Size = new Size(80, 20);

            txtTotalSalary.Location = new Point(650, 152);
            txtTotalSalary.Size = new Size(100, 20);
            txtTotalSalary.ReadOnly = true;
            txtTotalSalary.BackColor = SystemColors.Control;
            txtTotalSalary.TextAlign = HorizontalAlignment.Right;

            chkIsActive.Text = "Active";
            chkIsActive.Location = new Point(300, 155);
            chkIsActive.Size = new Size(80, 20);
            chkIsActive.Checked = true;

            // Employees Group
            grpEmployees.Text = "Department Employees";
            grpEmployees.Location = new Point(10, 220);
            grpEmployees.Size = new Size(760, 280);

            // DataGridView for Employees
            dgvEmployees.Location = new Point(10, 25);
            dgvEmployees.Size = new Size(740, 200);
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.ReadOnly = true;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.MultiSelect = false;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Buttons for Employees
            btnAddEmployee.Text = "Add Employee";
            btnAddEmployee.Location = new Point(500, 235);
            btnAddEmployee.Size = new Size(100, 30);
            btnAddEmployee.Click += BtnAddEmployee_Click;

            btnRemoveEmployee.Text = "Remove Employee";
            btnRemoveEmployee.Location = new Point(610, 235);
            btnRemoveEmployee.Size = new Size(120, 30);
            btnRemoveEmployee.Click += BtnRemoveEmployee_Click;

            // Main Buttons
            btnNew.Text = "New";
            btnNew.Location = new Point(400, 520);
            btnNew.Size = new Size(75, 30);
            btnNew.Click += BtnNew_Click;

            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(485, 520);
            btnEdit.Size = new Size(75, 30);
            btnEdit.Click += BtnEdit_Click;

            btnSave.Text = "Save";
            btnSave.Location = new Point(570, 520);
            btnSave.Size = new Size(75, 30);
            btnSave.Click += BtnSave_Click;
            btnSave.Enabled = false;

            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(655, 520);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Click += BtnCancel_Click;

            // Add controls to groups
            grpDepartmentInfo.Controls.AddRange(new Control[] {
                lblDepartmentName, txtDepartmentName, lblDescription, txtDescription,
                lblLocation, txtLocation, lblManager, cmbManager, lblBudget, txtBudget,
                lblEmployeeCount, txtEmployeeCount, lblTotalSalary, txtTotalSalary, chkIsActive
            });

            grpEmployees.Controls.AddRange(new Control[] {
                dgvEmployees, btnAddEmployee, btnRemoveEmployee
            });

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                grpDepartmentInfo, grpEmployees, btnNew, btnEdit, btnSave, btnCancel
            });
        }

        private void LoadManagers()
        {
            try
            {
                string query = @"SELECT e.EmployeeId, CONCAT(e.FirstName, ' ', e.LastName) AS FullName
                               FROM Employees e
                               INNER JOIN JobTitles jt ON e.JobTitleId = jt.JobTitleId
                               WHERE e.IsActive = 1 AND (jt.TitleName LIKE '%Manager%' OR jt.TitleName LIKE '%Supervisor%' OR jt.TitleName LIKE '%Head%')
                               ORDER BY e.FirstName, e.LastName";
                DataTable dt = DatabaseManager.GetDataTable(query);

                // Add empty option
                DataRow emptyRow = dt.NewRow();
                emptyRow["EmployeeId"] = DBNull.Value;
                emptyRow["FullName"] = "-- Select Manager --";
                dt.Rows.InsertAt(emptyRow, 0);

                cmbManager.DisplayMember = "FullName";
                cmbManager.ValueMember = "EmployeeId";
                cmbManager.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading managers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartmentDetails()
        {
            try
            {
                string query = "SELECT * FROM departments WHERE department_id = @DepartmentId";
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@DepartmentId"] = DepartmentId
                };
                DataTable dt = DatabaseManager.GetDataTable(query, parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray());

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    currentDepartmentId = DepartmentId;
                    txtDepartmentName.Text = row["department_name"].ToString();
                    txtDescription.Text = row["description"].ToString();
                    txtLocation.Text = row["location"].ToString();
                    
                    if (row["manager_id"] != DBNull.Value)
                        cmbManager.SelectedValue = row["manager_id"];
                    
                    if (row["budget"] != DBNull.Value)
                        txtBudget.Text = Convert.ToDecimal(row["budget"]).ToString("N2");
                    
                    chkIsActive.Checked = Convert.ToBoolean(row["is_active"]);

                    SetControlsReadOnly(true);
                    UpdateStatistics();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading department details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartmentEmployees()
        {
            try
            {
                string query = @"SELECT e.EmployeeId, e.EmployeeNumber, 
                               CONCAT(e.FirstName, ' ', e.LastName) AS FullName,
                               jt.TitleName, e.BasicSalary, e.HireDate, e.IsActive
                               FROM Employees e
                               INNER JOIN JobTitles jt ON e.JobTitleId = jt.JobTitleId
                               WHERE e.DepartmentId = @DepartmentId
                               ORDER BY e.LastName, e.FirstName";
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@DepartmentId"] = DepartmentId
                };
                DataTable dt = DatabaseManager.GetDataTable(query, parameters);
                dgvEmployees.DataSource = dt;

                // Hide EmployeeId column
                if (dgvEmployees.Columns["EmployeeId"] != null)
                    dgvEmployees.Columns["EmployeeId"].Visible = false;

                // Format columns
                if (dgvEmployees.Columns["BasicSalary"] != null)
                {
                    dgvEmployees.Columns["BasicSalary"].DefaultCellStyle.Format = "N2";
                    dgvEmployees.Columns["BasicSalary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                
                if (dgvEmployees.Columns["HireDate"] != null)
                    dgvEmployees.Columns["HireDate"].DefaultCellStyle.Format = "MM/dd/yyyy";

                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading department employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                string query = @"SELECT COUNT(*) as EmployeeCount, COALESCE(SUM(BasicSalary), 0) as TotalSalary
                               FROM Employees
                               WHERE DepartmentId = @DepartmentId AND IsActive = 1";
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@DepartmentId"] = DepartmentId
                };
                DataTable dt = DatabaseManager.GetDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtEmployeeCount.Text = row["EmployeeCount"].ToString();
                    txtTotalSalary.Text = Convert.ToDecimal(row["TotalSalary"]).ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetControlsReadOnly(bool readOnly)
        {
            txtDepartmentName.ReadOnly = readOnly;
            txtDescription.ReadOnly = readOnly;
            txtLocation.ReadOnly = readOnly;
            txtBudget.ReadOnly = readOnly;
            cmbManager.Enabled = !readOnly;
            chkIsActive.Enabled = !readOnly;
            btnAddEmployee.Enabled = !readOnly;
            btnRemoveEmployee.Enabled = !readOnly;
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
            isEditMode = true;
            currentDepartmentId = 0;
            DepartmentId = 0;
            SetControlsReadOnly(false);
            btnSave.Enabled = true;
            txtDepartmentName.Focus();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (DepartmentId > 0)
            {
                isEditMode = true;
                SetControlsReadOnly(false);
                btnSave.Enabled = true;
                txtDepartmentName.Focus();
            }
            else
            {
                MessageBox.Show("No department selected to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SaveDepartment();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                if (DepartmentId > 0)
                {
                    LoadDepartmentDetails();
                }
                else
                {
                    ClearFields();
                }
                isEditMode = false;
                btnSave.Enabled = false;
            }
            else
            {
                this.Close();
            }
        }

        private void BtnAddEmployee_Click(object sender, EventArgs e)
        {
            if (DepartmentId > 0)
            {
                // Open employee selection form
                using (var employeeForm = new frmEmployeeSelection(DepartmentId))
                {
                    if (employeeForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadDepartmentEmployees();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please save the department first before adding employees.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRemoveEmployee_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["EmployeeId"].Value);
                string employeeName = dgvEmployees.SelectedRows[0].Cells["FullName"].Value.ToString();

                if (MessageBox.Show($"Are you sure you want to remove {employeeName} from this department?", 
                    "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    RemoveEmployeeFromDepartment(employeeId);
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to remove.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RemoveEmployeeFromDepartment(int employeeId)
        {
            try
            {
                string query = "UPDATE Employees SET DepartmentId = NULL, ModifiedDate = @ModifiedDate WHERE EmployeeId = @EmployeeId";
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@EmployeeId"] = employeeId,
                    ["@ModifiedDate"] = DateTime.Now
                };

                int result = DatabaseManager.ExecuteNonQuery(query, parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray());

                if (result > 0)
                {
                    MessageBox.Show("Employee removed from department successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDepartmentEmployees();
                }
                else
                {
                    MessageBox.Show("Failed to remove employee from department.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
            {
                MessageBox.Show("Department name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDepartmentName.Focus();
                return false;
            }

            // Check for unique department name
            try
            {
                string checkNameQuery = "SELECT COUNT(*) FROM departments WHERE LOWER(department_name) = LOWER(@DepartmentName) AND department_id != @DepartmentId";
                var nameParameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@DepartmentName"] = txtDepartmentName.Text.Trim(),
                    ["@DepartmentId"] = currentDepartmentId
                };

                int nameCount = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkNameQuery, 
                    nameParameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray()));

                if (nameCount > 0)
                {
                    MessageBox.Show("A department with this name already exists. Please choose a different name.", 
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartmentName.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking department name: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtBudget.Text))
            {
                if (!decimal.TryParse(txtBudget.Text, out decimal budget) || budget < 0)
                {
                    MessageBox.Show("Please enter a valid budget amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBudget.Focus();
                    return false;
                }
            }

            return true;
        }

        private void SaveDepartment()
        {
            try
            {
                string query;
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@DepartmentName"] = txtDepartmentName.Text.Trim(),
                    ["@Description"] = txtDescription.Text.Trim(),
                    ["@Location"] = txtLocation.Text.Trim(),
                    ["@ManagerId"] = cmbManager.SelectedValue == DBNull.Value ? null : cmbManager.SelectedValue,
                    ["@Budget"] = string.IsNullOrWhiteSpace(txtBudget.Text) ? null : (object)decimal.Parse(txtBudget.Text),
                    ["@IsActive"] = chkIsActive.Checked,
                    ["@ModifiedDate"] = DateTime.Now
                };

                if (currentDepartmentId == 0)
                {
                    // Insert new department
                    query = @"INSERT INTO departments (department_name, description, location, manager_id, budget, is_active, created_date, modified_date)
                             VALUES (@DepartmentName, @Description, @Location, @ManagerId, @Budget, @IsActive, @CreatedDate, @ModifiedDate);
                             SELECT last_insert_rowid();";
                    parameters["@CreatedDate"] = DateTime.Now;
                }
                else
                {
                    // Update existing department
                    query = @"UPDATE departments SET department_name = @DepartmentName, description = @Description, location = @Location,
                             manager_id = @ManagerId, budget = @Budget, is_active = @IsActive, modified_date = @ModifiedDate
                             WHERE department_id = @DepartmentId";
                    parameters["@DepartmentId"] = currentDepartmentId;
                }

                var result = DatabaseManager.ExecuteScalar(query, parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray());

                if (currentDepartmentId == 0 && result != null)
                {
                    currentDepartmentId = Convert.ToInt32(result);
                    DepartmentId = currentDepartmentId;
                }

                MessageBox.Show("Department saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isEditMode = false;
                SetControlsReadOnly(true);
                btnSave.Enabled = false;
                
                if (DepartmentId > 0)
                {
                    LoadDepartmentEmployees();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving department: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtDepartmentName.Clear();
            txtDescription.Clear();
            txtLocation.Clear();
            cmbManager.SelectedIndex = 0;
            txtBudget.Clear();
            txtEmployeeCount.Clear();
            txtTotalSalary.Clear();
            chkIsActive.Checked = true;
            dgvEmployees.DataSource = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // DatabaseManager is static - no disposal needed
            }
            base.Dispose(disposing);
        }
    }

    // Helper form for employee selection
    public partial class frmEmployeeSelection : Form
    {
        // DatabaseManager is static - no instance needed
        private int departmentId;
        private DataGridView dgvAvailableEmployees;
        private Button btnSelect;
        private Button btnCancel;

        public frmEmployeeSelection(int deptId)
        {
            departmentId = deptId;
            InitializeComponent();
            LoadAvailableEmployees();
        }

        private void InitializeComponent()
        {
            this.Text = "Select Employee";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            dgvAvailableEmployees = new DataGridView();
            btnSelect = new Button();
            btnCancel = new Button();

            dgvAvailableEmployees.Location = new Point(10, 10);
            dgvAvailableEmployees.Size = new Size(560, 300);
            dgvAvailableEmployees.AllowUserToAddRows = false;
            dgvAvailableEmployees.AllowUserToDeleteRows = false;
            dgvAvailableEmployees.ReadOnly = true;
            dgvAvailableEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailableEmployees.MultiSelect = false;
            dgvAvailableEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            btnSelect.Text = "Select";
            btnSelect.Location = new Point(420, 320);
            btnSelect.Size = new Size(75, 30);
            btnSelect.Click += BtnSelect_Click;

            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(505, 320);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { dgvAvailableEmployees, btnSelect, btnCancel });
        }

        private void LoadAvailableEmployees()
        {
            try
            {
                string query = @"SELECT e.EmployeeId, e.EmployeeNumber, 
                               CONCAT(e.FirstName, ' ', e.LastName) AS FullName,
                               jt.TitleName, e.BasicSalary
                               FROM Employees e
                               INNER JOIN JobTitles jt ON e.JobTitleId = jt.JobTitleId
                               WHERE e.IsActive = 1 AND (e.DepartmentId IS NULL OR e.DepartmentId != @DepartmentId)
                               ORDER BY e.LastName, e.FirstName";
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@DepartmentId"] = departmentId
                };
                DataTable dt = DatabaseManager.GetDataTable(query, parameters);
                dgvAvailableEmployees.DataSource = dt;

                if (dgvAvailableEmployees.Columns["EmployeeId"] != null)
                    dgvAvailableEmployees.Columns["EmployeeId"].Visible = false;

                if (dgvAvailableEmployees.Columns["BasicSalary"] != null)
                {
                    dgvAvailableEmployees.Columns["BasicSalary"].DefaultCellStyle.Format = "N2";
                    dgvAvailableEmployees.Columns["BasicSalary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (dgvAvailableEmployees.SelectedRows.Count > 0)
            {
                int employeeId = Convert.ToInt32(dgvAvailableEmployees.SelectedRows[0].Cells["EmployeeId"].Value);
                
                try
                {
                    string query = "UPDATE Employees SET DepartmentId = @DepartmentId, ModifiedDate = @ModifiedDate WHERE EmployeeId = @EmployeeId";
                    var parameters = new System.Collections.Generic.Dictionary<string, object>
                    {
                        ["@DepartmentId"] = departmentId,
                        ["@EmployeeId"] = employeeId,
                        ["@ModifiedDate"] = DateTime.Now
                    };

                    MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                    int result = DatabaseManager.ExecuteNonQuery(query, mysqlParams);

                    if (result > 0)
                    {
                        MessageBox.Show("Employee added to department successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Failed to add employee to department.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an employee.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // DatabaseManager is static - no disposal needed
            }
            base.Dispose(disposing);
        }
    }
}
