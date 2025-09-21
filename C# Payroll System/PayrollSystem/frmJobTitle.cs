using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmJobTitle : Form
    {
        // DatabaseManager is static, no instance needed
        private int currentJobTitleId = 0;
        private bool isEditing = false;

        // Form Controls
    private Panel panelHeader;
    private Label lblTitle;
    private Panel panelForm;
    private Label lblJobCode;
    private Label lblJobTitle;
    private TextBox txtJobCode;
    private TextBox txtJobTitle;
    private Label lblDescription;
    private TextBox txtDescription;
    private Label lblDepartment;
    private ComboBox cboDepartment;
    private Label lblMinSalary;
    private NumericUpDown nudMinSalary;
    private Label lblMaxSalary;
    private NumericUpDown nudMaxSalary;
    private Label lblRequirements;
    private TextBox txtRequirements;
    private CheckBox chkActive;
    private Panel panelButtons;
    private Button btnSave;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnNew;
    private Button btnClose;
    private Panel panelList;
    private DataGridView dgvJobTitles;
    private Panel panelSearch;
    private Label lblSearch;
    private TextBox txtSearch;
    private ComboBox cboFilterDepartment;
    private Button btnRefresh;

        public frmJobTitle()
        {
            InitializeComponent();
            LoadDepartments();
            LoadJobTitles();
            SetFormMode(false);
        }

        public frmJobTitle(int jobTitleId)
        {
            InitializeComponent();
            if (jobTitleId > 0)
            {
                currentJobTitleId = jobTitleId;
                isEditing = true;
            }
            LoadDepartments();
            LoadJobTitles();
            if (isEditing)
            {
                LoadJobTitle(currentJobTitleId);
                SetFormMode(true);
            }
            else
            {
                SetFormMode(false);
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 650);
            this.Text = "Job Title Management";
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
                Text = "Job Title Management",
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
                Size = new Size(450, 400),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelForm);

            // Job Code
            lblJobCode = new Label
            {
                Text = "Job Code:",
                Location = new Point(20, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblJobCode);

            txtJobCode = new TextBox
            {
                Location = new Point(150, 20),
                Size = new Size(270, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(txtJobCode);
            
            // Job Title
            lblJobTitle = new Label
            {
                Text = "Job Title:",
                Location = new Point(20, 60),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblJobTitle);

            txtJobTitle = new TextBox
            {
                Location = new Point(150, 60),
                Size = new Size(270, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(txtJobTitle);

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
                Location = new Point(150, 100),
                Size = new Size(270, 60),
                Font = new Font("Segoe UI", 9),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            panelForm.Controls.Add(txtDescription);

            // Department
            lblDepartment = new Label
            {
                Text = "Department:",
                Location = new Point(20, 180),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblDepartment);

            cboDepartment = new ComboBox
            {
                Location = new Point(150, 180),
                Size = new Size(210, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelForm.Controls.Add(cboDepartment);

            // Min Salary
            lblMinSalary = new Label
            {
                Text = "Min Salary:",
                Location = new Point(20, 220),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblMinSalary);

            nudMinSalary = new NumericUpDown
            {
                Location = new Point(150, 220),
                Size = new Size(130, 23),
                Font = new Font("Segoe UI", 9),
                DecimalPlaces = 2,
                Maximum = 999999999,
                Minimum = 0,
                ThousandsSeparator = true
            };
            panelForm.Controls.Add(nudMinSalary);

            // Max Salary
            lblMaxSalary = new Label
            {
                Text = "Max Salary:",
                Location = new Point(290, 220),
                Size = new Size(80, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblMaxSalary);

            nudMaxSalary = new NumericUpDown
            {
                Location = new Point(370, 220),
                Size = new Size(130, 23),
                Font = new Font("Segoe UI", 9),
                DecimalPlaces = 2,
                Maximum = 999999999,
                Minimum = 0,
                ThousandsSeparator = true
            };
            panelForm.Controls.Add(nudMaxSalary);

            // Requirements
            lblRequirements = new Label
            {
                Text = "Requirements:",
                Location = new Point(20, 260),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelForm.Controls.Add(lblRequirements);

            txtRequirements = new TextBox
            {
                Location = new Point(150, 260),
                Size = new Size(270, 80),
                Font = new Font("Segoe UI", 9),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            panelForm.Controls.Add(txtRequirements);

            // Active Status
            chkActive = new CheckBox
            {
                Text = "Active",
                Location = new Point(150, 360),
                Size = new Size(100, 23),
                Font = new Font("Segoe UI", 9),
                Checked = true
            };
            panelForm.Controls.Add(chkActive);

            // Buttons Panel
            panelButtons = new Panel
            {
                Location = new Point(20, 500),
                Size = new Size(450, 50)
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
                Location = new Point(490, 80),
                Size = new Size(480, 470),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelList);

            // Search Panel
            panelSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70
            };
            //panelList.Controls.Add(panelSearch);

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
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            panelSearch.Controls.Add(txtSearch);

            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(280, 8),
                Size = new Size(80, 27),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.Click += BtnRefresh_Click;
            panelSearch.Controls.Add(btnRefresh);

            Label lblFilterDept = new Label
            {
                Text = "Department:",
                Location = new Point(10, 40),
                Size = new Size(80, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelSearch.Controls.Add(lblFilterDept);

            cboFilterDepartment = new ComboBox
            {
                Location = new Point(100, 40),
                Size = new Size(170, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboFilterDepartment.SelectedIndexChanged += CboFilterDepartment_SelectedIndexChanged;
            panelSearch.Controls.Add(cboFilterDepartment);

            // DataGridView
            dgvJobTitles = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false
            };
            dgvJobTitles.CellDoubleClick += DgvJobTitles_CellDoubleClick;
            dgvJobTitles.SelectionChanged += DgvJobTitles_SelectionChanged;

            Panel panelGrid = new Panel
            {
                Dock = DockStyle.Fill
            };
            panelGrid.Controls.Add(dgvJobTitles);
            //added
            panelList.Controls.Add(panelGrid);
            panelList.Controls.Add(panelSearch);

        }

        private void LoadDepartments()
        {
            try
            {
                string query = "SELECT department_id, department_name FROM departments WHERE is_active = 1 ORDER BY department_name";
                DataTable dt = DatabaseManager.GetDataTable(query);

                // For form combo
                cboDepartment.DisplayMember = "department_name";
                cboDepartment.ValueMember = "department_id";
                cboDepartment.DataSource = dt.Copy();

                // For filter combo
                DataTable filterDt = dt.Copy();
                DataRow allRow = filterDt.NewRow();
                allRow["department_id"] = 0;
                allRow["department_name"] = "All Departments";
                filterDt.Rows.InsertAt(allRow, 0);

                cboFilterDepartment.DisplayMember = "department_name";
                cboFilterDepartment.ValueMember = "department_id";
                cboFilterDepartment.DataSource = filterDt;
                cboFilterDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadJobTitles()
        {
            try
            {
                string query = @"
                    SELECT 
                        jt.job_title_id as 'ID',
                        jt.job_code as 'Code',
                        jt.job_title as 'Job Title',
                        jt.description as 'Description',
                        d.department_name as 'Department',
                        CONCAT('₱', FORMAT(jt.min_salary, 2)) as 'Min Salary',
                        CONCAT('₱', FORMAT(jt.max_salary, 2)) as 'Max Salary',
                        CASE WHEN jt.is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status'
                    FROM job_titles jt
                    LEFT JOIN departments d ON jt.department_id = d.department_id
                    ORDER BY jt.job_title_id DESC";

                DataTable dt = DatabaseManager.GetDataTable(query);
                dgvJobTitles.DataSource = dt;

                if (dgvJobTitles.Columns["ID"] != null)
                    dgvJobTitles.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job titles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadJobTitle(int jobTitleId)
        {
            try
            {
                string query = "SELECT * FROM job_titles WHERE job_title_id = @id";
                var parameters = new Dictionary<string, object> { { "@id", jobTitleId } };
                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                DataTable dt = DatabaseManager.GetDataTable(query, mysqlParams);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtJobCode.Text = row["job_code"].ToString();
                    txtJobTitle.Text = row["job_title"].ToString();
                    txtDescription.Text = row["description"].ToString();
                    cboDepartment.SelectedValue = Convert.ToInt32(row["department_id"]);
                    nudMinSalary.Value = Convert.ToDecimal(row["min_salary"]);
                    nudMaxSalary.Value = Convert.ToDecimal(row["max_salary"]);
                    txtRequirements.Text = row["requirements"].ToString();
                    chkActive.Checked = Convert.ToBoolean(row["is_active"]);
                    currentJobTitleId = jobTitleId;
                    isEditing = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job title: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    { "@job_code", txtJobCode.Text.Trim() },
                    { "@title", txtJobTitle.Text.Trim() },
                    { "@description", txtDescription.Text.Trim() },
                    { "@department_id", cboDepartment.SelectedValue },
                    { "@min_salary", nudMinSalary.Value },
                    { "@max_salary", nudMaxSalary.Value },
                    { "@requirements", txtRequirements.Text.Trim() },
                    { "@active", chkActive.Checked }
                };

                if (isEditing && currentJobTitleId > 0)
                {
                    query = @"
                        UPDATE job_titles SET 
                            job_code = @job_code,
                            job_title = @title,
                            description = @description,
                            department_id = @department_id,
                            min_salary = @min_salary,
                            max_salary = @max_salary,
                            requirements = @requirements,
                            is_active = @active,
                            updated_at = NOW()
                        WHERE job_title_id = @id";
                    parameters.Add("@id", currentJobTitleId);
                }
                else
                {
                    query = @"
                        INSERT INTO job_titles (job_code, job_title, description, department_id, min_salary, max_salary, requirements, is_active, created_at, updated_at)
                        VALUES (@job_code, @title, @description, @department_id, @min_salary, @max_salary, @requirements, @active, NOW(), NOW())";
                }

                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int result = DatabaseManager.ExecuteNonQuery(query, mysqlParams);
                if (result > 0)
                {
                    MessageBox.Show("Job title saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadJobTitles();
                    ClearForm();
                    SetFormMode(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving job title: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvJobTitles.SelectedRows.Count > 0)
            {
                currentJobTitleId = Convert.ToInt32(dgvJobTitles.SelectedRows[0].Cells["ID"].Value);
                LoadJobTitle(currentJobTitleId);
                isEditing = true;
                SetFormMode(true);
            }
            else
            {
                MessageBox.Show("Please select a job title to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvJobTitles.SelectedRows.Count > 0)
            {
                int jobTitleId = Convert.ToInt32(dgvJobTitles.SelectedRows[0].Cells["ID"].Value);
                string jobTitle = dgvJobTitles.SelectedRows[0].Cells["Job Title"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete the job title '{jobTitle}'?\n\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string query = "DELETE FROM job_titles WHERE job_title_id = @id";
                        var parameters = new Dictionary<string, object> { { "@id", jobTitleId } };
                        MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                        int deleteResult = DatabaseManager.ExecuteNonQuery(query, mysqlParams);

                        if (deleteResult > 0)
                        {
                            MessageBox.Show("Job title deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadJobTitles();
                            ClearForm();
                            SetFormMode(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting job title: {ex.Message}\n\nNote: Cannot delete job titles that have employees assigned.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a job title to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            isEditing = false;
            currentJobTitleId = 0;
            SetFormMode(true);
            txtJobTitle.Focus();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadJobTitles();
            txtSearch.Clear();
            cboFilterDepartment.SelectedIndex = 0;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void CboFilterDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (dgvJobTitles.DataSource is DataTable dt)
            {
                string searchText = txtSearch.Text.Trim();
                int selectedDeptId = Convert.ToInt32(cboFilterDepartment.SelectedValue ?? 0);

                string filter = string.Empty;
                List<string> conditions = new List<string>();

                if (!string.IsNullOrEmpty(searchText))
                {
                    conditions.Add($"([Job Title] LIKE '%{searchText}%' OR [Description] LIKE '%{searchText}%')");
                }

                if (selectedDeptId > 0)
                {
                    conditions.Add("[Department] = '" + cboFilterDepartment.Text + "'");
                }

                if (conditions.Count > 0)
                {
                    filter = string.Join(" AND ", conditions);
                }

                dt.DefaultView.RowFilter = filter;
                dt.DefaultView.Sort = "ID DESC";
            }
        }

        private void DgvJobTitles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEdit_Click(sender, e);
            }
        }

        private void DgvJobTitles_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dgvJobTitles.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private bool ValidateForm()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtJobCode.Text))
                {
                    MessageBox.Show("Job code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtJobCode.Focus();
                    return false;
                }

                if (txtJobCode.Text.Trim().Length < 2 || txtJobCode.Text.Trim().Length > 20)
                {
                    MessageBox.Show("Job code must be between 2 and 20 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtJobCode.Focus();
                    return false;
                }
                
                if (string.IsNullOrWhiteSpace(txtJobTitle.Text))
                {
                    MessageBox.Show("Job title is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtJobTitle.Focus();
                    return false;
                }

                if (txtJobTitle.Text.Trim().Length < 2)
                {
                    MessageBox.Show("Job title must be at least 2 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtJobTitle.Focus();
                    return false;
                }

                // Check for unique job code
                string query = "SELECT COUNT(*) FROM job_titles WHERE LOWER(job_code) = LOWER(@code) AND job_title_id != @id";
                var parameters = new Dictionary<string, object>
                {
                    { "@code", txtJobCode.Text.Trim() },
                    { "@id", currentJobTitleId }
                };
                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int count = Convert.ToInt32(DatabaseManager.ExecuteScalar(query, mysqlParams));
                
                if (count > 0)
                {
                    MessageBox.Show("This job code already exists. Please enter a unique job code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtJobCode.Focus();
                    return false;
                }
                
                // Check for unique job title
                query = "SELECT COUNT(*) FROM job_titles WHERE LOWER(job_title) = LOWER(@title) AND job_title_id != @id";
                parameters = new Dictionary<string, object>
                {
                    { "@title", txtJobTitle.Text.Trim() },
                    { "@id", currentJobTitleId }
                };
                mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                count = Convert.ToInt32(DatabaseManager.ExecuteScalar(query, mysqlParams));
                
                if (count > 0)
                {
                    MessageBox.Show("This job title already exists. Please enter a unique job title.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtJobTitle.Focus();
                    return false;
                }

                if (cboDepartment.SelectedValue == null)
                {
                    MessageBox.Show("Please select a department.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboDepartment.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtDescription.Text))
                {
                    MessageBox.Show("Job description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDescription.Focus();
                    return false;
                }

                if (txtDescription.Text.Trim().Length < 10)
                {
                    MessageBox.Show("Job description must be at least 10 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDescription.Focus();
                    return false;
                }

                if (nudMinSalary.Value <= 0)
                {
                    MessageBox.Show("Minimum salary must be greater than zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nudMinSalary.Focus();
                    return false;
                }

                if (nudMaxSalary.Value <= 0)
                {
                    MessageBox.Show("Maximum salary must be greater than zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nudMaxSalary.Focus();
                    return false;
                }

                if (nudMinSalary.Value > nudMaxSalary.Value)
                {
                    MessageBox.Show("Minimum salary cannot be greater than maximum salary.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nudMinSalary.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtRequirements.Text))
                {
                    MessageBox.Show("Job requirements are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRequirements.Focus();
                    return false;
                }

                if (txtRequirements.Text.Trim().Length < 10)
                {
                    MessageBox.Show("Job requirements must be at least 10 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRequirements.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void SetFormMode(bool editMode)
        {
            txtJobTitle.Enabled = editMode;
            txtDescription.Enabled = editMode;
            cboDepartment.Enabled = editMode;
            nudMinSalary.Enabled = editMode;
            nudMaxSalary.Enabled = editMode;
            txtRequirements.Enabled = editMode;
            chkActive.Enabled = editMode;
            btnSave.Enabled = editMode;
            btnNew.Enabled = !editMode;
        }

        private void ClearForm()
        {
            txtJobCode.Clear();
            txtJobTitle.Clear();
            txtDescription.Clear();
            if (cboDepartment.Items.Count > 0)
                cboDepartment.SelectedIndex = 0;
            nudMinSalary.Value = 0;
            nudMaxSalary.Value = 0;
            txtRequirements.Clear();
            chkActive.Checked = true;
            currentJobTitleId = 0;
            isEditing = false;
        }
    }
}
