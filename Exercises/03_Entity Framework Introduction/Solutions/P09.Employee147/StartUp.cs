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
                var result = GetEmployee147(context);
                Console.WriteLine(result);
            }
        }

        public static string GetEmployee147(SoftUniContext context) 
        {
            var employee147 = context.Employees
                .Select(e => new 
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    projects = e.EmployeesProjects
                    .Select(p => new 
                    {
                        p.Project.Name
                    })
                    .OrderBy(p => p.Name)
                })
                .FirstOrDefault(id => id.EmployeeId == 147);

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{employee147.FirstName} " +
                $"{employee147.LastName} - " +
                $"{employee147.JobTitle}");

            foreach (var project in employee147.projects)
            {
                stringBuilder.AppendLine($"{project.Name}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
