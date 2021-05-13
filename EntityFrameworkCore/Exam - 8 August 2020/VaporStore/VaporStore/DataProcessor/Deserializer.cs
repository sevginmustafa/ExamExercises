namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var jsonGames = JsonConvert.DeserializeObject<ImportGameDTO[]>(jsonString);

            foreach (var item in jsonGames)
            {
                if (!IsValid(item) || item.Tags.Length == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }


                Game game = new Game
                {
                    Name = item.Name,
                    Price = item.Price,
                    ReleaseDate = DateTime.ParseExact(item.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                };

                Developer developer = context.Developers.FirstOrDefault(x => x.Name == item.Developer);
                if (developer == null)
                {
                    game.Developer = new Developer { Name = item.Developer };
                }
                else
                {
                    game.DeveloperId = developer.Id;
                }

                Genre genre = context.Genres.FirstOrDefault(x => x.Name == item.Genre);
                if (genre == null)
                {
                    game.Genre = new Genre { Name = item.Genre };
                }
                else
                {
                    game.GenreId = genre.Id;
                }

                foreach (var gameTag in item.Tags)
                {
                    if (!IsValid(gameTag))
                    {
                        sb.AppendLine("Invalid Data");
                        break;
                    }

                    Tag tag = context.Tags.FirstOrDefault(x => x.Name == gameTag);
                    if (tag == null)
                    {
                        game.GameTags.Add(new GameTag { Tag = new Tag { Name = gameTag } });
                    }
                    else
                    {
                        game.GameTags.Add(new GameTag { Tag = tag });
                    }
                }

                if (game.GameTags.Count == 0)
                {
                    continue;
                }

                context.Games.Add(game);

                context.SaveChanges();

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            return sb.ToString();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var jsonUsers = JsonConvert.DeserializeObject<ImportUserDTO[]>(jsonString);

            foreach (var item in jsonUsers)
            {
                if (!IsValid(item))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                User user = new User
                {
                    FullName = item.FullName,
                    Username = item.Username,
                    Email = item.Email,
                    Age = item.Age
                };

                foreach (var card in item.Cards)
                {
                    if (!IsValid(card) || !Enum.TryParse(card.Type, out CardType cardType))
                    {
                        sb.AppendLine("Invalid Data");
                        user.Cards.Clear();
                        break;
                    }

                    user.Cards.Add(new Card
                    {
                        Number = card.Number,
                        Cvc = card.Cvc,
                        Type = (CardType)Enum.Parse(typeof(CardType), card.Type)
                    });
                }

                if (user.Cards.Count == 0)
                {
                    continue;
                }

                context.Users.Add(user);

                sb.AppendLine($"Imported {item.Username} with {user.Cards.Count} cards");
            }

            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ImportPurchaseDTO[]), new XmlRootAttribute("Purchases"));

            var reader = new StringReader(xmlString);

            var xmlPurchases = (ImportPurchaseDTO[])serializer.Deserialize(reader);

            reader.Close();

            foreach (var item in xmlPurchases)
            {
                if (!IsValid(item) || !Enum.TryParse(item.Type, out PurchaseType purchaseType))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                context.Purchases.Add(new Purchase
                {
                    Game = context.Games.FirstOrDefault(x => x.Name == item.GameName),
                    Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), item.Type),
                    ProductKey = item.ProductKey,
                    Card = context.Cards.FirstOrDefault(x => x.Number == item.Card),
                    Date = DateTime.ParseExact(item.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                });

                var username = context.Users.FirstOrDefault(x => x.Cards.Any(x => x.Number == item.Card));

                sb.AppendLine($"Imported {item.GameName} for {username.Username}");
            }

            context.SaveChanges();

            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}