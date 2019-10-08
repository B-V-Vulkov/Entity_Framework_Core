namespace SoftUni
{
    using Data;
    using System;
    using System.Text;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var resuult = GetEmployeesFullInformation(context);
                Console.WriteLine(resuult);
            }
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary,
                    e.EmployeeId
                })
                .OrderBy(x => x.EmployeeId);

            foreach (var employee in employees)
            {
                stringBuilder.AppendLine(
                    $"{employee.FirstName} " +
                    $"{employee.LastName} " +
                    $"{employee.MiddleName} " +
                    $"{employee.JobTitle} " +
                    $"{employee.Salary:f2}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
