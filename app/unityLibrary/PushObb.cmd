echo off
echo Attempting to push to /mnt/shell/emulated
C:\Program Files\Unity\Hub\Editor\2022.3.55f1c1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe push MM Puzzle.main.obb /mnt/shell/emulated/obb/com.DefaultCompany.com.unity.template.mobile2D/main.1.com.DefaultCompany.com.unity.template.mobile2D.obb
if "%ERRORLEVEL%"=="0" GOTO Success
echo ---------------------------------------------
echo Attempting to push to /mnt/shell/emulated/0/Android
C:\Program Files\Unity\Hub\Editor\2022.3.55f1c1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe push MM Puzzle.main.obb /mnt/shell/emulated/0/Android/obb/com.DefaultCompany.com.unity.template.mobile2D/main.1.com.DefaultCompany.com.unity.template.mobile2D.obb
if "%ERRORLEVEL%"=="0" GOTO Success
echo ---------------------------------------------
echo Attempting to push to /sdcard/Android
C:\Program Files\Unity\Hub\Editor\2022.3.55f1c1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe push MM Puzzle.main.obb /sdcard/Android/obb/com.DefaultCompany.com.unity.template.mobile2D/main.1.com.DefaultCompany.com.unity.template.mobile2D.obb
if "%ERRORLEVEL%"=="0" GOTO Success
echo ---------------------------------------------
:Failure
echo FAILURE
pause
exit -1
:Success
echo SUCCESS
pause
