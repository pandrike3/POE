CREATE TABLE [dbo].[LecturerClaim] (
    [Id]             INT IDENTITY(1,1) PRIMARY KEY,
    [LecturerName]   NVARCHAR(100) NOT NULL,
    [HoursWorked]    DECIMAL(18, 2) NOT NULL,
    [HourlyRate]     DECIMAL(18, 2) NOT NULL,
    [Notes]          NVARCHAR(500) NULL,
    [TotalAmount]    DECIMAL(18, 2) NOT NULL,
    [SubmissionDate] DATETIME NOT NULL,
    [Status]         NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    [DocumentPath]   NVARCHAR(255) NULL,

   
);
