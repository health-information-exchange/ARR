@echo off
rem stop services
net stop ARRService

rem install services
installutil /u Perceptive.ARR.Service.Installer.exe