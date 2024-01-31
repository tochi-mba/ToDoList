using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{

    internal class ListItem
    {
        private string task;
        private DateTime date;
        private int index;
        private bool complete;

        public ListItem(string aTask, DateTime aDate, int aIndex, bool aComplete)
        {
            Task = aTask;
            Date = aDate;
            Index = aIndex;
            complete = aComplete;
        }

        public string Task
        {
            get { return task; }
            set { task = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public int Index
        {
            get { return index; }
            set
            {
                try
                {
                    Convert.ToInt32(value);

                    index = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public bool Complete
        {
            get { return complete; }
            set { complete = value; }
        }
    }
}
