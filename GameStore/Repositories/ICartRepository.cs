namespace GameStore.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int gameId, int qty, string userId);
        Task<int> RemoveItem(int gameId, string userId);
        Task<ShoppingCart> GetUserCart(string userId);
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckout(string userId);
    }
}
