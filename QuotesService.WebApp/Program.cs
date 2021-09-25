using CommonLibraries.Web;

namespace QuotesService.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ProgramUtils.RunWebhost<Startup>(args);
        }
    }
}
