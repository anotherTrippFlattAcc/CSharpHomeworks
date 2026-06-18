using System.Linq.Expressions;

namespace TaskList
{
    class TaskItem
    {
        public string _description;
        public bool isDone;

        public TaskItem(string description)
        {
            _description = description;
            isDone = false;
        }

        public void Complete()
        {
            isDone = true;
        }
    }
    
    internal class Program
    {
        static void printTasks(List<TaskItem> taskList)
        {
            for (int i = 0; i < taskList.Count; i++)
            {
                string isDone = taskList[i].isDone ? "X" : " ";
                Console.WriteLine($"{i+1}. [{isDone}] {taskList[i]._description}");
            }
        }
        
        public static void Main(string[] args)
        {
            var taskList = new List<TaskItem>();

            int choice = 0;
            Console.WriteLine("--- Список задач ---\n1. Добавить задачу\n2. Посмотреть задачи\n3. Отметить задачу как выполненную\n4. Выйти");
            while (choice != 4)
            {
                Console.Write("Выберите команду (1, 2, 3, 4): ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.Write("Введите описание новой задачи: ");
                        taskList.Add(new TaskItem(Console.ReadLine()));
                        Console.WriteLine("Добавлена новая задача!");
                        break;
                    case 2:
                        Console.WriteLine("--- Текущие задачи ---");
                        printTasks(taskList);
                        break;
                    case 3:
                        Console.WriteLine("--- Отметить задачу как выполненную ---");
                        printTasks(taskList);
                        Console.Write("Введите номер задачи для выполнения: ");
                        int n = Convert.ToInt32(Console.ReadLine());
                        taskList[n-1].Complete();
                        Console.WriteLine($"Задача \"{taskList[n-1]._description}\" отмечена как выполненная!");
                        break;
                    case 4:
                        Console.WriteLine("Выход...");
                        break;
                    default:
                        Console.WriteLine("Неверная команда!");
                        break;
                }
            }
        }
    }
}