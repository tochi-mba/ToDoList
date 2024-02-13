using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    internal class ListFunctions
    {
        private List<ListItem> listItems = new List<ListItem>();

        static bool AddItem(ListItem newItem, List<ListItem> listItems)
        {

            DateTime date = DateTime.Now;

            newItem.Date = date;
            newItem.Index = listItems.Count;
            listItems.Add(newItem);

            return true;

        }

        static bool RemoveItem(int aIndex, List<ListItem> listItems)
        {
            listItems.RemoveAt(aIndex);

            int i = 0;

            foreach (var item in listItems)
            {
                item.Index = i;
                i++;
            }

            return true;
        }


        static bool GetAllItems(List<ListItem> listItems)
        {
            Console.WriteLine("--|completed|task|date|--");
            string complete;
            foreach (var item in listItems)
            {
                if (item.Complete)
                {
                    complete = "✔️";
                }
                else
                {
                    complete = "no";
                }
                Console.WriteLine("-------------------------");
                Console.WriteLine(" " + item.Index + "|" + complete + "|" + item.Task + "|" + item.Date.ToString() + "|");
            }
            return true;
        }

        static bool GetItem(List<ListItem> listItems, int aIndex)
        {

            Console.WriteLine("--|completed|task|date|--");
            string complete;
            if (listItems[aIndex].Complete)
            {
                complete = "✔️";
            }
            else
            {
                complete = "no";
            }
            Console.WriteLine("-------------------------");
            Console.WriteLine(" " + listItems[aIndex].Index + "|" + complete + "|" + listItems[aIndex].Task + "|" + listItems[aIndex].Date.ToString() + "|");
            Console.WriteLine("-------------------------");

            return true;

        }

        static bool Edit(List<ListItem> listItems, int aIndex, string editedTask)
        {

            foreach (var items in listItems)
            {
                if (items.Index == aIndex)
                {
                    items.Task = editedTask;

                    break;
                }
            }

            return true;

        }

        static void Help()
        {
            Console.WriteLine("--- Tochi's To-Do List Cheat Sheet ---\n");

            Console.WriteLine("- Add Task:");
            Console.WriteLine("  Syntax: -add [task_description]");
            Console.WriteLine("  Example: -add Buy groceries");
            Console.WriteLine("  Description: Adds a new task to the to-do list.\n");

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

            Console.WriteLine("- Help:");
            Console.WriteLine("  Syntax: help");
            Console.WriteLine("  Description: Displays tips and information about available commands.\n");

            Console.WriteLine("--- Usage Example ---");
            Console.WriteLine("- To add a task: -add Buy groceries");
            Console.WriteLine("- To remove a task: -remove 1");
            Console.WriteLine("- To get all tasks: -get 0");
            Console.WriteLine("- To get a specific task: -get 1 2");
            Console.WriteLine("- To edit a task: -edit 1 Update task description");
            Console.WriteLine("- For help: help");

        }


        public string[] Run(List <ListItem> listItems,string aInput = "")
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

                        if (AddItem(newItem, listItems))
                        {
                            Console.WriteLine(">> added '" + input + "' to list!");
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

                        RemoveItem(Convert.ToInt32(input), listItems);

                        Console.WriteLine(">> removed '" + listItems[Convert.ToInt32(input)].Task + "' from list!");
                        return ["true", "removed '" + listItems[Convert.ToInt32(input)].Task + "' from list!"];

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(">> " + e.Message);
                        Console.WriteLine(">> Unable to remove. Type 'help'");
                        return ["false", "Unable to remove. Type 'help'"];
                    }
                    break;
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
                    break;
                case string m when m == "-edit":
                    try
                    {
                        input = input.Replace("-edit ", "");

                        int index = Convert.ToInt32(input.Split(" ")[0]);

                        string editedTask = string.Join(" ", input.Split(" ").Skip(1));

                        Edit(listItems, index, editedTask);

                        Console.WriteLine(">> edited successfully!");

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(">> " + e.Message);
                    }
                    return ["true", ""];
                    break;
                case string m when m == "help":
                    Help();
                    return ["true", ""];
                    break;
                default:
                    return ["true", ""];
                    break;
            }
            return ["true", ""];

        }
    }
}
