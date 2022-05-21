USE master
GO
CREATE DATABASE Quiz  
ON   
( NAME = Quiz_Dat,  
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS01\MSSQL\DATA\Quiz_Dat.mdf',  
    SIZE = 10,  
    MAXSIZE = 50,  
    FILEGROWTH = 5 )  
LOG ON  
( NAME = Quiz_Log,  
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS01\MSSQL\DATA\Quiz_Log.ldf',  
    SIZE = 5MB,  
    MAXSIZE = 25MB,  
    FILEGROWTH = 5MB ) ;  
GO  
USE Quiz
GO
CREATE TABLE Theory(
ID INT PRIMARY KEY NOT NULL IDENTITY (1, 1),
Theory_data NTEXT NOT NULL
)
GO
CREATE TABLE Images(
ID INT PRIMARY KEY NOT NULL IDENTITY (1, 1),
Image_data IMAGE NOT NULL
)
GO
CREATE TABLE Tests(
Test_code INT PRIMARY KEY NOT NULL IDENTITY(1,1),
Test_title VARCHAR(50) NOT NULL UNIQUE,
Test_GUID UNIQUEIDENTIFIER  NOT NULL,
Theory_code INT,
Theory_flag BIT NOT NULL,
Theory_Source VARCHAR(MAX),
Test_time TIME NOT NULL,
Questions_amount INT NOT NULL,
Amount_for_pass INT NOT NULL
constraint FK_Tests_Theory
Foreign key(Theory_code) references Theory(Theory_code)
)

alter table Tests
add constraint FK_Tests_Image
Foreign key(Image_code) references Images(ID)

GO
CREATE TABLE Questions(
Question_code INT PRIMARY KEY NOT NULL IDENTITY(1,1),
Test_code INT NOT NULL,
Question_text NVARCHAR(4000) NOT NULL
constraint FK_Tests_Questions
Foreign key(Test_code) references Tests(Test_code)
)
GO
CREATE TABLE Answers(
Answer_code INT PRIMARY KEY NOT NULL IDENTITY(1,1),
Question_code INT NOT NULL,
Answer_text NVARCHAR(4000) NOT NULL,
Answer_flag BIT NOT NULL,
constraint FK_Answers_Questions
Foreign key(Question_code) references Questions(Question_code)
)