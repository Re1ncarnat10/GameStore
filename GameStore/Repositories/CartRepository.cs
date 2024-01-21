using GameStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameStore.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _userId = GetUserId();
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

        public async Task<int> AddItem(int gameId, int qty)
        {
            if (string.IsNullOrEmpty(_userId))
                throw new Exception("User is not logged-in");

            using var transaction = _db.Database.BeginTransaction();
            try
            {
                var cart = await GetOrCreateCart(_userId);
                await AddOrUpdateCartItem(gameId, qty, cart);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw;
            }
            var cartItemCount = await GetCartItemCount(_userId);
            return cartItemCount;
        }

        private async Task<ShoppingCart> GetOrCreateCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId
                };
                _db.ShoppingCarts.Add(cart);
                await _db.SaveChangesAsync();
            }
            return cart;
        }

        private async Task AddOrUpdateCartItem(int gameId, int qty, ShoppingCart cart)
        {
            var cartItem = await _db.CartDetails
                                .FirstOrDefaultAsync(a => a.ShoppingCartId == cart.Id && a.GameId == gameId);
            if (cartItem is not null)
            {
                cartItem.Quantity += qty;
            }
            else
            {
                var game = await _db.Games.FindAsync(gameId);
                cartItem = new CartDetails
                {
                    GameId = gameId,
                    ShoppingCartId = cart.Id,
                    Quantity = qty,
                    Price = game.Price  // it is a new line after update
                };
                _db.CartDetails.Add(cartItem);
            }
            await _db.SaveChangesAsync();
        }

        public async Task<int> RemoveItem(int gameId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.GameId == gameId);
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw;
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Game)
                                  .ThenInclude(a => a.Genre)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.ShoppingCarts
                              join cartDetail in _db.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == userId
                              select cartDetail.Quantity
                        ).SumAsync();
            return data;
        }


        public async Task<bool> DoCheckout()
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _db.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    OrderStatusId = 1//pending
                };
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        GameId = item.GameId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    _db.OrderDetails.Add(orderDetail);
                }
                await _db.SaveChangesAsync();

                // removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                // Log exception here
                throw;
            }
        }
        public async Task ClearCart()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var cart = await GetCart(userId);
            if (cart is null)
                throw new Exception("Invalid cart");
            var cartDetail = _db.CartDetails
                                .Where(a => a.ShoppingCartId == cart.Id).ToList();
            if (cartDetail.Count > 0)
            {
                _db.CartDetails.RemoveRange(cartDetail);
                await _db.SaveChangesAsync();
            }
        }
    }
}
