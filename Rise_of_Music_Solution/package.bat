:: This is run as a post-build event for the Install project
@echo off

:: Delete the target directory
rmdir target /s /q

:: Copy sounds\tracks\<MUSIC_MOOD>\README.txt files
xcopy ..\sounds\tracks\battle_defeat\README.txt target\Rise_of_Music\sounds\tracks\battle_defeat\
xcopy ..\sounds\tracks\battle_victory\README.txt target\Rise_of_Music\sounds\tracks\battle_victory\
xcopy ..\sounds\tracks\economic\README.txt target\Rise_of_Music\sounds\tracks\economic\
xcopy ..\sounds\tracks\lose\README.txt target\Rise_of_Music\sounds\tracks\lose\
xcopy ..\sounds\tracks\win\README.txt target\Rise_of_Music\sounds\tracks\win\

:: Copy Rise of Music files
copy Rise_of_Music\bin\Release\CSCore.dll target\Rise_of_Music\CSCore.dll
copy Rise_of_Music\bin\Release\CSCore.xml target\Rise_of_Music\CSCore.xml
copy Rise_of_Music\bin\Release\Rise_of_Music.exe target\Rise_of_Music\Rise_of_Music.exe
copy ..\LICENSE target\Rise_of_Music\LICENSE
copy ..\README.md target\Rise_of_Music\README.md
copy ..\Rise_of_Music.bhs target\Rise_of_Music\Rise_of_Music.bhs
copy ..\version.txt target\Rise_of_Music\version.txt

:: Copy the installer
copy Install\bin\Release\Install.exe target\Install.exe