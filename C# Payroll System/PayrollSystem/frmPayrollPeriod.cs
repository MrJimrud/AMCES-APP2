using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmPayrollPeriod : Form
    {
        private bool isEditing = false;
        private int currentPeriodId = 0;

        public frmPayrollPeriod()
        {
            InitializeComponent();
            this.Load += FrmPayrollPeriod_Load;
        }

        private void InitializeComponent()
        {
            this.panelHeader = new Panel();
            this.lblTitle = new Label();
            this.panelForm = new Panel();
            this.lblPeriodName = new Label();
            this.txtPeriodName = new TextBox();
            this.lblStartDate = new Label();
            this.dtpStartDate = new DateTimePicker();
            this.lblEndDate = new Label();
            this.dtpEndDate = new DateTimePicker();
            this.lblPayrollDate = new Label();
            this.dtpPayrollDate = new DateTimePicker();
            this.lblStatus = new Label();
            this.cmbStatus = new ComboBox();
            this.lblDescription = new Label();
            this.txtDescription = new TextBox();
            this.panelButtons = new Panel();
            this.btnSave = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnNew = new Button();
            this.btnClose = new Button();
            this.panelGrid = new Panel();
            this.dgvPayrollPeriods = new DataGridView();
            this.lblGridTitle = new Label();
            this.panelHeader.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayrollPeriods)).BeginInit();
            this.SuspendLayout();

            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = Color.FromArgb(52, 73, 94);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = DockStyle.Top;
            this.panelHeader.Location = new Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(1000, 60);
            this.panelHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(280, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Payroll Period Management";

            // 
            // panelForm
            // 
            this.panelForm.BackColor = Color.White;
            this.panelForm.BorderStyle = BorderStyle.FixedSingle;
            this.panelForm.Controls.Add(this.txtDescription);
            this.panelForm.Controls.Add(this.lblDescription);
            this.panelForm.Controls.Add(this.cmbStatus);
            this.panelForm.Controls.Add(this.lblStatus);
            this.panelForm.Controls.Add(this.dtpPayrollDate);
            this.panelForm.Controls.Add(this.lblPayrollDate);
            this.panelForm.Controls.Add(this.dtpEndDate);
            this.panelForm.Controls.Add(this.lblEndDate);
            this.panelForm.Controls.Add(this.dtpStartDate);
            this.panelForm.Controls.Add(this.lblStartDate);
            this.panelForm.Controls.Add(this.txtPeriodName);
            this.panelForm.Controls.Add(this.lblPeriodName);
            this.panelForm.Location = new Point(20, 80);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new Size(450, 350);
            this.panelForm.TabIndex = 1;

            // 
            // lblPeriodName
            // 
            this.lblPeriodName.AutoSize = true;
            this.lblPeriodName.Font = new Font("Segoe UI", 10F);
            this.lblPeriodName.Location = new Point(20, 20);
            this.lblPeriodName.Name = "lblPeriodName";
            this.lblPeriodName.Size = new Size(85, 19);
            this.lblPeriodName.TabIndex = 0;
            this.lblPeriodName.Text = "Period Name:";

            // 
            // txtPeriodName
            // 
            this.txtPeriodName.Font = new Font("Segoe UI", 10F);
            this.txtPeriodName.Location = new Point(150, 17);
            this.txtPeriodName.Name = "txtPeriodName";
            this.txtPeriodName.Size = new Size(280, 25);
            this.txtPeriodName.TabIndex = 1;

            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new Font("Segoe UI", 10F);
            this.lblStartDate.Location = new Point(20, 60);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new Size(73, 19);
            this.lblStartDate.TabIndex = 2;
            this.lblStartDate.Text = "Start Date:";

            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Font = new Font("Segoe UI", 10F);
            this.dtpStartDate.Format = DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new Point(150, 57);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new Size(280, 25);
            this.dtpStartDate.TabIndex = 3;

            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Font = new Font("Segoe UI", 10F);
            this.lblEndDate.Location = new Point(20, 100);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new Size(67, 19);
            this.lblEndDate.TabIndex = 4;
            this.lblEndDate.Text = "End Date:";

            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Font = new Font("Segoe UI", 10F);
            this.dtpEndDate.Format = DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new Point(150, 97);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new Size(280, 25);
            this.dtpEndDate.TabIndex = 5;

            // 
            // lblPayrollDate
            // 
            this.lblPayrollDate.AutoSize = true;
            this.lblPayrollDate.Font = new Font("Segoe UI", 10F);
            this.lblPayrollDate.Location = new Point(20, 140);
            this.lblPayrollDate.Name = "lblPayrollDate";
            this.lblPayrollDate.Size = new Size(83, 19);
            this.lblPayrollDate.TabIndex = 6;
            this.lblPayrollDate.Text = "Payroll Date:";

            // 
            // dtpPayrollDate
            // 
            this.dtpPayrollDate.Font = new Font("Segoe UI", 10F);
            this.dtpPayrollDate.Format = DateTimePickerFormat.Short;
            this.dtpPayrollDate.Location = new Point(150, 137);
            this.dtpPayrollDate.Name = "dtpPayrollDate";
            this.dtpPayrollDate.Size = new Size(280, 25);
            this.dtpPayrollDate.TabIndex = 7;

            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 10F);
            this.lblStatus.Location = new Point(20, 180);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(48, 19);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Status:";

            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new Font("Segoe UI", 10F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] { "Active", "Inactive", "Closed" });
            this.cmbStatus.Location = new Point(150, 177);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new Size(280, 25);
            this.cmbStatus.TabIndex = 9;

            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new Font("Segoe UI", 10F);
            this.lblDescription.Location = new Point(20, 220);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(78, 19);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "Description:";

            // 
            // txtDescription
            // 
            this.txtDescription.Font = new Font("Segoe UI", 10F);
            this.txtDescription.Location = new Point(150, 217);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(280, 100);
            this.txtDescription.TabIndex = 11;

            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Controls.Add(this.btnNew);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Location = new Point(20, 450);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new Size(450, 60);
            this.panelButtons.TabIndex = 2;

            // 
            // btnSave
            // 
            this.btnSave.BackColor = Color.FromArgb(46, 204, 113);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(10, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(80, 35);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += BtnSave_Click;

            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = Color.FromArgb(52, 152, 219);
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.Location = new Point(100, 10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(80, 35);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += BtnEdit_Click;

            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(190, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(80, 35);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += BtnDelete_Click;

            // 
            // btnNew
            // 
            this.btnNew.BackColor = Color.FromArgb(155, 89, 182);
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = FlatStyle.Flat;
            this.btnNew.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnNew.ForeColor = Color.White;
            this.btnNew.Location = new Point(280, 10);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new Size(80, 35);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += BtnNew_Click;

            // 
            // btnClose
            // 
            this.btnClose.BackColor = Color.FromArgb(149, 165, 166);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(370, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(80, 35);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += BtnClose_Click;

            // 
            // panelGrid
            // 
            this.panelGrid.BackColor = Color.White;
            this.panelGrid.BorderStyle = BorderStyle.FixedSingle;
            this.panelGrid.Controls.Add(this.dgvPayrollPeriods);
            this.panelGrid.Controls.Add(this.lblGridTitle);
            this.panelGrid.Location = new Point(490, 80);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new Size(490, 430);
            this.panelGrid.TabIndex = 3;

            // 
            // lblGridTitle
            // 
            this.lblGridTitle.BackColor = Color.FromArgb(52, 73, 94);
            this.lblGridTitle.Dock = DockStyle.Top;
            this.lblGridTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblGridTitle.ForeColor = Color.White;
            this.lblGridTitle.Location = new Point(0, 0);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new Size(488, 35);
            this.lblGridTitle.TabIndex = 0;
            this.lblGridTitle.Text = "Payroll Periods";
            this.lblGridTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblGridTitle.Padding = new Padding(10, 0, 0, 0);

            // 
            // dgvPayrollPeriods
            // 
            this.dgvPayrollPeriods.AllowUserToAddRows = false;
            this.dgvPayrollPeriods.AllowUserToDeleteRows = false;
            this.dgvPayrollPeriods.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPayrollPeriods.BackgroundColor = Color.White;
            this.dgvPayrollPeriods.BorderStyle = BorderStyle.None;
            this.dgvPayrollPeriods.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPayrollPeriods.Dock = DockStyle.Fill;
            this.dgvPayrollPeriods.Location = new Point(0, 35);
            this.dgvPayrollPeriods.MultiSelect = false;
            this.dgvPayrollPeriods.Name = "dgvPayrollPeriods";
            this.dgvPayrollPeriods.ReadOnly = true;
            this.dgvPayrollPeriods.RowHeadersVisible = false;
            this.dgvPayrollPeriods.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPayrollPeriods.Size = new Size(488, 393);
            this.dgvPayrollPeriods.TabIndex = 1;
            this.dgvPayrollPeriods.SelectionChanged += DgvPayrollPeriods_SelectionChanged;

            // 
            // frmPayrollPeriod
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.ClientSize = new Size(1000, 530);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmPayrollPeriod";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Payroll Period Management";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayrollPeriods)).EndInit();
            this.ResumeLayout(false);
        }

        private void FrmPayrollPeriod_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPayrollPeriods();
                SetFormMode(false);
                cmbStatus.SelectedIndex = 0; // Default to Active
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPayrollPeriods()
        {
            try
            {
                string query = @"
                    SELECT 
                        period_id,
                        period_name,
                        start_date,
                        end_date,
                        payroll_date,
                        status,
                        description
                    FROM payroll_periods 
                    ORDER BY start_date DESC";

                DataTable dt = DatabaseManager.GetDataTable(query);
                dgvPayrollPeriods.DataSource = dt;

                // Format columns
                if (dgvPayrollPeriods.Columns.Count > 0)
                {
                    dgvPayrollPeriods.Columns["period_id"].Visible = false;
                    dgvPayrollPeriods.Columns["period_name"].HeaderText = "Period Name";
                    dgvPayrollPeriods.Columns["start_date"].HeaderText = "Start Date";
                    dgvPayrollPeriods.Columns["end_date"].HeaderText = "End Date";
                    dgvPayrollPeriods.Columns["payroll_date"].HeaderText = "Payroll Date";
                    dgvPayrollPeriods.Columns["status"].HeaderText = "Status";
                    dgvPayrollPeriods.Columns["description"].HeaderText = "Description";

                    dgvPayrollPeriods.Columns["start_date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                    dgvPayrollPeriods.Columns["end_date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                    dgvPayrollPeriods.Columns["payroll_date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payroll periods: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvPayrollPeriods_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvPayrollPeriods.SelectedRows.Count > 0 && !isEditing)
                {
                    DataGridViewRow row = dgvPayrollPeriods.SelectedRows[0];
                    
                    currentPeriodId = Convert.ToInt32(row.Cells["period_id"].Value);
                    txtPeriodName.Text = row.Cells["period_name"].Value.ToString();
                    dtpStartDate.Value = Convert.ToDateTime(row.Cells["start_date"].Value);
                    dtpEndDate.Value = Convert.ToDateTime(row.Cells["end_date"].Value);
                    dtpPayrollDate.Value = Convert.ToDateTime(row.Cells["payroll_date"].Value);
                    cmbStatus.Text = row.Cells["status"].Value.ToString();
                    txtDescription.Text = row.Cells["description"].Value?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading period details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetFormMode(true);
            isEditing = false;
            currentPeriodId = 0;
            txtPeriodName.Focus();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (currentPeriodId == 0)
            {
                MessageBox.Show("Please select a payroll period to edit.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SetFormMode(true);
            isEditing = true;
            txtPeriodName.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                string query;
                if (isEditing)
                {
                    query = @"
                        UPDATE payroll_periods SET 
                            period_name = @period_name,
                            start_date = @start_date,
                            end_date = @end_date,
                            payroll_date = @payroll_date,
                            status = @status,
                            description = @description
                        WHERE period_id = @period_id";
                }
                else
                {
                    query = @"
                        INSERT INTO payroll_periods 
                        (period_name, start_date, end_date, payroll_date, status, description, created_date) 
                        VALUES 
                        (@period_name, @start_date, @end_date, @payroll_date, @status, @description, NOW())";
                }

                // Use DatabaseManager to handle connection management
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@period_name", txtPeriodName.Text.Trim() },
                    { "@start_date", dtpStartDate.Value.Date },
                    { "@end_date", dtpEndDate.Value.Date },
                    { "@payroll_date", dtpPayrollDate.Value.Date },
                    { "@status", cmbStatus.Text },
                    { "@description", txtDescription.Text.Trim() }
                };
                
                if (isEditing)
                    parameters.Add("@period_id", currentPeriodId);

                DatabaseManager.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Payroll period saved successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadPayrollPeriods();
                SetFormMode(false);
                isEditing = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving payroll period: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPeriodId == 0)
                {
                    MessageBox.Show("Please select a payroll period to delete.", "Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this payroll period?", 
                    "Confirm Delete", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM payroll_periods WHERE period_id = @period_id";
                    
                    // Use DatabaseManager to handle connection management
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "@period_id", currentPeriodId }
                    };
                    
                    DatabaseManager.ExecuteNonQuery(query, parameters);

                    MessageBox.Show("Payroll period deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadPayrollPeriods();
                    ClearForm();
                    currentPeriodId = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting payroll period: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtPeriodName.Text))
            {
                MessageBox.Show("Please enter period name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPeriodName.Focus();
                return false;
            }

            // Check for unique period name
            string query = "SELECT COUNT(*) FROM payroll_periods WHERE period_name = @name AND period_id != @id";
            var parameters = new Dictionary<string, object>
            {
                { "@name", txtPeriodName.Text.Trim() },
                { "@id", currentPeriodId }
            };
            MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
            int count = Convert.ToInt32(DatabaseManager.ExecuteScalar(query, mysqlParams));
            
            if (count > 0)
            {
                MessageBox.Show("This period name already exists. Please enter a unique period name.", "Validation Error", 
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

            if (dtpPayrollDate.Value < dtpEndDate.Value)
            {
                MessageBox.Show("Payroll date should be after or equal to end date.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpPayrollDate.Focus();
                return false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select status.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
                return false;
            }

            return true;
        }

        private void SetFormMode(bool editMode)
        {
            txtPeriodName.Enabled = editMode;
            dtpStartDate.Enabled = editMode;
            dtpEndDate.Enabled = editMode;
            dtpPayrollDate.Enabled = editMode;
            cmbStatus.Enabled = editMode;
            txtDescription.Enabled = editMode;
            
            btnSave.Enabled = editMode;
            btnEdit.Enabled = !editMode && currentPeriodId > 0;
            btnDelete.Enabled = !editMode && currentPeriodId > 0;
            btnNew.Enabled = !editMode;
        }

        private void ClearForm()
        {
            txtPeriodName.Clear();
            dtpStartDate.Value = DateTime.Now;
            dtpEndDate.Value = DateTime.Now.AddDays(15);
            dtpPayrollDate.Value = DateTime.Now.AddDays(20);
            cmbStatus.SelectedIndex = 0;
            txtDescription.Clear();
            currentPeriodId = 0;
        }

        #region Designer Variables
        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelForm;
        private Label lblPeriodName;
        private TextBox txtPeriodName;
        private Label lblStartDate;
        private DateTimePicker dtpStartDate;
        private Label lblEndDate;
        private DateTimePicker dtpEndDate;
        private Label lblPayrollDate;
        private DateTimePicker dtpPayrollDate;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private Label lblDescription;
        private TextBox txtDescription;
        private Panel panelButtons;
        private Button btnSave;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnNew;
        private Button btnClose;
        private Panel panelGrid;
        private DataGridView dgvPayrollPeriods;
        private Label lblGridTitle;
        #endregion
    }
}
