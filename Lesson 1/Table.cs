using System;


namespace Lesson_1
{
    public class Table
    {
        public TableState State { get; private set; }
        public int SeatsCount { get; }
        public int Id { get; }
        public Table(int id)
        {
            Id = id;
            State = TableState.Free;
            SeatsCount = new Random().Next(2, 5);
        }
        public bool SetState(TableState state)
        {

            if (state == State)
                return false;
            State = state;
            return true;

        }
    }
}
