namespace VaporStore.DataProcessor
{

    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Data;
    using Data.Models.Enumerations;
    using ExportDtos;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            var genres = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .Select(g => new
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games.Select(game => new
                    {
                        Id = game.Id,
                        Title = game.Name,
                        Developer = game.Developer.Name,
                        Tags = string.Join(", ", game.GameTags.Select(t => t.Tag.Name)),
                        Players = game.Purchases.Count,
                    })
                    .Where(p => p.Players > 0)
                    .OrderByDescending(p => p.Players)
                    .ThenBy(p => p.Id)
                    .ToList(),
                    TotalPlayers = g.Games.Sum(p => p.Purchases.Count)
                })
                .OrderByDescending(g => g.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToList();

            var result = JsonConvert.SerializeObject(genres, Formatting.Indented);

            return result;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
            var typePurchases = Enum.Parse<PurchaseType>(storeType);

            var users = context.Users
                .Select(u => new ExportUserDto
                {
                    Username = u.Username,
                    Purchases = u.Cards
                    .Where(p => p.Purchases.Any(t => t.Type == typePurchases))
                    .SelectMany(pp => pp.Purchases)
                    .Select(p => new ExportPurchaseDto
                    {
                        Card = p.Card.Number,
                        Cvc = p.Card.Cvc,
                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new ExportGameDto
                        {
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price,
                        }
                    })
                        .OrderBy(d => d.Date)
                        .ToArray(),
                    TotalSpent = u.Cards.SelectMany(p => p.Purchases)
                    .Sum(p => p.Game.Price),
                })
                .Where(p => p.Purchases.Any())
                .OrderByDescending(t => t.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportUserDto[]), new XmlRootAttribute("Users"));

            var stringBuilder = new StringBuilder();

            xmlSerializer.Serialize(new StringWriter(stringBuilder), users);

            var result = stringBuilder.ToString().TrimEnd();

            return result;
        }
	}
}