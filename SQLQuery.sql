drop table FormTable;
drop table QuestionTable;
drop table AnswerTable;
drop table ResponseTable;

create table FormTable(
	formId int identity(1,1) primary key,
	formName varchar(100),
	createdDate datetime default getdate()
);

create table QuestionTable(
	questionId int identity(1,1) primary key,
	questionDesc nvarchar(max),
	questionType varchar(10),
	formId int foreign key references FormTable(formId),
);

create table AnswerTable(
	answerId int identity(1,1) primary key,
	questionId int foreign key references QuestionTable(questionId),
	answerOption int,
	answerDesc nvarchar(max),
);

create table ResponseTable(
	responseId int identity(1,1) primary key,
	formId int foreign key references FormTable(formId),
	questionId int foreign key references QuestionTable(questionId),
	answerId int foreign key references AnswerTable(answerId),
);



INSERT INTO FormTable (formName) VALUES ('Customer Feedback Form');
INSERT INTO FormTable (formName) VALUES ('Employee Satisfaction Survey');

-- Questions for Customer Feedback Form
INSERT INTO QuestionTable (questionDesc, questionType, formId) VALUES ('How would you rate our service?', 'radio', 1);
INSERT INTO QuestionTable (questionDesc, questionType, formId) VALUES ('What features do you like?', 'checkbox', 1);
INSERT INTO QuestionTable (questionDesc, questionType, formId) VALUES ('Please select your favorite product:', 'dropdown', 1);
INSERT INTO QuestionTable (questionDesc, questionType, formId) VALUES ('Any additional comments?', 'text', 1);

-- Questions for Employee Satisfaction Survey
INSERT INTO QuestionTable (questionDesc, questionType, formId) VALUES ('Are you satisfied with your job?', 'radio', 2);
INSERT INTO QuestionTable (questionDesc, questionType, formId) VALUES ('What benefits do you value the most?', 'checkbox', 2);
INSERT INTO QuestionTable (questionDesc, questionType, formId) VALUES ('How would you rate the company culture?', 'dropdown', 2);
INSERT INTO QuestionTable (questionDesc, questionType, formId) VALUES ('Please provide any suggestions for improvement.', 'text', 2);

-- Answers for Customer Feedback Form
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (1, 1, 'Excellent');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (1, 2, 'Good');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (1, 3, 'Average');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (2, 1, 'Quality');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (2, 2, 'Price');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (3, 1, 'Product A');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (3, 2, 'Product B');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (3, 3, 'Product C');

-- Answers for Employee Satisfaction Survey
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (5, 1, 'Very Satisfied');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (5, 2, 'Satisfied');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (6, 1, 'Health Insurance');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (6, 2, 'Retirement Plan');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (7, 1, 'Excellent');
INSERT INTO AnswerTable (questionId, answerOption, answerDesc) VALUES (7, 2, 'Good');


-- Responses for Customer Feedback Form
INSERT INTO ResponseTable (formId, questionId, answerId) VALUES (1, 1, 1); -- Excellent service
INSERT INTO ResponseTable (formId, questionId, answerId) VALUES (1, 2, 1); -- Quality
INSERT INTO ResponseTable (formId, questionId, answerId) VALUES (1, 3, 2); -- Product B
INSERT INTO ResponseTable (formId, questionId, answerId) VALUES (1, 4, NULL); -- Additional comments: "Great experience!"

-- Responses for Employee Satisfaction Survey
INSERT INTO ResponseTable (formId, questionId, answerId) VALUES (2, 5, 2); -- Satisfied
INSERT INTO ResponseTable (formId, questionId, answerId) VALUES (2, 6, 1); -- Health Insurance
INSERT INTO ResponseTable (formId, questionId, answerId) VALUES (2, 7, 1); -- Excellent culture
INSERT INTO ResponseTable (formId, questionId, answerId) VALUES (2, 8, NULL); -- Suggestions: "More team-building activities."

drop procedure CopyFormIntoTable

CREATE PROCEDURE CopyFormIntoTable
    @formId INT,
    @newFormId INT OUTPUT
AS
BEGIN
    DECLARE @formName VARCHAR(100);
    DECLARE @oldQuestionId INT;
    DECLARE @newQuestionId INT;
    DECLARE @questionDesc NVARCHAR(MAX);
    DECLARE @questionType VARCHAR(10);

    -- Get the form name to copy
    SELECT @formName = formName
    FROM FormTable
    WHERE formId = @formId;

    -- Create a copy of the form
    INSERT INTO FormTable (formName, createdDate)
    VALUES ('Copy of ' + @formName, GETDATE());

    -- Get the new form ID
    SET @newFormId = SCOPE_IDENTITY();

    -- Declare a cursor for iterating through the questions
    DECLARE QuestionCursor CURSOR FOR
    SELECT questionId, questionDesc, questionType
    FROM QuestionTable
    WHERE formId = @formId;

    OPEN QuestionCursor;

    -- Fetch the first question from the cursor
    FETCH NEXT FROM QuestionCursor INTO @oldQuestionId, @questionDesc, @questionType;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Insert the question into the new form
        INSERT INTO QuestionTable (questionDesc, questionType, formId)
        VALUES (@questionDesc, @questionType, @newFormId);

        -- Get the new question ID
        SET @newQuestionId = SCOPE_IDENTITY();

        -- Copy the answers for the current question
        INSERT INTO AnswerTable (questionId, answerOption, answerDesc)
        SELECT @newQuestionId, answerOption, answerDesc
        FROM AnswerTable
        WHERE questionId = @oldQuestionId;

        -- Fetch the next question
        FETCH NEXT FROM QuestionCursor INTO @oldQuestionId, @questionDesc, @questionType;
    END;

    -- Close and deallocate the cursor
    CLOSE QuestionCursor;
    DEALLOCATE QuestionCursor;
END;



DECLARE @newFormId INT;

EXEC CopyFormIntoTable @formId = 1, @newFormId = @newFormId OUTPUT;

SELECT @newFormId AS NewFormId;


select * from FormTable;
select * from QuestionTable;
select * from AnswerTable;
select * from AnswerTable;

SELECT f.formId, f.formName, f.createdDate, q.questionId, q.questionDesc, q.questionType, a.answerId, a.answerOption, a.answerDesc
FROM FormTable f
INNER JOIN QuestionTable q ON f.formId = q.formId
Left JOIN AnswerTable a ON q.questionId = a.questionId
WHERE f.formId = 14;

