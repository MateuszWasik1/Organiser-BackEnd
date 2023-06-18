CREATE TABLE Categories (
	CID INT NOT NULL,
	CGID uniqueidentifier NOT NULL,
	CUID INT NOT NULL,
	CName nvarchar(300) NOT NULL,
	CStartDate DATE NOT NULL,
	CEndDate DATE NOT NULL,
	CBudget INT
)