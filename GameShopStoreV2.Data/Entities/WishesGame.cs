namespace GameShopStoreV2.Data.Entities
{
    public class WishesGame
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public Wishlist Wishlist { get; set; } = null!;
        public int WishID { get; set; }
        public int GameID { get; set; }
        public DateTime DateAdded { get; set; }
    }
}