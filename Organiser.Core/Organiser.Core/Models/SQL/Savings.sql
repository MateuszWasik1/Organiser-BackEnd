CREATE TABLE Savings (
	SID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SGID uniqueidentifier NOT NULL,
	SUID INT NOT NULL,
	SAmount decimal NOT NULL,
	STime DATETIME2 NOT NULL,
	SOnWhat nvarchar(300),
	SWhere nvarchar(300),
)	