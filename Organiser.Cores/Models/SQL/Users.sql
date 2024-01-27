CREATE TABLE Users (
	UID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	UGID uniqueidentifier NOT NULL,
	URID INT NOT NULL,
	UFirstName nvarchar(50) NOT NULL,
	ULastName nvarchar(50) NOT NULL,
	UUserName nvarchar(100) NOT NULL,
	UEmail nvarchar(100) NOT NULL,
	UPhone nvarchar(100) NOT NULL,
	UPassword nvarchar(max) NOT NULL,
)