CREATE TABLE batch
(
 batchid INT IDENTITY NOT NULL PRIMARY KEY,
 batchname  NVARCHAR(50) not null,
 startdate DATE NOT NULL,
 enddate DATE NOT NULL,
 sitavailable BIT
)
GO
CREATE TABLE students
(
 studentid INT IDENTITY NOT NULL PRIMARY KEY,
 studentname NVARCHAR(60) NOT NULL,
 tuitionfee NVARCHAR(60) NOT NULL,
 phone NVARCHAR(60) NOT NULL,
 picture NVARCHAR(50) NOT NULL,
 batchid INT NOT NULL REFERENCES batch ( batchid)

)
go