--create database BranchAndChicken

CREATE TABLE Trainer 
(
	Id int identity(1,1) primary key,
	[Name] nvarchar(255) not null,
	YearsOfExperience int not null default(0),
	Specialty int not null
)

SELECT * FROM Trainer
INSERT INTO Trainer([Name], Specialty, YearsOfExperience)
VALUES	('Nathan', 3, 5),
		('Martin', 1, 0),
		('Adam', 0, 2)