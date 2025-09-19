using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class FrmDashboard : Form
    {
        public FrmDashboard()
        {
            InitializeComponent();
            this.Load += FrmDashboard_Load;
        }

        private void InitializeComponent()
        {
            this.panelHeader = new Panel();
            this.lblTitle = new Label();
            this.lblDateTime = new Label();
            this.panelStats = new Panel();
            this.panelEmployees = new Panel();
            this.lblEmployeeCount = new Label();
            this.lblEmployeeTitle = new Label();
            this.panelDepartments = new Panel();
            this.lblDepartmentCount = new Label();
            this.lblDepartmentTitle = new Label();
            this.panelPayroll = new Panel();
            this.lblPayrollCount = new Label();
            this.lblPayrollTitle = new Label();
            this.panelCashAdvance = new Panel();
            this.lblCashAdvanceCount = new Label();
            this.lblCashAdvanceTitle = new Label();
            this.panelRecentActivity = new Panel();
            this.lblRecentTitle = new Label();
            this.dgvRecentActivity = new DataGridView();
            this.panelQuickActions = new Panel();
            this.lblQuickActionsTitle = new Label();
            this.btnAddEmployee = new Button();
            this.btnGeneratePayroll = new Button();
            this.btnViewReports = new Button();
            this.btnManageDTR = new Button();
            this.timerDateTime = new System.Windows.Forms.Timer();
            this.panelHeader.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.panelEmployees.SuspendLayout();
            this.panelDepartments.SuspendLayout();
            this.panelPayroll.SuspendLayout();
            this.panelCashAdvance.SuspendLayout();
            this.panelRecentActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentActivity)).BeginInit();
            this.panelQuickActions.SuspendLayout();
            this.SuspendLayout();

            // 
            // timerDateTime
            // 
            this.timerDateTime.Interval = 1000;
            this.timerDateTime.Tick += TimerDateTime_Tick;

            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = Color.FromArgb(52, 73, 94);
            this.panelHeader.Controls.Add(this.lblDateTime);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = DockStyle.Top;
            this.panelHeader.Location = new Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(1200, 80);
            this.panelHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(350, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Dashboard Overview";

            // 
            // lblDateTime
            // 
            this.lblDateTime.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblDateTime.Font = new Font("Segoe UI", 12F);
            this.lblDateTime.ForeColor = Color.White;
            this.lblDateTime.Location = new Point(900, 25);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new Size(280, 25);
            this.lblDateTime.TabIndex = 1;
            this.lblDateTime.Text = "Loading...";
            this.lblDateTime.TextAlign = ContentAlignment.MiddleRight;

            // 
            // panelStats
            // 
            this.panelStats.Dock = DockStyle.Top;
            this.panelStats.Location = new Point(0, 80);
            this.panelStats.Name = "panelStats";
            this.panelStats.Padding = new Padding(20);
            this.panelStats.Size = new Size(1200, 150);
            this.panelStats.TabIndex = 1;

            // 
            // panelEmployees
            // 
            this.panelEmployees.BackColor = Color.FromArgb(46, 204, 113);
            this.panelEmployees.Controls.Add(this.lblEmployeeCount);
            this.panelEmployees.Controls.Add(this.lblEmployeeTitle);
            this.panelEmployees.Location = new Point(20, 20);
            this.panelEmployees.Name = "panelEmployees";
            this.panelEmployees.Size = new Size(270, 110);
            this.panelEmployees.TabIndex = 0;

            // 
            // lblEmployeeCount
            // 
            this.lblEmployeeCount.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            this.lblEmployeeCount.ForeColor = Color.White;
            this.lblEmployeeCount.Location = new Point(10, 10);
            this.lblEmployeeCount.Name = "lblEmployeeCount";
            this.lblEmployeeCount.Size = new Size(250, 50);
            this.lblEmployeeCount.TabIndex = 0;
            this.lblEmployeeCount.Text = "0";
            this.lblEmployeeCount.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblEmployeeTitle
            // 
            this.lblEmployeeTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblEmployeeTitle.ForeColor = Color.White;
            this.lblEmployeeTitle.Location = new Point(10, 70);
            this.lblEmployeeTitle.Name = "lblEmployeeTitle";
            this.lblEmployeeTitle.Size = new Size(250, 25);
            this.lblEmployeeTitle.TabIndex = 1;
            this.lblEmployeeTitle.Text = "Total Employees";
            this.lblEmployeeTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // panelDepartments
            // 
            this.panelDepartments.BackColor = Color.FromArgb(52, 152, 219);
            this.panelDepartments.Controls.Add(this.lblDepartmentCount);
            this.panelDepartments.Controls.Add(this.lblDepartmentTitle);
            this.panelDepartments.Location = new Point(310, 20);
            this.panelDepartments.Name = "panelDepartments";
            this.panelDepartments.Size = new Size(270, 110);
            this.panelDepartments.TabIndex = 1;

            // 
            // lblDepartmentCount
            // 
            this.lblDepartmentCount.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            this.lblDepartmentCount.ForeColor = Color.White;
            this.lblDepartmentCount.Location = new Point(10, 10);
            this.lblDepartmentCount.Name = "lblDepartmentCount";
            this.lblDepartmentCount.Size = new Size(250, 50);
            this.lblDepartmentCount.TabIndex = 0;
            this.lblDepartmentCount.Text = "0";
            this.lblDepartmentCount.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblDepartmentTitle
            // 
            this.lblDepartmentTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblDepartmentTitle.ForeColor = Color.White;
            this.lblDepartmentTitle.Location = new Point(10, 70);
            this.lblDepartmentTitle.Name = "lblDepartmentTitle";
            this.lblDepartmentTitle.Size = new Size(250, 25);
            this.lblDepartmentTitle.TabIndex = 1;
            this.lblDepartmentTitle.Text = "Departments";
            this.lblDepartmentTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // panelPayroll
            // 
            this.panelPayroll.BackColor = Color.FromArgb(155, 89, 182);
            this.panelPayroll.Controls.Add(this.lblPayrollCount);
            this.panelPayroll.Controls.Add(this.lblPayrollTitle);
            this.panelPayroll.Location = new Point(600, 20);
            this.panelPayroll.Name = "panelPayroll";
            this.panelPayroll.Size = new Size(270, 110);
            this.panelPayroll.TabIndex = 2;

            // 
            // lblPayrollCount
            // 
            this.lblPayrollCount.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            this.lblPayrollCount.ForeColor = Color.White;
            this.lblPayrollCount.Location = new Point(10, 10);
            this.lblPayrollCount.Name = "lblPayrollCount";
            this.lblPayrollCount.Size = new Size(250, 50);
            this.lblPayrollCount.TabIndex = 0;
            this.lblPayrollCount.Text = "0";
            this.lblPayrollCount.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblPayrollTitle
            // 
            this.lblPayrollTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblPayrollTitle.ForeColor = Color.White;
            this.lblPayrollTitle.Location = new Point(10, 70);
            this.lblPayrollTitle.Name = "lblPayrollTitle";
            this.lblPayrollTitle.Size = new Size(250, 25);
            this.lblPayrollTitle.TabIndex = 1;
            this.lblPayrollTitle.Text = "Payroll Records";
            this.lblPayrollTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // panelCashAdvance
            // 
            this.panelCashAdvance.BackColor = Color.FromArgb(230, 126, 34);
            this.panelCashAdvance.Controls.Add(this.lblCashAdvanceCount);
            this.panelCashAdvance.Controls.Add(this.lblCashAdvanceTitle);
            this.panelCashAdvance.Location = new Point(890, 20);
            this.panelCashAdvance.Name = "panelCashAdvance";
            this.panelCashAdvance.Size = new Size(270, 110);
            this.panelCashAdvance.TabIndex = 3;

            // 
            // lblCashAdvanceCount
            // 
            this.lblCashAdvanceCount.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            this.lblCashAdvanceCount.ForeColor = Color.White;
            this.lblCashAdvanceCount.Location = new Point(10, 10);
            this.lblCashAdvanceCount.Name = "lblCashAdvanceCount";
            this.lblCashAdvanceCount.Size = new Size(250, 50);
            this.lblCashAdvanceCount.TabIndex = 0;
            this.lblCashAdvanceCount.Text = "0";
            this.lblCashAdvanceCount.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblCashAdvanceTitle
            // 
            this.lblCashAdvanceTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblCashAdvanceTitle.ForeColor = Color.White;
            this.lblCashAdvanceTitle.Location = new Point(10, 70);
            this.lblCashAdvanceTitle.Name = "lblCashAdvanceTitle";
            this.lblCashAdvanceTitle.Size = new Size(250, 25);
            this.lblCashAdvanceTitle.TabIndex = 1;
            this.lblCashAdvanceTitle.Text = "Cash Advances";
            this.lblCashAdvanceTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // panelRecentActivity
            // 
            this.panelRecentActivity.BackColor = Color.White;
            this.panelRecentActivity.BorderStyle = BorderStyle.FixedSingle;
            this.panelRecentActivity.Controls.Add(this.dgvRecentActivity);
            this.panelRecentActivity.Controls.Add(this.lblRecentTitle);
            this.panelRecentActivity.Location = new Point(20, 250);
            this.panelRecentActivity.Name = "panelRecentActivity";
            this.panelRecentActivity.Size = new Size(750, 350);
            this.panelRecentActivity.TabIndex = 2;

            // 
            // lblRecentTitle
            // 
            this.lblRecentTitle.BackColor = Color.FromArgb(52, 73, 94);
            this.lblRecentTitle.Dock = DockStyle.Top;
            this.lblRecentTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblRecentTitle.ForeColor = Color.White;
            this.lblRecentTitle.Location = new Point(0, 0);
            this.lblRecentTitle.Name = "lblRecentTitle";
            this.lblRecentTitle.Size = new Size(748, 40);
            this.lblRecentTitle.TabIndex = 0;
            this.lblRecentTitle.Text = "Recent Activity";
            this.lblRecentTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblRecentTitle.Padding = new Padding(10, 0, 0, 0);

            // 
            // dgvRecentActivity
            // 
            this.dgvRecentActivity.AllowUserToAddRows = false;
            this.dgvRecentActivity.AllowUserToDeleteRows = false;
            this.dgvRecentActivity.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecentActivity.BackgroundColor = Color.White;
            this.dgvRecentActivity.BorderStyle = BorderStyle.None;
            this.dgvRecentActivity.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecentActivity.Dock = DockStyle.Fill;
            this.dgvRecentActivity.Location = new Point(0, 40);
            this.dgvRecentActivity.Name = "dgvRecentActivity";
            this.dgvRecentActivity.ReadOnly = true;
            this.dgvRecentActivity.RowHeadersVisible = false;
            this.dgvRecentActivity.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecentActivity.Size = new Size(748, 308);
            this.dgvRecentActivity.TabIndex = 1;

            // 
            // panelQuickActions
            // 
            this.panelQuickActions.BackColor = Color.White;
            this.panelQuickActions.BorderStyle = BorderStyle.FixedSingle;
            this.panelQuickActions.Controls.Add(this.btnManageDTR);
            this.panelQuickActions.Controls.Add(this.btnViewReports);
            this.panelQuickActions.Controls.Add(this.btnGeneratePayroll);
            this.panelQuickActions.Controls.Add(this.btnAddEmployee);
            this.panelQuickActions.Controls.Add(this.lblQuickActionsTitle);
            this.panelQuickActions.Location = new Point(790, 250);
            this.panelQuickActions.Name = "panelQuickActions";
            this.panelQuickActions.Size = new Size(370, 350);
            this.panelQuickActions.TabIndex = 3;

            // 
            // lblQuickActionsTitle
            // 
            this.lblQuickActionsTitle.BackColor = Color.FromArgb(52, 73, 94);
            this.lblQuickActionsTitle.Dock = DockStyle.Top;
            this.lblQuickActionsTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblQuickActionsTitle.ForeColor = Color.White;
            this.lblQuickActionsTitle.Location = new Point(0, 0);
            this.lblQuickActionsTitle.Name = "lblQuickActionsTitle";
            this.lblQuickActionsTitle.Size = new Size(368, 40);
            this.lblQuickActionsTitle.TabIndex = 0;
            this.lblQuickActionsTitle.Text = "Quick Actions";
            this.lblQuickActionsTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblQuickActionsTitle.Padding = new Padding(10, 0, 0, 0);

            // 
            // btnAddEmployee
            // 
            this.btnAddEmployee.BackColor = Color.FromArgb(46, 204, 113);
            this.btnAddEmployee.FlatAppearance.BorderSize = 0;
            this.btnAddEmployee.FlatStyle = FlatStyle.Flat;
            this.btnAddEmployee.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnAddEmployee.ForeColor = Color.White;
            this.btnAddEmployee.Location = new Point(20, 60);
            this.btnAddEmployee.Name = "btnAddEmployee";
            this.btnAddEmployee.Size = new Size(320, 50);
            this.btnAddEmployee.TabIndex = 1;
            this.btnAddEmployee.Text = "Add New Employee";
            this.btnAddEmployee.UseVisualStyleBackColor = false;
            this.btnAddEmployee.Click += BtnAddEmployee_Click;

            // 
            // btnGeneratePayroll
            // 
            this.btnGeneratePayroll.BackColor = Color.FromArgb(155, 89, 182);
            this.btnGeneratePayroll.FlatAppearance.BorderSize = 0;
            this.btnGeneratePayroll.FlatStyle = FlatStyle.Flat;
            this.btnGeneratePayroll.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnGeneratePayroll.ForeColor = Color.White;
            this.btnGeneratePayroll.Location = new Point(20, 130);
            this.btnGeneratePayroll.Name = "btnGeneratePayroll";
            this.btnGeneratePayroll.Size = new Size(320, 50);
            this.btnGeneratePayroll.TabIndex = 2;
            this.btnGeneratePayroll.Text = "Generate Payroll";
            this.btnGeneratePayroll.UseVisualStyleBackColor = false;
            this.btnGeneratePayroll.Click += BtnGeneratePayroll_Click;

            // 
            // btnViewReports
            // 
            this.btnViewReports.BackColor = Color.FromArgb(52, 152, 219);
            this.btnViewReports.FlatAppearance.BorderSize = 0;
            this.btnViewReports.FlatStyle = FlatStyle.Flat;
            this.btnViewReports.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnViewReports.ForeColor = Color.White;
            this.btnViewReports.Location = new Point(20, 200);
            this.btnViewReports.Name = "btnViewReports";
            this.btnViewReports.Size = new Size(320, 50);
            this.btnViewReports.TabIndex = 3;
            this.btnViewReports.Text = "View Reports";
            this.btnViewReports.UseVisualStyleBackColor = false;
            this.btnViewReports.Click += BtnViewReports_Click;

            // 
            // btnManageDTR
            // 
            this.btnManageDTR.BackColor = Color.FromArgb(230, 126, 34);
            this.btnManageDTR.FlatAppearance.BorderSize = 0;
            this.btnManageDTR.FlatStyle = FlatStyle.Flat;
            this.btnManageDTR.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnManageDTR.ForeColor = Color.White;
            this.btnManageDTR.Location = new Point(20, 270);
            this.btnManageDTR.Name = "btnManageDTR";
            this.btnManageDTR.Size = new Size(320, 50);
            this.btnManageDTR.TabIndex = 4;
            this.btnManageDTR.Text = "Manage DTR";
            this.btnManageDTR.UseVisualStyleBackColor = false;
            this.btnManageDTR.Click += BtnManageDTR_Click;

            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.ClientSize = new Size(1200, 650);
            this.Controls.Add(this.panelQuickActions);
            this.Controls.Add(this.panelRecentActivity);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.panelHeader);
            this.Name = "frmDashboard";
            this.Text = "Dashboard";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelStats.ResumeLayout(false);
            this.panelEmployees.ResumeLayout(false);
            this.panelDepartments.ResumeLayout(false);
            this.panelPayroll.ResumeLayout(false);
            this.panelCashAdvance.ResumeLayout(false);
            this.panelRecentActivity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentActivity)).EndInit();
            this.panelQuickActions.ResumeLayout(false);
            this.ResumeLayout(false);

            // Add panels to stats panel
            this.panelStats.Controls.Add(this.panelEmployees);
            this.panelStats.Controls.Add(this.panelDepartments);
            this.panelStats.Controls.Add(this.panelPayroll);
            this.panelStats.Controls.Add(this.panelCashAdvance);
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                // Start the timer for date/time display
                timerDateTime.Start();
                
                // Load dashboard data
                LoadDashboardData();
                LoadRecentActivity();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Log the exception for debugging purposes
                Console.WriteLine($"Dashboard loading error: {ex}");
            }
        }

        private void TimerDateTime_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - HH:mm:ss");
        }

        private void LoadDashboardData()
        {
            try
            {
                // Load employee count
                string employeeQuery = "SELECT COUNT(*) FROM employees WHERE employment_status = 'Active'";
                lblEmployeeCount.Text = DatabaseManager.GetScalar(employeeQuery).ToString();

                // Load department count
                string departmentQuery = "SELECT COUNT(*) FROM departments WHERE is_active = 1";
                lblDepartmentCount.Text = DatabaseManager.GetScalar(departmentQuery).ToString();

                // Load payroll count
                string payrollQuery = "SELECT COUNT(*) FROM payroll_details WHERE YEAR(created_at) = YEAR(CURDATE())";
                lblPayrollCount.Text = DatabaseManager.GetScalar(payrollQuery).ToString();

                // Load cash advance count
                string cashAdvanceQuery = "SELECT COUNT(*) FROM cash_advances WHERE YEAR(request_date) = YEAR(CURDATE())";
                lblCashAdvanceCount.Text = DatabaseManager.GetScalar(cashAdvanceQuery).ToString();
            }
            catch (Exception ex)
            {
                // Set default values if database queries fail
                lblEmployeeCount.Text = "0";
                lblDepartmentCount.Text = "0";
                lblPayrollCount.Text = "0";
                lblCashAdvanceCount.Text = "0";
                
                MessageBox.Show($"Error loading dashboard statistics: {ex.Message}", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine($"Dashboard statistics loading error: {ex}");
                throw; // Re-throw the exception to be caught by the caller
            }
        }

        private void LoadRecentActivity()
        {
            try
            {
                string query = @"
                    SELECT 
                        'Employee' as Type,
                        CONCAT(first_name, ' ', last_name) as Description,
                        hire_date as Date
                     FROM employees 
                     WHERE hire_date >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                    
                    UNION ALL
                    
                    SELECT 
                         'Payroll' as Type,
                         CONCAT('Payroll for ', pp.period_name) as Description,
                         pd.created_at as Date
                      FROM payroll_details pd
                      INNER JOIN payroll_periods pp ON pd.period_id = pp.period_id
                      WHERE pd.created_at >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                    
                    UNION ALL
                    
                    SELECT 
                        'Cash Advance' as Type,
                        CONCAT('Cash Advance - ', amount) as Description,
                        request_date as Date
                     FROM cash_advances 
                     WHERE request_date >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                    
                    ORDER BY Date DESC
                    LIMIT 10";

                DataTable dt = DatabaseManager.GetDataTable(query);
                dgvRecentActivity.DataSource = dt;

                // Format the DataGridView
                if (dgvRecentActivity.Columns.Count > 0)
                {
                    dgvRecentActivity.Columns["Type"].Width = 100;
                    dgvRecentActivity.Columns["Description"].Width = 400;
                    dgvRecentActivity.Columns["Date"].Width = 150;
                    dgvRecentActivity.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                }
            }
            catch (Exception ex)
            {
                // Create empty DataTable if query fails
                DataTable emptyDt = new DataTable();
                emptyDt.Columns.Add("Type", typeof(string));
                emptyDt.Columns.Add("Description", typeof(string));
                emptyDt.Columns.Add("Date", typeof(DateTime));
                
                DataRow row = emptyDt.NewRow();
                row["Type"] = "Info";
                row["Description"] = "No recent activity found";
                row["Date"] = DateTime.Now;
                emptyDt.Rows.Add(row);
                
                dgvRecentActivity.DataSource = emptyDt;
                
                MessageBox.Show($"Error loading recent activity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Recent activity loading error: {ex}");
            }
        }

        private void BtnAddEmployee_Click(object sender, EventArgs e)
        {
            frmEmployee employeeForm = new frmEmployee();
            employeeForm.ShowDialog();
            LoadDashboardData(); // Refresh data after adding employee
        }

        private void BtnGeneratePayroll_Click(object sender, EventArgs e)
        {
            frmPayrollGeneration payrollForm = new frmPayrollGeneration();
            payrollForm.ShowDialog();
            LoadDashboardData(); // Refresh data after generating payroll
        }

        private void BtnViewReports_Click(object sender, EventArgs e)
        {
            frmReports reportsForm = new frmReports();
            reportsForm.ShowDialog();
        }

        private void BtnManageDTR_Click(object sender, EventArgs e)
        {
            FrmDTR dtrForm = new FrmDTR();
            dtrForm.ShowDialog();
            LoadDashboardData(); // Refresh data after managing DTR
        }

        #region Designer Variables
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblDateTime;
        private Panel panelStats;
        private Panel panelEmployees;
        private Label lblEmployeeCount;
        private Label lblEmployeeTitle;
        private Panel panelDepartments;
        private Label lblDepartmentCount;
        private Label lblDepartmentTitle;
        private Panel panelPayroll;
        private Label lblPayrollCount;
        private Label lblPayrollTitle;
        private Panel panelCashAdvance;
        private Label lblCashAdvanceCount;
        private Label lblCashAdvanceTitle;
        private Panel panelRecentActivity;
        private Label lblRecentTitle;
        private DataGridView dgvRecentActivity;
        private Panel panelQuickActions;
        private Label lblQuickActionsTitle;
        private Button btnAddEmployee;
        private Button btnGeneratePayroll;
        private Button btnViewReports;
        private Button btnManageDTR;
        private System.Windows.Forms.Timer timerDateTime;
        #endregion
    }
}
