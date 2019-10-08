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
                var result = GetAddressesByTown(context);
                Console.WriteLine(result);
            }
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count(),
                })
                .OrderByDescending(c => c.EmployeeCount)
                .ThenBy(t => t.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var address in addresses)
            {
                stringBuilder.AppendLine($"{address.AddressText}, " +
                    $"{address.TownName} - " +
                    $"{address.EmployeeCount} employees");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
