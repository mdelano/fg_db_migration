set restoreFilePath=%1
set descryptionKey=%2
set databaseName=%3
set instance=%4

C:\Program Files\Quest Software\LiteSpeed\SQL Server\Engine\sqllitespeed.exe -RLog -F"%restoreFilePath%" -N1 -K"%descryptionKey=%" -D "%databaseName%" -WNORECOVERY -A0 -S"%instance%" -T