using Microsoft.AspNetCore.Mvc;
using store_service_v1.Models;
using store_service_v1.Repositories.Interfaces;

namespace store_service_v1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;

        public StoreController(IStoreRepository petRepository)
        {
            _storeRepository = petRepository;
        }

        [HttpGet("/v1/store/inventory")]
        public async Task<IActionResult> GetInventory()
        {
            var result = await _storeRepository.GetInventory();
            return Ok(result);
        }

        [HttpGet("/v1/store/order/{orderId}")]
        public async Task<IActionResult> GetOrders(int orderId)
        {
            var result = await _storeRepository.GetOrder(orderId);
            return Ok(result);
        }

        [HttpDelete("/v1/store/order/{orderId}")]
        public async Task<ActionResult> Delete(int orderId)
        {
            await _storeRepository.DeleteOrder(orderId);
            return Ok(orderId);
        }

        [HttpPost("/v1/store/order")]
        public async Task<ActionResult> Place([FromBody] Order order)
        {
            if (order != null)
            {
                var orderId = await _storeRepository.PostOrder(order);
                var result = await _storeRepository.GetOrder(orderId);

                return Ok(result);
            }
            return StatusCode(400);
        }
    }
}