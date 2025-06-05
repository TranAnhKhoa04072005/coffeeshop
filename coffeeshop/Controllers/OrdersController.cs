using coffeeshop.Models.Interface;
using coffeeshop.Models.Interfaces;
using coffeeshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace coffeeshop.Controllers
{
    public class OrdersController : Controller
    {
        private IOrderRepository OrderRepository;
        private IShoppingCartRepository shoppingCartRepository;
        public OrdersController(IOrderRepository oderRepository,
        IShoppingCartRepository shoppingCartRepossitory)
        {
            this.OrderRepository = oderRepository;
            this.shoppingCartRepository = shoppingCartRepossitory;
        }

        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            OrderRepository.PlaceOrder(order);
            shoppingCartRepository.ClearCart();
            HttpContext.Session.SetInt32("CartCount", 0);

            return RedirectToAction("CheckoutComplete");
        }

        public IActionResult CheckoutComplete()
        {
            return View();
        }
    }
}
