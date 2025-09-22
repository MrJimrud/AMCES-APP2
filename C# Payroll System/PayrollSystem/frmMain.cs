using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;
using PayrollSystem.Database;

namespace PayrollSystem
{
    public partial class frmMain : Form
    {
        private FrmDashboard dashboardForm;
        private ToolStripMenuItem databaseSetupToolStripMenuItem;
        
        public frmMain()
        {
            InitializeComponent();
            UtilityHelper.InitializeConnection();
            this.Load += FrmMain_Load;
            this.Resize += FrmMain_Resize;
        }

        private void InitializeComponent()
        {
            this.menuStrip = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.employeeToolStripMenuItem = new ToolStripMenuItem();
            this.employeeListToolStripMenuItem = new ToolStripMenuItem();
            this.addEmployeeToolStripMenuItem = new ToolStripMenuItem();
            this.departmentToolStripMenuItem = new ToolStripMenuItem();
            this.jobTitleToolStripMenuItem = new ToolStripMenuItem();
            this.payrollToolStripMenuItem = new ToolStripMenuItem();
            this.payrollPeriodToolStripMenuItem = new ToolStripMenuItem();
            this.payrollGenerationToolStripMenuItem = new ToolStripMenuItem();
            this.payrollListToolStripMenuItem = new ToolStripMenuItem();
            
            this.cashAdvanceToolStripMenuItem = new ToolStripMenuItem();
            this.cashAdvanceManagementToolStripMenuItem = new ToolStripMenuItem();
            this.configurationToolStripMenuItem = new ToolStripMenuItem();
            this.sssToolStripMenuItem = new ToolStripMenuItem();
            this.philhealthToolStripMenuItem = new ToolStripMenuItem();
            this.taxToolStripMenuItem = new ToolStripMenuItem();
            this.rateToolStripMenuItem = new ToolStripMenuItem();
          
            this.unitToolStripMenuItem = new ToolStripMenuItem();
            this.reportsToolStripMenuItem = new ToolStripMenuItem();
            this.payrollReportToolStripMenuItem = new ToolStripMenuItem();
           
            this.cashAdvanceReportToolStripMenuItem = new ToolStripMenuItem();
            this.payslipReportToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.statusStrip = new StatusStrip();
            this.toolStripStatusLabel = new ToolStripStatusLabel();
            this.panelMain = new Panel();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
                this.fileToolStripMenuItem,
                this.employeeToolStripMenuItem,
                this.payrollToolStripMenuItem,
                this.cashAdvanceToolStripMenuItem,
                this.configurationToolStripMenuItem,
                this.reportsToolStripMenuItem,
                this.helpToolStripMenuItem});
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(1200, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";

            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";

            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;

            // 
            // employeeToolStripMenuItem
            // 
            this.employeeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.employeeListToolStripMenuItem,
                this.addEmployeeToolStripMenuItem,
                this.departmentToolStripMenuItem,
                this.jobTitleToolStripMenuItem});
            this.employeeToolStripMenuItem.Name = "employeeToolStripMenuItem";
            this.employeeToolStripMenuItem.Size = new Size(71, 20);
            this.employeeToolStripMenuItem.Text = "Employee";

            // 
            // employeeListToolStripMenuItem
            // 
            this.employeeListToolStripMenuItem.Name = "employeeListToolStripMenuItem";
            this.employeeListToolStripMenuItem.Size = new Size(154, 22);
            this.employeeListToolStripMenuItem.Text = "Employee List";
            this.employeeListToolStripMenuItem.Click += EmployeeListToolStripMenuItem_Click;

            // 
            // addEmployeeToolStripMenuItem
            // 
            this.addEmployeeToolStripMenuItem.Name = "addEmployeeToolStripMenuItem";
            this.addEmployeeToolStripMenuItem.Size = new Size(154, 22);
            this.addEmployeeToolStripMenuItem.Text = "Add Employee";
            this.addEmployeeToolStripMenuItem.Click += AddEmployeeToolStripMenuItem_Click;

            // 
            // departmentToolStripMenuItem
            // 
            this.departmentToolStripMenuItem.Name = "departmentToolStripMenuItem";
            this.departmentToolStripMenuItem.Size = new Size(154, 22);
            this.departmentToolStripMenuItem.Text = "Department";
            this.departmentToolStripMenuItem.Click += DepartmentToolStripMenuItem_Click;

            // 
            // jobTitleToolStripMenuItem
            // 
            this.jobTitleToolStripMenuItem.Name = "jobTitleToolStripMenuItem";
            this.jobTitleToolStripMenuItem.Size = new Size(154, 22);
            this.jobTitleToolStripMenuItem.Text = "Job Title";
            this.jobTitleToolStripMenuItem.Click += JobTitleToolStripMenuItem_Click;

            // 
            // payrollToolStripMenuItem
            // 
            this.payrollToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.payrollPeriodToolStripMenuItem,
                this.payrollGenerationToolStripMenuItem,
                this.payrollListToolStripMenuItem});
            this.payrollToolStripMenuItem.Name = "payrollToolStripMenuItem";
            this.payrollToolStripMenuItem.Size = new Size(54, 20);
            this.payrollToolStripMenuItem.Text = "Payroll";

            // 
            // payrollPeriodToolStripMenuItem
            // 
            this.payrollPeriodToolStripMenuItem.Name = "payrollPeriodToolStripMenuItem";
            this.payrollPeriodToolStripMenuItem.Size = new Size(170, 22);
            this.payrollPeriodToolStripMenuItem.Text = "Payroll Period";
            this.payrollPeriodToolStripMenuItem.Click += PayrollPeriodToolStripMenuItem_Click;

            // 
            // payrollGenerationToolStripMenuItem
            // 
            this.payrollGenerationToolStripMenuItem.Name = "payrollGenerationToolStripMenuItem";
            this.payrollGenerationToolStripMenuItem.Size = new Size(170, 22);
            this.payrollGenerationToolStripMenuItem.Text = "Payroll Generation";
            this.payrollGenerationToolStripMenuItem.Click += PayrollGenerationToolStripMenuItem_Click;

            // 
            // payrollListToolStripMenuItem
            // 
            this.payrollListToolStripMenuItem.Name = "payrollListToolStripMenuItem";
            this.payrollListToolStripMenuItem.Size = new Size(170, 22);
            this.payrollListToolStripMenuItem.Text = "Payroll List";
            this.payrollListToolStripMenuItem.Click += PayrollListToolStripMenuItem_Click;

            
        
           
            // 
            // cashAdvanceToolStripMenuItem
            // 
            this.cashAdvanceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.cashAdvanceManagementToolStripMenuItem});
            this.cashAdvanceToolStripMenuItem.Name = "cashAdvanceToolStripMenuItem";
            this.cashAdvanceToolStripMenuItem.Size = new Size(94, 20);
            this.cashAdvanceToolStripMenuItem.Text = "Cash Advance";

            // 
            // cashAdvanceManagementToolStripMenuItem
            // 
            this.cashAdvanceManagementToolStripMenuItem.Name = "cashAdvanceManagementToolStripMenuItem";
            this.cashAdvanceManagementToolStripMenuItem.Size = new Size(216, 22);
            this.cashAdvanceManagementToolStripMenuItem.Text = "Cash Advance Management";
            this.cashAdvanceManagementToolStripMenuItem.Click += CashAdvanceManagementToolStripMenuItem_Click;

            // 
            // configurationToolStripMenuItem
            // 
            this.databaseSetupToolStripMenuItem = new ToolStripMenuItem();
            this.databaseSetupToolStripMenuItem.Name = "databaseSetupToolStripMenuItem";
            this.databaseSetupToolStripMenuItem.Size = new Size(140, 22);
            this.databaseSetupToolStripMenuItem.Text = "Database Setup";
            this.databaseSetupToolStripMenuItem.Click += DatabaseSetupToolStripMenuItem_Click;
            
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.databaseSetupToolStripMenuItem,
                this.sssToolStripMenuItem,
                this.philhealthToolStripMenuItem,
                this.taxToolStripMenuItem,
                this.rateToolStripMenuItem,
                this.unitToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new Size(93, 20);
            this.configurationToolStripMenuItem.Text = "Configuration";

            // 
            // sssToolStripMenuItem
            // 
            this.sssToolStripMenuItem.Name = "sssToolStripMenuItem";
            this.sssToolStripMenuItem.Size = new Size(140, 22);
            this.sssToolStripMenuItem.Text = "SSS Settings";
            this.sssToolStripMenuItem.Click += SssToolStripMenuItem_Click;

            // 
            // philhealthToolStripMenuItem
            // 
            this.philhealthToolStripMenuItem.Name = "philhealthToolStripMenuItem";
            this.philhealthToolStripMenuItem.Size = new Size(140, 22);
            this.philhealthToolStripMenuItem.Text = "PhilHealth";
            this.philhealthToolStripMenuItem.Click += PhilhealthToolStripMenuItem_Click;

            // 
            // taxToolStripMenuItem
            // 
            this.taxToolStripMenuItem.Name = "taxToolStripMenuItem";
            this.taxToolStripMenuItem.Size = new Size(140, 22);
            this.taxToolStripMenuItem.Text = "Tax Settings";
            this.taxToolStripMenuItem.Click += TaxToolStripMenuItem_Click;

            // 
            // rateToolStripMenuItem
            // 
            this.rateToolStripMenuItem.Name = "rateToolStripMenuItem";
            this.rateToolStripMenuItem.Size = new Size(140, 22);
            this.rateToolStripMenuItem.Text = "Rate Settings";
            this.rateToolStripMenuItem.Click += RateToolStripMenuItem_Click;

            // 
            // unitToolStripMenuItem
            // 
            this.unitToolStripMenuItem.Name = "unitToolStripMenuItem";
            this.unitToolStripMenuItem.Size = new Size(140, 22);
            this.unitToolStripMenuItem.Text = "Unit Settings";
            this.unitToolStripMenuItem.Click += UnitToolStripMenuItem_Click;

            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.payrollReportToolStripMenuItem,
                this.cashAdvanceReportToolStripMenuItem,
                this.payslipReportToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new Size(59, 20);
            this.reportsToolStripMenuItem.Text = "Reports";

            // 
            // payrollReportToolStripMenuItem
            // 
            this.payrollReportToolStripMenuItem.Name = "payrollReportToolStripMenuItem";
            this.payrollReportToolStripMenuItem.Size = new Size(186, 22);
            this.payrollReportToolStripMenuItem.Text = "Payroll Report";
            this.payrollReportToolStripMenuItem.Click += PayrollReportToolStripMenuItem_Click;

           
            

            // 
            // cashAdvanceReportToolStripMenuItem
            // 
            this.cashAdvanceReportToolStripMenuItem.Name = "cashAdvanceReportToolStripMenuItem";
            this.cashAdvanceReportToolStripMenuItem.Size = new Size(186, 22);
            this.cashAdvanceReportToolStripMenuItem.Text = "Cash Advance Report";
            this.cashAdvanceReportToolStripMenuItem.Click += CashAdvanceReportToolStripMenuItem_Click;

            // 
            // payslipReportToolStripMenuItem
            // 
            this.payslipReportToolStripMenuItem.Name = "payslipReportToolStripMenuItem";
            this.payslipReportToolStripMenuItem.Size = new Size(186, 22);
            this.payslipReportToolStripMenuItem.Text = "Payslip Report";
            this.payslipReportToolStripMenuItem.Click += PayslipReportToolStripMenuItem_Click;

            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";

            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;

            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new ToolStripItem[] {
                this.toolStripStatusLabel});
            this.statusStrip.Location = new Point(0, 739);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Size(1200, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";

            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";

            // 
            // panelMain
            // 
            this.panelMain.Dock = DockStyle.Fill;
            this.panelMain.Location = new Point(0, 24);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new Size(1200, 715);
            this.panelMain.TabIndex = 2;

            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 761);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "frmMain";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Payroll Management System";
            this.WindowState = FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                // Load dashboard by default
                LoadDashboard();
                
                // Update status bar
                toolStripStatusLabel.Text = $"Welcome, {DatabaseManager.CurrentUser} | {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading main form: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            // Update dashboard size if it's loaded
            if (dashboardForm != null && !dashboardForm.IsDisposed)
            {
                dashboardForm.Size = panelMain.Size;
            }
        }

        private void LoadDashboard()
        {
            try
            {
                // Clear existing controls
                panelMain.Controls.Clear();
                
                // Create and load dashboard
                dashboardForm = new FrmDashboard();
                dashboardForm.TopLevel = false;
                dashboardForm.FormBorderStyle = FormBorderStyle.None;
                dashboardForm.Dock = DockStyle.Fill;
                
                panelMain.Controls.Add(dashboardForm);
                dashboardForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Menu Event Handlers
        
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void EmployeeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmployeeList frmEmpList = new frmEmployeeList();
            frmEmpList.ShowDialog();
        }

        private void AddEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmployee frmEmp = new frmEmployee();
            frmEmp.ShowDialog();
        }

        private void DepartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDepartment departmentForm = new frmDepartment();
            departmentForm.ShowDialog();
        }

        private void JobTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmJobTitle jobTitleForm = new frmJobTitle();
            jobTitleForm.ShowDialog();
        }

        private void PayrollPeriodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPayrollPeriod payrollPeriodForm = new frmPayrollPeriod();
            payrollPeriodForm.ShowDialog();
        }

        private void PayrollGenerationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPayrollGeneration payrollGenerationForm = new frmPayrollGeneration();
            payrollGenerationForm.ShowDialog();
        }

        private void PayrollListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPayrollGenerationList payrollListForm = new frmPayrollGenerationList();
            payrollListForm.ShowDialog();
        }

        private void CashAdvanceManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCashAdvance cashAdvanceForm = new frmCashAdvance();
            cashAdvanceForm.ShowDialog();
        }
        private void SssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSSS sssForm = new frmSSS();
            sssForm.ShowDialog();
        }

        private void PhilhealthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPhilhealth philhealthForm = new frmPhilhealth();
            philhealthForm.ShowDialog();
        }

        private void TaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTax taxForm = new frmTax();
            taxForm.ShowDialog();
        }

        private void RateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRate rateForm = new frmRate();
            rateForm.ShowDialog();
        }

     

        private void UnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUnit unitForm = new frmUnit();
            unitForm.ShowDialog();
        }
        
        private void DatabaseSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDatabaseSetup databaseSetupForm = new frmDatabaseSetup();
            if (databaseSetupForm.ShowDialog() == DialogResult.OK)
            {
                // Reload database connection after setup
                DatabaseManager.LoadConfiguration();
                DatabaseManager.InitializeConnection();
                MessageBox.Show("Database connection has been updated.", "Database Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PayrollReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReportPayroll payrollReportForm = new frmReportPayroll();
            payrollReportForm.ShowDialog();
        }



        private void CashAdvanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReportCA caReportForm = new frmReportCA();
            caReportForm.ShowDialog();
        }

        private void PayslipReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReportPayslip payslipReportForm = new frmReportPayslip();
            payslipReportForm.ShowDialog();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Payroll Management System\nVersion 1.0\nDeveloped in C#", "About", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        #endregion

        #region Designer Variables
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem employeeToolStripMenuItem;
        private ToolStripMenuItem employeeListToolStripMenuItem;
        private ToolStripMenuItem addEmployeeToolStripMenuItem;
        private ToolStripMenuItem departmentToolStripMenuItem;
        private ToolStripMenuItem jobTitleToolStripMenuItem;
        private ToolStripMenuItem payrollToolStripMenuItem;
        private ToolStripMenuItem payrollPeriodToolStripMenuItem;
        private ToolStripMenuItem payrollGenerationToolStripMenuItem;
        private ToolStripMenuItem payrollListToolStripMenuItem;
        private ToolStripMenuItem cashAdvanceToolStripMenuItem;
        private ToolStripMenuItem cashAdvanceManagementToolStripMenuItem;
        private ToolStripMenuItem configurationToolStripMenuItem;
        private ToolStripMenuItem sssToolStripMenuItem;
        private ToolStripMenuItem philhealthToolStripMenuItem;
        private ToolStripMenuItem taxToolStripMenuItem;
        private ToolStripMenuItem rateToolStripMenuItem;
        private ToolStripMenuItem unitToolStripMenuItem;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem payrollReportToolStripMenuItem;
        private ToolStripMenuItem cashAdvanceReportToolStripMenuItem;
        private ToolStripMenuItem payslipReportToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private Panel panelMain;
        #endregion
    }
}
