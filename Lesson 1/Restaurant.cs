using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_1
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new();
        private readonly Producer _producer = new("BookingNotification", "localhost");
        public Restaurant()
        {
            for (ushort i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }
        public void BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Вам придет уведомление");
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons && t.State == State.Free);
                await Task.Delay(1000 * 5);
                table?.SetState(State.Booked);
                _producer.Send(table is null
                ? $"УВЕДОМЛЕНИЕ: К сожалению, сейчас все столики заняты"
                : $"УВЕДОМЛЕНИЕ: Готово! Ваш столик номер {table.Id}");
            });
        }
        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Оставайтесь на линии");
            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons && t.State == State.Free);
            Thread.Sleep(1000 * 5);
            table?.SetState(State.Booked);
            _producer.Send(table is null
                ? $"К сожалению, сейчас все столики заняты"
                : $"Готово! Ваш столик номер {table.Id}");
        }
        public void CancelBookTableAsync(int countOfPersons, int id)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я сниму Вашу бронь. Вам придет уведомление");
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.Id == id && t.State == State.Booked);
                await Task.Delay(1000 * 5);
                table?.SetState(State.Free);
                _producer.Send($"УВЕДОМЛЕНИЕ: Готово! Бронь снята с Вашего столик номер {table.Id}");
            });
        }
        public void CancelBookTable(int countOfPersons, int id)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я отменю вашу бронь. Оставайтесь на линии");
            var table = _tables.FirstOrDefault(t => t.Id == id && t.State == State.Booked);
            Thread.Sleep(1000 * 5);
            table?.SetState(State.Free);
            _producer.Send($"Готово! Бронь снята с Вашего столик номер {table.Id}");
        }
    }
}
