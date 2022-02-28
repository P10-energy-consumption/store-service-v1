using store_service_v1.Models;

namespace store_service_v1.Repositories.Interfaces
{
    public interface IStoreRepository
    {
        Task<List<InventoryLine>> GetInventory();
        Task<Order> GetOrder(int orderId);
        Task<int> PostOrder(Order order);
        Task DeleteOrder(int orderId);
    }
}
