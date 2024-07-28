USE OnlineLearningPlatform;

INSERT into Lessons
VALUES(
    'English',
    1,
    'https://youtu.be/s6m8Aby2at8',
    'English is a language',
    1,
    1
);

select *
from Lessons

delete from Lessons
WHERE Id = 11

DBCC CHECKIdent('Lessons', RESEED, 0)

select *
from QuizQuestions

select *
from AspNetRoles

select *
from AspNetUserRoles

-- delete all AspNetUsers Rows
delete from AspNetUsers

select *
from Enrollments


