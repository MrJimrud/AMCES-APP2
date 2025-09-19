using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PayrollSystem
{
    public partial class frmReports : Form
    {
        // DatabaseManager is static - no instance needed

        // Form Controls
        private Panel panelHeader;
        private Label lblTitle;
        private TabControl tabControl;
        private TabPage tabPayroll;
        private TabPage tabDTR;
        private TabPage tabCashAdvance;

        // Payroll Report Controls
        private Panel panelPayrollFilters;
        private Label lblPayrollPeriod;
        private ComboBox cboPayrollPeriod;
        private Label lblPayrollEmployee;
        private ComboBox cboPayrollEmployee;
        private Label lblPayrollDepartment;
        private ComboBox cboPayrollDepartment;
        private Button btnGeneratePayroll;
        private Button btnExportPayroll;
        private DataGridView dgvPayrollReport;

        // DTR Report Controls
        private Panel panelDTRFilters;
        private Label lblDTREmployee;
        private ComboBox cboDTREmployee;
        private Label lblDTRDepartment;
        private ComboBox cboDTRDepartment;
        private Label lblDTRDateFrom;
        private DateTimePicker dtpDTRFrom;
        private Label lblDTRDateTo;
        private DateTimePicker dtpDTRTo;
        private Button btnGenerateDTR;
        private Button btnExportDTR;
        private DataGridView dgvDTRReport;

        // Cash Advance Report Controls
        private Panel panelCashAdvanceFilters;
        private Label lblCAEmployee;
        private ComboBox cboCAEmployee;
        private Label lblCAStatus;
        private ComboBox cboCAStatus;
        private Label lblCADateFrom;
        private DateTimePicker dtpCAFrom;
        private Label lblCADateTo;
        private DateTimePicker dtpCATo;
        private Button btnGenerateCA;
        private Button btnExportCA;
        private DataGridView dgvCAReport;

        public frmReports()
        {
            InitializeComponent();
            LoadComboBoxData();
            SetDefaultDates();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1200, 800);
            this.Text = "Reports";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1000, 600);

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
                Text = "Reports",
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
                Size = new Size(1140, 680),
                Font = new Font("Segoe UI", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(tabControl);

            // Payroll Report Tab
            tabPayroll = new TabPage
            {
                Text = "Payroll Report",
                BackColor = Color.White
            };
            tabControl.TabPages.Add(tabPayroll);
            InitializePayrollTab();

            // DTR Report Tab
            tabDTR = new TabPage
            {
                Text = "DTR Report",
                BackColor = Color.White
            };
            tabControl.TabPages.Add(tabDTR);
            InitializeDTRTab();

            // Cash Advance Report Tab
            tabCashAdvance = new TabPage
            {
                Text = "Cash Advance Report",
                BackColor = Color.White
            };
            tabControl.TabPages.Add(tabCashAdvance);
            InitializeCashAdvanceTab();
        }

        private void InitializePayrollTab()
        {
            // Filter Panel
            panelPayrollFilters = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1080, 80),
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            tabPayroll.Controls.Add(panelPayrollFilters);

            // Payroll Period
            lblPayrollPeriod = new Label
            {
                Text = "Payroll Period:",
                Location = new Point(20, 20),
                Size = new Size(100, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelPayrollFilters.Controls.Add(lblPayrollPeriod);

            cboPayrollPeriod = new ComboBox
            {
                Location = new Point(130, 20),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelPayrollFilters.Controls.Add(cboPayrollPeriod);

            // Employee
            lblPayrollEmployee = new Label
            {
                Text = "Employee:",
                Location = new Point(350, 20),
                Size = new Size(70, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelPayrollFilters.Controls.Add(lblPayrollEmployee);

            cboPayrollEmployee = new ComboBox
            {
                Location = new Point(430, 20),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelPayrollFilters.Controls.Add(cboPayrollEmployee);

            // Department
            lblPayrollDepartment = new Label
            {
                Text = "Department:",
                Location = new Point(650, 20),
                Size = new Size(80, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelPayrollFilters.Controls.Add(lblPayrollDepartment);

            cboPayrollDepartment = new ComboBox
            {
                Location = new Point(740, 20),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelPayrollFilters.Controls.Add(cboPayrollDepartment);

            // Buttons
            btnGeneratePayroll = new Button
            {
                Text = "Generate",
                Location = new Point(800, 50),
                Size = new Size(90, 25),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGeneratePayroll.Click += BtnGeneratePayroll_Click;
            panelPayrollFilters.Controls.Add(btnGeneratePayroll);

            btnExportPayroll = new Button
            {
                Text = "Export",
                Location = new Point(900, 50),
                Size = new Size(90, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportPayroll.Click += BtnExportPayroll_Click;
            panelPayrollFilters.Controls.Add(btnExportPayroll);

            // DataGridView
            dgvPayrollReport = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(1080, 500),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            tabPayroll.Controls.Add(dgvPayrollReport);
        }

        private void InitializeDTRTab()
        {
            // Filter Panel
            panelDTRFilters = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1080, 80),
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            tabDTR.Controls.Add(panelDTRFilters);

            // Employee
            lblDTREmployee = new Label
            {
                Text = "Employee:",
                Location = new Point(20, 20),
                Size = new Size(70, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelDTRFilters.Controls.Add(lblDTREmployee);

            cboDTREmployee = new ComboBox
            {
                Location = new Point(100, 20),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelDTRFilters.Controls.Add(cboDTREmployee);

            // Department
            lblDTRDepartment = new Label
            {
                Text = "Department:",
                Location = new Point(320, 20),
                Size = new Size(80, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelDTRFilters.Controls.Add(lblDTRDepartment);

            cboDTRDepartment = new ComboBox
            {
                Location = new Point(410, 20),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelDTRFilters.Controls.Add(cboDTRDepartment);

            // Date From
            lblDTRDateFrom = new Label
            {
                Text = "From:",
                Location = new Point(580, 20),
                Size = new Size(40, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelDTRFilters.Controls.Add(lblDTRDateFrom);

            dtpDTRFrom = new DateTimePicker
            {
                Location = new Point(630, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelDTRFilters.Controls.Add(dtpDTRFrom);

            // Date To
            lblDTRDateTo = new Label
            {
                Text = "To:",
                Location = new Point(760, 20),
                Size = new Size(25, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelDTRFilters.Controls.Add(lblDTRDateTo);

            dtpDTRTo = new DateTimePicker
            {
                Location = new Point(790, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelDTRFilters.Controls.Add(dtpDTRTo);

            // Buttons
            btnGenerateDTR = new Button
            {
                Text = "Generate",
                Location = new Point(800, 50),
                Size = new Size(90, 25),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerateDTR.Click += BtnGenerateDTR_Click;
            panelDTRFilters.Controls.Add(btnGenerateDTR);

            btnExportDTR = new Button
            {
                Text = "Export",
                Location = new Point(900, 50),
                Size = new Size(90, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportDTR.Click += BtnExportDTR_Click;
            panelDTRFilters.Controls.Add(btnExportDTR);

            // DataGridView
            dgvDTRReport = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(1080, 500),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            tabDTR.Controls.Add(dgvDTRReport);
        }

        private void InitializeCashAdvanceTab()
        {
            // Filter Panel
            panelCashAdvanceFilters = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1080, 80),
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            tabCashAdvance.Controls.Add(panelCashAdvanceFilters);

            // Employee
            lblCAEmployee = new Label
            {
                Text = "Employee:",
                Location = new Point(20, 20),
                Size = new Size(70, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelCashAdvanceFilters.Controls.Add(lblCAEmployee);

            cboCAEmployee = new ComboBox
            {
                Location = new Point(100, 20),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelCashAdvanceFilters.Controls.Add(cboCAEmployee);

            // Status
            lblCAStatus = new Label
            {
                Text = "Status:",
                Location = new Point(320, 20),
                Size = new Size(50, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelCashAdvanceFilters.Controls.Add(lblCAStatus);

            cboCAStatus = new ComboBox
            {
                Location = new Point(380, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboCAStatus.Items.AddRange(new string[] { "All", "Pending", "Approved", "Rejected" });
            cboCAStatus.SelectedIndex = 0;
            panelCashAdvanceFilters.Controls.Add(cboCAStatus);

            // Date From
            lblCADateFrom = new Label
            {
                Text = "From:",
                Location = new Point(520, 20),
                Size = new Size(40, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelCashAdvanceFilters.Controls.Add(lblCADateFrom);

            dtpCAFrom = new DateTimePicker
            {
                Location = new Point(570, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelCashAdvanceFilters.Controls.Add(dtpCAFrom);

            // Date To
            lblCADateTo = new Label
            {
                Text = "To:",
                Location = new Point(700, 20),
                Size = new Size(25, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelCashAdvanceFilters.Controls.Add(lblCADateTo);

            dtpCATo = new DateTimePicker
            {
                Location = new Point(730, 20),
                Size = new Size(120, 23),
                Font = new Font("Segoe UI", 9)
            };
            panelCashAdvanceFilters.Controls.Add(dtpCATo);

            // Buttons
            btnGenerateCA = new Button
            {
                Text = "Generate",
                Location = new Point(800, 50),
                Size = new Size(90, 25),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerateCA.Click += BtnGenerateCA_Click;
            panelCashAdvanceFilters.Controls.Add(btnGenerateCA);

            btnExportCA = new Button
            {
                Text = "Export",
                Location = new Point(900, 50),
                Size = new Size(90, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportCA.Click += BtnExportCA_Click;
            panelCashAdvanceFilters.Controls.Add(btnExportCA);

            // DataGridView
            dgvCAReport = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(1080, 500),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            tabCashAdvance.Controls.Add(dgvCAReport);
        }

        private void LoadComboBoxData()
        {
            try
            {
                LoadEmployees();
                LoadDepartments();
                LoadPayrollPeriods();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEmployees()
        {
            string query = @"
                SELECT employee_id, CONCAT(first_name, ' ', last_name) as full_name 
                FROM employees 
                WHERE employment_status = 'Active' 
                ORDER BY first_name, last_name";
            DataTable dt = DatabaseManager.GetDataTable(query);

            // Add "All Employees" option
            DataRow allRow = dt.NewRow();
            allRow["employee_id"] = 0;
            allRow["full_name"] = "All Employees";
            dt.Rows.InsertAt(allRow, 0);

            // Payroll Employee ComboBox
            cboPayrollEmployee.DisplayMember = "full_name";
            cboPayrollEmployee.ValueMember = "employee_id";
            cboPayrollEmployee.DataSource = dt.Copy();
            cboPayrollEmployee.SelectedIndex = 0;

            // DTR Employee ComboBox
            cboDTREmployee.DisplayMember = "full_name";
            cboDTREmployee.ValueMember = "employee_id";
            cboDTREmployee.DataSource = dt.Copy();
            cboDTREmployee.SelectedIndex = 0;

            // Cash Advance Employee ComboBox
            cboCAEmployee.DisplayMember = "full_name";
            cboCAEmployee.ValueMember = "employee_id";
            cboCAEmployee.DataSource = dt.Copy();
            cboCAEmployee.SelectedIndex = 0;
        }

        private void LoadDepartments()
        {
            string query = "SELECT department_id, department_name FROM departments WHERE is_active = 1 ORDER BY department_name";
            DataTable dt = DatabaseManager.GetDataTable(query);

            // Add "All Departments" option
            DataRow allRow = dt.NewRow();
            allRow["department_id"] = 0;
            allRow["department_name"] = "All Departments";
            dt.Rows.InsertAt(allRow, 0);

            // Payroll Department ComboBox
            cboPayrollDepartment.DisplayMember = "department_name";
            cboPayrollDepartment.ValueMember = "department_id";
            cboPayrollDepartment.DataSource = dt.Copy();
            cboPayrollDepartment.SelectedIndex = 0;

            // DTR Department ComboBox
            cboDTRDepartment.DisplayMember = "department_name";
            cboDTRDepartment.ValueMember = "department_id";
            cboDTRDepartment.DataSource = dt.Copy();
            cboDTRDepartment.SelectedIndex = 0;
        }

        private void LoadPayrollPeriods()
        {
            string query = @"
                SELECT period_id, 
                       CONCAT(DATE_FORMAT(start_date, '%M %d, %Y'), ' - ', DATE_FORMAT(end_date, '%M %d, %Y')) as period_display
                FROM payroll_periods 
                ORDER BY start_date DESC
                LIMIT 12";
            DataTable dt = DatabaseManager.GetDataTable(query);

            cboPayrollPeriod.DisplayMember = "period_display";
            cboPayrollPeriod.ValueMember = "period_id";
            cboPayrollPeriod.DataSource = dt;
            if (cboPayrollPeriod.Items.Count > 0)
                cboPayrollPeriod.SelectedIndex = 0;
        }

        private void SetDefaultDates()
        {
            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // DTR dates - current month
            dtpDTRFrom.Value = firstDayOfMonth;
            dtpDTRTo.Value = lastDayOfMonth;

            // Cash Advance dates - last 3 months
            dtpCAFrom.Value = firstDayOfMonth.AddMonths(-3);
            dtpCATo.Value = lastDayOfMonth;
        }

        private void BtnGeneratePayroll_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboPayrollPeriod.SelectedValue == null)
                {
                    MessageBox.Show("Please select a payroll period.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string query = @"
                    SELECT 
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        d.department_name as 'Department',
                        jt.job_title as 'Job Title',
                        CONCAT('₱', FORMAT(pd.basic_salary, 2)) as 'Basic Salary',
                        CONCAT('₱', FORMAT(pd.overtime_pay, 2)) as 'Overtime Pay',
                        CONCAT('₱', FORMAT(pd.allowances, 2)) as 'Allowances',
                        CONCAT('₱', FORMAT(pd.gross_pay, 2)) as 'Gross Pay',
                        CONCAT('₱', FORMAT(pd.sss_deduction, 2)) as 'SSS',
                        CONCAT('₱', FORMAT(pd.philhealth_deduction, 2)) as 'PhilHealth',
                        CONCAT('₱', FORMAT(pd.pagibig_deduction, 2)) as 'Pag-IBIG',
                        CONCAT('₱', FORMAT(pd.tax_deduction, 2)) as 'Tax',
                        CONCAT('₱', FORMAT(pd.cash_advance_deduction, 2)) as 'Cash Advance',
                        CONCAT('₱', FORMAT(pd.other_deductions, 2)) as 'Other Deductions',
                        CONCAT('₱', FORMAT(pd.total_deductions, 2)) as 'Total Deductions',
                        CONCAT('₱', FORMAT(pd.net_pay, 2)) as 'Net Pay'
                    FROM payroll_details pd
                    INNER JOIN employees e ON pd.employee_id = e.employee_id
                    INNER JOIN departments d ON e.department_id = d.department_id
                    INNER JOIN job_titles jt ON e.job_title_id = jt.job_title_id
                    WHERE pd.period_id = @period_id";

                var parameters = new Dictionary<string, object>
                {
                    { "@period_id", cboPayrollPeriod.SelectedValue }
                };

                // Add employee filter if specific employee selected
                int selectedEmployeeId = Convert.ToInt32(cboPayrollEmployee.SelectedValue ?? 0);
                if (selectedEmployeeId > 0)
                {
                    query += " AND e.employee_id = @employee_id";
                    parameters.Add("@employee_id", selectedEmployeeId);
                }

                // Add department filter if specific department selected
                int selectedDepartmentId = Convert.ToInt32(cboPayrollDepartment.SelectedValue ?? 0);
                if (selectedDepartmentId > 0)
                {
                    query += " AND e.department_id = @department_id";
                    parameters.Add("@department_id", selectedDepartmentId);
                }

                query += " ORDER BY e.first_name, e.last_name";

                DataTable dt = DatabaseManager.GetDataTable(query, parameters);
                dgvPayrollReport.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No payroll data found for the selected criteria.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating payroll report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenerateDTR_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
                    SELECT 
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        d.department_name as 'Department',
                        DATE_FORMAT(dtr.date, '%Y-%m-%d') as 'Date',
                        TIME_FORMAT(dtr.time_in, '%h:%i %p') as 'Time In',
                        TIME_FORMAT(dtr.break_out, '%h:%i %p') as 'Break Out',
                        TIME_FORMAT(dtr.break_in, '%h:%i %p') as 'Break In',
                        TIME_FORMAT(dtr.time_out, '%h:%i %p') as 'Time Out',
                        CONCAT(dtr.total_hours, ' hrs') as 'Total Hours',
                        CONCAT(dtr.overtime_hours, ' hrs') as 'Overtime Hours',
                        dtr.status as 'Status'
                    FROM dtr_records dtr
                    INNER JOIN employees e ON dtr.employee_id = e.employee_id
                    INNER JOIN departments d ON e.department_id = d.department_id
                    WHERE dtr.date >= @from_date AND dtr.date <= @to_date";

                var parameters = new Dictionary<string, object>
                {
                    { "@from_date", dtpDTRFrom.Value.Date },
                    { "@to_date", dtpDTRTo.Value.Date }
                };

                // Add employee filter if specific employee selected
                int selectedEmployeeId = Convert.ToInt32(cboDTREmployee.SelectedValue ?? 0);
                if (selectedEmployeeId > 0)
                {
                    query += " AND e.employee_id = @employee_id";
                    parameters.Add("@employee_id", selectedEmployeeId);
                }

                // Add department filter if specific department selected
                int selectedDepartmentId = Convert.ToInt32(cboDTRDepartment.SelectedValue ?? 0);
                if (selectedDepartmentId > 0)
                {
                    query += " AND e.department_id = @department_id";
                    parameters.Add("@department_id", selectedDepartmentId);
                }

                query += " ORDER BY dtr.date DESC, e.first_name, e.last_name";

                DataTable dt = DatabaseManager.GetDataTable(query, parameters);
                dgvDTRReport.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No DTR data found for the selected criteria.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating DTR report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenerateCA_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
                    SELECT 
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        d.department_name as 'Department',
                        CONCAT('₱', FORMAT(ca.amount, 2)) as 'Amount',
                        ca.reason as 'Reason',
                        DATE_FORMAT(ca.request_date, '%Y-%m-%d') as 'Request Date',
                        ca.status as 'Status',
                        CASE 
                            WHEN ca.status IN ('Approved', 'Rejected') THEN DATE_FORMAT(ca.approval_date, '%Y-%m-%d')
                            ELSE 'N/A'
                        END as 'Action Date',
                        ca.deduction_months as 'Months',
                        CONCAT('₱', FORMAT(ca.monthly_deduction, 2)) as 'Monthly Deduction',
                        COALESCE(ca.remarks, '') as 'Remarks'
                    FROM cash_advances ca
                    INNER JOIN employees e ON ca.employee_id = e.employee_id
                    INNER JOIN departments d ON e.department_id = d.department_id
                    WHERE ca.request_date >= @from_date AND ca.request_date <= @to_date";

                var parameters = new Dictionary<string, object>
                {
                    { "@from_date", dtpCAFrom.Value.Date },
                    { "@to_date", dtpCATo.Value.Date }
                };

                // Add employee filter if specific employee selected
                int selectedEmployeeId = Convert.ToInt32(cboCAEmployee.SelectedValue ?? 0);
                if (selectedEmployeeId > 0)
                {
                    query += " AND e.employee_id = @employee_id";
                    parameters.Add("@employee_id", selectedEmployeeId);
                }

                // Add status filter if specific status selected
                if (cboCAStatus.SelectedIndex > 0)
                {
                    query += " AND ca.status = @status";
                    parameters.Add("@status", cboCAStatus.Text);
                }

                query += " ORDER BY ca.request_date DESC, e.first_name, e.last_name";

                DataTable dt = DatabaseManager.GetDataTable(query, parameters);
                dgvCAReport.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No cash advance data found for the selected criteria.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating cash advance report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportPayroll_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvPayrollReport, "Payroll_Report");
        }

        private void BtnExportDTR_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvDTRReport, "DTR_Report");
        }

        private void BtnExportCA_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvCAReport, "CashAdvance_Report");
        }

        private void ExportToCSV(DataGridView dgv, string reportName)
        {
            try
            {
                if (dgv.DataSource == null || dgv.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export. Please generate a report first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FilterIndex = 1,
                    FileName = $"{reportName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder csv = new StringBuilder();

                    // Add headers
                    List<string> headers = new List<string>();
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Visible)
                            headers.Add($"\"{column.HeaderText}\"");
                    }
                    csv.AppendLine(string.Join(",", headers));

                    // Add data rows
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            List<string> values = new List<string>();
                            foreach (DataGridViewColumn column in dgv.Columns)
                            {
                                if (column.Visible)
                                {
                                    string value = row.Cells[column.Index].Value?.ToString() ?? "";
                                    values.Add($"\"{value}\"");
                                }
                            }
                            csv.AppendLine(string.Join(",", values));
                        }
                    }

                    File.WriteAllText(saveDialog.FileName, csv.ToString(), Encoding.UTF8);
                    MessageBox.Show($"Report exported successfully to:\n{saveDialog.FileName}", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
