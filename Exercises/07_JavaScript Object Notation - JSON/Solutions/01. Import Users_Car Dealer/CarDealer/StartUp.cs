namespace CarDealer
{
    using System;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    using Data;
    using Models;
    using DTO;
    using System.Collections.Generic;

    public class StartUp
    {
        public static void Main()
        {
            string fileName = "cars.json";
            string directory = "Datasets";
            string filePath = Path.Combine(directory, fileName);

            using (var context = new CarDealerContext())
            {
                var inputJson = File.ReadAllText(filePath);
                Console.WriteLine(ImportCars(context, inputJson));
            }
        }

        //09.Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        //10.Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
                .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);
            var saved = context.SaveChanges();

            return $"Successfully imported {saved}.";
        }

        //11.Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDto = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);

            var cars = new List<Car>();
            var partsCars = new List<PartCar>();

            foreach (var carDto in carsDto)
            {
                var partCars = new List<PartCar>();

                var car = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance,
                };

                foreach (var part in carDto.PartsId.Distinct())
                {
                    var partCar = new PartCar()
                    {
                        PartId = part,
                        Car = car,
                    };

                    partsCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.AddRange(cars);
            context.AddRange(partsCars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }
    }
}