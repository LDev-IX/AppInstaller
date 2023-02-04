using System;
using System.IO;

namespace AppInstaller;

static class Program{
    static App constructed_app = new App();
    static string working_directory = Environment.CurrentDirectory;

    static void Main(){
        if(!File.Exists($"{working_directory}\\.lixi")){
            Console.WriteLine("Error: .lixi file not found!");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            return;
        }

        int current_index = 0;
        foreach(string f_line in File.ReadAllLines($"{working_directory}\\.lixi")){
            string f_temp = f_line.Replace(", ", ",").Replace(");", "");
            Command constructed_cmd = new Command();
            constructed_cmd.name = f_temp.Split('(', 2)[0];
            constructed_cmd.arguments = f_temp.Split('(', 2)[1].Split(',', 2);
            switch(ParseCommand(constructed_cmd, false)){
                case CommandType.Attribute:
                    constructed_cmd.type = ParseCommand(constructed_cmd, true);
                    break;

                case CommandType.Method:
                    constructed_cmd.type = CommandType.Method;
                    break;

                case CommandType.Unknown:
                    Console.WriteLine("Error: Incorrect .lixi file syntax!");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    return;
            }
            constructed_app.commands[current_index] = constructed_cmd;
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
        Directory.CreateDirectory($"C:\\ProgramData\\{constructed_app.name}");

        foreach(Command f_command in constructed_app.commands){
            if(f_command.type == CommandType.Method){
                ParseCommand(f_command, true);
            }
        }

        Console.WriteLine("Done!");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    static CommandType ParseCommand(Command command, bool execute){
        switch(command.name){
            default:
                return CommandType.Unknown;
            
            case "set_name":
                if(execute){
                    constructed_app.name = command.arguments[0];
                }
                return CommandType.Attribute;

            case "set_description":
                if(execute){
                    constructed_app.description = command.arguments[0];
                }
                return CommandType.Attribute;

            case "set_author":
                if(execute){
                    constructed_app.author = command.arguments[0];
                }
                return CommandType.Attribute;

            case "set_website":
                if(execute){
                    constructed_app.website = command.arguments[0];
                }
                return CommandType.Attribute;
            
            case "shortcut":
                if(execute){
                    Console.WriteLine("Creating a desktop shortcut");
                    File.CreateSymbolicLink($"C:\\Users\\{Environment.UserName}\\Desktop\\{constructed_app.name}", $"C:\\ProgramData\\{constructed_app.name}\\{command.arguments[0]}");
                }
                return CommandType.Method;

            case "move":
                if(execute){
                    Console.WriteLine("Moving files");
                    File.Copy($"{working_directory}\\assets\\{command.arguments[0]}", $"C:\\ProgramData\\{constructed_app.name}\\{command.arguments[1]}", true);
                }
                return CommandType.Method;

            case "mkdir":
                if(execute){
                    Console.WriteLine("Creating directories");
                    Directory.CreateDirectory($"C:\\ProgramData\\{constructed_app.name}\\{command.arguments[0]}");
                }
                return CommandType.Method;
        }
    }
}

public class App{
    public string name {get; set;}
    public string description {get; set;}
    public string author {get; set;}
    public string website {get; set;}
    public Command[] commands {get; set;}

    public App(){
        name = "";
        description = "";
        author = "";
        website = "";
        commands = new Command[256];
        for(int i = 0; i < 256; i++){
            commands[i] = new Command();
        }
    }
}

public class Command{
    public string name {get; set;}
    public string[] arguments {get; set;}
    public CommandType type {get; set;}

    public Command(){
        name = "";
        arguments = new string[2]{"", ""};
        type = CommandType.Unknown;
    }
}

public enum CommandType{
    Attribute,
    Method,
    Unknown
}