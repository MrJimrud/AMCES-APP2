using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmDepartment : Form
    {
        // DatabaseManager is static, no instance needed
        private int currentDepartmentId = 0;
        private bool isEditing = false;

        // Form Controls
        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelForm;
        private Label lblDepartmentCode;
        private TextBox txtDepartmentCode;
        private Label lblDepartmentName;
        private TextBox txtDepartmentName;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblManager;
        private TextBox txtManager;
        private Label lblBudget;
        private NumericUpDown nudBudget;
        private CheckBox chkActive;
        private Panel panelButtons;
        private Button btnSave;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnNew;
        private Button btnClose;
        private Panel panelList;
        private DataGridView dgvDepartments;
        private Panel panelSearch;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnRefresh;

        public frmDepartment()
        {
            InitializeComponent();
            LoadDepartments();
            SetFormMode(false);
        }

        public frmDepartment(int departmentId)
        {
            InitializeComponent();
            if (departmentId > 0)
            {
                currentDepartmentId = departmentId;
                isEditing = true;
            }
            LoadDepartments();
            if (isEditing)
            {
                LoadDepartment(currentDepartmentId);
                SetFormMode(true);
            }
            else
            {
                SetFormMode(false);
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(900, 600);
            this.Text = "Department Management";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Header Panel
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(41, 128, 185)
            };
            this.Controls.Add(panelHeader);

            lblTitle = new Label
            {
                Text = "Department Management",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            panelHeader.Controls.Add(lblTitle);

            // Form Panel
            panelForm = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(400, 300),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelForm);

            // Department Code
            lblDepartmentCode = new Label
            {
                Text = "Department Code:",
                Location = new Point(20, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblDepartmentCode);

            txtDepartmentCode = new TextBox
            {
                Location = new Point(150, 20),
                Size = new Size(220, 23),
                Font = new Font("Segoe UI", 9),
                MaxLength = 10
            };
            panelForm.Controls.Add(txtDepartmentCode);

            // Department Name
            lblDepartmentName = new Label
            {
                Text = "Department Name:",
                Location = new Point(20, 60),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblDepartmentName);

            txtDepartmentName = new TextBox
            {
                Location = new Point(150, 100),
                Size = new Size(220, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(txtDepartmentName);

            // Description
            lblDescription = new Label
            {
                Text = "Description:",
                Location = new Point(20, 100),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblDescription);

            txtDescription = new TextBox
            {
                Location = new Point(150, 60),
                Size = new Size(220, 60),
                Font = new Font("Segoe UI", 9),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            panelForm.Controls.Add(txtDescription);

            // Manager
            lblManager = new Label
            {
                Text = "Manager:",
                Location = new Point(20, 220),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblManager);

            txtManager = new TextBox
            {
                Location = new Point(150, 260),
                Size = new Size(220, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(txtManager);

            // Budget
            lblBudget = new Label
            {
                Text = "Budget:",
                Location = new Point(20, 180),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblBudget);

            nudBudget = new NumericUpDown
            {
                Location = new Point(150, 180),
                Size = new Size(220, 23),
                Font = new Font("Segoe UI", 9),
                DecimalPlaces = 2,
                Maximum = 999999999,
                Minimum = 0
            };
            panelForm.Controls.Add(nudBudget);

            // Active Status
            chkActive = new CheckBox
            {
                Text = "Active",
                Location = new Point(150, 220),
                Size = new Size(100, 23),
                Font = new Font("Segoe UI", 9),
                Checked = true
            };
            panelForm.Controls.Add(chkActive);

            // Buttons Panel
            panelButtons = new Panel
            {
                Location = new Point(20, 400),
                Size = new Size(400, 50)
            };
            this.Controls.Add(panelButtons);

            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(0, 10),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;
            panelButtons.Controls.Add(btnSave);

            btnEdit = new Button
            {
                Text = "Edit",
                Location = new Point(85, 10),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.Click += BtnEdit_Click;
            panelButtons.Controls.Add(btnEdit);

            btnDelete = new Button
            {
                Text = "Delete",
                Location = new Point(170, 10),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.Click += BtnDelete_Click;
            panelButtons.Controls.Add(btnDelete);

            btnNew = new Button
            {
                Text = "New",
                Location = new Point(255, 10),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnNew.Click += BtnNew_Click;
            panelButtons.Controls.Add(btnNew);

            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(340, 10),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += BtnClose_Click;
            panelButtons.Controls.Add(btnClose);

            // List Panel
            panelList = new Panel
            {
                Location = new Point(440, 80),
                Size = new Size(430, 370),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelList);

            // Search Panel
            panelSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40
            };
            panelList.Controls.Add(panelSearch);

            lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(10, 10),
                Size = new Size(50, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelSearch.Controls.Add(lblSearch);

            txtSearch = new TextBox
            {
                Location = new Point(70, 10),
                Size = new Size(250, 23),
                Font = new Font("Segoe UI", 9)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            panelSearch.Controls.Add(txtSearch);

            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(330, 8),
                Size = new Size(80, 27),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.Click += BtnRefresh_Click;
            panelSearch.Controls.Add(btnRefresh);

            // DataGridView
            dgvDepartments = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                GridColor = Color.LightGray,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowTemplate = { Height = 30 }
            };
            
            // Set column header style
            dgvDepartments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvDepartments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDepartments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDepartments.ColumnHeadersHeight = 35;
            dgvDepartments.EnableHeadersVisualStyles = false;
            dgvDepartments.CellDoubleClick += DgvDepartments_CellDoubleClick;
            dgvDepartments.SelectionChanged += DgvDepartments_SelectionChanged;

            Panel panelGrid = new Panel
            {
                Dock = DockStyle.Fill
            };
            panelGrid.Controls.Add(dgvDepartments);
            panelList.Controls.Add(panelGrid);
        }

        private void LoadDepartments()
        {
            try
            {
                string query = @"
                    SELECT 
                        department_id as ID,
                        department_code as Code,
                        department_name as `Department Name`,
                        description as Description,
                        manager as Manager,
                        budget as Budget,
                        CASE WHEN is_active = 1 THEN 'Active' ELSE 'Inactive' END as Status
                    FROM departments 
                    ORDER BY department_id DESC";

                DataTable dt = DatabaseManager.GetDataTable(query);
                dgvDepartments.DataSource = dt;

                // Format the columns
                if (dgvDepartments.Columns["ID"] != null)
                    dgvDepartments.Columns["ID"].Visible = false;
                
                // Format the Budget column to show currency
                if (dgvDepartments.Columns["Budget"] != null)
                    dgvDepartments.Columns["Budget"].DefaultCellStyle.Format = "C2";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartment(int departmentId)
        {
            try
            {
                string query = "SELECT * FROM departments WHERE department_id = @id";
                var parameters = new[] { DatabaseManager.CreateParameter("@id", departmentId) };
                DataTable dt = DatabaseManager.GetDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtDepartmentCode.Text = row["department_code"].ToString();
                    txtDepartmentName.Text = row["department_name"].ToString();
                    txtDescription.Text = row["description"].ToString();
                    txtManager.Text = row["manager"].ToString();
                    nudBudget.Value = Convert.ToDecimal(row["budget"]);
                    chkActive.Checked = Convert.ToBoolean(row["is_active"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading department: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                string query;
                var parameters = new Dictionary<string, object>
                {
                    { "@code", txtDepartmentCode.Text.Trim() },
                    { "@name", txtDepartmentName.Text.Trim() },
                    { "@description", txtDescription.Text.Trim() },
                    { "@manager", txtManager.Text.Trim() },
                    { "@budget", nudBudget.Value },
                    { "@active", chkActive.Checked }
                };

                if (isEditing && currentDepartmentId > 0)
                {
                    query = @"
                        UPDATE departments SET 
                            department_code = @code,
                            department_name = @name,
                            description = @description,
                            manager = @manager,
                            budget = @budget,
                            is_active = @active,
                            updated_at = NOW()
                        WHERE department_id = @id";
                    parameters.Add("@id", currentDepartmentId);
                }
                else
                {
                    query = @"
                        INSERT INTO departments (department_code, department_name, description, manager, budget, is_active, created_at, updated_at)
                        VALUES (@code, @name, @description, @manager, @budget, @active, NOW(), NOW())";
                }

                int result = DatabaseManager.ExecuteNonQuery(query, parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray());
                if (result > 0)
                {
                    MessageBox.Show("Department saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDepartments();
                    ClearForm();
                    SetFormMode(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving department: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count > 0)
            {
                currentDepartmentId = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["ID"].Value);
                LoadDepartment(currentDepartmentId);
                isEditing = true;
                SetFormMode(true);
            }
            else
            {
                MessageBox.Show("Please select a department to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count > 0)
            {
                int departmentId = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["ID"].Value);
                string departmentName = dgvDepartments.SelectedRows[0].Cells["Department Name"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete the department '{departmentName}'?\n\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string query = "DELETE FROM departments WHERE department_id = @id";
                        var parameters = new[] { DatabaseManager.CreateParameter("@id", departmentId) };
                        int deleteResult = DatabaseManager.ExecuteNonQuery(query, parameters);

                        if (deleteResult > 0)
                        {
                            MessageBox.Show("Department deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDepartments();
                            ClearForm();
                            SetFormMode(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting department: {ex.Message}\n\nNote: Cannot delete departments that have employees assigned.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a department to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            isEditing = false;
            currentDepartmentId = 0;
            SetFormMode(true);
            txtDepartmentName.Focus();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadDepartments();
            txtSearch.Clear();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvDepartments.DataSource is DataTable dt)
            {
                string searchText = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchText))
                {
                    dt.DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    dt.DefaultView.RowFilter = $"[Department Name] LIKE '%{searchText}%' OR [Description] LIKE '%{searchText}%' OR [Manager] LIKE '%{searchText}%'";
                }
            }
        }

        private void DgvDepartments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEdit_Click(sender, e);
            }
        }

        private void DgvDepartments_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dgvDepartments.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private bool ValidateForm()
        {
            try
            {
                // Validate department code
                string departmentCode = txtDepartmentCode.Text.Trim();
                if (string.IsNullOrWhiteSpace(departmentCode))
                {
                    MessageBox.Show("Department code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartmentCode.Focus();
                    return false;
                }

                if (departmentCode.Length < 2 || departmentCode.Length > 10)
                {
                    MessageBox.Show("Department code must be between 2 and 10 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartmentCode.Focus();
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(departmentCode, @"^[A-Z0-9]+$"))
                {
                    MessageBox.Show("Department code must contain only uppercase letters and numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartmentCode.Focus();
                    return false;
                }

                // Validate department name
                string departmentName = txtDepartmentName.Text.Trim();
                if (string.IsNullOrWhiteSpace(departmentName))
                {
                    MessageBox.Show("Department name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartmentName.Focus();
                    return false;
                }

                if (departmentName.Length < 2 || departmentName.Length > 50)
                {
                    MessageBox.Show("Department name must be between 2 and 50 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartmentName.Focus();
                    return false;
                }

                // Validate description
                string description = txtDescription.Text.Trim();
                if (description.Length > 200)
                {
                    MessageBox.Show("Description cannot exceed 200 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDescription.Focus();
                    return false;
                }

                // Validate manager
                string manager = txtManager.Text.Trim();
                if (!string.IsNullOrWhiteSpace(manager) && (manager.Length < 2 || manager.Length > 100))
                {
                    MessageBox.Show("Manager name must be between 2 and 100 characters if provided.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtManager.Focus();
                    return false;
                }

                // Validate budget
                if (nudBudget.Value < 0 || nudBudget.Value > 999999999.99m)
                {
                    MessageBox.Show("Budget must be between 0 and 999,999,999.99.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nudBudget.Focus();
                    return false;
                }

                // Check for unique department code
                string checkCodeQuery = isEditing ?
                    "SELECT COUNT(*) FROM departments WHERE department_code = @code AND department_id != @id" :
                    "SELECT COUNT(*) FROM departments WHERE department_code = @code";

                var codeParameters = new Dictionary<string, object>
                {
                    { "@code", departmentCode }
                };

                if (isEditing)
                {
                    codeParameters.Add("@id", currentDepartmentId);
                }

                MySqlParameter[] codeParams = codeParameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int codeCount = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkCodeQuery, codeParams));
                if (codeCount > 0)
                {
                    MessageBox.Show("This department code is already in use.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartmentCode.Focus();
                    return false;
                }

                // Check for unique department name
                string checkNameQuery = isEditing ?
                    "SELECT COUNT(*) FROM departments WHERE department_name = @name AND department_id != @id" :
                    "SELECT COUNT(*) FROM departments WHERE department_name = @name";

                var nameParameters = new Dictionary<string, object>
                {
                    { "@name", departmentName }
                };

                if (isEditing)
                {
                    nameParameters.Add("@id", currentDepartmentId);
                }

                MySqlParameter[] nameParams = nameParameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int nameCount = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkNameQuery, nameParams));
                if (nameCount > 0)
                {
                    MessageBox.Show("This department name is already in use.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartmentName.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during form validation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void SetFormMode(bool editMode)
        {
            txtDepartmentCode.Enabled = editMode && !isEditing; // Only enable for new records
            txtDepartmentName.Enabled = editMode;
            txtDescription.Enabled = editMode;
            txtManager.Enabled = editMode;
            nudBudget.Enabled = editMode;
            chkActive.Enabled = editMode;
            btnSave.Enabled = editMode;
            btnNew.Enabled = !editMode;
        }

        private void ClearForm()
        {
            txtDepartmentCode.Clear();
            txtDepartmentName.Clear();
            txtDescription.Clear();
            txtManager.Clear();
            nudBudget.Value = 0;
            chkActive.Checked = true;
            currentDepartmentId = 0;
            isEditing = false;
        }
    }
}
