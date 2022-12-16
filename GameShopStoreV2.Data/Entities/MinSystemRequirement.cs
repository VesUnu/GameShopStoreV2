namespace GameShopStoreV2.Data.Entities
{
    public class MinSystemRequirement
    {
        public int SRMId { get; set; }
        public string OpSystem { get; set; } = null!;
        public string Processor { get; set; } = null!;
        public string Memory { get; set; } = null!;
        public string Graphics { get; set; } = null!;
        public string Storage { get; set; } = null!;
        public string AdditionalNotes { get; set; } = null!;
        public Game Game { get; set; } = null!;
        public int GameID { get; set; }
        public string SoundCard { get; set; } = null!;
    }
}