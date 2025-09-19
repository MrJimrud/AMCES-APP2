using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmCASearch : Form
    {
        // DatabaseManager is static - no instance needed
        public int SelectedCashAdvanceId { get; private set; } = -1;
        public DataRow SelectedCashAdvance { get; private set; }
        
        // Form Controls
        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelFilters;
        private Label lblEmployee;
        private ComboBox cboEmployee;
        private Label lblStatus;
        private ComboBox cboStatus;
        private Label lblDateFrom;
        private DateTimePicker dtpDateFrom;
        private Label lblDateTo;
        private DateTimePicker dtpDateTo;
        private Button btnSearch;
        private Button btnClear;
        private DataGridView dgvCashAdvances;
        private Button btnSelect;
        private Button btnCancel;
        private Label lblTotal;
        
        public frmCASearch()
        {
            InitializeComponent();
            LoadEmployees();
            LoadCashAdvances();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Cash Advance Search";
            this.Size = new Size(900, 600);
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
            
            lblTitle = new Label
            {
                Text = "Cash Advance Search",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            
            panelHeader.Controls.Add(lblTitle);
            
            // Filters Panel
            panelFilters = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(840, 100),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            // Employee Filter
            lblEmployee = new Label
            {
                Text = "Employee:",
                Location = new Point(20, 20),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            cboEmployee = new ComboBox
            {
                Location = new Point(110, 20),
                Size = new Size(200, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            // Status Filter
            lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(330, 20),
                Size = new Size(60, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            cboStatus = new ComboBox
            {
                Location = new Point(400, 20),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatus.Items.AddRange(new string[] { "All", "Pending", "Approved", "Rejected", "Paid" });
            cboStatus.SelectedIndex = 0;
            
            // Date From
            lblDateFrom = new Label
            {
                Text = "Date From:",
                Location = new Point(540, 20),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            dtpDateFrom = new DateTimePicker
            {
                Location = new Point(630, 20),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };
            
            // Date To
            lblDateTo = new Label
            {
                Text = "Date To:",
                Location = new Point(20, 60),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            dtpDateTo = new DateTimePicker
            {
                Location = new Point(110, 60),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };
            
            // Search Button
            btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(250, 60),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.Click += BtnSearch_Click;
            
            // Clear Button
            btnClear = new Button
            {
                Text = "Clear",
                Location = new Point(340, 60),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClear.Click += BtnClear_Click;
            
            panelFilters.Controls.AddRange(new Control[] {
                lblEmployee, cboEmployee, lblStatus, cboStatus,
                lblDateFrom, dtpDateFrom, lblDateTo, dtpDateTo,
                btnSearch, btnClear
            });
            
            // DataGridView
            dgvCashAdvances = new DataGridView
            {
                Location = new Point(20, 200),
                Size = new Size(840, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvCashAdvances.DoubleClick += DgvCashAdvances_DoubleClick;
            
            // Total Label
            lblTotal = new Label
            {
                Text = "Total Records: 0",
                Location = new Point(20, 510),
                Size = new Size(200, 23),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            
            // Select Button
            btnSelect = new Button
            {
                Text = "Select",
                Location = new Point(680, 510),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSelect.Click += BtnSelect_Click;
            
            // Cancel Button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(770, 510),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += BtnCancel_Click;
            
            this.Controls.AddRange(new Control[] {
                panelHeader, panelFilters, dgvCashAdvances, lblTotal, btnSelect, btnCancel
            });
            
            // Set default date range (last 30 days)
            dtpDateFrom.Value = DateTime.Now.AddDays(-30);
            dtpDateTo.Value = DateTime.Now;
        }
        
        private void LoadEmployees()
        {
            try
            {
                string query = "SELECT employee_id as emp_id, CONCAT(first_name, ' ', last_name) as fullname FROM employees WHERE employment_status = 'Active' ORDER BY last_name";
                DataTable dt = DatabaseManager.GetDataTable(query);
                
                // Add "All" option
                DataRow allRow = dt.NewRow();
                allRow["emp_id"] = -1;
                allRow["fullname"] = "All Employees";
                dt.Rows.InsertAt(allRow, 0);
                
                cboEmployee.DataSource = dt;
                cboEmployee.DisplayMember = "fullname";
                cboEmployee.ValueMember = "emp_id";
                cboEmployee.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadCashAdvances()
        {
            try
            {
                string query = @"SELECT ca.advance_id, CONCAT(e.first_name, ' ', e.last_name) as employee_name,
                               ca.request_date, ca.amount, ca.reason as purpose, ca.status, ca.approved_date,
                               ca.remaining_balance, ca.monthly_deduction
                               FROM cash_advances ca
                               INNER JOIN employees e ON ca.employee_id = e.employee_id
                               WHERE 1=1";
                
                var parameters = new System.Collections.Generic.Dictionary<string, object>();
                
                // Apply filters
                if (cboEmployee.SelectedValue != null && Convert.ToInt32(cboEmployee.SelectedValue) != -1)
                {
                    query += " AND ca.employee_id = @employee_id";
                    parameters.Add("@employee_id", cboEmployee.SelectedValue);
                }
                
                if (cboStatus.SelectedItem.ToString() != "All")
                {
                    query += " AND ca.status = @status";
                    parameters.Add("@status", cboStatus.SelectedItem.ToString());
                }
                
                query += " AND DATE(ca.request_date) BETWEEN @date_from AND @date_to";
                parameters.Add("@date_from", dtpDateFrom.Value.Date);
                parameters.Add("@date_to", dtpDateTo.Value.Date);
                
                query += " ORDER BY ca.request_date DESC";
                
                DataTable dt = DatabaseManager.GetDataTable(query, parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray());
                dgvCashAdvances.DataSource = dt;
                
                // Format columns
                if (dgvCashAdvances.Columns.Count > 0)
                {
                    dgvCashAdvances.Columns["advance_id"].HeaderText = "ID";
                    dgvCashAdvances.Columns["advance_id"].Width = 50;
                    dgvCashAdvances.Columns["employee_name"].HeaderText = "Employee";
                    dgvCashAdvances.Columns["request_date"].HeaderText = "Request Date";
                    dgvCashAdvances.Columns["amount"].HeaderText = "Amount";
                    dgvCashAdvances.Columns["amount"].DefaultCellStyle.Format = "C2";
                    dgvCashAdvances.Columns["purpose"].HeaderText = "Purpose";
                    dgvCashAdvances.Columns["status"].HeaderText = "Status";
                    dgvCashAdvances.Columns["approved_date"].HeaderText = "Approved Date";
                    dgvCashAdvances.Columns["remaining_balance"].HeaderText = "Remaining Balance";
                    dgvCashAdvances.Columns["remaining_balance"].DefaultCellStyle.Format = "C2";
                    dgvCashAdvances.Columns["monthly_deduction"].HeaderText = "Monthly Deduction";
                    dgvCashAdvances.Columns["monthly_deduction"].DefaultCellStyle.Format = "C2";
                    
                    // Color code status
                    foreach (DataGridViewRow row in dgvCashAdvances.Rows)
                    {
                        string status = row.Cells["status"].Value?.ToString();
                        switch (status)
                        {
                            case "Pending":
                                row.DefaultCellStyle.BackColor = Color.LightYellow;
                                break;
                            case "Approved":
                                row.DefaultCellStyle.BackColor = Color.LightGreen;
                                break;
                            case "Rejected":
                                row.DefaultCellStyle.BackColor = Color.LightCoral;
                                break;
                            case "Paid":
                                row.DefaultCellStyle.BackColor = Color.LightBlue;
                                break;
                        }
                    }
                }
                
                lblTotal.Text = $"Total Records: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cash advances: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadCashAdvances();
        }
        
        private void BtnClear_Click(object sender, EventArgs e)
        {
            cboEmployee.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            dtpDateFrom.Value = DateTime.Now.AddDays(-30);
            dtpDateTo.Value = DateTime.Now;
            LoadCashAdvances();
        }
        
        private void DgvCashAdvances_DoubleClick(object sender, EventArgs e)
        {
            SelectCashAdvance();
        }
        
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            SelectCashAdvance();
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void SelectCashAdvance()
        {
            if (dgvCashAdvances.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvCashAdvances.SelectedRows[0];
                SelectedCashAdvanceId = Convert.ToInt32(selectedRow.Cells["advance_id"].Value);
                
                // Get the full data row
                DataTable dt = (DataTable)dgvCashAdvances.DataSource;
                int rowIndex = selectedRow.Index;
                SelectedCashAdvance = dt.Rows[rowIndex];
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a cash advance record.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
