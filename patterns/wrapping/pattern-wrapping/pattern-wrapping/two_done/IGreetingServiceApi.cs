using System;
using System.Collections.Generic;
using System.Text;
using pattern_wrapping.two;
using pattern_wrapping.two.GreetingsProvider;

namespace pattern_wrapping.two_done
{
    public interface IGreetingServiceApi
    {
        void Say(Prisoner prisoner, string message);

    }

    public class GreetingServiceApiAdapter : IGreetingServiceApi
    {
        private readonly GreetingServiceApi _original;

        public GreetingServiceApiAdapter()
        {
            _original = new GreetingServiceApi();
        }

        public void Say(Prisoner prisoner, string message)
        {
            _original.Say(prisoner, message);
        }
    }
}
