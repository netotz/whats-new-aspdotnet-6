using WiredBrainCoffee.Shared;

namespace WiredBrainCoffee.UI.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
    }
}