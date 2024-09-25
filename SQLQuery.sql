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


select * from FormTable;