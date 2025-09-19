# Database Setup Instructions

## Missing Tables Error Fix

If you encounter the error "Table 'payroll_system.employee_allowances' doesn't exist" when using the Payroll Generation feature, you need to create the missing tables.

## Steps to Fix:

### Option 1: Using MySQL Command Line
1. Open Command Prompt or PowerShell as Administrator
2. Navigate to the PayrollSystem folder:
   ```
   cd "C:\Users\joshu\source\repos\AMCES APP2\C# Payroll System\PayrollSystem"
   ```
3. Run the SQL script:
   ```
   mysql -u root -p payroll_system < create_missing_tables.sql
   ```
4. Enter your MySQL root password when prompted

### Option 2: Using MySQL Workbench or phpMyAdmin
1. Open MySQL Workbench or phpMyAdmin
2. Connect to your MySQL server
3. Select the `payroll_system` database
4. Open the file `create_missing_tables.sql` in the PayrollSystem folder
5. Copy and paste the entire content into the SQL editor
6. Execute the script

### Option 3: Manual Table Creation
If the above options don't work, you can manually create the tables by running these SQL commands:

```sql
-- Employee allowances table
CREATE TABLE IF NOT EXISTS employee_allowances (
    allowance_id INT AUTO_INCREMENT PRIMARY KEY,
    employee_id VARCHAR(20) NOT NULL,
    allowance_type ENUM('fixed', 'variable') DEFAULT 'fixed',
    allowance_name VARCHAR(100),
    amount DECIMAL(10,2) DEFAULT 0.00,
    is_active BOOLEAN DEFAULT TRUE,
    effective_date DATE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (employee_id) REFERENCES employees(employee_id),
    INDEX idx_employee_allowance (employee_id),
    INDEX idx_allowance_type (allowance_type)
);

-- Employee deductions table
CREATE TABLE IF NOT EXISTS employee_deductions (
    deduction_id INT AUTO_INCREMENT PRIMARY KEY,
    employee_id VARCHAR(20) NOT NULL,
    deduction_type VARCHAR(50),
    deduction_name VARCHAR(100),
    amount DECIMAL(10,2) DEFAULT 0.00,
    is_active BOOLEAN DEFAULT TRUE,
    effective_date DATE,
    deduction_date DATE,
    period_id INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (employee_id) REFERENCES employees(employee_id),
    INDEX idx_employee_deduction (employee_id),
    INDEX idx_deduction_date (deduction_date)
);

-- Allowance transactions table
CREATE TABLE IF NOT EXISTS allowance_transactions (
    transaction_id INT AUTO_INCREMENT PRIMARY KEY,
    allowance_id INT,
    employee_id VARCHAR(20),
    amount DECIMAL(10,2),
    transaction_date DATE,
    period_id INT,
    description TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (allowance_id) REFERENCES employee_allowances(allowance_id),
    FOREIGN KEY (employee_id) REFERENCES employees(employee_id),
    FOREIGN KEY (period_id) REFERENCES payroll_periods(period_id)
);

-- Payroll settings table
CREATE TABLE IF NOT EXISTS payroll_settings (
    id INT PRIMARY KEY DEFAULT 1,
    overtime_rate DECIMAL(5,2) DEFAULT 1.25,
    working_hours_per_day INT DEFAULT 8,
    working_days_per_week INT DEFAULT 5,
    working_days_per_month INT DEFAULT 22,
    auto_calculate_overtime BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    modified_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Insert default settings
INSERT IGNORE INTO payroll_settings (id, overtime_rate, working_hours_per_day, working_days_per_week, working_days_per_month, auto_calculate_overtime)
VALUES (1, 1.25, 8, 5, 22, TRUE);
```

## Verification

After running the script, you can verify the tables were created by running:

```sql
SHOW TABLES LIKE '%allowance%';
SHOW TABLES LIKE '%deduction%';
SHOW TABLES LIKE '%payroll_settings%';
```

You should see:
- employee_allowances
- allowance_transactions  
- employee_deductions
- payroll_settings

## What These Tables Do

- **employee_allowances**: Stores fixed and variable allowances for employees
- **allowance_transactions**: Tracks allowance payments per payroll period
- **employee_deductions**: Stores various deductions (loans, penalties, etc.)
- **payroll_settings**: Contains system-wide payroll configuration

After creating these tables, the Payroll Generation feature should work without errors.