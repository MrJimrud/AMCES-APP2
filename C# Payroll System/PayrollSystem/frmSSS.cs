using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmSSS : Form
    {
        private TabControl tabControl;
        private TabPage tabContributionTable;
        private TabPage tabEmployeeContributions;
        private TabPage tabSettings;
        private TabPage tabReports;
        
        // Contribution Table Tab Controls
        private DataGridView dgvContributionTable;
        private Button btnAddRange;
        private Button btnEditRange;
        private Button btnDeleteRange;
        private Button btnImportTable;
        private Button btnExportTable;
        private Label lblTotalRanges;
        
        // Employee Contributions Tab Controls
        private DataGridView dgvEmployeeContributions;
        private ComboBox cmbDepartment;
        private ComboBox cmbPayPeriod;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private TextBox txtSearchEmployee;
        private Button btnCalculateContributions;
        private Button btnExportContributions;
        private Label lblTotalContributions;
        private Label lblTotalAmount;
        
        // Settings Tab Controls
        private NumericUpDown nudEmployerRate;
        private NumericUpDown nudEmployeeRate;
        private NumericUpDown nudMaxSalaryCredit;
        private NumericUpDown nudMinSalaryCredit;
        private CheckBox chkAutoCalculate;
        private CheckBox chkIncludeAllowances;
        private CheckBox chkIncludeOvertime;
        private Button btnSaveSettings;
        private Button btnResetSettings;
        private Button btnLoadDefaults;
        
        // Reports Tab Controls
        private DataGridView dgvReports;
        private ComboBox cmbReportType;
        private DateTimePicker dtpReportFrom;
        private DateTimePicker dtpReportTo;
        private Button btnGenerateReport;
        private Button btnExportReport;
        private Button btnPrintReport;

        public frmSSS()
        {
            InitializeComponent();
            LoadInitialData();
        }

        private void InitializeComponent()
        {
            this.Text = "SSS Management";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1000, 700);

            // Create tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // Create tabs
            tabContributionTable = new TabPage("Contribution Table");
            tabEmployeeContributions = new TabPage("Employee Contributions");
            tabSettings = new TabPage("Settings");
            tabReports = new TabPage("Reports");

            tabControl.TabPages.AddRange(new TabPage[] { tabContributionTable, tabEmployeeContributions, tabSettings, tabReports });

            InitializeContributionTableTab();
            InitializeEmployeeContributionsTab();
            InitializeSettingsTab();
            InitializeReportsTab();

            this.Controls.Add(tabControl);
        }

        private void InitializeContributionTableTab()
        {
            // Top panel
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblTitle = new Label
            {
                Text = "SSS Contribution Table Management",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(300, 25)
            };

            Label lblInfo = new Label
            {
                Text = "Manage SSS contribution ranges and rates",
                Location = new Point(20, 45),
                Size = new Size(300, 20),
                ForeColor = Color.Gray
            };

            btnAddRange = new Button
            {
                Text = "Add Range",
                Location = new Point(700, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddRange.Click += BtnAddRange_Click;

            btnEditRange = new Button
            {
                Text = "Edit",
                Location = new Point(790, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnEditRange.Click += BtnEditRange_Click;

            btnDeleteRange = new Button
            {
                Text = "Delete",
                Location = new Point(860, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteRange.Click += BtnDeleteRange_Click;

            btnImportTable = new Button
            {
                Text = "Import",
                Location = new Point(930, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnImportTable.Click += BtnImportTable_Click;

            btnExportTable = new Button
            {
                Text = "Export",
                Location = new Point(1000, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportTable.Click += BtnExportTable_Click;

            pnlTop.Controls.AddRange(new Control[] {
                lblTitle, lblInfo, btnAddRange, btnEditRange, btnDeleteRange, btnImportTable, btnExportTable
            });

            // DataGridView for contribution table
            dgvContributionTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvContributionTable.CellDoubleClick += DgvContributionTable_CellDoubleClick;

            // Bottom panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalRanges = new Label
            {
                Text = "Total Ranges: 0",
                Location = new Point(20, 10),
                Size = new Size(200, 23)
            };

            Button btnRefreshTable = new Button
            {
                Text = "Refresh",
                Location = new Point(700, 5),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshTable.Click += (s, e) => LoadContributionTable();

            pnlBottom.Controls.AddRange(new Control[] { lblTotalRanges, btnRefreshTable });

            tabContributionTable.Controls.AddRange(new Control[] { pnlTop, dgvContributionTable, pnlBottom });
            UtilityHelper.ApplyLightMode(dgvContributionTable);
        }

        private void InitializeEmployeeContributionsTab()
        {
            // Top panel
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblTitle = new Label
            {
                Text = "Employee SSS Contributions",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(250, 25)
            };

            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 50),
                Size = new Size(60, 23)
            };

            txtSearchEmployee = new TextBox
            {
                Location = new Point(85, 47),
                Size = new Size(150, 23)
            };
            txtSearchEmployee.SetPlaceholderText("Employee name or ID...");
            txtSearchEmployee.TextChanged += TxtSearchEmployee_TextChanged;

            Label lblDepartment = new Label
            {
                Text = "Department:",
                Location = new Point(250, 50),
                Size = new Size(80, 23)
            };

            cmbDepartment = new ComboBox
            {
                Location = new Point(335, 47),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDepartment.SelectedIndexChanged += CmbDepartment_SelectedIndexChanged;

            Label lblPayPeriod = new Label
            {
                Text = "Pay Period:",
                Location = new Point(470, 50),
                Size = new Size(80, 23)
            };

            cmbPayPeriod = new ComboBox
            {
                Location = new Point(555, 47),
                Size = new Size(100, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPayPeriod.SelectedIndexChanged += CmbPayPeriod_SelectedIndexChanged;

            Label lblFromDate = new Label
            {
                Text = "From:",
                Location = new Point(20, 85),
                Size = new Size(40, 23)
            };

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(65, 82),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };
            dtpFromDate.ValueChanged += DtpFromDate_ValueChanged;

            Label lblToDate = new Label
            {
                Text = "To:",
                Location = new Point(200, 85),
                Size = new Size(30, 23)
            };

            dtpToDate = new DateTimePicker
            {
                Location = new Point(235, 82),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };
            dtpToDate.ValueChanged += DtpToDate_ValueChanged;

            btnCalculateContributions = new Button
            {
                Text = "Calculate",
                Location = new Point(700, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCalculateContributions.Click += BtnCalculateContributions_Click;

            btnExportContributions = new Button
            {
                Text = "Export",
                Location = new Point(790, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportContributions.Click += BtnExportContributions_Click;

            pnlTop.Controls.AddRange(new Control[] {
                lblTitle, lblSearch, txtSearchEmployee, lblDepartment, cmbDepartment,
                lblPayPeriod, cmbPayPeriod, lblFromDate, dtpFromDate, lblToDate, dtpToDate,
                btnCalculateContributions, btnExportContributions
            });

            // DataGridView for employee contributions
            dgvEmployeeContributions = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Bottom panel
            Panel pnlBottomContrib = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalContributions = new Label
            {
                Text = "Total Employees: 0",
                Location = new Point(20, 10),
                Size = new Size(150, 23)
            };

            lblTotalAmount = new Label
            {
                Text = "Total Amount: ₱0.00",
                Location = new Point(180, 10),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            pnlBottomContrib.Controls.AddRange(new Control[] { lblTotalContributions, lblTotalAmount });

            tabEmployeeContributions.Controls.AddRange(new Control[] { pnlTop, dgvEmployeeContributions, pnlBottomContrib });
            UtilityHelper.ApplyLightMode(dgvEmployeeContributions);
        }

        private void InitializeSettingsTab()
        {
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "SSS Configuration Settings",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(300, 30)
            };

            // Contribution Rates Group
            GroupBox grpRates = new GroupBox
            {
                Text = "Contribution Rates",
                Location = new Point(0, 40),
                Size = new Size(400, 120)
            };

            Label lblEmployerRate = new Label
            {
                Text = "Employer Rate (%):",
                Location = new Point(20, 30),
                Size = new Size(120, 23)
            };

            nudEmployerRate = new NumericUpDown
            {
                Location = new Point(150, 27),
                Size = new Size(80, 23),
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 100,
                Value = 8.5m
            };

            Label lblEmployeeRate = new Label
            {
                Text = "Employee Rate (%):",
                Location = new Point(20, 60),
                Size = new Size(120, 23)
            };

            nudEmployeeRate = new NumericUpDown
            {
                Location = new Point(150, 57),
                Size = new Size(80, 23),
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 100,
                Value = 4.5m
            };

            grpRates.Controls.AddRange(new Control[] { lblEmployerRate, nudEmployerRate, lblEmployeeRate, nudEmployeeRate });

            // Salary Credit Limits Group
            GroupBox grpLimits = new GroupBox
            {
                Text = "Salary Credit Limits",
                Location = new Point(420, 40),
                Size = new Size(400, 120)
            };

            Label lblMaxSalary = new Label
            {
                Text = "Maximum Salary Credit:",
                Location = new Point(20, 30),
                Size = new Size(150, 23)
            };

            nudMaxSalaryCredit = new NumericUpDown
            {
                Location = new Point(180, 27),
                Size = new Size(100, 23),
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 999999,
                Value = 25000m,
                ThousandsSeparator = true
            };

            Label lblMinSalary = new Label
            {
                Text = "Minimum Salary Credit:",
                Location = new Point(20, 60),
                Size = new Size(150, 23)
            };

            nudMinSalaryCredit = new NumericUpDown
            {
                Location = new Point(180, 57),
                Size = new Size(100, 23),
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 999999,
                Value = 1000m,
                ThousandsSeparator = true
            };

            grpLimits.Controls.AddRange(new Control[] { lblMaxSalary, nudMaxSalaryCredit, lblMinSalary, nudMinSalaryCredit });

            // Calculation Options Group
            GroupBox grpOptions = new GroupBox
            {
                Text = "Calculation Options",
                Location = new Point(0, 180),
                Size = new Size(400, 150)
            };

            chkAutoCalculate = new CheckBox
            {
                Text = "Auto-calculate contributions during payroll",
                Location = new Point(20, 30),
                Size = new Size(300, 23),
                Checked = true
            };

            chkIncludeAllowances = new CheckBox
            {
                Text = "Include allowances in salary credit calculation",
                Location = new Point(20, 60),
                Size = new Size(300, 23),
                Checked = false
            };

            chkIncludeOvertime = new CheckBox
            {
                Text = "Include overtime pay in salary credit calculation",
                Location = new Point(20, 90),
                Size = new Size(300, 23),
                Checked = false
            };

            grpOptions.Controls.AddRange(new Control[] { chkAutoCalculate, chkIncludeAllowances, chkIncludeOvertime });

            // Buttons
            btnSaveSettings = new Button
            {
                Text = "Save Settings",
                Location = new Point(0, 350),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSaveSettings.Click += BtnSaveSettings_Click;

            btnResetSettings = new Button
            {
                Text = "Reset",
                Location = new Point(110, 350),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnResetSettings.Click += BtnResetSettings_Click;

            btnLoadDefaults = new Button
            {
                Text = "Load Defaults",
                Location = new Point(200, 350),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLoadDefaults.Click += BtnLoadDefaults_Click;

            pnlMain.Controls.AddRange(new Control[] {
                lblTitle, grpRates, grpLimits, grpOptions,
                btnSaveSettings, btnResetSettings, btnLoadDefaults
            });

            tabSettings.Controls.Add(pnlMain);
        }

        private void InitializeReportsTab()
        {
            // Top panel
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblTitle = new Label
            {
                Text = "SSS Reports",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(150, 25)
            };

            Label lblReportType = new Label
            {
                Text = "Report Type:",
                Location = new Point(20, 50),
                Size = new Size(80, 23)
            };

            cmbReportType = new ComboBox
            {
                Location = new Point(105, 47),
                Size = new Size(150, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbReportType.Items.AddRange(new string[] {
                "Monthly Contributions",
                "Quarterly Summary",
                "Annual Report",
                "Employee Listing",
                "Contribution History"
            });
            cmbReportType.SelectedIndex = 0;

            Label lblReportFrom = new Label
            {
                Text = "From:",
                Location = new Point(270, 50),
                Size = new Size(40, 23)
            };

            dtpReportFrom = new DateTimePicker
            {
                Location = new Point(315, 47),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };

            Label lblReportTo = new Label
            {
                Text = "To:",
                Location = new Point(450, 50),
                Size = new Size(30, 23)
            };

            dtpReportTo = new DateTimePicker
            {
                Location = new Point(485, 47),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };

            btnGenerateReport = new Button
            {
                Text = "Generate",
                Location = new Point(700, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerateReport.Click += BtnGenerateReport_Click;

            btnExportReport = new Button
            {
                Text = "Export",
                Location = new Point(790, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportReport.Click += BtnExportReport_Click;

            btnPrintReport = new Button
            {
                Text = "Print",
                Location = new Point(860, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPrintReport.Click += BtnPrintReport_Click;

            pnlTop.Controls.AddRange(new Control[] {
                lblTitle, lblReportType, cmbReportType, lblReportFrom, dtpReportFrom,
                lblReportTo, dtpReportTo, btnGenerateReport, btnExportReport, btnPrintReport
            });

            // DataGridView for reports
            dgvReports = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            tabReports.Controls.AddRange(new Control[] { pnlTop, dgvReports });
            UtilityHelper.ApplyLightMode(dgvReports);
        }

        private void LoadInitialData()
        {
            LoadContributionTable();
            LoadDepartments();
            LoadPayPeriods();
            LoadEmployeeContributions();
            LoadSettings();
        }

        private void LoadContributionTable()
        {
            try
            {
                // Clear any existing data
                dgvContributionTable.DataSource = null;
                dgvContributionTable.Rows.Clear();
                dgvContributionTable.Columns.Clear();

                string query = @"
                    SELECT 
                        id,
                        salary_from,
                        salary_to,
                        salary_credit,
                        employee_contribution,
                        employer_contribution,
                        total_contribution,
                        effective_date,
                        CASE WHEN is_active = 1 THEN 'Active' ELSE 'Inactive' END as Status
                    FROM sss_contribution_table
                    ORDER BY salary_from";

                DataTable dt = UtilityHelper.GetDataSet(query);
                
                // Set up the DataGridView columns with proper headers
                dgvContributionTable.Columns.Add("ID", "ID");
                dgvContributionTable.Columns.Add("RangeFrom", "Range From");
                dgvContributionTable.Columns.Add("RangeTo", "Range To");
                dgvContributionTable.Columns.Add("SalaryCredit", "Salary Credit");
                dgvContributionTable.Columns.Add("EmployeeShare", "Employee Share");
                dgvContributionTable.Columns.Add("EmployerShare", "Employer Share");
                dgvContributionTable.Columns.Add("TotalContribution", "Total Contribution");
                dgvContributionTable.Columns.Add("EffectiveDate", "Effective Date");
                dgvContributionTable.Columns.Add("Status", "Status");
                
                // Add rows from the DataTable
                foreach (DataRow row in dt.Rows)
                {
                    dgvContributionTable.Rows.Add(
                        row["id"],
                        decimal.Parse(row["salary_from"].ToString()).ToString("N2"),
                        decimal.Parse(row["salary_to"].ToString()).ToString("N2"),
                        decimal.Parse(row["salary_credit"].ToString()).ToString("N2"),
                        decimal.Parse(row["employee_contribution"].ToString()).ToString("N2"),
                        decimal.Parse(row["employer_contribution"].ToString()).ToString("N2"),
                        decimal.Parse(row["total_contribution"].ToString()).ToString("N2"),
                        Convert.ToDateTime(row["effective_date"]).ToString("MM/dd/yyyy"),
                        row["Status"]
                    );
                }

                // Hide ID column
                dgvContributionTable.Columns["ID"].Visible = false;

                // Format currency columns
                FormatCurrencyColumns(dgvContributionTable, new[] {
                    "RangeFrom", "RangeTo", "SalaryCredit",
                    "EmployeeShare", "EmployerShare", "TotalContribution"
                });

                UpdateRangeCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading contribution table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                string query = "SELECT DISTINCT department FROM employees WHERE department IS NOT NULL ORDER BY department";
                DataTable dt = UtilityHelper.GetDataSet(query);
                
                cmbDepartment.Items.Clear();
                cmbDepartment.Items.Add("All Departments");
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbDepartment.Items.Add(row["department"].ToString());
                }
                
                cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPayPeriods()
        {
            try
            {
                cmbPayPeriod.Items.Clear();
                cmbPayPeriod.Items.AddRange(new string[] {
                    "Current Period",
                    "Previous Period",
                    "Custom Range",
                    "All Periods"
                });
                cmbPayPeriod.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pay periods: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEmployeeContributions()
        {
            try
            {

                // Build the query with filters


                    FROM tbl_sss_contribution sc
                    INNER JOIN tbl_employee e ON sc.employee_id = e.id
                    WHERE 1=1";


                List<MySqlParameter> parameters = new List<MySqlParameter>();

                // Add search filter
                if (!string.IsNullOrWhiteSpace(txtSearchEmployee.Text))
                {
                    query += @" AND (e.first_name LIKE @searchTerm 
                              OR e.last_name LIKE @searchTerm 
                              OR e.employee_id LIKE @searchTerm)";
                    parameters.Add(new MySqlParameter("@searchTerm", $"%{txtSearchEmployee.Text}%"));
                }

                // Add department filter
                if (cmbDepartment.SelectedIndex > 0 && cmbDepartment.SelectedValue != null)
                {
                    query += " AND e.department_id = @departmentId";
                    parameters.Add(new MySqlParameter("@departmentId", cmbDepartment.SelectedValue));
                }

                // Add pay period filter
                if (cmbPayPeriod.SelectedIndex > 0 && cmbPayPeriod.SelectedValue != null)
                {
                    query += " AND sc.pay_period = @payPeriod";
                    parameters.Add(new MySqlParameter("@payPeriod", cmbPayPeriod.SelectedValue));
                }

                // Add date range filter
                query += " AND sc.contribution_date BETWEEN @fromDate AND @toDate";
                parameters.Add(new MySqlParameter("@fromDate", dtpFromDate.Value.Date));
                parameters.Add(new MySqlParameter("@toDate", dtpToDate.Value.Date));

                // Add order by clause
                query += " ORDER BY sc.contribution_date DESC, e.last_name, e.first_name";

                DataTable dt = UtilityHelper.GetDataSet(query, parameters.ToArray());
                dgvEmployeeContributions.DataSource = dt;

                if (dgvEmployeeContributions.Columns["id"] != null)
                    dgvEmployeeContributions.Columns["id"].Visible = false;

                // Format currency columns
                FormatCurrencyColumns(dgvEmployeeContributions, new[] {
                    "Salary Credit", "Employee Share", "Employer Share", "Total Contribution"
                });

                // Format date column
                if (dgvEmployeeContributions.Columns["Date"] != null)
                {
                    dgvEmployeeContributions.Columns["Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
                }

                UpdateContributionCount();
                UpdateTotalAmount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employee contributions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSettings()
        {
            try
            {
                string query = "SELECT * FROM sss_settings WHERE id = 1";
                DataTable dt = UtilityHelper.GetDataSet(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    nudEmployerRate.Value = Convert.ToDecimal(row["employer_rate"]);
                    nudEmployeeRate.Value = Convert.ToDecimal(row["employee_rate"]);
                    nudMaxSalaryCredit.Value = Convert.ToDecimal(row["max_salary_credit"]);
                    nudMinSalaryCredit.Value = Convert.ToDecimal(row["min_salary_credit"]);
                    chkAutoCalculate.Checked = Convert.ToBoolean(row["auto_calculate"]);
                    chkIncludeAllowances.Checked = Convert.ToBoolean(row["include_allowances"]);
                    chkIncludeOvertime.Checked = Convert.ToBoolean(row["include_overtime"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatCurrencyColumns(DataGridView dgv, string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns[columnName] != null)
                {
                    dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv.Columns[columnName].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void UpdateRangeCount()
        {
            lblTotalRanges.Text = $"Total Ranges: {dgvContributionTable.Rows.Count:N0}";
        }

        private void UpdateContributionCount()
        {
            lblTotalContributions.Text = $"Total Employees: {dgvEmployeeContributions.Rows.Count:N0}";
        }

        private void UpdateTotalAmount()
        {
            try
            {
                decimal total = 0;
                foreach (DataGridViewRow row in dgvEmployeeContributions.Rows)
                {
                    if (row.Cells["Total Contribution"].Value != null)
                    {
                        string value = row.Cells["Total Contribution"].Value.ToString().Replace(",", "");
                        if (decimal.TryParse(value, out decimal amount))
                        {
                            total += amount;
                        }
                    }
                }
                lblTotalAmount.Text = $"Total Amount: ₱{total:N2}";
            }
            catch (Exception ex)
            {
                lblTotalAmount.Text = "Total Amount: ₱0.00";
                Console.WriteLine($"Error calculating total amount: {ex}");
            }
        }

        // Event handlers for Contribution Table tab
        private void BtnAddRange_Click(object sender, EventArgs e)
        {
            using (Form addRangeForm = new Form())
            {
                addRangeForm.Text = "Add SSS Contribution Range";
                addRangeForm.Size = new Size(400, 400);
                addRangeForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                addRangeForm.StartPosition = FormStartPosition.CenterParent;
                addRangeForm.MaximizeBox = false;
                addRangeForm.MinimizeBox = false;

                // Create controls
                Label lblSalaryFrom = new Label { Text = "Salary From:", Location = new Point(20, 20) };
                NumericUpDown nudSalaryFrom = new NumericUpDown
                {
                    Location = new Point(150, 20),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true
                };

                Label lblSalaryTo = new Label { Text = "Salary To:", Location = new Point(20, 60) };
                NumericUpDown nudSalaryTo = new NumericUpDown
                {
                    Location = new Point(150, 60),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true
                };

                Label lblSalaryCredit = new Label { Text = "Salary Credit:", Location = new Point(20, 100) };
                NumericUpDown nudSalaryCredit = new NumericUpDown
                {
                    Location = new Point(150, 100),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true
                };

                Label lblEmployeeShare = new Label { Text = "Employee Share:", Location = new Point(20, 140) };
                NumericUpDown nudEmployeeShare = new NumericUpDown
                {
                    Location = new Point(150, 140),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true
                };

                Label lblEmployerShare = new Label { Text = "Employer Share:", Location = new Point(20, 180) };
                NumericUpDown nudEmployerShare = new NumericUpDown
                {
                    Location = new Point(150, 180),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true
                };

                Label lblEffectiveDate = new Label { Text = "Effective Date:", Location = new Point(20, 220) };
                DateTimePicker dtpEffectiveDate = new DateTimePicker
                {
                    Location = new Point(150, 220),
                    Size = new Size(200, 23),
                    Format = DateTimePickerFormat.Short
                };

                CheckBox chkActive = new CheckBox
                {
                    Text = "Active",
                    Location = new Point(150, 260),
                    Checked = true
                };

                Button btnSave = new Button
                {
                    Text = "Save",
                    DialogResult = DialogResult.OK,
                    Location = new Point(150, 300)
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(270, 300)
                };

                // Add controls to form
                addRangeForm.Controls.AddRange(new Control[] {
                    lblSalaryFrom, nudSalaryFrom,
                    lblSalaryTo, nudSalaryTo,
                    lblSalaryCredit, nudSalaryCredit,
                    lblEmployeeShare, nudEmployeeShare,
                    lblEmployerShare, nudEmployerShare,
                    lblEffectiveDate, dtpEffectiveDate,
                    chkActive,
                    btnSave, btnCancel
                });

                // Show dialog
                if (addRangeForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string query = @"INSERT INTO sss_contribution_table 
                            (salary_from, salary_to, salary_credit, employee_contribution, 
                             employer_contribution, total_contribution, effective_date, is_active)
                            VALUES 
                            (@salaryFrom, @salaryTo, @salaryCredit, @employeeShare,
                             @employerShare, @totalContribution, @effectiveDate, @isActive)";

                        decimal totalContribution = nudEmployeeShare.Value + nudEmployerShare.Value;

                        var parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@salaryFrom", nudSalaryFrom.Value),
                            new MySqlParameter("@salaryTo", nudSalaryTo.Value),
                            new MySqlParameter("@salaryCredit", nudSalaryCredit.Value),
                            new MySqlParameter("@employeeShare", nudEmployeeShare.Value),
                            new MySqlParameter("@employerShare", nudEmployerShare.Value),
                            new MySqlParameter("@totalContribution", totalContribution),
                            new MySqlParameter("@effectiveDate", dtpEffectiveDate.Value.Date),
                            new MySqlParameter("@isActive", chkActive.Checked)
                        };

                        DatabaseManager.ExecuteNonQuery(query, parameters);
                        LoadContributionTable();
                        MessageBox.Show("SSS contribution range added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding SSS contribution range: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnEditRange_Click(object sender, EventArgs e)
        {
            if (dgvContributionTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a contribution range to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dgvContributionTable.SelectedRows[0];
            int rangeId = Convert.ToInt32(selectedRow.Cells["id"].Value);

            using (Form editRangeForm = new Form())
            {
                editRangeForm.Text = "Edit SSS Contribution Range";
                editRangeForm.Size = new Size(400, 400);
                editRangeForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                editRangeForm.StartPosition = FormStartPosition.CenterParent;
                editRangeForm.MaximizeBox = false;
                editRangeForm.MinimizeBox = false;

                // Create controls
                Label lblSalaryFrom = new Label { Text = "Salary From:", Location = new Point(20, 20) };
                NumericUpDown nudSalaryFrom = new NumericUpDown
                {
                    Location = new Point(150, 20),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true,
                    Value = Convert.ToDecimal(selectedRow.Cells["RangeFrom"].Value.ToString().Replace(",", ""))
                };

                Label lblSalaryTo = new Label { Text = "Salary To:", Location = new Point(20, 60) };
                NumericUpDown nudSalaryTo = new NumericUpDown
                {
                    Location = new Point(150, 60),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true,
                    Value = Convert.ToDecimal(selectedRow.Cells["RangeTo"].Value.ToString().Replace(",", ""))
                };

                Label lblSalaryCredit = new Label { Text = "Salary Credit:", Location = new Point(20, 100) };
                NumericUpDown nudSalaryCredit = new NumericUpDown
                {
                    Location = new Point(150, 100),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true,
                    Value = Convert.ToDecimal(selectedRow.Cells["SalaryCredit"].Value.ToString().Replace(",", ""))
                };

                Label lblEmployeeShare = new Label { Text = "Employee Share:", Location = new Point(20, 140) };
                NumericUpDown nudEmployeeShare = new NumericUpDown
                {
                    Location = new Point(150, 140),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true,
                    Value = Convert.ToDecimal(selectedRow.Cells["EmployeeShare"].Value.ToString().Replace(",", ""))
                };

                Label lblEmployerShare = new Label { Text = "Employer Share:", Location = new Point(20, 180) };
                NumericUpDown nudEmployerShare = new NumericUpDown
                {
                    Location = new Point(150, 180),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999,
                    ThousandsSeparator = true,
                    Value = Convert.ToDecimal(selectedRow.Cells["EmployerShare"].Value.ToString().Replace(",", ""))
                };

                Label lblEffectiveDate = new Label { Text = "Effective Date:", Location = new Point(20, 220) };
                DateTimePicker dtpEffectiveDate = new DateTimePicker
                {
                    Location = new Point(150, 220),
                    Size = new Size(200, 23),
                    Format = DateTimePickerFormat.Short,
                    Value = Convert.ToDateTime(selectedRow.Cells["EffectiveDate"].Value)
                };

                CheckBox chkActive = new CheckBox
                {
                    Text = "Active",
                    Location = new Point(150, 260),
                    Checked = selectedRow.Cells["Status"].Value.ToString() == "Active"
                };

                Button btnSave = new Button
                {
                    Text = "Save",
                    DialogResult = DialogResult.OK,
                    Location = new Point(150, 300)
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(270, 300)
                };

                // Add controls to form
                editRangeForm.Controls.AddRange(new Control[] {
                    lblSalaryFrom, nudSalaryFrom,
                    lblSalaryTo, nudSalaryTo,
                    lblSalaryCredit, nudSalaryCredit,
                    lblEmployeeShare, nudEmployeeShare,
                    lblEmployerShare, nudEmployerShare,
                    lblEffectiveDate, dtpEffectiveDate,
                    chkActive,
                    btnSave, btnCancel
                });

                // Show dialog
                if (editRangeForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string query = @"UPDATE sss_contribution_table 
                            SET salary_from = @salaryFrom,
                                salary_to = @salaryTo,
                                salary_credit = @salaryCredit,
                                employee_contribution = @employeeShare,
                                employer_contribution = @employerShare,
                                total_contribution = @totalContribution,
                                effective_date = @effectiveDate,
                                is_active = @isActive
                            WHERE id = @id";

                        decimal totalContribution = nudEmployeeShare.Value + nudEmployerShare.Value;

                        var parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@id", rangeId),
                            new MySqlParameter("@salaryFrom", nudSalaryFrom.Value),
                            new MySqlParameter("@salaryTo", nudSalaryTo.Value),
                            new MySqlParameter("@salaryCredit", nudSalaryCredit.Value),
                            new MySqlParameter("@employeeShare", nudEmployeeShare.Value),
                            new MySqlParameter("@employerShare", nudEmployerShare.Value),
                            new MySqlParameter("@totalContribution", totalContribution),
                            new MySqlParameter("@effectiveDate", dtpEffectiveDate.Value.Date),
                            new MySqlParameter("@isActive", chkActive.Checked)
                        };

                        DatabaseManager.ExecuteNonQuery(query, parameters);
                        LoadContributionTable();
                        MessageBox.Show("SSS contribution range updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating SSS contribution range: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnDeleteRange_Click(object sender, EventArgs e)
        {
            if (dgvContributionTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a contribution range to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this contribution range?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int rangeId = Convert.ToInt32(dgvContributionTable.SelectedRows[0].Cells["id"].Value);
                    string query = "DELETE FROM sss_contribution_table WHERE id = @id";
                    var parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@id", rangeId)
                    };

                    DatabaseManager.ExecuteNonQuery(query, parameters);
                    LoadContributionTable();
                    MessageBox.Show("SSS contribution range deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting SSS contribution range: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnImportTable_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int successCount = 0;
                        int errorCount = 0;
                        string[] lines = File.ReadAllLines(openFileDialog.FileName);

                        // Skip header row
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] values = lines[i].Split(',');

                            try
                            {
                                if (values.Length >= 7)
                                {
                                    string query = @"INSERT INTO sss_contribution_table 
                                        (salary_from, salary_to, salary_credit, employee_contribution, 
                                         employer_contribution, total_contribution, effective_date, is_active)
                                        VALUES 
                                        (@salaryFrom, @salaryTo, @salaryCredit, @employeeShare,
                                         @employerShare, @totalContribution, @effectiveDate, @isActive)";

                                    decimal employeeShare = decimal.Parse(values[3].Trim());
                                    decimal employerShare = decimal.Parse(values[4].Trim());
                                    decimal totalContribution = employeeShare + employerShare;

                                    var parameters = new MySqlParameter[]
                                    {
                                        new MySqlParameter("@salaryFrom", decimal.Parse(values[0].Trim())),
                                        new MySqlParameter("@salaryTo", decimal.Parse(values[1].Trim())),
                                        new MySqlParameter("@salaryCredit", decimal.Parse(values[2].Trim())),
                                        new MySqlParameter("@employeeShare", employeeShare),
                                        new MySqlParameter("@employerShare", employerShare),
                                        new MySqlParameter("@totalContribution", totalContribution),
                                        new MySqlParameter("@effectiveDate", DateTime.Parse(values[5].Trim())),
                                        new MySqlParameter("@isActive", bool.Parse(values[6].Trim()))
                                    };

                                    DatabaseManager.ExecuteNonQuery(query, parameters);
                                    successCount++;
                                }
                            }
                            catch (Exception)
                            {
                                errorCount++;
                            }
                        }

                        LoadContributionTable();
                        MessageBox.Show($"Import completed:\n{successCount} records imported successfully\n{errorCount} records failed", 
                            "Import Result", MessageBoxButtons.OK, 
                            errorCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error importing SSS contribution table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnExportTable_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvContributionTable, "SSS_ContributionTable");
        }

        private void DgvContributionTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditRange_Click(sender, e);
            }
        }

        // Event handlers for Employee Contributions tab
        private void TxtSearchEmployee_TextChanged(object sender, EventArgs e)
        {
            LoadEmployeeContributions(); // Simplified - would include search filter in real implementation
        }

        private void CmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployeeContributions(); // Simplified - would include department filter in real implementation
        }

        private void CmbPayPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployeeContributions(); // Simplified - would include pay period filter in real implementation
        }

        private void DtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            LoadEmployeeContributions(); // Simplified - would include date filter in real implementation
        }

        private void DtpToDate_ValueChanged(object sender, EventArgs e)
        {
            LoadEmployeeContributions(); // Simplified - would include date filter in real implementation
        }

        private void BtnCalculateContributions_Click(object sender, EventArgs e)
        {
            try
            {
                // First, calculate the contributions based on salary ranges
                string calcQuery = @"UPDATE payroll_details pd
                    INNER JOIN employees ei ON pd.employee_id = ei.employee_id 
                    INNER JOIN sss_contribution_table sct ON 
                        pd.gross_pay BETWEEN sct.salary_from AND sct.salary_to
                        AND sct.is_active = 1
                    SET 
                        pd.sss_contribution = sct.employee_contribution,
                        pd.sss_employer_contribution = sct.employer_contribution
                    WHERE 
                        pd.payroll_date BETWEEN @fromDate AND @toDate
                        AND (@departmentId = 0 OR ei.department_id = @departmentId)
                        AND pd.period_id = @payPeriodId";

                var calcParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@fromDate", dtpFromDate.Value.Date),
                    new MySqlParameter("@toDate", dtpToDate.Value.Date),
                    new MySqlParameter("@departmentId", cmbDepartment.SelectedValue ?? 0),
                    new MySqlParameter("@payPeriodId", cmbPayPeriod.SelectedValue ?? 0)
                };

                DatabaseManager.ExecuteNonQuery(calcQuery, calcParameters);

                // Then, insert the calculated contributions into the SSS contributions table
                string insertQuery = @"INSERT INTO tbl_sss_contribution 
                    (employee_id, salary_credit, employee_contribution, employer_contribution, 
                     total_contribution, pay_period, contribution_date)
                    SELECT 
                        ei.id,
                        pd.gross_pay,
                        pd.sss_contribution,
                        pd.sss_employer_contribution,
                        (pd.sss_contribution + pd.sss_employer_contribution),
                        pd.period_id,
                        pd.payroll_date
                    FROM payroll_details pd
                    INNER JOIN employees ei ON pd.employee_id = ei.employee_id
                    WHERE 
                        pd.payroll_date BETWEEN @fromDate AND @toDate
                        AND (@departmentId = 0 OR ei.department_id = @departmentId)
                        AND pd.period_id = @payPeriodId
                        AND pd.sss_contribution > 0";

                var insertParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@fromDate", dtpFromDate.Value.Date),
                    new MySqlParameter("@toDate", dtpToDate.Value.Date),
                    new MySqlParameter("@departmentId", cmbDepartment.SelectedValue ?? 0),
                    new MySqlParameter("@payPeriodId", cmbPayPeriod.SelectedValue ?? 0)
                };

                DatabaseManager.ExecuteNonQuery(insertQuery, insertParameters);
                
                // Refresh the grid with the new data
                LoadEmployeeContributions();
                MessageBox.Show("SSS contributions calculated and saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating SSS contributions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportContributions_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmployeeContributions.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                ExportToCSV(dgvEmployeeContributions, "SSS_EmployeeContributions");
                MessageBox.Show("Data exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event handlers for Settings tab
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
                    UPDATE sss_settings SET 
                        employer_rate = @employer_rate,
                        employee_rate = @employee_rate,
                        max_salary_credit = @max_salary_credit,
                        min_salary_credit = @min_salary_credit,
                        auto_calculate = @auto_calculate,
                        include_allowances = @include_allowances,
                        include_overtime = @include_overtime,
                        updated_date = NOW()
                    WHERE id = 1";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    DatabaseManager.CreateParameter("@employer_rate", nudEmployerRate.Value),
                    DatabaseManager.CreateParameter("@employee_rate", nudEmployeeRate.Value),
                    DatabaseManager.CreateParameter("@max_salary_credit", nudMaxSalaryCredit.Value),
                    DatabaseManager.CreateParameter("@min_salary_credit", nudMinSalaryCredit.Value),
                    DatabaseManager.CreateParameter("@auto_calculate", chkAutoCalculate.Checked),
                    DatabaseManager.CreateParameter("@include_allowances", chkIncludeAllowances.Checked),
                    DatabaseManager.CreateParameter("@include_overtime", chkIncludeOvertime.Checked)
                };

                DatabaseManager.ExecuteNonQuery(query, parameters);
                MessageBox.Show("SSS settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnResetSettings_Click(object sender, EventArgs e)
        {
            LoadSettings();
            MessageBox.Show("Settings reset to last saved values.", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLoadDefaults_Click(object sender, EventArgs e)
        {
            nudEmployerRate.Value = 8.5m;
            nudEmployeeRate.Value = 4.5m;
            nudMaxSalaryCredit.Value = 25000m;
            nudMinSalaryCredit.Value = 1000m;
            chkAutoCalculate.Checked = true;
            chkIncludeAllowances.Checked = false;
            chkIncludeOvertime.Checked = false;
            MessageBox.Show("Default settings loaded.", "Defaults", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Event handlers for Reports tab
        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string reportType = cmbReportType.SelectedItem?.ToString() ?? "Monthly Contributions";
                DateTime fromDate = dtpReportFrom.Value;
                DateTime toDate = dtpReportTo.Value;

                // Generate report based on selected type
                string query = GetReportQuery(reportType, fromDate, toDate);
                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvReports.DataSource = dt;

                // Format currency columns if present
                FormatReportColumns(dgvReports);

                MessageBox.Show($"{reportType} report generated successfully!", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportReport_Click(object sender, EventArgs e)
        {
            string reportType = cmbReportType.SelectedItem?.ToString() ?? "SSS_Report";
            ExportToCSV(dgvReports, reportType.Replace(" ", "_"));
        }

        private void BtnPrintReport_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("No data to print. Please generate a report first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                PrintDocument printDoc = new PrintDocument();
                printDoc.DefaultPageSettings.Landscape = true;
                printDoc.PrintPage += new PrintPageEventHandler(PrintReport);

                PrintPreviewDialog previewDialog = new PrintPreviewDialog();
                previewDialog.Document = printDoc;
                previewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error preparing print preview: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReport(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                float leftMargin = e.MarginBounds.Left;
                float topMargin = e.MarginBounds.Top;
                float availableWidth = e.MarginBounds.Width;

                // Print title
                string title = $"SSS {cmbReportType.Text} Report";
                string dateRange = $"From: {dtpReportFrom.Value.ToShortDateString()} To: {dtpReportTo.Value.ToShortDateString()}";
                using (Font titleFont = new Font("Arial", 14, FontStyle.Bold))
                {
                    g.DrawString(title, titleFont, Brushes.Black, leftMargin, topMargin);
                    topMargin += titleFont.GetHeight() + 5;
                }

                using (Font dateFont = new Font("Arial", 10))
                {
                    g.DrawString(dateRange, dateFont, Brushes.Black, leftMargin, topMargin);
                    topMargin += dateFont.GetHeight() * 2;
                }

                // Calculate column widths
                float[] columnWidths = new float[dgvReports.Columns.Count];
                float totalWidth = 0;
                for (int i = 0; i < dgvReports.Columns.Count; i++)
                {
                    columnWidths[i] = dgvReports.Columns[i].Width;
                    totalWidth += columnWidths[i];
                }

                // Scale column widths to fit page
                float scaleFactor = availableWidth / totalWidth;
                for (int i = 0; i < columnWidths.Length; i++)
                {
                    columnWidths[i] *= scaleFactor;
                }

                // Print column headers
                float currentX = leftMargin;
                using (Font headerFont = new Font("Arial", 10, FontStyle.Bold))
                {
                    for (int i = 0; i < dgvReports.Columns.Count; i++)
                    {
                        RectangleF cellBounds = new RectangleF(currentX, topMargin, columnWidths[i], headerFont.GetHeight());
                        g.DrawString(dgvReports.Columns[i].HeaderText, headerFont, Brushes.Black, cellBounds);
                        currentX += columnWidths[i];
                    }
                    topMargin += headerFont.GetHeight() + 5;
                }

                // Print data rows
                using (Font dataFont = new Font("Arial", 9))
                {
                    foreach (DataGridViewRow row in dgvReports.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            currentX = leftMargin;
                            for (int i = 0; i < dgvReports.Columns.Count; i++)
                            {
                                RectangleF cellBounds = new RectangleF(currentX, topMargin, columnWidths[i], dataFont.GetHeight());
                                string cellValue = row.Cells[i].Value?.ToString() ?? "";
                                g.DrawString(cellValue, dataFont, Brushes.Black, cellBounds);
                                currentX += columnWidths[i];
                            }
                            topMargin += dataFont.GetHeight() + 2;

                            // Check if we need a new page
                            if (topMargin > e.MarginBounds.Bottom - dataFont.GetHeight())
                            {
                                e.HasMorePages = true;
                                return;
                            }
                        }
                    }
                }

                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.HasMorePages = false;
            }
        }

        private string GetReportQuery(string reportType, DateTime fromDate, DateTime toDate)
        {
            switch (reportType)
            {
                case "Monthly Contributions":
                    return $@"
                        SELECT 
                            CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                            e.employee_id as 'Employee ID',
                            e.department as 'Department',
                            FORMAT(SUM(sc.salary_credit), 2) as 'Total Salary Credit',
                            FORMAT(SUM(sc.employee_contribution), 2) as 'Employee Share',
                            FORMAT(SUM(sc.employer_contribution), 2) as 'Employer Share',
                            FORMAT(SUM(sc.total_contribution), 2) as 'Total Contribution'
                        FROM sss_contributions sc
                        INNER JOIN employees e ON sc.employee_id = e.employee_id
                        WHERE sc.contribution_date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'
                        GROUP BY e.employee_id, e.first_name, e.last_name, e.department
                        ORDER BY e.last_name, e.first_name";

                case "Quarterly Summary":
                    //COUNT(DISTINCT e.id) as 'Employee Count',
                    return $@"
                        SELECT 
                            e.department as 'Department',
                            FORMAT(SUM(sc.salary_credit), 2) as 'Total Salary Credit',
                            FORMAT(SUM(sc.employee_contribution), 2) as 'Employee Share',
                            FORMAT(SUM(sc.employer_contribution), 2) as 'Employer Share',
                            FORMAT(SUM(sc.total_contribution), 2) as 'Total Contribution'
                        FROM sss_contributions sc
                        INNER JOIN employees e ON sc.employee_id = e.employee_id
                        WHERE sc.contribution_date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'
                        GROUP BY e.department
                        ORDER BY e.department";

                default:
                    return $@"
                        SELECT 
                            CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                            e.employee_id as 'Employee ID',
                            FORMAT(sc.salary_credit, 2) as 'Salary Credit',
                            FORMAT(sc.total_contribution, 2) as 'Total Contribution',
                            sc.contribution_date as 'Date'
                        FROM sss_contributions sc
                        INNER JOIN employees e ON sc.employee_id = e.employee_id
                        WHERE sc.contribution_date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'
                        ORDER BY sc.contribution_date DESC";
            }
        }

        private void FormatReportColumns(DataGridView dgv)
        {
            string[] currencyColumns = {
                "Total Salary Credit", "Employee Share", "Employer Share", "Total Contribution",
                "Salary Credit"
            };

            FormatCurrencyColumns(dgv, currencyColumns);

            // Format date columns
            if (dgv.Columns["Date"] != null)
            {
                dgv.Columns["Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
            }
        }

        // Utility method for exporting data
        private void ExportToCSV(DataGridView dgv, string fileName)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv";
                    sfd.FileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder csv = new StringBuilder();

                        // Add headers
                        var headers = new List<string>();
                        for (int i = 1; i < dgv.Columns.Count; i++) // Skip ID column
                        {
                            if (dgv.Columns[i].Visible)
                                headers.Add(dgv.Columns[i].HeaderText);
                        }
                        csv.AppendLine(string.Join(",", headers));

                        // Add data
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            var values = new List<string>();
                            for (int i = 1; i < dgv.Columns.Count; i++) // Skip ID column
                            {
                                if (dgv.Columns[i].Visible)
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
    }
}
