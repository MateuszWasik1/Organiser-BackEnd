﻿CREATE TABLE Users (
	UID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	UGID uniqueidentifier NOT NULL,
	URID INT NOT NULL,
	UFirstName nvarchar(50) NOT NULL,
	ULastName nvarchar(50) NOT NULL,
	UUserName nvarchar(100) NOT NULL,
	UEmail nvarchar(100) NOT NULL,
	UPhone nvarchar(100) NOT NULL,
	UPassword nvarchar(max) NOT NULL,
);

CREATE TABLE AppRoles (
	RID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	RGID uniqueidentifier NOT NULL,
	RName nvarchar(100) NOT NULL,
)

INSERT INTO AppRoles (RGID, RName) VALUES ('1A29FC40-CA47-1067-B31D-00DD0106621A', 'User'), ('2B29FC40-CA47-1067-B31D-00DD0106622B', 'Support'), ('3C29FC40-CA47-1067-B31D-00DD0106623C', 'Admin'); 

CREATE TABLE Categories (
	CID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CGID uniqueidentifier NOT NULL,
	CUID INT NOT NULL,
	CName nvarchar(300) NOT NULL,
	CStartDate DATETIME2 NOT NULL,
	CEndDate DATETIME2 NOT NULL,
	CBudget INT
);

CREATE TABLE Tasks (
	TID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TGID uniqueidentifier NOT NULL,
	TCGID uniqueidentifier NOT NULL,
	TUID INT NOT NULL,
	TName nvarchar(300) NOT NULL,
	TLocalization nvarchar(300) NOT NULL,
	TTime DATETIME2 NOT NULL,
	TBudget INT,
	TStatus int NOT NULL
);

CREATE TABLE TasksNotes (
	TNID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TNGID uniqueidentifier NOT NULL,
	TNTGID uniqueidentifier NOT NULL,
	TNUID INT NOT NULL,
	TNNote nvarchar(2000) NOT NULL,
	TNDate DATETIME2 NOT NULL,
	TNEditDate DATETIME2 NULL,
);

CREATE TABLE Savings (
	SID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SGID uniqueidentifier NOT NULL,
	SUID INT NOT NULL,
	SAmount decimal NOT NULL,
	STime DATETIME2 NOT NULL,
	SOnWhat nvarchar(300),
	SWhere nvarchar(300),
);