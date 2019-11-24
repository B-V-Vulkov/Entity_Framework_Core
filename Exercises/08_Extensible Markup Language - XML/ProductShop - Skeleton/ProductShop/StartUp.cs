namespace ProductShop
{
    using System;
    using System.IO;
    using System.Xml.Linq;

    using Data;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            string fileName = "products.xml";
            string directory = "Datasets";
            string filePath = Path.Combine(directory, fileName);

            using (var context = new ProductShopContext())
            {
                var inputXml = File.ReadAllText(filePath);

                Console.WriteLine(ImportProducts(context, inputXml));
            }
        }

        //01.Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var document = XDocument.Parse(inputXml);

            foreach (var element in document.Root.Elements())
            {
                var user = new User
                {
                    FirstName = element.Element("firstName").Value,
                    LastName = element.Element("lastName").Value,
                    Age = int.Parse(element.Element("age").Value),
                };

                context.Users.Add(user);
            }

            int saved = context.SaveChanges();

            return $"Successfully imported {saved}";
        }

        //02.Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var document = XDocument.Parse(inputXml);

            foreach (var element in document.Root.Elements())
            {
                var product = new Product
                {
                    Name = element.Element("name").Value,
                    Price = decimal.Parse(element.Element("price").Value),
                    SellerId = int.Parse(element.Element("sellerId").Value),
                };

                context.Products.Add(product);
            }

            int saved = context.SaveChanges();

            return $"Successfully imported {saved}";
        }
    }
}