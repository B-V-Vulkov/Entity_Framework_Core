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
                var result = DeleteProjectById(context);
                Console.WriteLine(result);
            }
        }

        public static string DeleteProjectById(SoftUniContext context) 
        {
            var project = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            var employeesProjects = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeesProjects);

            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                .Select(p => new
                {
                    p.Name
                })
                .Take(10);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var p in projects)
            {
                stringBuilder.AppendLine($"{p.Name}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
