using System;
using System.IO;

namespace AppInstaller;

static class Program{
    static App constructed_app = new App();
    static string working_directory = Environment.CurrentDirectory;

    static void Main(){
        if(!File.Exists(working_directory + @"\.lixi")){
            Console.WriteLine("Error: .lixi file not found!");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            return;
        }

        int current_index = 0;
        for(current_index = 0; current_index < 256; current_index++){
            Command unset_cmd = new Command();
            unset_cmd.name = "unset";
            unset_cmd.arguments[0] = "";
            unset_cmd.arguments[1] = "";
            constructed_app.commands[current_index].name = "unset";
        }

        current_index = 0;
        foreach(string f_line in File.ReadAllLines(working_directory + @"\.lixi")){
            string f_temp = f_line.Replace(" ", "").Replace("(", ":").Replace(");", "");
            Command constructed_cmd = new Command();
            constructed_cmd.name = f_temp.Split(':', 2)[0];
            constructed_cmd.arguments = f_temp.Split(':', 2)[1].Split(',', 2);
            constructed_app.commands[current_index] = constructed_cmd;
            current_index++;
        }

        current_index = 0;
        foreach(Command f_command in constructed_app.commands){
            if(f_command.name == "unset"){
                break;
            }
            if(LoadAppProperties(f_command)){
                constructed_app.commands[current_index].name = "propcmd";
            }
            current_index++;
        }

        Console.WriteLine("App info:");
        Console.WriteLine("Name: " + constructed_app.name);
        Console.WriteLine("Description: " + constructed_app.description);
        Console.WriteLine("Made by: " + constructed_app.author);
        Console.WriteLine("Website: " + constructed_app.website);
        Console.WriteLine("");
        Console.WriteLine("Do you want to install this app? [Y/n]");

        if(Console.ReadLine().ToLower() != "y"){
            return;
        }

        Console.Clear();
        Console.WriteLine("Installing " + constructed_app.name);

        foreach(Command f_command in constructed_app.commands){
            if(f_command.name == "unset"){
                break;
            }
            if(f_command.name != "propcmd"){
                if(!ExecuteCommand(f_command)){
                    Console.WriteLine("Error: Incorrect .lixi file syntax!");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    return;
                }
            }
        }

        Console.WriteLine("Done!");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    static bool LoadAppProperties(Command command){
        switch(command.name){
            default:
                return false;

            case "set_name":
                constructed_app.name = command.arguments[0];
                break;

            case "set_description":
                constructed_app.description = command.arguments[0];
                break;

            case "set_author":
                constructed_app.author = command.arguments[0];
                break;

            case "set_website":
                constructed_app.website = command.arguments[0];
                break;
        }
        return true;
    }

    static bool ExecuteCommand(Command command){
        Console.WriteLine(command.name);
        switch(command.name){
            default:
                return false;
            
            case "shortcut":
                Console.WriteLine("Creating a desktop shortcut");
                File.CreateSymbolicLink(@"C:\Users\" + Environment.UserName + @"\Desktop\" + constructed_app.name, @"C:\ProgramData\" + constructed_app.name + @"\" + command.arguments[0]);
                break;

            case "move":
                Console.WriteLine("Moving files");
                File.Copy(working_directory + @"\assets\" + command.arguments[0], @"C:\ProgramData\" + constructed_app.name + @"\" + command.arguments[1], true);
                break;

            case "mkdir":
                Console.WriteLine("Creating directories");
                Directory.CreateDirectory(@"C:\ProgramData\" + constructed_app.name + @"\" + command.arguments[0]);
                break;
        }
        return true;
    }
}

public class App{
    public string name = "Null";
    public string description = "Null";
    public string author = "Null";
    public string website = "Null";
    public Command[] commands = new Command[256];
}

public class Command{
    public string name = "Null";
    public string[] arguments = new string[2];
}