exec master.dbo.xp_restore_database @database = N'BB02' ,
@filename = N'Q:\BB02_Full_20121123070001.bak',
@filenumber = 1,
@encryptionkey = N'BB02',
@with = N'REPLACE',
@with = N'STATS = 10',
@with = N'NORECOVERY',
@with = N'MOVE N''BB02_Data'' TO N''M:\FGSQL_Cluster\MSSQL\Data\BB02_Data.MDF''',
@with = N'MOVE N''BB02_Log'' TO N''L:\FGSQL_Cluster\MSSQL\Logs\BB02_1.LDF''',
@with = N'MOVE N''ftrow_BB02_CAT'' TO N''M:\FGSQL_Cluster\MSSQL\Index\ftrow_BB02_CAT.ndf''',
@with = N'MOVE N''ftrow_QTV_Default'' TO N''M:\FGSQL_Cluster\MSSQL\Index\ftrow_QTV_Default.ndf''',
@affinity = 0,
@logging = 0


GO
