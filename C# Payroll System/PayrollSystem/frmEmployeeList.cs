using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmEmployeeList : Form
    {
        private DataGridView dgvEmployees;
        private TextBox txtSearch;
        private ComboBox cmbDepartment;
        private ComboBox cmbStatus;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnClose;
        private Label lblSearch;
        private Label lblDepartment;
        private Label lblStatus;
        private Panel pnlControls;
        private Panel pnlGrid;

        public frmEmployeeList()
        {
            InitializeComponent();
            LoadEmployees();
            LoadDepartments();
        }

        private void InitializeComponent()
        {
            this.dgvEmployees = new DataGridView();
            this.txtSearch = new TextBox();
            this.cmbDepartment = new ComboBox();
            this.cmbStatus = new ComboBox();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnRefresh = new Button();
            this.btnClose = new Button();
            this.lblSearch = new Label();
            this.lblDepartment = new Label();
            this.lblStatus = new Label();
            this.pnlControls = new Panel();
            this.pnlGrid = new Panel();
            
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Employee List";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            
            // Controls Panel
            this.pnlControls.Location = new Point(12, 12);
            this.pnlControls.Size = new Size(976, 80);
            this.pnlControls.BorderStyle = BorderStyle.FixedSingle;
            
            // Search Label
            this.lblSearch.Text = "Search:";
            this.lblSearch.Location = new Point(10, 15);
            this.lblSearch.Size = new Size(50, 23);
            
            // Search TextBox
            this.txtSearch.Location = new Point(65, 12);
            this.txtSearch.Size = new Size(200, 23);
            this.txtSearch.TextChanged += new EventHandler(this.txtSearch_TextChanged);
            
            // Department Label
            this.lblDepartment.Text = "Department:";
            this.lblDepartment.Location = new Point(280, 15);
            this.lblDepartment.Size = new Size(80, 23);
            
            // Department ComboBox
            this.cmbDepartment.Location = new Point(365, 12);
            this.cmbDepartment.Size = new Size(150, 23);
            this.cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDepartment.SelectedIndexChanged += new EventHandler(this.cmbDepartment_SelectedIndexChanged);
            
            // Status Label
            this.lblStatus.Text = "Status:";
            this.lblStatus.Location = new Point(530, 15);
            this.lblStatus.Size = new Size(50, 23);
            
            // Status ComboBox
            this.cmbStatus.Location = new Point(585, 12);
            this.cmbStatus.Size = new Size(120, 23);
            this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatus.Items.AddRange(new string[] { "All", "Active", "Inactive", "Terminated" });
            this.cmbStatus.SelectedIndex = 0;
            this.cmbStatus.SelectedIndexChanged += new EventHandler(this.cmbStatus_SelectedIndexChanged);
            
            // Buttons
            this.btnAdd.Text = "Add New";
            this.btnAdd.Location = new Point(10, 45);
            this.btnAdd.Size = new Size(80, 25);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            
            this.btnEdit.Text = "Edit";
            this.btnEdit.Location = new Point(100, 45);
            this.btnEdit.Size = new Size(80, 25);
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            
            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new Point(190, 45);
            this.btnDelete.Size = new Size(80, 25);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Location = new Point(280, 45);
            this.btnRefresh.Size = new Size(80, 25);
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
            
            this.btnClose.Text = "Close";
            this.btnClose.Location = new Point(880, 45);
            this.btnClose.Size = new Size(80, 25);
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            
            // Add controls to panel
            this.pnlControls.Controls.Add(this.lblSearch);
            this.pnlControls.Controls.Add(this.txtSearch);
            this.pnlControls.Controls.Add(this.lblDepartment);
            this.pnlControls.Controls.Add(this.cmbDepartment);
            this.pnlControls.Controls.Add(this.lblStatus);
            this.pnlControls.Controls.Add(this.cmbStatus);
            this.pnlControls.Controls.Add(this.btnAdd);
            this.pnlControls.Controls.Add(this.btnEdit);
            this.pnlControls.Controls.Add(this.btnDelete);
            this.pnlControls.Controls.Add(this.btnRefresh);
            this.pnlControls.Controls.Add(this.btnClose);
            
            // Grid Panel
            this.pnlGrid.Location = new Point(12, 100);
            this.pnlGrid.Size = new Size(976, 450);
            this.pnlGrid.BorderStyle = BorderStyle.FixedSingle;
            
            // DataGridView
            this.dgvEmployees.Location = new Point(5, 5);
            this.dgvEmployees.Size = new Size(966, 440);
            this.dgvEmployees.AllowUserToAddRows = false;
            this.dgvEmployees.AllowUserToDeleteRows = false;
            this.dgvEmployees.ReadOnly = true;
            this.dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvEmployees.MultiSelect = false;
            this.dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEmployees.DoubleClick += new EventHandler(this.dgvEmployees_DoubleClick);
            
            this.pnlGrid.Controls.Add(this.dgvEmployees);
            
            // Add panels to form
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlGrid);
            
            this.ResumeLayout(false);
        }

        private void LoadEmployees()
        {
            try
            {
                string query = @"SELECT 
                    e.employee_id as 'Employee ID',
                    CONCAT(e.first_name, ' ', e.last_name) as 'Full Name',
                    e.email as 'Email',
                    d.department_name as 'Department',
                    jt.job_title as 'Job Title',
                    e.employment_type as 'Employment Type',
                    e.employment_status as 'Status',
                    e.basic_salary as 'Basic Salary',
                    e.hire_date as 'Hire Date'
                FROM employees e
                LEFT JOIN departments d ON e.department_id = d.department_id
                LEFT JOIN job_titles jt ON e.job_title_id = jt.job_title_id
                ORDER BY e.last_name, e.first_name";
                
                DataTable dt = DatabaseManager.ExecuteQuery(query);
                dgvEmployees.DataSource = dt;
                
                // Format columns
                if (dgvEmployees.Columns["Basic Salary"] != null)
                {
                    dgvEmployees.Columns["Basic Salary"].DefaultCellStyle.Format = "C2";
                    dgvEmployees.Columns["Basic Salary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                
                if (dgvEmployees.Columns["Hire Date"] != null)
                {
                    dgvEmployees.Columns["Hire Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                cmbDepartment.Items.Clear();
                cmbDepartment.Items.Add("All Departments");
                
                string query = "SELECT department_name FROM departments ORDER BY department_name";
                DataTable dt = DatabaseManager.ExecuteQuery(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    cmbDepartment.Items.Add(row["department_name"].ToString());
                }
                
                cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterEmployees()
        {
            try
            {
                string whereClause = "WHERE 1=1";
                
                // Search filter
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    whereClause += $" AND (CONCAT(e.first_name, ' ', e.last_name) LIKE '%{txtSearch.Text}%' OR e.employee_id LIKE '%{txtSearch.Text}%' OR e.email LIKE '%{txtSearch.Text}%')";
                }
                
                // Department filter
                if (cmbDepartment.SelectedIndex > 0)
                {
                    whereClause += $" AND d.department_name = '{cmbDepartment.SelectedItem}'";
                }
                
                // Status filter
                if (cmbStatus.SelectedIndex > 0)
                {
                    whereClause += $" AND e.employment_status = '{cmbStatus.SelectedItem}'";
                }
                
                string query = $@"SELECT 
                    e.employee_id as 'Employee ID',
                    CONCAT(e.first_name, ' ', e.last_name) as 'Full Name',
                    e.email as 'Email',
                    d.department_name as 'Department',
                    jt.job_title as 'Job Title',
                    e.employment_type as 'Employment Type',
                    e.employment_status as 'Status',
                    e.basic_salary as 'Basic Salary',
                    e.hire_date as 'Hire Date'
                FROM employees e
                LEFT JOIN departments d ON e.department_id = d.department_id
                LEFT JOIN job_titles jt ON e.job_title_id = jt.job_title_id
                {whereClause}
                ORDER BY e.last_name, e.first_name";
                
                DataTable dt = DatabaseManager.ExecuteQuery(query);
                dgvEmployees.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterEmployees();
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterEmployees();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterEmployees();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (frmEmployee frm = new frmEmployee())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadEmployees();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditSelectedEmployee();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedEmployee();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadEmployees();
            LoadDepartments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvEmployees_DoubleClick(object sender, EventArgs e)
        {
            EditSelectedEmployee();
        }

        private void EditSelectedEmployee()
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                string employeeId = dgvEmployees.SelectedRows[0].Cells["Employee ID"].Value.ToString();
                using (frmEmployee frm = new frmEmployee(employeeId))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadEmployees();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteSelectedEmployee()
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                string employeeId = dgvEmployees.SelectedRows[0].Cells["Employee ID"].Value.ToString();
                string employeeName = dgvEmployees.SelectedRows[0].Cells["Full Name"].Value.ToString();
                
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete employee '{employeeName}'?\n\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string query = "DELETE FROM employees WHERE employee_id = @employeeId";
                        var parameters = new Dictionary<string, object>
                        {
                            { "@employeeId", employeeId }
                        };
                        
                        MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                        int rowsAffected = DatabaseManager.ExecuteNonQuery(query, mysqlParams);
                        
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEmployees();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
