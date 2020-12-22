
@echo off

color 1F
echo NetworkStatus Advanced Menu
echo.
set ip=google.com

if [%ip%]==[] (
echo No ip/Netbios name/Domain name chosen
) else (
echo You are curently using this ip/Netbios name/Domain name : %ip%
)
:menu
echo.
echo 1 for netstat, 2 for nslookup, 3 for arp, 4 arp using google.com
set /p action= Action?

if %action%==1 goto netstat
if %action%==2 goto nslookup
if %action%==3 goto arp
if %action%==4 goto arpmore

pause > nul

:netstat
netstat

pause > nul
goto menu

:nslookup
nslookup google.com

pause > nul
goto menu

:arp
arp -a
pause > nul
goto menu
:arpmore
arp -a google.com
pause > nul

goto menu




