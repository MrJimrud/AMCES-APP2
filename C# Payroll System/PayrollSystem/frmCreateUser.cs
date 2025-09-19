using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmCreateUser : Form
    {
        // DatabaseManager is static - no instance needed
        private bool isEditMode = false;
        private int currentUserId = 0;

        // Controls
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private ComboBox cmbUserType;
        private ComboBox cmbDepartment;
        private CheckBox chkIsActive;
        private DataGridView dgvUsers;
        private Button btnSave;
        private Button btnCancel;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnNew;
        private Button btnRefresh;
        private GroupBox grpUserDetails;
        private GroupBox grpUserList;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblConfirmPassword;
        private Label lblFirstName;
        private Label lblLastName;
        private Label lblEmail;
        private Label lblUserType;
        private Label lblDepartment;
        private Label lblPasswordStrength;
        private ProgressBar pbPasswordStrength;

        public frmCreateUser()
        {
            InitializeComponent();
            LoadDepartments();
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.Text = "User Management";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // Initialize controls
            grpUserDetails = new GroupBox();
            grpUserList = new GroupBox();
            lblUsername = new Label();
            lblPassword = new Label();
            lblConfirmPassword = new Label();
            lblFirstName = new Label();
            lblLastName = new Label();
            lblEmail = new Label();
            lblUserType = new Label();
            lblDepartment = new Label();
            lblPasswordStrength = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            txtEmail = new TextBox();
            cmbUserType = new ComboBox();
            cmbDepartment = new ComboBox();
            chkIsActive = new CheckBox();
            pbPasswordStrength = new ProgressBar();
            dgvUsers = new DataGridView();
            btnSave = new Button();
            btnCancel = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnNew = new Button();
            btnRefresh = new Button();

            // User Details Group
            grpUserDetails.Text = "User Details";
            grpUserDetails.Location = new Point(10, 10);
            grpUserDetails.Size = new Size(400, 350);

            // Labels and Controls for User Details
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(15, 25);
            lblUsername.Size = new Size(80, 20);

            txtUsername.Location = new Point(100, 22);
            txtUsername.Size = new Size(150, 20);

            lblPassword.Text = "Password:";
            lblPassword.Location = new Point(15, 55);
            lblPassword.Size = new Size(80, 20);

            txtPassword.Location = new Point(100, 52);
            txtPassword.Size = new Size(150, 20);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += TxtPassword_TextChanged;

            lblPasswordStrength.Text = "Strength:";
            lblPasswordStrength.Location = new Point(260, 55);
            lblPasswordStrength.Size = new Size(60, 20);

            pbPasswordStrength.Location = new Point(260, 75);
            pbPasswordStrength.Size = new Size(120, 15);
            pbPasswordStrength.Maximum = 100;

            lblConfirmPassword.Text = "Confirm:";
            lblConfirmPassword.Location = new Point(15, 85);
            lblConfirmPassword.Size = new Size(80, 20);

            txtConfirmPassword.Location = new Point(100, 82);
            txtConfirmPassword.Size = new Size(150, 20);
            txtConfirmPassword.UseSystemPasswordChar = true;

            lblFirstName.Text = "First Name:";
            lblFirstName.Location = new Point(15, 115);
            lblFirstName.Size = new Size(80, 20);

            txtFirstName.Location = new Point(100, 112);
            txtFirstName.Size = new Size(150, 20);

            lblLastName.Text = "Last Name:";
            lblLastName.Location = new Point(15, 145);
            lblLastName.Size = new Size(80, 20);

            txtLastName.Location = new Point(100, 142);
            txtLastName.Size = new Size(150, 20);

            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(15, 175);
            lblEmail.Size = new Size(80, 20);

            txtEmail.Location = new Point(100, 172);
            txtEmail.Size = new Size(200, 20);

            lblUserType.Text = "User Type:";
            lblUserType.Location = new Point(15, 205);
            lblUserType.Size = new Size(80, 20);

            cmbUserType.Location = new Point(100, 202);
            cmbUserType.Size = new Size(150, 20);
            cmbUserType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUserType.Items.AddRange(new string[] { "Admin", "HR", "Manager", "Employee" });

            lblDepartment.Text = "Department:";
            lblDepartment.Location = new Point(15, 235);
            lblDepartment.Size = new Size(80, 20);

            cmbDepartment.Location = new Point(100, 232);
            cmbDepartment.Size = new Size(150, 20);
            cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;

            chkIsActive.Text = "Active";
            chkIsActive.Location = new Point(100, 265);
            chkIsActive.Size = new Size(80, 20);
            chkIsActive.Checked = true;

            // Buttons for User Details
            btnNew.Text = "New";
            btnNew.Location = new Point(50, 300);
            btnNew.Size = new Size(70, 30);
            btnNew.Click += BtnNew_Click;

            btnSave.Text = "Save";
            btnSave.Location = new Point(130, 300);
            btnSave.Size = new Size(70, 30);
            btnSave.Click += BtnSave_Click;
            btnSave.Enabled = false;

            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(210, 300);
            btnCancel.Size = new Size(70, 30);
            btnCancel.Click += BtnCancel_Click;

            // User List Group
            grpUserList.Text = "User List";
            grpUserList.Location = new Point(420, 10);
            grpUserList.Size = new Size(450, 500);

            // DataGridView for Users
            dgvUsers.Location = new Point(10, 25);
            dgvUsers.Size = new Size(430, 400);
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;

            // Buttons for User List
            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(200, 440);
            btnEdit.Size = new Size(70, 30);
            btnEdit.Click += BtnEdit_Click;

            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(280, 440);
            btnDelete.Size = new Size(70, 30);
            btnDelete.Click += BtnDelete_Click;

            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(360, 440);
            btnRefresh.Size = new Size(70, 30);
            btnRefresh.Click += BtnRefresh_Click;

            // Add controls to groups
            grpUserDetails.Controls.AddRange(new Control[] {
                lblUsername, txtUsername, lblPassword, txtPassword, lblPasswordStrength, pbPasswordStrength,
                lblConfirmPassword, txtConfirmPassword, lblFirstName, txtFirstName, lblLastName, txtLastName,
                lblEmail, txtEmail, lblUserType, cmbUserType, lblDepartment, cmbDepartment,
                chkIsActive, btnNew, btnSave, btnCancel
            });

            grpUserList.Controls.AddRange(new Control[] {
                dgvUsers, btnEdit, btnDelete, btnRefresh
            });

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                grpUserDetails, grpUserList
            });
        }

        private void LoadDepartments()
        {
            try
            {
                string query = "SELECT department_id as DepartmentId, department_name FROM departments WHERE is_active = 1 ORDER BY department_name";
                DataTable dt = DatabaseManager.GetDataTable(query);

                cmbDepartment.DisplayMember = "department_name";
                cmbDepartment.ValueMember = "DepartmentId";
                cmbDepartment.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers()
        {
            try
            {
                string query = @"SELECT u.UserId, u.Username, u.FirstName, u.LastName, u.Email, u.UserType,
                               d.DepartmentName, u.IsActive, u.CreatedDate
                               FROM Users u
                               LEFT JOIN Departments d ON u.DepartmentId = d.DepartmentId
                               ORDER BY u.Username";
                DataTable dt = DatabaseManager.GetDataTable(query);
                dgvUsers.DataSource = dt;

                // Hide UserId column
                if (dgvUsers.Columns["UserId"] != null)
                    dgvUsers.Columns["UserId"].Visible = false;

                // Format columns
                if (dgvUsers.Columns["CreatedDate"] != null)
                    dgvUsers.Columns["CreatedDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            UpdatePasswordStrength();
        }

        private void UpdatePasswordStrength()
        {
            string password = txtPassword.Text;
            int strength = CalculatePasswordStrength(password);
            pbPasswordStrength.Value = strength;

            if (strength < 30)
                pbPasswordStrength.ForeColor = Color.Red;
            else if (strength < 70)
                pbPasswordStrength.ForeColor = Color.Orange;
            else
                pbPasswordStrength.ForeColor = Color.Green;
        }

        private int CalculatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
                return 0;

            int score = 0;
            
            // Length
            if (password.Length >= 8) score += 25;
            else if (password.Length >= 6) score += 15;
            else if (password.Length >= 4) score += 10;

            // Character variety
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) score += 15;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) score += 15;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]")) score += 15;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[^a-zA-Z0-9]")) score += 30;

            return Math.Min(score, 100);
        }

        private void DgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvUsers.SelectedRows[0];
                currentUserId = Convert.ToInt32(row.Cells["UserId"].Value);
                LoadUserDetails(currentUserId);
            }
        }

        private void LoadUserDetails(int userId)
        {
            try
            {
                string query = "SELECT * FROM Users WHERE UserId = @UserId";
                var parameters = new[] { DatabaseManager.CreateParameter("@UserId", userId) };
                DataTable dt = DatabaseManager.GetDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtUsername.Text = row["Username"].ToString();
                    txtFirstName.Text = row["FirstName"].ToString();
                    txtLastName.Text = row["LastName"].ToString();
                    txtEmail.Text = row["Email"].ToString();
                    cmbUserType.Text = row["UserType"].ToString();
                    
                    if (row["DepartmentId"] != DBNull.Value)
                        cmbDepartment.SelectedValue = row["DepartmentId"];
                    
                    chkIsActive.Checked = Convert.ToBoolean(row["IsActive"]);
                    
                    // Clear password fields for security
                    txtPassword.Clear();
                    txtConfirmPassword.Clear();
                    pbPasswordStrength.Value = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
            isEditMode = true;
            currentUserId = 0;
            btnSave.Enabled = true;
            txtUsername.Focus();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (currentUserId > 0)
            {
                isEditMode = true;
                btnSave.Enabled = true;
                txtUsername.Focus();
            }
            else
            {
                MessageBox.Show("Please select a user to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SaveUser();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                ClearFields();
                isEditMode = false;
                btnSave.Enabled = false;
                currentUserId = 0;
            }
            else
            {
                this.Close();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (currentUserId > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteUser();
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (currentUserId == 0 && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required for new users.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Password and confirm password do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmPassword.Focus();
                    return false;
                }

                if (CalculatePasswordStrength(txtPassword.Text) < 30)
                {
                    MessageBox.Show("Password is too weak. Please use a stronger password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return false;
            }

            if (cmbUserType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a user type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbUserType.Focus();
                return false;
            }

            return true;
        }

        private void SaveUser()
        {
            try
            {
                string query;
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@Username"] = txtUsername.Text.Trim(),
                    ["@FirstName"] = txtFirstName.Text.Trim(),
                    ["@LastName"] = txtLastName.Text.Trim(),
                    ["@Email"] = txtEmail.Text.Trim(),
                    ["@UserType"] = cmbUserType.Text,
                    ["@DepartmentId"] = cmbDepartment.SelectedValue,
                    ["@IsActive"] = chkIsActive.Checked,
                    ["@ModifiedDate"] = DateTime.Now
                };

                if (currentUserId == 0)
                {
                    // Insert new user
                    query = @"INSERT INTO Users (Username, PasswordHash, FirstName, LastName, Email, UserType, DepartmentId, IsActive, CreatedDate, ModifiedDate)
                             VALUES (@Username, @PasswordHash, @FirstName, @LastName, @Email, @UserType, @DepartmentId, @IsActive, @CreatedDate, @ModifiedDate)";
                    parameters["@PasswordHash"] = HashPassword(txtPassword.Text);
                    parameters["@CreatedDate"] = DateTime.Now;
                }
                else
                {
                    // Update existing user
                    if (string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        query = @"UPDATE Users SET Username = @Username, FirstName = @FirstName, LastName = @LastName, Email = @Email,
                                 UserType = @UserType, DepartmentId = @DepartmentId, IsActive = @IsActive, ModifiedDate = @ModifiedDate
                                 WHERE UserId = @UserId";
                    }
                    else
                    {
                        query = @"UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, FirstName = @FirstName, LastName = @LastName, Email = @Email,
                                 UserType = @UserType, DepartmentId = @DepartmentId, IsActive = @IsActive, ModifiedDate = @ModifiedDate
                                 WHERE UserId = @UserId";
                        parameters["@PasswordHash"] = HashPassword(txtPassword.Text);
                    }
                    parameters["@UserId"] = currentUserId;
                }

                int result = DatabaseManager.ExecuteNonQuery(query, parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray());

                if (result > 0)
                {
                    MessageBox.Show("User saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                    ClearFields();
                    isEditMode = false;
                    btnSave.Enabled = false;
                    currentUserId = 0;
                }
                else
                {
                    MessageBox.Show("Failed to save user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteUser()
        {
            try
            {
                string query = "UPDATE Users SET IsActive = 0, ModifiedDate = @ModifiedDate WHERE UserId = @UserId";
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@UserId"] = currentUserId,
                    ["@ModifiedDate"] = DateTime.Now
                };

                int result = DatabaseManager.ExecuteNonQuery(query, parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray());

                if (result > 0)
                {
                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                    ClearFields();
                    currentUserId = 0;
                }
                else
                {
                    MessageBox.Show("Failed to delete user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            cmbUserType.SelectedIndex = -1;
            cmbDepartment.SelectedIndex = -1;
            chkIsActive.Checked = true;
            pbPasswordStrength.Value = 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // DatabaseManager is static - no disposal needed
            }
            base.Dispose(disposing);
        }
    }
}
