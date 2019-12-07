namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2:f2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDto = JsonConvert.DeserializeObject<ImportMovieDto[]>(jsonString);

            var movies = new List<Movie>();

            var sb = new StringBuilder();

            foreach (var movieDto in moviesDto)
            {
                if (!IsValid(movieDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                movies.Add(new Movie
                {
                    Title = movieDto.Title,
                    Genre = Enum.Parse<Genre>(movieDto.Genre),
                    Duration = TimeSpan.ParseExact(movieDto.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = movieDto.Rating,
                    Director = movieDto.Director
                });

                sb.AppendLine(string.Format(SuccessfulImportMovie,movieDto.Title, movieDto.Genre, movieDto.Rating));
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsDto = JsonConvert.DeserializeObject<ImportHallDto[]>(jsonString);

            var halls = new List<Hall>();
            var seats = new List<Seat>();

            var sb = new StringBuilder();

            foreach (var hallDto in hallsDto)
            {
                if (!IsValid(hallDto) || hallDto.Seats < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = (new Hall
                {
                    Name = hallDto.Name,
                    Is4Dx = hallDto.Is4Dx,
                    Is3D = hallDto.Is3D,
                });

                for (int i = 0; i < hallDto.Seats; i++)
                {
                    hall.Seats.Add(new Seat());
                }

                halls.Add(hall);

                string projectionType = "Normal";

                if (hall.Is3D && !hall.Is4Dx)
                {
                    projectionType = "3D";
                }
                else if (!hall.Is3D && hall.Is4Dx)
                {
                    projectionType = "4Dx";
                }
                else if (hall.Is3D && hall.Is4Dx)
                {
                    projectionType = "4Dx/3D";
                }

                sb.AppendLine(string.Format(SuccessfulImportHallSeat, 
                    hall.Name, projectionType, hallDto.Seats));
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportProjectionDto[]), new XmlRootAttribute("Projections"));
            var projectionsDtos = (ImportProjectionDto[])serializer.Deserialize(new StringReader(xmlString));

            var projections = new List<Projection>();

            var sb = new StringBuilder();

            foreach (var projectionDto in projectionsDtos)
            {
                if (!IsValid(projectionDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = context.Movies
                    .FirstOrDefault(x => x.Id == projectionDto.MovieId);

                var hall = context.Halls
                    .FirstOrDefault(x => x.Id == projectionDto.HallId);

                if (movie == null || hall ==  null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = (new Projection
                {
                    Movie = movie,
                    Hall = hall,
                    DateTime = DateTime.ParseExact(projectionDto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                });

                projections.Add(projection);

                sb.AppendLine(string.Format(SuccessfulImportProjection,
                    movie.Title, projection.DateTime.ToString("MM/dd/yyyy")));
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            var customersDto = (ImportCustomerDto[])serializer.Deserialize(new StringReader(xmlString));

            var customers = new List<Customer>();

            var sb = new StringBuilder();

            foreach (var customerDto in customersDto)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isCustomerValid = true;

                var tickets = new List<Ticket>();

                foreach (var ticketDto in customerDto.Tickets)
                {
                    var projection = context.Projections
                        .FirstOrDefault(x => x.Id == ticketDto.ProjectionId);

                    tickets.Add(new Ticket
                    {
                        Projection = projection,
                        Price = ticketDto.Price,
                    });

                    if (projection == null)
                    {
                        isCustomerValid = false;
                        sb.AppendLine(ErrorMessage);
                        break;
                    }
                }

                if (isCustomerValid)
                {
                    customers.Add(new Customer
                    {
                        FirstName = customerDto.FirstName,
                        LastName = customerDto.LastName,
                        Age = customerDto.Age,
                        Balance = customerDto.Balance,
                        Tickets = tickets
                    });
                }

                sb.AppendLine(string.Format(SuccessfulImportCustomerTicket,
                    customerDto.FirstName, customerDto.LastName, tickets.Count));
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }
    }
}