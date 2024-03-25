CREATE TABLE TasksSubTasks (
	TSTID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TSTGID uniqueidentifier NOT NULL,
	TSTTGID uniqueidentifier NOT NULL,
	TSTUID INT NOT NULL,
	TSTTitle nvarchar(200) NOT NULL,
	TSTNext nvarchar(2000) NOT NULL,
	TSTCreationDate DATETIME2 NOT NULL,
	TSTModifyDate DATETIME2 NOT NULL,
	TSTStatus int NOT NULL,
)