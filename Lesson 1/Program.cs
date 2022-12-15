using System.Diagnostics;

namespace Lesson_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var rest = new Restaurant();
            while (true)
            {
                Console.WriteLine("Привет! Желаете забронировать столик?\n1 - мы уведомим Вас по SMS (асинхронно)" +
                    "\n2 - подождите на линии, мы Вас оповестим (синхронно)" +
                    "\nЖелаете отменить бронь?\n3 - мы уведомим Вас по SMS (асинхронно)" +
                    "\n4 - подождите на линии, мы Вас оповестим (синхронно)");
                if (!int.TryParse(Console.ReadLine(), out var choice) && choice is not (1 or 2 or 3 or 4))
                {
                    Console.WriteLine("Введите, пожалуйста 1, 2, 3 или 4");
                    continue;
                }
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                if (choice == 1)
                {
                    rest.BookFreeTableAsync(1);
                }
                if (choice == 2)
                {
                    rest.BookFreeTable(1);
                }
                if (choice == 3)
                {
                    rest.CancelBookTableAsync(1, 1);
                }
                if (choice == 4)
                {
                    rest.CancelBookTable(1, 1);
                }
                Console.WriteLine("Спасибо за Ваше обращение!");
                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
            }
        }
    }
}