using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PayrollSystem
{
    public partial class frmRate : Form
    {
        private TabControl tabControl;
        private TabPage tabSalaryRates;
        private TabPage tabPayGrades;
        private TabPage tabAllowances;
        private TabPage tabDeductions;
        
        // Salary Rates Tab Controls
        private DataGridView dgvSalaryRates;
        private ComboBox cmbDepartment;
        private ComboBox cmbPosition;
        private TextBox txtSearchRates;
        private Button btnAddRate;
        private Button btnEditRate;
        private Button btnDeleteRate;
        private Button btnRefreshRates;
        private Label lblTotalRates;
        
        // Pay Grades Tab Controls
        private DataGridView dgvPayGrades;
        private Button btnAddGrade;
        private Button btnEditGrade;
        private Button btnDeleteGrade;
        private Button btnCopyGrade;
        private Label lblTotalGrades;
        
        // Allowances Tab Controls
        private DataGridView dgvAllowances;
        private Button btnAddAllowance;
        private Button btnEditAllowance;
        private Button btnDeleteAllowance;
        private CheckBox chkTaxableAllowances;
        private Label lblTotalAllowances;
        
        // Deductions Tab Controls
        private DataGridView dgvDeductions;
        private Button btnAddDeduction;
        private Button btnEditDeduction;
        private Button btnDeleteDeduction;
        private CheckBox chkMandatoryDeductions;
        private Label lblTotalDeductions;

        public frmRate()
        {
            InitializeComponent();
            LoadInitialData();
        }

        private void InitializeComponent()
        {
            this.Text = "Rate Management";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(900, 600);

            // Create tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // Create tabs
            tabSalaryRates = new TabPage("Salary Rates");
            tabPayGrades = new TabPage("Pay Grades");
            tabAllowances = new TabPage("Allowances");
            tabDeductions = new TabPage("Deductions");

            tabControl.TabPages.AddRange(new TabPage[] { tabSalaryRates, tabPayGrades, tabAllowances, tabDeductions });

            InitializeSalaryRatesTab();
            InitializePayGradesTab();
            InitializeAllowancesTab();
            InitializeDeductionsTab();

            this.Controls.Add(tabControl);
        }

        private void InitializeSalaryRatesTab()
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
                Text = "Salary Rate Management",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(200, 25)
            };

            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 50),
                Size = new Size(60, 23)
            };

            txtSearchRates = new TextBox
            {
                Location = new Point(85, 47),
                Size = new Size(150, 23)
            };
            txtSearchRates.SetPlaceholderText("Employee name or ID...");
            txtSearchRates.TextChanged += TxtSearchRates_TextChanged;

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

            Label lblPosition = new Label
            {
                Text = "Position:",
                Location = new Point(470, 50),
                Size = new Size(60, 23)
            };

            cmbPosition = new ComboBox
            {
                Location = new Point(535, 47),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPosition.SelectedIndexChanged += CmbPosition_SelectedIndexChanged;

            btnAddRate = new Button
            {
                Text = "Add Rate",
                Location = new Point(700, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddRate.Click += BtnAddRate_Click;

            btnEditRate = new Button
            {
                Text = "Edit",
                Location = new Point(790, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnEditRate.Click += BtnEditRate_Click;

            btnDeleteRate = new Button
            {
                Text = "Delete",
                Location = new Point(860, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteRate.Click += BtnDeleteRate_Click;

            btnRefreshRates = new Button
            {
                Text = "Refresh",
                Location = new Point(930, 15),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshRates.Click += BtnRefreshRates_Click;

            pnlTop.Controls.AddRange(new Control[] {
                lblTitle, lblSearch, txtSearchRates, lblDepartment, cmbDepartment,
                lblPosition, cmbPosition, btnAddRate, btnEditRate, btnDeleteRate, btnRefreshRates
            });

            // DataGridView for salary rates
            dgvSalaryRates = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvSalaryRates.CellDoubleClick += DgvSalaryRates_CellDoubleClick;

            // Bottom panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalRates = new Label
            {
                Text = "Total Rates: 0",
                Location = new Point(20, 10),
                Size = new Size(200, 23)
            };

            Button btnExportRates = new Button
            {
                Text = "Export",
                Location = new Point(700, 5),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportRates.Click += BtnExportRates_Click;

            Button btnCloseRates = new Button
            {
                Text = "Close",
                Location = new Point(780, 5),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCloseRates.Click += (s, e) => this.Close();

            pnlBottom.Controls.AddRange(new Control[] { lblTotalRates, btnExportRates, btnCloseRates });

            tabSalaryRates.Controls.AddRange(new Control[] { pnlTop, dgvSalaryRates, pnlBottom });
            UtilityHelper.ApplyLightMode(dgvSalaryRates);
        }

        private void InitializePayGradesTab()
        {
            // Top panel
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblTitle = new Label
            {
                Text = "Pay Grade Structure",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(200, 25)
            };

            btnAddGrade = new Button
            {
                Text = "Add Grade",
                Location = new Point(700, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddGrade.Click += BtnAddGrade_Click;

            btnEditGrade = new Button
            {
                Text = "Edit",
                Location = new Point(790, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnEditGrade.Click += BtnEditGrade_Click;

            btnDeleteGrade = new Button
            {
                Text = "Delete",
                Location = new Point(860, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteGrade.Click += BtnDeleteGrade_Click;

            btnCopyGrade = new Button
            {
                Text = "Copy",
                Location = new Point(930, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCopyGrade.Click += BtnCopyGrade_Click;

            pnlTop.Controls.AddRange(new Control[] { lblTitle, btnAddGrade, btnEditGrade, btnDeleteGrade, btnCopyGrade });

            // DataGridView for pay grades
            dgvPayGrades = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvPayGrades.CellDoubleClick += DgvPayGrades_CellDoubleClick;

            // Bottom panel
            Panel pnlBottomGrades = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalGrades = new Label
            {
                Text = "Total Grades: 0",
                Location = new Point(20, 10),
                Size = new Size(200, 23)
            };

            pnlBottomGrades.Controls.Add(lblTotalGrades);

            tabPayGrades.Controls.AddRange(new Control[] { pnlTop, dgvPayGrades, pnlBottomGrades });
            UtilityHelper.ApplyLightMode(dgvPayGrades);
        }

        private void InitializeAllowancesTab()
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
                Text = "Allowance Management",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(200, 25)
            };

            chkTaxableAllowances = new CheckBox
            {
                Text = "Show only taxable allowances",
                Location = new Point(20, 50),
                Size = new Size(200, 23)
            };
            chkTaxableAllowances.CheckedChanged += ChkTaxableAllowances_CheckedChanged;

            btnAddAllowance = new Button
            {
                Text = "Add Allowance",
                Location = new Point(700, 15),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddAllowance.Click += BtnAddAllowance_Click;

            btnEditAllowance = new Button
            {
                Text = "Edit",
                Location = new Point(810, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnEditAllowance.Click += BtnEditAllowance_Click;

            btnDeleteAllowance = new Button
            {
                Text = "Delete",
                Location = new Point(880, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteAllowance.Click += BtnDeleteAllowance_Click;

            pnlTop.Controls.AddRange(new Control[] { lblTitle, chkTaxableAllowances, btnAddAllowance, btnEditAllowance, btnDeleteAllowance });

            // DataGridView for allowances
            dgvAllowances = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvAllowances.CellDoubleClick += DgvAllowances_CellDoubleClick;

            // Bottom panel
            Panel pnlBottomAllowances = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalAllowances = new Label
            {
                Text = "Total Allowances: 0",
                Location = new Point(20, 10),
                Size = new Size(200, 23)
            };

            pnlBottomAllowances.Controls.Add(lblTotalAllowances);

            tabAllowances.Controls.AddRange(new Control[] { pnlTop, dgvAllowances, pnlBottomAllowances });
            UtilityHelper.ApplyLightMode(dgvAllowances);
        }

        private void InitializeDeductionsTab()
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
                Text = "Deduction Management",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(200, 25)
            };

            chkMandatoryDeductions = new CheckBox
            {
                Text = "Show only mandatory deductions",
                Location = new Point(20, 50),
                Size = new Size(220, 23)
            };
            chkMandatoryDeductions.CheckedChanged += ChkMandatoryDeductions_CheckedChanged;

            btnAddDeduction = new Button
            {
                Text = "Add Deduction",
                Location = new Point(700, 15),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddDeduction.Click += BtnAddDeduction_Click;

            btnEditDeduction = new Button
            {
                Text = "Edit",
                Location = new Point(810, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnEditDeduction.Click += BtnEditDeduction_Click;

            btnDeleteDeduction = new Button
            {
                Text = "Delete",
                Location = new Point(880, 15),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteDeduction.Click += BtnDeleteDeduction_Click;

            pnlTop.Controls.AddRange(new Control[] { lblTitle, chkMandatoryDeductions, btnAddDeduction, btnEditDeduction, btnDeleteDeduction });

            // DataGridView for deductions
            dgvDeductions = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvDeductions.CellDoubleClick += DgvDeductions_CellDoubleClick;

            // Bottom panel
            Panel pnlBottomDeductions = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalDeductions = new Label
            {
                Text = "Total Deductions: 0",
                Location = new Point(20, 10),
                Size = new Size(200, 23)
            };

            pnlBottomDeductions.Controls.Add(lblTotalDeductions);

            tabDeductions.Controls.AddRange(new Control[] { pnlTop, dgvDeductions, pnlBottomDeductions });
            UtilityHelper.ApplyLightMode(dgvDeductions);
        }

        private void LoadInitialData()
        {
            LoadDepartments();
            LoadPositions();
            LoadSalaryRates();
            LoadPayGrades();
            LoadAllowances();
            LoadDeductions();
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
                    cmbPosition.Items.Add(row["position"].ToString());
                }
                
                cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPositions()
        {
            try
            {
                string query = "SELECT DISTINCT position FROM employees WHERE position IS NOT NULL ORDER BY position";
                DataTable dt = UtilityHelper.GetDataSet(query);
                
                cmbPosition.Items.Clear();
                cmbPosition.Items.Add("All Positions");
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbPosition.Items.Add(row["position"].ToString());
                }
                
                cmbPosition.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading positions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSalaryRates()
        {
            try
            {
                string query = @"
                    SELECT 
                        sr.id,
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        e.employee_id as 'Employee ID',
                        e.department as 'Department',
                        e.position as 'Position',
                        FORMAT(sr.basic_salary, 2) as 'Basic Salary',
                        FORMAT(sr.hourly_rate, 2) as 'Hourly Rate',
                        FORMAT(sr.daily_rate, 2) as 'Daily Rate',
                        FORMAT(sr.overtime_rate, 2) as 'Overtime Rate',
                        sr.effective_date as 'Effective Date',
                        CASE WHEN sr.is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status'
                    FROM salary_rates sr
                INNER JOIN employees e ON sr.employee_id = e.employee_id
                    ORDER BY e.last_name, e.first_name";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvSalaryRates.DataSource = dt;

                if (dgvSalaryRates.Columns["id"] != null)
                    dgvSalaryRates.Columns["id"].Visible = false;

                // Format currency columns
                FormatCurrencyColumns(dgvSalaryRates, new[] { "Basic Salary", "Hourly Rate", "Daily Rate", "Overtime Rate" });
                
                // Format date column
                if (dgvSalaryRates.Columns["Effective Date"] != null)
                {
                    dgvSalaryRates.Columns["Effective Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
                }

                UpdateRateCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading salary rates: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPayGrades()
        {
            try
            {
                string query = @"
                    SELECT 
                        id,
                        grade_level as 'Grade Level',
                        grade_name as 'Grade Name',
                        FORMAT(min_salary, 2) as 'Minimum Salary',
                        FORMAT(max_salary, 2) as 'Maximum Salary',
                        FORMAT(step_increment, 2) as 'Step Increment',
                        total_steps as 'Total Steps',
                        description as 'Description',
                        CASE WHEN is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status'
                    FROM pay_grades
                    ORDER BY grade_level";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvPayGrades.DataSource = dt;

                if (dgvPayGrades.Columns["id"] != null)
                    dgvPayGrades.Columns["id"].Visible = false;

                FormatCurrencyColumns(dgvPayGrades, new[] { "Minimum Salary", "Maximum Salary", "Step Increment" });

                UpdateGradeCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pay grades: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllowances()
        {
            try
            {
                string whereClause = "";
                if (chkTaxableAllowances.Checked)
                {
                    whereClause = "WHERE is_taxable = 1";
                }

                string query = $@"
                    SELECT 
                        id,
                        allowance_name as 'Allowance Name',
                        allowance_type as 'Type',
                        FORMAT(default_amount, 2) as 'Default Amount',
                        calculation_method as 'Calculation Method',
                        CASE WHEN is_taxable = 1 THEN 'Yes' ELSE 'No' END as 'Taxable',
                        CASE WHEN is_mandatory = 1 THEN 'Yes' ELSE 'No' END as 'Mandatory',
                        CASE WHEN is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status',
                        description as 'Description'
                    FROM allowances
                    {whereClause}
                    ORDER BY allowance_name";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvAllowances.DataSource = dt;

                if (dgvAllowances.Columns["id"] != null)
                    dgvAllowances.Columns["id"].Visible = false;

                FormatCurrencyColumns(dgvAllowances, new[] { "Default Amount" });

                UpdateAllowanceCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading allowances: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDeductions()
        {
            try
            {
                string whereClause = "";
                if (chkMandatoryDeductions.Checked)
                {
                    whereClause = "WHERE is_mandatory = 1";
                }

                string query = $@"
                    SELECT 
                        id,
                        deduction_name as 'Deduction Name',
                        deduction_type as 'Type',
                        FORMAT(default_amount, 2) as 'Default Amount',
                        calculation_method as 'Calculation Method',
                        CASE WHEN is_mandatory = 1 THEN 'Yes' ELSE 'No' END as 'Mandatory',
                        CASE WHEN affects_tax = 1 THEN 'Yes' ELSE 'No' END as 'Affects Tax',
                        CASE WHEN is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status',
                        description as 'Description'
                    FROM deductions
                    {whereClause}
                    ORDER BY deduction_name";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvDeductions.DataSource = dt;

                if (dgvDeductions.Columns["id"] != null)
                    dgvDeductions.Columns["id"].Visible = false;

                FormatCurrencyColumns(dgvDeductions, new[] { "Default Amount" });

                UpdateDeductionCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading deductions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void UpdateRateCount()
        {
            lblTotalRates.Text = $"Total Rates: {dgvSalaryRates.Rows.Count:N0}";
        }

        private void UpdateGradeCount()
        {
            lblTotalGrades.Text = $"Total Grades: {dgvPayGrades.Rows.Count:N0}";
        }

        private void UpdateAllowanceCount()
        {
            lblTotalAllowances.Text = $"Total Allowances: {dgvAllowances.Rows.Count:N0}";
        }

        private void UpdateDeductionCount()
        {
            lblTotalDeductions.Text = $"Total Deductions: {dgvDeductions.Rows.Count:N0}";
        }

        // Event handlers for Salary Rates tab
        private void TxtSearchRates_TextChanged(object sender, EventArgs e)
        {
            LoadSalaryRates(); // Simplified - would include search filter in real implementation
        }

        private void CmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSalaryRates(); // Simplified - would include department filter in real implementation
        }

        private void CmbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSalaryRates(); // Simplified - would include position filter in real implementation
        }

        private void BtnAddRate_Click(object sender, EventArgs e)
        {
            try
            {
                Form addRateForm = new Form
                {
                    Text = "Add New Salary Rate",
                    Size = new Size(400, 350),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                ComboBox cmbPositionAdd = new ComboBox
                {
                    Location = new Point(150, 20),
                    Size = new Size(200, 23),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                // Initialize positions in the dropdown
                cmbPositionAdd.Items.Clear();
                cmbPositionAdd.Items.Add("All Positions");
                
                string query = "SELECT DISTINCT position FROM employees WHERE position IS NOT NULL ORDER BY position";
                DataTable dt = UtilityHelper.GetDataSet(query);
                foreach (DataRow row in dt.Rows)
                {
                    cmbPositionAdd.Items.Add(row["position"].ToString());
                }
                cmbPositionAdd.SelectedIndex = 0;

                NumericUpDown nudBasicRate = new NumericUpDown
                {
                    Location = new Point(150, 60),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999999.99m,
                    Minimum = 0,
                    Value = 0
                };

                NumericUpDown nudOvertimeRate = new NumericUpDown
                {
                    Location = new Point(150, 100),
                    Size = new Size(200, 23),
                    DecimalPlaces = 2,
                    Maximum = 999.99m,
                    Minimum = 0,
                    Value = 1.25m
                };

                DateTimePicker dtpEffectiveDate = new DateTimePicker
                {
                    Location = new Point(150, 140),
                    Size = new Size(200, 23),
                    Format = DateTimePickerFormat.Short,
                    Value = DateTime.Now
                };

                CheckBox chkActive = new CheckBox
                {
                    Text = "Active",
                    Location = new Point(150, 180),
                    Checked = true
                };

                Button btnSave = new Button
                {
                    Text = "Save",
                    Location = new Point(150, 220),
                    Size = new Size(90, 30)
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Location = new Point(260, 220),
                    Size = new Size(90, 30)
                };

                btnSave.Click += (s, ev) =>
                {
                    try
                    {
                        if (cmbPositionAdd.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select a position.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string query = @"
                            INSERT INTO salary_rates 
                            (position_id, basic_rate, overtime_rate, effective_date, is_active)
                            VALUES 
                            (@positionId, @basicRate, @overtimeRate, @effectiveDate, @isActive)";

                        var parameters = new MySqlConnector.MySqlParameter[]
                        {
                            new MySqlConnector.MySqlParameter("@positionId", cmbPositionAdd.SelectedValue),
                            new MySqlConnector.MySqlParameter("@basicRate", nudBasicRate.Value),
                            new MySqlConnector.MySqlParameter("@overtimeRate", nudOvertimeRate.Value),
                            new MySqlConnector.MySqlParameter("@effectiveDate", dtpEffectiveDate.Value),
                            new MySqlConnector.MySqlParameter("@isActive", chkActive.Checked)
                        };

                        DatabaseManager.ExecuteNonQuery(query, parameters);
                        MessageBox.Show("Salary rate added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        addRateForm.Close();
                        LoadSalaryRates();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding salary rate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnCancel.Click += (s, ev) => addRateForm.Close();

                addRateForm.Controls.AddRange(new Control[]
                {
                    new Label { Text = "Position:", Location = new Point(20, 23), Size = new Size(120, 23) },
                    cmbPositionAdd,
                    new Label { Text = "Basic Rate:", Location = new Point(20, 63), Size = new Size(120, 23) },
                    nudBasicRate,
                    new Label { Text = "Overtime Rate:", Location = new Point(20, 103), Size = new Size(120, 23) },
                    nudOvertimeRate,
                    new Label { Text = "Effective Date:", Location = new Point(20, 143), Size = new Size(120, 23) },
                    dtpEffectiveDate,
                    chkActive,
                    btnSave,
                    btnCancel
                });

                addRateForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditRate_Click(object sender, EventArgs e)
        {
            if (dgvSalaryRates.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a salary rate to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Edit Salary Rate functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteRate_Click(object sender, EventArgs e)
        {
            if (dgvSalaryRates.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a salary rate to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this salary rate?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Delete Salary Rate functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRefreshRates_Click(object sender, EventArgs e)
        {
            LoadSalaryRates();
        }

        private void DgvSalaryRates_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditRate_Click(sender, e);
            }
        }

        private void BtnExportRates_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvSalaryRates, "SalaryRates");
        }

        // Event handlers for Pay Grades tab
        private void BtnAddGrade_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add Pay Grade functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditGrade_Click(object sender, EventArgs e)
        {
            if (dgvPayGrades.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a pay grade to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Edit Pay Grade functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteGrade_Click(object sender, EventArgs e)
        {
            if (dgvPayGrades.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a pay grade to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this pay grade?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Delete Pay Grade functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCopyGrade_Click(object sender, EventArgs e)
        {
            if (dgvPayGrades.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a pay grade to copy.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Copy Pay Grade functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvPayGrades_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditGrade_Click(sender, e);
            }
        }

        // Event handlers for Allowances tab
        private void ChkTaxableAllowances_CheckedChanged(object sender, EventArgs e)
        {
            LoadAllowances();
        }

        private void BtnAddAllowance_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add Allowance functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditAllowance_Click(object sender, EventArgs e)
        {
            if (dgvAllowances.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an allowance to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Edit Allowance functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteAllowance_Click(object sender, EventArgs e)
        {
            if (dgvAllowances.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an allowance to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this allowance?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Delete Allowance functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DgvAllowances_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditAllowance_Click(sender, e);
            }
        }

        // Event handlers for Deductions tab
        private void ChkMandatoryDeductions_CheckedChanged(object sender, EventArgs e)
        {
            LoadDeductions();
        }

        private void BtnAddDeduction_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add Deduction functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditDeduction_Click(object sender, EventArgs e)
        {
            if (dgvDeductions.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a deduction to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Edit Deduction functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteDeduction_Click(object sender, EventArgs e)
        {
            if (dgvDeductions.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a deduction to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this deduction?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Delete Deduction functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DgvDeductions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditDeduction_Click(sender, e);
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
