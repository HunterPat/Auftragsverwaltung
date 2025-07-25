@echo off

rem Start Backend
start cmd /k "cd /d D:\Users\Marcel\Documents\KIC\Verwaltung\Auftragsverwaltung_Marcel_Keiser\Backend\Backend && dotnet watch run"

rem Start Frontend
start cmd /k "cd /d D:\Users\Marcel\Documents\KIC\Verwaltung\Auftragsverwaltung_Marcel_Keiser\Auftragsverrwaltung_Frontend && ng serve"
PAUSE