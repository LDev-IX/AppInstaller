using System;
using System.IO;

namespace AppInstaller;

static class Program{
    static string var_working_dir = Environment.CurrentDirectory;
    static string var_app_name = "Null";

    static void Main(){
        string var_current_dir = Environment.CurrentDirectory;
        string var_app_name = "Unknown";
        int var_current_line = 0;

        if(!File.Exists(var_current_dir + @"\.lixi")){
            Console.WriteLine(".lixi file not found!");
            return;
        }

        foreach(string fLine in File.ReadAllLines(var_current_dir + @"\.lixi")){
            switch(var_current_line){
                case 0:
                    var_app_name = fLine;
                    Directory.CreateDirectory(@"C:\ProgramData\" + var_app_name);
                    Console.WriteLine("Installing " + var_app_name);
                break;

                case 1:
                    if(fLine == "Shortcut"){
                        Console.WriteLine("Creating desktop shortcut");
                        File.Copy(var_current_dir + @"\assets\" + var_app_name + @".exe", @"C:\ProgramData\" + var_app_name + @"\" +  var_app_name + @".exe", true);
                        File.CreateSymbolicLink(@"C:\Users\" + Environment.UserName + @"\Desktop\" + var_app_name, @"C:\ProgramData\" + var_app_name + @"\" + var_app_name + @".exe");
                    }
                break;

                default:
                    if(fLine.StartsWith("mkdir ")){
                        Console.WriteLine("Creating directory " + @"C:\ProgramData\" + var_app_name + @"\" + fLine.Replace("mkdir ", ""));
                        Directory.CreateDirectory(@"C:\ProgramData\" + var_app_name + @"\" + fLine.Replace("mkdir ", ""));
                    }else{
                        Console.WriteLine("Copying " + var_current_dir + @"\" + fLine + " to " + @"C:\ProgramData\" + var_app_name + @"\" + fLine.Replace(@"assets\", ""));
                        File.Copy(var_current_dir + @"\" + fLine, @"C:\ProgramData\" + var_app_name + @"\" + fLine.Replace(@"assets\", ""), true);
                    }
                break;
            }
            var_current_line++;
        }
        Console.WriteLine("Done!");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    static void Rework(){
        if(!File.Exists(var_working_dir + @"\.lixi")){
            Console.WriteLine(".lixi file not found!");
            return;
        }

        foreach(string f_line in File.ReadAllLines(var_working_dir + @"\.lixi")){
            
        }
    }
}