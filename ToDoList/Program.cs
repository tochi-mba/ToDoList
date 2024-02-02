

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

static void Start(List<ListItem> listItems)
{ 
    Console.Write(">> ");
    string input;
    input = Console.ReadLine();
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
                    Console.WriteLine(">> added '"+ input + "' to list!");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(">> " + e.Message);
                Console.WriteLine(">> Unable to add. Type 'help'");
            }
            break;
        case string m when m == "-remove":
            try
            {
                input = input.Replace("-remove ", "");

                RemoveItem(Convert.ToInt32(input), listItems);

                Console.WriteLine(">> removed '" + input + "' from list!");

            }
            catch (Exception e)
            {
                Console.WriteLine(">> "+e.Message);
                Console.WriteLine(">> Unable to remove. Type 'help'");
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
                Console.WriteLine(">> "+e.Message);
            }
            break;
        default:
            Help();
            break;
    }
    Start(listItems);
}

Start(listItems);









