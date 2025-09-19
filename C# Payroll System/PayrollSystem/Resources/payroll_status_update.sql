-- Add status constraints to payroll_details table
ALTER TABLE payroll_details
MODIFY COLUMN status ENUM('Draft', 'Calculated', 'Approved', 'Paid') NOT NULL DEFAULT 'Draft';

-- Add approval tracking columns
ALTER TABLE payroll_details
ADD COLUMN calculated_by INT NULL REFERENCES users(user_id),
ADD COLUMN calculated_date DATETIME NULL,
ADD COLUMN approved_by INT NULL REFERENCES users(user_id),
ADD COLUMN approved_date DATETIME NULL,
ADD COLUMN paid_by INT NULL REFERENCES users(user_id),
ADD COLUMN paid_date DATETIME NULL,
ADD COLUMN remarks TEXT NULL;

-- Add indexes for better performance
CREATE INDEX idx_payroll_status ON payroll_details(status);
CREATE INDEX idx_payroll_calculated ON payroll_details(calculated_by, calculated_date);
CREATE INDEX idx_payroll_approved ON payroll_details(approved_by, approved_date);
CREATE INDEX idx_payroll_paid ON payroll_details(paid_by, paid_date);