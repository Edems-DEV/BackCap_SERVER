-- Create and use the database
CREATE DATABASE IF NOT EXISTS `myDb`;
USE `myDb`;


-- User table
DROP TABLE IF EXISTS `User`;
CREATE TABLE `User` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` LONGTEXT NOT NULL,
  `Password` LONGTEXT NOT NULL,
  `Email` LONGTEXT NOT NULL,
  `Interval_Report` LONGTEXT NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Config table
DROP TABLE IF EXISTS `Config`;
CREATE TABLE `Config` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` LONGTEXT DEFAULT NULL,
  `Description` LONGTEXT DEFAULT NULL,
  `Type` SMALLINT(6) NOT NULL,
  `Retention` INT(11) NOT NULL,
  `PackageSize` INT(11) NOT NULL,
  `IsCompressed` TINYINT(1) NOT NULL,
  `Backup_interval` LONGTEXT NOT NULL,
  `Interval_end` DATETIME(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Destination table
DROP TABLE IF EXISTS `Destination`;
CREATE TABLE `Destination` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Id_Config` INT(11) NOT NULL,
  `DestPath` LONGTEXT NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Destination_Id_Config` (`Id_Config`),
  CONSTRAINT `FK_Destination_Config_Id_Config` FOREIGN KEY (`Id_Config`) REFERENCES `Config` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Groups table
DROP TABLE IF EXISTS `Groups`;
CREATE TABLE `Groups` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` LONGTEXT NOT NULL,
  `Description` LONGTEXT DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Machine table
DROP TABLE IF EXISTS `Machine`;
CREATE TABLE `Machine` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` LONGTEXT DEFAULT NULL,
  `Description` LONGTEXT DEFAULT NULL,
  `Os` LONGTEXT DEFAULT NULL,
  `Ip_Address` LONGTEXT DEFAULT NULL,
  `Mac_Address` LONGTEXT DEFAULT NULL,
  `Is_Active` TINYINT(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- MachineGroup table
DROP TABLE IF EXISTS `MachineGroup`;
CREATE TABLE `MachineGroup` (
  `Id_Machine` INT(11) NOT NULL,
  `Id_Group` INT(11) NOT NULL,
  PRIMARY KEY (`Id_Machine`, `Id_Group`),
  KEY `IX_MachineGroup_Id_Group` (`Id_Group`),
  CONSTRAINT `FK_MachineGroup_Groups_Id_Group` FOREIGN KEY (`Id_Group`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_MachineGroup_Machine_Id_Machine` FOREIGN KEY (`Id_Machine`) REFERENCES `Machine` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Sources table
DROP TABLE IF EXISTS `Sources`;
CREATE TABLE `Sources` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Id_Config` INT(11) NOT NULL,
  `Path` LONGTEXT NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Sources_Id_Config` (`Id_Config`),
  CONSTRAINT `FK_Sources_Config_Id_Config` FOREIGN KEY (`Id_Config`) REFERENCES `Config` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Job table
DROP TABLE IF EXISTS `Job`;
CREATE TABLE `Job` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Id_Machine` INT(11) DEFAULT NULL,
  `Id_Group` INT(11) DEFAULT NULL,
  `Id_Config` INT(11) NOT NULL,
  `Status` SMALLINT(6) NOT NULL,
  `Time_schedule` DATETIME(6) NOT NULL,
  `Time_start` DATETIME(6) DEFAULT NULL,
  `Time_end` DATETIME(6) DEFAULT NULL,
  `Bytes` INT(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Job_Id_Config` (`Id_Config`),
  KEY `IX_Job_Id_Group` (`Id_Group`),
  KEY `IX_Job_Id_Machine` (`Id_Machine`),
  CONSTRAINT `FK_Job_Config_Id_Config` FOREIGN KEY (`Id_Config`) REFERENCES `Config` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Job_Groups_Id_Group` FOREIGN KEY (`Id_Group`) REFERENCES `Groups` (`Id`),
  CONSTRAINT `FK_Job_Machine_Id_Machine` FOREIGN KEY (`Id_Machine`) REFERENCES `Machine` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Log table
DROP TABLE IF EXISTS `Log`;
CREATE TABLE `Log` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Id_Job` INT(11) NOT NULL,
  `Message` LONGTEXT NOT NULL,
  `Time` DATETIME(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Log_Id_Job` (`Id_Job`),
  CONSTRAINT `FK_Log_Job_Id_Job` FOREIGN KEY (`Id_Job`) REFERENCES `Job` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


----------------------------------------------------------------
--------------- Insert data into tables
----------------------------------------------------------------


-- Use the database
USE `myDb`;

-- Insert data into User table
INSERT INTO `User` (`Name`, `Password`, `Email`, `Interval_Report`)
VALUES
('admin', 'admin', 'admin@admin.com', '0 0 * * *'),
('Alice Johnson', 'password123', 'alice.johnson@example.com', '0 0 * * *'),
('Bob Smith', 'securepass456', 'bob.smith@example.com', '0 12 * * 1'),
('Charlie Brown', 'mypassword789', 'charlie.brown@example.com', '0 18 * * 5'),
('Alan Turing', 'enigma123', 'alan.turing@example.com', '0 12 * * 2'),
('Emma Wilson', 'emmaPass567', 'emma.wilson@example.com', '0 8 * * 2'),
('Linus Torvalds', 'linuxRocks!', 'linus.torvalds@example.com', '0 3 * * *'),
('Satoshi Nakamoto', 'bitcoin4life', 'satoshi.nakamoto@example.com', '0 4 * * *'),
('Ross Ulbricht', 'silkroad420', 'ross.ulbricht@example.com', '0 11 * * 1'),
('Jack Davis', 'jackDavis999', 'jack.davis@example.com', '0 16 * * 1'),
('Edward Snowden', 'nsaWatcher', 'edward.snowden@example.com', '0 8 * * 2'),
('John McAfee', 'runningFromTheIRS', 'john.mcafee@example.com', '0 21 * * 6'),
('Henry Adams', 'henrySecure!', 'henry.adams@example.com', '0 22 * * 6'),
('Mia Scott', 'MiaS!pass', 'mia.scott@example.com', '0 19 * * 5'),
('Alice Devlin', 'alicePass123', 'alice.devlin@example.com', '0 6 * * *'),
('Bob Reynolds', 'secureBob456', 'bob.reynolds@example.com', '0 12 * * 1'),
('Charlie Evans', 'charlieCode789', 'charlie.evans@example.com', '0 18 * * 5'),
('David Sinclair', 'davidSecure!', 'david.sinclair@example.com', '0 4 * * 0'),
('Emma Brooks', 'emma@dev', 'emma.brooks@example.com', '0 9 * * 2');

-- Insert data into Config table
INSERT INTO `Config` (`Name`, `Description`, `Type`, `Retention`, `PackageSize`, `IsCompressed`, `Backup_interval`, `Interval_end`)
VALUES
('W_Docs_Daily', 'Backup documents folder on Windows daily', 2, 30, 1024, 1, '0 2 * * *', '2024-12-31 23:59:59'),
('W_Apps_Weekly', 'Backup installed applications on Windows weekly', 2, 90, 2048, 1, '0 3 * * 1', '2024-12-31 23:59:59'),
('W_Videos_Monthly', 'Backup videos folder on Windows monthly', 2, 180, 4096, 1, '0 4 1 * *', '2024-12-31 23:59:59'),
('L_Config_Weekly', 'Backup system configurations on Linux weekly', 3, 30, 1024, 1, '0 2 * * 0', '2024-12-31 23:59:59'),
('L_Home_Daily', 'Backup home directories on Linux daily', 3, 30, 1024, 1, '0 2 * * *', '2024-12-31 23:59:59'),
('L_Logs_Monthly', 'Backup system logs on Linux monthly', 3, 180, 4096, 1, '0 4 1 * *', '2024-12-31 23:59:59'),
('Server_Data_Daily', 'Backup critical server data daily', 1, 30, 2048, 1, '0 2 * * *', '2024-12-31 23:59:59'),
('Server_DB_Weekly', 'Backup databases on the server weekly', 1, 90, 4096, 1, '0 3 * * 1', '2024-12-31 23:59:59');

-- Insert corresponding Source paths for these configurations
INSERT INTO `Sources` (`Id_Config`, `Path`)
VALUES
(1, 'C:\\Users\\Alice\\Documents'),
(2, 'C:\\Program Files'),
(3, 'C:\\Videos'),
(4, '/etc/'),
(5, '/home/'),
(6, '/var/log/'),
(7, '/mnt/server_data/'),
(8, '/mnt/db_backups/');

-- Insert corresponding Destination paths for these configurations
INSERT INTO `Destination` (`Id_Config`, `DestPath`)
VALUES
(1, '/mnt/backup/windows_docs'),
(2, '/mnt/backup/windows_apps'),
(3, '/mnt/backup/windows_videos'),
(4, '/mnt/backup/linux_config'),
(5, '/mnt/backup/linux_home'),
(6, '/mnt/backup/linux_logs'),
(7, '/mnt/backup/server_data'),
(8, '/mnt/backup/db_backups');

-- Insert data into Machine table
INSERT INTO `Machine` (`Name`, `Description`, `Os`, `Ip_Address`, `Mac_Address`, `Is_Active`)
VALUES
('Server01', 'Main server', 'Linux', '192.168.1.10', '00:14:22:01:23:45', 1),
('Server02', 'Proxy server', 'Linux', '192.168.1.12', '00:14:22:01:23:47', 1),
('Server03', 'Web server', 'Linux', '192.168.1.13', '00:14:22:01:23:48', 1),
('WS_01_Windows', 'Employee workstation', 'Windows 10', '192.168.1.14', '00:14:22:01:23:49', 1),
('WS_02_Windows', 'Employee workstation', 'Windows 11', '192.168.1.15', '00:14:22:01:23:50', 0),
('WS_03_Windows', 'Employee workstation', 'Windows 10', '192.168.1.16', '00:14:22:01:23:51', 1),
('WS_04_Linux', 'Employee workstation', 'Linux', '192.168.1.17', '00:14:22:01:23:52', 1),
('WS_05_Linux', 'Employee workstation', 'Linux', '192.168.1.18', '00:14:22:01:23:53', 1),
('WS_06_Linux', 'Employee workstation', 'Linux', '192.168.1.19', '00:14:22:01:23:54', 0);

-- Insert data into Groups table
INSERT INTO `Groups` (`Name`, `Description`)
VALUES
('Servers', 'Servers with critical data'),
('WS_Windows', 'Employee Windows workstation'),
('WS_Linux', 'Employee Linux workstation');

-- Insert data into MachineGroup table with correct associations
INSERT INTO `MachineGroup` (`Id_Machine`, `Id_Group`)
VALUES
(1, 1),
(2, 1),
(3, 1),
(4, 2),
(5, 2),
(6, 2),
(7, 3),
(8, 3),
(9, 3);

-- Insert data into Job table (Success, Running, and Waiting statuses)
INSERT INTO `Job` (`Id_Machine`, `Id_Group`, `Id_Config`, `Status`, `Time_schedule`, `Time_start`, `Time_end`, `Bytes`)
VALUES
(1, 1, 1, 2, '2024-07-27 02:00:00', '2024-07-27 02:05:00', '2024-07-27 02:10:00', 1048576),
(2, 2, 2, 1, '2024-07-27 03:00:00', '2024-07-27 03:05:00', NULL, 2097152),
(1, 3, 3, 0, '2024-07-27 04:00:00', '2024-07-27 04:05:00', NULL, 4194304);

-- Insert Log messages for these jobs
INSERT INTO `Log` (`Id_Job`, `Message`, `Time`)
VALUES
(1, 'Backup started', '2024-07-27 02:10:00'),
(1, 'Backup finished successfully!', '2024-07-27 03:15:00'),
(1, 'Backup started', '2024-07-27 04:20:00'),
(1, 'Backup finished successfully!', '2024-07-28 02:10:00'),
(1, 'Backup encountered errors!', '2024-07-28 03:20:00'),
(1, 'Backup started', '2024-07-28 04:25:00');
