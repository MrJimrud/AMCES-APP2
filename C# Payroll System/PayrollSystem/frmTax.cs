using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using MySqlConnector;

public static class TextBoxExtensions
{
    public static void SetPlaceholderText(this TextBox textBox, string placeholderText)
    {
        textBox.Text = placeholderText;
        textBox.ForeColor = System.Drawing.Color.Gray;
        textBox.GotFocus += (s, e) => {
            if (textBox.Text == placeholderText)
            {
                textBox.Text = "";
                textBox.ForeColor = System.Drawing.Color.Black;
            }
        };
        textBox.LostFocus += (s, e) => {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholderText;
                textBox.ForeColor = System.Drawing.Color.Gray;
            }
        };
    }
}

namespace PayrollSystem
{
    public partial class frmTax : Form
    {
        private TabControl tabControl;
        private TabPage tabTaxBrackets;
        private TabPage tabEmployeeTax;
        private TabPage tabSettings;
        private TabPage tabReports;
        
        // Tax Brackets Tab Controls
        private DataGridView dgvTaxBrackets;
        private ComboBox cmbTaxYear;
        private ComboBox cmbFilingStatus;
        private Button btnAddBracket;
        private Button btnEditBracket;
        private Button btnDeleteBracket;
        private Button btnImportBrackets;
        private Button btnExportBrackets;
        private Label lblTotalBrackets;
        
        // Employee Tax Tab Controls
        private DataGridView dgvEmployeeTax;
        private ComboBox cmbDepartmentTax;
        private ComboBox cmbPayPeriodTax;
        private DateTimePicker dtpTaxFromDate;
        private DateTimePicker dtpTaxToDate;
        private TextBox txtSearchEmployeeTax;
        private Button btnCalculateTax;
        private Button btnExportTax;
        private Label lblTotalEmployees;
        private Label lblTotalTaxAmount;
        
        // Settings Tab Controls
        private NumericUpDown nudWithholdingRate;
        private NumericUpDown nudMinimumWage;
        private NumericUpDown nudPersonalExemption;
        private NumericUpDown nudAdditionalExemption;
        private CheckBox chkAutoCalculateTax;
        private CheckBox chkUseAnnualization;
        private CheckBox chkIncludeAllowancesInTax;
        private CheckBox chkIncludeBonusInTax;
        private Button btnSaveTaxSettings;
        private Button btnResetTaxSettings;
        private Button btnLoadTaxDefaults;
        
        // Reports Tab Controls
        private DataGridView dgvTaxReports;
        private ComboBox cmbTaxReportType;
        private DateTimePicker dtpTaxReportFrom;
        private DateTimePicker dtpTaxReportTo;
        private Button btnGenerateTaxReport;
        private Button btnExportTaxReport;
        private Button btnPrintTaxReport;

        public frmTax()
        {
            InitializeComponent();
            LoadInitialData();
        }

        private void InitializeComponent()
        {
            this.Text = "Tax Management";
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
            tabTaxBrackets = new TabPage("Tax Brackets");
            tabEmployeeTax = new TabPage("Employee Tax");
            tabSettings = new TabPage("Settings");
            tabReports = new TabPage("Reports");

            tabControl.TabPages.AddRange(new TabPage[] { tabTaxBrackets, tabEmployeeTax, tabSettings, tabReports });

            InitializeTaxBracketsTab();
            InitializeEmployeeTaxTab();
            InitializeSettingsTab();
            InitializeReportsTab();

            this.Controls.Add(tabControl);
        }

        private void InitializeTaxBracketsTab()
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
                Text = "Tax Bracket Management",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(250, 25)
            };

            Label lblTaxYear = new Label
            {
                Text = "Tax Year:",
                Location = new Point(20, 50),
                Size = new Size(70, 23)
            };

            cmbTaxYear = new ComboBox
            {
                Location = new Point(95, 47),
                Size = new Size(80, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTaxYear.SelectedIndexChanged += CmbTaxYear_SelectedIndexChanged;

            Label lblFilingStatus = new Label
            {
                Text = "Filing Status:",
                Location = new Point(190, 50),
                Size = new Size(80, 23)
            };

            cmbFilingStatus = new ComboBox
            {
                Location = new Point(275, 47),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilingStatus.Items.AddRange(new string[] {
                "Single",
                "Married Filing Jointly",
                "Married Filing Separately",
                "Head of Household"
            });
            cmbFilingStatus.SelectedIndex = 0;
            cmbFilingStatus.SelectedIndexChanged += CmbFilingStatus_SelectedIndexChanged;

            btnAddBracket = new Button
            {
                Text = "Add Bracket",
                Location = new Point(700, 15),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddBracket.Click += BtnAddBracket_Click;

            btnEditBracket = new Button
            {
                Text = "Edit",
                Location = new Point(800, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnEditBracket.Click += BtnEditBracket_Click;

            btnDeleteBracket = new Button
            {
                Text = "Delete",
                Location = new Point(870, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteBracket.Click += BtnDeleteBracket_Click;

            btnImportBrackets = new Button
            {
                Text = "Import",
                Location = new Point(940, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnImportBrackets.Click += BtnImportBrackets_Click;

            btnExportBrackets = new Button
            {
                Text = "Export",
                Location = new Point(1010, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportBrackets.Click += BtnExportBrackets_Click;

            pnlTop.Controls.AddRange(new Control[] {
                lblTitle, lblTaxYear, cmbTaxYear, lblFilingStatus, cmbFilingStatus,
                btnAddBracket, btnEditBracket, btnDeleteBracket, btnImportBrackets, btnExportBrackets
            });

            // DataGridView for tax brackets
            dgvTaxBrackets = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvTaxBrackets.CellDoubleClick += DgvTaxBrackets_CellDoubleClick;

            // Bottom panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalBrackets = new Label
            {
                Text = "Total Brackets: 0",
                Location = new Point(20, 10),
                Size = new Size(200, 23)
            };

            Button btnRefreshBrackets = new Button
            {
                Text = "Refresh",
                Location = new Point(700, 5),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshBrackets.Click += (s, e) => LoadTaxBrackets();

            pnlBottom.Controls.AddRange(new Control[] { lblTotalBrackets, btnRefreshBrackets });

            tabTaxBrackets.Controls.AddRange(new Control[] { pnlTop, dgvTaxBrackets, pnlBottom });
            UtilityHelper.ApplyLightMode(dgvTaxBrackets);
        }

        private void InitializeEmployeeTaxTab()
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
                Text = "Employee Tax Calculations",
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

            txtSearchEmployeeTax = new TextBox
            {
                Location = new Point(85, 47),
                Size = new Size(150, 23)
            };
            txtSearchEmployeeTax.SetPlaceholderText("Employee name or ID...");
            txtSearchEmployeeTax.TextChanged += TxtSearchEmployeeTax_TextChanged;

            Label lblDepartmentTax = new Label
            {
                Text = "Department:",
                Location = new Point(250, 50),
                Size = new Size(80, 23)
            };

            cmbDepartmentTax = new ComboBox
            {
                Location = new Point(335, 47),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDepartmentTax.SelectedIndexChanged += CmbDepartmentTax_SelectedIndexChanged;

            Label lblPayPeriodTax = new Label
            {
                Text = "Pay Period:",
                Location = new Point(470, 50),
                Size = new Size(80, 23)
            };

            cmbPayPeriodTax = new ComboBox
            {
                Location = new Point(555, 47),
                Size = new Size(100, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPayPeriodTax.SelectedIndexChanged += CmbPayPeriodTax_SelectedIndexChanged;

            Label lblTaxFromDate = new Label
            {
                Text = "From:",
                Location = new Point(20, 85),
                Size = new Size(40, 23)
            };

            dtpTaxFromDate = new DateTimePicker
            {
                Location = new Point(65, 82),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };
            dtpTaxFromDate.ValueChanged += DtpTaxFromDate_ValueChanged;

            Label lblTaxToDate = new Label
            {
                Text = "To:",
                Location = new Point(200, 85),
                Size = new Size(30, 23)
            };

            dtpTaxToDate = new DateTimePicker
            {
                Location = new Point(235, 82),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };
            dtpTaxToDate.ValueChanged += DtpTaxToDate_ValueChanged;

            btnCalculateTax = new Button
            {
                Text = "Calculate Tax",
                Location = new Point(700, 15),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCalculateTax.Click += BtnCalculateTax_Click;

            btnExportTax = new Button
            {
                Text = "Export",
                Location = new Point(810, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportTax.Click += BtnExportTax_Click;

            pnlTop.Controls.AddRange(new Control[] {
                lblTitle, lblSearch, txtSearchEmployeeTax, lblDepartmentTax, cmbDepartmentTax,
                lblPayPeriodTax, cmbPayPeriodTax, lblTaxFromDate, dtpTaxFromDate, lblTaxToDate, dtpTaxToDate,
                btnCalculateTax, btnExportTax
            });

            // DataGridView for employee tax
            dgvEmployeeTax = new DataGridView
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
            Panel pnlBottomTax = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalEmployees = new Label
            {
                Text = "Total Employees: 0",
                Location = new Point(20, 10),
                Size = new Size(150, 23)
            };

            lblTotalTaxAmount = new Label
            {
                Text = "Total Tax: ₱0.00",
                Location = new Point(180, 10),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            pnlBottomTax.Controls.AddRange(new Control[] { lblTotalEmployees, lblTotalTaxAmount });

            tabEmployeeTax.Controls.AddRange(new Control[] { pnlTop, dgvEmployeeTax, pnlBottomTax });
            UtilityHelper.ApplyLightMode(dgvEmployeeTax);
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
                Text = "Tax Configuration Settings",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(300, 30)
            };

            // Tax Rates Group
            GroupBox grpTaxRates = new GroupBox
            {
                Text = "Tax Rates & Exemptions",
                Location = new Point(0, 40),
                Size = new Size(400, 150)
            };

            Label lblWithholdingRate = new Label
            {
                Text = "Withholding Rate (%):",
                Location = new Point(20, 30),
                Size = new Size(130, 23)
            };

            nudWithholdingRate = new NumericUpDown
            {
                Location = new Point(160, 27),
                Size = new Size(80, 23),
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 100,
                Value = 5.0m
            };

            Label lblPersonalExemption = new Label
            {
                Text = "Personal Exemption:",
                Location = new Point(20, 60),
                Size = new Size(130, 23)
            };

            nudPersonalExemption = new NumericUpDown
            {
                Location = new Point(160, 57),
                Size = new Size(100, 23),
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 999999,
                Value = 50000m,
                ThousandsSeparator = true
            };

            Label lblAdditionalExemption = new Label
            {
                Text = "Additional Exemption:",
                Location = new Point(20, 90),
                Size = new Size(130, 23)
            };

            nudAdditionalExemption = new NumericUpDown
            {
                Location = new Point(160, 87),
                Size = new Size(100, 23),
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 999999,
                Value = 25000m,
                ThousandsSeparator = true
            };

            grpTaxRates.Controls.AddRange(new Control[] {
                lblWithholdingRate, nudWithholdingRate, lblPersonalExemption, nudPersonalExemption,
                lblAdditionalExemption, nudAdditionalExemption
            });

            // Minimum Wage Group
            GroupBox grpWage = new GroupBox
            {
                Text = "Minimum Wage Settings",
                Location = new Point(420, 40),
                Size = new Size(400, 80)
            };

            Label lblMinimumWage = new Label
            {
                Text = "Minimum Daily Wage:",
                Location = new Point(20, 30),
                Size = new Size(130, 23)
            };

            nudMinimumWage = new NumericUpDown
            {
                Location = new Point(160, 27),
                Size = new Size(100, 23),
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 999999,
                Value = 537m,
                ThousandsSeparator = true
            };

            grpWage.Controls.AddRange(new Control[] { lblMinimumWage, nudMinimumWage });

            // Calculation Options Group
            GroupBox grpTaxOptions = new GroupBox
            {
                Text = "Tax Calculation Options",
                Location = new Point(0, 210),
                Size = new Size(400, 180)
            };

            chkAutoCalculateTax = new CheckBox
            {
                Text = "Auto-calculate tax during payroll processing",
                Location = new Point(20, 30),
                Size = new Size(350, 23),
                Checked = true
            };

            chkUseAnnualization = new CheckBox
            {
                Text = "Use annualization method for tax calculation",
                Location = new Point(20, 60),
                Size = new Size(350, 23),
                Checked = true
            };

            chkIncludeAllowancesInTax = new CheckBox
            {
                Text = "Include taxable allowances in tax computation",
                Location = new Point(20, 90),
                Size = new Size(350, 23),
                Checked = true
            };

            chkIncludeBonusInTax = new CheckBox
            {
                Text = "Include bonus and 13th month pay in tax calculation",
                Location = new Point(20, 120),
                Size = new Size(350, 23),
                Checked = false
            };

            grpTaxOptions.Controls.AddRange(new Control[] {
                chkAutoCalculateTax, chkUseAnnualization, chkIncludeAllowancesInTax, chkIncludeBonusInTax
            });

            // Buttons
            btnSaveTaxSettings = new Button
            {
                Text = "Save Settings",
                Location = new Point(0, 410),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSaveTaxSettings.Click += BtnSaveTaxSettings_Click;

            btnResetTaxSettings = new Button
            {
                Text = "Reset",
                Location = new Point(110, 410),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnResetTaxSettings.Click += BtnResetTaxSettings_Click;

            btnLoadTaxDefaults = new Button
            {
                Text = "Load Defaults",
                Location = new Point(200, 410),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLoadTaxDefaults.Click += BtnLoadTaxDefaults_Click;

            pnlMain.Controls.AddRange(new Control[] {
                lblTitle, grpTaxRates, grpWage, grpTaxOptions,
                btnSaveTaxSettings, btnResetTaxSettings, btnLoadTaxDefaults
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
                Text = "Tax Reports",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(150, 25)
            };

            Label lblTaxReportType = new Label
            {
                Text = "Report Type:",
                Location = new Point(20, 50),
                Size = new Size(80, 23)
            };

            cmbTaxReportType = new ComboBox
            {
                Location = new Point(105, 47),
                Size = new Size(150, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTaxReportType.Items.AddRange(new string[] {
                "Monthly Tax Summary",
                "Quarterly Tax Report",
                "Annual Tax Report",
                "BIR Form 2316",
                "Withholding Tax Report",
                "Tax by Department"
            });
            cmbTaxReportType.SelectedIndex = 0;

            Label lblTaxReportFrom = new Label
            {
                Text = "From:",
                Location = new Point(270, 50),
                Size = new Size(40, 23)
            };

            dtpTaxReportFrom = new DateTimePicker
            {
                Location = new Point(315, 47),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };

            Label lblTaxReportTo = new Label
            {
                Text = "To:",
                Location = new Point(450, 50),
                Size = new Size(30, 23)
            };

            dtpTaxReportTo = new DateTimePicker
            {
                Location = new Point(485, 47),
                Size = new Size(120, 23),
                Format = DateTimePickerFormat.Short
            };

            btnGenerateTaxReport = new Button
            {
                Text = "Generate",
                Location = new Point(700, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerateTaxReport.Click += BtnGenerateTaxReport_Click;

            btnExportTaxReport = new Button
            {
                Text = "Export",
                Location = new Point(790, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportTaxReport.Click += BtnExportTaxReport_Click;

            btnPrintTaxReport = new Button
            {
                Text = "Print",
                Location = new Point(860, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPrintTaxReport.Click += BtnPrintTaxReport_Click;

            pnlTop.Controls.AddRange(new Control[] {
                lblTitle, lblTaxReportType, cmbTaxReportType, lblTaxReportFrom, dtpTaxReportFrom,
                lblTaxReportTo, dtpTaxReportTo, btnGenerateTaxReport, btnExportTaxReport, btnPrintTaxReport
            });

            // DataGridView for tax reports
            dgvTaxReports = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            tabReports.Controls.AddRange(new Control[] { pnlTop, dgvTaxReports });
            UtilityHelper.ApplyLightMode(dgvTaxReports);
        }

        private void LoadInitialData()
        {
            LoadTaxYears();
            LoadTaxBrackets();
            LoadDepartments();
            LoadPayPeriods();
            LoadEmployeeTax();
            LoadTaxSettings();
        }

        private void LoadTaxYears()
        {
            try
            {
                cmbTaxYear.Items.Clear();
                int currentYear = DateTime.Now.Year;
                for (int year = currentYear - 5; year <= currentYear + 1; year++)
                {
                    cmbTaxYear.Items.Add(year.ToString());
                }
                cmbTaxYear.SelectedItem = currentYear.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tax years: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTaxBrackets()
        {
            try
            {
                string taxYear = cmbTaxYear.SelectedItem?.ToString() ?? DateTime.Now.Year.ToString();
                string filingStatus = cmbFilingStatus.SelectedItem?.ToString() ?? "Single";

                string query = $@"
                    SELECT 
                        id,
                        bracket_order as 'Bracket',
                        FORMAT(income_from, 2) as 'Income From',
                        FORMAT(income_to, 2) as 'Income To',
                        tax_rate as 'Tax Rate (%)',
                        FORMAT(fixed_amount, 2) as 'Fixed Amount',
                        filing_status as 'Filing Status',
                        tax_year as 'Tax Year',
                        CASE WHEN is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status'
                    FROM tax_brackets
                    WHERE tax_year = '{taxYear}' AND filing_status = '{filingStatus}'
                    ORDER BY bracket_order";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvTaxBrackets.DataSource = dt;

                if (dgvTaxBrackets.Columns["id"] != null)
                    dgvTaxBrackets.Columns["id"].Visible = false;

                // Format currency columns
                FormatCurrencyColumns(dgvTaxBrackets, new[] { "Income From", "Income To", "Fixed Amount" });

                // Format percentage column
                if (dgvTaxBrackets.Columns["Tax Rate (%)"] != null)
                {
                    dgvTaxBrackets.Columns["Tax Rate (%)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvTaxBrackets.Columns["Tax Rate (%)"].DefaultCellStyle.Format = "N2";
                }

                UpdateBracketCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tax brackets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                string query = "SELECT DISTINCT department FROM employees WHERE department IS NOT NULL ORDER BY department";
                DataTable dt = UtilityHelper.GetDataSet(query);
                
                cmbDepartmentTax.Items.Clear();
                cmbDepartmentTax.Items.Add("All Departments");
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbDepartmentTax.Items.Add(row["department"].ToString());
                }
                
                cmbDepartmentTax.SelectedIndex = 0;
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
                cmbPayPeriodTax.Items.Clear();
                cmbPayPeriodTax.Items.AddRange(new string[] {
                    "Current Period",
                    "Previous Period",
                    "Custom Range",
                    "All Periods"
                });
                cmbPayPeriodTax.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pay periods: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEmployeeTax()
        {
            try
            {
                string query = @"
                    SELECT 
                        et.id,
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        e.employee_id as 'Employee ID',
                        e.department as 'Department',
                        FORMAT(et.gross_income, 2) as 'Gross Income',
                        FORMAT(et.taxable_income, 2) as 'Taxable Income',
                        FORMAT(et.tax_due, 2) as 'Tax Due',
                        FORMAT(et.withholding_tax, 2) as 'Withholding Tax',
                        FORMAT(et.net_tax, 2) as 'Net Tax',
                        et.pay_period as 'Pay Period',
                        et.tax_date as 'Date'
                    FROM employee_tax et
                INNER JOIN employees e ON et.employee_id = e.employee_id
                    WHERE et.tax_date >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH)
                    ORDER BY et.tax_date DESC, e.last_name, e.first_name";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvEmployeeTax.DataSource = dt;

                if (dgvEmployeeTax.Columns["id"] != null)
                    dgvEmployeeTax.Columns["id"].Visible = false;

                // Format currency columns
                FormatCurrencyColumns(dgvEmployeeTax, new[] {
                    "Gross Income", "Taxable Income", "Tax Due", "Withholding Tax", "Net Tax"
                });

                // Format date column
                if (dgvEmployeeTax.Columns["Date"] != null)
                {
                    dgvEmployeeTax.Columns["Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
                }

                UpdateEmployeeCount();
                UpdateTotalTaxAmount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employee tax: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTaxSettings()
        {
            try
            {
                string query = "SELECT * FROM tax_settings WHERE id = 1";
                DataTable dt = UtilityHelper.GetDataSet(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    nudWithholdingRate.Value = Convert.ToDecimal(row["withholding_rate"]);
                    nudMinimumWage.Value = Convert.ToDecimal(row["minimum_wage"]);
                    nudPersonalExemption.Value = Convert.ToDecimal(row["personal_exemption"]);
                    nudAdditionalExemption.Value = Convert.ToDecimal(row["additional_exemption"]);
                    chkAutoCalculateTax.Checked = Convert.ToBoolean(row["auto_calculate"]);
                    chkUseAnnualization.Checked = Convert.ToBoolean(row["use_annualization"]);
                    chkIncludeAllowancesInTax.Checked = Convert.ToBoolean(row["include_allowances"]);
                    chkIncludeBonusInTax.Checked = Convert.ToBoolean(row["include_bonus"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tax settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void UpdateBracketCount()
        {
            lblTotalBrackets.Text = $"Total Brackets: {dgvTaxBrackets.Rows.Count:N0}";
        }

        private void UpdateEmployeeCount()
        {
            lblTotalEmployees.Text = $"Total Employees: {dgvEmployeeTax.Rows.Count:N0}";
        }

        private void UpdateTotalTaxAmount()
        {
            try
            {
                decimal total = 0;
                foreach (DataGridViewRow row in dgvEmployeeTax.Rows)
                {
                    if (row.Cells["Net Tax"].Value != null)
                    {
                        string value = row.Cells["Net Tax"].Value.ToString().Replace(",", "");
                        if (decimal.TryParse(value, out decimal amount))
                        {
                            total += amount;
                        }
                    }
                }
                lblTotalTaxAmount.Text = $"Total Tax: ₱{total:N2}";
            }
            catch
            {
                lblTotalTaxAmount.Text = "Total Tax: ₱0.00";
            }
        }

        // Event handlers for Tax Brackets tab
        private void CmbTaxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTaxBrackets();
        }

        private void CmbFilingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTaxBrackets();
        }

        private void BtnAddBracket_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add Tax Bracket functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditBracket_Click(object sender, EventArgs e)
        {
            if (dgvTaxBrackets.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a tax bracket to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Edit Tax Bracket functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteBracket_Click(object sender, EventArgs e)
        {
            if (dgvTaxBrackets.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a tax bracket to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this tax bracket?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Delete Tax Bracket functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnImportBrackets_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Import Tax Brackets functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExportBrackets_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvTaxBrackets, "TaxBrackets");
        }

        private void DgvTaxBrackets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditBracket_Click(sender, e);
            }
        }

        // Event handlers for Employee Tax tab
        private void TxtSearchEmployeeTax_TextChanged(object sender, EventArgs e)
        {
            LoadEmployeeTax(); // Simplified - would include search filter in real implementation
        }

        private void CmbDepartmentTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployeeTax(); // Simplified - would include department filter in real implementation
        }

        private void CmbPayPeriodTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployeeTax(); // Simplified - would include pay period filter in real implementation
        }

        private void DtpTaxFromDate_ValueChanged(object sender, EventArgs e)
        {
            LoadEmployeeTax(); // Simplified - would include date filter in real implementation
        }

        private void DtpTaxToDate_ValueChanged(object sender, EventArgs e)
        {
            LoadEmployeeTax(); // Simplified - would include date filter in real implementation
        }

        private void BtnCalculateTax_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Calculate Employee Tax functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExportTax_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvEmployeeTax, "EmployeeTax");
        }

        // Event handlers for Settings tab
        private void BtnSaveTaxSettings_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
                    UPDATE tax_settings SET 
                        withholding_rate = @withholding_rate,
                        minimum_wage = @minimum_wage,
                        personal_exemption = @personal_exemption,
                        additional_exemption = @additional_exemption,
                        auto_calculate = @auto_calculate,
                        use_annualization = @use_annualization,
                        include_allowances = @include_allowances,
                        include_bonus = @include_bonus,
                        updated_date = NOW()
                    WHERE id = 1";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    DatabaseManager.CreateParameter("@withholding_rate", nudWithholdingRate.Value),
                    DatabaseManager.CreateParameter("@minimum_wage", nudMinimumWage.Value),
                    DatabaseManager.CreateParameter("@personal_exemption", nudPersonalExemption.Value),
                    DatabaseManager.CreateParameter("@additional_exemption", nudAdditionalExemption.Value),
                    DatabaseManager.CreateParameter("@auto_calculate", chkAutoCalculateTax.Checked),
                    DatabaseManager.CreateParameter("@use_annualization", chkUseAnnualization.Checked),
                    DatabaseManager.CreateParameter("@include_allowances", chkIncludeAllowancesInTax.Checked),
                    DatabaseManager.CreateParameter("@include_bonus", chkIncludeBonusInTax.Checked)
                };

                DatabaseManager.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Tax settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving tax settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnResetTaxSettings_Click(object sender, EventArgs e)
        {
            LoadTaxSettings();
            MessageBox.Show("Tax settings reset to last saved values.", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLoadTaxDefaults_Click(object sender, EventArgs e)
        {
            nudWithholdingRate.Value = 5.0m;
            nudMinimumWage.Value = 537m;
            nudPersonalExemption.Value = 50000m;
            nudAdditionalExemption.Value = 25000m;
            chkAutoCalculateTax.Checked = true;
            chkUseAnnualization.Checked = true;
            chkIncludeAllowancesInTax.Checked = true;
            chkIncludeBonusInTax.Checked = false;
            MessageBox.Show("Default tax settings loaded.", "Defaults", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Event handlers for Reports tab
        private void BtnGenerateTaxReport_Click(object sender, EventArgs e)
        {
            try
            {
                string reportType = cmbTaxReportType.SelectedItem?.ToString() ?? "Monthly Tax Summary";
                DateTime fromDate = dtpTaxReportFrom.Value;
                DateTime toDate = dtpTaxReportTo.Value;

                // Generate report based on selected type
                string query = GetTaxReportQuery(reportType, fromDate, toDate);
                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvTaxReports.DataSource = dt;

                // Format currency columns if present
                FormatTaxReportColumns(dgvTaxReports);

                MessageBox.Show($"{reportType} generated successfully!", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating tax report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportTaxReport_Click(object sender, EventArgs e)
        {
            string reportType = cmbTaxReportType.SelectedItem?.ToString() ?? "Tax_Report";
            ExportToCSV(dgvTaxReports, reportType.Replace(" ", "_"));
        }

        private void BtnPrintTaxReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Print Tax Report functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetTaxReportQuery(string reportType, DateTime fromDate, DateTime toDate)
        {
            switch (reportType)
            {
                case "Monthly Tax Summary":
                    return $@"
                        SELECT 
                            CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                            e.employee_id as 'Employee ID',
                            e.department as 'Department',
                            FORMAT(SUM(et.gross_income), 2) as 'Gross Income',
                            FORMAT(SUM(et.taxable_income), 2) as 'Taxable Income',
                            FORMAT(SUM(et.tax_due), 2) as 'Tax Due',
                            FORMAT(SUM(et.withholding_tax), 2) as 'Withholding Tax',
                            FORMAT(SUM(et.net_tax), 2) as 'Net Tax'
                        FROM employee_tax et
                        INNER JOIN employees e ON et.employee_id = e.employee_id
                        WHERE et.tax_date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'
                        GROUP BY e.employee_id, e.first_name, e.last_name, e.department
                        ORDER BY e.last_name, e.first_name";

                case "Tax by Department":
                      // COUNT(DISTINCT e.id) as 'Employee Count',
                    return $@"
                        SELECT 
                            e.department as 'Department',
                            FORMAT(SUM(et.gross_income), 2) as 'Total Gross Income',
                            FORMAT(SUM(et.taxable_income), 2) as 'Total Taxable Income',
                            FORMAT(SUM(et.tax_due), 2) as 'Total Tax Due',
                            FORMAT(SUM(et.withholding_tax), 2) as 'Total Withholding Tax',
                            FORMAT(SUM(et.net_tax), 2) as 'Total Net Tax'
                        FROM employee_tax et
                         INNER JOIN employees e ON et.employee_id = e.employee_id
                         WHERE et.tax_date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'
                         GROUP BY e.department
                         ORDER BY e.department";

                case "BIR Form 2316":
                    return $@"
                        SELECT 
                            CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                            e.employee_id as 'TIN',
                            e.department as 'Department',
                            FORMAT(SUM(et.gross_income), 2) as 'Gross Compensation',
                            FORMAT(SUM(et.taxable_income), 2) as 'Taxable Income',
                            FORMAT(SUM(et.tax_due), 2) as 'Tax Due',
                            FORMAT(SUM(et.withholding_tax), 2) as 'Tax Withheld',
                            YEAR(et.tax_date) as 'Year'
                        FROM tbl_employee_tax et
                        INNER JOIN tbl_employee e ON et.employee_id = e.employee_id
                        WHERE et.tax_date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'
                        GROUP BY e.id, e.first_name, e.last_name, e.employee_id, e.department, YEAR(et.tax_date)
                        ORDER BY e.last_name, e.first_name";

                default:
                    return $@"
                        SELECT 
                            CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                            e.employee_id as 'Employee ID',
                            FORMAT(et.gross_income, 2) as 'Gross Income',
                            FORMAT(et.taxable_income, 2) as 'Taxable Income',
                            FORMAT(et.net_tax, 2) as 'Net Tax',
                            et.tax_date as 'Date'
                        FROM employee_tax et
                         INNER JOIN employees e ON et.employee_id = e.employee_id
                         WHERE et.tax_date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'
                         ORDER BY et.tax_date DESC";
            }
        }

        private void FormatTaxReportColumns(DataGridView dgv)
        {
            string[] currencyColumns = {
                "Gross Income", "Taxable Income", "Tax Due", "Withholding Tax", "Net Tax",
                "Total Gross Income", "Total Taxable Income", "Total Tax Due", "Total Withholding Tax", "Total Net Tax",
                "Gross Compensation", "Tax Withheld"
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
