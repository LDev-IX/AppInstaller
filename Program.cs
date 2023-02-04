using System;
using System.IO;

namespace AppInstaller;

static class Program{
    static string var_working_dir = Environment.CurrentDirectory;
    static string var_app_name = "Null";

    static void Main(){
        string met_method = "Null";
        string[] met_args = {"Null", "Null", "Null", "Null"};

        if(!File.Exists(var_working_dir + @"\.lixi")){
            Console.WriteLine(".lixi file not found!");
            return;
        }

        foreach(string f_line in File.ReadAllLines(var_working_dir + @"\.lixi")){
            string f_temp = f_line.Replace(" ", "").Replace("(", ":").Replace(");", "");
            met_method = f_temp.Split(':', 2)[0];
            met_args = f_temp.Split(':', 2)[1].Split(',', 4);
            if(!ExecuteMethod(met_method, met_args)){
                return;
            }
        }
    }

    static bool ExecuteMethod(string arg_Name, string[] arg_Arguments){
        switch(arg_Name){
            default:
                Console.WriteLine(".lixi method \"" + arg_Name + "\" not found!");
                return false;
            
            case "set_app_name":
                var_app_name = arg_Arguments[0];
                Directory.CreateDirectory(@"C:\ProgramData\" + var_app_name);
                break;

            case "create_shortcut":
                File.CreateSymbolicLink(@"C:\Users\" + Environment.UserName + @"\Desktop\" + var_app_name, @"C:\ProgramData\" + var_app_name + @"\" + arg_Arguments[0]);
                break;

            case "move":
                File.Copy(var_working_dir + @"\assets\" + arg_Arguments[0], @"C:\ProgramData\" + var_app_name + @"\" + arg_Arguments[1], true);
                break;

            case "mkdir":
                Directory.CreateDirectory(@"C:\ProgramData\" + var_app_name + @"\" + arg_Arguments[0]);
                break;
        }
        return true;
    }
}