CREATE TABLE Tasks (
	TID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TGID uniqueidentifier NOT NULL,
	TUID INT NOT NULL,
	TName nvarchar(300) NOT NULL,
	TLocalization nvarchar(300) NOT NULL,
	TTime DATE NOT NULL,
	TBudget INT
)