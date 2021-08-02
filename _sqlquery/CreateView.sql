SELECT * FROM VidelsView
SELECT * FROM Videls
SELECT NumKvartal AS  [Номер квартала],CreateDateKvartal AS [Дата и время создания квартала]FROM Kvartals


CREATE VIEW VidelsView AS
SELECT Kvartals.NumKvartal AS [Номер квартала],
       Forestries.NameForestry AS [Лесничество],
       Videls.NumVidel AS [Номер выдела],
       Videls.Area AS [Площадь], 
       TypeEarths.NameTypeEarth AS [Вид земель], 
       TypeOrls.NameTypeOrl AS [ОРЛ/ОЗУ], 
       ExpositionSlopes.NameExpositionSlope AS [Экспозиция склона],
       Videls.Krut AS [Крутизна],
       TypeErrosions.NameTypeErrosion AS [Вид эрозии],      
       DegreeErrosions.NameDegreeErrosion AS [Степень эррозии],      
                   
       Accounts.Username AS [Создал],
       Videls.CreateDateVidel AS [Дата и время создания документа]      
FROM Videls INNER JOIN Accounts ON Videls.AccountId = Accounts.Id
INNER JOIN Forestries ON Videls.ForestryId = Forestries.Id
INNER JOIN Kvartals ON  Videls.KvartalId = Kvartals.Id

INNER JOIN DegreeErrosions ON  Videls.DegreeErrosionId = DegreeErrosions.Id
INNER JOIN TypeEarths ON  Videls.TypeEarthId = TypeEarths.Id
INNER JOIN TypeErrosions ON  Videls.TypeErrosionId = TypeErrosions.Id
INNER JOIN ExpositionSlopes ON  Videls.ExpositionSlopeId = ExpositionSlopes.Id
INNER JOIN TypeOrls ON  Videls.TypeOrlId = TypeOrls.Id





