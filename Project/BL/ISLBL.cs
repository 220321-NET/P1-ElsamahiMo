using Models;
namespace BL;

public interface ISLBL 
{
    Task<Customer> CreateCustomerAsync(Customer newCustomer);
    Task<int> LoginCheckAsync(string cusName, string cusPass);
    Task<Customer> GetCustomerAsync(string cusName);
    Task<Product> CreateProductAsync(Product newPro);
    // Product GetProduct(int id);
    Task<List<Inventory>> GetInventoryAsync(int storeID);
    Task<List<Store>> GetStoresAsync();
    Task<Order> UpdateOrdersAsync(Order updateOrder);
    Task UpdateQuantityOrderAsync(Cart cartItem, int storeID);
    Task UpdateQuantityAsync(Inventory replenishPro, int storeID);
    Task<List<History>> GetOrderHistoryAsync(int currentID);
    Task<List<Product>> GetAllProductsAsync();
    Task AddProductAsync(Inventory proToAdd, int storeID);
}