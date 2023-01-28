# AppInstaller
Simple tool that makes your project easy to install.  

# How to use
[Here](https://github.com/LDev-IX/AppInstaller/tree/main/example) is a quick example of a simple AppInstaller project.  
> .lixi file documentation:
> - first line contains the name of the app
> - if the second line is "Shortcut" then the installer will copy the executable and add a desktop shortcut
> - if the current line starts with "mkdir" then the installer will create a directory
> - if the current line is not any of the above, then the installer will copy the specified file

# FAQ
Q: Where does AppInstaller copy files ?  
A: AppInstaller copies files to C:\ProgramData\{App_Name}\  
