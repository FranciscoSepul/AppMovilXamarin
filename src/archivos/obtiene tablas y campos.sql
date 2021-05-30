USE PawaBackEnd
GO

 SELECT T.name AS Table_Name ,
       C.name AS Column_Name
       --,P.name AS Data_Type 
       --,P.max_length AS Size 
       --CAST(P.precision AS VARCHAR) + '/' + CAST(P.scale AS VARCHAR) AS Precision_Scale
FROM   sys.objects AS T
       JOIN sys.columns AS C ON T.object_id = C.object_id
       JOIN sys.types AS P ON C.system_type_id = P.system_type_id
WHERE  T.type_desc = 'USER_TABLE'
and T.name NOT IN ('Logs', 'LogsAudit', '__EFMigrationsHistory')
and P.name <> 'sysname'
and P.name NOT LIKE '%binary%'
and C.name NOT LIKE '%Id%'
and C.name NOT LIKE '%Comments%'
and C.name NOT LIKE '%Description%'
and C.name NOT LIKE '%Body%'
and C.name NOT LIKE '%Html%'
ORDER BY T.name DESC