@echo off

rem install services
installutil /i Perceptive.ARR.Service.Installer.exe

rem start services
net start ARRService
