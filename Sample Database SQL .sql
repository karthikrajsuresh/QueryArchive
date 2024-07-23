-- Create Database
CREATE DATABASE QueryArchive;
GO
USE QueryArchive;
GO

-- Create Topics Table
CREATE TABLE Topics (
    TopicID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    QuestionsCount INT DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Create Questions Table
CREATE TABLE Questions (
    QuestionID INT PRIMARY KEY IDENTITY(1,1),
    TopicID INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Attachment Text,
    AnswersCount INT DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TopicID) REFERENCES Topics(TopicID)
);

-- Create Answers Table
CREATE TABLE Answers (
    AnswerID INT PRIMARY KEY IDENTITY(1,1),
    QuestionID INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Attachment Text,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID)
);

-- Create simple indexes for foreign keys
CREATE INDEX TopicIndex ON Questions(TopicID);
CREATE INDEX QuestionIndex ON Answers(QuestionID);
GO
-- Trigger to update UpdatedDate on update for Topics
CREATE OR ALTER TRIGGER tr_UpdatedDate_Topics ON Topics
AFTER UPDATE AS
BEGIN
    BEGIN TRY
        UPDATE Topics
        SET UpdatedDate = CURRENT_TIMESTAMP
        WHERE TopicID IN (SELECT DISTINCT TopicID FROM inserted);
    END TRY
    BEGIN CATCH
        PRINT 'An error occurred while updating Topics: ' + ERROR_MESSAGE();
    END CATCH
END;
GO
-- Trigger to update UpdatedDate on update for Questions
CREATE OR ALTER TRIGGER tr_UpdatedDate_Questions ON Questions
AFTER UPDATE AS
BEGIN
    BEGIN TRY
        UPDATE Questions
        SET UpdatedDate = CURRENT_TIMESTAMP
        WHERE QuestionID IN (SELECT DISTINCT QuestionID FROM inserted);
    END TRY
    BEGIN CATCH
        PRINT 'An error occurred while updating Questions: ' + ERROR_MESSAGE();
    END CATCH
END;
GO
-- Trigger to update UpdatedDate on update for Answers
CREATE OR ALTER TRIGGER tr_UpdatedDate_Answers ON Answers
AFTER UPDATE AS
BEGIN
    BEGIN TRY
        UPDATE Answers
        SET UpdatedDate = CURRENT_TIMESTAMP
        WHERE AnswerID IN (SELECT DISTINCT AnswerID FROM inserted);
    END TRY
    BEGIN CATCH
        PRINT 'An error occurred while updating Answers: ' + ERROR_MESSAGE();
    END CATCH
END;
GO
-- Insert Sample Topics
INSERT INTO Topics (Name) VALUES ('Programming');
INSERT INTO Topics (Name) VALUES ('Database');
INSERT INTO Topics (Name) VALUES ('Web Development');

-- Insert Sample Questions
INSERT INTO Questions (TopicID, Title, Content, Attachment) VALUES 
(1, 'What is polymorphism in OOP?', 'Can someone explain the concept of polymorphism with examples?', NULL),
(2, 'How to optimize SQL queries?', 'I need tips on optimizing SQL queries for better performance.', NULL),
(3, 'What is the best way to learn React?', 'I am new to web development and want to learn React. Any suggestions?', NULL);

-- Insert Sample Answers
INSERT INTO Answers (QuestionID, Content, Attachment) VALUES 
(1, 'Polymorphism is the ability of an object to take on many forms. It is one of the core concepts of OOP.', NULL),
(2, 'To optimize SQL queries, you can use indexes, avoid unnecessary columns in SELECT, and optimize joins.', NULL),
(3, 'The best way to learn React is to start with the official documentation and build small projects.', NULL);

-- Query Examples
-- Retrieve All The Topics ordered by UpdatedDate
SELECT * FROM Topics ORDER BY UpdatedDate DESC;

-- Retrieve All Topics
SELECT * FROM Topics;

-- Retrieve Questions by Topic   
SELECT * FROM Questions WHERE TopicID = 1;

-- Retrieve Answers by Question
SELECT * FROM Answers WHERE QuestionID = 1;