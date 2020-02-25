using System.Threading.Tasks;
using pattern_wrapping.two_done;

namespace pattern_wrapping.three_done
{
    public class TravelingCart
    {
        private readonly IGreetingServiceApi _greetingsApi;
        private readonly IPrisoner _prisoner;

        public TravelingCart(IGreetingServiceApi greetingsApi, 
            IPrisoner prisoner)
        {
            _greetingsApi = greetingsApi;
            _prisoner = prisoner;
        }

        public async Task FadeFromBlack(params string[] prisonerNames)
        {
            foreach (var name in prisonerNames)
            {
                _prisoner.Name = name;
                await _prisoner.SleepAsync();
                _greetingsApi.Say(_prisoner.Original, "Ah, you're finally awake");
            }
        }
    }
}