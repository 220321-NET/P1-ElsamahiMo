using Microsoft.AspNetCore.Mvc;
using BL;
using Models;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ISLBL _bl;
        public ItemsController(ISLBL bl)
        {
            _bl = bl;
        }

        [HttpGet("GetLogin/{cusName}/{cusPass}")]
        public async Task<int> LoginCheckAsync(string cusName, string cusPass)
        {
            return await _bl.LoginCheckAsync(cusName, cusPass);
        }

        [HttpGet("GetCustomer/{cusName}")]
        public async Task<Customer> GetCustomerAsync(string cusName)
        {
            return await _bl.GetCustomerAsync(cusName);
        }

        // GET: api/<ItemsController>
        [HttpGet("GetAllProducts")]
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _bl.GetAllProductsAsync();
        }

        [HttpGet("GetAllStores")]
        public async Task<List<Store>> GetStoresAsync()
        {
            return await _bl.GetStoresAsync();
        }

        [HttpGet("GetInventory/{storeID}")]
        public async Task<List<Inventory>> GetInventoryAsync(int storeID)
        {
            return await _bl.GetInventoryAsync(storeID);
        }

        [HttpGet("GetHistory/{custID}")]
        public async Task<List<History>> GetHistoryAsync(int custID)
        {
            return await _bl.GetOrderHistoryAsync(custID);

        }

        //PUT api/<ItemsController>

        [HttpPut("UpdateQuantity/{storeID}")]
        public async Task UpdateQuantity(Inventory replenishPro, int storeID)
        {   
            await _bl.UpdateQuantityAsync(replenishPro, storeID);
        }

        [HttpPut("UpdateQuantityOrder/{storeID}")]
        public async Task UpdateQuantityOrderAsync([FromBody]Cart cartItem, int storeID)
        {
            await _bl.UpdateQuantityOrderAsync(cartItem, storeID);
        }


        // POST api/<ItemsController>
        [HttpPost("CreateCustomer")]
        public async Task<Customer> CreateCustomerAsync([FromBody] Customer currentCreate)
        {
            return await _bl.CreateCustomerAsync(currentCreate);
        }

        [HttpPost("CreateProduct")]
        public async Task<Product> CreateProduct([FromBody]Product proCreate)
        {
            return await _bl.CreateProductAsync(proCreate);
        }

        [HttpPost("CreateOrder")]
        public async Task<Order> CreateOrderAsync([FromBody] Order orderToCreate)
        {
            return await _bl.UpdateOrdersAsync(orderToCreate);
        }

        [HttpPost("AddProduct/{storeID}")]
        public async Task AddProductAsync(Inventory proToAdd, int storeID)
        {
            await _bl.AddProductAsync(proToAdd, storeID);
        }

        
    }
}
