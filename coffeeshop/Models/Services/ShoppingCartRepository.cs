using coffeeshop.Data;
using coffeeshop.Models.Interface;
using coffeeshop.Models;
using Microsoft.EntityFrameworkCore;


namespace coffeeshop.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private CoffeeshopDbContext DbContext;

        public ShoppingCartRepository(CoffeeshopDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public List<ShoppingCartItem>? shopingcartItems { get; set; }
        public string? ShoppingCartId { get; set; }

        public static ShoppingCartRepository GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            CoffeeshopDbContext context = services.GetService<CoffeeshopDbContext>()
                ?? throw new Exception("Error initializing CoffeeshopDbContext");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);

            return new ShoppingCartRepository(context)
            {
                ShoppingCartId = cartId
            };
        }

        public void AddToCart(Product product)
        {
            var shoppingCartItem = DbContext.ShoppingCartItems.FirstOrDefault(s =>
                s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Qty = 1,
                };
                DbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Qty++;
            }

            DbContext.SaveChanges();
        }

        public void ClearCart()
        {
            var cartItems = DbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId);

            DbContext.ShoppingCartItems.RemoveRange(cartItems);
            DbContext.SaveChanges();
        }

        public List<ShoppingCartItem> GetAllShoppingCartItems()
        {
            return shopingcartItems ??= DbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId)
                .Include(p => p.Product)
                .ToList();
        }

        public decimal GetShoppingCartTotal()
        {
            var totalCost = DbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId)
                .Select(s => s.Product.Price * s.Qty)
                .Sum();

            return totalCost;
        }

        public int RemoveFromCart(Product product)
        {
            var shoppingCartItem = DbContext.ShoppingCartItems.FirstOrDefault(s =>
                s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);

            var quantity = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Qty > 1)
                {
                    shoppingCartItem.Qty--;
                    quantity = shoppingCartItem.Qty;
                }
                else
                {
                    DbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            DbContext.SaveChanges();
            return quantity;
        }
    }
}