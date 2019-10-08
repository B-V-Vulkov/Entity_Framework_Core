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
                var result = GetEmployeesFromResearchAndDevelopment(context);
                Console.WriteLine(result);
            }
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context) 
        {
            StringBuilder stringBuilder = new StringBuilder();

            var employees = context.Employees
                .Where(d => d.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName);

            foreach (var employee in employees)
            {
                stringBuilder.AppendLine($"{employee.FirstName} " +
                    $"{employee.LastName} from " +
                    $"{employee.DepartmentName} - " +
                    $"${employee.Salary:f2}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
