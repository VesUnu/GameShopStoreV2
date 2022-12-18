namespace GameShopStoreV2.Data.Entities
{
    public class WishesGame
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public Wishlist Wishlist { get; set; } = null!;
        public int WishId { get; set; }
        public int GameId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}