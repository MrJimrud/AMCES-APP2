using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PayrollSystem
{
    public partial class frmJobStatusList : Form
    {
        private DataGridView dgvJobStatus;
        private TextBox txtSearch;
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

        public frmJobStatusList()
        {
            InitializeComponent();
            LoadJobStatusList();
        }

        private void InitializeComponent()
        {
            this.Text = "Job Status Management";
            this.Size = new Size(900, 600);
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
            txtSearch.SetPlaceholderText("Search by status name or description...");
            txtSearch.TextChanged += TxtSearch_TextChanged;

            Label lblFilter = new Label
            {
                Text = "Filter:",
                Location = new Point(300, 25),
                Size = new Size(50, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cmbFilter = new ComboBox
            {
                Location = new Point(355, 22),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilter.Items.AddRange(new[] { "All", "Active", "Inactive" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += CmbFilter_SelectedIndexChanged;

            // Buttons
            btnAdd = new Button
            {
                Text = "Add New",
                Location = new Point(500, 20),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "Edit",
                Location = new Point(590, 20),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "Delete",
                Location = new Point(660, 20),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(730, 20),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.Click += BtnRefresh_Click;

            pnlTop.Controls.AddRange(new Control[] { lblSearch, txtSearch, lblFilter, cmbFilter, btnAdd, btnEdit, btnDelete, btnRefresh });

            // DataGridView
            dgvJobStatus = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvJobStatus.CellDoubleClick += DgvJobStatus_CellDoubleClick;

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

            btnExport = new Button
            {
                Text = "Export to CSV",
                Location = new Point(650, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExport.Click += BtnExport_Click;

            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(760, 10),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += BtnClose_Click;

            pnlBottom.Controls.AddRange(new Control[] { lblTotal, btnExport, btnClose });

            // Add controls to form
            this.Controls.AddRange(new Control[] { pnlTop, dgvJobStatus, pnlBottom });

            // Apply styling
            UtilityHelper.ApplyLightMode(dgvJobStatus);
        }

        private void LoadJobStatusList()
        {
            try
            {
                string query = @"
                    SELECT 
                        id,
                        status_name as 'Status Name',
                        description as 'Description',
                        CASE WHEN is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status',
                        created_date as 'Created Date',
                        modified_date as 'Modified Date'
                    FROM tbl_job_status 
                    ORDER BY status_name";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvJobStatus.DataSource = dt;

                // Hide ID column
                if (dgvJobStatus.Columns["id"] != null)
                    dgvJobStatus.Columns["id"].Visible = false;

                // Format date columns
                if (dgvJobStatus.Columns["Created Date"] != null)
                    dgvJobStatus.Columns["Created Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
                if (dgvJobStatus.Columns["Modified Date"] != null)
                    dgvJobStatus.Columns["Modified Date"].DefaultCellStyle.Format = "MM/dd/yyyy";

                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job status list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                string filterStatus = cmbFilter.SelectedItem?.ToString() ?? "All";

                string query = @"
                    SELECT 
                        id,
                        status_name as 'Status Name',
                        description as 'Description',
                        CASE WHEN is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status',
                        created_date as 'Created Date',
                        modified_date as 'Modified Date'
                    FROM tbl_job_status 
                    WHERE 1=1";

                if (!string.IsNullOrEmpty(searchText))
                {
                    query += $" AND (status_name LIKE '%{searchText}%' OR description LIKE '%{searchText}%')";
                }

                if (filterStatus != "All")
                {
                    bool isActive = filterStatus == "Active";
                    query += $" AND is_active = {(isActive ? 1 : 0)}";
                }

                query += " ORDER BY status_name";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvJobStatus.DataSource = dt;

                // Hide ID column
                if (dgvJobStatus.Columns["id"] != null)
                    dgvJobStatus.Columns["id"].Visible = false;

                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRecordCount()
        {
            int count = dgvJobStatus.Rows.Count;
            lblTotal.Text = $"Total Records: {count:N0}";
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void CmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new frmJobStatusEdit())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadJobStatusList();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditSelectedJobStatus();
        }

        private void DgvJobStatus_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditSelectedJobStatus();
            }
        }

        private void EditSelectedJobStatus()
        {
            if (dgvJobStatus.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a job status to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int jobStatusId = Convert.ToInt32(dgvJobStatus.SelectedRows[0].Cells["id"].Value);
                using (var form = new frmJobStatusEdit(jobStatusId))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadJobStatusList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing job status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvJobStatus.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a job status to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(GlobalVariables.DeleteMessage, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int jobStatusId = Convert.ToInt32(dgvJobStatus.SelectedRows[0].Cells["id"].Value);
                    string statusName = dgvJobStatus.SelectedRows[0].Cells["Status Name"].Value.ToString();

                    // Check if job status is being used
                    string checkQuery = $"SELECT COUNT(*) FROM tbl_employee WHERE job_status_id = {jobStatusId}";
                    int usageCount = Convert.ToInt32(UtilityHelper.GetScalar(checkQuery));

                    if (usageCount > 0)
                    {
                        MessageBox.Show($"Cannot delete job status '{statusName}' because it is being used by {usageCount} employee(s).", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string deleteQuery = $"DELETE FROM tbl_job_status WHERE id = {jobStatusId}";
                    DatabaseManager.ExecuteNonQuery(deleteQuery);

                    MessageBox.Show(GlobalVariables.DeleteSuccessMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadJobStatusList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting job status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadJobStatusList();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv";
                    sfd.FileName = $"JobStatusList_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder csv = new StringBuilder();

                        // Add headers
                        var headers = new List<string>();
                        for (int i = 1; i < dgvJobStatus.Columns.Count; i++) // Skip ID column
                        {
                            if (dgvJobStatus.Columns[i].Visible)
                                headers.Add(dgvJobStatus.Columns[i].HeaderText);
                        }
                        csv.AppendLine(string.Join(",", headers));

                        // Add data
                        foreach (DataGridViewRow row in dgvJobStatus.Rows)
                        {
                            var values = new List<string>();
                            for (int i = 1; i < dgvJobStatus.Columns.Count; i++) // Skip ID column
                            {
                                if (dgvJobStatus.Columns[i].Visible)
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

    // Helper form for editing job status
    public partial class frmJobStatusEdit : Form
    {
        private int jobStatusId;
        private bool isEditMode;
        private TextBox txtStatusName;
        private TextBox txtDescription;
        private CheckBox chkIsActive;
        private Button btnSave;
        private Button btnCancel;

        public frmJobStatusEdit(int id = 0)
        {
            jobStatusId = id;
            isEditMode = id > 0;
            InitializeComponent();
            if (isEditMode)
                LoadJobStatusData();
        }

        private void InitializeComponent()
        {
            this.Text = isEditMode ? "Edit Job Status" : "Add New Job Status";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblStatusName = new Label
            {
                Text = "Status Name:",
                Location = new Point(20, 30),
                Size = new Size(100, 23)
            };

            txtStatusName = new TextBox
            {
                Location = new Point(130, 27),
                Size = new Size(220, 23),
                MaxLength = 50
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
                Size = new Size(220, 60),
                Multiline = true,
                MaxLength = 255
            };

            chkIsActive = new CheckBox
            {
                Text = "Active",
                Location = new Point(130, 140),
                Size = new Size(100, 23),
                Checked = true
            };

            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(190, 180),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(275, 180),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += BtnCancel_Click;

            this.Controls.AddRange(new Control[] { lblStatusName, txtStatusName, lblDescription, txtDescription, chkIsActive, btnSave, btnCancel });
        }

        private void LoadJobStatusData()
        {
            try
            {
                string query = $"SELECT * FROM tbl_job_status WHERE id = {jobStatusId}";
                DataTable dt = UtilityHelper.GetDataSet(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtStatusName.Text = row["status_name"].ToString();
                    txtDescription.Text = row["description"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(row["is_active"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job status data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SaveJobStatus();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtStatusName.Text))
            {
                MessageBox.Show("Please enter a status name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStatusName.Focus();
                return false;
            }

            // Check for duplicate status name
            string checkQuery = $"SELECT COUNT(*) FROM tbl_job_status WHERE status_name = '{txtStatusName.Text.Trim()}' AND id != {jobStatusId}";
            int count = Convert.ToInt32(UtilityHelper.GetScalar(checkQuery));
            if (count > 0)
            {
                MessageBox.Show("A job status with this name already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStatusName.Focus();
                return false;
            }

            return true;
        }

        private void SaveJobStatus()
        {
            try
            {
                string query;
                if (isEditMode)
                {
                    query = $@"
                        UPDATE tbl_job_status SET 
                            status_name = '{txtStatusName.Text.Trim()}',
                            description = '{txtDescription.Text.Trim()}',
                            is_active = {(chkIsActive.Checked ? 1 : 0)},
                            modified_date = NOW()
                        WHERE id = {jobStatusId}";
                }
                else
                {
                    query = $@"
                        INSERT INTO tbl_job_status (status_name, description, is_active, created_date, modified_date)
                        VALUES ('{txtStatusName.Text.Trim()}', '{txtDescription.Text.Trim()}', {(chkIsActive.Checked ? 1 : 0)}, NOW(), NOW())";
                }

                DatabaseManager.ExecuteNonQuery(query);

                string message = isEditMode ? GlobalVariables.UpdateSuccessMessage : GlobalVariables.SaveSuccessMessage;
                MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving job status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
