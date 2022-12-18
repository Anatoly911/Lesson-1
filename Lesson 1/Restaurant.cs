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
        public Restaurant()
        {
            for (ushort i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }
        public async Task<bool?> BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Вам придет уведомление");
            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons && t.State == TableState.Free);
            //await Task.Delay(1000 * 5);
            return table?.SetState(TableState.Booked);
        }
        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Оставайтесь на линии");
            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons && t.State == TableState.Free);
            Thread.Sleep(1000 * 5);
            table?.SetState(TableState.Booked);
        }
        public async Task<bool?> CancelBookTableAsync(int countOfPersons, int id)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я сниму Вашу бронь. Вам придет уведомление");
            var table = _tables.FirstOrDefault(t => t.Id == id && t.State == TableState.Booked);
            await Task.Delay(1000 * 5);
            return table?.SetState(TableState.Free);
        }
        public void CancelBookTable(int countOfPersons, int id)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я отменю вашу бронь. Оставайтесь на линии");
            var table = _tables.FirstOrDefault(t => t.Id == id && t.State == TableState.Booked);
            Thread.Sleep(1000 * 5);
            table?.SetState(TableState.Free);
        }
    }
}
