namespace SoftUni
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var result = GetEmployeesInPeriod(context);
                Console.WriteLine(result);
            }
        }

        public static string GetEmployeesInPeriod(SoftUniContext context) 
        {
            var employees = context.Employees
                .Where(e => e.EmployeesProjects.Any(p => 
                    p.Project.StartDate.Year >= 2001 &&
                    p.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    EmployeeFullName = $"{e.FirstName} {e.LastName}",
                    ManagerFullName = $"{e.Manager.FirstName} {e.Manager.LastName}",
                    Projects = e.EmployeesProjects.Select(p => new 
                    {
                        ProjectName = p.Project.Name,
                        StartDate = p.Project.StartDate,
                        EndDate = p.Project.EndDate
                    }).ToList()
                })
                .Take(10)
                .ToList();

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var employee in employees)
            {
                stringBuilder.AppendLine($"{employee.EmployeeFullName} - Manager: {employee.ManagerFullName}");

                foreach (var project in employee.Projects)
                {
                    string endDate = project.EndDate.HasValue
                        ? project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                        : "not finished";

                    stringBuilder.AppendLine($"--{project.ProjectName} - " +
                        $"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - " +
                        $"{endDate}");
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
