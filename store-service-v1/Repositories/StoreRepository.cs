using Dapper;
using store_service_v1.Database.Interfaces;
using store_service_v1.Models;
using store_service_v1.Repositories.Interfaces;
using System.Linq;

namespace store_service_v1.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public StoreRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<InventoryLine>> GetInventory()
        {
            var sql = @" /* PetStore.Store.Api */
select p.Status, count(p.Id) from pets.pet p
where p.IsDelete = false
group by p.Status";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    var result = await _connection.QueryAsync<InventoryLine>(sql);
                    return result.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public async Task DeleteOrder(int orderId)
        {
            var sql = @" /* PetStore.Store.Api */
update orders.order set
    Deleted = current_timestamp,
    DeletedBy = 'PetStore.Store.Api',
    IsDelete = true
where id = @Id";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    await _connection.ExecuteAsync(sql, new { id = orderId });
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public async Task<int> PostOrder(Order order)
        {
            var sql = @" /* PetStore.Store.Api */
insert into orders.order (petid, quantity, shipdate, status, complete, created, createdby) 
values (@petid, @quantity, @shipdate, @status, @complete, current_timestamp, 'PetStore.Store.Api');
select currval('orders.order_id_seq');";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    var result = await _connection.ExecuteScalarAsync<int>(sql, order);
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public async Task<Order> GetOrder(int orderId)
        {
            var sql = @" /* PetStore.Store.Api */
select o.Id, o.Status, o.PetId, o.Quantity, o.ShipDate, o.Complete 
from orders.order o
where o.IsDelete = false
and o.id = @id";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    var result = await _connection.QuerySingleAsync<Order>(sql, new { id = orderId });
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
    }
}
