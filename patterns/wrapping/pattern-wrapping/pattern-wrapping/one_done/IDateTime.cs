using System;

namespace pattern_wrapping.one_done
{
    public interface IDateTime
    {
        DateTime Now();
    }

    public class DateTimeWrapper : IDateTime
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}