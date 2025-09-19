using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmAdjustTime : Form
    {
        // DatabaseManager is static, no instance needed
        
        // Form Controls
        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelMain;
        private Label lblEmployee;
        private ComboBox cboEmployee;
        private Label lblDate;
        private DateTimePicker dtpDate;
        private Label lblTimeIn;
        private DateTimePicker dtpTimeIn;
        private Label lblTimeOut;
        private DateTimePicker dtpTimeOut;
        private Label lblReason;
        private TextBox txtReason;
        private Button btnSave;
        private Button btnCancel;
        private Button btnClose;
        private DataGridView dgvAdjustments;
        
        public frmAdjustTime()
        {
            InitializeComponent();
            LoadEmployees();
            LoadAdjustments();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Time Adjustment";
            this.Size = new Size(800, 600);
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
                Text = "Time Adjustment",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            
            panelHeader.Controls.Add(lblTitle);
            
            // Main Panel
            panelMain = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(740, 200),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            // Employee
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
                Size = new Size(250, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            // Date
            lblDate = new Label
            {
                Text = "Date:",
                Location = new Point(380, 20),
                Size = new Size(50, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            dtpDate = new DateTimePicker
            {
                Location = new Point(440, 20),
                Size = new Size(150, 23),
                Format = DateTimePickerFormat.Short
            };
            
            // Time In
            lblTimeIn = new Label
            {
                Text = "Time In:",
                Location = new Point(20, 60),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            dtpTimeIn = new DateTimePicker
            {
                Location = new Point(110, 60),
                Size = new Size(150, 23),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true
            };
            
            // Time Out
            lblTimeOut = new Label
            {
                Text = "Time Out:",
                Location = new Point(280, 60),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            dtpTimeOut = new DateTimePicker
            {
                Location = new Point(370, 60),
                Size = new Size(150, 23),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true
            };
            
            // Reason
            lblReason = new Label
            {
                Text = "Reason:",
                Location = new Point(20, 100),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            txtReason = new TextBox
            {
                Location = new Point(110, 100),
                Size = new Size(480, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            
            // Buttons
            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(400, 170),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;
            
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(490, 170),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += BtnCancel_Click;
            
            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(580, 170),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += BtnClose_Click;
            
            panelMain.Controls.AddRange(new Control[] {
                lblEmployee, cboEmployee, lblDate, dtpDate,
                lblTimeIn, dtpTimeIn, lblTimeOut, dtpTimeOut,
                lblReason, txtReason, btnSave, btnCancel, btnClose
            });
            
            // DataGridView for adjustments
            dgvAdjustments = new DataGridView
            {
                Location = new Point(20, 300),
                Size = new Size(740, 250),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            
            this.Controls.AddRange(new Control[] {
                panelHeader, panelMain, dgvAdjustments
            });
        }
        
        private void LoadEmployees()
        {
            try
            {
                string query = "SELECT employee_id as emp_id, CONCAT(first_name, ' ', last_name) as fullname FROM employees WHERE employment_status = 'Active' ORDER BY last_name";
                DataTable dt = DatabaseManager.GetDataTable(query);
                
                cboEmployee.DataSource = dt;
                cboEmployee.DisplayMember = "fullname";
                cboEmployee.ValueMember = "emp_id";
                cboEmployee.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadAdjustments()
        {
            try
            {
                string query = @"SELECT ta.adjustment_id, CONCAT(e.first_name, ' ', e.last_name) as employee_name, 
                               ta.adjustment_date, ta.original_time_in, ta.original_time_out, 
                               ta.adjusted_time_in, ta.adjusted_time_out, ta.reason, ta.created_date
                               FROM time_adjustments ta
                               INNER JOIN employees e ON ta.employee_id = e.employee_id
                               ORDER BY ta.created_date DESC";
                
                DataTable dt = DatabaseManager.GetDataTable(query);
                dgvAdjustments.DataSource = dt;
                
                // Format columns
                if (dgvAdjustments.Columns.Count > 0)
                {
                    dgvAdjustments.Columns["adjustment_id"].HeaderText = "ID";
                    dgvAdjustments.Columns["employee_name"].HeaderText = "Employee";
                    dgvAdjustments.Columns["adjustment_date"].HeaderText = "Date";
                    dgvAdjustments.Columns["original_time_in"].HeaderText = "Original Time In";
                    dgvAdjustments.Columns["original_time_out"].HeaderText = "Original Time Out";
                    dgvAdjustments.Columns["adjusted_time_in"].HeaderText = "Adjusted Time In";
                    dgvAdjustments.Columns["adjusted_time_out"].HeaderText = "Adjusted Time Out";
                    dgvAdjustments.Columns["reason"].HeaderText = "Reason";
                    dgvAdjustments.Columns["created_date"].HeaderText = "Created Date";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading adjustments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SaveAdjustment();
            }
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private bool ValidateInput()
        {
            try
            {
                if (cboEmployee.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select an employee.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboEmployee.Focus();
                    return false;
                }

                if (dtpDate.Value.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("Cannot adjust time for future dates.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpDate.Focus();
                    return false;
                }

                if (dtpDate.Value.Date < DateTime.Now.Date.AddMonths(-3))
                {
                    MessageBox.Show("Cannot adjust time for dates older than 3 months.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpDate.Focus();
                    return false;
                }

                TimeSpan workHours = dtpTimeOut.Value - dtpTimeIn.Value;
                if (workHours.TotalHours > 16)
                {
                    MessageBox.Show("Total work hours cannot exceed 16 hours per day.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpTimeOut.Focus();
                    return false;
                }

                if (dtpTimeOut.Value <= dtpTimeIn.Value)
                {
                    MessageBox.Show("Time out must be later than time in.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpTimeOut.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtReason.Text))
                {
                    MessageBox.Show("Please enter a reason for the adjustment.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtReason.Focus();
                    return false;
                }

                if (txtReason.Text.Trim().Length < 10)
                {
                    MessageBox.Show("Please provide a detailed reason for the adjustment (minimum 10 characters).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtReason.Focus();
                    return false;
                }

                // Check for existing adjustment
                string checkQuery = @"SELECT COUNT(*) FROM time_adjustments 
                                    WHERE employee_id = @employee_id 
                                    AND DATE(adjustment_date) = @date";
                var parameters = new Dictionary<string, object>
                {
                    { "@employee_id", cboEmployee.SelectedValue },
                    { "@date", dtpDate.Value.Date }
                };
                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int existingCount = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkQuery, mysqlParams));

                if (existingCount > 0)
                {
                    MessageBox.Show("An adjustment already exists for this employee on the selected date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpDate.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating input: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        private void SaveAdjustment()
        {
            try
            {
                // Get original DTR record
                string getOriginalQuery = @"SELECT time_in, time_out FROM dtr 
                                           WHERE employee_id = @employee_id AND DATE(dtr_date) = @date";
                
                var parameters = new Dictionary<string, object>
                {
                    { "@employee_id", cboEmployee.SelectedValue },
                    { "@date", dtpDate.Value.Date }
                };
                
                MySqlParameter[] paramArray = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                DataTable originalDtr = DatabaseManager.GetDataTable(getOriginalQuery, paramArray);
                
                DateTime originalTimeIn = DateTime.MinValue;
                DateTime originalTimeOut = DateTime.MinValue;
                
                if (originalDtr.Rows.Count > 0)
                {
                    originalTimeIn = Convert.ToDateTime(originalDtr.Rows[0]["time_in"]);
                    originalTimeOut = Convert.ToDateTime(originalDtr.Rows[0]["time_out"]);
                }
                
                // Insert adjustment record
                string insertQuery = @"INSERT INTO time_adjustments 
                                     (employee_id, adjustment_date, original_time_in, original_time_out, 
                                      adjusted_time_in, adjusted_time_out, reason, created_date, created_by)
                                     VALUES (@employee_id, @adjustment_date, @original_time_in, @original_time_out,
                                             @adjusted_time_in, @adjusted_time_out, @reason, NOW(), @created_by)";
                
                var adjustmentParams = new Dictionary<string, object>
                {
                    { "@employee_id", cboEmployee.SelectedValue },
                    { "@adjustment_date", dtpDate.Value.Date },
                    { "@original_time_in", originalTimeIn },
                    { "@original_time_out", originalTimeOut },
                    { "@adjusted_time_in", dtpTimeIn.Value },
                    { "@adjusted_time_out", dtpTimeOut.Value },
                    { "@reason", txtReason.Text.Trim() },
                    { "@created_by", "admin" } // Replace with actual user
                };
                
                MySqlParameter[] adjustmentParamArray = adjustmentParams.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                DatabaseManager.ExecuteNonQuery(insertQuery, adjustmentParamArray);
                
                // Update DTR record
                string updateDtrQuery = @"UPDATE dtr SET time_in = @time_in, time_out = @time_out, 
                                         total_hours = TIMESTAMPDIFF(HOUR, @time_in, @time_out)
                                         WHERE employee_id = @employee_id AND DATE(dtr_date) = @date";
                
                var updateParams = new Dictionary<string, object>
                {
                    { "@time_in", dtpTimeIn.Value },
                    { "@time_out", dtpTimeOut.Value },
                    { "@employee_id", cboEmployee.SelectedValue },
                    { "@date", dtpDate.Value.Date }
                };
                
                MySqlParameter[] updateParamArray = updateParams.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                DatabaseManager.ExecuteNonQuery(updateDtrQuery, updateParamArray);
                
                MessageBox.Show("Time adjustment saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ClearForm();
                LoadAdjustments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving adjustment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ClearForm()
        {
            cboEmployee.SelectedIndex = -1;
            dtpDate.Value = DateTime.Now;
            dtpTimeIn.Value = DateTime.Now;
            dtpTimeOut.Value = DateTime.Now;
            txtReason.Clear();
        }
    }
}
