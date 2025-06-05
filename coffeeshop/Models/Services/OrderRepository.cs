using coffeeshop.Data;
using coffeeshop.Models.Interface;
using coffeeshop.Models.Interfaces;


namespace coffeeshop.Models.Services
{
    public class OrderRepository : IOrderRepository
    {
        private CoffeeshopDbContext DbContext;
        private IShoppingCartRepository ShoppingCartRepository;

        public OrderRepository(CoffeeshopDbContext DbContext, IShoppingCartRepository shoppingCartRepository)
        {
            this.DbContext = DbContext;
            this.ShoppingCartRepository = shoppingCartRepository;
        }

        public void PlaceOrder(Order order)
        {
            var shoppingCartItems = ShoppingCartRepository.GetAllShoppingCartItems();
            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = item.Qty,
                    ProductId = item.Product.Id,
                    Price = item.Product.Price
                };

                order.OrderDetails.Add(orderDetail);
            }

            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = ShoppingCartRepository.GetShoppingCartTotal();
            DbContext.Orders.Add(order);
            DbContext.SaveChanges();
        }
    }
}
