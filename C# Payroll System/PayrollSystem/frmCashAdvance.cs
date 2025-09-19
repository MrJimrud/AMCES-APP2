using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmCashAdvance : Form
    {
        // DatabaseManager is static, no instance needed
        private int currentCashAdvanceId = 0;

        // Form Controls
        private Panel panelHeader;
        private Label lblTitle;
        private TabControl tabControl;
        private TabPage tabRequest;
        private TabPage tabApproval;
        private TabPage tabHistory;

        // Request Tab Controls
        private Panel panelRequestForm;
        private Label lblEmployee;
        private ComboBox cboEmployee;
        private Label lblAmount;
        private NumericUpDown nudAmount;
        private Label lblReason;
        private TextBox txtReason;
        private Label lblRequestDate;
        private DateTimePicker dtpRequestDate;
        private Label lblDeductionMonths;
        private NumericUpDown nudDeductionMonths;
        private Label lblMonthlyDeduction;
        private Label lblMonthlyDeductionAmount;
        private Panel panelRequestButtons;
        private Button btnSubmitRequest;
        private Button btnClearRequest;

        // Approval Tab Controls
        private Panel panelApprovalList;
        private DataGridView dgvPendingRequests;
        private Panel panelApprovalForm;
        private Label lblApprovalEmployee;
        private TextBox txtApprovalEmployee;
        private Label lblApprovalAmount;
        private TextBox txtApprovalAmount;
        private Label lblApprovalReason;
        private TextBox txtApprovalReason;
        private Label lblApprovalDate;
        private DateTimePicker dtpApprovalDate;
        private Label lblApprovalStatus;
        private ComboBox cboApprovalStatus;
        private Label lblApprovalRemarks;
        private TextBox txtApprovalRemarks;
        private Panel panelApprovalButtons;
        private Button btnApprove;
        private Button btnReject;
        private Button btnRefreshApproval;

        // History Tab Controls
        private Panel panelHistoryFilter;
        private Label lblHistoryEmployee;
        private ComboBox cboHistoryEmployee;
        private Label lblHistoryStatus;
        private ComboBox cboHistoryStatus;
        private Label lblHistoryDateFrom;
        private DateTimePicker dtpHistoryFrom;
        private Label lblHistoryDateTo;
        private DateTimePicker dtpHistoryTo;
        private Button btnFilterHistory;
        private Button btnRefreshHistory;
        private DataGridView dgvHistory;

        public frmCashAdvance()
        {
            InitializeComponent();
            LoadEmployees();
            LoadPendingRequests();
            LoadHistory();
            SetupEventHandlers();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 700);
            this.Text = "Cash Advance Management";
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
                Text = "Cash Advance Management",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            panelHeader.Controls.Add(lblTitle);

            // Tab Control
            tabControl = new TabControl
            {
                Location = new Point(20, 80),
                Size = new Size(940, 580),
                Font = new Font("Segoe UI", 9)
            };
            this.Controls.Add(tabControl);

            // Request Tab
            tabRequest = new TabPage
            {
                Text = "New Request",
                BackColor = Color.White
            };
            tabControl.TabPages.Add(tabRequest);

            InitializeRequestTab();

            // Approval Tab
            tabApproval = new TabPage
            {
                Text = "Pending Approvals",
                BackColor = Color.White
            };
            tabControl.TabPages.Add(tabApproval);

            InitializeApprovalTab();

            // History Tab
            tabHistory = new TabPage
            {
                Text = "History",
                BackColor = Color.White
            };
            tabControl.TabPages.Add(tabHistory);

            InitializeHistoryTab();
        }

        private void InitializeRequestTab()
        {
            panelRequestForm = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(500, 400),
                BorderStyle = BorderStyle.FixedSingle
            };
            tabRequest.Controls.Add(panelRequestForm);

            // Employee
            lblEmployee = new Label
            {
                Text = "Employee:",
                Location = new Point(20, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelRequestForm.Controls.Add(lblEmployee);

            cboEmployee = new ComboBox
            {
                Location = new Point(150, 20),
                Size = new Size(320, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelRequestForm.Controls.Add(cboEmployee);

            // Amount
            lblAmount = new Label
            {
                Text = "Amount:",
                Location = new Point(20, 60),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelRequestForm.Controls.Add(lblAmount);

            nudAmount = new NumericUpDown
            {
                Location = new Point(150, 60),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 9),
                DecimalPlaces = 2,
                Maximum = 999999,
                Minimum = 100,
                ThousandsSeparator = true
            };
            nudAmount.ValueChanged += NudAmount_ValueChanged;
            panelRequestForm.Controls.Add(nudAmount);

            // Deduction Months
            lblDeductionMonths = new Label
            {
                Text = "Deduction Months:",
                Location = new Point(320, 60),
                Size = new Size(100, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelRequestForm.Controls.Add(lblDeductionMonths);

            nudDeductionMonths = new NumericUpDown
            {
                Location = new Point(430, 60),
                Size = new Size(60, 23),
                Font = new Font("Segoe UI", 9),
                Minimum = 1,
                Maximum = 12,
                Value = 3
            };
            nudDeductionMonths.ValueChanged += NudDeductionMonths_ValueChanged;
            panelRequestForm.Controls.Add(nudDeductionMonths);

            // Monthly Deduction Display
            lblMonthlyDeduction = new Label
            {
                Text = "Monthly Deduction:",
                Location = new Point(20, 100),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelRequestForm.Controls.Add(lblMonthlyDeduction);

            lblMonthlyDeductionAmount = new Label
            {
                Text = "₱0.00",
                Location = new Point(150, 100),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 76, 60)
            };
            panelRequestForm.Controls.Add(lblMonthlyDeductionAmount);

            // Request Date
            lblRequestDate = new Label
            {
                Text = "Request Date:",
                Location = new Point(20, 140),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelRequestForm.Controls.Add(lblRequestDate);

            dtpRequestDate = new DateTimePicker
            {
                Location = new Point(150, 140),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9),
                Value = DateTime.Now
            };
            panelRequestForm.Controls.Add(dtpRequestDate);

            // Reason
            lblReason = new Label
            {
                Text = "Reason:",
                Location = new Point(20, 180),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelRequestForm.Controls.Add(lblReason);

            txtReason = new TextBox
            {
                Location = new Point(150, 180),
                Size = new Size(320, 80),
                Font = new Font("Segoe UI", 9),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            panelRequestForm.Controls.Add(txtReason);

            // Request Buttons
            panelRequestButtons = new Panel
            {
                Location = new Point(20, 280),
                Size = new Size(450, 50)
            };
            panelRequestForm.Controls.Add(panelRequestButtons);

            btnSubmitRequest = new Button
            {
                Text = "Submit Request",
                Location = new Point(0, 10),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSubmitRequest.Click += BtnSubmitRequest_Click;
            panelRequestButtons.Controls.Add(btnSubmitRequest);

            btnClearRequest = new Button
            {
                Text = "Clear",
                Location = new Point(130, 10),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClearRequest.Click += BtnClearRequest_Click;
            panelRequestButtons.Controls.Add(btnClearRequest);
        }

        private void InitializeApprovalTab()
        {
            // Pending Requests List
            panelApprovalList = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(880, 250),
                BorderStyle = BorderStyle.FixedSingle
            };
            tabApproval.Controls.Add(panelApprovalList);

            Label lblPendingTitle = new Label
            {
                Text = "Pending Cash Advance Requests",
                Location = new Point(10, 10),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panelApprovalList.Controls.Add(lblPendingTitle);

            dgvPendingRequests = new DataGridView
            {
                Location = new Point(10, 40),
                Size = new Size(860, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false
            };
            dgvPendingRequests.SelectionChanged += DgvPendingRequests_SelectionChanged;
            panelApprovalList.Controls.Add(dgvPendingRequests);

            // Approval Form
            panelApprovalForm = new Panel
            {
                Location = new Point(20, 290),
                Size = new Size(880, 220),
                BorderStyle = BorderStyle.FixedSingle
            };
            tabApproval.Controls.Add(panelApprovalForm);

            Label lblApprovalTitle = new Label
            {
                Text = "Approval Details",
                Location = new Point(10, 10),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panelApprovalForm.Controls.Add(lblApprovalTitle);

            // Employee (Read-only)
            lblApprovalEmployee = new Label
            {
                Text = "Employee:",
                Location = new Point(20, 40),
                Size = new Size(80, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelApprovalForm.Controls.Add(lblApprovalEmployee);

            txtApprovalEmployee = new TextBox
            {
                Location = new Point(110, 40),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9),
                ReadOnly = true
            };
            panelApprovalForm.Controls.Add(txtApprovalEmployee);

            // Amount (Read-only)
            lblApprovalAmount = new Label
            {
                Text = "Amount:",
                Location = new Point(330, 40),
                Size = new Size(60, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelApprovalForm.Controls.Add(lblApprovalAmount);

            txtApprovalAmount = new TextBox
            {
                Location = new Point(400, 40),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9),
                ReadOnly = true
            };
            panelApprovalForm.Controls.Add(txtApprovalAmount);

            // Reason (Read-only)
            lblApprovalReason = new Label
            {
                Text = "Reason:",
                Location = new Point(20, 80),
                Size = new Size(80, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelApprovalForm.Controls.Add(lblApprovalReason);

            txtApprovalReason = new TextBox
            {
                Location = new Point(110, 80),
                Size = new Size(410, 40),
                Font = new Font("Segoe UI", 9),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            panelApprovalForm.Controls.Add(txtApprovalReason);

            // Approval Date
            lblApprovalDate = new Label
            {
                Text = "Approval Date:",
                Location = new Point(540, 40),
                Size = new Size(90, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelApprovalForm.Controls.Add(lblApprovalDate);

            dtpApprovalDate = new DateTimePicker
            {
                Location = new Point(640, 40),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 9),
                Value = DateTime.Now
            };
            panelApprovalForm.Controls.Add(dtpApprovalDate);

            // Status
            lblApprovalStatus = new Label
            {
                Text = "Status:",
                Location = new Point(540, 80),
                Size = new Size(60, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelApprovalForm.Controls.Add(lblApprovalStatus);

            cboApprovalStatus = new ComboBox
            {
                Location = new Point(640, 80),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboApprovalStatus.Items.AddRange(new string[] { "Approved", "Rejected" });
            panelApprovalForm.Controls.Add(cboApprovalStatus);

            // Remarks
            lblApprovalRemarks = new Label
            {
                Text = "Remarks:",
                Location = new Point(20, 140),
                Size = new Size(80, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelApprovalForm.Controls.Add(lblApprovalRemarks);

            txtApprovalRemarks = new TextBox
            {
                Location = new Point(110, 140),
                Size = new Size(680, 40),
                Font = new Font("Segoe UI", 9),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            panelApprovalForm.Controls.Add(txtApprovalRemarks);

            // Approval Buttons
            panelApprovalButtons = new Panel
            {
                Location = new Point(540, 190),
                Size = new Size(320, 40)
            };
            panelApprovalForm.Controls.Add(panelApprovalButtons);

            btnApprove = new Button
            {
                Text = "Approve",
                Location = new Point(0, 5),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnApprove.Click += BtnApprove_Click;
            panelApprovalButtons.Controls.Add(btnApprove);

            btnReject = new Button
            {
                Text = "Reject",
                Location = new Point(90, 5),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnReject.Click += BtnReject_Click;
            panelApprovalButtons.Controls.Add(btnReject);

            btnRefreshApproval = new Button
            {
                Text = "Refresh",
                Location = new Point(180, 5),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshApproval.Click += BtnRefreshApproval_Click;
            panelApprovalButtons.Controls.Add(btnRefreshApproval);
        }

        private void InitializeHistoryTab()
        {
            // Filter Panel
            panelHistoryFilter = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(880, 80),
                BorderStyle = BorderStyle.FixedSingle
            };
            tabHistory.Controls.Add(panelHistoryFilter);

            // Employee Filter
            lblHistoryEmployee = new Label
            {
                Text = "Employee:",
                Location = new Point(20, 20),
                Size = new Size(70, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelHistoryFilter.Controls.Add(lblHistoryEmployee);

            cboHistoryEmployee = new ComboBox
            {
                Location = new Point(100, 20),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelHistoryFilter.Controls.Add(cboHistoryEmployee);

            // Status Filter
            lblHistoryStatus = new Label
            {
                Text = "Status:",
                Location = new Point(320, 20),
                Size = new Size(50, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelHistoryFilter.Controls.Add(lblHistoryStatus);

            cboHistoryStatus = new ComboBox
            {
                Location = new Point(380, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboHistoryStatus.Items.AddRange(new string[] { "All", "Pending", "Approved", "Rejected" });
            cboHistoryStatus.SelectedIndex = 0;
            panelHistoryFilter.Controls.Add(cboHistoryStatus);

            // Date Range
            lblHistoryDateFrom = new Label
            {
                Text = "From:",
                Location = new Point(520, 20),
                Size = new Size(40, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelHistoryFilter.Controls.Add(lblHistoryDateFrom);

            dtpHistoryFrom = new DateTimePicker
            {
                Location = new Point(570, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9),
                Value = DateTime.Now.AddMonths(-3)
            };
            panelHistoryFilter.Controls.Add(dtpHistoryFrom);

            lblHistoryDateTo = new Label
            {
                Text = "To:",
                Location = new Point(700, 20),
                Size = new Size(25, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelHistoryFilter.Controls.Add(lblHistoryDateTo);

            dtpHistoryTo = new DateTimePicker
            {
                Location = new Point(730, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9),
                Value = DateTime.Now
            };
            panelHistoryFilter.Controls.Add(dtpHistoryTo);

            // Filter Buttons
            btnFilterHistory = new Button
            {
                Text = "Filter",
                Location = new Point(650, 50),
                Size = new Size(75, 25),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnFilterHistory.Click += BtnFilterHistory_Click;
            panelHistoryFilter.Controls.Add(btnFilterHistory);

            btnRefreshHistory = new Button
            {
                Text = "Refresh",
                Location = new Point(735, 50),
                Size = new Size(75, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshHistory.Click += BtnRefreshHistory_Click;
            panelHistoryFilter.Controls.Add(btnRefreshHistory);

            // History DataGridView
            dgvHistory = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(880, 390),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false
            };
            tabHistory.Controls.Add(dgvHistory);
        }

        private void SetupEventHandlers()
        {
            // Calculate monthly deduction when amount or months change
            CalculateMonthlyDeduction();
        }

        private void LoadEmployees()
        {
            try
            {
                string query = @"
                    SELECT employee_id, CONCAT(first_name, ' ', last_name) as full_name 
                    FROM employees 
                    WHERE employment_status = 'Active' 
                    ORDER BY first_name, last_name";
                DataTable dt = DatabaseManager.GetDataTable(query);

                // For request form
                cboEmployee.DisplayMember = "full_name";
                cboEmployee.ValueMember = "employee_id";
                cboEmployee.DataSource = dt.Copy();

                // For history filter
                DataTable historyDt = dt.Copy();
                DataRow allRow = historyDt.NewRow();
                allRow["employee_id"] = 0;
                allRow["full_name"] = "All Employees";
                historyDt.Rows.InsertAt(allRow, 0);

                cboHistoryEmployee.DisplayMember = "full_name";
                cboHistoryEmployee.ValueMember = "employee_id";
                cboHistoryEmployee.DataSource = historyDt;
                cboHistoryEmployee.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPendingRequests()
        {
            try
            {
                string query = @"
                    SELECT 
                        ca.advance_id as 'ID',
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee',
                        CONCAT('₱', FORMAT(ca.amount, 2)) as 'Amount',
                        ca.reason as 'Reason',
                        DATE_FORMAT(ca.request_date, '%Y-%m-%d') as 'Request Date',
                        ca.deduction_months as 'Months',
                        CONCAT('₱', FORMAT(ca.monthly_deduction, 2)) as 'Monthly Deduction'
                    FROM cash_advances ca
                    INNER JOIN employees e ON ca.employee_id = e.employee_id
                    WHERE ca.status = 'Pending'
                    ORDER BY ca.request_date";

                DataTable dt = DatabaseManager.GetDataTable(query);
                dgvPendingRequests.DataSource = dt;

                if (dgvPendingRequests.Columns["ID"] != null)
                    dgvPendingRequests.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pending requests: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadHistory()
        {
            try
            {
                string query = @"
                    SELECT 
                        ca.advance_id as 'ID',
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee',
                        CONCAT('₱', FORMAT(ca.amount, 2)) as 'Amount',
                        ca.reason as 'Reason',
                        DATE_FORMAT(ca.request_date, '%Y-%m-%d') as 'Request Date',
                        ca.status as 'Status',
                        CASE 
                            WHEN ca.status = 'Approved' THEN DATE_FORMAT(ca.approved_date, '%Y-%m-%d')
                            WHEN ca.status = 'Rejected' THEN DATE_FORMAT(ca.approved_date, '%Y-%m-%d')
                            ELSE 'N/A'
                        END as 'Action Date',
                        COALESCE(ca.remarks, '') as 'Remarks'
                    FROM cash_advances ca
                    INNER JOIN employees e ON ca.employee_id = e.employee_id
                    WHERE ca.request_date >= @from_date AND ca.request_date <= @to_date
                    ORDER BY ca.request_date DESC";

                var parameters = new Dictionary<string, object>
                {
                    { "@from_date", dtpHistoryFrom.Value.Date },
                    { "@to_date", dtpHistoryTo.Value.Date }
                };

                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                DataTable dt = DatabaseManager.GetDataTable(query, mysqlParams);
                dgvHistory.DataSource = dt;

                if (dgvHistory.Columns["ID"] != null)
                    dgvHistory.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NudAmount_ValueChanged(object sender, EventArgs e)
        {
            CalculateMonthlyDeduction();
        }

        private void NudDeductionMonths_ValueChanged(object sender, EventArgs e)
        {
            CalculateMonthlyDeduction();
        }

        private void CalculateMonthlyDeduction()
        {
            if (nudAmount.Value > 0 && nudDeductionMonths.Value > 0)
            {
                decimal monthlyDeduction = nudAmount.Value / nudDeductionMonths.Value;
                lblMonthlyDeductionAmount.Text = $"₱{monthlyDeduction:N2}";
            }
            else
            {
                lblMonthlyDeductionAmount.Text = "₱0.00";
            }
        }

        private void BtnSubmitRequest_Click(object sender, EventArgs e)
        {
            if (!ValidateRequestForm())
                return;

            try
            {
                decimal monthlyDeduction = nudAmount.Value / nudDeductionMonths.Value;

                string query = @"
                    INSERT INTO cash_advances 
                    (employee_id, amount, reason, request_date, deduction_months, monthly_deduction, status, remaining_balance, created_at, updated_at)
                    VALUES (@employee_id, @amount, @reason, @request_date, @deduction_months, @monthly_deduction, 'Pending', @amount, NOW(), NOW())";

                var parameters = new Dictionary<string, object>
                {
                    { "@employee_id", cboEmployee.SelectedValue },
                    { "@amount", nudAmount.Value },
                    { "@reason", txtReason.Text.Trim() },
                    { "@request_date", dtpRequestDate.Value.Date },
                    { "@deduction_months", nudDeductionMonths.Value },
                    { "@monthly_deduction", monthlyDeduction }
                };

                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int result = DatabaseManager.ExecuteNonQuery(query, mysqlParams);
                if (result > 0)
                {
                    MessageBox.Show("Cash advance request submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearRequestForm();
                    LoadPendingRequests();
                    LoadHistory();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting request: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClearRequest_Click(object sender, EventArgs e)
        {
            ClearRequestForm();
        }

        private void DgvPendingRequests_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPendingRequests.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPendingRequests.SelectedRows[0];
                currentCashAdvanceId = Convert.ToInt32(row.Cells["ID"].Value);
                
                txtApprovalEmployee.Text = row.Cells["Employee"].Value.ToString();
                txtApprovalAmount.Text = row.Cells["Amount"].Value.ToString();
                txtApprovalReason.Text = row.Cells["Reason"].Value.ToString();
                
                btnApprove.Enabled = true;
                btnReject.Enabled = true;
            }
            else
            {
                currentCashAdvanceId = 0;
                txtApprovalEmployee.Clear();
                txtApprovalAmount.Clear();
                txtApprovalReason.Clear();
                txtApprovalRemarks.Clear();
                
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
            }
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            ProcessApproval("Approved");
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            ProcessApproval("Rejected");
        }

        private void ProcessApproval(string status)
        {
            if (currentCashAdvanceId == 0)
            {
                MessageBox.Show("Please select a request to process.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string query = @"
                    UPDATE cash_advances SET 
                        status = @status,
                        approved_date = @approval_date,
                        remarks = @remarks,
                        updated_at = NOW()
                    WHERE advance_id = @id";

                var parameters = new Dictionary<string, object>
                {
                    { "@status", status },
                    { "@approval_date", dtpApprovalDate.Value.Date },
                    { "@remarks", txtApprovalRemarks.Text.Trim() },
                    { "@id", currentCashAdvanceId }
                };

                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int result = DatabaseManager.ExecuteNonQuery(query, mysqlParams);
                if (result > 0)
                {
                    MessageBox.Show($"Request {status.ToLower()} successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPendingRequests();
                    LoadHistory();
                    ClearApprovalForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing approval: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefreshApproval_Click(object sender, EventArgs e)
        {
            LoadPendingRequests();
            ClearApprovalForm();
        }

        private void BtnFilterHistory_Click(object sender, EventArgs e)
        {
            LoadHistory();
            ApplyHistoryFilters();
        }

        private void BtnRefreshHistory_Click(object sender, EventArgs e)
        {
            cboHistoryEmployee.SelectedIndex = 0;
            cboHistoryStatus.SelectedIndex = 0;
            dtpHistoryFrom.Value = DateTime.Now.AddMonths(-3);
            dtpHistoryTo.Value = DateTime.Now;
            LoadHistory();
        }

        private void ApplyHistoryFilters()
        {
            if (dgvHistory.DataSource is DataTable dt)
            {
                List<string> conditions = new List<string>();

                // Employee filter
                int selectedEmployeeId = Convert.ToInt32(cboHistoryEmployee.SelectedValue ?? 0);
                if (selectedEmployeeId > 0)
                {
                    conditions.Add($"[Employee] = '{cboHistoryEmployee.Text}'");
                }

                // Status filter
                if (cboHistoryStatus.SelectedIndex > 0)
                {
                    conditions.Add($"[Status] = '{cboHistoryStatus.Text}'");
                }

                string filter = conditions.Count > 0 ? string.Join(" AND ", conditions) : string.Empty;
                dt.DefaultView.RowFilter = filter;
            }
        }

        private bool ValidateRequestForm()
        {
            try
            {
                // Validate Employee Selection
                if (cboEmployee.SelectedValue == null)
                {
                    MessageBox.Show("Please select an employee.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboEmployee.Focus();
                    return false;
                }

                // Check for existing pending or approved cash advances
                string checkExistingQuery = @"SELECT COUNT(*) FROM cash_advances 
                                            WHERE employee_id = @employee_id 
                                            AND (status = 'Pending' OR (status = 'Approved' AND remaining_balance > 0))";
                var parameters = new Dictionary<string, object>
                {
                    { "@employee_id", cboEmployee.SelectedValue }
                };
                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                
                int existingCount = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkExistingQuery, mysqlParams));
                if (existingCount > 0)
                {
                    MessageBox.Show("This employee already has a pending or active cash advance.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboEmployee.Focus();
                    return false;
                }

                // Validate Amount
                if (nudAmount.Value < 100)
                {
                    MessageBox.Show("Amount must be at least ₱100.00.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nudAmount.Focus();
                    return false;
                }

                // Check if amount exceeds maximum limit (e.g., 50% of monthly salary)
                string checkSalaryQuery = "SELECT basic_salary FROM employees WHERE employee_id = @employee_id";
                decimal monthlySalary = Convert.ToDecimal(DatabaseManager.ExecuteScalar(checkSalaryQuery, mysqlParams));
                decimal maxAllowedAmount = monthlySalary * 0.5m;

                if (nudAmount.Value > maxAllowedAmount)
                {
                    MessageBox.Show($"Cash advance amount cannot exceed 50% of monthly salary (₱{maxAllowedAmount:N2}).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nudAmount.Focus();
                    return false;
                }

                // Validate Monthly Deduction
                decimal monthlyDeduction = nudAmount.Value / nudDeductionMonths.Value;
                if (monthlyDeduction > (monthlySalary * 0.2m))
                {
                    MessageBox.Show($"Monthly deduction cannot exceed 20% of monthly salary (₱{monthlySalary * 0.2m:N2}).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nudDeductionMonths.Focus();
                    return false;
                }

                // Validate Request Date
                if (dtpRequestDate.Value.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("Request date cannot be in the future.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpRequestDate.Focus();
                    return false;
                }

                if (dtpRequestDate.Value.Date < DateTime.Now.Date.AddDays(-7))
                {
                    MessageBox.Show("Request date cannot be more than 7 days old.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpRequestDate.Focus();
                    return false;
                }

                // Validate Reason
                if (string.IsNullOrWhiteSpace(txtReason.Text))
                {
                    MessageBox.Show("Please provide a reason for the cash advance.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtReason.Focus();
                    return false;
                }

                string reason = txtReason.Text.Trim();
                if (reason.Length < 10 || reason.Length > 500)
                {
                    MessageBox.Show("Reason must be between 10 and 500 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtReason.Focus();
                    return false;
                }

                // Validate Deduction Months
                if (nudDeductionMonths.Value < 1 || nudDeductionMonths.Value > 12)
                {
                    MessageBox.Show("Deduction months must be between 1 and 12.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nudDeductionMonths.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during validation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void ClearRequestForm()
        {
            if (cboEmployee.Items.Count > 0)
                cboEmployee.SelectedIndex = 0;
            nudAmount.Value = 100;
            nudDeductionMonths.Value = 3;
            txtReason.Clear();
            dtpRequestDate.Value = DateTime.Now;
            CalculateMonthlyDeduction();
        }

        private void ClearApprovalForm()
        {
            txtApprovalEmployee.Clear();
            txtApprovalAmount.Clear();
            txtApprovalReason.Clear();
            txtApprovalRemarks.Clear();
            dtpApprovalDate.Value = DateTime.Now;
            if (cboApprovalStatus.Items.Count > 0)
                cboApprovalStatus.SelectedIndex = 0;
            currentCashAdvanceId = 0;
        }
    }
}
