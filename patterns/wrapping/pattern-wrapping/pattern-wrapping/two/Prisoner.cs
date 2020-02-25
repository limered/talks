using System.Threading.Tasks;

namespace pattern_wrapping.two
{
    public class Prisoner
    {
        public string Name { get; set; }
        public Prisoner(string name)
        {
            Name = name;
        }

        public async Task SleepAsync()
        {
            await Task.FromResult(0);
        }
    }
}