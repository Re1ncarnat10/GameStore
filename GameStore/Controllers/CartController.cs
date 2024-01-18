using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GameStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(ICartRepository cartRepo, UserManager<IdentityUser> userManager)
        {
            _cartRepo = cartRepo;
            _userManager = userManager;
        }
        public async Task<IActionResult> AddItem(int gameId, int qty = 1, int redirect = 0)
        {
            string userId = _userManager.GetUserId(User);
            var cartCount = await _cartRepo.AddItem(gameId, qty, userId);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int gameId)
        {
            string userId = _userManager.GetUserId(User);
            var cartCount = await _cartRepo.RemoveItem(gameId, userId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            string userId = _userManager.GetUserId(User);
            var cart = await _cartRepo.GetUserCart(userId);
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            string userId = _userManager.GetUserId(User);
            int cartItem = await _cartRepo.GetCartItemCount(userId);
            return Ok(cartItem);
        }

        public async Task<IActionResult> Checkout()
        {
            string userId = _userManager.GetUserId(User);
            bool isCheckedOut = await _cartRepo.DoCheckout(userId);
            if (!isCheckedOut)
                throw new Exception("Server side error");
            return RedirectToAction("Index", "Home");
        }

    }
}
