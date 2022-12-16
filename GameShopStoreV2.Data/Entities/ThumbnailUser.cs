namespace GameShopStoreV2.Data.Entities
{
    public class ThumbnailUser
    {
        public int ImageId { get; set; }
        public AppUser AppUser { get; set; } = null!;
        public Guid UserID { get; set; }
        public DateTime DateUpdate { get; set; }
        public string ImagePath { get; set; } = null!;
    }
}