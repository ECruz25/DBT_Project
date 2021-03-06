/*
   Wednesday, September 6, 201711:33:52 PM
   User: 
   Server: DESKTOP-HJRGROH\SERVER337
   Database: DBTheory
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Status
	(
	StatusID int NOT NULL,
	StatusName varchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Status ADD CONSTRAINT
	PK_Status PRIMARY KEY CLUSTERED 
	(
	StatusID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Status SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Department
	(
	DepartmentID int NOT NULL,
	DepartmentName varchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Department ADD CONSTRAINT
	PK_Department PRIMARY KEY CLUSTERED 
	(
	DepartmentID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Department SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Profile
	(
	ProfileID int NOT NULL,
	ProfileName varchar(50) NOT NULL,
	DepartmentID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Profile ADD CONSTRAINT
	PK_Profile PRIMARY KEY CLUSTERED 
	(
	ProfileID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Profile ADD CONSTRAINT
	FK_Profile_Department FOREIGN KEY
	(
	DepartmentID
	) REFERENCES dbo.Department
	(
	DepartmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Profile SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.[User]
	(
	UserID int NOT NULL,
	UserEmail varchar(50) NOT NULL,
	UserPassword varchar(50) NOT NULL,
	UserLastActivity varchar(50) NOT NULL,
	UserName varchar(50) NOT NULL,
	UserBirthday date NOT NULL,
	ProfileID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.[User] ADD CONSTRAINT
	PK_User PRIMARY KEY CLUSTERED 
	(
	UserID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.[User] ADD CONSTRAINT
	FK_User_Profile FOREIGN KEY
	(
	ProfileID
	) REFERENCES dbo.Profile
	(
	ProfileID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.[User] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Urgency
	(
	UrgencyID int NOT NULL,
	UrgencyName varchar(50) NOT NULL,
	UrgencyDescription varbinary(100) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Urgency ADD CONSTRAINT
	PK_Urgency PRIMARY KEY CLUSTERED 
	(
	UrgencyID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Urgency SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Incident
	(
	IncidentID int NOT NULL,
	IncidentTitle varchar(50) NULL,
	IncidentDescription varbinary(140) NULL,
	IncidentCreationDate date NULL,
	StatusID int NULL,
	UrgencyID int NULL,
	UserID int NULL,
	TechnicianID int NULL,
	DepartmentID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Incident ADD CONSTRAINT
	PK_Incident PRIMARY KEY CLUSTERED 
	(
	IncidentID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Incident ADD CONSTRAINT
	FK_Incident_Status FOREIGN KEY
	(
	StatusID
	) REFERENCES dbo.Status
	(
	StatusID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Incident ADD CONSTRAINT
	FK_Incident_Urgency FOREIGN KEY
	(
	UrgencyID
	) REFERENCES dbo.Urgency
	(
	UrgencyID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Incident ADD CONSTRAINT
	FK_Incident_Department FOREIGN KEY
	(
	DepartmentID
	) REFERENCES dbo.Department
	(
	DepartmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Incident ADD CONSTRAINT
	FK_Incident_User FOREIGN KEY
	(
	UserID
	) REFERENCES dbo.[User]
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Incident ADD CONSTRAINT
	FK_Incident_User1 FOREIGN KEY
	(
	TechnicianID
	) REFERENCES dbo.[User]
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Incident SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
