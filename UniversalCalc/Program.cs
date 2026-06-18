namespace UniversalCalc
{
    internal class Program
    {
        static double Calculate(double x, double y, Func<double, double, double> operation)
        {
            return operation(x, y);
        }
        
        public static void Main(string[] args)
        {
            Console.WriteLine("--- Универсальный калькулятор ---");
            
            Console.WriteLine("Введите первое число:");
            double num1 = Convert.ToDouble(Console.ReadLine());
            
            Console.WriteLine("Введите второе число:");
            double num2 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("--- Результаты вычислений ---");
            
            Console.WriteLine($"Сложение: {Calculate(num1, num2, (a, b) => a + b)}");
            Console.WriteLine($"Вычитание: {Calculate(num1, num2, (a, b) => a - b)}");
            Console.WriteLine($"Умножение: {Calculate(num1, num2, (a, b) => a * b)}");
            
            if (num2 != 0)
            {
                Console.WriteLine($"Деление: {Calculate(num1, num2, (a, b) => a / b)}");
            }
            else
            {
                Console.WriteLine("Деление: на ноль делить нельзя!");
            }
        }
    }
}