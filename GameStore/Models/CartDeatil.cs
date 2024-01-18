namespace GameStore.Models
{
    public class CartDeatil
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public int GameId { get; set; }

        public int Quantity { get; set; }
        public Game Game { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
