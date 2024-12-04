-- Table: LecturerClaims
CREATE TABLE LecturerClaims (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    HoursWorked DECIMAL(5,2) NOT NULL,
    HourlyRate DECIMAL(7,2) NOT NULL,
    Notes NVARCHAR(500),
    SubmissionDate DATETIME NOT NULL,
    DocumentPath NVARCHAR(255)
);

-- Table: AdminUsers
CREATE TABLE AdminUsers (
    UserId UNIQUEIDENTIFIER PRIMARY KEY,
    UserNames NVARCHAR(50) NOT NULL,
    UserPassword NVARCHAR(50) NOT NULL
);

-- Table: Lecturers
CREATE TABLE Lecturers (
    LecturerId UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    ContactNumber NVARCHAR(20),
    Address NVARCHAR(200)
);
