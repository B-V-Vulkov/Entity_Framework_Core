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
                var result = GetDepartmentsWithMoreThan5Employees(context);
                Console.WriteLine(result);
            }
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context) 
        {
            var departments = context.Departments
                .Where(e => e.Employees.Count() > 5)
                .Select(d => new
                {
                    CountOfEmployees = d.Employees.Count(),
                    DepartmentName = d.Name,
                    ManagerFullName = $"{d.Manager.FirstName} {d.Manager.LastName}",
                    Employees = d.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(n => n.FirstName)
                    .ThenBy(n => n.LastName)
                    .ToList()
                })
                .OrderBy(c => c.CountOfEmployees)
                .ThenBy(d => d.DepartmentName)
                .Take(5)
                .ToList();

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var department in departments)
            {
                stringBuilder.AppendLine($"{department.DepartmentName} - " +
                    $"{department.ManagerFullName}");

                foreach (var employee in department.Employees)
                {
                    stringBuilder.AppendLine($"{employee.FirstName} " +
                        $"{employee.LastName} - " +
                        $"{employee.JobTitle}");
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
