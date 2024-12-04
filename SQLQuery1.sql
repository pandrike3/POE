

CREATE TABLE LecturerClaims (
    Id INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing in SQL Server
    LecturerName NVARCHAR(255) NOT NULL,
    HoursWorked DECIMAL(5, 2) NOT NULL,
    HourlyRate DECIMAL(7, 2) NOT NULL,
    Notes NVARCHAR(500), -- Optional, max length of 500
    SubmissionDate DATETIME NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending', -- Default status is 'Pending'
    DocumentPath NVARCHAR(255) -- Nullable, max length of 255
);
