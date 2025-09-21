-- Complete Payroll System Database Schema
-- This script creates a complete database schema for the Payroll System application

-- Create database if not exists
CREATE DATABASE IF NOT EXISTS payroll_system;
USE payroll_system;

-- Create users table if not exists
CREATE TABLE IF NOT EXISTS `users` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `full_name` varchar(100) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `role` enum('Admin','HR','Manager','Employee') NOT NULL DEFAULT 'Employee',
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `last_login` datetime DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `unique_username` (`username`),
  KEY `idx_role` (`role`),
  KEY `idx_active` (`is_active`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create departments table if not exists
CREATE TABLE IF NOT EXISTS `departments` (
  `department_id` int(11) NOT NULL AUTO_INCREMENT,
  `department_code` varchar(20) NOT NULL,
  `department_name` varchar(100) NOT NULL,
  `description` text,
  `manager_id` varchar(20) DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`department_id`),
  UNIQUE KEY `unique_department_code` (`department_code`),
  UNIQUE KEY `unique_department_name` (`department_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create job_titles table if not exists
CREATE TABLE IF NOT EXISTS `job_titles` (
  `job_title_id` int(11) NOT NULL AUTO_INCREMENT,
  `job_code` varchar(20) NOT NULL,
  `job_title` varchar(100) NOT NULL,
  `description` text,
  `department_id` int(11) NOT NULL,
  `min_salary` decimal(12,2) NOT NULL DEFAULT 0.00,
  `max_salary` decimal(12,2) NOT NULL DEFAULT 0.00,
  `requirements` text,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`job_title_id`),
  UNIQUE KEY `unique_job_code` (`job_code`),
  UNIQUE KEY `unique_job_title` (`job_title`),
  KEY `idx_department` (`department_id`),
  CONSTRAINT `fk_job_department` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create tbl_job_status table if not exists
CREATE TABLE IF NOT EXISTS `tbl_job_status` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `status_name` varchar(50) NOT NULL,
  `description` text,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_date` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `unique_status_name` (`status_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create employees table if not exists
CREATE TABLE IF NOT EXISTS `employees` (
  `employee_id` varchar(20) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `middle_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) NOT NULL,
  `gender` enum('Male','Female','Other') DEFAULT NULL,
  `birth_date` date DEFAULT NULL,
  `address` text,
  `contact_number` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `department_id` int(11) DEFAULT NULL,
  `job_title_id` int(11) DEFAULT NULL,
  `job_status_id` int(11) DEFAULT NULL,
  `hire_date` date DEFAULT NULL,
  `end_date` date DEFAULT NULL,
  `basic_salary` decimal(12,2) NOT NULL DEFAULT 0.00,
  `salary_type` enum('Monthly','Daily','Hourly') NOT NULL DEFAULT 'Monthly',
  `tax_status` varchar(30) DEFAULT 'Single',
  `sss_number` varchar(20) DEFAULT NULL,
  `philhealth_number` varchar(20) DEFAULT NULL,
  `pagibig_number` varchar(20) DEFAULT NULL,
  `tin_number` varchar(20) DEFAULT NULL,
  `bank_account` varchar(30) DEFAULT NULL,
  `bank_name` varchar(50) DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`employee_id`),
  KEY `idx_department` (`department_id`),
  KEY `idx_job_title` (`job_title_id`),
  KEY `idx_job_status` (`job_status_id`),
  KEY `idx_active` (`is_active`),
  CONSTRAINT `fk_employee_department` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_employee_job_status` FOREIGN KEY (`job_status_id`) REFERENCES `tbl_job_status` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_employee_job_title` FOREIGN KEY (`job_title_id`) REFERENCES `job_titles` (`job_title_id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create philhealth_rates table if not exists
CREATE TABLE IF NOT EXISTS `philhealth_rates` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `salary_from` decimal(12,2) NOT NULL,
  `salary_to` decimal(12,2) NOT NULL,
  `employee_share` decimal(12,2) NOT NULL,
  `employer_share` decimal(12,2) NOT NULL,
  `total_monthly_premium` decimal(12,2) NOT NULL,
  `effective_date` date NOT NULL,
  `expiry_date` date DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `idx_salary_range` (`salary_from`, `salary_to`),
  KEY `idx_effective_date` (`effective_date`, `expiry_date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create philhealth_contributions table if not exists
CREATE TABLE IF NOT EXISTS `philhealth_contributions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` varchar(20) NOT NULL,
  `contribution_date` date NOT NULL,
  `salary` decimal(12,2) NOT NULL,
  `employee_share` decimal(12,2) NOT NULL,
  `employer_share` decimal(12,2) NOT NULL,
  `total_contribution` decimal(12,2) NOT NULL,
  `period_id` int(11) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `employee_id` (`employee_id`),
  KEY `idx_contribution_date` (`contribution_date`),
  CONSTRAINT `fk_philhealth_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create philhealth_settings table if not exists
CREATE TABLE IF NOT EXISTS `philhealth_settings` (
  `id` int(11) NOT NULL DEFAULT 1,
  `premium_rate` decimal(5,2) NOT NULL DEFAULT 4.00,
  `salary_floor` decimal(12,2) NOT NULL DEFAULT 10000.00,
  `salary_ceiling` decimal(12,2) NOT NULL DEFAULT 80000.00,
  `updated_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create sss_contribution_table if not exists
CREATE TABLE IF NOT EXISTS `sss_contribution_table` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `range_from` decimal(12,2) NOT NULL,
  `range_to` decimal(12,2) NOT NULL,
  `monthly_salary_credit` decimal(12,2) NOT NULL,
  `employee_share` decimal(12,2) NOT NULL,
  `employer_share` decimal(12,2) NOT NULL,
  `total_contribution` decimal(12,2) NOT NULL,
  `effective_date` date NOT NULL,
  `expiry_date` date DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `idx_range` (`range_from`, `range_to`),
  KEY `idx_effective_date` (`effective_date`, `expiry_date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create sss_contributions table if not exists
CREATE TABLE IF NOT EXISTS `sss_contributions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` varchar(20) NOT NULL,
  `contribution_date` date NOT NULL,
  `salary_credit` decimal(12,2) NOT NULL,
  `employee_share` decimal(12,2) NOT NULL,
  `employer_share` decimal(12,2) NOT NULL,
  `total_contribution` decimal(12,2) NOT NULL,
  `period_id` int(11) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `idx_employee_id` (`employee_id`),
  KEY `idx_contribution_date` (`contribution_date`),
  CONSTRAINT `fk_sss_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create sss_settings table if not exists
CREATE TABLE IF NOT EXISTS `sss_settings` (
  `id` int(11) NOT NULL DEFAULT 1,
  `contribution_rate` decimal(5,2) NOT NULL DEFAULT 13.00,
  `employee_share_rate` decimal(5,2) NOT NULL DEFAULT 4.50,
  `employer_share_rate` decimal(5,2) NOT NULL DEFAULT 8.50,
  `updated_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create tax_brackets table if not exists
CREATE TABLE IF NOT EXISTS `tax_brackets` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `bracket_order` int(11) NOT NULL,
  `income_from` decimal(12,2) NOT NULL,
  `income_to` decimal(12,2) NOT NULL,
  `tax_rate` decimal(5,2) NOT NULL,
  `fixed_amount` decimal(12,2) NOT NULL,
  `filing_status` varchar(30) NOT NULL,
  `tax_year` varchar(4) NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `tax_year_status` (`tax_year`, `filing_status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create tax_settings table if not exists
CREATE TABLE IF NOT EXISTS `tax_settings` (
  `id` int(11) NOT NULL DEFAULT 1,
  `withholding_rate` decimal(5,2) NOT NULL DEFAULT 5.00,
  `minimum_wage` decimal(10,2) NOT NULL DEFAULT 537.00,
  `personal_exemption` decimal(12,2) NOT NULL DEFAULT 50000.00,
  `additional_exemption` decimal(12,2) NOT NULL DEFAULT 25000.00,
  `auto_calculate` tinyint(1) NOT NULL DEFAULT 1,
  `use_annualization` tinyint(1) NOT NULL DEFAULT 1,
  `include_allowances` tinyint(1) NOT NULL DEFAULT 1,
  `include_bonus` tinyint(1) NOT NULL DEFAULT 0,
  `updated_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create employee_tax table if not exists
CREATE TABLE IF NOT EXISTS `employee_tax` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` varchar(20) NOT NULL,
  `taxable_income` decimal(12,2) NOT NULL,
  `tax_amount` decimal(12,2) NOT NULL,
  `filing_status` varchar(30) NOT NULL,
  `tax_period` varchar(20) NOT NULL,
  `tax_date` date NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `employee_id` (`employee_id`),
  CONSTRAINT `fk_tax_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create payroll_periods table if not exists
CREATE TABLE IF NOT EXISTS `payroll_periods` (
  `period_id` int(11) NOT NULL AUTO_INCREMENT,
  `period_name` varchar(100) NOT NULL,
  `start_date` date NOT NULL,
  `end_date` date NOT NULL,
  `payroll_date` date NOT NULL,
  `status` enum('Open','Closed','Processing') NOT NULL DEFAULT 'Open',
  `description` text,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`period_id`),
  KEY `idx_period_dates` (`start_date`, `end_date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create payroll_details table if not exists
CREATE TABLE IF NOT EXISTS `payroll_details` (
  `payroll_id` int(11) NOT NULL AUTO_INCREMENT,
  `period_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `basic_pay` decimal(12,2) NOT NULL DEFAULT 0.00,
  `overtime_pay` decimal(12,2) NOT NULL DEFAULT 0.00,
  `allowances` decimal(12,2) NOT NULL DEFAULT 0.00,
  `gross_pay` decimal(12,2) NOT NULL DEFAULT 0.00,
  `sss_deduction` decimal(12,2) NOT NULL DEFAULT 0.00,
  `philhealth_deduction` decimal(12,2) NOT NULL DEFAULT 0.00,
  `pagibig_deduction` decimal(12,2) NOT NULL DEFAULT 0.00,
  `tax_deduction` decimal(12,2) NOT NULL DEFAULT 0.00,
  `other_deductions` decimal(12,2) NOT NULL DEFAULT 0.00,
  `total_deductions` decimal(12,2) NOT NULL DEFAULT 0.00,
  `net_pay` decimal(12,2) NOT NULL DEFAULT 0.00,
  `status` enum('Draft','Calculated','Approved','Paid') NOT NULL DEFAULT 'Draft',
  `calculated_by` int(11) DEFAULT NULL,
  `calculated_date` datetime DEFAULT NULL,
  `approved_by` int(11) DEFAULT NULL,
  `approved_date` datetime DEFAULT NULL,
  `paid_by` int(11) DEFAULT NULL,
  `paid_date` datetime DEFAULT NULL,
  `remarks` text,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`payroll_id`),
  KEY `idx_employee_period` (`employee_id`, `period_id`),
  KEY `idx_period` (`period_id`),
  KEY `idx_payroll_status` (`status`),
  KEY `idx_payroll_calculated` (`calculated_by`, `calculated_date`),
  KEY `idx_payroll_approved` (`approved_by`, `approved_date`),
  KEY `idx_payroll_paid` (`paid_by`, `paid_date`),
  CONSTRAINT `fk_payroll_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_payroll_period` FOREIGN KEY (`period_id`) REFERENCES `payroll_periods` (`period_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create payroll_generations table if not exists
CREATE TABLE IF NOT EXISTS `payroll_generations` (
  `generation_id` int(11) NOT NULL AUTO_INCREMENT,
  `generation_name` varchar(100) NOT NULL,
  `period_id` int(11) NOT NULL,
  `status` enum('Pending','Processing','Completed','Failed') NOT NULL DEFAULT 'Pending',
  `total_employees` int(11) NOT NULL DEFAULT 0,
  `total_amount` decimal(15,2) NOT NULL DEFAULT 0.00,
  `created_by` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `completed_date` datetime DEFAULT NULL,
  `notes` text,
  PRIMARY KEY (`generation_id`),
  KEY `idx_period` (`period_id`),
  KEY `idx_status` (`status`),
  CONSTRAINT `fk_generation_period` FOREIGN KEY (`period_id`) REFERENCES `payroll_periods` (`period_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create employee_allowances table if not exists
CREATE TABLE IF NOT EXISTS `employee_allowances` (
  `allowance_id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` varchar(20) NOT NULL,
  `allowance_name` varchar(100) NOT NULL,
  `allowance_type` enum('Fixed','Variable') NOT NULL DEFAULT 'Fixed',
  `amount` decimal(12,2) NOT NULL DEFAULT 0.00,
  `is_taxable` tinyint(1) NOT NULL DEFAULT 1,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `start_date` date DEFAULT NULL,
  `end_date` date DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`allowance_id`),
  KEY `idx_employee` (`employee_id`),
  KEY `idx_active` (`is_active`),
  CONSTRAINT `fk_allowance_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create allowance_transactions table if not exists
CREATE TABLE IF NOT EXISTS `allowance_transactions` (
  `transaction_id` int(11) NOT NULL AUTO_INCREMENT,
  `allowance_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `period_id` int(11) NOT NULL,
  `amount` decimal(12,2) NOT NULL DEFAULT 0.00,
  `payroll_id` int(11) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`transaction_id`),
  KEY `idx_allowance` (`allowance_id`),
  KEY `idx_employee_period` (`employee_id`, `period_id`),
  KEY `idx_payroll` (`payroll_id`),
  CONSTRAINT `fk_allowance_trans_allowance` FOREIGN KEY (`allowance_id`) REFERENCES `employee_allowances` (`allowance_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_allowance_trans_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_allowance_trans_payroll` FOREIGN KEY (`payroll_id`) REFERENCES `payroll_details` (`payroll_id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_allowance_trans_period` FOREIGN KEY (`period_id`) REFERENCES `payroll_periods` (`period_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create employee_deductions table if not exists
CREATE TABLE IF NOT EXISTS `employee_deductions` (
  `deduction_id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` varchar(20) NOT NULL,
  `deduction_name` varchar(100) NOT NULL,
  `deduction_type` enum('Loan','Penalty','Other') NOT NULL DEFAULT 'Other',
  `amount` decimal(12,2) NOT NULL DEFAULT 0.00,
  `is_recurring` tinyint(1) NOT NULL DEFAULT 0,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `start_date` date DEFAULT NULL,
  `end_date` date DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`deduction_id`),
  KEY `idx_employee` (`employee_id`),
  KEY `idx_active` (`is_active`),
  CONSTRAINT `fk_deduction_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create cash_advances table if not exists
CREATE TABLE IF NOT EXISTS `cash_advances` (
  `ca_id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` varchar(20) NOT NULL,
  `amount` decimal(12,2) NOT NULL,
  `purpose` text,
  `request_date` date NOT NULL,
  `approval_date` date DEFAULT NULL,
  `approved_by` int(11) DEFAULT NULL,
  `status` enum('Pending','Approved','Rejected','Paid','Cancelled') NOT NULL DEFAULT 'Pending',
  `payment_date` date DEFAULT NULL,
  `repayment_method` enum('One-time','Installment') NOT NULL DEFAULT 'One-time',
  `installment_amount` decimal(12,2) DEFAULT NULL,
  `remaining_balance` decimal(12,2) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`ca_id`),
  KEY `idx_employee` (`employee_id`),
  KEY `idx_status` (`status`),
  CONSTRAINT `fk_ca_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create ca_payments table if not exists
CREATE TABLE IF NOT EXISTS `ca_payments` (
  `payment_id` int(11) NOT NULL AUTO_INCREMENT,
  `ca_id` int(11) NOT NULL,
  `payment_date` date NOT NULL,
  `amount` decimal(12,2) NOT NULL,
  `payment_method` varchar(50) DEFAULT NULL,
  `reference_number` varchar(50) DEFAULT NULL,
  `payroll_id` int(11) DEFAULT NULL,
  `remarks` text,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`payment_id`),
  KEY `idx_ca` (`ca_id`),
  KEY `idx_payroll` (`payroll_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Add foreign key constraints after all tables are created
ALTER TABLE `ca_payments` 
  ADD CONSTRAINT `fk_payment_ca` FOREIGN KEY (`ca_id`) REFERENCES `cash_advances` (`ca_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_payment_payroll` FOREIGN KEY (`payroll_id`) REFERENCES `payroll_details` (`payroll_id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- Create attendance table if not exists
CREATE TABLE IF NOT EXISTS `attendance` (
  `attendance_id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` varchar(20) NOT NULL,
  `attendance_date` date NOT NULL,
  `time_in` time DEFAULT NULL,
  `time_out` time DEFAULT NULL,
  `status` enum('Present','Absent','Late','Half-day','Leave') NOT NULL DEFAULT 'Present',
  `hours_worked` decimal(5,2) DEFAULT NULL,
  `overtime_hours` decimal(5,2) DEFAULT 0.00,
  `remarks` text,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`attendance_id`),
  UNIQUE KEY `unique_employee_date` (`employee_id`, `attendance_date`),
  KEY `idx_employee` (`employee_id`),
  KEY `idx_date` (`attendance_date`),
  KEY `idx_status` (`status`),
  CONSTRAINT `fk_attendance_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create leaves table if not exists
CREATE TABLE IF NOT EXISTS `leaves` (
  `leave_id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` varchar(20) NOT NULL,
  `leave_type` enum('Sick','Vacation','Maternity','Paternity','Emergency','Other') NOT NULL,
  `start_date` date NOT NULL,
  `end_date` date NOT NULL,
  `total_days` decimal(5,2) NOT NULL,
  `reason` text,
  `status` enum('Pending','Approved','Rejected','Cancelled') NOT NULL DEFAULT 'Pending',
  `approved_by` int(11) DEFAULT NULL,
  `approval_date` datetime DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`leave_id`),
  KEY `idx_employee` (`employee_id`),
  KEY `idx_status` (`status`),
  KEY `idx_date_range` (`start_date`, `end_date`),
  CONSTRAINT `fk_leave_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create payroll_settings table if not exists
CREATE TABLE IF NOT EXISTS `payroll_settings` (
  `setting_id` int(11) NOT NULL DEFAULT 1,
  `company_name` varchar(100) NOT NULL,
  `payroll_cutoff_day` int(11) NOT NULL DEFAULT 15,
  `payroll_release_day` int(11) NOT NULL DEFAULT 30,
  `default_work_hours` decimal(5,2) NOT NULL DEFAULT 8.00,
  `default_work_days` int(11) NOT NULL DEFAULT 22,
  `overtime_rate` decimal(5,2) NOT NULL DEFAULT 1.25,
  `night_diff_rate` decimal(5,2) NOT NULL DEFAULT 0.10,
  `holiday_rate` decimal(5,2) NOT NULL DEFAULT 2.00,
  `special_holiday_rate` decimal(5,2) NOT NULL DEFAULT 1.30,
  `auto_compute_government` tinyint(1) NOT NULL DEFAULT 1,
  `auto_compute_tax` tinyint(1) NOT NULL DEFAULT 1,
  `updated_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`setting_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create company_info table if not exists
CREATE TABLE IF NOT EXISTS `company_info` (
  `id` int(11) NOT NULL DEFAULT 1,
  `company_name` varchar(100) NOT NULL,
  `address` text NOT NULL,
  `contact_number` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `website` varchar(100) DEFAULT NULL,
  `tax_id` varchar(20) DEFAULT NULL,
  `registration_number` varchar(50) DEFAULT NULL,
  `logo_path` varchar(255) DEFAULT NULL,
  `updated_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Insert default values for settings tables
-- Insert default philhealth settings
INSERT IGNORE INTO `philhealth_settings` 
(`id`, `premium_rate`, `salary_floor`, `salary_ceiling`, `updated_at`)
VALUES 
(1, 4.00, 10000.00, 80000.00, NOW());

-- Insert default SSS settings
INSERT IGNORE INTO `sss_settings` 
(`id`, `contribution_rate`, `employee_share_rate`, `employer_share_rate`, `updated_at`)
VALUES 
(1, 13.00, 4.50, 8.50, NOW());

-- Insert default tax settings
INSERT IGNORE INTO `tax_settings` 
(`id`, `withholding_rate`, `minimum_wage`, `personal_exemption`, `additional_exemption`, `auto_calculate`, `use_annualization`, `include_allowances`, `include_bonus`, `updated_date`)
VALUES 
(1, 5.00, 537.00, 50000.00, 25000.00, 1, 1, 1, 0, NOW());

-- Insert default payroll settings
INSERT IGNORE INTO `payroll_settings` 
(`setting_id`, `company_name`, `payroll_cutoff_day`, `payroll_release_day`, 
 `default_work_hours`, `default_work_days`, `overtime_rate`, 
 `night_diff_rate`, `holiday_rate`, `special_holiday_rate`, 
 `auto_compute_government`, `auto_compute_tax`)
VALUES 
(1, 'AMCES Company', 15, 30, 8.00, 22, 1.25, 0.10, 2.00, 1.30, 1, 1);

-- Insert default company info
INSERT IGNORE INTO `company_info` 
(`id`, `company_name`, `address`, `contact_number`, `email`, `website`, `tax_id`)
VALUES 
(1, 'AMCES Company', '123 Main Street, City, Country', '+1234567890', 'info@amces.com', 'www.amces.com', '123-456-789');

-- Insert default job statuses
INSERT IGNORE INTO `tbl_job_status` (`status_name`, `description`, `is_active`, `created_date`) VALUES
('Full-Time', 'Regular full-time employee working standard hours', 1, NOW()),
('Part-Time', 'Employee working less than standard full-time hours', 1, NOW()),
('Contract', 'Employee hired for a specific period or project', 1, NOW()),
('Probationary', 'Employee under evaluation period before regular employment', 1, NOW()),
('Temporary', 'Short-term employee for seasonal or temporary needs', 1, NOW()),
('Intern', 'Student or trainee working to gain experience', 1, NOW()),
('Consultant', 'External professional providing specialized services', 1, NOW());

-- Insert sample tax brackets for 2023
INSERT IGNORE INTO `tax_brackets` 
(`bracket_order`, `income_from`, `income_to`, `tax_rate`, `fixed_amount`, `filing_status`, `tax_year`, `is_active`) 
VALUES
(1, 0.00, 250000.00, 0.00, 0.00, 'Single', '2023', 1),
(2, 250000.01, 400000.00, 20.00, 0.00, 'Single', '2023', 1),
(3, 400000.01, 800000.00, 25.00, 30000.00, 'Single', '2023', 1),
(4, 800000.01, 2000000.00, 30.00, 130000.00, 'Single', '2023', 1),
(5, 2000000.01, 8000000.00, 32.00, 490000.00, 'Single', '2023', 1),
(6, 8000000.01, 999999999.99, 35.00, 2410000.00, 'Single', '2023', 1);

-- Create admin user
INSERT IGNORE INTO `users` 
(`username`, `password`, `full_name`, `email`, `role`, `is_active`, `created_at`) 
VALUES 
('admin', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'System Administrator', 'admin@example.com', 'Admin', 1, NOW());