CREATE TABLE LecturerClaims (
    Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    LecturerName NVARCHAR(100) NOT NULL,
    HoursWorked DECIMAL(5, 2) NOT NULL,
    HourlyRate DECIMAL(7, 2) NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    Notes NVARCHAR(500) NULL,
    SubmissionDate DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    DocumentPath NVARCHAR(255) NULL
);

