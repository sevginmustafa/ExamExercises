namespace VaporStore.DataProcessor.Dto.Export
{
    public class ExportGenresDTO
    {
        public int Id { get; set; }

        public string Genre { get; set; }

        public ExportGamesDTO[] Games { get; set; }

        public int TotalPlayers { get; set; }
    }
}
