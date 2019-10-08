namespace SoftUni
{
    using System;
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
                var result = AddNewAddressToEmployee(context);
                Console.WriteLine(result);
            }
        }

        public static string AddNewAddressToEmployee(SoftUniContext context) 
        {
            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(address);

            var nakov = context.Employees
                .FirstOrDefault(n => n.LastName == "Nakov");

            nakov.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .Select(e => new
                {
                    e.AddressId,
                    e.Address.AddressText
                })
                .OrderByDescending(e => e.AddressId)
                .Take(10);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var employee in employees)
            {
                stringBuilder.AppendLine(employee.AddressText);
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
