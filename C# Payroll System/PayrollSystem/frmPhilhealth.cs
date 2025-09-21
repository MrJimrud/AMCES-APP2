using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PayrollSystem
{
    public partial class frmPhilhealth : Form
    {
        private TabControl tabControl;
        private TabPage tabRates;
        private TabPage tabContributions;
        private TabPage tabSettings;
        
        // Rates Tab Controls
        private DataGridView dgvRates;
        private Button btnAddRate;
        private Button btnEditRate;
        private Button btnDeleteRate;
        private Button btnActivateRate;
        private Label lblTotalRates;
        
        // Contributions Tab Controls
        private DataGridView dgvContributions;
        private TextBox txtSearchContributions;
        private ComboBox cmbYear;
        private ComboBox cmbMonth;
        private Button btnCalculateContributions;
        private Button btnExportContributions;
        private Label lblTotalContributions;
        
        // Settings Tab Controls
        private NumericUpDown nudMinSalary;
        private NumericUpDown nudMaxSalary;
        private NumericUpDown nudEmployeeRate;
        private NumericUpDown nudEmployerRate;
        private CheckBox chkAutoCalculate;
        private CheckBox chkIncludeAllowances;
        private Button btnSaveSettings;
        private Button btnResetSettings;

        public frmPhilhealth()
        {
            InitializeComponent();
            LoadInitialData();
        }

        private void InitializeComponent()
        {
            this.Text = "PhilHealth Management";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(800, 500);

            // Create tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // Create tabs
            tabRates = new TabPage("Contribution Rates");
            tabContributions = new TabPage("Employee Contributions");
            tabSettings = new TabPage("Settings");

            tabControl.TabPages.AddRange(new TabPage[] { tabRates, tabContributions, tabSettings });

            InitializeRatesTab();
            InitializeContributionsTab();
            InitializeSettingsTab();

            this.Controls.Add(tabControl);
        }

        private void InitializeRatesTab()
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
                Text = "PhilHealth Contribution Rate Schedule",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(300, 25)
            };

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

            pnlTop.Controls.AddRange(new Control[] { lblTitle, btnAddRate, btnEditRate, btnDeleteRate });

            // DataGridView for rates
            dgvRates = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvRates.CellDoubleClick += DgvRates_CellDoubleClick;

            // Bottom panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalRates = new Label
            {
                Text = "Total Rates: 0",
                Location = new Point(20, 15),
                Size = new Size(200, 23)
            };

            btnActivateRate = new Button
            {
                Text = "Activate Selected",
                Location = new Point(700, 10),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnActivateRate.Click += BtnActivateRate_Click;

            Button btnCloseRates = new Button
            {
                Text = "Close",
                Location = new Point(830, 10),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCloseRates.Click += (s, e) => this.Close();

            pnlBottom.Controls.AddRange(new Control[] { lblTotalRates, btnActivateRate, btnCloseRates });

            tabRates.Controls.AddRange(new Control[] { pnlTop, dgvRates, pnlBottom });
            UtilityHelper.ApplyLightMode(dgvRates);
        }

        private void InitializeContributionsTab()
        {
            // Top panel
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 25),
                Size = new Size(60, 23)
            };

            txtSearchContributions = new TextBox
            {
                Location = new Point(85, 22),
                Size = new Size(180, 23)
            };
            txtSearchContributions.SetPlaceholderText("Employee name or ID...");
            txtSearchContributions.TextChanged += TxtSearchContributions_TextChanged;

            Label lblYear = new Label
            {
                Text = "Year:",
                Location = new Point(280, 25),
                Size = new Size(40, 23)
            };

            cmbYear = new ComboBox
            {
                Location = new Point(325, 22),
                Size = new Size(80, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Populate years
            for (int year = DateTime.Now.Year - 5; year <= DateTime.Now.Year + 1; year++)
            {
                cmbYear.Items.Add(year);
            }
            cmbYear.SelectedItem = DateTime.Now.Year;
            cmbYear.SelectedIndexChanged += CmbYear_SelectedIndexChanged;

            Label lblMonth = new Label
            {
                Text = "Month:",
                Location = new Point(420, 25),
                Size = new Size(50, 23)
            };

            cmbMonth = new ComboBox
            {
                Location = new Point(475, 22),
                Size = new Size(100, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMonth.Items.AddRange(new[] { "All", "January", "February", "March", "April", "May", "June", 
                "July", "August", "September", "October", "November", "December" });
            cmbMonth.SelectedIndex = 0;
            cmbMonth.SelectedIndexChanged += CmbMonth_SelectedIndexChanged;

            btnCalculateContributions = new Button
            {
                Text = "Calculate",
                Location = new Point(600, 20),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCalculateContributions.Click += BtnCalculateContributions_Click;

            btnExportContributions = new Button
            {
                Text = "Export",
                Location = new Point(690, 20),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportContributions.Click += BtnExportContributions_Click;

            pnlTop.Controls.AddRange(new Control[] { 
                lblSearch, txtSearchContributions, lblYear, cmbYear, lblMonth, cmbMonth,
                btnCalculateContributions, btnExportContributions
            });

            // DataGridView for contributions
            dgvContributions = new DataGridView
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
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalContributions = new Label
            {
                Text = "Total Contributions: ₱0.00",
                Location = new Point(20, 10),
                Size = new Size(300, 23)
            };

            pnlBottom.Controls.Add(lblTotalContributions);

            tabContributions.Controls.AddRange(new Control[] { pnlTop, dgvContributions, pnlBottom });
            UtilityHelper.ApplyLightMode(dgvContributions);
        }

        private void InitializeSettingsTab()
        {
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            GroupBox grpSalaryRange = new GroupBox
            {
                Text = "Salary Range for Contribution",
                Location = new Point(30, 30),
                Size = new Size(400, 120),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblMinSalary = new Label
            {
                Text = "Minimum Salary:",
                Location = new Point(20, 35),
                Size = new Size(120, 23)
            };

            nudMinSalary = new NumericUpDown
            {
                Location = new Point(150, 32),
                Size = new Size(120, 23),
                DecimalPlaces = 2,
                Maximum = 999999.99m,
                Minimum = 0,
                Value = 10000
            };

            Label lblMaxSalary = new Label
            {
                Text = "Maximum Salary:",
                Location = new Point(20, 70),
                Size = new Size(120, 23)
            };

            nudMaxSalary = new NumericUpDown
            {
                Location = new Point(150, 67),
                Size = new Size(120, 23),
                DecimalPlaces = 2,
                Maximum = 999999.99m,
                Minimum = 0,
                Value = 80000
            };

            grpSalaryRange.Controls.AddRange(new Control[] { lblMinSalary, nudMinSalary, lblMaxSalary, nudMaxSalary });

            GroupBox grpContributionRates = new GroupBox
            {
                Text = "Contribution Rates (%)",
                Location = new Point(450, 30),
                Size = new Size(400, 120),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblEmployeeRate = new Label
            {
                Text = "Employee Rate:",
                Location = new Point(20, 35),
                Size = new Size(120, 23)
            };

            nudEmployeeRate = new NumericUpDown
            {
                Location = new Point(150, 32),
                Size = new Size(120, 23),
                DecimalPlaces = 2,
                Maximum = 100,
                Minimum = 0,
                Value = 4.5m
            };

            Label lblEmployerRate = new Label
            {
                Text = "Employer Rate:",
                Location = new Point(20, 70),
                Size = new Size(120, 23)
            };

            nudEmployerRate = new NumericUpDown
            {
                Location = new Point(150, 67),
                Size = new Size(120, 23),
                DecimalPlaces = 2,
                Maximum = 100,
                Minimum = 0,
                Value = 4.5m
            };

            grpContributionRates.Controls.AddRange(new Control[] { lblEmployeeRate, nudEmployeeRate, lblEmployerRate, nudEmployerRate });

            GroupBox grpOptions = new GroupBox
            {
                Text = "Calculation Options",
                Location = new Point(30, 170),
                Size = new Size(400, 100),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            chkAutoCalculate = new CheckBox
            {
                Text = "Auto-calculate contributions during payroll",
                Location = new Point(20, 35),
                Size = new Size(350, 23),
                Checked = true
            };

            chkIncludeAllowances = new CheckBox
            {
                Text = "Include allowances in contribution calculation",
                Location = new Point(20, 65),
                Size = new Size(350, 23),
                Checked = false
            };

            grpOptions.Controls.AddRange(new Control[] { chkAutoCalculate, chkIncludeAllowances });

            // Buttons
            btnSaveSettings = new Button
            {
                Text = "Save Settings",
                Location = new Point(450, 200),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSaveSettings.Click += BtnSaveSettings_Click;

            btnResetSettings = new Button
            {
                Text = "Reset to Default",
                Location = new Point(580, 200),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnResetSettings.Click += BtnResetSettings_Click;

            pnlMain.Controls.AddRange(new Control[] { grpSalaryRange, grpContributionRates, grpOptions, btnSaveSettings, btnResetSettings });
            tabSettings.Controls.Add(pnlMain);
        }

        private void LoadInitialData()
        {
            LoadPhilhealthRates();
            LoadPhilhealthContributions();
            LoadPhilhealthSettings();
        }

        private void LoadPhilhealthRates()
        {
            try
            {
                string query = @"
                    SELECT 
                        id,
                        effective_date as 'Effective Date',
                        FORMAT(min_salary, 2) as 'Minimum Salary',
                        FORMAT(max_salary, 2) as 'Maximum Salary',
                        CONCAT(employee_rate, '%') as 'Employee Rate',
                        CONCAT(employer_rate, '%') as 'Employer Rate',
                        CASE WHEN is_active = 1 THEN 'Active' ELSE 'Inactive' END as 'Status',
                        created_date as 'Created Date'
                    FROM philhealth_rates
                    ORDER BY effective_date DESC";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvRates.DataSource = dt;

                if (dgvRates.Columns["id"] != null)
                    dgvRates.Columns["id"].Visible = false;

                // Format columns
                FormatCurrencyColumns(dgvRates, new[] { "Minimum Salary", "Maximum Salary" });
                FormatDateColumns(dgvRates, new[] { "Effective Date", "Created Date" });

                UpdateRateCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PhilHealth rates: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPhilhealthContributions()
        {
            try
            {
                string query = @"
                    SELECT 
                        pc.id,
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        e.employee_id as 'Employee ID',
                        FORMAT(pc.basic_salary, 2) as 'Basic Salary',
                        FORMAT(pc.employee_contribution, 2) as 'Employee Contribution',
                        FORMAT(pc.employer_contribution, 2) as 'Employer Contribution',
                        FORMAT(pc.total_contribution, 2) as 'Total Contribution',
                        MONTHNAME(pc.contribution_date) as 'Month',
                        YEAR(pc.contribution_date) as 'Year',
                        pc.contribution_date as 'Date'
                    FROM philihealth_contributions pc
                INNER JOIN employees e ON pc.employee_id = e.employee_id
                    WHERE YEAR(pc.contribution_date) = YEAR(CURDATE())
                    ORDER BY pc.contribution_date DESC";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvContributions.DataSource = dt;

                if (dgvContributions.Columns["id"] != null)
                    dgvContributions.Columns["id"].Visible = false;
                if (dgvContributions.Columns["Date"] != null)
                    dgvContributions.Columns["Date"].Visible = false;

                FormatCurrencyColumns(dgvContributions, new[] { "Basic Salary", "Employee Contribution", "Employer Contribution", "Total Contribution" });

                UpdateContributionTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PhilHealth contributions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPhilhealthSettings()
        {
            try
            {
                string query = "SELECT * FROM philhealth_settings WHERE id = 1";
                DataTable dt = UtilityHelper.GetDataSet(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    nudMinSalary.Value = Convert.ToDecimal(row["min_salary"]);
                    nudMaxSalary.Value = Convert.ToDecimal(row["max_salary"]);
                    nudEmployeeRate.Value = Convert.ToDecimal(row["employee_rate"]);
                    nudEmployerRate.Value = Convert.ToDecimal(row["employer_rate"]);
                    chkAutoCalculate.Checked = Convert.ToBoolean(row["auto_calculate"]);
                    chkIncludeAllowances.Checked = Convert.ToBoolean(row["include_allowances"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PhilHealth settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void FormatDateColumns(DataGridView dgv, string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns[columnName] != null)
                {
                    dgv.Columns[columnName].DefaultCellStyle.Format = "MM/dd/yyyy";
                }
            }
        }

        private void UpdateRateCount()
        {
            lblTotalRates.Text = $"Total Rates: {dgvRates.Rows.Count:N0}";
        }

        private void UpdateContributionTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvContributions.Rows)
            {
                if (row.Cells["Total Contribution"].Value != null)
                {
                    string value = row.Cells["Total Contribution"].Value.ToString().Replace("₱", "").Replace(",", "");
                    if (decimal.TryParse(value, out decimal amount))
                    {
                        total += amount;
                    }
                }
            }
            lblTotalContributions.Text = $"Total Contributions: ₱{total:N2}";
        }

        // Event handlers
        private void BtnAddRate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add PhilHealth Rate functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditRate_Click(object sender, EventArgs e)
        {
            if (dgvRates.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a rate to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Edit PhilHealth Rate functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteRate_Click(object sender, EventArgs e)
        {
            if (dgvRates.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a rate to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this rate?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Delete PhilHealth Rate functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnActivateRate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Activate PhilHealth Rate functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvRates_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditRate_Click(sender, e);
            }
        }

        private void TxtSearchContributions_TextChanged(object sender, EventArgs e)
        {
            LoadPhilhealthContributions(); // Simplified
        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPhilhealthContributions(); // Simplified
        }

        private void CmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPhilhealthContributions(); // Simplified
        }

        private void BtnCalculateContributions_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Calculate PhilHealth Contributions functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExportContributions_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv";
                    sfd.FileName = $"PhilHealthContributions_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder csv = new StringBuilder();

                        // Add headers
                        var headers = new List<string>();
                        for (int i = 1; i < dgvContributions.Columns.Count; i++) // Skip ID column
                        {
                            if (dgvContributions.Columns[i].Visible)
                                headers.Add(dgvContributions.Columns[i].HeaderText);
                        }
                        csv.AppendLine(string.Join(",", headers));

                        // Add data
                        foreach (DataGridViewRow row in dgvContributions.Rows)
                        {
                            var values = new List<string>();
                            for (int i = 1; i < dgvContributions.Columns.Count; i++) // Skip ID column
                            {
                                if (dgvContributions.Columns[i].Visible)
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

        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                string query = $@"
                    INSERT INTO philhealth_settings (id, min_salary, max_salary, employee_rate, employer_rate, auto_calculate, include_allowances, modified_date)
                    VALUES (1, {nudMinSalary.Value}, {nudMaxSalary.Value}, {nudEmployeeRate.Value}, {nudEmployerRate.Value}, 
                            {(chkAutoCalculate.Checked ? 1 : 0)}, {(chkIncludeAllowances.Checked ? 1 : 0)}, NOW())
                    ON DUPLICATE KEY UPDATE
                    min_salary = {nudMinSalary.Value},
                    max_salary = {nudMaxSalary.Value},
                    employee_rate = {nudEmployeeRate.Value},
                    employer_rate = {nudEmployerRate.Value},
                    auto_calculate = {(chkAutoCalculate.Checked ? 1 : 0)},
                    include_allowances = {(chkIncludeAllowances.Checked ? 1 : 0)},
                    modified_date = NOW()";

                DatabaseManager.ExecuteNonQuery(query);

                MessageBox.Show("PhilHealth settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnResetSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset to default settings?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                nudMinSalary.Value = 10000;
                nudMaxSalary.Value = 80000;
                nudEmployeeRate.Value = 4.5m;
                nudEmployerRate.Value = 4.5m;
                chkAutoCalculate.Checked = true;
                chkIncludeAllowances.Checked = false;
            }
        }
    }
}
