-- SQL to add government benefits columns to the employee table
ALTER TABLE employee
ADD COLUMN sss_number VARCHAR(20) NULL COMMENT 'Social Security System Number',
ADD COLUMN pagibig_number VARCHAR(20) NULL COMMENT 'Pag-IBIG Fund Number',
ADD COLUMN philhealth_number VARCHAR(20) NULL COMMENT 'PhilHealth Number',
ADD COLUMN tin_number VARCHAR(20) NULL COMMENT 'Tax Identification Number';

-- Update existing records with empty values if needed
-- UPDATE employee SET sss_number = '', pagibig_number = '', philhealth_number = '', tin_number = '' WHERE sss_number IS NULL;

-- Optional: Create indexes for faster searching
-- CREATE INDEX idx_employee_sss ON employee(sss_number);
-- CREATE INDEX idx_employee_tin ON employee(tin_number);