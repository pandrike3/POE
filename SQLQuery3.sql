CREATE TABLE [dbo].[Lecturer] (
    [LecturerId]     INT IDENTITY(1,1) PRIMARY KEY,
    [Name]           NVARCHAR(100) NOT NULL,
    [Email]          NVARCHAR(100) NOT NULL,
    [ContactNumber]  NVARCHAR(20)  NULL,
    [Address]        NVARCHAR(200) NULL
);