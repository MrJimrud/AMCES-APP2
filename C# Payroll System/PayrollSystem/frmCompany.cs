using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySqlConnector;

namespace PayrollSystem
{
    public partial class frmCompany : Form
    {
        // DatabaseManager is static - no instance needed
        private bool isEditMode = false;
        private int currentCompanyId = 0;

        // Controls
        private TextBox txtCompanyName;
        private TextBox txtAddress;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtTIN;
        private TextBox txtSSS;
        private TextBox txtPhilHealth;
        private TextBox txtPagIbig;
        private PictureBox picLogo;
        private Button btnBrowseLogo;
        private Button btnSave;
        private Button btnCancel;
        private Button btnEdit;
        private Button btnNew;
        private GroupBox grpCompanyInfo;
        private GroupBox grpGovernmentNumbers;
        private Label lblCompanyName;
        private Label lblAddress;
        private Label lblPhone;
        private Label lblEmail;
        private Label lblTIN;
        private Label lblSSS;
        private Label lblPhilHealth;
        private Label lblPagIbig;
        private Label lblLogo;
        private OpenFileDialog openFileDialog;

        public frmCompany()
        {
            InitializeComponent();
            LoadCompanyInfo();
        }

        private void InitializeComponent()
        {
            this.Text = "Company Information";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Initialize controls
            grpCompanyInfo = new GroupBox();
            grpGovernmentNumbers = new GroupBox();
            lblCompanyName = new Label();
            lblAddress = new Label();
            lblPhone = new Label();
            lblEmail = new Label();
            lblTIN = new Label();
            lblSSS = new Label();
            lblPhilHealth = new Label();
            lblPagIbig = new Label();
            lblLogo = new Label();
            txtCompanyName = new TextBox();
            txtAddress = new TextBox();
            txtPhone = new TextBox();
            txtEmail = new TextBox();
            txtTIN = new TextBox();
            txtSSS = new TextBox();
            txtPhilHealth = new TextBox();
            txtPagIbig = new TextBox();
            picLogo = new PictureBox();
            btnBrowseLogo = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            btnEdit = new Button();
            btnNew = new Button();
            openFileDialog = new OpenFileDialog();

            // Company Info Group
            grpCompanyInfo.Text = "Company Information";
            grpCompanyInfo.Location = new Point(10, 10);
            grpCompanyInfo.Size = new Size(560, 200);

            // Labels and TextBoxes for Company Info
            lblCompanyName.Text = "Company Name:";
            lblCompanyName.Location = new Point(15, 25);
            lblCompanyName.Size = new Size(100, 20);

            txtCompanyName.Location = new Point(120, 22);
            txtCompanyName.Size = new Size(200, 20);

            lblAddress.Text = "Address:";
            lblAddress.Location = new Point(15, 55);
            lblAddress.Size = new Size(100, 20);

            txtAddress.Location = new Point(120, 52);
            txtAddress.Size = new Size(300, 20);
            txtAddress.Multiline = true;
            txtAddress.Height = 40;

            lblPhone.Text = "Phone:";
            lblPhone.Location = new Point(15, 105);
            lblPhone.Size = new Size(100, 20);

            txtPhone.Location = new Point(120, 102);
            txtPhone.Size = new Size(150, 20);

            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(15, 135);
            lblEmail.Size = new Size(100, 20);

            txtEmail.Location = new Point(120, 132);
            txtEmail.Size = new Size(200, 20);

            lblLogo.Text = "Logo:";
            lblLogo.Location = new Point(350, 25);
            lblLogo.Size = new Size(50, 20);

            picLogo.Location = new Point(350, 45);
            picLogo.Size = new Size(100, 80);
            picLogo.BorderStyle = BorderStyle.FixedSingle;
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;

            btnBrowseLogo.Text = "Browse...";
            btnBrowseLogo.Location = new Point(350, 130);
            btnBrowseLogo.Size = new Size(100, 25);
            btnBrowseLogo.Click += BtnBrowseLogo_Click;

            // Government Numbers Group
            grpGovernmentNumbers.Text = "Government Numbers";
            grpGovernmentNumbers.Location = new Point(10, 220);
            grpGovernmentNumbers.Size = new Size(560, 120);

            lblTIN.Text = "TIN:";
            lblTIN.Location = new Point(15, 25);
            lblTIN.Size = new Size(100, 20);

            txtTIN.Location = new Point(120, 22);
            txtTIN.Size = new Size(150, 20);

            lblSSS.Text = "SSS:";
            lblSSS.Location = new Point(290, 25);
            lblSSS.Size = new Size(50, 20);

            txtSSS.Location = new Point(350, 22);
            txtSSS.Size = new Size(150, 20);

            lblPhilHealth.Text = "PhilHealth:";
            lblPhilHealth.Location = new Point(15, 55);
            lblPhilHealth.Size = new Size(100, 20);

            txtPhilHealth.Location = new Point(120, 52);
            txtPhilHealth.Size = new Size(150, 20);

            lblPagIbig.Text = "Pag-IBIG:";
            lblPagIbig.Location = new Point(290, 55);
            lblPagIbig.Size = new Size(50, 20);

            txtPagIbig.Location = new Point(350, 52);
            txtPagIbig.Size = new Size(150, 20);

            // Buttons
            btnNew.Text = "New";
            btnNew.Location = new Point(200, 360);
            btnNew.Size = new Size(75, 30);
            btnNew.Click += BtnNew_Click;

            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(285, 360);
            btnEdit.Size = new Size(75, 30);
            btnEdit.Click += BtnEdit_Click;

            btnSave.Text = "Save";
            btnSave.Location = new Point(370, 360);
            btnSave.Size = new Size(75, 30);
            btnSave.Click += BtnSave_Click;
            btnSave.Enabled = false;

            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(455, 360);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Click += BtnCancel_Click;

            // Add controls to groups
            grpCompanyInfo.Controls.AddRange(new Control[] {
                lblCompanyName, txtCompanyName, lblAddress, txtAddress,
                lblPhone, txtPhone, lblEmail, txtEmail,
                lblLogo, picLogo, btnBrowseLogo
            });

            grpGovernmentNumbers.Controls.AddRange(new Control[] {
                lblTIN, txtTIN, lblSSS, txtSSS,
                lblPhilHealth, txtPhilHealth, lblPagIbig, txtPagIbig
            });

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                grpCompanyInfo, grpGovernmentNumbers,
                btnNew, btnEdit, btnSave, btnCancel
            });

            // OpenFileDialog settings
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Select Company Logo";
        }

        private void LoadCompanyInfo()
        {
            try
            {
                string query = "SELECT * FROM company_settings WHERE is_active = 1 ORDER BY company_id DESC LIMIT 1";
                DataTable dt = DatabaseManager.GetDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    currentCompanyId = Convert.ToInt32(row["CompanyId"]);
                    txtCompanyName.Text = row["CompanyName"].ToString();
                    txtAddress.Text = row["Address"].ToString();
                    txtPhone.Text = row["Phone"].ToString();
                    txtEmail.Text = row["Email"].ToString();
                    txtTIN.Text = row["TIN"].ToString();
                    txtSSS.Text = row["SSS"].ToString();
                    txtPhilHealth.Text = row["PhilHealth"].ToString();
                    txtPagIbig.Text = row["PagIbig"].ToString();

                    // Load logo if exists
                    if (row["Logo"] != DBNull.Value)
                    {
                        byte[] logoBytes = (byte[])row["Logo"];
                        if (logoBytes.Length > 0)
                        {
                            using (var ms = new System.IO.MemoryStream(logoBytes))
                            {
                                picLogo.Image = Image.FromStream(ms);
                            }
                        }
                    }

                    SetControlsReadOnly(true);
                }
                else
                {
                    // No company info exists, enable for new entry
                    isEditMode = true;
                    SetControlsReadOnly(false);
                    btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading company information: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetControlsReadOnly(bool readOnly)
        {
            txtCompanyName.ReadOnly = readOnly;
            txtAddress.ReadOnly = readOnly;
            txtPhone.ReadOnly = readOnly;
            txtEmail.ReadOnly = readOnly;
            txtTIN.ReadOnly = readOnly;
            txtSSS.ReadOnly = readOnly;
            txtPhilHealth.ReadOnly = readOnly;
            txtPagIbig.ReadOnly = readOnly;
            btnBrowseLogo.Enabled = !readOnly;
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
            isEditMode = true;
            currentCompanyId = 0;
            SetControlsReadOnly(false);
            btnSave.Enabled = true;
            txtCompanyName.Focus();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (currentCompanyId > 0)
            {
                isEditMode = true;
                SetControlsReadOnly(false);
                btnSave.Enabled = true;
                txtCompanyName.Focus();
            }
            else
            {
                MessageBox.Show("No company information to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SaveCompanyInfo();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                LoadCompanyInfo();
                isEditMode = false;
                btnSave.Enabled = false;
            }
            else
            {
                this.Close();
            }
        }

        private void BtnBrowseLogo_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    picLogo.Image = Image.FromFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                MessageBox.Show("Company name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCompanyName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return false;
            }

            return true;
        }

        private void SaveCompanyInfo()
        {
            try
            {
                byte[] logoBytes = null;
                if (picLogo.Image != null)
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        picLogo.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        logoBytes = ms.ToArray();
                    }
                }

                string query;
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["@CompanyName"] = txtCompanyName.Text.Trim(),
                    ["@Address"] = txtAddress.Text.Trim(),
                    ["@Phone"] = txtPhone.Text.Trim(),
                    ["@Email"] = txtEmail.Text.Trim(),
                    ["@TIN"] = txtTIN.Text.Trim(),
                    ["@SSS"] = txtSSS.Text.Trim(),
                    ["@PhilHealth"] = txtPhilHealth.Text.Trim(),
                    ["@PagIbig"] = txtPagIbig.Text.Trim(),
                    ["@Logo"] = logoBytes,
                    ["@IsActive"] = true,
                    ["@ModifiedDate"] = DateTime.Now
                };

                if (currentCompanyId == 0)
                {
                    // Insert new company
                    query = @"INSERT INTO Company (CompanyName, Address, Phone, Email, TIN, SSS, PhilHealth, PagIbig, Logo, IsActive, CreatedDate, ModifiedDate)
                             VALUES (@CompanyName, @Address, @Phone, @Email, @TIN, @SSS, @PhilHealth, @PagIbig, @Logo, @IsActive, @ModifiedDate, @ModifiedDate)";
                    parameters["@CreatedDate"] = DateTime.Now;
                }
                else
                {
                    // Update existing company
                    query = @"UPDATE Company SET CompanyName = @CompanyName, Address = @Address, Phone = @Phone, Email = @Email,
                             TIN = @TIN, SSS = @SSS, PhilHealth = @PhilHealth, PagIbig = @PagIbig, Logo = @Logo, ModifiedDate = @ModifiedDate
                             WHERE CompanyId = @CompanyId";
                    parameters["@CompanyId"] = currentCompanyId;
                }

                MySqlParameter[] mysqlParams = parameters.Select(p => DatabaseManager.CreateParameter(p.Key, p.Value)).ToArray();
                int result = DatabaseManager.ExecuteNonQuery(query, mysqlParams);

                if (result > 0)
                {
                    MessageBox.Show("Company information saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isEditMode = false;
                    SetControlsReadOnly(true);
                    btnSave.Enabled = false;
                    
                    if (currentCompanyId == 0)
                    {
                        LoadCompanyInfo(); // Reload to get the new ID
                    }
                }
                else
                {
                    MessageBox.Show("Failed to save company information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving company information: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtCompanyName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtTIN.Clear();
            txtSSS.Clear();
            txtPhilHealth.Clear();
            txtPagIbig.Clear();
            picLogo.Image = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // DatabaseManager is static - no disposal needed
                // dbManager?.Dispose();
                openFileDialog?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
