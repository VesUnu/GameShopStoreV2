namespace GameShopStoreV2.Data.Entities
{
    public class RecommendSystemRequirement
    {
        public int SRRId { get; set; }
        public string OpSystem { get; set; } = null!;
        public string Processor { get; set; } = null!;
        public string Memory { get; set; } = null!;
        public string Graphics { get; set; } = null!;
        public string Storage { get; set; } = null!;
        public string AdditionalNotes { get; set; } = null!;
        public Game Game { get; set; } = null!;
        public int GameId { get; set; }
        public string Soundcard { get; set; } = null!;
    }
}