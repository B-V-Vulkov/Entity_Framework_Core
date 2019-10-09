namespace SoftUni
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Data;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var result = IncreaseSalaries(context);
                Console.WriteLine(result);
            }
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = GetEmployeesByDepartments(context);

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }

            context.SaveChanges();

            employees = GetEmployeesByDepartments(context)
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var employee in employees)
            {
                stringBuilder.AppendLine($"{employee.FirstName} " +
                    $"{employee.LastName} " +
                    $"(${employee.Salary:f2})");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        private static List<Employee> GetEmployeesByDepartments(SoftUniContext context)
        {
            return context.Employees
                .Where(d =>
                    d.Department.Name == "Engineering" ||
                    d.Department.Name == "Tool Design" ||
                    d.Department.Name == "Marketing" ||
                    d.Department.Name == "Information Services")
                .ToList();
        }
    }
}
