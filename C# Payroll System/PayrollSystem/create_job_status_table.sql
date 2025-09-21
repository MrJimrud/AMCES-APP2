-- Create job status table
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

-- Insert default job statuses
INSERT INTO `tbl_job_status` (`status_name`, `description`, `is_active`, `created_date`) VALUES
('Full-Time', 'Regular full-time employee working standard hours', 1, NOW()),
('Part-Time', 'Employee working less than standard full-time hours', 1, NOW()),
('Contract', 'Employee hired for a specific period or project', 1, NOW()),
('Probationary', 'Employee under evaluation period before regular employment', 1, NOW()),
('Temporary', 'Short-term employee for seasonal or temporary needs', 1, NOW()),
('Intern', 'Student or trainee working to gain experience', 1, NOW()),
('Consultant', 'External professional providing specialized services', 1, NOW())
ON DUPLICATE KEY UPDATE
  `modified_date` = NOW();