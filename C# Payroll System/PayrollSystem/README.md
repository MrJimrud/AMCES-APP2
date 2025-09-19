# C# Payroll System

A comprehensive payroll management system built with C# Windows Forms and MySQL database.

## Features

### Core Functionality
- **Employee Management**: Complete CRUD operations for employee records
- **Payroll Processing**: Automated payroll calculation with deductions
- **DTR Management**: Daily Time Record with QR code support
- **Cash Advance**: Employee cash advance requests and tracking
- **Reporting**: Comprehensive reports for payroll, DTR, and cash advances
- **User Management**: Role-based access control

### Key Components
- **Dashboard**: Real-time statistics and quick actions
- **Employee Forms**: Add, edit, view employee information
- **Payroll Generation**: Period-based payroll processing
- **Time Tracking**: QR code-enabled time in/out system
- **Configuration Management**: INI-based settings

## System Requirements

### Software Requirements
- .NET Framework 4.7.2 or higher
- MySQL Server 8.0 or higher
- Visual Studio 2019 or higher (for development)
- Windows 10 or higher

### Hardware Requirements
- Minimum 4GB RAM
- 500MB available disk space
- Network connectivity for database access

## Installation & Setup

### 1. Database Setup

1. **Install MySQL Server**
   - Download and install MySQL Server 8.0+
   - Create a new database user for the application
   - Note the connection details (server, port, username, password)

2. **Create Database Schema**
   - Open MySQL Workbench or command line
   - Execute the `database_schema.sql` file:
   ```sql
   source /path/to/database_schema.sql
   ```
   - This will create:
     - Database: `payroll_system`
     - All required tables with relationships
     - Sample data for testing
     - Default admin user (username: `admin`, password: `admin123`)

### 2. Application Configuration

1. **Update Database Connection**
   - Edit the `config.ini` file in the application directory
   - Update the database connection settings:
   ```ini
   [Database]
   Server=localhost
   Port=3306
   Database=payroll_system
   Username=your_username
   Password=your_password
   ```

2. **Configure Application Settings**
   - Review and update other settings in `config.ini` as needed:
     - Company information
     - Payroll parameters
     - System configurations

### 3. Dependencies

The application requires the following NuGet packages:
- `MySql.Data` (9.4.0+) - MySQL database connectivity
- `ThoughtWorks.QRCode` (1.1.0+) - QR code generation

To install dependencies:
```bash
Install-Package MySql.Data
Install-Package ThoughtWorks.QRCode
```

## Usage

### First Time Setup

1. **Login**
   - Run the application
   - Use default credentials:
     - Username: `admin`
     - Password: `admin123`

2. **Configure Company Settings**
   - Go to File → Company Settings
   - Update company information
   - Configure payroll parameters

3. **Setup Departments and Job Titles**
   - Navigate to Employee → Departments
   - Add/modify departments as needed
   - Navigate to Employee → Job Titles
   - Add/modify job titles with salary ranges

### Daily Operations

#### Employee Management
- **Add Employee**: Employee → Add Employee
- **View Employees**: Employee → Employee List
- **Edit Employee**: Double-click employee in list or use Edit button

#### Time Tracking
- **DTR Management**: DTR → DTR Management
- **Generate QR Codes**: Use QR Code section in DTR form
- **Record Time**: Use Time In/Out buttons or scan QR codes

#### Payroll Processing
1. **Create Payroll Period**: Payroll → Payroll Periods
2. **Generate Payroll**: Payroll → Payroll Generation
3. **Calculate Deductions**: Use Calculate button in payroll form
4. **Review and Approve**: Check calculations before finalizing

#### Cash Advances
- **Request Cash Advance**: Cash Advance → New Request
- **Approve Requests**: Cash Advance → Pending Requests
- **Track Deductions**: Automatically handled in payroll

## Database Schema

### Main Tables
- `employees` - Employee master data
- `departments` - Department information
- `job_titles` - Job title definitions
- `dtr_records` - Daily time records
- `payroll_periods` - Payroll period definitions
- `payroll_details` - Individual payroll calculations
- `cash_advances` - Cash advance requests
- `users` - System users and authentication
- `company_settings` - Company configuration
- `payroll_settings` - Payroll parameters

### Sample Data Included
- 5 sample employees across different departments
- 5 departments (HR, IT, Finance, Operations, Marketing)
- 7 job titles with salary ranges
- Default admin user
- Company and payroll settings

## Security Features

- **User Authentication**: Login system with role-based access
- **Data Validation**: Input validation on all forms
- **SQL Injection Prevention**: Parameterized queries
- **Configuration Security**: Sensitive data in config files

## Troubleshooting

### Common Issues

1. **Database Connection Failed**
   - Check MySQL server is running
   - Verify connection settings in `config.ini`
   - Ensure database user has proper permissions

2. **Login Failed**
   - Use default credentials: admin/admin123
   - Check users table in database
   - Verify database connection

3. **QR Code Not Generating**
   - Ensure ThoughtWorks.QRCode package is installed
   - Check for any missing dependencies

4. **Payroll Calculation Errors**
   - Verify payroll settings in database
   - Check employee salary and deduction data
   - Review DTR records for the period

### Error Logs
- Application errors are displayed in message boxes
- Database errors include detailed MySQL error messages
- Check Windows Event Viewer for system-level issues

## Development

### Project Structure
```
PayrollSystem/
├── Forms/
│   ├── frmLogin.cs          # Login form
│   ├── frmMain.cs           # Main dashboard
│   ├── frmEmployee.cs       # Employee management
│   ├── frmEmployeeList.cs   # Employee listing
│   ├── frmDTR.cs           # Time tracking
│   ├── frmPayrollPeriod.cs  # Payroll periods
│   └── frmPayrollGeneration.cs # Payroll processing
├── Classes/
│   ├── DatabaseManager.cs   # Database operations
│   └── IniFileManager.cs    # Configuration management
├── Resources/
│   ├── config.ini           # Application configuration
│   └── database_schema.sql  # Database setup script
└── README.md               # This file
```

### Adding New Features
1. Create new form classes inheriting from Form
2. Add database operations to DatabaseManager
3. Update main form menu if needed
4. Test thoroughly with sample data

## Support

For technical support or feature requests:
- Review this documentation
- Check the database schema for data relationships
- Examine existing code patterns for consistency
- Test with sample data before production use

## License

This project is developed for educational and business purposes. Please ensure compliance with all applicable software licenses for third-party components.

---

**Version**: 1.0  
**Last Updated**: 2024  
**Developed for**: AMCES Company