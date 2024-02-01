

using ToDoList;


List<ListItem> listItems = new List<ListItem>();

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
        Console.WriteLine(" "+item.Index+"|"+complete+"|"+item.Task+"|"+ item.Date.ToString()+"|");
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

}


Console.WriteLine("---Welcome to Tochi's To Do list. Type help for tips.---");

static void Start()
{ 
    Console.Write(">>");
    string input;
    input = Console.ReadLine();
    string mode = input.Split(" ")[0];

    switch (mode)
    {
        case string m when m == "add":
            Console.WriteLine("hi");
            break;
        default:
            Help();
            break;
    }
    Start();
}

Start();



string task;
DateTime date = DateTime.Now;

Console.Write("Enter your task: ");
task = Console.ReadLine();

ListItem newItem = new ListItem(task, date, 0, false);

if (AddItem(newItem, listItems))
{
    Console.WriteLine("added new task to list!");
    
}





