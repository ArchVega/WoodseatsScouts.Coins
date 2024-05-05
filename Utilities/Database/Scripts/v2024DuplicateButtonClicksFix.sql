BEGIN TRAN
-- Counts Before
SELECT (SELECT Count(*) FROM ScavengedCoins) as 'ScavengedCoins Before', (SELECT Count(*) FROM ScavengeResults) as 'ScavengeResults' 


SELECT
	SR1.Id AS 'DuplicatedScavengedResultIds'
	,SR1.MemberId
	,SR1.CompletedAt as SR1
	,SR2.CompletedAt as SR2
FROM 
	ScavengeResults SR1
INNER JOIN 
	ScavengeResults SR2 
ON
	SR2.MemberId = SR1.MemberId AND
	SR2.CompletedAt > SR1.CompletedAt AND
	SR2.CompletedAt <= DATEADD(SECOND, 30, SR1.CompletedAt)

DELETE FROM
	ScavengedCoins
WHERE 
	ScavengeResultId
IN
(
	SELECT
		SR1.Id		
	FROM 
		ScavengeResults SR1
	INNER JOIN 
		ScavengeResults SR2 
	ON
		SR2.MemberId = SR1.MemberId AND
		SR2.CompletedAt > SR1.CompletedAt AND
		SR2.CompletedAt <= DATEADD(SECOND, 30, SR1.CompletedAt)
)

DELETE FROM
	ScavengeResults
WHERE 
	Id
IN
(
	SELECT
		SR1.Id		
	FROM 
		ScavengeResults SR1
	INNER JOIN 
		ScavengeResults SR2 
	ON
		SR2.MemberId = SR1.MemberId AND
		SR2.CompletedAt > SR1.CompletedAt AND
		SR2.CompletedAt <= DATEADD(SECOND, 5, SR1.CompletedAt)
)

-- Counts After
SELECT (SELECT Count(*) FROM ScavengedCoins) as 'ScavengedCoins', (SELECT Count(*) FROM ScavengeResults) as 'ScavengeResults' 

ROLLBACK TRAN