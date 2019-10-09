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
                var result = GetEmployeesByFirstNameStartingWithSa(context);
                Console.WriteLine(result);
            }
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var employee in employees)
            {
                stringBuilder.AppendLine($"{employee.FirstName} " +
                    $"{employee.LastName} - " +
                    $"{employee.JobTitle} - " +
                    $"(${employee.Salary:f2})");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
