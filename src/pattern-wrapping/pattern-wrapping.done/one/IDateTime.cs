using System;

namespace pattern_wrapping.done.one
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }

    public class DateTimeAdapter : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}