namespace Strings
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string input = " иванов иван,петров петр, сидорова Анна, бобров БОРИС ";
            
            Console.WriteLine("--- Форматирование списка пользователей ---\n");
            
            Console.WriteLine($"Исходная строка: {input}\n");
            
            Console.WriteLine("Отформатированный список:");
            
            string[] names = input.Split(',');
            int num = 1;

            foreach (var rawName in names)
            {
                string name = rawName.Trim();
                
                if (string.IsNullOrEmpty(name)) continue;
                
                name = name.ToLower();
                
                string[] parts = name.Split(' ');

                string formattedName = "";
                foreach (var part in parts)
                {
                    if (part.Length > 0)
                    {
                        formattedName += char.ToUpper(part[0]) + part.Substring(1) + " ";
                    }
                }
                
                Console.WriteLine($"{num}. {formattedName.Trim()}");

                num++;
            }
        }
    }
}