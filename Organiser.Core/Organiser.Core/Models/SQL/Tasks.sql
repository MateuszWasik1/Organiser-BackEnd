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
)