namespace VaporStore.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres
                .Where(x => genreNames.Contains(x.Name))
                .ToArray()
                .Select(x => new ExportGenresDTO
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games
                    .Where(y => y.Purchases.Count > 0)
                    .Select(g => new ExportGamesDTO
                    {
                        Id = g.Id,
                        Title = g.Name,
                        Developer = g.Developer.Name,
                        Tags = string.Join(", ", g.GameTags.Select(y => y.Tag.Name)),
                        Players = g.Purchases.Count
                    })
                    .OrderByDescending(x => x.Players)
                    .ThenBy(x => x.Id)
                    .ToArray(),
                    TotalPlayers = x.Games.Sum(g => g.Purchases.Count)
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id);

            var result = JsonConvert.SerializeObject(genres, Formatting.Indented);

            return result;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var namespacees = new XmlSerializerNamespaces();
            namespacees.Add("", "");

            var users = context.Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == storeType)))
                .Select(u => new ExportUsersDTO
                {
                    Username = u.Username,
                    Purchases = u.Cards.SelectMany(c => c.Purchases.Where(p => p.Type.ToString() == storeType).Select(p => new ExportPurchasesDTO
                    {
                        CardNumber = c.Number,
                        Cvc = c.Cvc,
                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new ExportUserGamesDTO
                        {
                            Name = p.Game.Name,
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price
                        }
                    }))
                    .OrderBy(x => x.Date)
                    .ToArray(),
                    TotalSpent = u.Cards.SelectMany(p => p.Purchases.Where(p => p.Type.ToString() == storeType).Select(g => g.Game.Price)).Sum()
                })
                .OrderByDescending(x => x.TotalSpent)
                .ThenBy(x => x.Username)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportUsersDTO[]), new XmlRootAttribute("Users"));

            var writer = new StringWriter();

            serializer.Serialize(writer, users, namespacees);

            writer.Close();

            return writer.ToString();
        }
    }
}