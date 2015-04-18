:: This is run as a post-build event for the Install project
@echo off

:: Delete the target directory
rmdir target /s /q

:: Make the required directories
mkdir target\scenario
mkdir target\scenario\Scripts
mkdir target\scenario\Scripts\Rise_of_Music


:: Copy Rise of Music files
copy Rise_of_Music\bin\Release\CSCore.dll target\scenario\Scripts\Rise_of_Music\CSCore.dll
copy Rise_of_Music\bin\Release\CSCore.xml target\scenario\Scripts\Rise_of_Music\CSCore.xml
copy Rise_of_Music\bin\Release\Rise_of_Music.exe target\scenario\Scripts\Rise_of_Music\Rise_of_Music.exe
copy ..\LICENSE target\scenario\Scripts\Rise_of_Music\LICENSE
copy ..\README.md target\scenario\Scripts\Rise_of_Music\README.md
copy ..\Rise_of_Music.bhs target\scenario\Scripts\Rise_of_Music\Rise_of_Music.bhs
copy ..\version.txt target\scenario\Scripts\Rise_of_Music\version.txt