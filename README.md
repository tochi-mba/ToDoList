--- Tochi's To-Do List Cheat Sheet ---

- Add Task:
  Syntax: -add [task_description]
  Example: -add Buy groceries
  Description: Adds a new task to the to-do list.

- Remove Task:
  Syntax: -remove [task_index]
  Example: -remove 1
  Description: Removes the task at the specified index from the to-do list.

- Get Tasks:
  Syntax: -get [type] [index (optional)]
  Example 1: -get 0
  Example 2: -get 1 2
  Description:
    - -get 0: Get all tasks in the to-do list.
    - -get 1 [index]: Get details of the task at the specified index.

- Edit Task:
  Syntax: -edit [task_index] [new_task_description]
  Example: -edit 1 Update task description
  Description: Edits the task description at the specified index.

- Help:
  Syntax: help
  Description: Displays tips and information about available commands.

--- Usage Example ---
- To add a task: -add Buy groceries
- To remove a task: -remove 1
- To get all tasks: -get 0
- To get a specific task: -get 1 2
- To edit a task: -edit 1 Update task description
- For help: help
