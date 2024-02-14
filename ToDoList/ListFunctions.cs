using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToDoList
{
    internal class ListFunctions
    {
        private List<ListItem> listItems = new List<ListItem>();

        static bool AddItem(ListItem newItem, List<ListItem> listItems, bool isRaw = false)
        {
            if (!isRaw)
            {
                newItem.Date = DateTime.Now;
            }

            newItem.Index = listItems.Count;
            listItems.Add(newItem);

            return true;

        }

        static string RemoveItem(int aIndex, List<ListItem> listItems)
        {
            string removedTask = listItems[aIndex].Task;
            listItems.RemoveAt(aIndex);

            RefreshIndices(listItems);

            return removedTask;
        }

        static void RefreshIndices(List<ListItem> listItems)
        {
            for (int i = 0; i < listItems.Count; i++)
            {
                listItems[i].Index = i;
            }
        }



        static bool GetAllItems(List<ListItem> listItems)
        {
            Console.WriteLine("--|completed|task|date|--");
            foreach (var item in listItems)
            {
                string complete = item.Complete ? "✔️" : "no";
                Console.WriteLine("-------------------------");
                Console.WriteLine($" {item.Index}|{complete}|{item.Task}|{item.Date:yyyy-MM-dd HH:mm:ss}|");
            }
            return true;
        }

        static bool GetItem(List<ListItem> listItems, int aIndex)
        {
            Console.WriteLine("--|completed|task|date|--");
            string complete = listItems[aIndex].Complete ? "✔️" : "no";
            Console.WriteLine("-------------------------");
            Console.WriteLine($" {listItems[aIndex].Index}|{complete}|{listItems[aIndex].Task}|{listItems[aIndex].Date:yyyy-MM-dd HH:mm:ss}|");
            Console.WriteLine("-------------------------");

            return true;

        }

        static bool Edit(List<ListItem> listItems, int aIndex, string editedTask)
        {
            var itemToEdit = listItems.FirstOrDefault(item => item.Index == aIndex);

            if (itemToEdit != null)
            {
                itemToEdit.Task = editedTask;
                return true;
            }

            return false;

        }

        static string TochiCompiler(List<ListItem> listItems)
        {
            StringBuilder data = new StringBuilder();

            foreach (ListItem item in listItems)
            {
                data.Append($"-add -raw(;) {item.Task.Replace(";", ":")};{item.Date};{item.Complete}\n");
            }

            return data.ToString();
        }

        static string[] Save(string[] args, List<ListItem> listItems, string groupName = "", string dirPath = "default", bool AutoSave = false)
        {
            string directory = dirPath;
            string file;

            if (args.Length > 0 && directory == "default")
            {
                directory = Directory.GetCurrentDirectory();
            }
            else if (directory == "default")
            {
                directory = Path.Combine(Directory.GetCurrentDirectory(), "groups/");
            }

            file = Path.Combine(directory, $"{groupName}.tochi");

            if (args.Length > 0 && groupName == "")
            {
                file = args[0];
            }

            try
            {
                if (args.Length == 0 && File.Exists(file))
                {
                    Console.Write($"File '{file}' already exists. Do you want to override it? (Y/N): ");
                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.WriteLine();

                    if (key.Key != ConsoleKey.Y)
                    {
                        Console.WriteLine(">> Save operation canceled.");
                        return new string[] { "false", file };
                    }
                }

                if (File.Exists(file) && args.Length > 0 && file != args[0])
                {
                    Console.Write($"File '{file}' already exists. Do you want to clone or merge the list to '{groupName}'? (C/M/N): ");
                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.WriteLine();

                    if (key.Key == ConsoleKey.C)
                    {
                        file = Path.Combine(directory, $"{groupName}.tochi");

                        using (StreamWriter sw = new StreamWriter(file))
                        {
                            sw.Write(TochiCompiler(listItems));
                        }

                        Console.WriteLine($">> Successfully cloned to {file}");
                        Console.WriteLine(">> Opening file...");

                        ProcessStartInfo psi1 = new ProcessStartInfo
                        {
                            FileName = file,
                            UseShellExecute = true
                        };

                        Process.Start(psi1);

                        return new string[] { "true", file };
                    }
                    else if (key.Key == ConsoleKey.M)
                    {
                        string content1 = File.ReadAllText(file);

                        string content2 = File.ReadAllText(args[0]);

                        string mergedContent = content1 + content2;

                        using (StreamWriter sw = new StreamWriter(file))
                        {
                            sw.Write(mergedContent);
                        }

                        Console.WriteLine($">> Successfully merged to {file}");
                        Console.WriteLine(">> Opening merged file...");

                        ProcessStartInfo psi2 = new ProcessStartInfo
                        {
                            FileName = file,
                            UseShellExecute = true
                        };

                        Process.Start(psi2);

                        return new string[] { "true", file };
                    }
                    else
                    {
                        return new string[] { "true", file };
                    }
                }


                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(TochiCompiler(listItems));
                }

                if (!AutoSave)
                {
                    Console.WriteLine(">> Successfully saved to " + file);
                    Console.WriteLine(">> Opening file...");

                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = file,
                        UseShellExecute = true
                    };

                    Process.Start(psi);
                }

                

                return new string[] { "true", file };
            }
            catch (Exception e)
            {
                Console.WriteLine(">> " + e.Message);
                return new string[] { "false", file };
            }
        }



        static void Help()
        {
            Console.WriteLine("--- Tochi's To-Do List Cheat Sheet ---\n");

            Console.WriteLine("- Add Task:");
            Console.WriteLine("  Syntax: -add [task_description] [-raw(delimiter)]");
            Console.WriteLine("  Example 1: -add Buy groceries");
            Console.WriteLine("  Example 2: -add -raw(;) Buy groceries;2024-02-14;false");
            Console.WriteLine("  Description:");
            Console.WriteLine("    - Adds a new task to the to-do list.");
            Console.WriteLine("    - Optionally, use -raw(delimiter) for advanced task input.\n");

            Console.WriteLine("- Remove Task:");
            Console.WriteLine("  Syntax: -remove [task_index]");
            Console.WriteLine("  Example: -remove 1");
            Console.WriteLine("  Description: Removes the task at the specified index from the to-do list.\n");

            Console.WriteLine("- Get Tasks:");
            Console.WriteLine("  Syntax: -get [type] [index (optional)]");
            Console.WriteLine("  Example 1: -get 0");
            Console.WriteLine("  Example 2: -get 1 2");
            Console.WriteLine("  Description:");
            Console.WriteLine("    - -get 0: Get all tasks in the to-do list.");
            Console.WriteLine("    - -get 1 [index]: Get details of the task at the specified index.\n");

            Console.WriteLine("- Edit Task:");
            Console.WriteLine("  Syntax: -edit [task_index] [new_task_description]");
            Console.WriteLine("  Example: -edit 1 Update task description");
            Console.WriteLine("  Description: Edits the task description at the specified index.\n");

            Console.WriteLine("- Save To-Do List:");
            Console.WriteLine("  Syntax: -save [group_name] [directory (optional)]");
            Console.WriteLine("  Example 1: -save mytasks");
            Console.WriteLine("  Example 2: -save mytasks /path/to/directory");
            Console.WriteLine("  Description: Saves the current to-do list to a file with the specified group name and directory.\n");

            Console.WriteLine("- Get List of Groups:");
            Console.WriteLine("  Syntax: -getgrps [directory (optional)]");
            Console.WriteLine("  Example 1: -getgrps");
            Console.WriteLine("  Example 2: -getgrps /path/to/directory");
            Console.WriteLine("  Description: Displays a list of available groups in the specified directory or the default 'groups' directory.\n");

            Console.WriteLine("- Get Current Group:");
            Console.WriteLine("  Syntax: -getgrp");
            Console.WriteLine("  Description: Displays the name of the current group, if any. Use '-save group_name' to create a group.\n");

            Console.WriteLine("- Help:");
            Console.WriteLine("  Syntax: help");
            Console.WriteLine("  Description: Displays tips and information about available commands.\n");

            Console.WriteLine("--- Usage Example ---");
            Console.WriteLine("- To add a task: -add Buy groceries");
            Console.WriteLine("- To remove a task: -remove 1");
            Console.WriteLine("- To get all tasks: -get 0");
            Console.WriteLine("- To get a specific task: -get 1 2");
            Console.WriteLine("- To edit a task: -edit 1 Update task description");
            Console.WriteLine("- To save the to-do list: -save mytasks");
            Console.WriteLine("- To get a list of groups: -getgrps");
            Console.WriteLine("- To get the current group: -getgrp");
            Console.WriteLine("- For help: help");
        }

        static void AutoSave(string[] args, List<ListItem> listItems)
        {
            if (args.Length > 0)
            {
                Save(args, listItems, AutoSave: true);
            }
        }

        static void GetGroup(string[] args)
        {
            if (args.Length > 0)
            {
                string[] group = args[0].Split("\\");
                string name = group[group.Length - 1];
                string[] fileParts = name.Split(".");
                Array.Resize(ref fileParts, fileParts.Length - 1);
                name = String.Join(".", fileParts);
                Console.WriteLine(name);
            }
            else
            {
                Console.WriteLine("Not in a group! use '-save group_name' to make group.");
            }
        }

        static void GetGroups(string[] args, string dir = "default")
        {
            string[] files;
            string[] fileParts;
            string name;

            dir = "default";

            if (args.Length > 0 && dir == "default")
            {
                dir = Directory.GetCurrentDirectory();
            }
            else if (dir == "default")
            {
                dir = Path.Combine(Directory.GetCurrentDirectory(), "groups");
            }
            try
            {
                files = Directory.GetFiles(dir, "*.tochi");

                foreach (string file in files)
                {
                    fileParts = file.Split('\\');
                    if (fileParts.Length > 1)
                    {
                        name = fileParts[fileParts.Length - 1];
                        fileParts = name.Split(".");
                        Array.Resize(ref fileParts, fileParts.Length - 1);
                        name = String.Join(".", fileParts);
                        Console.WriteLine(name);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(">> Error: " + e.Message);
            }
        }



        public string[] Run(List <ListItem> listItems, string[] args, string aInput = "")
        {
            string input;
            if (aInput != "")
            {
                input = aInput;
            }
            else
            {
                Console.Write(">> ");
                input = Console.ReadLine();
            }
            string mode = input.Split(" ")[0];

            switch (mode)
            {
                case string m when m == "-add":
                    try
                    {
                        input = input.Replace("-add ", "");
                        DateTime date = DateTime.Now;
                        ListItem newItem = new ListItem(input, date, 0, false);

                        string pattern = @"-raw\(([^)]*)\) ";
                        Regex regex = new Regex(pattern);
                        Match match = regex.Match(input);

                        if (match.Success)
                        {
                            string delimiter = match.Groups[1].Value;

                            input = regex.Replace(input, string.Empty);

                            string[] inputListAdd = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                            newItem.Task = inputListAdd[0];
                            newItem.Date = DateTime.Parse(inputListAdd[1]);
                            newItem.Complete = Convert.ToBoolean(inputListAdd[2]);

                            bool isRaw = true;

                            if (AddItem(newItem, listItems, isRaw))
                            {
                                Console.WriteLine(">> added '" + newItem.Task + "' to list!");
                                AutoSave(args, listItems);
                                return ["true", "added '" + input + "' to list!"];

                            }
                        }
                        else if (AddItem(newItem, listItems))
                        { 
                            Console.WriteLine(">> added '" + input + "' to list!");
                            AutoSave(args, listItems);
                            return ["true", "added '" + input + "' to list!"];
                            
                        }
                        

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(">> " + e.Message);
                        Console.WriteLine(">> Unable to add. Type 'help'");
                        return ["false", "Unable to add. Type 'help'"];
                    }
                    break;
                case string m when m == "-remove":
                    try
                    {
                        input = input.Replace("-remove ", "");

                        string removedTask = RemoveItem(Convert.ToInt32(input), listItems);

                        Console.WriteLine(">> removed '" + removedTask  + "' from list!");
                        AutoSave(args, listItems);
                        return ["true", "removed '" + removedTask + "' from list!"];

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(">> " + e.Message);
                        Console.WriteLine(">> Unable to remove. Type 'help'");
                        return ["false", "Unable to remove. Type 'help'"];
                    }

                case string m when m == "-get":
                    try
                    {
                        input = input.Replace("-get ", "");

                        int type = Convert.ToInt32(input.Split(" ")[0]);

                        if (type == 0)
                        {
                            GetAllItems(listItems);
                        }
                        else if (type == 1)
                        {
                            GetItem(listItems, Convert.ToInt32(input.Split(" ")[1]));
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(">> " + e.Message);
                    }
                    return ["true", ""];
                case string m when m == "-edit":
                    try
                    {
                        input = input.Replace("-edit ", "");

                        int index = Convert.ToInt32(input.Split(" ")[0]);

                        string editedTask = string.Join(" ", input.Split(" ").Skip(1));

                        Edit(listItems, index, editedTask);
                        AutoSave(args, listItems);
                        Console.WriteLine(">> edited successfully!");

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(">> " + e.Message);
                    }
                    return ["true", ""];
                case string m when m == "-completed":
                    break;
                case string m when m == "-save":

                    input = input.Replace("-save ", "");

                    string[] inputList = input.Split(" ");

                    string name = inputList[0];

                    if (inputList.Length > 1)
                    {
                        string dir = String.Join(" ", inputList.Skip(1));

                        Save(args, listItems ,name, dir);
                    }else if (inputList.Length == 0)
                    {
                        Save(args, listItems);
                    }
                    else
                    {
                        Save(args, listItems, name);
                    }
                    break;
                case string m when m == "-getgrps":
                    input = input.Replace("-getgrps ", "");

                    inputList = input.Split(" ");

                    string directory = inputList[0];

                    if (inputList.Length == 1)
                    {
                        GetGroups(args, directory);
                    }
                    else if (inputList.Length == 0)
                    {
                        GetGroups(args);

                    }
                    break;
                case string m when m == "-getgrp":
                    GetGroup(args);
                    break;
                case string m when m == "help":
                    Help();
                    return ["true", ""];
                default:
                    return ["false", ""];
            }
            return ["false", ""];

        }
    }
}
