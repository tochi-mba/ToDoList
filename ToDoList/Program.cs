

using ToDoList;


List<ListItem> listItems = new List<ListItem>();

static bool AddItem(ListItem newItem, List<ListItem> listItems)
{

    bool added = false;


    newItem.Index = listItems.Count;
    listItems.Add(newItem);
    added = true;

    return added;

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




string task;
DateTime date = DateTime.Now;

Console.Write("Enter your task: ");
task = Console.ReadLine();

ListItem newItem = new ListItem(task, date, 0, false);

if (AddItem(newItem, listItems))
{
    Console.WriteLine("added new task to list!");
    
}

GetAllItems(listItems);
RemoveItem(0, listItems);
GetAllItems(listItems);


