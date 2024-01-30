CREATE TABLE Roles (
	RID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	RGID uniqueidentifier NOT NULL,
	RName nvarchar(100) NOT NULL,
)

INSERT INTO ROLES (RGID, RName) VALUES ('1A29FC40-CA47-1067-B31D-00DD0106621A', 'User'), ('2B29FC40-CA47-1067-B31D-00DD0106622B', 'Support'), ('3C29FC40-CA47-1067-B31D-00DD0106623C', 'Admin'); 