INSERT INTO Skill(Id,Name) VALUES (1,'SQL');
INSERT INTO Skill (Id,Name) VALUES (2,'JavaScript');
INSERT INTO Skill (Id,Name) VALUES (3,'C#');
INSERT INTO Skill (Id,Name) VALUES (4,'Java');
INSERT INTO Skill (Id,Name) VALUES (5,'Python');

INSERT INTO Candidate (Id,FirstName,LastName,EnteredDate) VALUES (1,'Jon','Snow','2000-01-01 00:00:00');
INSERT INTO Candidate (Id,FirstName,LastName,EnteredDate) VALUES (2,'Daenerys','Tangaryen','2000-01-01 00:00:00');
INSERT INTO Candidate (Id,FirstName,LastName,EnteredDate) VALUES (3,'Cersei','Lannister','2000-01-01 00:00:00');
INSERT INTO Candidate (Id,FirstName,LastName,EnteredDate) VALUES (4,'Sansa','Stark','2000-01-01 00:00:00');
INSERT INTO Candidate (Id,FirstName,LastName,EnteredDate) VALUES (5,'Arya','Stark','2000-01-01 00:00:00');
INSERT INTO Candidate (Id,FirstName,LastName,EnteredDate) VALUES (6,'Tyrion','Lannister','2000-01-01 00:00:00');
INSERT INTO Candidate (Id,FirstName,LastName,EnteredDate) VALUES (7,'New','Candidate','2018-08-07 07:31:59');
INSERT INTO Candidate (Id,FirstName,LastName,EnteredDate) VALUES (8,'Nikolay','Mikheev','2018-08-07 12:19:28');

INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (1,2);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (1,1);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (2,2);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (2,3);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (4,4);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (4,5);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (4,3);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (8,1);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (8,2);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (8,3);
INSERT INTO CandidateSkill (CandidateID,SkillID) VALUES (8,5);



select FirstName, LastName, name as 'Skill' from 
                         Candidate inner join CandidateSkill on Candidate.Id = CandidateSkill.CandidateID
                         inner join Skill on Skill.Id = CandidateSkill.SkillID