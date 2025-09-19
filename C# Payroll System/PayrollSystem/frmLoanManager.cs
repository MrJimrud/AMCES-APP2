using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PayrollSystem
{
    public partial class frmLoanManager : Form
    {
        private TabControl tabControl;
        private TabPage tabLoanList;
        private TabPage tabLoanApplications;
        private TabPage tabLoanPayments;
        
        // Loan List Tab Controls
        private DataGridView dgvLoans;
        private TextBox txtSearchLoans;
        private ComboBox cmbLoanStatus;
        private ComboBox cmbLoanType;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Button btnNewLoan;
        private Button btnEditLoan;
        private Button btnDeleteLoan;
        private Button btnApproveLoan;
        private Button btnRejectLoan;
        private Label lblTotalLoans;
        
        // Loan Applications Tab Controls
        private DataGridView dgvApplications;
        private TextBox txtSearchApplications;
        private ComboBox cmbApplicationStatus;
        private Button btnViewApplication;
        private Button btnApproveApplication;
        private Button btnRejectApplication;
        private Label lblTotalApplications;
        
        // Loan Payments Tab Controls
        private DataGridView dgvPayments;
        private TextBox txtSearchPayments;
        private ComboBox cmbPaymentStatus;
        private Button btnRecordPayment;
        private Button btnViewPaymentHistory;
        private Label lblTotalPayments;

        public frmLoanManager()
        {
            InitializeComponent();
            LoadInitialData();
        }

        private void InitializeComponent()
        {
            this.Text = "Loan Management System";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1000, 600);

            // Create tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // Create tabs
            tabLoanList = new TabPage("Active Loans");
            tabLoanApplications = new TabPage("Loan Applications");
            tabLoanPayments = new TabPage("Loan Payments");

            tabControl.TabPages.AddRange(new TabPage[] { tabLoanList, tabLoanApplications, tabLoanPayments });

            InitializeLoanListTab();
            InitializeLoanApplicationsTab();
            InitializeLoanPaymentsTab();

            this.Controls.Add(tabControl);
        }

        private void InitializeLoanListTab()
        {
            // Top panel for controls
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Search controls
            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 25),
                Size = new Size(60, 23)
            };

            txtSearchLoans = new TextBox
            {
                Location = new Point(85, 22),
                Size = new Size(200, 23)
            };
            txtSearchLoans.SetPlaceholderText("Employee name or loan ID...");
            txtSearchLoans.TextChanged += TxtSearchLoans_TextChanged;

            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(250, 25),
                Size = new Size(50, 23)
            };

            cmbLoanStatus = new ComboBox
            {
                Location = new Point(305, 22),
                Size = new Size(100, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbLoanStatus.Items.AddRange(new[] { "All", "Active", "Completed", "Defaulted" });
            cmbLoanStatus.SelectedIndex = 0;
            cmbLoanStatus.SelectedIndexChanged += CmbLoanStatus_SelectedIndexChanged;

            Label lblType = new Label
            {
                Text = "Type:",
                Location = new Point(420, 25),
                Size = new Size(40, 23)
            };

            cmbLoanType = new ComboBox
            {
                Location = new Point(465, 22),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbLoanType.Items.AddRange(new[] { "All", "Personal", "Emergency", "Salary", "Equipment" });
            cmbLoanType.SelectedIndex = 0;
            cmbLoanType.SelectedIndexChanged += CmbLoanType_SelectedIndexChanged;

            Label lblFromDate = new Label
            {
                Text = "From:",
                Location = new Point(600, 25),
                Size = new Size(40, 23)
            };

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(645, 22),
                Size = new Size(100, 23),
                Format = DateTimePickerFormat.Short
            };
            dtpFromDate.ValueChanged += DtpFromDate_ValueChanged;

            Label lblToDate = new Label
            {
                Text = "To:",
                Location = new Point(760, 25),
                Size = new Size(25, 23)
            };

            dtpToDate = new DateTimePicker
            {
                Location = new Point(790, 22),
                Size = new Size(100, 23),
                Format = DateTimePickerFormat.Short
            };
            dtpToDate.ValueChanged += DtpToDate_ValueChanged;

            // Buttons
            btnNewLoan = new Button
            {
                Text = "New Loan",
                Location = new Point(920, 20),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnNewLoan.Click += BtnNewLoan_Click;

            btnEditLoan = new Button
            {
                Text = "Edit",
                Location = new Point(1010, 20),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnEditLoan.Click += BtnEditLoan_Click;

            btnDeleteLoan = new Button
            {
                Text = "Delete",
                Location = new Point(1080, 20),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteLoan.Click += BtnDeleteLoan_Click;

            pnlTop.Controls.AddRange(new Control[] { 
                lblSearch, txtSearchLoans, lblStatus, cmbLoanStatus, lblType, cmbLoanType,
                lblFromDate, dtpFromDate, lblToDate, dtpToDate, btnNewLoan, btnEditLoan, btnDeleteLoan
            });

            // DataGridView
            dgvLoans = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvLoans.CellDoubleClick += DgvLoans_CellDoubleClick;

            // Bottom panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalLoans = new Label
            {
                Text = "Total Loans: 0",
                Location = new Point(20, 15),
                Size = new Size(200, 23)
            };

            btnApproveLoan = new Button
            {
                Text = "Approve",
                Location = new Point(900, 10),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnApproveLoan.Click += BtnApproveLoan_Click;

            btnRejectLoan = new Button
            {
                Text = "Reject",
                Location = new Point(990, 10),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRejectLoan.Click += BtnRejectLoan_Click;

            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(1070, 10),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += (s, e) => this.Close();

            pnlBottom.Controls.AddRange(new Control[] { lblTotalLoans, btnApproveLoan, btnRejectLoan, btnClose });

            tabLoanList.Controls.AddRange(new Control[] { pnlTop, dgvLoans, pnlBottom });
            UtilityHelper.ApplyLightMode(dgvLoans);
        }

        private void InitializeLoanApplicationsTab()
        {
            // Top panel
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 20),
                Size = new Size(60, 23)
            };

            txtSearchApplications = new TextBox
            {
                Location = new Point(85, 17),
                Size = new Size(200, 23)
            };
            txtSearchApplications.SetPlaceholderText("Employee name or application ID...");
            txtSearchApplications.TextChanged += TxtSearchApplications_TextChanged;

            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(300, 20),
                Size = new Size(50, 23)
            };

            cmbApplicationStatus = new ComboBox
            {
                Location = new Point(355, 17),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbApplicationStatus.Items.AddRange(new[] { "All", "Pending", "Approved", "Rejected" });
            cmbApplicationStatus.SelectedIndex = 0;
            cmbApplicationStatus.SelectedIndexChanged += CmbApplicationStatus_SelectedIndexChanged;

            btnViewApplication = new Button
            {
                Text = "View Details",
                Location = new Point(500, 15),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnViewApplication.Click += BtnViewApplication_Click;

            btnApproveApplication = new Button
            {
                Text = "Approve",
                Location = new Point(600, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnApproveApplication.Click += BtnApproveApplication_Click;

            btnRejectApplication = new Button
            {
                Text = "Reject",
                Location = new Point(690, 15),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRejectApplication.Click += BtnRejectApplication_Click;

            pnlTop.Controls.AddRange(new Control[] { 
                lblSearch, txtSearchApplications, lblStatus, cmbApplicationStatus,
                btnViewApplication, btnApproveApplication, btnRejectApplication
            });

            // DataGridView
            dgvApplications = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvApplications.CellDoubleClick += DgvApplications_CellDoubleClick;

            // Bottom panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalApplications = new Label
            {
                Text = "Total Applications: 0",
                Location = new Point(20, 10),
                Size = new Size(200, 23)
            };

            pnlBottom.Controls.Add(lblTotalApplications);

            tabLoanApplications.Controls.AddRange(new Control[] { pnlTop, dgvApplications, pnlBottom });
            UtilityHelper.ApplyLightMode(dgvApplications);
        }

        private void InitializeLoanPaymentsTab()
        {
            // Top panel
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 20),
                Size = new Size(60, 23)
            };

            txtSearchPayments = new TextBox
            {
                Location = new Point(85, 17),
                Size = new Size(200, 23)
            };
            txtSearchPayments.SetPlaceholderText("Employee name or loan ID...");
            txtSearchPayments.TextChanged += TxtSearchPayments_TextChanged;

            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(300, 20),
                Size = new Size(50, 23)
            };

            cmbPaymentStatus = new ComboBox
            {
                Location = new Point(355, 17),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPaymentStatus.Items.AddRange(new[] { "All", "Paid", "Overdue", "Pending" });
            cmbPaymentStatus.SelectedIndex = 0;
            cmbPaymentStatus.SelectedIndexChanged += CmbPaymentStatus_SelectedIndexChanged;

            btnRecordPayment = new Button
            {
                Text = "Record Payment",
                Location = new Point(500, 15),
                Size = new Size(110, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRecordPayment.Click += BtnRecordPayment_Click;

            btnViewPaymentHistory = new Button
            {
                Text = "View History",
                Location = new Point(620, 15),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnViewPaymentHistory.Click += BtnViewPaymentHistory_Click;

            pnlTop.Controls.AddRange(new Control[] { 
                lblSearch, txtSearchPayments, lblStatus, cmbPaymentStatus,
                btnRecordPayment, btnViewPaymentHistory
            });

            // DataGridView
            dgvPayments = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvPayments.CellDoubleClick += DgvPayments_CellDoubleClick;

            // Bottom panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblTotalPayments = new Label
            {
                Text = "Total Payments: 0",
                Location = new Point(20, 10),
                Size = new Size(200, 23)
            };

            pnlBottom.Controls.Add(lblTotalPayments);

            tabLoanPayments.Controls.AddRange(new Control[] { pnlTop, dgvPayments, pnlBottom });
            UtilityHelper.ApplyLightMode(dgvPayments);
        }

        private void LoadInitialData()
        {
            LoadLoanList();
            LoadLoanApplications();
            LoadLoanPayments();
        }

        private void LoadLoanList()
        {
            try
            {
                string query = @"
                    SELECT 
                        l.id,
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        l.loan_type as 'Loan Type',
                        FORMAT(l.loan_amount, 2) as 'Loan Amount',
                        FORMAT(l.monthly_payment, 2) as 'Monthly Payment',
                        l.loan_term as 'Term (Months)',
                        FORMAT(l.remaining_balance, 2) as 'Remaining Balance',
                        l.loan_status as 'Status',
                        l.application_date as 'Application Date',
                        l.approval_date as 'Approval Date',
                        l.next_payment_date as 'Next Payment'
                    FROM tbl_loan l
                    INNER JOIN tbl_employee e ON l.employee_id = e.id
                    ORDER BY l.application_date DESC";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvLoans.DataSource = dt;

                if (dgvLoans.Columns["id"] != null)
                    dgvLoans.Columns["id"].Visible = false;

                // Format currency columns
                FormatCurrencyColumns(dgvLoans, new[] { "Loan Amount", "Monthly Payment", "Remaining Balance" });
                FormatDateColumns(dgvLoans, new[] { "Application Date", "Approval Date", "Next Payment" });

                UpdateLoanCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoanApplications()
        {
            try
            {
                string query = @"
                    SELECT 
                        la.id,
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        la.loan_type as 'Loan Type',
                        FORMAT(la.requested_amount, 2) as 'Requested Amount',
                        la.loan_term as 'Term (Months)',
                        la.purpose as 'Purpose',
                        la.application_status as 'Status',
                        la.application_date as 'Application Date',
                        la.reviewed_by as 'Reviewed By',
                        la.review_date as 'Review Date'
                    FROM tbl_loan_application la
                    INNER JOIN tbl_employee e ON la.employee_id = e.id
                    ORDER BY la.application_date DESC";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvApplications.DataSource = dt;

                if (dgvApplications.Columns["id"] != null)
                    dgvApplications.Columns["id"].Visible = false;

                FormatCurrencyColumns(dgvApplications, new[] { "Requested Amount" });
                FormatDateColumns(dgvApplications, new[] { "Application Date", "Review Date" });

                UpdateApplicationCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan applications: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoanPayments()
        {
            try
            {
                string query = @"
                    SELECT 
                        lp.id,
                        CONCAT(e.first_name, ' ', e.last_name) as 'Employee Name',
                        l.loan_type as 'Loan Type',
                        FORMAT(lp.payment_amount, 2) as 'Payment Amount',
                        lp.payment_date as 'Payment Date',
                        lp.payment_method as 'Payment Method',
                        lp.payment_status as 'Status',
                        lp.reference_number as 'Reference Number',
                        FORMAT(l.remaining_balance, 2) as 'Remaining Balance'
                    FROM tbl_loan_payment lp
                    INNER JOIN tbl_loan l ON lp.loan_id = l.id
                    INNER JOIN tbl_employee e ON l.employee_id = e.id
                    ORDER BY lp.payment_date DESC";

                DataTable dt = UtilityHelper.GetDataSet(query);
                dgvPayments.DataSource = dt;

                if (dgvPayments.Columns["id"] != null)
                    dgvPayments.Columns["id"].Visible = false;

                FormatCurrencyColumns(dgvPayments, new[] { "Payment Amount", "Remaining Balance" });
                FormatDateColumns(dgvPayments, new[] { "Payment Date" });

                UpdatePaymentCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan payments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void UpdateLoanCount()
        {
            lblTotalLoans.Text = $"Total Loans: {dgvLoans.Rows.Count:N0}";
        }

        private void UpdateApplicationCount()
        {
            lblTotalApplications.Text = $"Total Applications: {dgvApplications.Rows.Count:N0}";
        }

        private void UpdatePaymentCount()
        {
            lblTotalPayments.Text = $"Total Payments: {dgvPayments.Rows.Count:N0}";
        }

        // Event handlers for Loan List tab
        private void TxtSearchLoans_TextChanged(object sender, EventArgs e)
        {
            ApplyLoanFilters();
        }

        private void CmbLoanStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyLoanFilters();
        }

        private void CmbLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyLoanFilters();
        }

        private void DtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            ApplyLoanFilters();
        }

        private void DtpToDate_ValueChanged(object sender, EventArgs e)
        {
            ApplyLoanFilters();
        }

        private void ApplyLoanFilters()
        {
            // Implementation for filtering loan list based on search criteria
            // This would involve rebuilding the query with WHERE clauses
            LoadLoanList(); // Simplified for now
        }

        private void BtnNewLoan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("New Loan functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditLoan_Click(object sender, EventArgs e)
        {
            if (dgvLoans.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a loan to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Edit Loan functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteLoan_Click(object sender, EventArgs e)
        {
            if (dgvLoans.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a loan to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this loan record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Delete Loan functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnApproveLoan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Approve Loan functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRejectLoan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reject Loan functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvLoans_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditLoan_Click(sender, e);
            }
        }

        // Event handlers for Loan Applications tab
        private void TxtSearchApplications_TextChanged(object sender, EventArgs e)
        {
            LoadLoanApplications(); // Simplified
        }

        private void CmbApplicationStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLoanApplications(); // Simplified
        }

        private void BtnViewApplication_Click(object sender, EventArgs e)
        {
            MessageBox.Show("View Application Details functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnApproveApplication_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Approve Application functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRejectApplication_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reject Application functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvApplications_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnViewApplication_Click(sender, e);
            }
        }

        // Event handlers for Loan Payments tab
        private void TxtSearchPayments_TextChanged(object sender, EventArgs e)
        {
            LoadLoanPayments(); // Simplified
        }

        private void CmbPaymentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLoanPayments(); // Simplified
        }

        private void BtnRecordPayment_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Record Payment functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnViewPaymentHistory_Click(object sender, EventArgs e)
        {
            MessageBox.Show("View Payment History functionality would be implemented here.", "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvPayments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnRecordPayment_Click(sender, e);
            }
        }
    }
}
