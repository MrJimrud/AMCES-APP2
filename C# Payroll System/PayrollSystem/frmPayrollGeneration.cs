using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmPayrollGeneration : Form
    {
        private int selectedPeriodId = 0;
        private bool isGenerating = false;

        public frmPayrollGeneration()
        {
            InitializeComponent();
            this.Load += FrmPayrollGeneration_Load;
        }

        private void FrmPayrollGeneration_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPayrollPeriods();
                SetButtonStates();
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
                        CONCAT(period_name, ' (', DATE_FORMAT(start_date, '%Y-%m-%d'), ' to ', DATE_FORMAT(end_date, '%Y-%m-%d'), ')') as period_display,
                        start_date,
                        end_date,
                        payroll_date,
                        status
                    FROM payroll_periods 
                    WHERE status = 'Active'
                    ORDER BY start_date DESC";

                DataTable dt = DatabaseManager.GetDataTable(query);
                
                cmbPeriod.DisplayMember = "period_display";
                cmbPeriod.ValueMember = "period_id";
                cmbPeriod.DataSource = dt;
                
                if (cmbPeriod.Items.Count > 0)
                    cmbPeriod.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payroll periods: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPeriod.SelectedValue != null)
                {
                    selectedPeriodId = Convert.ToInt32(cmbPeriod.SelectedValue);
                    LoadPeriodInfo();
                    LoadEmployees();
                    LoadExistingPayroll();
                    SetButtonStates();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading period data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPeriodInfo()
        {
            try
            {
                DataRowView row = (DataRowView)cmbPeriod.SelectedItem;
                if (row != null)
                {
                    DateTime startDate = Convert.ToDateTime(row["start_date"]);
                    DateTime endDate = Convert.ToDateTime(row["end_date"]);
                    DateTime payrollDate = Convert.ToDateTime(row["payroll_date"]);
                    
                    lblPeriodInfo.Text = $"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd} | Payroll Date: {payrollDate:yyyy-MM-dd}";
                }
            }
            catch (Exception ex)
            {
                lblPeriodInfo.Text = "Error loading period information";
                MessageBox.Show($"Error loading period information: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEmployees()
        {
            try
            {
                string query = @"
                    SELECT 
                        e.employee_id,
                        CONCAT(e.first_name, ' ', e.last_name) as full_name,
                        d.department_name,
                        jt.job_title,
                        e.basic_salary,
                        e.status
                    FROM employees e
                    LEFT JOIN departments d ON e.department_id = d.department_id
                    LEFT JOIN job_titles jt ON e.job_title_id = jt.job_title_id
                    WHERE e.status = 'Active'
                    ORDER BY e.employee_id";

                DataTable dt = DatabaseManager.GetDataTable(query);
                
                // Check if we have data
                if (dt == null || dt.Rows.Count == 0)
                {
                    // Create a sample employee if no employees exist
                    dt = new DataTable();
                    dt.Columns.Add("employee_id", typeof(string));
                    dt.Columns.Add("full_name", typeof(string));
                    dt.Columns.Add("department_name", typeof(string));
                    dt.Columns.Add("job_title", typeof(string));
                    dt.Columns.Add("basic_salary", typeof(decimal));
                    dt.Columns.Add("status", typeof(string));
                    
                    // Add a sample employee
                    DataRow row = dt.NewRow();
                    row["employee_id"] = "EMP001";
                    row["full_name"] = "Sample Employee";
                    row["department_name"] = "Sample Department";
                    row["job_title"] = "Sample Position";
                    row["basic_salary"] = 20000.00m;
                    row["status"] = "Active";
                    dt.Rows.Add(row);
                }
                
                dgvEmployees.DataSource = dt;

                // Format columns
                if (dgvEmployees.Columns.Count > 0)
                {
                    dgvEmployees.Columns["employee_id"].Visible = false;
                    // Employee ID is used as identifier
                    dgvEmployees.Columns["full_name"].HeaderText = "Employee Name";
                    dgvEmployees.Columns["department_name"].HeaderText = "Department";
                    dgvEmployees.Columns["job_title"].HeaderText = "Job Title";
                    dgvEmployees.Columns["basic_salary"].HeaderText = "Basic Salary";
                    dgvEmployees.Columns["status"].HeaderText = "Status";

                    dgvEmployees.Columns["basic_salary"].DefaultCellStyle.Format = "₱#,##0.00";
                    dgvEmployees.Columns["basic_salary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExistingPayroll()
        {
            try
            {
                string query = @"
                    SELECT 
                        p.payroll_id,
                        e.employee_id,
                        CONCAT(e.first_name, ' ', e.last_name) as full_name,
                        p.basic_pay,
                        p.overtime_pay,
                        p.allowances,
                        p.gross_pay,
                        p.sss_deduction,
                        p.philhealth_deduction,
                        p.pagibig_deduction,
                        p.tax_deduction,
                        p.other_deductions,
                        p.total_deductions,
                        p.net_pay,
                        p.status
                    FROM payroll_details p
                    INNER JOIN employees e ON p.employee_id = e.employee_id
                    WHERE p.period_id = @period_id
                    ORDER BY e.employee_id";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    DatabaseManager.CreateParameter("@period_id", selectedPeriodId)
                };
                DataTable dt = DatabaseManager.GetDataTable(query, (MySqlParameter[])parameters);
                dgvPayroll.DataSource = dt;

                // Format columns
                if (dgvPayroll.Columns.Count > 0)
                {
                    dgvPayroll.Columns["payroll_id"].Visible = false;
                    dgvPayroll.Columns["employee_id"].HeaderText = "ID";
                    dgvPayroll.Columns["full_name"].HeaderText = "Employee";
                    dgvPayroll.Columns["basic_pay"].HeaderText = "Basic";
                    dgvPayroll.Columns["overtime_pay"].HeaderText = "OT";
                    dgvPayroll.Columns["allowances"].HeaderText = "Allow.";
                    dgvPayroll.Columns["gross_pay"].HeaderText = "Gross";
                    dgvPayroll.Columns["sss_deduction"].HeaderText = "SSS";
                    dgvPayroll.Columns["philhealth_deduction"].HeaderText = "PhilHealth";
                    dgvPayroll.Columns["pagibig_deduction"].HeaderText = "Pag-IBIG";
                    dgvPayroll.Columns["tax_deduction"].HeaderText = "Tax";
                    dgvPayroll.Columns["other_deductions"].HeaderText = "Others";
                    dgvPayroll.Columns["total_deductions"].HeaderText = "Total Ded.";
                    dgvPayroll.Columns["net_pay"].HeaderText = "Net Pay";
                    dgvPayroll.Columns["status"].HeaderText = "Status";

                    // Format currency columns
                    string[] currencyColumns = { "basic_pay", "overtime_pay", "allowances", "gross_pay", 
                                                "sss_deduction", "philhealth_deduction", "pagibig_deduction", 
                                                "tax_deduction", "other_deductions", "total_deductions", "net_pay" };
                    
                    foreach (string col in currencyColumns)
                    {
                        if (dgvPayroll.Columns[col] != null)
                        {
                            dgvPayroll.Columns[col].DefaultCellStyle.Format = "₱#,##0.00";
                            dgvPayroll.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                }

                UpdateSummary(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading existing payroll: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary(DataTable payrollData)
        {
            try
            {
                if (payrollData.Rows.Count > 0)
                {
                    decimal totalGross = 0;
                    decimal totalDeductions = 0;
                    decimal totalNet = 0;

                    foreach (DataRow row in payrollData.Rows)
                    {
                        totalGross += Convert.ToDecimal(row["gross_pay"]);
                        totalDeductions += Convert.ToDecimal(row["total_deductions"]);
                        totalNet += Convert.ToDecimal(row["net_pay"]);
                    }

                    lblTotalEmployees.Text = $"Total Employees: {payrollData.Rows.Count}";
                    lblTotalGross.Text = $"Total Gross: ₱{totalGross:N2}";
                    lblTotalDeductions.Text = $"Total Deductions: ₱{totalDeductions:N2}";
                    lblTotalNet.Text = $"Total Net: ₱{totalNet:N2}";
                }
                else
                {
                    lblTotalEmployees.Text = "Total Employees: 0";
                    lblTotalGross.Text = "Total Gross: ₱0.00";
                    lblTotalDeductions.Text = "Total Deductions: ₱0.00";
                    lblTotalNet.Text = "Total Net: ₱0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating summary: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedPeriodId == 0)
                {
                    MessageBox.Show("Please select a payroll period.", "Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "This will generate payroll for all active employees. Continue?", 
                    "Confirm Generation", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    GeneratePayroll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating payroll: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GeneratePayroll()
        {
            try
            {
                isGenerating = true;
                SetButtonStates();
                progressBar.Visible = true;
                lblProgress.Text = "Generating payroll...";

                // Get active employees
                string employeeQuery = "SELECT employee_id, basic_salary FROM employees WHERE status = 'Active'";
                DataTable employees = DatabaseManager.GetDataTable(employeeQuery);
                
                progressBar.Maximum = employees.Rows.Count;
                progressBar.Value = 0;

                // Clear existing draft payroll for this period
                string deleteQuery = "DELETE FROM payroll_details WHERE period_id = @period_id AND status = 'Draft'";
                Dictionary<string, object> deleteParams = new Dictionary<string, object>
                {
                    { "@period_id", selectedPeriodId }
                };
                DatabaseManager.ExecuteNonQuery(deleteQuery, deleteParams);

                // Generate payroll for each employee
                foreach (DataRow emp in employees.Rows)
                {
                    string employeeId = emp["employee_id"].ToString();
                    decimal basicSalary = Convert.ToDecimal(emp["basic_salary"]);
                    
                    GenerateEmployeePayroll(employeeId, basicSalary);
                    
                    progressBar.Value++;
                    lblProgress.Text = $"Processing employee {progressBar.Value} of {progressBar.Maximum}...";
                    Application.DoEvents();
                }

                progressBar.Visible = false;
                lblProgress.Text = "";
                isGenerating = false;
                SetButtonStates();

                MessageBox.Show("Payroll generated successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadExistingPayroll();
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                lblProgress.Text = "";
                isGenerating = false;
                SetButtonStates();
                MessageBox.Show($"Error during payroll generation: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Payroll generation error: {ex}");
                throw;
            }
        }

        private void GenerateEmployeePayroll(string employeeId, decimal basicSalary)
        {
            try
            {
                // Calculate basic pay (assuming monthly salary / 2 for semi-monthly)
                decimal basicPay = basicSalary / 2;
                
                // Calculate overtime pay based on DTR records
                decimal overtimePay = CalculateOvertimePay(employeeId, basicSalary);
                
                // Calculate allowances (fixed and variable)
                decimal allowances = CalculateAllowances(employeeId);
                
                // Calculate gross pay
                decimal grossPay = basicPay + overtimePay + allowances;
                
                // Calculate deductions
                decimal sssDeduction = CalculateSSS(grossPay);
                decimal philhealthDeduction = CalculatePhilHealth(grossPay);
                decimal pagibigDeduction = CalculatePagIBIG(grossPay);
                decimal taxDeduction = CalculateTax(grossPay - sssDeduction - philhealthDeduction - pagibigDeduction);
                
                // Calculate cash advance and other deductions
                decimal otherDeductions = CalculateOtherDeductions(employeeId);
                
                decimal totalDeductions = sssDeduction + philhealthDeduction + pagibigDeduction + taxDeduction + otherDeductions;
                decimal netPay = grossPay - totalDeductions;

                // Insert payroll record
                string insertQuery = @"
                    INSERT INTO payroll_details 
                    (period_id, employee_id, basic_pay, overtime_pay, allowances, gross_pay, 
                     sss_deduction, philhealth_deduction, pagibig_deduction, tax_deduction, 
                     other_deductions, total_deductions, net_pay, status, created_at) 
                    VALUES 
                    (@period_id, @employee_id, @basic_pay, @overtime_pay, @allowances, @gross_pay, 
                     @sss_deduction, @philhealth_deduction, @pagibig_deduction, @tax_deduction, 
                     @other_deductions, @total_deductions, @net_pay, 'Draft', @created_at)";

                // Use DatabaseManager to handle connection management
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@period_id", selectedPeriodId },
                    { "@employee_id", employeeId },
                    { "@basic_pay", basicPay },
                    { "@overtime_pay", overtimePay },
                    { "@allowances", allowances },
                    { "@gross_pay", grossPay },
                    { "@sss_deduction", sssDeduction },
                    { "@philhealth_deduction", philhealthDeduction },
                    { "@pagibig_deduction", pagibigDeduction },
                    { "@tax_deduction", taxDeduction },
                    { "@other_deductions", otherDeductions },
                    { "@total_deductions", totalDeductions },
                    { "@net_pay", netPay },
                    { "@created_at", DateTime.Now }
                };
                
                DatabaseManager.ExecuteNonQuery(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating payroll for employee {employeeId}: {ex.Message}");
            }
        }

        private decimal CalculateSSS(decimal grossPay)
        {
            // Simplified SSS calculation - should be based on actual SSS table
            if (grossPay <= 3250) return 135;
            if (grossPay <= 3750) return 157.5m;
            if (grossPay <= 4250) return 180;
            if (grossPay <= 4750) return 202.5m;
            if (grossPay <= 5250) return 225;
            if (grossPay <= 5750) return 247.5m;
            if (grossPay <= 6250) return 270;
            if (grossPay <= 6750) return 292.5m;
            if (grossPay <= 7250) return 315;
            if (grossPay <= 7750) return 337.5m;
            if (grossPay <= 8250) return 360;
            if (grossPay <= 8750) return 382.5m;
            if (grossPay <= 9250) return 405;
            if (grossPay <= 9750) return 427.5m;
            if (grossPay <= 10250) return 450;
            if (grossPay <= 10750) return 472.5m;
            if (grossPay <= 11250) return 495;
            if (grossPay <= 11750) return 517.5m;
            if (grossPay <= 12250) return 540;
            if (grossPay <= 12750) return 562.5m;
            if (grossPay <= 13250) return 585;
            if (grossPay <= 13750) return 607.5m;
            if (grossPay <= 14250) return 630;
            if (grossPay <= 14750) return 652.5m;
            if (grossPay <= 15250) return 675;
            if (grossPay <= 15750) return 697.5m;
            if (grossPay <= 16250) return 720;
            if (grossPay <= 16750) return 742.5m;
            if (grossPay <= 17250) return 765;
            if (grossPay <= 17750) return 787.5m;
            if (grossPay <= 18250) return 810;
            if (grossPay <= 18750) return 832.5m;
            if (grossPay <= 19250) return 855;
            if (grossPay <= 19750) return 877.5m;
            return 900; // Maximum
        }

        private decimal CalculatePhilHealth(decimal grossPay)
        {
            // Simplified PhilHealth calculation - 2.75% of gross pay, max 1,800
            decimal philhealth = grossPay * 0.0275m;
            return Math.Min(philhealth, 1800);
        }

        private decimal CalculatePagIBIG(decimal grossPay)
        {
            // Simplified Pag-IBIG calculation - 2% of gross pay, max 100
            decimal pagibig = grossPay * 0.02m;
            return Math.Min(pagibig, 100);
        }

        private decimal CalculateOtherDeductions(string employeeId)
        {
            try
            {
                // Get cash advances for the current period
                string cashAdvanceQuery = @"
                    SELECT COALESCE(SUM(deduction_amount), 0) as total_ca_deductions
                    FROM cash_advances
                    WHERE employee_id = @employee_id
                    AND status = 'approved'
                    AND period_id = @period_id";

                MySqlParameter[] caParams = {
                    DatabaseManager.CreateParameter("@employee_id", employeeId),
                    DatabaseManager.CreateParameter("@period_id", selectedPeriodId)
                };

                object caResult = DatabaseManager.ExecuteScalar(cashAdvanceQuery, caParams);
                decimal cashAdvanceDeductions = caResult != DBNull.Value ? Convert.ToDecimal(caResult) : 0;

                // Get other deductions (loans, penalties, etc.)
                string otherDeductionsQuery = @"
                    SELECT COALESCE(SUM(amount), 0) as total_other_deductions
                    FROM employee_deductions
                    WHERE employee_id = @employee_id
                    AND is_active = 1
                    AND deduction_date BETWEEN 
                        (SELECT start_date FROM payroll_periods WHERE period_id = @period_id)
                    AND
                        (SELECT end_date FROM payroll_periods WHERE period_id = @period_id)";

                MySqlParameter[] odParams = {
                    DatabaseManager.CreateParameter("@employee_id", employeeId),
                    DatabaseManager.CreateParameter("@period_id", selectedPeriodId)
                };

                object odResult = DatabaseManager.ExecuteScalar(otherDeductionsQuery, odParams);
                decimal miscDeductions = odResult != DBNull.Value ? Convert.ToDecimal(odResult) : 0;

                // Calculate total deductions
                decimal totalDeductions = cashAdvanceDeductions + miscDeductions;
                
                return Math.Round(totalDeductions, 2);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating other deductions: {ex.Message}");
            }
        }

        private decimal CalculateAllowances(string employeeId)
        {
            try
            {
                // Get fixed allowances from employee_allowances table
                string fixedAllowanceQuery = @"
                    SELECT COALESCE(SUM(amount), 0) as total_fixed_allowances
                    FROM employee_allowances
                    WHERE employee_id = @employee_id
                    AND allowance_type = 'fixed'
                    AND is_active = 1";

                MySqlParameter[] fixedParams = {
                    DatabaseManager.CreateParameter("@employee_id", employeeId)
                };

                object fixedResult = DatabaseManager.ExecuteScalar(fixedAllowanceQuery, fixedParams);
                decimal fixedAllowances = fixedResult != DBNull.Value ? Convert.ToDecimal(fixedResult) : 0;

                // Get variable allowances for the current period
                string variableAllowanceQuery = @"
                    SELECT COALESCE(SUM(at.amount), 0) as total_variable_allowances
                    FROM employee_allowances ea
                    JOIN allowance_transactions at ON ea.allowance_id = at.allowance_id
                    WHERE ea.employee_id = @employee_id
                    AND ea.allowance_type = 'variable'
                    AND ea.is_active = 1
                    AND at.transaction_date BETWEEN 
                        (SELECT start_date FROM payroll_periods WHERE period_id = @period_id)
                    AND
                        (SELECT end_date FROM payroll_periods WHERE period_id = @period_id)";

                MySqlParameter[] variableParams = {
                    DatabaseManager.CreateParameter("@employee_id", employeeId),
                    DatabaseManager.CreateParameter("@period_id", selectedPeriodId)
                };

                object variableResult = DatabaseManager.ExecuteScalar(variableAllowanceQuery, variableParams);
                decimal variableAllowances = variableResult != DBNull.Value ? Convert.ToDecimal(variableResult) : 0;

                // Calculate total allowances (fixed allowances are divided by 2 for semi-monthly payroll)
                decimal totalAllowances = (fixedAllowances / 2) + variableAllowances;
                
                return Math.Round(totalAllowances, 2);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating allowances: {ex.Message}");
            }
        }

        private decimal CalculateOvertimePay(string employeeId, decimal basicSalary)
        {
            //Need DTR
            try
            {
                // Get payroll settings for overtime rate
                string settingsQuery = "SELECT overtime_rate, working_hours_per_day FROM payroll_settings LIMIT 1";
                DataTable settings = DatabaseManager.GetDataTable(settingsQuery);
                
                if (settings.Rows.Count == 0)
                    return 0;

                decimal overtimeRate = Convert.ToDecimal(settings.Rows[0]["overtime_rate"]);
                decimal workingHoursPerDay = Convert.ToDecimal(settings.Rows[0]["working_hours_per_day"]);
                
                // Calculate hourly rate (monthly salary / (working days * working hours))
                decimal hourlyRate = basicSalary / (22 * workingHoursPerDay); // Assuming 22 working days per month

                // Get overtime hours from DTR for the current period
                //Need DTR
                string dtrQuery = @"
                    SELECT COALESCE(SUM(overtime_hours), 0) as total_ot_hours
                    FROM dtr_records
                    WHERE employee_id = @employee_id
                    AND dtr_date BETWEEN 
                        (SELECT start_date FROM payroll_periods WHERE period_id = @period_id)
                    AND
                        (SELECT end_date FROM payroll_periods WHERE period_id = @period_id)";

                MySqlParameter[] parameters = {
                    DatabaseManager.CreateParameter("@employee_id", employeeId),
                    DatabaseManager.CreateParameter("@period_id", selectedPeriodId)
                };

                object result = DatabaseManager.ExecuteScalar(dtrQuery, parameters);
                decimal totalOvertimeHours = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                
                // Calculate overtime pay
                decimal overtimePay = hourlyRate * overtimeRate * totalOvertimeHours;
                
                return Math.Round(overtimePay, 2);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating overtime pay: {ex.Message}");
            }
        }

        private decimal CalculateTax(decimal taxableIncome)
        {
            // Simplified tax calculation based on 2018 TRAIN law (semi-monthly)
            if (taxableIncome <= 10417) return 0;
            if (taxableIncome <= 16667) return (taxableIncome - 10417) * 0.15m;
            if (taxableIncome <= 33333) return 937.5m + (taxableIncome - 16667) * 0.20m;
            if (taxableIncome <= 83333) return 4270.83m + (taxableIncome - 33333) * 0.25m;
            if (taxableIncome <= 333333) return 16770.83m + (taxableIncome - 83333) * 0.30m;
            return 91770.83m + (taxableIncome - 333333) * 0.35m;
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            // Recalculate existing payroll
            BtnGenerate_Click(sender, e);
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPayroll.Rows.Count == 0)
                {
                    MessageBox.Show("No payroll data to preview.", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Generate payroll preview report
                string reportQuery = @"
                    SELECT 
                        e.employee_code,
                        CONCAT(e.last_name, ', ', e.first_name, ' ', COALESCE(e.middle_name, '')) as employee_name,
                        d.department_name,
                        pd.basic_pay,
                        pd.overtime_pay,
                        pd.allowances,
                        pd.gross_pay,
                        pd.sss_deduction,
                        pd.philhealth_deduction,
                        pd.pagibig_deduction,
                        pd.tax_deduction,
                        pd.other_deductions,
                        pd.net_pay,
                        pp.start_date,
                        pp.end_date,
                        pp.pay_date
                    FROM payroll_details pd
                    JOIN employees e ON pd.employee_id = e.employee_id
                    JOIN departments d ON e.department_id = d.department_id
                    JOIN payroll_periods pp ON pd.period_id = pp.period_id
                    WHERE pd.period_id = @period_id
                    ORDER BY d.department_name, e.last_name, e.first_name";

                MySqlParameter[] parameters = {
                    DatabaseManager.CreateParameter("@period_id", selectedPeriodId)
                };

                DataTable reportData = DatabaseManager.GetDataTable(reportQuery, parameters);

                // Create and show the preview form
                using (var previewForm = new frmPayrollPreview((System.Data.DataTable)reportData))
                {
                    previewForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating payroll preview: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPayroll.Rows.Count == 0)
                {
                    MessageBox.Show("No payroll data to export.", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get payroll data for export
                string exportQuery = @"
                    SELECT 
                        e.employee_code as 'Employee Code',
                        CONCAT(e.last_name, ', ', e.first_name, ' ', COALESCE(e.middle_name, '')) as 'Employee Name',
                        d.department_name as 'Department',
                        pd.basic_pay as 'Basic Pay',
                        pd.overtime_pay as 'Overtime Pay',
                        pd.allowances as 'Allowances',
                        pd.gross_pay as 'Gross Pay',
                        pd.sss_deduction as 'SSS',
                        pd.philhealth_deduction as 'PhilHealth',
                        pd.pagibig_deduction as 'Pag-IBIG',
                        pd.tax_deduction as 'Tax',
                        pd.other_deductions as 'Other Deductions',
                        pd.net_pay as 'Net Pay'
                    FROM payroll_details pd
                    JOIN employees e ON pd.employee_id = e.employee_id
                    JOIN departments d ON e.department_id = d.department_id
                    WHERE pd.period_id = @period_id
                    ORDER BY d.department_name, e.last_name, e.first_name";

                MySqlParameter[] parameters = {
                    DatabaseManager.CreateParameter("@period_id", selectedPeriodId)
                };

                DataTable exportData = DatabaseManager.GetDataTable(exportQuery, parameters);

                // Show save file dialog
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Export Payroll";
                saveDialog.FileName = $"Payroll_Export_{DateTime.Now:yyyyMMdd}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Export to Excel
                    using (var workbook = new ClosedXML.Excel.XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Payroll");

                        // Add title
                        worksheet.Cell(1, 1).Value = "AMCES PAYROLL SYSTEM";
                        worksheet.Cell(1, 1).Style.Font.Bold = true;
                        worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                        worksheet.Range(1, 1, 1, exportData.Columns.Count).Merge();
                        worksheet.Cell(1, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                        // Add period information
                        string periodQuery = "SELECT start_date, end_date FROM payroll_periods WHERE period_id = @period_id";
                        DataTable periodData = DatabaseManager.GetDataTable(periodQuery, parameters);
                        if (periodData.Rows.Count > 0)
                        {
                            DateTime startDate = Convert.ToDateTime(periodData.Rows[0]["start_date"]);
                            DateTime endDate = Convert.ToDateTime(periodData.Rows[0]["end_date"]);
                            worksheet.Cell(2, 1).Value = $"Period: {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";
                            worksheet.Cell(2, 1).Style.Font.Bold = true;
                            worksheet.Range(2, 1, 2, exportData.Columns.Count).Merge();
                            worksheet.Cell(2, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                        }

                        // Add headers
                        for (int i = 0; i < exportData.Columns.Count; i++)
                        {
                            worksheet.Cell(4, i + 1).Value = exportData.Columns[i].ColumnName;
                            worksheet.Cell(4, i + 1).Style.Font.Bold = true;
                            worksheet.Cell(4, i + 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                        }

                        // Add data
                        for (int i = 0; i < exportData.Rows.Count; i++)
                        {
                            for (int j = 0; j < exportData.Columns.Count; j++)
                            {
                                var cell = worksheet.Cell(i + 5, j + 1);
                                cell.Value = exportData.Rows[i][j].ToString();

                                // Format numeric columns
                                if (j >= 3) // Starting from Basic Pay column
                                {
                                    cell.Style.NumberFormat.Format = "#,##0.00";
                                }
                            }
                        }

                        // Add totals row
                        int lastRow = exportData.Rows.Count + 5;
                        worksheet.Cell(lastRow, 1).Value = "TOTAL";
                        worksheet.Cell(lastRow, 1).Style.Font.Bold = true;

                        // Calculate totals for numeric columns
                        for (int j = 4; j <= exportData.Columns.Count; j++)
                        {
                            worksheet.Cell(lastRow, j).FormulaA1 = $"=SUM({worksheet.Cell(5, j).Address}:{worksheet.Cell(lastRow - 1, j).Address})";
                            worksheet.Cell(lastRow, j).Style.Font.Bold = true;
                            worksheet.Cell(lastRow, j).Style.NumberFormat.Format = "#,##0.00";
                        }

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        // Save the workbook
                        workbook.SaveAs(saveDialog.FileName);
                    }

                    MessageBox.Show("Payroll data exported successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting payroll: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetButtonStates()
        {
            bool hasData = dgvPayroll.Rows.Count > 0;
            bool periodSelected = selectedPeriodId > 0;
            
            btnGenerate.Enabled = periodSelected && !isGenerating;
            btnCalculate.Enabled = periodSelected && !isGenerating;
            btnPreview.Enabled = hasData && !isGenerating;
            btnExport.Enabled = hasData && !isGenerating;
        }


    }
}
