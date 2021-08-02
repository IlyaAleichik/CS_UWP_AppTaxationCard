 FROM Videls 
SELECT Id,AccountId,(SELECT DISTINCT NumKvartal) AS NumKvartal FROM Videls WHERE AccountId == "2"