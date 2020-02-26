using NativeFramework;
using System.Threading.Tasks;

namespace pattern_wrapping.done.two
{
    public class TravelingCart
    {
        private readonly IGreetingServiceApi _greetingsApi;

        public TravelingCart(IGreetingServiceApi greetingsApi)
        {
            _greetingsApi = greetingsApi;
        }

        public async Task FadeFromBlack(params string[] prisonerNames)
        {
            foreach (var name in prisonerNames)
            {
                var prisoner = new Prisoner(name);
                await prisoner.SleepAsync();
                _greetingsApi.Say(prisoner, "Ah, you're finally awake");
            }
        }
    }
}