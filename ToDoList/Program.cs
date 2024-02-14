

using ToDoList;


Console.WriteLine("---Welcome to Tochi's To Do list. Type help for tips.---");
ListFunctions functions = new ListFunctions();
List<ListItem> listItems = new List<ListItem>();
TochiFileHandler tochiFileHandler = new TochiFileHandler();

if (args.Length > 0)
{
    tochiFileHandler.FileHandler(listItems, args);
}
else
{
    while (true)
    {
        functions.Run(listItems, args);
    }
}

