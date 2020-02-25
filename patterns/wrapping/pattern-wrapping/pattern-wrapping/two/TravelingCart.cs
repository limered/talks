using System.Threading.Tasks;

namespace pattern_wrapping.two
{
    public class TravelingCart
    {
        public async Task FadeFromBlack(params string[] prisonerNames)
        {
            var greetingService = new GreetingsProvider.GreetingServiceApi();
            foreach (var name in prisonerNames)
            {
                var prisoner = new Prisoner(name);
                await prisoner.SleepAsync();
                greetingService.Say(prisoner, "Ah, you're finally awake");
            }
        }
    }
}