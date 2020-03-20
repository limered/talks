using System.Threading.Tasks;
using NativeFramework;

namespace pattern_wrapping.two
{
    public class TravelingCart
    {
        public async Task StartGame(params string[] prisonerNames)
        {
            var greetingService = new GreetingServiceApi();
            foreach (var name in prisonerNames)
            {
                var prisoner = new Prisoner(name);
                await prisoner.SleepAsync();
                greetingService.Say(prisoner, "Hey, you. You're finally awake.");
            }
        }
    }
}