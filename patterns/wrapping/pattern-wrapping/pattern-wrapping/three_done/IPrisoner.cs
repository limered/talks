using pattern_wrapping.two;
using System.Threading.Tasks;

namespace pattern_wrapping.three_done
{
    public interface IPrisoner
    {
        Prisoner Original { get; set; }
        string Name { get; set; }
        Task SleepAsync();
    }

    public class PrisonerWrapper : IPrisoner
    {
        public PrisonerWrapper(string name)
        {
            Original = new Prisoner(name);
        }

        public Prisoner Original { get; set; }

        public string Name
        {
            get => Original.Name;
            set => Original.Name = value;
        }

        public Task SleepAsync()
        {
            return Original.SleepAsync();
        }
    }
}