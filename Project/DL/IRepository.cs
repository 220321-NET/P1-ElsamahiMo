using Models;

namespace DL;

public interface IRepository
{
    Task<Customer> CreateCustomerAsync(Customer newCustomer);
    Task<int> LoginCheckAsync (string cusName, string cusPass);
    Task<Customer> GetCustomerAsync(string cusName);
    Task<Product> CreateProductAsync(Product newPro);
    Product GetProduct(int id);
    List<Inventory> GetInventory(int storeID);
    Task<List<Store>> GetStoresAsync();
    Order UpdateOrders(Order updateOrder);
    void UpdateQuantityOrder(Cart cartItem, int storeID);
    Inventory UpdateQuantity(int newQuan, Inventory replenishPro, Store replenishStore);
    List<History> GetOrderHistory(Customer current);
    Task<List<Product>> GetAllProductsAsync();
}