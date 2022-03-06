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
            var result = new List<InventoryLine>();
            var sql = @"select Status, count(Id) AS 'Count' from pets
                    where IsDelete = 0
                    group by Status";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    result = (await _connection.QueryAsync<InventoryLine>(sql)).ToList();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }

                return result;
            }
        }

        public async Task<int> DeleteOrder(int orderId)
        {
            var result = -1;
            var sql = @"delete from orders where id = @Id";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    result = await _connection.ExecuteAsync(sql, new { id = orderId });
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

            return result;
        }

        public async Task<Order> PostOrder(Order order)
        {
            var sql = @" /* PetStore.Store.Api */
insert into orders (id, petid, quantity, shipdate, status, complete, created, createdby) 
OUTPUT Inserted.ID
values (@id, @petid, @quantity, @shipdate, @status, @complete, current_timestamp, 'PetStore.Store.Api');";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    await _connection.ExecuteScalarAsync<int>(sql, order);
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

            return order;
        }

        public async Task<Order> GetOrder(int orderId)
        {
            var sql = @"select Id, Status, PetId, Quantity, ShipDate, Complete 
                        from orders
                        where IsDelete = 0
                        and id = @id";

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
