using CommonLibraries.Web;

namespace QuotesService.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ProgramUtils.RunWebhost<Startup>(args);
        }
    }
}
