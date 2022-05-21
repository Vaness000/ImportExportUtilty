USE Quiz
GO
CREATE PROC ExistsTest(@guid UNIQUEIDENTIFIER)
AS
IF EXISTS(SELECT * FROM Tests WHERE Test_GUID = @guid)
SELECT 1
ELSE
SELECT 0
GO
CREATE PROC GetTest(@guid UNIQUEIDENTIFIER)
AS
SELECT Test_code, Test_title, Test_GUID, Theory.Theory_data as Theory, Theory_flag, Theory_Source, Test_time, Questions_amount, Amount_for_pass
FROM Tests INNER JOIN Theory ON Tests.Theory_code = Theory.Theory_code
WHERE Test_GUID = @guid
GO
CREATE PROC GetTestByTitle(@title VARCHAR(50))
AS
SELECT Test_code, Test_title, Test_GUID, Theory.Theory_data as Theory, Theory_flag, Theory_Source, Test_time, Questions_amount, Amount_for_pass
FROM Tests INNER JOIN Theory ON Tests.Theory_code = Theory.Theory_code
WHERE Test_title = @title
GO
CREATE PROC GetAllQuestions(@test_code INT)
AS
SELECT Question_code, Question_text
FROM Questions
WHERE Test_code = @test_code
GO
CREATE PROC GetAllAnswers(@question INT)--один для всех
AS
SELECT Answer_text, Answer_flag
FROM Answers
WHERE Question_code = @question
GO
CREATE PROC AddTest(@title VARCHAR(50), @guid UNIQUEIDENTIFIER, @theory NTEXT, @theory_flag BIT, @theory_source VARCHAR(MAX),
					@test_time TIME, @question_amount INT, @amount_for_pass INT, @image IMAGE)
AS
INSERT INTO Images
VALUES
(@)
INSERT INTO Theory
VALUES
(@theory);
DECLARE @theory_code INT
SET @theory_code = (SELECT MAX(Theory_code) FROM Theory);
INSERT INTO Tests
VALUES
(@title, @guid, @theory_code, @theory_flag, @theory_source, @test_time, @question_amount, @amount_for_pass);
GO
CREATE PROC AddQuestion(@test_code INT, @text NVARCHAR(4000))
AS
INSERT INTO Questions
VALUES
(@test_code, @text)
SELECT 
GO
CREATE PROC AddAnswer(@question INT, @text NVARCHAR(4000), @flag BIT)
AS
INSERT INTO Answers
VALUES
(@question, @text, @flag)
GO
CREATE PROC GetTestId(@guid UNIQUEIDENTIFIER)
AS
SELECT Test_code
FROM Tests
WHERE Test_GUID = @guid
GO
CREATE PROC GetLastQuestion
AS
SELECT MAX(Question_code)
FROM Questions
GO
CREATE PROC DeleteTest(@guid UNIQUEIDENTIFIER)
AS
DECLARE @image INT
SET @image = (SELECT Image_code FROM Tests where Test_GUID = @guid)
DECLARE @theory INT
SET @theory = (SELECT Theory_code FROM Tests where Test_GUID = @guid)
DECLARE @code INT
SET @code = (SELECT Test_code  FROM Tests where Test_GUID = @guid)
DELETE FROM Answers
WHERE Question_code IN (SELECT Question_code FROM Questions WHERE Test_code = @code)
DELETE FROM Questions
WHERE Test_code = @code
DELETE FROM Tests
WHERE Test_code = @code
IF @image IS NOT NULL
DELETE FROM Images
WHERE ID = @image
IF @theory IS NOT NULL
DELETE FROM Theory
WHERE ID = @theory
GO
CREATE TRIGGER OnInsertTest
ON Tests
INSTEAD OF INSERT
AS
DECLARE @guid UNIQUEIDENTIFIER
SET @guid = (SELECT Test_GUID FROM inserted)
DECLARE @title VARCHAR(50)
SET @title = (SELECT Test_title FROM inserted)
IF EXISTS (SELECT * FROM Tests WHERE Test_GUID = @guid OR Test_title = @title)
ROLLBACK TRAN
ELSE
INSERT INTO Tests
SELECT Test_title, Test_GUID, Theory_code, Theory_flag, Theory_Source, Test_time, Questions_amount, Amount_for_pass
FROM inserted
go
select Test_code, Test_title, Test_GUID, Theory_flag, Theory_Source, Test_time, Questions_amount, Amount_for_pass, Image_data, Theory_data
from Tests full join Images on Tests.Image_Code = Images.Image_code full join Theory on Theory.Theory_code = Tests.Theory_code
where Test_GUID = '56FFC0BF-13A4-4821-9DF3-9418A4740B01'
select Question_code, Question_text
from Questions
where Test_code = 1
select Answer_text, Answer_flag, Question_code
from Answers
where Question_code between 1 and 5
go

INSERT INTO Theory(Theory_code, Theory_data)
VALUES
(2, 'FDFDF')
go
INSERT INTO Theory
OUTPUT INSERTED.ID
VALUES
(3, 'FFF')
go
INSERT INTO Images
OUTPUT inserted.ID
VALUES
(2, 'FFF')
go
UPDATE Tests
SET Theory_code = 1,
Image_Code = 1
WHERE Test_code = 1
GO
INSERT INTO Theory OUTPUT INSERTED.ID VALUES ('fff')
delete from Theory


EXEC DeleteTest '56FFC0BF-13A4-4821-9DF3-9418A4740B01'
EXEC DeleteTest '56FFC0BF-13A4-4821-9DF3-9418A4740B02'
EXEC DeleteTest '56FFC0BF-13A4-4821-9DF3-9418A4740B03'

SELECT Answer_text, Answer_flag, Answers.Question_code
FROM Answers  inner join Questions on Questions.Question_code = Answers.Question_code
WHERE Questions.Test_code = 1