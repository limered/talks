using NativeFramework;

namespace pattern_wrapping.done.two
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