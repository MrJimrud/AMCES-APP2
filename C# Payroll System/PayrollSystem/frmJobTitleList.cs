using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PayrollSystem
{
    public partial class frmJobTitleList : Form
    {
        private DataGridView dgvJobTitle;
        private TextBox txtSearch;
        private ComboBox cmbDepartment;
        private ComboBox cmbFilter;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnExport;
        private Button btnClose;
        private Label lblTotal;
        private Panel pnlTop;
        private Panel pnlBottom;

        public frmJobTitleList()
        {
            InitializeComponent();
            LoadDepartments();
            LoadJobTitleList();
        }

        private void InitializeComponent()
        {
            this.Text = "Job Title Management";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Top panel
            pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Search controls
            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 25),
                Size = new Size(60, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            txtSearch = new TextBox
            {
                Location = new Point(85, 22),
                Size = new Size(200, 23)
            };
            txtSearch.SetPlaceholderText("Search by title or description...");
            txtSearch.TextChanged += TxtSearch_TextChanged;

            Label lblDepartment = new Label
            {
                Text = "Department:",
                Location = new Point(280, 25),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cmbDepartment = new ComboBox
            {
                Location = new Point(365, 22),
                Size = new Size(140, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDepartment.SelectedIndexChanged += CmbDepartment_SelectedIndexChanged;

            Label lblFilter = new Label
            {
                Text = "Status:",
                Location = new Point(520, 25),
                Size = new Size(50, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cmbFilter = new ComboBox
            {
                Location = new Point(575, 22),
                Size = new Size(100, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilter.Items.AddRange(new[] { "All", "Active", "Inactive" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += CmbFilter_SelectedIndexChanged;

            // Buttons
            btnAdd = new Button
            {
                Text = "Add New",
                Location = new Point(700, 20),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "Edit",
                Location = new Point(790, 20),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "Delete",
                Location = new Point(860, 20),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.Click += BtnDelete_Click;

            pnlTop.Controls.AddRange(new Control[] { lblSearch, txtSearch, lblDepartment, cmbDepartment, lblFilter, cmbFilter, btnAdd, btnEdit, btnDelete });

            // DataGridView
            dgvJobTitle = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvJobTitle.CellDoubleClick += DgvJobTitle_CellDoubleClick;

            // Bottom panel
            pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotal = new Label
            {
                Text = "Total Records: 0",
                Location = new Point(20, 15),
                Size = new Size(200, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(650, 10),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.Click += BtnRefresh_Click;

            btnExport = new Button
            {
                Text = "Export to CSV",
                Location = new Point(730, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExport.Click += BtnExport_Click;

            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(840, 10),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += BtnClose_Click;

            pnlBottom.Controls.AddRange(new Control[] { lblTotal, btnRefresh, btnExport, btnClose });

            // Add controls to form
            this.Controls.AddRange(new Control[] { pnlTop, dgvJobTitle, pnlBottom });

            // Apply styling
            UtilityHelper.ApplyLightMode(dgvJobTitle);
        }

        private void LoadDepartments()
        {
            try
            {
                cmbDepartment.Items.Clear();
                cmbDepartment.Items.Add("All Departments");

                string query = "SELECT id, department_name FROM tbl_department WHERE is_active = 1 ORDER BY department_name";
                DataTable dt = UtilityHelper.GetDataSet(query);

                foreach (DataRow row in dt.Rows)
                {
                    cmbDepartment.Items.Add(new ComboBoxItem
                    {
                        Text = row["department_name"].ToString(),
                        Value = Convert.ToInt32(row["id"])
                    });
                }

                cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadJobTitleList()
        {
            try
            {
                string query = @"
                    SELECT 
                        jt.job_title_id,
                        jt.job_title as 'Job Title',
                        jt.description as 'Description',
                        d.department_name as 'Department',
                        FORMAT(jt.min_salary, 2) as 'Min Salary',
                        FORMAT(jt.max_salary, 2) as 'Max Salary',
                        CASE WHEN jt.is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status',
                        jt.created_at as 'Created Date',
                        jt.updated_at as 'Modified Date'
                    FROM job_titles jt
                    LEFT JOIN departments d ON jt.department_id = d.department_id
                    ORDER BY jt.job_title";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvJobTitle.DataSource = dt;

                // Hide ID column
                if (dgvJobTitle.Columns["job_title_id"] != null)
                    dgvJobTitle.Columns["job_title_id"].Visible = false;

                // Format columns
                if (dgvJobTitle.Columns["Min Salary"] != null)
                {
                    dgvJobTitle.Columns["Min Salary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvJobTitle.Columns["Min Salary"].DefaultCellStyle.Format = "N2";
                }
                
                if (dgvJobTitle.Columns["Max Salary"] != null)
                {
                    dgvJobTitle.Columns["Max Salary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvJobTitle.Columns["Max Salary"].DefaultCellStyle.Format = "N2";
                }

                if (dgvJobTitle.Columns["Created Date"] != null)
                    dgvJobTitle.Columns["Created Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
                if (dgvJobTitle.Columns["Modified Date"] != null)
                    dgvJobTitle.Columns["Modified Date"].DefaultCellStyle.Format = "MM/dd/yyyy";

                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job title list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                string filterStatus = cmbFilter.SelectedItem?.ToString() ?? "All";
                int departmentId = 0;

                if (cmbDepartment.SelectedItem is ComboBoxItem selectedDept)
                {
                    departmentId = selectedDept.Value;
                }

                string query = @"
                    SELECT 
                        jt.job_title_id,
                        jt.job_title as 'Job Title',
                        jt.description as 'Description',
                        d.department_name as 'Department',
                        FORMAT(jt.min_salary, 2) as 'Min Salary',
                        FORMAT(jt.max_salary, 2) as 'Max Salary',
                        CASE WHEN jt.is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status',
                        jt.created_at as 'Created Date',
                        jt.updated_at as 'Modified Date'
                    FROM job_titles jt
                    LEFT JOIN departments d ON jt.department_id = d.department_id
                    WHERE 1=1";

                if (!string.IsNullOrEmpty(searchText))
                {
                    query += $" AND (jt.job_title LIKE '%{searchText}%' OR jt.description LIKE '%{searchText}%')";
                }

                if (departmentId > 0)
                {
                    query += $" AND jt.department_id = {departmentId}";
                }

                if (filterStatus != "All")
                {
                    bool isActive = filterStatus == "Active";
                    query += $" AND jt.is_active = {(isActive ? 1 : 0)}";
                }

                query += " ORDER BY jt.job_title";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvJobTitle.DataSource = dt;

                // Hide ID column
                if (dgvJobTitle.Columns["job_title_id"] != null)
                    dgvJobTitle.Columns["job_title_id"].Visible = false;

                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRecordCount()
        {
            int count = dgvJobTitle.Rows.Count;
            lblTotal.Text = $"Total Records: {count:N0}";
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void CmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void CmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new frmJobTitleEdit())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadJobTitleList();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditSelectedJobTitle();
        }

        private void DgvJobTitle_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditSelectedJobTitle();
            }
        }

        private void EditSelectedJobTitle()
        {
            if (dgvJobTitle.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a job title to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int jobTitleId = Convert.ToInt32(dgvJobTitle.SelectedRows[0].Cells["job_title_id"].Value);
                using (var form = new frmJobTitleEdit(jobTitleId))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadJobTitleList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing job title: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvJobTitle.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a job title to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(GlobalVariables.DeleteMessage, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int jobTitleId = Convert.ToInt32(dgvJobTitle.SelectedRows[0].Cells["job_title_id"].Value);
                    string titleName = dgvJobTitle.SelectedRows[0].Cells["Job Title"].Value.ToString();

                    // Check if job title is being used
                    string checkQuery = $"SELECT COUNT(*) FROM employees WHERE job_title_id = {jobTitleId}";
                    int usageCount = Convert.ToInt32(UtilityHelper.GetScalar(checkQuery));

                    if (usageCount > 0)
                    {
                        MessageBox.Show($"Cannot delete job title '{titleName}' because it is being used by {usageCount} employee(s).", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string deleteQuery = $"DELETE FROM job_titles WHERE job_title_id = {jobTitleId}";
                    DatabaseManager.ExecuteNonQuery(deleteQuery);

                    MessageBox.Show(GlobalVariables.DeleteSuccessMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadJobTitleList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting job title: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadJobTitleList();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv";
                    sfd.FileName = $"JobTitleList_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder csv = new StringBuilder();

                        // Add headers
                        var headers = new List<string>();
                        for (int i = 1; i < dgvJobTitle.Columns.Count; i++) // Skip ID column
                        {
                            if (dgvJobTitle.Columns[i].Visible)
                                headers.Add(dgvJobTitle.Columns[i].HeaderText);
                        }
                        csv.AppendLine(string.Join(",", headers));

                        // Add data
                        foreach (DataGridViewRow row in dgvJobTitle.Rows)
                        {
                            var values = new List<string>();
                            for (int i = 1; i < dgvJobTitle.Columns.Count; i++) // Skip ID column
                            {
                                if (dgvJobTitle.Columns[i].Visible)
                                {
                                    string value = row.Cells[i].Value?.ToString() ?? "";
                                    values.Add($"\"{value}\"");
                                }
                            }
                            csv.AppendLine(string.Join(",", values));
                        }

                        File.WriteAllText(sfd.FileName, csv.ToString());
                        MessageBox.Show($"Data exported successfully to {sfd.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // Helper class for ComboBox items
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    // Helper form for editing job titles
    public partial class frmJobTitleEdit : Form
    {
        private int jobTitleId;
        private bool isEditMode;
        private TextBox txtTitleName;
        private TextBox txtDescription;
        private ComboBox cmbDepartment;
        private NumericUpDown nudBaseSalary;
        private TextBox txtSalaryGrade;
        private CheckBox chkIsActive;
        private Button btnSave;
        private Button btnCancel;

        public frmJobTitleEdit(int id = 0)
        {
            jobTitleId = id;
            isEditMode = id > 0;
            InitializeComponent();
            LoadDepartments();
            if (isEditMode)
                LoadJobTitleData();
        }

        private void InitializeComponent()
        {
            this.Text = isEditMode ? "Edit Job Title" : "Add New Job Title";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblTitleName = new Label
            {
                Text = "Title Name:",
                Location = new Point(20, 30),
                Size = new Size(100, 23)
            };

            txtTitleName = new TextBox
            {
                Location = new Point(130, 27),
                Size = new Size(280, 23),
                MaxLength = 100
            };

            Label lblDescription = new Label
            {
                Text = "Description:",
                Location = new Point(20, 70),
                Size = new Size(100, 23)
            };

            txtDescription = new TextBox
            {
                Location = new Point(130, 67),
                Size = new Size(280, 60),
                Multiline = true,
                MaxLength = 255
            };

            Label lblDepartment = new Label
            {
                Text = "Department:",
                Location = new Point(20, 140),
                Size = new Size(100, 23)
            };

            cmbDepartment = new ComboBox
            {
                Location = new Point(130, 137),
                Size = new Size(280, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblBaseSalary = new Label
            {
                Text = "Base Salary:",
                Location = new Point(20, 180),
                Size = new Size(100, 23)
            };

            nudBaseSalary = new NumericUpDown
            {
                Location = new Point(130, 177),
                Size = new Size(120, 23),
                DecimalPlaces = 2,
                Maximum = 999999.99m,
                Minimum = 0
            };

            Label lblSalaryGrade = new Label
            {
                Text = "Salary Grade:",
                Location = new Point(260, 180),
                Size = new Size(80, 23)
            };

            txtSalaryGrade = new TextBox
            {
                Location = new Point(345, 177),
                Size = new Size(65, 23),
                MaxLength = 10
            };

            chkIsActive = new CheckBox
            {
                Text = "Active",
                Location = new Point(130, 220),
                Size = new Size(100, 23),
                Checked = true
            };

            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(250, 270),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(335, 270),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += BtnCancel_Click;

            this.Controls.AddRange(new Control[] { 
                lblTitleName, txtTitleName, lblDescription, txtDescription, 
                lblDepartment, cmbDepartment, lblBaseSalary, nudBaseSalary, 
                lblSalaryGrade, txtSalaryGrade, chkIsActive, btnSave, btnCancel 
            });
        }

        private void LoadDepartments()
        {
            try
            {
                cmbDepartment.Items.Clear();

                string query = "SELECT id, department_name FROM tbl_department WHERE is_active = 1 ORDER BY department_name";
                DataTable dt = UtilityHelper.GetDataSet(query);

                foreach (DataRow row in dt.Rows)
                {
                    cmbDepartment.Items.Add(new ComboBoxItem
                    {
                        Text = row["department_name"].ToString(),
                        Value = Convert.ToInt32(row["id"])
                    });
                }

                if (cmbDepartment.Items.Count > 0)
                    cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadJobTitleData()
        {
            try
            {
                string query = $"SELECT * FROM job_titles WHERE job_title_id = {jobTitleId}";
                DataTable dt = UtilityHelper.GetDataSet(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtTitleName.Text = row["job_title"].ToString();
                    txtDescription.Text = row["description"].ToString();
                    nudBaseSalary.Value = Convert.ToDecimal(row["min_salary"]);
                    txtSalaryGrade.Text = row["max_salary"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(row["is_active"]);

                    // Select department
                    int departmentId = Convert.ToInt32(row["department_id"]);
                    for (int i = 0; i < cmbDepartment.Items.Count; i++)
                    {
                        if (cmbDepartment.Items[i] is ComboBoxItem item && item.Value == departmentId)
                        {
                            cmbDepartment.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job title data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SaveJobTitle();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitleName.Text))
            {
                MessageBox.Show("Please enter a job title name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitleName.Focus();
                return false;
            }

            if (cmbDepartment.SelectedItem == null)
            {
                MessageBox.Show("Please select a department.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDepartment.Focus();
                return false;
            }

            // Check for duplicate title name in the same department
            var selectedDept = (ComboBoxItem)cmbDepartment.SelectedItem;
            string checkQuery = $"SELECT COUNT(*) FROM job_titles WHERE job_title = '{txtTitleName.Text.Trim()}' AND department_id = {selectedDept.Value} AND job_title_id != {jobTitleId}";
            int count = Convert.ToInt32(UtilityHelper.GetScalar(checkQuery));
            if (count > 0)
            {
                MessageBox.Show("A job title with this name already exists in the selected department.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitleName.Focus();
                return false;
            }

            return true;
        }

        private void SaveJobTitle()
        {
            try
            {
                var selectedDept = (ComboBoxItem)cmbDepartment.SelectedItem;
                
                string query;
                if (isEditMode)
                {
                    query = $@"
                        UPDATE job_titles SET 
                            job_title = '{txtTitleName.Text.Trim()}',
                            description = '{txtDescription.Text.Trim()}',
                            department_id = {selectedDept.Value},
                            min_salary = {nudBaseSalary.Value},
                            max_salary = '{txtSalaryGrade.Text.Trim()}',
                            is_active = {(chkIsActive.Checked ? 1 : 0)},
                            updated_at = NOW()
                        WHERE job_title_id = {jobTitleId}";
                }
                else
                {
                    query = $@"
                        INSERT INTO job_titles (job_title, description, department_id, min_salary, max_salary, is_active, created_at, updated_at)
                        VALUES ('{txtTitleName.Text.Trim()}', '{txtDescription.Text.Trim()}', {selectedDept.Value}, {nudBaseSalary.Value}, '{txtSalaryGrade.Text.Trim()}', {(chkIsActive.Checked ? 1 : 0)}, NOW(), NOW())";
                }

                DatabaseManager.ExecuteNonQuery(query);

                string message = isEditMode ? GlobalVariables.UpdateSuccessMessage : GlobalVariables.SaveSuccessMessage;
                MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving job title: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
