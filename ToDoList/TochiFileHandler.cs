using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    internal class TochiFileHandler
    {

        public void FileHandler(List<ListItem> listItems, string[] args) {
            ListFunctions functions = new ListFunctions();
            
            string fileContents = File.ReadAllText(args[0]);
            string[] fileRawDataList = fileContents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> fileDataList = new List<string>();

            foreach (string line in fileRawDataList)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    fileDataList.Add(line);
                }
            }


            for (int j = 0; j < fileDataList.Count; j++)
            {

                string[] returnVal = functions.Run(listItems, fileDataList[j]);

                if (returnVal[0] == "true")
                {
                    if (returnVal[1] == "")
                    {
                        Console.WriteLine(">> " + args[0] + " File Corrupted!: Line " + (j + 1));
                    }
                }
                else if (returnVal[0] == "false" && returnVal[1] == "")
                {
                    Console.WriteLine(">> " + args[0] + " File Error " + returnVal[1] + ": Line " + (j + 1));
                }
                else
                {
                    Console.WriteLine(">> " + args[0] + " File Error!: Line " + (j + 1));

                }

            }

            while (true)
            {
                functions.Run(listItems);
            }
            
        }
    }
}
