-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 20, 2025 at 05:21 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `payroll_system`
--

-- --------------------------------------------------------

--
-- Table structure for table `activity_logs`
--

CREATE TABLE `activity_logs` (
  `log_id` int(11) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `action` varchar(100) DEFAULT NULL,
  `table_name` varchar(50) DEFAULT NULL,
  `record_id` varchar(50) DEFAULT NULL,
  `old_values` text DEFAULT NULL,
  `new_values` text DEFAULT NULL,
  `ip_address` varchar(45) DEFAULT NULL,
  `user_agent` text DEFAULT NULL,
  `log_date` datetime DEFAULT current_timestamp(),
  `sdate` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `allowance_transactions`
--

CREATE TABLE `allowance_transactions` (
  `transaction_id` int(11) NOT NULL,
  `allowance_id` int(11) DEFAULT NULL,
  `employee_id` varchar(20) DEFAULT NULL,
  `amount` decimal(10,2) DEFAULT NULL,
  `transaction_date` date DEFAULT NULL,
  `period_id` int(11) DEFAULT NULL,
  `description` text DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `cash_advances`
--

CREATE TABLE `cash_advances` (
  `advance_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `request_date` date NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `reason` text DEFAULT NULL,
  `status` enum('Pending','Approved','Rejected','Paid','Deducted') DEFAULT 'Pending',
  `approved_by` int(11) DEFAULT NULL,
  `approved_date` date DEFAULT NULL,
  `payment_date` date DEFAULT NULL,
  `deduction_start_period` int(11) DEFAULT NULL,
  `deduction_amount` decimal(10,2) DEFAULT NULL,
  `monthly_deduction` decimal(10,2) DEFAULT NULL,
  `deduction_months` int(11) DEFAULT 1,
  `remaining_balance` decimal(10,2) DEFAULT NULL,
  `remarks` text DEFAULT NULL,
  `period_id` int(11) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `cash_advances`
--

INSERT INTO `cash_advances` (`advance_id`, `employee_id`, `request_date`, `amount`, `reason`, `status`, `approved_by`, `approved_date`, `payment_date`, `deduction_start_period`, `deduction_amount`, `monthly_deduction`, `deduction_months`, `remaining_balance`, `remarks`, `period_id`, `created_at`, `updated_at`) VALUES
(1, 'EMP0001', '2025-09-18', 2000.00, 'For personal use', 'Approved', NULL, '2025-09-18', NULL, NULL, NULL, 666.67, 3, 2000.00, 'not yet paid', NULL, '2025-09-18 13:41:54', '2025-09-18 13:44:42'),
(2, 'EMP0002', '2025-09-20', 5000.00, 'TO BUY NAIL POLISH', 'Pending', NULL, NULL, NULL, NULL, NULL, 833.33, 6, 5000.00, NULL, NULL, '2025-09-20 09:31:24', '2025-09-20 09:31:24');

-- --------------------------------------------------------

--
-- Table structure for table `ca_payments`
--

CREATE TABLE `ca_payments` (
  `payment_id` int(11) NOT NULL,
  `ca_id` int(11) NOT NULL,
  `payment_date` date NOT NULL,
  `amount` decimal(12,2) NOT NULL,
  `payment_method` varchar(50) DEFAULT NULL,
  `reference_number` varchar(50) DEFAULT NULL,
  `payroll_id` int(11) DEFAULT NULL,
  `remarks` text DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `company`
--

CREATE TABLE `company` (
  `CompanyId` int(11) NOT NULL,
  `CompanyName` varchar(200) NOT NULL,
  `Address` text DEFAULT NULL,
  `Phone` varchar(50) DEFAULT NULL,
  `Email` varchar(150) DEFAULT NULL,
  `TIN` varchar(50) DEFAULT NULL,
  `SSS` varchar(50) DEFAULT NULL,
  `PhilHealth` varchar(50) DEFAULT NULL,
  `PagIbig` varchar(50) DEFAULT NULL,
  `Logo` longblob DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT 1,
  `CreatedDate` datetime DEFAULT current_timestamp(),
  `ModifiedDate` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `company_settings`
--

CREATE TABLE `company_settings` (
  `company_id` int(11) NOT NULL,
  `company_name` varchar(200) NOT NULL,
  `address` text DEFAULT NULL,
  `phone` varchar(50) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `tin` varchar(50) DEFAULT NULL,
  `sss` varchar(50) DEFAULT NULL,
  `philhealth` varchar(50) DEFAULT NULL,
  `pagibig` varchar(50) DEFAULT NULL,
  `logo` longblob DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `departments`
--

CREATE TABLE `departments` (
  `department_id` int(11) NOT NULL,
  `department_code` varchar(20) DEFAULT NULL,
  `department_name` varchar(100) NOT NULL,
  `description` text DEFAULT NULL,
  `location` varchar(200) DEFAULT NULL,
  `manager` varchar(100) DEFAULT NULL,
  `manager_id` int(11) DEFAULT NULL,
  `budget` decimal(15,2) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp(),
  `created_at` datetime DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `departments`
--

INSERT INTO `departments` (`department_id`, `department_code`, `department_name`, `description`, `location`, `manager`, `manager_id`, `budget`, `is_active`, `created_date`, `created_at`, `modified_date`, `updated_at`) VALUES
(1, 'IT', 'COMPUTERS', 'INFORMATION TECHNOLOGY', NULL, 'SHARMAIN LUMACAD', NULL, 100000.00, 1, '2025-09-18 01:12:06', '2025-09-18 01:12:06', '2025-09-18 01:12:06', '2025-09-18 01:12:06'),
(2, 'HRM', 'Cooking traveling', 'Human Resource Manangement', NULL, 'Jimrud Rojas', NULL, 20000.00, 1, '2025-09-19 22:01:27', '2025-09-19 22:01:27', '2025-09-19 22:01:27', '2025-09-19 22:01:27');

-- --------------------------------------------------------

--
-- Table structure for table `dtr`
--

CREATE TABLE `dtr` (
  `dtr_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `dtr_date` date NOT NULL,
  `time_in` time DEFAULT NULL,
  `time_out` time DEFAULT NULL,
  `break_out` time DEFAULT NULL,
  `break_in` time DEFAULT NULL,
  `total_hours` decimal(4,2) DEFAULT 0.00,
  `overtime_hours` decimal(4,2) DEFAULT 0.00,
  `status` varchar(20) DEFAULT 'Present'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `dtr_records`
--

CREATE TABLE `dtr_records` (
  `dtr_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `dtr_date` date NOT NULL,
  `date` date DEFAULT NULL,
  `time_in` time DEFAULT NULL,
  `time_out` time DEFAULT NULL,
  `break_out` time DEFAULT NULL,
  `break_in` time DEFAULT NULL,
  `total_hours` decimal(4,2) DEFAULT 0.00,
  `regular_hours` decimal(4,2) DEFAULT 0.00,
  `overtime_hours` decimal(4,2) DEFAULT 0.00,
  `late_minutes` int(11) DEFAULT 0,
  `undertime_minutes` int(11) DEFAULT 0,
  `status` enum('Present','Absent','Late','Overtime','Holiday') DEFAULT 'Present',
  `remarks` text DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `dtr_records`
--

INSERT INTO `dtr_records` (`dtr_id`, `employee_id`, `dtr_date`, `date`, `time_in`, `time_out`, `break_out`, `break_in`, `total_hours`, `regular_hours`, `overtime_hours`, `late_minutes`, `undertime_minutes`, `status`, `remarks`, `created_date`) VALUES
(1, 'EMP0001', '2025-09-18', NULL, '02:12:00', '21:37:00', NULL, NULL, NULL, 0.00, NULL, 0, 0, 'Present', NULL, '2025-09-18 02:12:41'),
(2, 'EMP0002', '2025-09-20', NULL, '17:21:00', '20:49:00', NULL, NULL, NULL, 0.00, NULL, 0, 0, 'Present', NULL, '2025-09-20 17:21:52');

-- --------------------------------------------------------

--
-- Table structure for table `employees`
--

CREATE TABLE `employees` (
  `employee_id` varchar(20) NOT NULL,
  `employee_code` varchar(50) DEFAULT NULL,
  `employee_number` varchar(50) DEFAULT NULL,
  `first_name` varchar(100) NOT NULL,
  `middle_name` varchar(100) DEFAULT NULL,
  `last_name` varchar(100) NOT NULL,
  `gender` enum('Male','Female') NOT NULL,
  `birth_date` date DEFAULT NULL,
  `civil_status` enum('Single','Married','Divorced','Widowed') DEFAULT NULL,
  `nationality` varchar(50) DEFAULT NULL,
  `religion` varchar(50) DEFAULT NULL,
  `department_id` int(11) DEFAULT NULL,
  `job_title_id` int(11) DEFAULT NULL,
  `employment_type` enum('Regular','Contractual','Part-time','Probationary') DEFAULT 'Regular',
  `employment_status` enum('Active','Inactive','Terminated') DEFAULT 'Active',
  `hire_date` date DEFAULT NULL,
  `status` enum('Active','Inactive','Terminated') DEFAULT 'Active',
  `supervisor_id` varchar(20) DEFAULT NULL,
  `basic_salary` decimal(10,2) NOT NULL DEFAULT 0.00,
  `allowances` decimal(10,2) DEFAULT 0.00,
  `payroll_type` enum('Monthly','Semi-monthly','Weekly') DEFAULT 'Semi-monthly',
  `bank_account` varchar(50) DEFAULT NULL,
  `bank_name` varchar(100) DEFAULT NULL,
  `address` text DEFAULT NULL,
  `city` varchar(100) DEFAULT NULL,
  `province` varchar(100) DEFAULT NULL,
  `zip_code` varchar(10) DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `mobile` varchar(20) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `emergency_contact` varchar(100) DEFAULT NULL,
  `emergency_phone` varchar(20) DEFAULT NULL,
  `photo_path` varchar(500) DEFAULT NULL,
  `position` varchar(100) DEFAULT NULL,
  `department` varchar(100) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp(),
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `sss_number` varchar(20) DEFAULT NULL COMMENT 'Social Security System Number',
  `pagibig_number` varchar(20) DEFAULT NULL COMMENT 'Pag-IBIG Fund Number',
  `philhealth_number` varchar(20) DEFAULT NULL COMMENT 'PhilHealth Number',
  `tin_number` varchar(20) DEFAULT NULL COMMENT 'Tax Identification Number'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `employees`
--

INSERT INTO `employees` (`employee_id`, `employee_code`, `employee_number`, `first_name`, `middle_name`, `last_name`, `gender`, `birth_date`, `civil_status`, `nationality`, `religion`, `department_id`, `job_title_id`, `employment_type`, `employment_status`, `hire_date`, `status`, `supervisor_id`, `basic_salary`, `allowances`, `payroll_type`, `bank_account`, `bank_name`, `address`, `city`, `province`, `zip_code`, `phone`, `mobile`, `email`, `emergency_contact`, `emergency_phone`, `photo_path`, `position`, `department`, `is_active`, `created_date`, `created_at`, `updated_at`, `sss_number`, `pagibig_number`, `philhealth_number`, `tin_number`) VALUES
('EMP0001', NULL, NULL, 'JIMRUD', 'AMISTA', 'ROJAS', 'Male', '1998-07-12', 'Married', 'FILIPINO', 'CATHOLIC', 1, 1, 'Regular', 'Active', '2025-09-18', 'Active', NULL, 30000.00, 1000.00, 'Semi-monthly', '123456789012', 'BDO', 'PUROK 2 TARIPE MANGAGOY', 'BISLIG CITY', 'SURIGAO DEL SUR', '8311', '09193375154', '09852346641', 'jimrudrojas060060@gmail.com', 'ROSIE ROJAS', '1234567', 'C:\\Users\\joshu\\Downloads\\473362178_1377152640328289_5345618874151702930_n.jpg', NULL, NULL, 1, '2025-09-18 01:27:16', '2025-09-18 01:27:16', '2025-09-18 01:42:35', NULL, NULL, NULL, NULL),
('EMP0002', NULL, NULL, 'SHARMAIN', 'LELIS', 'LUMACAD', 'Female', '2002-04-06', 'Married', 'FILIPINO', 'CATHOLIC', 2, 1, 'Contractual', 'Active', '2025-09-20', 'Active', 'EMP0001', 25000.00, 1000.00, 'Semi-monthly', '123459871234', 'LANDBANK', 'MAYAON PUROK 2', 'MONTEVISTA', 'DAVAO DE ORO', '8000', '09852346641', '09193375154', 'SHARMAINLUMACAD@GMAIL.COM', 'JIMRUD ROJAS', '1237865', 'C:\\Users\\joshu\\Downloads\\65f9e2f9-1d53-4ff0-8a62-d1badaa37266.jpeg', NULL, NULL, 1, '2025-09-20 17:05:00', '2025-09-20 17:05:00', '2025-09-20 17:27:57', '4433445555', '33333454456', '3333555333', '334343434');

-- --------------------------------------------------------

--
-- Table structure for table `employee_allowances`
--

CREATE TABLE `employee_allowances` (
  `allowance_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `allowance_type` enum('fixed','variable') DEFAULT 'fixed',
  `allowance_name` varchar(100) DEFAULT NULL,
  `amount` decimal(10,2) DEFAULT 0.00,
  `is_active` tinyint(1) DEFAULT 1,
  `effective_date` date DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `employee_deductions`
--

CREATE TABLE `employee_deductions` (
  `deduction_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `deduction_type` varchar(50) DEFAULT NULL,
  `deduction_name` varchar(100) DEFAULT NULL,
  `amount` decimal(10,2) DEFAULT 0.00,
  `is_active` tinyint(1) DEFAULT 1,
  `effective_date` date DEFAULT NULL,
  `deduction_date` date DEFAULT NULL,
  `period_id` int(11) DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `employee_tax`
--

CREATE TABLE `employee_tax` (
  `id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `taxable_income` decimal(12,2) NOT NULL,
  `tax_amount` decimal(12,2) NOT NULL,
  `filing_status` varchar(30) NOT NULL,
  `tax_period` varchar(20) NOT NULL,
  `tax_date` date NOT NULL,
  `created_date` datetime NOT NULL DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `job_titles`
--

CREATE TABLE `job_titles` (
  `job_title_id` int(11) NOT NULL,
  `job_code` varchar(20) DEFAULT NULL,
  `job_title` varchar(100) NOT NULL,
  `job_title_name` varchar(100) DEFAULT NULL,
  `description` text DEFAULT NULL,
  `department_id` int(11) DEFAULT NULL,
  `min_salary` decimal(10,2) DEFAULT NULL,
  `max_salary` decimal(10,2) DEFAULT NULL,
  `requirements` text DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `job_titles`
--

INSERT INTO `job_titles` (`job_title_id`, `job_code`, `job_title`, `job_title_name`, `description`, `department_id`, `min_salary`, `max_salary`, `requirements`, `is_active`, `created_at`, `updated_at`) VALUES
(1, 'IT1', 'INFOTECH', NULL, 'SOMETHING ABOUT COMPUTERS', 1, 30000.00, 35000.00, 'Willing  to be train and computer literate most have expirence teach 2  years', 1, '2025-09-18 01:18:40', '2025-09-18 01:18:40');

-- --------------------------------------------------------

--
-- Table structure for table `payroll`
--

CREATE TABLE `payroll` (
  `payroll_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `period_id` int(11) NOT NULL,
  `basic_salary` decimal(10,2) DEFAULT 0.00,
  `overtime_pay` decimal(10,2) DEFAULT 0.00,
  `holiday_pay` decimal(10,2) DEFAULT 0.00,
  `night_differential` decimal(10,2) DEFAULT 0.00,
  `allowances` decimal(10,2) DEFAULT 0.00,
  `bonus` decimal(10,2) DEFAULT 0.00,
  `gross_pay` decimal(10,2) DEFAULT 0.00,
  `sss_contribution` decimal(10,2) DEFAULT 0.00,
  `philhealth_contribution` decimal(10,2) DEFAULT 0.00,
  `pagibig_contribution` decimal(10,2) DEFAULT 0.00,
  `tax_withheld` decimal(10,2) DEFAULT 0.00,
  `loan_deduction` decimal(10,2) DEFAULT 0.00,
  `other_deductions` decimal(10,2) DEFAULT 0.00,
  `total_deductions` decimal(10,2) DEFAULT 0.00,
  `net_pay` decimal(10,2) DEFAULT 0.00,
  `status` varchar(20) DEFAULT 'Draft',
  `processed_date` datetime DEFAULT NULL,
  `processed_by` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `payroll_details`
--

CREATE TABLE `payroll_details` (
  `payroll_id` int(11) NOT NULL,
  `period_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `basic_pay` decimal(10,2) DEFAULT 0.00,
  `basic_salary` decimal(10,2) DEFAULT 0.00,
  `overtime_pay` decimal(10,2) DEFAULT 0.00,
  `overtime` decimal(10,2) DEFAULT 0.00,
  `holiday_pay` decimal(10,2) DEFAULT 0.00,
  `night_differential` decimal(10,2) DEFAULT 0.00,
  `allowances` decimal(10,2) DEFAULT 0.00,
  `bonus` decimal(10,2) DEFAULT 0.00,
  `gross_pay` decimal(10,2) DEFAULT 0.00,
  `sss_deduction` decimal(10,2) DEFAULT 0.00,
  `sss_contribution` decimal(10,2) DEFAULT 0.00,
  `philhealth_deduction` decimal(10,2) DEFAULT 0.00,
  `philhealth_contribution` decimal(10,2) DEFAULT 0.00,
  `pagibig_deduction` decimal(10,2) DEFAULT 0.00,
  `pagibig_contribution` decimal(10,2) DEFAULT 0.00,
  `tax_deduction` decimal(10,2) DEFAULT 0.00,
  `tax_withheld` decimal(10,2) DEFAULT 0.00,
  `loan_deduction` decimal(10,2) DEFAULT 0.00,
  `other_deductions` decimal(10,2) DEFAULT 0.00,
  `deductions` decimal(10,2) DEFAULT 0.00,
  `total_deductions` decimal(10,2) DEFAULT 0.00,
  `net_pay` decimal(10,2) DEFAULT 0.00,
  `status` enum('Draft','Calculated','Approved','Paid') NOT NULL DEFAULT 'Draft',
  `payroll_date` date DEFAULT NULL,
  `period_start` date DEFAULT NULL,
  `period_end` date DEFAULT NULL,
  `processed_date` datetime DEFAULT NULL,
  `processed_by` varchar(50) DEFAULT NULL,
  `calculated_by` int(11) DEFAULT NULL,
  `calculated_date` datetime DEFAULT NULL,
  `approved_by` int(11) DEFAULT NULL,
  `approved_date` datetime DEFAULT NULL,
  `paid_by` int(11) DEFAULT NULL,
  `paid_date` datetime DEFAULT NULL,
  `remarks` text DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp(),
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `payroll_details`
--

INSERT INTO `payroll_details` (`payroll_id`, `period_id`, `employee_id`, `basic_pay`, `basic_salary`, `overtime_pay`, `overtime`, `holiday_pay`, `night_differential`, `allowances`, `bonus`, `gross_pay`, `sss_deduction`, `sss_contribution`, `philhealth_deduction`, `philhealth_contribution`, `pagibig_deduction`, `pagibig_contribution`, `tax_deduction`, `tax_withheld`, `loan_deduction`, `other_deductions`, `deductions`, `total_deductions`, `net_pay`, `status`, `payroll_date`, `period_start`, `period_end`, `processed_date`, `processed_by`, `calculated_by`, `calculated_date`, `approved_by`, `approved_date`, `paid_by`, `paid_date`, `remarks`, `created_date`, `created_at`) VALUES
(19, 1, 'EMP0001', 15000.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 15000.00, 675.00, 0.00, 412.50, 0.00, 100.00, 0.00, 509.33, 0.00, 0.00, 0.00, 0.00, 1696.83, 13303.18, 'Draft', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2025-09-19 21:09:39', '2025-09-19 21:09:39'),
(24, 2, 'EMP0001', 15000.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 15000.00, 675.00, 0.00, 412.50, 0.00, 100.00, 0.00, 509.33, 0.00, 0.00, 0.00, 0.00, 1696.83, 13303.18, 'Draft', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2025-09-20 17:20:03', '2025-09-20 17:20:03'),
(25, 2, 'EMP0002', 12500.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 12500.00, 562.50, 0.00, 343.75, 0.00, 100.00, 0.00, 161.51, 0.00, 0.00, 0.00, 0.00, 1167.76, 11332.24, 'Draft', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2025-09-20 17:20:03', '2025-09-20 17:20:03');

-- --------------------------------------------------------

--
-- Table structure for table `payroll_generations`
--

CREATE TABLE `payroll_generations` (
  `generation_id` int(11) NOT NULL,
  `generation_name` varchar(100) NOT NULL,
  `period_id` int(11) NOT NULL,
  `department_id` int(11) NOT NULL,
  `generation_date` date NOT NULL,
  `status` varchar(20) DEFAULT 'Draft',
  `total_employees` int(11) DEFAULT 0,
  `total_amount` decimal(15,2) DEFAULT 0.00,
  `created_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp(),
  `completed_date` datetime DEFAULT NULL,
  `error_message` text DEFAULT NULL,
  `notes` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `payroll_periods`
--

CREATE TABLE `payroll_periods` (
  `period_id` int(11) NOT NULL,
  `period_name` varchar(100) NOT NULL,
  `start_date` date NOT NULL,
  `end_date` date NOT NULL,
  `payroll_date` date DEFAULT NULL,
  `pay_date` date DEFAULT NULL,
  `date_from` date DEFAULT NULL,
  `date_to` date DEFAULT NULL,
  `status` enum('Active','Inactive','Closed','Draft','Processing') DEFAULT 'Draft',
  `is_active` tinyint(1) DEFAULT 1,
  `description` text DEFAULT NULL,
  `notes` text DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp(),
  `created_at` datetime DEFAULT current_timestamp(),
  `created_by` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `payroll_periods`
--

INSERT INTO `payroll_periods` (`period_id`, `period_name`, `start_date`, `end_date`, `payroll_date`, `pay_date`, `date_from`, `date_to`, `status`, `is_active`, `description`, `notes`, `created_date`, `created_at`, `created_by`) VALUES
(1, 'HOLIDAY', '2025-09-16', '2025-09-30', '2025-10-08', NULL, NULL, NULL, 'Active', 1, 'CUT OFF', NULL, '2025-09-18 02:01:01', '2025-09-18 02:01:01', NULL),
(2, 'HOLIDAY DUTY', '2025-10-01', '2025-10-15', '2025-10-16', NULL, NULL, NULL, 'Active', 1, 'HOLIDAY DUTY ALL EMPLOYEEE', NULL, '2025-09-19 21:11:20', '2025-09-19 21:11:20', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `payroll_settings`
--

CREATE TABLE `payroll_settings` (
  `id` int(11) NOT NULL DEFAULT 1,
  `overtime_rate` decimal(5,2) DEFAULT 1.25,
  `working_hours_per_day` int(11) DEFAULT 8,
  `working_days_per_week` int(11) DEFAULT 5,
  `auto_calculate_overtime` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `payroll_settings`
--

INSERT INTO `payroll_settings` (`id`, `overtime_rate`, `working_hours_per_day`, `working_days_per_week`, `auto_calculate_overtime`, `created_date`, `modified_date`) VALUES
(1, 1.25, 8, 5, 1, '2025-09-18 00:18:46', '2025-09-18 00:18:46');

-- --------------------------------------------------------

--
-- Table structure for table `philhealth_contributions`
--

CREATE TABLE `philhealth_contributions` (
  `id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `contribution_date` date NOT NULL,
  `salary` decimal(12,2) NOT NULL,
  `employee_share` decimal(12,2) NOT NULL,
  `employer_share` decimal(12,2) NOT NULL,
  `total_contribution` decimal(12,2) NOT NULL,
  `period_id` int(11) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `philhealth_rates`
--

CREATE TABLE `philhealth_rates` (
  `id` int(11) NOT NULL,
  `effective_date` date NOT NULL,
  `min_salary` decimal(10,2) NOT NULL,
  `max_salary` decimal(10,2) NOT NULL,
  `employee_rate` decimal(5,2) NOT NULL,
  `employer_rate` decimal(5,2) NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 0,
  `created_date` datetime NOT NULL DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `philhealth_settings`
--

CREATE TABLE `philhealth_settings` (
  `id` int(11) NOT NULL DEFAULT 1,
  `min_salary` decimal(10,2) DEFAULT 10000.00,
  `max_salary` decimal(10,2) DEFAULT 80000.00,
  `employee_rate` decimal(5,2) DEFAULT 2.75,
  `employer_rate` decimal(5,2) DEFAULT 2.75,
  `auto_calculate` tinyint(1) DEFAULT 1,
  `include_allowances` tinyint(1) DEFAULT 0,
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `column_name` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `philhealth_settings`
--

INSERT INTO `philhealth_settings` (`id`, `min_salary`, `max_salary`, `employee_rate`, `employer_rate`, `auto_calculate`, `include_allowances`, `modified_date`, `column_name`) VALUES
(1, 10000.00, 80000.00, 2.75, 2.75, 1, 0, '2025-09-18 00:18:46', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `sss_contributions`
--

CREATE TABLE `sss_contributions` (
  `id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `salary_credit` decimal(10,2) NOT NULL,
  `employee_share` decimal(10,2) NOT NULL,
  `employer_share` decimal(10,2) NOT NULL,
  `total_contribution` decimal(10,2) NOT NULL,
  `contribution_date` date NOT NULL,
  `created_date` datetime NOT NULL DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `sss_contribution_table`
--

CREATE TABLE `sss_contribution_table` (
  `id` int(11) NOT NULL,
  `salary_from` decimal(10,2) NOT NULL,
  `salary_to` decimal(10,2) NOT NULL,
  `salary_credit` decimal(10,2) NOT NULL,
  `employee_contribution` decimal(10,2) NOT NULL,
  `employer_contribution` decimal(10,2) NOT NULL,
  `total_contribution` decimal(10,2) NOT NULL,
  `effective_date` date DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `sss_contribution_table`
--

INSERT INTO `sss_contribution_table` (`id`, `salary_from`, `salary_to`, `salary_credit`, `employee_contribution`, `employer_contribution`, `total_contribution`, `effective_date`, `is_active`, `created_date`) VALUES
(2, 30.00, 31000.00, 15000.00, 480.00, 480.00, 960.00, '2025-09-20', 1, '2025-09-20 17:49:02'),
(5, 20000.00, 25000.00, 12000.00, 400.00, 400.00, 800.00, '2025-09-20', 1, '2025-09-20 21:51:37'),
(6, 20000.00, 22000.00, 11000.00, 250.00, 250.00, 500.00, '2025-09-20', 1, '2025-09-20 22:54:25');

-- --------------------------------------------------------

--
-- Table structure for table `sss_settings`
--

CREATE TABLE `sss_settings` (
  `id` int(11) NOT NULL DEFAULT 1,
  `employer_rate` decimal(5,2) DEFAULT 8.50,
  `employee_rate` decimal(5,2) DEFAULT 4.50,
  `max_salary_credit` decimal(10,2) DEFAULT 25000.00,
  `min_salary_credit` decimal(10,2) DEFAULT 1000.00,
  `auto_calculate` tinyint(1) DEFAULT 1,
  `include_allowances` tinyint(1) DEFAULT 0,
  `include_overtime` tinyint(1) DEFAULT 0,
  `updated_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `sss_settings`
--

INSERT INTO `sss_settings` (`id`, `employer_rate`, `employee_rate`, `max_salary_credit`, `min_salary_credit`, `auto_calculate`, `include_allowances`, `include_overtime`, `updated_date`) VALUES
(1, 8.50, 4.50, 25000.00, 1000.00, 1, 1, 1, '2025-09-20 20:19:29');

-- --------------------------------------------------------

--
-- Table structure for table `tax_brackets`
--

CREATE TABLE `tax_brackets` (
  `id` int(11) NOT NULL,
  `bracket_order` int(11) NOT NULL,
  `income_from` decimal(12,2) NOT NULL,
  `income_to` decimal(12,2) NOT NULL,
  `tax_rate` decimal(5,2) NOT NULL,
  `fixed_amount` decimal(12,2) NOT NULL,
  `filing_status` varchar(30) NOT NULL,
  `tax_year` varchar(4) NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_date` datetime NOT NULL DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tax_settings`
--

CREATE TABLE `tax_settings` (
  `id` int(11) NOT NULL DEFAULT 1,
  `withholding_rate` decimal(5,2) DEFAULT 10.00,
  `minimum_wage` decimal(10,2) DEFAULT 15000.00,
  `personal_exemption` decimal(10,2) DEFAULT 50000.00,
  `additional_exemption` decimal(10,2) DEFAULT 25000.00,
  `auto_calculate` tinyint(1) DEFAULT 1,
  `use_annualization` tinyint(1) DEFAULT 0,
  `include_allowances` tinyint(1) DEFAULT 1,
  `include_bonus` tinyint(1) DEFAULT 1,
  `updated_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `tax_settings`
--

INSERT INTO `tax_settings` (`id`, `withholding_rate`, `minimum_wage`, `personal_exemption`, `additional_exemption`, `auto_calculate`, `use_annualization`, `include_allowances`, `include_bonus`, `updated_date`) VALUES
(1, 10.00, 15000.00, 50000.00, 25000.00, 1, 0, 1, 1, '2025-09-18 00:18:46');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_ca`
--

CREATE TABLE `tbl_ca` (
  `id` int(11) NOT NULL,
  `employee_id` varchar(20) DEFAULT NULL,
  `cash_advance` decimal(10,2) DEFAULT NULL,
  `amount` decimal(10,2) DEFAULT NULL,
  `payrollperiod` date DEFAULT NULL,
  `status` varchar(20) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_company`
--

CREATE TABLE `tbl_company` (
  `id` int(11) NOT NULL,
  `company_name` varchar(200) DEFAULT NULL,
  `address` text DEFAULT NULL,
  `phone` varchar(50) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_department`
--

CREATE TABLE `tbl_department` (
  `id` int(11) NOT NULL,
  `department_name` varchar(100) NOT NULL,
  `description` text DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_employee`
--

CREATE TABLE `tbl_employee` (
  `id` int(11) NOT NULL,
  `employee_id` varchar(20) DEFAULT NULL,
  `first_name` varchar(100) DEFAULT NULL,
  `last_name` varchar(100) DEFAULT NULL,
  `middle_name` varchar(100) DEFAULT NULL,
  `department` varchar(100) DEFAULT NULL,
  `position` varchar(100) DEFAULT NULL,
  `job_status_id` int(11) DEFAULT NULL,
  `basic_salary` decimal(10,2) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_employee_tax`
--

CREATE TABLE `tbl_employee_tax` (
  `id` int(11) NOT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `tax_amount` decimal(10,2) DEFAULT NULL,
  `tax_period` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_job_status`
--

CREATE TABLE `tbl_job_status` (
  `id` int(11) NOT NULL,
  `status_name` varchar(50) NOT NULL,
  `description` text DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_log`
--

CREATE TABLE `tbl_log` (
  `id` int(11) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `action` varchar(100) DEFAULT NULL,
  `sdate` date DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_login_log`
--

CREATE TABLE `tbl_login_log` (
  `id` int(11) NOT NULL,
  `employee_id` varchar(20) DEFAULT NULL,
  `login_time` datetime DEFAULT NULL,
  `ip_address` varchar(45) DEFAULT NULL,
  `user_agent` text DEFAULT NULL,
  `successful` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_payrollperiod`
--

CREATE TABLE `tbl_payrollperiod` (
  `id` int(11) NOT NULL,
  `period_name` varchar(100) DEFAULT NULL,
  `start_date` date DEFAULT NULL,
  `end_date` date DEFAULT NULL,
  `status` varchar(20) DEFAULT 'ACTIVE',
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_qr`
--

CREATE TABLE `tbl_qr` (
  `id` int(11) NOT NULL,
  `qr_data` text DEFAULT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_qr_history`
--

CREATE TABLE `tbl_qr_history` (
  `id` int(11) NOT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `qr_type` varchar(50) DEFAULT NULL,
  `qr_data` text DEFAULT NULL,
  `file_path` varchar(500) DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp(),
  `created_by` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_qr_settings`
--

CREATE TABLE `tbl_qr_settings` (
  `id` int(11) NOT NULL DEFAULT 1,
  `qr_size` int(11) DEFAULT 200,
  `error_correction_level` varchar(10) DEFAULT 'M',
  `output_format` varchar(10) DEFAULT 'PNG',
  `data_prefix` varchar(50) DEFAULT NULL,
  `include_employee_info` tinyint(1) DEFAULT 1,
  `auto_generate` tinyint(1) DEFAULT 0,
  `created_date` datetime DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `tbl_qr_settings`
--

INSERT INTO `tbl_qr_settings` (`id`, `qr_size`, `error_correction_level`, `output_format`, `data_prefix`, `include_employee_info`, `auto_generate`, `created_date`, `modified_date`) VALUES
(1, 200, 'M', 'PNG', NULL, 1, 0, '2025-09-18 00:18:46', '2025-09-18 00:18:46');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_sss_contribution`
--

CREATE TABLE `tbl_sss_contribution` (
  `id` int(11) NOT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `employee_contribution` decimal(10,2) DEFAULT NULL,
  `employer_contribution` decimal(10,2) DEFAULT NULL,
  `total_contribution` decimal(10,2) DEFAULT NULL,
  `pay_period` varchar(50) DEFAULT NULL,
  `contribution_date` date DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp(),
  `salary_credit` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `time_adjustments`
--

CREATE TABLE `time_adjustments` (
  `adjustment_id` int(11) NOT NULL,
  `employee_id` varchar(20) NOT NULL,
  `adjustment_date` date NOT NULL,
  `original_time_in` time DEFAULT NULL,
  `original_time_out` time DEFAULT NULL,
  `adjusted_time_in` time DEFAULT NULL,
  `adjusted_time_out` time DEFAULT NULL,
  `reason` text DEFAULT NULL,
  `approved_by` int(11) DEFAULT NULL,
  `approved_date` datetime DEFAULT NULL,
  `status` enum('Pending','Approved','Rejected') DEFAULT 'Pending',
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `units`
--

CREATE TABLE `units` (
  `id` int(11) NOT NULL,
  `unit_name` varchar(100) NOT NULL,
  `unit_symbol` varchar(20) DEFAULT NULL,
  `category_id` int(11) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `unit_categories`
--

CREATE TABLE `unit_categories` (
  `id` int(11) NOT NULL,
  `category_name` varchar(100) NOT NULL,
  `description` text DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `unit_settings`
--

CREATE TABLE `unit_settings` (
  `id` int(11) NOT NULL DEFAULT 1,
  `default_unit` varchar(50) DEFAULT NULL,
  `auto_convert` tinyint(1) DEFAULT 0,
  `show_in_reports` tinyint(1) DEFAULT 1,
  `created_date` datetime DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `unit_settings`
--

INSERT INTO `unit_settings` (`id`, `default_unit`, `auto_convert`, `show_in_reports`, `created_date`, `modified_date`) VALUES
(1, 'pieces', 0, 1, '2025-09-18 00:18:46', '2025-09-18 00:18:46');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `user_id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `password_hash` varchar(255) DEFAULT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `email` varchar(150) DEFAULT NULL,
  `user_type` enum('Admin','HR','Manager','Employee') NOT NULL DEFAULT 'Employee',
  `department_id` int(11) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `last_password_change` datetime DEFAULT NULL,
  `created_date` datetime DEFAULT current_timestamp(),
  `modified_date` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`user_id`, `username`, `password`, `password_hash`, `first_name`, `last_name`, `email`, `user_type`, `department_id`, `is_active`, `last_password_change`, `created_date`, `modified_date`) VALUES
(1, 'admin', 'admin123', '0192023a7bbd73250516f069df18b500', 'System', 'Administrator', 'admin@amces.com', 'Admin', NULL, 1, NULL, '2025-09-18 00:32:06', '2025-09-18 00:32:06');

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_cash_advances`
-- (See below for the actual view)
--
CREATE TABLE `vw_cash_advances` (
`ca_id` int(11)
,`employee_id` varchar(20)
,`ca_amount` decimal(10,2)
,`ca_date` date
,`ca_status` enum('Pending','Approved','Rejected','Paid','Deducted')
,`reason` text
,`purpose` text
,`approved_by` int(11)
,`approved_date` date
,`payment_date` date
,`deduction_start_period` int(11)
,`deduction_amount` decimal(10,2)
,`monthly_deduction` decimal(10,2)
,`deduction_months` int(11)
,`remaining_balance` decimal(10,2)
,`remarks` text
,`period_id` int(11)
,`created_at` timestamp
,`updated_at` timestamp
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_departments`
-- (See below for the actual view)
--
CREATE TABLE `vw_departments` (
`id` int(11)
,`department_id` int(11)
,`department_code` varchar(20)
,`department_name` varchar(100)
,`description` text
,`location` varchar(200)
,`manager` varchar(100)
,`manager_id` int(11)
,`budget` decimal(15,2)
,`is_active` tinyint(1)
,`created_date` datetime
,`created_at` datetime
,`modified_date` datetime
,`updated_at` datetime
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_employees`
-- (See below for the actual view)
--
CREATE TABLE `vw_employees` (
`id` varchar(20)
,`employee_id` varchar(20)
,`employee_code` varchar(50)
,`employee_number` varchar(50)
,`first_name` varchar(100)
,`last_name` varchar(100)
,`middle_name` varchar(100)
,`gender` enum('Male','Female')
,`birth_date` date
,`civil_status` enum('Single','Married','Divorced','Widowed')
,`nationality` varchar(50)
,`religion` varchar(50)
,`department_id` int(11)
,`job_title_id` int(11)
,`employment_type` enum('Regular','Contractual','Part-time','Probationary')
,`employment_status` enum('Active','Inactive','Terminated')
,`hire_date` date
,`status` enum('Active','Inactive','Terminated')
,`supervisor_id` varchar(20)
,`basic_salary` decimal(10,2)
,`allowances` decimal(10,2)
,`payroll_type` enum('Monthly','Semi-monthly','Weekly')
,`bank_account` varchar(50)
,`bank_name` varchar(100)
,`address` text
,`city` varchar(100)
,`province` varchar(100)
,`zip_code` varchar(10)
,`phone` varchar(20)
,`mobile` varchar(20)
,`email` varchar(150)
,`emergency_contact` varchar(100)
,`emergency_phone` varchar(20)
,`photo_path` varchar(500)
,`position` varchar(100)
,`department` varchar(100)
,`is_active` tinyint(1)
,`created_date` datetime
,`created_at` datetime
,`updated_at` datetime
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_job_titles`
-- (See below for the actual view)
--
CREATE TABLE `vw_job_titles` (
`id` int(11)
,`job_title_id` int(11)
,`job_code` varchar(20)
,`job_title` varchar(100)
,`job_title_name` varchar(100)
,`description` text
,`department_id` int(11)
,`min_salary` decimal(10,2)
,`max_salary` decimal(10,2)
,`requirements` text
,`is_active` tinyint(1)
,`created_at` datetime
,`updated_at` datetime
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_payroll`
-- (See below for the actual view)
--
CREATE TABLE `vw_payroll` (
`payroll_id` int(11)
,`period_id` int(11)
,`employee_id` varchar(20)
,`basic_pay` decimal(10,2)
,`basic_salary` decimal(10,2)
,`overtime_pay` decimal(10,2)
,`overtime` decimal(10,2)
,`holiday_pay` decimal(10,2)
,`night_differential` decimal(10,2)
,`allowances` decimal(10,2)
,`bonus` decimal(10,2)
,`gross_pay` decimal(10,2)
,`sss_deduction` decimal(10,2)
,`sss_contribution` decimal(10,2)
,`philhealth_deduction` decimal(10,2)
,`philhealth_contribution` decimal(10,2)
,`pagibig_deduction` decimal(10,2)
,`pagibig_contribution` decimal(10,2)
,`tax_deduction` decimal(10,2)
,`tax_withheld` decimal(10,2)
,`loan_deduction` decimal(10,2)
,`other_deductions` decimal(10,2)
,`deductions` decimal(10,2)
,`total_deductions` decimal(10,2)
,`net_pay` decimal(10,2)
,`status` enum('Draft','Calculated','Approved','Paid')
,`payroll_date` date
,`period_start` date
,`period_end` date
,`processed_date` datetime
,`processed_by` varchar(50)
,`calculated_by` int(11)
,`calculated_date` datetime
,`approved_by` int(11)
,`approved_date` datetime
,`paid_by` int(11)
,`paid_date` datetime
,`remarks` text
,`created_date` datetime
,`created_at` datetime
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_payroll_periods`
-- (See below for the actual view)
--
CREATE TABLE `vw_payroll_periods` (
`id` int(11)
,`period_id` int(11)
,`period_name` varchar(100)
,`start_date` date
,`end_date` date
,`payroll_date` date
,`pay_date` date
,`date_from` date
,`date_to` date
,`status` enum('Active','Inactive','Closed','Draft','Processing')
,`is_active` tinyint(1)
,`description` text
,`notes` text
,`created_date` datetime
,`created_at` datetime
,`created_by` varchar(50)
);

-- --------------------------------------------------------

--
-- Structure for view `vw_cash_advances`
--
DROP TABLE IF EXISTS `vw_cash_advances`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_cash_advances`  AS SELECT `cash_advances`.`advance_id` AS `ca_id`, `cash_advances`.`employee_id` AS `employee_id`, `cash_advances`.`amount` AS `ca_amount`, `cash_advances`.`request_date` AS `ca_date`, `cash_advances`.`status` AS `ca_status`, `cash_advances`.`reason` AS `reason`, `cash_advances`.`reason` AS `purpose`, `cash_advances`.`approved_by` AS `approved_by`, `cash_advances`.`approved_date` AS `approved_date`, `cash_advances`.`payment_date` AS `payment_date`, `cash_advances`.`deduction_start_period` AS `deduction_start_period`, `cash_advances`.`deduction_amount` AS `deduction_amount`, `cash_advances`.`monthly_deduction` AS `monthly_deduction`, `cash_advances`.`deduction_months` AS `deduction_months`, `cash_advances`.`remaining_balance` AS `remaining_balance`, `cash_advances`.`remarks` AS `remarks`, `cash_advances`.`period_id` AS `period_id`, `cash_advances`.`created_at` AS `created_at`, `cash_advances`.`updated_at` AS `updated_at` FROM `cash_advances` ;

-- --------------------------------------------------------

--
-- Structure for view `vw_departments`
--
DROP TABLE IF EXISTS `vw_departments`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_departments`  AS SELECT `departments`.`department_id` AS `id`, `departments`.`department_id` AS `department_id`, `departments`.`department_code` AS `department_code`, `departments`.`department_name` AS `department_name`, `departments`.`description` AS `description`, `departments`.`location` AS `location`, `departments`.`manager` AS `manager`, `departments`.`manager_id` AS `manager_id`, `departments`.`budget` AS `budget`, `departments`.`is_active` AS `is_active`, `departments`.`created_date` AS `created_date`, `departments`.`created_at` AS `created_at`, `departments`.`modified_date` AS `modified_date`, `departments`.`updated_at` AS `updated_at` FROM `departments` ;

-- --------------------------------------------------------

--
-- Structure for view `vw_employees`
--
DROP TABLE IF EXISTS `vw_employees`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_employees`  AS SELECT `employees`.`employee_id` AS `id`, `employees`.`employee_id` AS `employee_id`, `employees`.`employee_code` AS `employee_code`, `employees`.`employee_number` AS `employee_number`, `employees`.`first_name` AS `first_name`, `employees`.`last_name` AS `last_name`, `employees`.`middle_name` AS `middle_name`, `employees`.`gender` AS `gender`, `employees`.`birth_date` AS `birth_date`, `employees`.`civil_status` AS `civil_status`, `employees`.`nationality` AS `nationality`, `employees`.`religion` AS `religion`, `employees`.`department_id` AS `department_id`, `employees`.`job_title_id` AS `job_title_id`, `employees`.`employment_type` AS `employment_type`, `employees`.`employment_status` AS `employment_status`, `employees`.`hire_date` AS `hire_date`, `employees`.`status` AS `status`, `employees`.`supervisor_id` AS `supervisor_id`, `employees`.`basic_salary` AS `basic_salary`, `employees`.`allowances` AS `allowances`, `employees`.`payroll_type` AS `payroll_type`, `employees`.`bank_account` AS `bank_account`, `employees`.`bank_name` AS `bank_name`, `employees`.`address` AS `address`, `employees`.`city` AS `city`, `employees`.`province` AS `province`, `employees`.`zip_code` AS `zip_code`, `employees`.`phone` AS `phone`, `employees`.`mobile` AS `mobile`, `employees`.`email` AS `email`, `employees`.`emergency_contact` AS `emergency_contact`, `employees`.`emergency_phone` AS `emergency_phone`, `employees`.`photo_path` AS `photo_path`, `employees`.`position` AS `position`, `employees`.`department` AS `department`, `employees`.`is_active` AS `is_active`, `employees`.`created_date` AS `created_date`, `employees`.`created_at` AS `created_at`, `employees`.`updated_at` AS `updated_at` FROM `employees` ;

-- --------------------------------------------------------

--
-- Structure for view `vw_job_titles`
--
DROP TABLE IF EXISTS `vw_job_titles`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_job_titles`  AS SELECT `job_titles`.`job_title_id` AS `id`, `job_titles`.`job_title_id` AS `job_title_id`, `job_titles`.`job_code` AS `job_code`, `job_titles`.`job_title` AS `job_title`, `job_titles`.`job_title_name` AS `job_title_name`, `job_titles`.`description` AS `description`, `job_titles`.`department_id` AS `department_id`, `job_titles`.`min_salary` AS `min_salary`, `job_titles`.`max_salary` AS `max_salary`, `job_titles`.`requirements` AS `requirements`, `job_titles`.`is_active` AS `is_active`, `job_titles`.`created_at` AS `created_at`, `job_titles`.`updated_at` AS `updated_at` FROM `job_titles` ;

-- --------------------------------------------------------

--
-- Structure for view `vw_payroll`
--
DROP TABLE IF EXISTS `vw_payroll`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_payroll`  AS SELECT `payroll_details`.`payroll_id` AS `payroll_id`, `payroll_details`.`period_id` AS `period_id`, `payroll_details`.`employee_id` AS `employee_id`, `payroll_details`.`basic_pay` AS `basic_pay`, `payroll_details`.`basic_salary` AS `basic_salary`, `payroll_details`.`overtime_pay` AS `overtime_pay`, `payroll_details`.`overtime` AS `overtime`, `payroll_details`.`holiday_pay` AS `holiday_pay`, `payroll_details`.`night_differential` AS `night_differential`, `payroll_details`.`allowances` AS `allowances`, `payroll_details`.`bonus` AS `bonus`, `payroll_details`.`gross_pay` AS `gross_pay`, `payroll_details`.`sss_deduction` AS `sss_deduction`, `payroll_details`.`sss_contribution` AS `sss_contribution`, `payroll_details`.`philhealth_deduction` AS `philhealth_deduction`, `payroll_details`.`philhealth_contribution` AS `philhealth_contribution`, `payroll_details`.`pagibig_deduction` AS `pagibig_deduction`, `payroll_details`.`pagibig_contribution` AS `pagibig_contribution`, `payroll_details`.`tax_deduction` AS `tax_deduction`, `payroll_details`.`tax_withheld` AS `tax_withheld`, `payroll_details`.`loan_deduction` AS `loan_deduction`, `payroll_details`.`other_deductions` AS `other_deductions`, `payroll_details`.`deductions` AS `deductions`, `payroll_details`.`total_deductions` AS `total_deductions`, `payroll_details`.`net_pay` AS `net_pay`, `payroll_details`.`status` AS `status`, `payroll_details`.`payroll_date` AS `payroll_date`, `payroll_details`.`period_start` AS `period_start`, `payroll_details`.`period_end` AS `period_end`, `payroll_details`.`processed_date` AS `processed_date`, `payroll_details`.`processed_by` AS `processed_by`, `payroll_details`.`calculated_by` AS `calculated_by`, `payroll_details`.`calculated_date` AS `calculated_date`, `payroll_details`.`approved_by` AS `approved_by`, `payroll_details`.`approved_date` AS `approved_date`, `payroll_details`.`paid_by` AS `paid_by`, `payroll_details`.`paid_date` AS `paid_date`, `payroll_details`.`remarks` AS `remarks`, `payroll_details`.`created_date` AS `created_date`, `payroll_details`.`created_at` AS `created_at` FROM `payroll_details` ;

-- --------------------------------------------------------

--
-- Structure for view `vw_payroll_periods`
--
DROP TABLE IF EXISTS `vw_payroll_periods`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_payroll_periods`  AS SELECT `payroll_periods`.`period_id` AS `id`, `payroll_periods`.`period_id` AS `period_id`, `payroll_periods`.`period_name` AS `period_name`, `payroll_periods`.`start_date` AS `start_date`, `payroll_periods`.`end_date` AS `end_date`, `payroll_periods`.`payroll_date` AS `payroll_date`, `payroll_periods`.`pay_date` AS `pay_date`, `payroll_periods`.`date_from` AS `date_from`, `payroll_periods`.`date_to` AS `date_to`, `payroll_periods`.`status` AS `status`, `payroll_periods`.`is_active` AS `is_active`, `payroll_periods`.`description` AS `description`, `payroll_periods`.`notes` AS `notes`, `payroll_periods`.`created_date` AS `created_date`, `payroll_periods`.`created_at` AS `created_at`, `payroll_periods`.`created_by` AS `created_by` FROM `payroll_periods` ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `activity_logs`
--
ALTER TABLE `activity_logs`
  ADD PRIMARY KEY (`log_id`),
  ADD KEY `idx_log_date` (`log_date`),
  ADD KEY `idx_log_user` (`user_id`),
  ADD KEY `idx_log_action` (`action`);

--
-- Indexes for table `allowance_transactions`
--
ALTER TABLE `allowance_transactions`
  ADD PRIMARY KEY (`transaction_id`),
  ADD KEY `allowance_id` (`allowance_id`),
  ADD KEY `idx_transaction_employee` (`employee_id`),
  ADD KEY `idx_transaction_date` (`transaction_date`),
  ADD KEY `idx_transaction_period` (`period_id`);

--
-- Indexes for table `cash_advances`
--
ALTER TABLE `cash_advances`
  ADD PRIMARY KEY (`advance_id`),
  ADD KEY `approved_by` (`approved_by`),
  ADD KEY `deduction_start_period` (`deduction_start_period`),
  ADD KEY `idx_employee` (`employee_id`),
  ADD KEY `idx_status` (`status`),
  ADD KEY `idx_request_date` (`request_date`),
  ADD KEY `idx_cash_advances_employee_status` (`employee_id`,`status`);

--
-- Indexes for table `ca_payments`
--
ALTER TABLE `ca_payments`
  ADD PRIMARY KEY (`payment_id`),
  ADD KEY `idx_ca` (`ca_id`),
  ADD KEY `idx_payroll` (`payroll_id`);

--
-- Indexes for table `company`
--
ALTER TABLE `company`
  ADD PRIMARY KEY (`CompanyId`);

--
-- Indexes for table `company_settings`
--
ALTER TABLE `company_settings`
  ADD PRIMARY KEY (`company_id`);

--
-- Indexes for table `departments`
--
ALTER TABLE `departments`
  ADD PRIMARY KEY (`department_id`),
  ADD UNIQUE KEY `department_code` (`department_code`),
  ADD KEY `idx_dept_code` (`department_code`),
  ADD KEY `idx_dept_name` (`department_name`),
  ADD KEY `idx_is_active` (`is_active`);

--
-- Indexes for table `dtr`
--
ALTER TABLE `dtr`
  ADD PRIMARY KEY (`dtr_id`),
  ADD KEY `employee_id` (`employee_id`);

--
-- Indexes for table `dtr_records`
--
ALTER TABLE `dtr_records`
  ADD PRIMARY KEY (`dtr_id`),
  ADD UNIQUE KEY `unique_employee_date` (`employee_id`,`dtr_date`),
  ADD KEY `idx_dtr_employee` (`employee_id`),
  ADD KEY `idx_dtr_date` (`dtr_date`),
  ADD KEY `idx_dtr_status` (`status`),
  ADD KEY `idx_dtr_employee_date` (`employee_id`,`dtr_date`);

--
-- Indexes for table `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`employee_id`),
  ADD KEY `supervisor_id` (`supervisor_id`),
  ADD KEY `idx_employee_code` (`employee_code`),
  ADD KEY `idx_employee_number` (`employee_number`),
  ADD KEY `idx_name` (`last_name`,`first_name`),
  ADD KEY `idx_department` (`department_id`),
  ADD KEY `idx_job_title` (`job_title_id`),
  ADD KEY `idx_employment_status` (`employment_status`),
  ADD KEY `idx_status` (`status`),
  ADD KEY `idx_employees_dept_status` (`department_id`,`status`),
  ADD KEY `idx_employees_hire_date` (`hire_date`);

--
-- Indexes for table `employee_allowances`
--
ALTER TABLE `employee_allowances`
  ADD PRIMARY KEY (`allowance_id`),
  ADD KEY `idx_employee_allowance` (`employee_id`),
  ADD KEY `idx_allowance_type` (`allowance_type`);

--
-- Indexes for table `employee_deductions`
--
ALTER TABLE `employee_deductions`
  ADD PRIMARY KEY (`deduction_id`),
  ADD KEY `idx_employee_deduction` (`employee_id`),
  ADD KEY `idx_deduction_date` (`deduction_date`),
  ADD KEY `idx_deduction_period` (`period_id`);

--
-- Indexes for table `employee_tax`
--
ALTER TABLE `employee_tax`
  ADD PRIMARY KEY (`id`),
  ADD KEY `employee_id` (`employee_id`);

--
-- Indexes for table `job_titles`
--
ALTER TABLE `job_titles`
  ADD PRIMARY KEY (`job_title_id`),
  ADD UNIQUE KEY `job_code` (`job_code`),
  ADD KEY `idx_job_code` (`job_code`),
  ADD KEY `idx_job_title` (`job_title`),
  ADD KEY `idx_department` (`department_id`);

--
-- Indexes for table `payroll`
--
ALTER TABLE `payroll`
  ADD PRIMARY KEY (`payroll_id`),
  ADD KEY `employee_id` (`employee_id`),
  ADD KEY `period_id` (`period_id`);

--
-- Indexes for table `payroll_details`
--
ALTER TABLE `payroll_details`
  ADD PRIMARY KEY (`payroll_id`),
  ADD KEY `idx_payroll_period` (`period_id`),
  ADD KEY `idx_payroll_employee` (`employee_id`),
  ADD KEY `idx_payroll_status` (`status`),
  ADD KEY `idx_payroll_calculated` (`calculated_by`,`calculated_date`),
  ADD KEY `idx_payroll_approved` (`approved_by`,`approved_date`),
  ADD KEY `idx_payroll_paid` (`paid_by`,`paid_date`),
  ADD KEY `idx_payroll_details_period_status` (`period_id`,`status`);

--
-- Indexes for table `payroll_generations`
--
ALTER TABLE `payroll_generations`
  ADD PRIMARY KEY (`generation_id`),
  ADD KEY `idx_payroll_gen_period` (`period_id`),
  ADD KEY `idx_payroll_gen_dept` (`department_id`),
  ADD KEY `idx_payroll_gen_date` (`generation_date`);

--
-- Indexes for table `payroll_periods`
--
ALTER TABLE `payroll_periods`
  ADD PRIMARY KEY (`period_id`),
  ADD KEY `idx_period_dates` (`start_date`,`end_date`),
  ADD KEY `idx_status` (`status`),
  ADD KEY `idx_is_active` (`is_active`);

--
-- Indexes for table `payroll_settings`
--
ALTER TABLE `payroll_settings`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `philhealth_contributions`
--
ALTER TABLE `philhealth_contributions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `employee_id` (`employee_id`),
  ADD KEY `idx_contribution_date` (`contribution_date`);

--
-- Indexes for table `philhealth_rates`
--
ALTER TABLE `philhealth_rates`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `philhealth_settings`
--
ALTER TABLE `philhealth_settings`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sss_contributions`
--
ALTER TABLE `sss_contributions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `employee_id` (`employee_id`);

--
-- Indexes for table `sss_contribution_table`
--
ALTER TABLE `sss_contribution_table`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idx_salary_range` (`salary_from`,`salary_to`),
  ADD KEY `idx_effective_date` (`effective_date`);

--
-- Indexes for table `sss_settings`
--
ALTER TABLE `sss_settings`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tax_brackets`
--
ALTER TABLE `tax_brackets`
  ADD PRIMARY KEY (`id`),
  ADD KEY `tax_year_status` (`tax_year`,`filing_status`);

--
-- Indexes for table `tax_settings`
--
ALTER TABLE `tax_settings`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_ca`
--
ALTER TABLE `tbl_ca`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_company`
--
ALTER TABLE `tbl_company`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_department`
--
ALTER TABLE `tbl_department`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_employee`
--
ALTER TABLE `tbl_employee`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_employee_tax`
--
ALTER TABLE `tbl_employee_tax`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_job_status`
--
ALTER TABLE `tbl_job_status`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_log`
--
ALTER TABLE `tbl_log`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_login_log`
--
ALTER TABLE `tbl_login_log`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idx_login_employee` (`employee_id`),
  ADD KEY `idx_login_time` (`login_time`);

--
-- Indexes for table `tbl_payrollperiod`
--
ALTER TABLE `tbl_payrollperiod`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_qr`
--
ALTER TABLE `tbl_qr`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_qr_history`
--
ALTER TABLE `tbl_qr_history`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_qr_settings`
--
ALTER TABLE `tbl_qr_settings`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_sss_contribution`
--
ALTER TABLE `tbl_sss_contribution`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `time_adjustments`
--
ALTER TABLE `time_adjustments`
  ADD PRIMARY KEY (`adjustment_id`),
  ADD KEY `approved_by` (`approved_by`),
  ADD KEY `idx_adjustment_employee` (`employee_id`),
  ADD KEY `idx_adjustment_date` (`adjustment_date`);

--
-- Indexes for table `units`
--
ALTER TABLE `units`
  ADD PRIMARY KEY (`id`),
  ADD KEY `category_id` (`category_id`);

--
-- Indexes for table `unit_categories`
--
ALTER TABLE `unit_categories`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `unit_settings`
--
ALTER TABLE `unit_settings`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD KEY `idx_username` (`username`),
  ADD KEY `idx_user_type` (`user_type`),
  ADD KEY `idx_is_active` (`is_active`),
  ADD KEY `department_id` (`department_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `activity_logs`
--
ALTER TABLE `activity_logs`
  MODIFY `log_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `allowance_transactions`
--
ALTER TABLE `allowance_transactions`
  MODIFY `transaction_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `cash_advances`
--
ALTER TABLE `cash_advances`
  MODIFY `advance_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `ca_payments`
--
ALTER TABLE `ca_payments`
  MODIFY `payment_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `company`
--
ALTER TABLE `company`
  MODIFY `CompanyId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `company_settings`
--
ALTER TABLE `company_settings`
  MODIFY `company_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `departments`
--
ALTER TABLE `departments`
  MODIFY `department_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `dtr`
--
ALTER TABLE `dtr`
  MODIFY `dtr_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `dtr_records`
--
ALTER TABLE `dtr_records`
  MODIFY `dtr_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `employee_allowances`
--
ALTER TABLE `employee_allowances`
  MODIFY `allowance_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `employee_deductions`
--
ALTER TABLE `employee_deductions`
  MODIFY `deduction_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `employee_tax`
--
ALTER TABLE `employee_tax`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `job_titles`
--
ALTER TABLE `job_titles`
  MODIFY `job_title_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `payroll`
--
ALTER TABLE `payroll`
  MODIFY `payroll_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `payroll_details`
--
ALTER TABLE `payroll_details`
  MODIFY `payroll_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT for table `payroll_generations`
--
ALTER TABLE `payroll_generations`
  MODIFY `generation_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `payroll_periods`
--
ALTER TABLE `payroll_periods`
  MODIFY `period_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `philhealth_contributions`
--
ALTER TABLE `philhealth_contributions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `philhealth_rates`
--
ALTER TABLE `philhealth_rates`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `sss_contributions`
--
ALTER TABLE `sss_contributions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `sss_contribution_table`
--
ALTER TABLE `sss_contribution_table`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `tax_brackets`
--
ALTER TABLE `tax_brackets`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_ca`
--
ALTER TABLE `tbl_ca`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_company`
--
ALTER TABLE `tbl_company`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_department`
--
ALTER TABLE `tbl_department`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_employee`
--
ALTER TABLE `tbl_employee`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_employee_tax`
--
ALTER TABLE `tbl_employee_tax`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_job_status`
--
ALTER TABLE `tbl_job_status`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_log`
--
ALTER TABLE `tbl_log`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_login_log`
--
ALTER TABLE `tbl_login_log`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_payrollperiod`
--
ALTER TABLE `tbl_payrollperiod`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_qr`
--
ALTER TABLE `tbl_qr`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_qr_history`
--
ALTER TABLE `tbl_qr_history`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_sss_contribution`
--
ALTER TABLE `tbl_sss_contribution`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `time_adjustments`
--
ALTER TABLE `time_adjustments`
  MODIFY `adjustment_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `units`
--
ALTER TABLE `units`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `unit_categories`
--
ALTER TABLE `unit_categories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `activity_logs`
--
ALTER TABLE `activity_logs`
  ADD CONSTRAINT `activity_logs_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `allowance_transactions`
--
ALTER TABLE `allowance_transactions`
  ADD CONSTRAINT `allowance_transactions_ibfk_1` FOREIGN KEY (`allowance_id`) REFERENCES `employee_allowances` (`allowance_id`),
  ADD CONSTRAINT `allowance_transactions_ibfk_2` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`),
  ADD CONSTRAINT `allowance_transactions_ibfk_3` FOREIGN KEY (`period_id`) REFERENCES `payroll_periods` (`period_id`);

--
-- Constraints for table `cash_advances`
--
ALTER TABLE `cash_advances`
  ADD CONSTRAINT `cash_advances_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`),
  ADD CONSTRAINT `cash_advances_ibfk_2` FOREIGN KEY (`approved_by`) REFERENCES `users` (`user_id`),
  ADD CONSTRAINT `cash_advances_ibfk_3` FOREIGN KEY (`deduction_start_period`) REFERENCES `payroll_periods` (`period_id`);

--
-- Constraints for table `dtr`
--
ALTER TABLE `dtr`
  ADD CONSTRAINT `dtr_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`);

--
-- Constraints for table `dtr_records`
--
ALTER TABLE `dtr_records`
  ADD CONSTRAINT `dtr_records_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`);

--
-- Constraints for table `employees`
--
ALTER TABLE `employees`
  ADD CONSTRAINT `employees_ibfk_1` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`),
  ADD CONSTRAINT `employees_ibfk_2` FOREIGN KEY (`job_title_id`) REFERENCES `job_titles` (`job_title_id`),
  ADD CONSTRAINT `employees_ibfk_3` FOREIGN KEY (`supervisor_id`) REFERENCES `employees` (`employee_id`);

--
-- Constraints for table `employee_allowances`
--
ALTER TABLE `employee_allowances`
  ADD CONSTRAINT `employee_allowances_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`);

--
-- Constraints for table `employee_deductions`
--
ALTER TABLE `employee_deductions`
  ADD CONSTRAINT `employee_deductions_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`),
  ADD CONSTRAINT `employee_deductions_ibfk_2` FOREIGN KEY (`period_id`) REFERENCES `payroll_periods` (`period_id`);

--
-- Constraints for table `job_titles`
--
ALTER TABLE `job_titles`
  ADD CONSTRAINT `job_titles_ibfk_1` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`);

--
-- Constraints for table `payroll`
--
ALTER TABLE `payroll`
  ADD CONSTRAINT `payroll_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`),
  ADD CONSTRAINT `payroll_ibfk_2` FOREIGN KEY (`period_id`) REFERENCES `payroll_periods` (`period_id`);

--
-- Constraints for table `payroll_details`
--
ALTER TABLE `payroll_details`
  ADD CONSTRAINT `payroll_details_ibfk_1` FOREIGN KEY (`period_id`) REFERENCES `payroll_periods` (`period_id`),
  ADD CONSTRAINT `payroll_details_ibfk_2` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`),
  ADD CONSTRAINT `payroll_details_ibfk_3` FOREIGN KEY (`calculated_by`) REFERENCES `users` (`user_id`),
  ADD CONSTRAINT `payroll_details_ibfk_4` FOREIGN KEY (`approved_by`) REFERENCES `users` (`user_id`),
  ADD CONSTRAINT `payroll_details_ibfk_5` FOREIGN KEY (`paid_by`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `payroll_generations`
--
ALTER TABLE `payroll_generations`
  ADD CONSTRAINT `payroll_generations_ibfk_1` FOREIGN KEY (`period_id`) REFERENCES `payroll_periods` (`period_id`),
  ADD CONSTRAINT `payroll_generations_ibfk_2` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`);

--
-- Constraints for table `tbl_login_log`
--
ALTER TABLE `tbl_login_log`
  ADD CONSTRAINT `tbl_login_log_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`);

--
-- Constraints for table `time_adjustments`
--
ALTER TABLE `time_adjustments`
  ADD CONSTRAINT `time_adjustments_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`),
  ADD CONSTRAINT `time_adjustments_ibfk_2` FOREIGN KEY (`approved_by`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `units`
--
ALTER TABLE `units`
  ADD CONSTRAINT `units_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `unit_categories` (`id`);

--
-- Constraints for table `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
