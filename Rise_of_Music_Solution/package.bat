:: This is run as a post-build event for the Install project
@echo off

:: Delete the Rise_of_Music_target directory
rmdir Rise_of_Music_target /s /q

:: Make the required directories
mkdir Rise_of_Music_target\scenario
mkdir Rise_of_Music_target\scenario\Scripts
mkdir Rise_of_Music_target\scenario\Scripts\Rise_of_Music


:: Copy Rise of Music files
copy Rise_of_Music\bin\Release\CSCore.dll Rise_of_Music_target\scenario\Scripts\Rise_of_Music\CSCore.dll
copy Rise_of_Music\bin\Release\CSCore.xml Rise_of_Music_target\scenario\Scripts\Rise_of_Music\CSCore.xml
copy "Rise_of_Music\bin\Release\Rise of Music.exe" "Rise_of_Music_target\scenario\Scripts\Rise_of_Music\Rise of Music.exe"
copy "Rise_of_Music\bin\Release\Rise of Music.exe.config" "Rise_of_Music_target\scenario\Scripts\Rise_of_Music\Rise of Music.exe.config"
copy Rise_of_Music\rise-of-music.ico Rise_of_Music_target\scenario\Scripts\Rise_of_Music\rise-of-music.ico
copy ..\LICENSE Rise_of_Music_target\scenario\Scripts\Rise_of_Music\LICENSE
copy ..\README.md Rise_of_Music_target\scenario\Scripts\Rise_of_Music\README.md
copy Rise_of_Music\Rise_of_Music.bhs Rise_of_Music_target\scenario\Scripts\Rise_of_Music\Rise_of_Music.bhs
copy Rise_of_Music\Rise_of_Music.bhs Rise_of_Music_target\scenario\Scripts\Rise_of_Music\Rise_of_Music.bhs.bak
copy ..\version.txt Rise_of_Music_target\scenario\Scripts\Rise_of_Music\version.txt