using DL;
using Models;
namespace BL;

public class SLBL : ISLBL
{
    private readonly IRepository _repo;

    public SLBL(IRepository repo)
    {
        _repo = repo;
    }

    public async Task<Customer> CreateCustomerAsync(Customer newCustomer)
    {
        return await _repo.CreateCustomerAsync(newCustomer);
    }

    public async Task<int> LoginCheckAsync(string cusName, string cusPass)
    {
        return await _repo.LoginCheckAsync(cusName, cusPass);
    }

    public async Task<Customer> GetCustomerAsync(string cusName)
    {
        return await _repo.GetCustomerAsync(cusName);
    }

    public async Task<Product> CreateProductAsync(Product newPro)
    {
        return await _repo.CreateProductAsync(newPro);
    }

    public Product GetProduct(int id)
    {
        return _repo.GetProduct(id);
    }

    public List<Inventory> GetInventory (int storeID)
    {
        return _repo.GetInventory(storeID);
    }

    public async Task<List<Store>> GetStoresAsync()
    {
        return await _repo.GetStoresAsync();
    }

    public Order UpdateOrders(Order updateOrder)
    {
        return _repo.UpdateOrders(updateOrder);
    }

    public void UpdateQuantityOrder(Cart cartItem, int storeID)
    {
        _repo.UpdateQuantityOrder(cartItem, storeID);
    }

    public Inventory UpdateQuantity(int newQuan, Inventory replenishPro, Store replenishStore)
    {
        return _repo.UpdateQuantity(newQuan, replenishPro, replenishStore);
    }

    public List<History> GetOrderHistory(Customer current)
    {
        return _repo.GetOrderHistory(current);
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _repo.GetAllProductsAsync();
    }
}