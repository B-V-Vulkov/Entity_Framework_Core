namespace P03_FootballBetting
{
    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics;

    using Data;

    public class StartUp
    {
        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            System.Console.WriteLine(stopWatch.Elapsed);

            using var db = new FootballBettingContext();
            db.Database.Migrate();

            System.Console.WriteLine(stopWatch.Elapsed);
        }
    }
}
