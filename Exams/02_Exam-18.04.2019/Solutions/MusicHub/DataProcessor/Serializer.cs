namespace MusicHub.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(x => x.ProducerId == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new 
                    {
                        SongName = s.Name,
                        Price = $"{s.Price:f2}",
                        Writer = s.Writer.Name
                    })
                    .OrderByDescending(x => x.SongName)
                    .ThenBy(x => x.Writer)
                    .ToArray(),
                    AlbumPrice = $"{a.Songs.Sum(p => p.Price):f2}",
                })
                .OrderByDescending(x => decimal.Parse(x.AlbumPrice))
                .ToArray();

            var result = JsonConvert.SerializeObject(albums, Formatting.Indented);

            return result;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}