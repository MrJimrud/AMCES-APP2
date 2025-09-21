using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;
using System.IO;
using System.Collections.Generic;

namespace PayrollSystem
{
    public partial class frmEmployee : Form
    {
        private bool isEditing = false;
        private string currentEmployeeId = "";
        private string imagePath = "";
        private TextBox txtEmployeeId;
        private Label lblEmployeeId;

        public frmEmployee()
        {
            InitializeComponent();
            this.Load += FrmEmployee_Load;
        }

        public frmEmployee(string employeeId)
        {
            InitializeComponent();
            this.Load += FrmEmployee_Load;
            // Store the employeeId as string since it's VARCHAR in database
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                MessageBox.Show("Invalid Employee ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }
            currentEmployeeId = employeeId;
            isEditing = true;
        }

        private void InitializeComponent()
        {
            this.panelHeader = new Panel();
            this.lblTitle = new Label();
            this.tabControl = new TabControl();
            this.tabPersonal = new TabPage();
            this.tabEmployment = new TabPage();
            this.tabSalary = new TabPage();
            this.tabContact = new TabPage();
            
            // Personal Info Tab Controls
            this.picEmployee = new PictureBox();
            this.btnBrowseImage = new Button();
            this.lblEmployeeId = new Label();
            this.txtEmployeeId = new TextBox();
            this.lblFirstName = new Label();
            this.txtFirstName = new TextBox();
            this.lblMiddleName = new Label();
            this.txtMiddleName = new TextBox();
            this.lblLastName = new Label();
            this.txtLastName = new TextBox();
            this.lblGender = new Label();
            this.cmbGender = new ComboBox();
            this.lblBirthDate = new Label();
            this.dtpBirthDate = new DateTimePicker();
            this.lblCivilStatus = new Label();
            this.cmbCivilStatus = new ComboBox();
            this.lblNationality = new Label();
            this.txtNationality = new TextBox();
            this.lblReligion = new Label();
            this.txtReligion = new TextBox();
            
            // Employment Tab Controls
            this.lblDepartment = new Label();
            this.cmbDepartment = new ComboBox();
            this.lblJobTitle = new Label();
            this.cmbJobTitle = new ComboBox();
            this.lblEmploymentType = new Label();
            this.cmbEmploymentType = new ComboBox();
            this.lblDateHired = new Label();
            this.dtpDateHired = new DateTimePicker();
            this.lblEmploymentStatus = new Label();
            this.cmbEmploymentStatus = new ComboBox();
            this.lblImmediateSupervisor = new Label();
            this.cmbSupervisor = new ComboBox();
            
            // Salary Tab Controls
            this.lblBasicSalary = new Label();
            this.txtBasicSalary = new TextBox();
            this.lblAllowances = new Label();
            this.txtAllowances = new TextBox();
            this.lblPayrollType = new Label();
            this.cmbPayrollType = new ComboBox();
            this.lblBankAccount = new Label();
            this.txtBankAccount = new TextBox();
            this.lblBankName = new Label();
            this.txtBankName = new TextBox();
            
            // Contact Tab Controls
            this.lblAddress = new Label();
            this.txtAddress = new TextBox();
            this.lblCity = new Label();
            this.txtCity = new TextBox();
            this.lblProvince = new Label();
            this.txtProvince = new TextBox();
            this.lblZipCode = new Label();
            this.txtZipCode = new TextBox();
            this.lblPhone = new Label();
            this.txtPhone = new TextBox();
            this.lblMobile = new Label();
            this.txtMobile = new TextBox();
            this.lblEmail = new Label();
            this.txtEmail = new TextBox();
            this.lblEmergencyContact = new Label();
            this.txtEmergencyContact = new TextBox();
            this.lblEmergencyPhone = new Label();
            this.txtEmergencyPhone = new TextBox();
            
            // Buttons
            this.panelButtons = new Panel();
            this.btnSave = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnNew = new Button();
            this.btnClose = new Button();
            
            this.panelHeader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPersonal.SuspendLayout();
            this.tabEmployment.SuspendLayout();
            this.tabSalary.SuspendLayout();
            this.tabContact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmployee)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = Color.FromArgb(52, 73, 94);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = DockStyle.Top;
            this.panelHeader.Location = new Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(900, 60);
            this.panelHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(220, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Employee Management";

            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPersonal);
            this.tabControl.Controls.Add(this.tabEmployment);
            this.tabControl.Controls.Add(this.tabSalary);
            this.tabControl.Controls.Add(this.tabContact);
            this.tabControl.Font = new Font("Segoe UI", 10F);
            this.tabControl.Location = new Point(20, 80);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(860, 550);
            this.tabControl.TabIndex = 1;

            // 
            // tabPersonal
            // 
            this.tabPersonal.BackColor = Color.White;
            this.tabPersonal.Controls.Add(this.txtReligion);
            this.tabPersonal.Controls.Add(this.lblReligion);
            this.tabPersonal.Controls.Add(this.txtNationality);
            this.tabPersonal.Controls.Add(this.lblNationality);
            this.tabPersonal.Controls.Add(this.cmbCivilStatus);
            this.tabPersonal.Controls.Add(this.lblCivilStatus);
            this.tabPersonal.Controls.Add(this.dtpBirthDate);
            this.tabPersonal.Controls.Add(this.lblBirthDate);
            this.tabPersonal.Controls.Add(this.cmbGender);
            this.tabPersonal.Controls.Add(this.lblGender);
            this.tabPersonal.Controls.Add(this.txtLastName);
            this.tabPersonal.Controls.Add(this.lblLastName);
            this.tabPersonal.Controls.Add(this.txtMiddleName);
            this.tabPersonal.Controls.Add(this.lblMiddleName);
            this.tabPersonal.Controls.Add(this.txtFirstName);
            this.tabPersonal.Controls.Add(this.lblFirstName);
            this.tabPersonal.Controls.Add(this.txtEmployeeId);
            this.tabPersonal.Controls.Add(this.lblEmployeeId);
            this.tabPersonal.Controls.Add(this.btnBrowseImage);
            this.tabPersonal.Controls.Add(this.picEmployee);
            this.tabPersonal.Location = new Point(4, 28);
            this.tabPersonal.Name = "tabPersonal";
            this.tabPersonal.Padding = new Padding(3);
            this.tabPersonal.Size = new Size(852, 418);
            this.tabPersonal.TabIndex = 0;
            this.tabPersonal.Text = "Personal Information";

            // 
            // picEmployee
            // 
            this.picEmployee.BorderStyle = BorderStyle.FixedSingle;
            this.picEmployee.Location = new Point(20, 20);
            this.picEmployee.Name = "picEmployee";
            this.picEmployee.Size = new Size(150, 180);
            this.picEmployee.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picEmployee.TabIndex = 0;
            this.picEmployee.TabStop = false;

            // 
            // btnBrowseImage
            // 
            this.btnBrowseImage.BackColor = Color.FromArgb(52, 152, 219);
            this.btnBrowseImage.FlatAppearance.BorderSize = 0;
            this.btnBrowseImage.FlatStyle = FlatStyle.Flat;
            this.btnBrowseImage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnBrowseImage.ForeColor = Color.White;
            this.btnBrowseImage.Location = new Point(20, 210);
            this.btnBrowseImage.Name = "btnBrowseImage";
            this.btnBrowseImage.Size = new Size(150, 30);
            this.btnBrowseImage.TabIndex = 1;
            this.btnBrowseImage.Text = "Browse Image";
            this.btnBrowseImage.UseVisualStyleBackColor = false;
            this.btnBrowseImage.Click += BtnBrowseImage_Click;

            // Personal Info Controls Layout
            int startX = 200, startY = 20, labelWidth = 120, controlWidth = 200, rowHeight = 40;
            
            // Employee ID
            this.lblEmployeeId.AutoSize = true;
            this.lblEmployeeId.Location = new Point(startX, startY);
            this.lblEmployeeId.Name = "lblEmployeeId";
            this.lblEmployeeId.Size = new Size(100, 19);
            this.lblEmployeeId.Text = "Employee ID:";
            
            this.txtEmployeeId.Location = new Point(startX + labelWidth, startY - 3);
            this.txtEmployeeId.Name = "txtEmployeeId";
            this.txtEmployeeId.Size = new Size(controlWidth, 25);
            this.txtEmployeeId.ReadOnly = true;
            this.txtEmployeeId.BackColor = Color.LightGray;

            // First Name
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new Point(startX, startY + rowHeight);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new Size(75, 19);
            this.lblFirstName.Text = "First Name:";
            
            this.txtFirstName.Location = new Point(startX + labelWidth, startY + rowHeight - 3);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new Size(controlWidth, 25);

            // Middle Name
            this.lblMiddleName.AutoSize = true;
            this.lblMiddleName.Location = new Point(startX, startY + rowHeight * 2);
            this.lblMiddleName.Name = "lblMiddleName";
            this.lblMiddleName.Size = new Size(85, 19);
            this.lblMiddleName.Text = "Middle Name:";
            
            this.txtMiddleName.Location = new Point(startX + labelWidth, startY + rowHeight * 2 - 3);
            this.txtMiddleName.Name = "txtMiddleName";
            this.txtMiddleName.Size = new Size(controlWidth, 25);

            // Last Name
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new Point(startX, startY + rowHeight * 3);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new Size(72, 19);
            this.lblLastName.Text = "Last Name:";
            
            this.txtLastName.Location = new Point(startX + labelWidth, startY + rowHeight * 3 - 3);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new Size(controlWidth, 25);

            // Gender
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new Point(startX, startY + rowHeight * 4);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new Size(54, 19);
            this.lblGender.Text = "Gender:";
            
            this.cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] { "Male", "Female" });
            this.cmbGender.Location = new Point(startX + labelWidth, startY + rowHeight * 4 - 3);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new Size(controlWidth, 25);

            // Birth Date
            this.lblBirthDate.AutoSize = true;
            this.lblBirthDate.Location = new Point(startX, startY + rowHeight * 5);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new Size(72, 19);
            this.lblBirthDate.Text = "Birth Date:";
            
            this.dtpBirthDate.Format = DateTimePickerFormat.Short;
            this.dtpBirthDate.Location = new Point(startX + labelWidth, startY + rowHeight * 5 - 3);
            this.dtpBirthDate.Name = "dtpBirthDate";
            this.dtpBirthDate.Size = new Size(controlWidth, 25);
            this.dtpBirthDate.MinDate = new DateTime(1753, 1, 1);
            this.dtpBirthDate.MaxDate = DateTime.Today;

            // Civil Status
            this.lblCivilStatus.AutoSize = true;
            this.lblCivilStatus.Location = new Point(startX, startY + rowHeight * 6);
            this.lblCivilStatus.Name = "lblCivilStatus";
            this.lblCivilStatus.Size = new Size(78, 19);
            this.lblCivilStatus.Text = "Civil Status:";
            
            this.cmbCivilStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCivilStatus.FormattingEnabled = true;
            this.cmbCivilStatus.Items.AddRange(new object[] { "Single", "Married", "Divorced", "Widowed" });
            this.cmbCivilStatus.Location = new Point(startX + labelWidth, startY + rowHeight * 6 - 3);
            this.cmbCivilStatus.Name = "cmbCivilStatus";
            this.cmbCivilStatus.Size = new Size(controlWidth, 25);

            // Second column
            int col2X = startX + 350;
            
            // Nationality
            this.lblNationality.AutoSize = true;
            this.lblNationality.Location = new Point(col2X, startY);
            this.lblNationality.Name = "lblNationality";
            this.lblNationality.Size = new Size(78, 19);
            this.lblNationality.Text = "Nationality:";
            
            this.txtNationality.Location = new Point(col2X + labelWidth, startY - 3);
            this.txtNationality.Name = "txtNationality";
            this.txtNationality.Size = new Size(controlWidth, 25);

            // Religion
            this.lblReligion.AutoSize = true;
            this.lblReligion.Location = new Point(col2X, startY + rowHeight);
            this.lblReligion.Name = "lblReligion";
            this.lblReligion.Size = new Size(58, 19);
            this.lblReligion.Text = "Religion:";
            
            this.txtReligion.Location = new Point(col2X + labelWidth, startY + rowHeight - 3);
            this.txtReligion.Name = "txtReligion";
            this.txtReligion.Size = new Size(controlWidth, 25);

            // 
            // tabEmployment
            // 
            this.tabEmployment.BackColor = Color.White;
            this.tabEmployment.Controls.Add(this.cmbSupervisor);
            this.tabEmployment.Controls.Add(this.lblImmediateSupervisor);
            this.tabEmployment.Controls.Add(this.cmbEmploymentStatus);
            this.tabEmployment.Controls.Add(this.lblEmploymentStatus);
            this.tabEmployment.Controls.Add(this.dtpDateHired);
            this.tabEmployment.Controls.Add(this.lblDateHired);
            this.tabEmployment.Controls.Add(this.cmbEmploymentType);
            this.tabEmployment.Controls.Add(this.lblEmploymentType);
            this.tabEmployment.Controls.Add(this.cmbJobTitle);
            this.tabEmployment.Controls.Add(this.lblJobTitle);
            this.tabEmployment.Controls.Add(this.cmbDepartment);
            this.tabEmployment.Controls.Add(this.lblDepartment);
            this.tabEmployment.Location = new Point(4, 28);
            this.tabEmployment.Name = "tabEmployment";
            this.tabEmployment.Padding = new Padding(3);
            this.tabEmployment.Size = new Size(852, 418);
            this.tabEmployment.TabIndex = 1;
            this.tabEmployment.Text = "Employment Details";

            // Employment Controls Layout
            startX = 50;
            startY = 50;
            
            // Department
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Location = new Point(startX, startY);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new Size(82, 19);
            this.lblDepartment.Text = "Department:";
            
            this.cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new Point(startX + labelWidth + 50, startY - 3);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new Size(300, 25);

            // Job Title
            this.lblJobTitle.AutoSize = true;
            this.lblJobTitle.Location = new Point(startX, startY + rowHeight);
            this.lblJobTitle.Name = "lblJobTitle";
            this.lblJobTitle.Size = new Size(63, 19);
            this.lblJobTitle.Text = "Job Title:";
            
            this.cmbJobTitle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbJobTitle.FormattingEnabled = true;
            this.cmbJobTitle.Location = new Point(startX + labelWidth + 50, startY + rowHeight - 3);
            this.cmbJobTitle.Name = "cmbJobTitle";
            this.cmbJobTitle.Size = new Size(300, 25);

            // Employment Type
            this.lblEmploymentType.AutoSize = true;
            this.lblEmploymentType.Location = new Point(startX, startY + rowHeight * 2);
            this.lblEmploymentType.Name = "lblEmploymentType";
            this.lblEmploymentType.Size = new Size(115, 19);
            this.lblEmploymentType.Text = "Employment Type:";
            
            this.cmbEmploymentType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEmploymentType.FormattingEnabled = true;
            this.cmbEmploymentType.Items.AddRange(new object[] { "Regular", "Contractual", "Probationary", "Part-time" });
            this.cmbEmploymentType.Location = new Point(startX + labelWidth + 50, startY + rowHeight * 2 - 3);
            this.cmbEmploymentType.Name = "cmbEmploymentType";
            this.cmbEmploymentType.Size = new Size(300, 25);

            // Date Hired
            this.lblDateHired.AutoSize = true;
            this.lblDateHired.Location = new Point(startX, startY + rowHeight * 3);
            this.lblDateHired.Name = "lblDateHired";
            this.lblDateHired.Size = new Size(75, 19);
            this.lblDateHired.Text = "Date Hired:";
            
            this.dtpDateHired.Format = DateTimePickerFormat.Short;
            this.dtpDateHired.Location = new Point(startX + labelWidth + 50, startY + rowHeight * 3 - 3);
            this.dtpDateHired.Name = "dtpDateHired";
            this.dtpDateHired.Size = new Size(300, 25);
            this.dtpDateHired.MinDate = new DateTime(1753, 1, 1);
            this.dtpDateHired.MaxDate = DateTime.Today;

            // Employment Status
            this.lblEmploymentStatus.AutoSize = true;
            this.lblEmploymentStatus.Location = new Point(startX, startY + rowHeight * 4);
            this.lblEmploymentStatus.Name = "lblEmploymentStatus";
            this.lblEmploymentStatus.Size = new Size(125, 19);
            this.lblEmploymentStatus.Text = "Employment Status:";
            
            this.cmbEmploymentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEmploymentStatus.FormattingEnabled = true;
            this.cmbEmploymentStatus.Items.AddRange(new object[] { "Active", "Inactive", "Terminated", "Resigned" });
            this.cmbEmploymentStatus.Location = new Point(startX + labelWidth + 50, startY + rowHeight * 4 - 3);
            this.cmbEmploymentStatus.Name = "cmbEmploymentStatus";
            this.cmbEmploymentStatus.Size = new Size(300, 25);

            // Immediate Supervisor
            this.lblImmediateSupervisor.AutoSize = true;
            this.lblImmediateSupervisor.Location = new Point(startX, startY + rowHeight * 5);
            this.lblImmediateSupervisor.Name = "lblImmediateSupervisor";
            this.lblImmediateSupervisor.Size = new Size(140, 19);
            this.lblImmediateSupervisor.Text = "Immediate Supervisor:";
            
            this.cmbSupervisor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSupervisor.FormattingEnabled = true;
            this.cmbSupervisor.Location = new Point(startX + labelWidth + 50, startY + rowHeight * 5 - 3);
            this.cmbSupervisor.Name = "cmbSupervisor";
            this.cmbSupervisor.Size = new Size(300, 25);

            // 
            // tabSalary
            // 
            this.tabSalary.BackColor = Color.White;
            this.tabSalary.Controls.Add(this.txtBankName);
            this.tabSalary.Controls.Add(this.lblBankName);
            this.tabSalary.Controls.Add(this.txtBankAccount);
            this.tabSalary.Controls.Add(this.lblBankAccount);
            this.tabSalary.Controls.Add(this.cmbPayrollType);
            this.tabSalary.Controls.Add(this.lblPayrollType);
            this.tabSalary.Controls.Add(this.txtAllowances);
            this.tabSalary.Controls.Add(this.lblAllowances);
            this.tabSalary.Controls.Add(this.txtBasicSalary);
            this.tabSalary.Controls.Add(this.lblBasicSalary);
            this.tabSalary.Location = new Point(4, 28);
            this.tabSalary.Name = "tabSalary";
            this.tabSalary.Size = new Size(852, 418);
            this.tabSalary.TabIndex = 2;
            this.tabSalary.Text = "Salary & Benefits";

            // Salary Controls Layout
            // Basic Salary
            this.lblBasicSalary.AutoSize = true;
            this.lblBasicSalary.Location = new Point(startX, startY);
            this.lblBasicSalary.Name = "lblBasicSalary";
            this.lblBasicSalary.Size = new Size(85, 19);
            this.lblBasicSalary.Text = "Basic Salary:";
            
            this.txtBasicSalary.Location = new Point(startX + labelWidth + 50, startY - 3);
            this.txtBasicSalary.Name = "txtBasicSalary";
            this.txtBasicSalary.Size = new Size(300, 25);
            this.txtBasicSalary.TextAlign = HorizontalAlignment.Right;

            // Allowances
            this.lblAllowances.AutoSize = true;
            this.lblAllowances.Location = new Point(startX, startY + rowHeight);
            this.lblAllowances.Name = "lblAllowances";
            this.lblAllowances.Size = new Size(78, 19);
            this.lblAllowances.Text = "Allowances:";
            
            this.txtAllowances.Location = new Point(startX + labelWidth + 50, startY + rowHeight - 3);
            this.txtAllowances.Name = "txtAllowances";
            this.txtAllowances.Size = new Size(300, 25);
            this.txtAllowances.TextAlign = HorizontalAlignment.Right;

            // Payroll Type
            this.lblPayrollType.AutoSize = true;
            this.lblPayrollType.Location = new Point(startX, startY + rowHeight * 2);
            this.lblPayrollType.Name = "lblPayrollType";
            this.lblPayrollType.Size = new Size(85, 19);
            this.lblPayrollType.Text = "Payroll Type:";
            
            this.cmbPayrollType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPayrollType.FormattingEnabled = true;
            this.cmbPayrollType.Items.AddRange(new object[] { "Monthly", "Semi-Monthly", "Weekly" });
            this.cmbPayrollType.Location = new Point(startX + labelWidth + 50, startY + rowHeight * 2 - 3);
            this.cmbPayrollType.Name = "cmbPayrollType";
            this.cmbPayrollType.Size = new Size(300, 25);

            // Bank Account
            this.lblBankAccount.AutoSize = true;
            this.lblBankAccount.Location = new Point(startX, startY + rowHeight * 3);
            this.lblBankAccount.Name = "lblBankAccount";
            this.lblBankAccount.Size = new Size(95, 19);
            this.lblBankAccount.Text = "Bank Account:";
            
            this.txtBankAccount.Location = new Point(startX + labelWidth + 50, startY + rowHeight * 3 - 3);
            this.txtBankAccount.Name = "txtBankAccount";
            this.txtBankAccount.Size = new Size(300, 25);

            // Bank Name
            this.lblBankName.AutoSize = true;
            this.lblBankName.Location = new Point(startX, startY + rowHeight * 4);
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Size = new Size(78, 19);
            this.lblBankName.Text = "Bank Name:";
            
            this.txtBankName.Location = new Point(startX + labelWidth + 50, startY + rowHeight * 4 - 3);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new Size(300, 25);

            // 
            // tabContact
            // 
            this.tabContact.BackColor = Color.White;
            this.tabContact.Controls.Add(this.txtEmergencyPhone);
            this.tabContact.Controls.Add(this.lblEmergencyPhone);
            this.tabContact.Controls.Add(this.txtEmergencyContact);
            this.tabContact.Controls.Add(this.lblEmergencyContact);
            this.tabContact.Controls.Add(this.txtEmail);
            this.tabContact.Controls.Add(this.lblEmail);
            this.tabContact.Controls.Add(this.txtMobile);
            this.tabContact.Controls.Add(this.lblMobile);
            this.tabContact.Controls.Add(this.txtPhone);
            this.tabContact.Controls.Add(this.lblPhone);
            this.tabContact.Controls.Add(this.txtZipCode);
            this.tabContact.Controls.Add(this.lblZipCode);
            this.tabContact.Controls.Add(this.txtProvince);
            this.tabContact.Controls.Add(this.lblProvince);
            this.tabContact.Controls.Add(this.txtCity);
            this.tabContact.Controls.Add(this.lblCity);
            this.tabContact.Controls.Add(this.txtAddress);
            this.tabContact.Controls.Add(this.lblAddress);
            this.tabContact.Location = new Point(4, 28);
            this.tabContact.Name = "tabContact";
            this.tabContact.Size = new Size(852, 418);
            this.tabContact.TabIndex = 3;
            this.tabContact.Text = "Contact Information";

            // Contact Controls Layout
            // Address
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new Point(startX, startY);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new Size(59, 19);
            this.lblAddress.Text = "Address:";
            
            this.txtAddress.Location = new Point(startX + labelWidth + 50, startY - 3);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new Size(400, 60);

            // City
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new Point(startX, startY + 80);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new Size(33, 19);
            this.lblCity.Text = "City:";
            
            this.txtCity.Location = new Point(startX + labelWidth + 50, startY + 80 - 3);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new Size(300, 25);

            // Province
            this.lblProvince.AutoSize = true;
            this.lblProvince.Location = new Point(startX, startY + 120);
            this.lblProvince.Name = "lblProvince";
            this.lblProvince.Size = new Size(62, 19);
            this.lblProvince.Text = "Province:";
            
            this.txtProvince.Location = new Point(startX + labelWidth + 50, startY + 120 - 3);
            this.txtProvince.Name = "txtProvince";
            this.txtProvince.Size = new Size(300, 25);

            // Zip Code
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new Point(startX, startY + 160);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new Size(65, 19);
            this.lblZipCode.Text = "Zip Code:";
            
            this.txtZipCode.Location = new Point(startX + labelWidth + 50, startY + 160 - 3);
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new Size(150, 25);

            // Phone
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new Point(startX, startY + 200);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new Size(47, 19);
            this.lblPhone.Text = "Phone:";
            
            this.txtPhone.Location = new Point(startX + labelWidth + 50, startY + 200 - 3);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new Size(200, 25);

            // Mobile
            this.lblMobile.AutoSize = true;
            this.lblMobile.Location = new Point(startX, startY + 240);
            this.lblMobile.Name = "lblMobile";
            this.lblMobile.Size = new Size(50, 19);
            this.lblMobile.Text = "Mobile:";
            
            this.txtMobile.Location = new Point(startX + labelWidth + 50, startY + 240 - 3);
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new Size(200, 25);

            // Email
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new Point(startX, startY + 280);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new Size(42, 19);
            this.lblEmail.Text = "Email:";
            
            this.txtEmail.Location = new Point(startX + labelWidth + 50, startY + 280 - 3);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new Size(300, 25);

            // Emergency Contact
            this.lblEmergencyContact.AutoSize = true;
            this.lblEmergencyContact.Location = new Point(startX, startY + 320);
            this.lblEmergencyContact.Name = "lblEmergencyContact";
            this.lblEmergencyContact.Size = new Size(125, 19);
            this.lblEmergencyContact.Text = "Emergency Contact:";
            
            this.txtEmergencyContact.Location = new Point(startX + labelWidth + 50, startY + 320 - 3);
            this.txtEmergencyContact.Name = "txtEmergencyContact";
            this.txtEmergencyContact.Size = new Size(300, 25);

            // Emergency Phone
            this.lblEmergencyPhone.AutoSize = true;
            this.lblEmergencyPhone.Location = new Point(startX, startY + 360);
            this.lblEmergencyPhone.Name = "lblEmergencyPhone";
            this.lblEmergencyPhone.Size = new Size(115, 19);
            this.lblEmergencyPhone.Text = "Emergency Phone:";
            
            this.txtEmergencyPhone.Location = new Point(startX + labelWidth + 50, startY + 360 - 3);
            this.txtEmergencyPhone.Name = "txtEmergencyPhone";
            this.txtEmergencyPhone.Size = new Size(200, 25);

            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Controls.Add(this.btnNew);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Location = new Point(20, 550);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new Size(860, 60);
            this.panelButtons.TabIndex = 2;

            // 
            // btnSave
            // 
            this.btnSave.BackColor = Color.FromArgb(46, 204, 113);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(10, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(100, 40);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += BtnSave_Click;

            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = Color.FromArgb(52, 152, 219);
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.Location = new Point(120, 10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(100, 40);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += BtnEdit_Click;

            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(230, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(100, 40);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += BtnDelete_Click;

            // 
            // btnNew
            // 
            this.btnNew.BackColor = Color.FromArgb(155, 89, 182);
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = FlatStyle.Flat;
            this.btnNew.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnNew.ForeColor = Color.White;
            this.btnNew.Location = new Point(340, 10);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new Size(100, 40);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += BtnNew_Click;

            // 
            // btnClose
            // 
            this.btnClose.BackColor = Color.FromArgb(149, 165, 166);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(450, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(100, 40);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += BtnClose_Click;

            // 
            // frmEmployee
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.ClientSize = new Size(900, 630);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmEmployee";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Employee Management";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPersonal.ResumeLayout(false);
            this.tabPersonal.PerformLayout();
            this.tabEmployment.ResumeLayout(false);
            this.tabEmployment.PerformLayout();
            this.tabSalary.ResumeLayout(false);
            this.tabSalary.PerformLayout();
            this.tabContact.ResumeLayout(false);
            this.tabContact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmployee)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDepartments();
                LoadJobTitles();
                LoadSupervisors();
                
                if (isEditing && !string.IsNullOrEmpty(currentEmployeeId))
                {
                    // Load employee data when editing
                    LoadEmployee(currentEmployeeId);
                    SetFormMode(true);
                }
                else
                {
                    SetFormMode(false);
                    GenerateEmployeeId();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                string query = "SELECT department_id, department_code, department_name FROM departments WHERE is_active = 1 ORDER BY department_code, department_name";
                DataTable dt = DatabaseManager.GetDataTable(query);
                
                cmbDepartment.DisplayMember = "department_name";
                cmbDepartment.ValueMember = "department_id";
                cmbDepartment.DataSource = dt;
                cmbDepartment.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadJobTitles()
        {
            try
            {
                string query = "SELECT job_title_id, job_title FROM job_titles WHERE is_active = 1 ORDER BY job_title";
                DataTable dt = DatabaseManager.GetDataTable(query);
                
                cmbJobTitle.DisplayMember = "job_title";
                cmbJobTitle.ValueMember = "job_title_id";
                cmbJobTitle.DataSource = dt;
                cmbJobTitle.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job titles: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSupervisors()
        {
            try
            {
                string query = @"
                    SELECT 
                        employee_id, 
                        CONCAT(first_name, ' ', last_name) as full_name 
                    FROM employees 
                    WHERE employment_status = 'Active' 
                    ORDER BY first_name, last_name";
                
                DataTable dt = DatabaseManager.GetDataTable(query);
                
                // Add empty row for no supervisor
                DataRow emptyRow = dt.NewRow();
                emptyRow["employee_id"] = DBNull.Value;
                emptyRow["full_name"] = "-- No Supervisor --";
                dt.Rows.InsertAt(emptyRow, 0);
                
                cmbSupervisor.DisplayMember = "full_name";
                cmbSupervisor.ValueMember = "employee_id";
                cmbSupervisor.DataSource = dt;
                cmbSupervisor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading supervisors: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateEmployeeId()
        {
            try
            {
                string newId = DatabaseManager.GenerateNewId("employees", "employee_id", "EMP");
                txtEmployeeId.Text = newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating employee ID: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBrowseImage_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                    openFileDialog.Title = "Select Employee Photo";
                    
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        imagePath = openFileDialog.FileName;
                        picEmployee.Image = Image.FromFile(imagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetFormMode(true);
            isEditing = false;
            currentEmployeeId = "";
            GenerateEmployeeId();
            txtFirstName.Focus();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentEmployeeId))
            {
                MessageBox.Show("Please select an employee to edit.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SetFormMode(true);
            isEditing = true;
            txtFirstName.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                string query;
                if (isEditing)
                {
                    query = @"
                        UPDATE employees SET 
                            first_name = @first_name,
                            middle_name = @middle_name,
                            last_name = @last_name,
                            gender = @gender,
                            birth_date = @birth_date,
                            civil_status = @civil_status,
                            nationality = @nationality,
                            religion = @religion,
                            department_id = @department_id,
                            job_title_id = @job_title_id,
                            employment_type = @employment_type,
                            hire_date = @hire_date,
                            status = @status,
                            supervisor_id = @supervisor_id,
                            basic_salary = @basic_salary,
                            allowances = @allowances,
                            payroll_type = @payroll_type,
                            bank_account = @bank_account,
                            bank_name = @bank_name,
                            address = @address,
                            city = @city,
                            province = @province,
                            zip_code = @zip_code,
                            phone = @phone,
                            mobile = @mobile,
                            email = @email,
                            emergency_contact = @emergency_contact,
                            emergency_phone = @emergency_phone,
                            photo_path = @photo_path,
                            updated_at = NOW()
                        WHERE employee_id = @employee_id";
                }
                else
                {
                    query = @"
                        INSERT INTO employees 
                        (employee_id, first_name, middle_name, last_name, gender, birth_date, civil_status, 
                         nationality, religion, department_id, job_title_id, employment_type, hire_date, status, 
                         supervisor_id, basic_salary, allowances, payroll_type, bank_account, bank_name, 
                         address, city, province, zip_code, phone, mobile, email, emergency_contact, 
                         emergency_phone, photo_path, created_date) 
                        VALUES 
                        (@employee_id, @first_name, @middle_name, @last_name, @gender, @birth_date, @civil_status, 
                         @nationality, @religion, @department_id, @job_title_id, @employment_type, @hire_date, @status, 
                         @supervisor_id, @basic_salary, @allowances, @payroll_type, @bank_account, @bank_name, 
                         @address, @city, @province, @zip_code, @phone, @mobile, @email, @emergency_contact, 
                         @emergency_phone, @photo_path, NOW())";
                }

                // Use DatabaseManager to handle connection management
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                
                parameters.Add("@employee_id", isEditing ? currentEmployeeId : txtEmployeeId.Text.Trim());
                
                parameters.Add("@first_name", txtFirstName.Text.Trim());
                parameters.Add("@middle_name", txtMiddleName.Text.Trim());
                parameters.Add("@last_name", txtLastName.Text.Trim());
                parameters.Add("@gender", cmbGender.Text);
                parameters.Add("@birth_date", dtpBirthDate.Value.Date);
                parameters.Add("@civil_status", cmbCivilStatus.Text);
                parameters.Add("@nationality", txtNationality.Text.Trim());
                parameters.Add("@religion", txtReligion.Text.Trim());
                parameters.Add("@department_id", cmbDepartment.SelectedValue ?? DBNull.Value);
                parameters.Add("@job_title_id", cmbJobTitle.SelectedValue ?? DBNull.Value);
                parameters.Add("@employment_type", cmbEmploymentType.Text);
                
                // Ensure hire date is not earlier than SQL minimum date (1/1/1753)
                DateTime hireDate = dtpDateHired.Value.Date;
                if (hireDate < minSqlDate)
                    hireDate = minSqlDate;
                parameters.Add("@hire_date", hireDate);
                
                parameters.Add("@status", cmbEmploymentStatus.Text);
                parameters.Add("@supervisor_id", cmbSupervisor.SelectedValue == DBNull.Value ? DBNull.Value : cmbSupervisor.SelectedValue);
                parameters.Add("@basic_salary", DatabaseManager.ConvertCurrencyToNumber(txtBasicSalary.Text));
                parameters.Add("@allowances", DatabaseManager.ConvertCurrencyToNumber(txtAllowances.Text));
                parameters.Add("@payroll_type", cmbPayrollType.Text);
                parameters.Add("@bank_account", txtBankAccount.Text.Trim());
                parameters.Add("@bank_name", txtBankName.Text.Trim());
                parameters.Add("@address", txtAddress.Text.Trim());
                parameters.Add("@city", txtCity.Text.Trim());
                parameters.Add("@province", txtProvince.Text.Trim());
                parameters.Add("@zip_code", txtZipCode.Text.Trim());
                parameters.Add("@phone", txtPhone.Text.Trim());
                parameters.Add("@mobile", txtMobile.Text.Trim());
                parameters.Add("@email", txtEmail.Text.Trim());
                parameters.Add("@emergency_contact", txtEmergencyContact.Text.Trim());
                parameters.Add("@emergency_phone", txtEmergencyPhone.Text.Trim());
                parameters.Add("@photo_path", imagePath);

                // Execute the query using DatabaseManager
                DatabaseManager.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Employee saved successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetFormMode(false);
                isEditing = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving employee: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(currentEmployeeId))
                {
                    MessageBox.Show("Please select an employee to delete.", "Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this employee?", 
                    "Confirm Delete", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = "UPDATE employees SET employment_status = 'Terminated', updated_at = NOW() WHERE employee_id = @employee_id";
                    
                    // Use DatabaseManager to execute the query
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "@employee_id", currentEmployeeId }
                    };
                    
                    DatabaseManager.ExecuteNonQuery(query, parameters);
                    
                    MessageBox.Show("Employee deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearForm();
                    currentEmployeeId = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private readonly DateTime minSqlDate = new DateTime(1753, 1, 1);

        private bool ValidateForm()
        {
            try {
                // Validate Employee Code
                string employeeId = txtEmployeeId.Text.Trim();
                if (string.IsNullOrWhiteSpace(employeeId))
                {
                    ShowValidationError("Please enter employee ID.", tabPersonal, txtEmployeeId);
                    return false;
                }
                if (employeeId.Length < 4 || employeeId.Length > 10)
                {
                    ShowValidationError("Employee ID must be between 4 and 10 characters.", tabPersonal, txtEmployeeId);
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(employeeId, @"^[A-Z0-9]+$"))
                {
                    ShowValidationError("Employee ID must contain only uppercase letters and numbers.", tabPersonal, txtEmployeeId);
                    return false;
                }

                // Only check for uniqueness if not editing (for new employees)
                if (!isEditing)
                {
                    // Check for unique employee ID
                    string checkQuery = "SELECT COUNT(*) FROM employees WHERE employee_id = @id";

                    // Create parameters dictionary
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "@id", employeeId }
                    };

                    // Use DatabaseManager's GetScalar method instead of direct connection
                    string result = DatabaseManager.GetScalar(checkQuery, parameters);
                    int count = Convert.ToInt32(result);
                    if (count > 0)
                    {
                        ShowValidationError("Employee ID must be unique. This ID is already in use.", tabPersonal, txtEmployeeId);
                        return false;
                    }
                }

            // Validate Personal Information
            string firstName = txtFirstName.Text.Trim();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                ShowValidationError("Please enter first name.", tabPersonal, txtFirstName);
                return false;
            }
            if (firstName.Length < 2 || firstName.Length > 50)
            {
                ShowValidationError("First name must be between 2 and 50 characters.", tabPersonal, txtFirstName);
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(firstName, @"^[A-Za-z\s\-']+$"))
            {
                ShowValidationError("First name can only contain letters, spaces, hyphens, and apostrophes.", tabPersonal, txtFirstName);
                return false;
            }

            string lastName = txtLastName.Text.Trim();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                ShowValidationError("Please enter last name.", tabPersonal, txtLastName);
                return false;
            }
            if (lastName.Length < 2 || lastName.Length > 50)
            {
                ShowValidationError("Last name must be between 2 and 50 characters.", tabPersonal, txtLastName);
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(lastName, @"^[A-Za-z\s\-']+$"))
            {
                ShowValidationError("Last name can only contain letters, spaces, hyphens, and apostrophes.", tabPersonal, txtLastName);
                return false;
            }

            string middleName = txtMiddleName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(middleName))
            {
                if (middleName.Length > 50)
                {
                    ShowValidationError("Middle name cannot exceed 50 characters.", tabPersonal, txtMiddleName);
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(middleName, @"^[A-Za-z\s\-']+$"))
                {
                    ShowValidationError("Middle name can only contain letters, spaces, hyphens, and apostrophes.", tabPersonal, txtMiddleName);
                    return false;
                }
            }

            if (cmbGender.SelectedIndex == -1)
            {
                ShowValidationError("Please select gender.", tabPersonal, cmbGender);
                return false;
            }

            // Validate birth date
            if (dtpBirthDate.Value.Date < minSqlDate)
            {
                ShowValidationError($"Birth date cannot be earlier than {minSqlDate.ToShortDateString()}.", tabPersonal, dtpBirthDate);
                return false;
            }
            if (dtpBirthDate.Value.Date > DateTime.Today)
            {
                ShowValidationError("Birth date cannot be in the future.", tabPersonal, dtpBirthDate);
                return false;
            }

            // Validate age (must be at least 18 years old and not over 65)
            int age = DateTime.Today.Year - dtpBirthDate.Value.Year;
            if (dtpBirthDate.Value.Date > DateTime.Today.AddYears(-age)) age--;
            if (age < 18)
            {
                ShowValidationError("Employee must be at least 18 years old.", tabPersonal, dtpBirthDate);
                return false;
            }
            if (age > 65)
            {
                ShowValidationError("Employee age cannot exceed 65 years.", tabPersonal, dtpBirthDate);
                return false;
            }

            // Validate Employment Information
            if (cmbDepartment.SelectedIndex == -1)
            {
                ShowValidationError("Please select department.", tabEmployment, cmbDepartment);
                return false;
            }

            if (cmbJobTitle.SelectedIndex == -1)
            {
                ShowValidationError("Please select job title.", tabEmployment, cmbJobTitle);
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbEmploymentType.Text))
            {
                ShowValidationError("Please select employment type.", tabEmployment, cmbEmploymentType);
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbEmploymentStatus.Text))
            {
                ShowValidationError("Please select employment status.", tabEmployment, cmbEmploymentStatus);
                return false;
            }

            // Validate hire date
            if (dtpDateHired.Value.Date < minSqlDate)
            {
                ShowValidationError($"Hire date cannot be earlier than {minSqlDate.ToShortDateString()}.", tabEmployment, dtpDateHired);
                return false;
            }
            if (dtpDateHired.Value.Date > DateTime.Today)
            {
                ShowValidationError("Hire date cannot be in the future.", tabEmployment, dtpDateHired);
                return false;
            }

            // Validate hire date is not before employee turns 18
            DateTime minimumHireDate = dtpBirthDate.Value.Date.AddYears(18);
            if (dtpDateHired.Value.Date < minimumHireDate)
            {
                ShowValidationError("Hire date cannot be earlier than employee's 18th birthday.", tabEmployment, dtpDateHired);
                return false;
            }

            // Validate Salary Information
            if (string.IsNullOrWhiteSpace(txtBasicSalary.Text))
            {
                ShowValidationError("Please enter basic salary.", tabSalary, txtBasicSalary);
                return false;
            }

            decimal basicSalary;
            if (!decimal.TryParse(DatabaseManager.ConvertCurrencyToNumber(txtBasicSalary.Text).ToString(), out basicSalary) || basicSalary <= 0)
            {
                ShowValidationError("Please enter a valid basic salary amount greater than 0.", tabSalary, txtBasicSalary);
                return false;
            }

            // Validate maximum salary limit (e.g., ₱999,999,999.99)
            if (basicSalary > 999999999.99m)
            {
                ShowValidationError("Basic salary cannot exceed ₱999,999,999.99.", tabSalary, txtBasicSalary);
                return false;
            }

            // Validate allowances if provided
            if (!string.IsNullOrWhiteSpace(txtAllowances.Text))
            {
                decimal allowances;
                if (!decimal.TryParse(DatabaseManager.ConvertCurrencyToNumber(txtAllowances.Text).ToString(), out allowances) || allowances < 0)
                {
                    ShowValidationError("Please enter a valid allowance amount (must be 0 or greater).", tabSalary, txtAllowances);
                    return false;
                }

                if (allowances > basicSalary)
                {
                    ShowValidationError("Total allowances cannot exceed basic salary.", tabSalary, txtAllowances);
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(cmbPayrollType.Text))
            {
                ShowValidationError("Please select payroll type.", tabSalary, cmbPayrollType);
                return false;
            }

            // Validate bank information if provided
            if (!string.IsNullOrWhiteSpace(txtBankAccount.Text))
            {
                if (string.IsNullOrWhiteSpace(txtBankName.Text))
                {
                    ShowValidationError("Bank name is required when bank account is provided.", tabSalary, txtBankName);
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtBankAccount.Text.Trim(), @"^[0-9]{10,20}$"))
                {
                    ShowValidationError("Bank account number must be between 10 and 20 digits.", tabSalary, txtBankAccount);
                    return false;
                }
            }

            // Validate contact information
            string address = txtAddress.Text.Trim();
            if (string.IsNullOrWhiteSpace(address))
            {
                ShowValidationError("Please enter address.", tabContact, txtAddress);
                return false;
            }
            if (address.Length < 5 || address.Length > 200)
            {
                ShowValidationError("Address must be between 5 and 200 characters.", tabContact, txtAddress);
                return false;
            }

            string city = txtCity.Text.Trim();
            if (string.IsNullOrWhiteSpace(city))
            {
                ShowValidationError("Please enter city.", tabContact, txtCity);
                return false;
            }
            if (city.Length < 2 || city.Length > 50)
            {
                ShowValidationError("City must be between 2 and 50 characters.", tabContact, txtCity);
                return false;
            }

            string province = txtProvince.Text.Trim();
            if (!string.IsNullOrWhiteSpace(province) && (province.Length < 2 || province.Length > 50))
            {
                ShowValidationError("Province must be between 2 and 50 characters.", tabContact, txtProvince);
                return false;
            }

            string zipCode = txtZipCode.Text.Trim();
            if (!string.IsNullOrWhiteSpace(zipCode) && !System.Text.RegularExpressions.Regex.IsMatch(zipCode, @"^\d{4,10}$"))
            {
                ShowValidationError("ZIP code must be between 4 and 10 digits.", tabContact, txtZipCode);
                return false;
            }

            // Validate contact numbers
            string mobile = txtMobile.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(mobile) && string.IsNullOrWhiteSpace(phone))
            {
                ShowValidationError("Please provide at least one contact number (mobile or phone).", tabContact, txtMobile);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(mobile) && !System.Text.RegularExpressions.Regex.IsMatch(mobile, @"^\+?[0-9]{10,15}$"))
            {
                ShowValidationError("Mobile number must be between 10 and 15 digits, optionally starting with +.", tabContact, txtMobile);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(phone) && !System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+?[0-9]{7,15}$"))
            {
                ShowValidationError("Phone number must be between 7 and 15 digits, optionally starting with +.", tabContact, txtPhone);
                return false;
            }

            // Validate email
            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (email.Length > 100)
                {
                    ShowValidationError("Email address cannot exceed 100 characters.", tabContact, txtEmail);
                    return false;
                }
                if (!IsValidEmail(email))
                {
                    ShowValidationError("Please enter a valid email address.", tabContact, txtEmail);
                    return false;
                }
            }

            // Validate emergency contact
            string emergencyContact = txtEmergencyContact.Text.Trim();
            if (string.IsNullOrWhiteSpace(emergencyContact))
            {
                ShowValidationError("Please enter emergency contact name.", tabContact, txtEmergencyContact);
                return false;
            }
            if (emergencyContact.Length < 2 || emergencyContact.Length > 100)
            {
                ShowValidationError("Emergency contact name must be between 2 and 100 characters.", tabContact, txtEmergencyContact);
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(emergencyContact, @"^[A-Za-z\s\-'.]+$"))
            {
                ShowValidationError("Emergency contact name can only contain letters, spaces, hyphens, apostrophes, and periods.", tabContact, txtEmergencyContact);
                return false;
            }

            string emergencyPhone = txtEmergencyPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(emergencyPhone))
            {
                ShowValidationError("Please enter emergency contact phone number.", tabContact, txtEmergencyPhone);
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(emergencyPhone, @"^\+?[0-9]{7,15}$"))
            {
                ShowValidationError("Emergency phone number must be between 7 and 15 digits, optionally starting with +.", tabContact, txtEmergencyPhone);
                return false;
            }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Validation error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void ShowValidationError(string message, TabPage tab, Control control)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tabControl.SelectedTab = tab;
            control.Focus();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                if (email.Length > 100) return false;
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && 
                       System.Text.RegularExpressions.Regex.IsMatch(email, 
                           @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            }
            catch
            {
                return false;
            }
        }

        private void SetFormMode(bool editMode)
        {
            // Enable/disable controls based on edit mode
            txtFirstName.ReadOnly = !editMode;
            txtMiddleName.ReadOnly = !editMode;
            txtLastName.ReadOnly = !editMode;
            cmbGender.Enabled = editMode;
            dtpBirthDate.Enabled = editMode;
            cmbCivilStatus.Enabled = editMode;
            txtNationality.ReadOnly = !editMode;
            txtReligion.ReadOnly = !editMode;
            
            cmbDepartment.Enabled = editMode;
            cmbJobTitle.Enabled = editMode;
            cmbEmploymentType.Enabled = editMode;
            dtpDateHired.Enabled = editMode;
            cmbEmploymentStatus.Enabled = editMode;
            cmbSupervisor.Enabled = editMode;
            
            txtBasicSalary.ReadOnly = !editMode;
            txtAllowances.ReadOnly = !editMode;
            cmbPayrollType.Enabled = editMode;
            txtBankAccount.ReadOnly = !editMode;
            txtBankName.ReadOnly = !editMode;
            
            txtAddress.ReadOnly = !editMode;
            txtCity.ReadOnly = !editMode;
            txtProvince.ReadOnly = !editMode;
            txtZipCode.ReadOnly = !editMode;
            txtPhone.ReadOnly = !editMode;
            txtMobile.ReadOnly = !editMode;
            txtEmail.ReadOnly = !editMode;
            txtEmergencyContact.ReadOnly = !editMode;
            txtEmergencyPhone.ReadOnly = !editMode;
            
            btnBrowseImage.Enabled = editMode;
            
            // Button states
            btnSave.Enabled = editMode;
            btnEdit.Enabled = !editMode && !string.IsNullOrEmpty(currentEmployeeId);
            btnDelete.Enabled = !editMode && !string.IsNullOrEmpty(currentEmployeeId);
            btnNew.Enabled = !editMode;
        }

        private void ClearForm()
        {
            txtEmployeeId.Clear();
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            cmbGender.SelectedIndex = -1;
            dtpBirthDate.Value = DateTime.Today.AddYears(-18); // Set to 18 years ago by default
            cmbCivilStatus.SelectedIndex = -1;
            txtNationality.Clear();
            txtReligion.Clear();
            
            cmbDepartment.SelectedIndex = -1;
            cmbJobTitle.SelectedIndex = -1;
            cmbEmploymentType.SelectedIndex = -1;
            dtpDateHired.Value = DateTime.Today; // Set to today's date without time component
            cmbEmploymentStatus.SelectedIndex = -1;
            cmbSupervisor.SelectedIndex = 0;
            
            txtBasicSalary.Clear();
            txtAllowances.Clear();
            cmbPayrollType.SelectedIndex = -1;
            txtBankAccount.Clear();
            txtBankName.Clear();
            
            txtAddress.Clear();
            txtCity.Clear();
            txtProvince.Clear();
            txtZipCode.Clear();
            txtPhone.Clear();
            txtMobile.Clear();
            txtEmail.Clear();
            txtEmergencyContact.Clear();
            txtEmergencyPhone.Clear();
            
            picEmployee.Image = null;
            imagePath = "";
        }

        public void LoadEmployee(string employeeId)
        {
            try
            {
                currentEmployeeId = employeeId;
                
                string query = @"
                    SELECT e.*, d.department_name, jt.job_title, 
                           CONCAT(s.first_name, ' ', s.last_name) as supervisor_name
                    FROM employees e
                    LEFT JOIN departments d ON e.department_id = d.department_id
                    LEFT JOIN job_titles jt ON e.job_title_id = jt.job_title_id
                    LEFT JOIN employees s ON e.supervisor_id = s.employee_id
                    WHERE e.employee_id = @employee_id";
                
                // Use DatabaseManager to handle connection management
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@employee_id", employeeId }
                };
                
                DataTable dt = DatabaseManager.GetDataTable(query, parameters);
                
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    
                    txtEmployeeId.Text = row["employee_id"].ToString();
                    txtFirstName.Text = row["first_name"].ToString();
                    txtMiddleName.Text = row["middle_name"].ToString();
                    txtLastName.Text = row["last_name"].ToString();
                    cmbGender.Text = row["gender"].ToString();
                    dtpBirthDate.Value = Convert.ToDateTime(row["birth_date"]);
                    cmbCivilStatus.Text = row["civil_status"].ToString();
                    txtNationality.Text = row["nationality"].ToString();
                    txtReligion.Text = row["religion"].ToString();
                
                    cmbDepartment.SelectedValue = row["department_id"];
                    cmbJobTitle.SelectedValue = row["job_title_id"];
                    cmbEmploymentType.Text = row["employment_type"].ToString();
                    dtpDateHired.Value = Convert.ToDateTime(row["hire_date"]);
                    cmbEmploymentStatus.Text = row["status"].ToString();
                    cmbSupervisor.SelectedValue = row["supervisor_id"] == DBNull.Value ? DBNull.Value : row["supervisor_id"];
                
                    txtBasicSalary.Text = DatabaseManager.FormatCurrency(Convert.ToDecimal(row["basic_salary"]));
                    txtAllowances.Text = DatabaseManager.FormatCurrency(Convert.ToDecimal(row["allowances"]));
                    cmbPayrollType.Text = row["payroll_type"].ToString();
                    txtBankAccount.Text = row["bank_account"].ToString();
                    txtBankName.Text = row["bank_name"].ToString();
                
                    txtAddress.Text = row["address"].ToString();
                    txtCity.Text = row["city"].ToString();
                    txtProvince.Text = row["province"].ToString();
                    txtZipCode.Text = row["zip_code"].ToString();
                    txtPhone.Text = row["phone"].ToString();
                    txtMobile.Text = row["mobile"].ToString();
                    txtEmail.Text = row["email"].ToString();
                    txtEmergencyContact.Text = row["emergency_contact"].ToString();
                    txtEmergencyPhone.Text = row["emergency_phone"].ToString();
                
                    // Load photo if exists
                    string photoPath = row["photo_path"].ToString();
                    if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
                    {
                        picEmployee.Image = Image.FromFile(photoPath);
                        imagePath = photoPath;
                    }
                }
                
                SetFormMode(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employee: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Form Controls Declaration
        private Panel panelHeader;
        private Label lblTitle;
        private TabControl tabControl;
        private TabPage tabPersonal;
        private TabPage tabEmployment;
        private TabPage tabSalary;
        private TabPage tabContact;
        
        // Personal Info Controls
        private PictureBox picEmployee;
        private Button btnBrowseImage;
        // Employee ID fields are declared at the class level
        private Label lblFirstName;
        private TextBox txtFirstName;
        private Label lblMiddleName;
        private TextBox txtMiddleName;
        private Label lblLastName;
        private TextBox txtLastName;
        private Label lblGender;
        private ComboBox cmbGender;
        private Label lblBirthDate;
        private DateTimePicker dtpBirthDate;
        private Label lblCivilStatus;
        private ComboBox cmbCivilStatus;
        private Label lblNationality;
        private TextBox txtNationality;
        private Label lblReligion;
        private TextBox txtReligion;
        
        // Employment Controls
        private Label lblDepartment;
        private ComboBox cmbDepartment;
        private Label lblJobTitle;
        private ComboBox cmbJobTitle;
        private Label lblEmploymentType;
        private ComboBox cmbEmploymentType;
        private Label lblDateHired;
        private DateTimePicker dtpDateHired;
        private Label lblEmploymentStatus;
        private ComboBox cmbEmploymentStatus;
        private Label lblImmediateSupervisor;
        private ComboBox cmbSupervisor;
        
        // Salary Controls
        private Label lblBasicSalary;
        private TextBox txtBasicSalary;
        private Label lblAllowances;
        private TextBox txtAllowances;
        private Label lblPayrollType;
        private ComboBox cmbPayrollType;
        private Label lblBankAccount;
        private TextBox txtBankAccount;
        private Label lblBankName;
        private TextBox txtBankName;
        
        // Contact Controls
        private Label lblAddress;
        private TextBox txtAddress;
        private Label lblCity;
        private TextBox txtCity;
        private Label lblProvince;
        private TextBox txtProvince;
        private Label lblZipCode;
        private TextBox txtZipCode;
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblMobile;
        private TextBox txtMobile;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblEmergencyContact;
        private TextBox txtEmergencyContact;
        private Label lblEmergencyPhone;
        private TextBox txtEmergencyPhone;
        
        // Buttons
        private Panel panelButtons;
        private Button btnSave;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnNew;
        private Button btnClose;
        #endregion
     }
}
