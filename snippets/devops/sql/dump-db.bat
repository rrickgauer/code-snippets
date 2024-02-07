mysqldump -u main -h 000.000.000.000 -p ^
--databases SharpStudio_Web_Dev ^
--column-statistics=FALSE ^
--routines ^
--events ^
--triggers ^
--add-drop-table ^
--allow-keywords ^
--no-create-db ^
--no-data ^
--result-file "C:\xampp\htdocs\files\sharp-studio\sharpstudio.io\sql\ddl\.schemas.sql"


mysqldump -u main -h 000.000.000.000 -p ^
--column-statistics=FALSE ^
--allow-keywords ^
--no-create-db ^
--no-create-info ^
--replace ^
--order-by-primary ^
--result-file "C:\xampp\htdocs\files\sharp-studio\sharpstudio.io\sql\ddl\.data.sql" ^
SharpStudio_Web_Dev Error_Message_Group Error_Message Project_Node_Type


@echo off

cd "C:\xampp\htdocs\files\sharp-studio\sharpstudio.io\sql\ddl"

type ".schemas.sql" ".data.sql" > "dump.sql"

del ".schemas.sql" /Q
del ".data.sql" /Q

pause
