CREATE TABLE adminUsers (
    UserId UNIQUEIDENTIFIER PRIMARY KEY,  -- Primary key of type GUID
    UserNames NVARCHAR(50) NOT NULL,           -- Required username with a maximum length of 50
    UserPassword NVARCHAR(50) NOT NULL          -- Required password with a maximum length of 50
);
