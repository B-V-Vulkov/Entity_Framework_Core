namespace MusicHub.DataProcessor
{
    using Data;
    using Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage 
            = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var writersDto = JsonConvert.DeserializeObject<ImportWriterDto[]>(jsonString);

            var writers = new List<Writer>();

            var sb = new StringBuilder();

            foreach (var writerDto in writersDto)
            {
                if (!IsValid(writerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                writers.Add(new Writer
                {
                    Name = writerDto.Name,
                    Pseudonym = writerDto.Pseudonym,
                });

                sb.AppendLine($"Imported {writerDto.Name}");
            }

            context.Writers.AddRange(writers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var ProducersDto = JsonConvert.DeserializeObject<ImportProducersDto[]>(jsonString);

            var producers = new List<Producer>();
            var albums = new List<Album>();

            var sb = new StringBuilder();

            foreach (var producerDto in ProducersDto)
            {
                if (!IsValid(producerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool albumsAreValid = true;

                foreach (var albumDto in producerDto.Albums)
                {
                    if (!IsValid(albumDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        albumsAreValid = false;
                        break;
                    }
                }

                if (albumsAreValid)
                {
                    var producer = (new Producer
                    {
                        Name = producerDto.Name,
                        Pseudonym = producerDto.Pseudonym,
                        PhoneNumber = producerDto.PhoneNumber,
                    });

                    foreach (var albumDto in producerDto.Albums)
                    {
                        albums.Add(new Album
                        {
                            Name = albumDto.Name,
                            ReleaseDate = DateTime.ParseExact(albumDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Producer = producer,
                        });
                    }

                    producers.Add(producer);

                    if (producer.PhoneNumber == null)
                    {
                        sb.AppendLine($"Imported {producerDto.Name} with no phone number produces {producerDto.Albums.Count} albums");
                    }
                    else
                    {
                        sb.AppendLine($"Imported {producerDto.Name} with phone: {producerDto.PhoneNumber} produces {producerDto.Albums.Count} albums");
                    }
                }
            }

            context.Producers.AddRange(producers);
            context.Albums.AddRange(albums);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportSongDto[]), new XmlRootAttribute("Songs"));
            var songsDtos = (ImportSongDto[])serializer.Deserialize(new StringReader(xmlString));

            var songs = new List<Song>();

            var sb = new StringBuilder();

            foreach (var songDto in songsDtos)
            {
                if (!IsValid(songDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var album = context.Albums
                    .FirstOrDefault(x => x.Id == songDto.AlbumId);

                var writer = context.Writers
                    .FirstOrDefault(x => x.Id == songDto.WriterId);

                bool genreIsValid = Enum.TryParse(songDto.Genre, out Genre generResult);

                if (!genreIsValid || album == null || writer == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                songs.Add(new Song
                {
                    Name = songDto.Name,
                    Duration = TimeSpan.ParseExact(songDto.Duration, "c", CultureInfo.InvariantCulture),
                    CreatedOn = DateTime.ParseExact(songDto.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Genre = generResult,
                    Album = album,
                    Writer = writer,
                    Price = songDto.Price,
                });

                sb.AppendLine($"Imported {songDto.Name} ({songDto.Genre} genre) with duration {songDto.Duration}");
            }

            context.Songs.AddRange(songs);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportPerformerDto[]), new XmlRootAttribute("Performers"));
            var performersDto = (ImportPerformerDto[])serializer.Deserialize(new StringReader(xmlString));

            var performers = new List<Performer>();
            var songPerformers = new List<SongPerformer>();

            var sb = new StringBuilder();

            foreach (var performerDto in performersDto)
            {
                if (!IsValid(performerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validSongsCount = context.Songs.Count(s => performerDto.PerformersSongs.Any(i => i.Id == s.Id));

                if (validSongsCount != performerDto.PerformersSongs.Length)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var performer = (new Performer
                {
                    FirstName = performerDto.FirstName,
                    LastName = performerDto.LastName,
                    Age = performerDto.Age,
                    NetWorth = performerDto.NetWorth,
                });

                foreach (var song in performerDto.PerformersSongs)
                {
                    songPerformers.Add(new SongPerformer
                    {
                        Performer = performer,
                        SongId = song.Id,
                    });
                }

                performers.Add(performer);

                sb.AppendLine($"Imported {performer.FirstName} ({performerDto.PerformersSongs.Length} songs)");
            }

            context.Performers.AddRange(performers);
            context.SongsPerformers.AddRange(songPerformers);
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