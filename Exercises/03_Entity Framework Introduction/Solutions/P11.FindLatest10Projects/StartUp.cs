namespace SoftUni
{
    using System;
    using System.Linq;
    using System.Text;

    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var result = GetLatestProjects(context);
                Console.WriteLine(result);
            }
        }

        public static string GetLatestProjects(SoftUniContext context) 
        {
            var projects = context.Projects
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderByDescending(sd => sd.StartDate)
                .Take(10)
                .ToList()
                .OrderBy(n => n.Name);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var project in projects)
            {
                stringBuilder.AppendLine($"{project.Name}");
                stringBuilder.AppendLine($"{project.Description}");
                stringBuilder.AppendLine($"{project.StartDate}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
