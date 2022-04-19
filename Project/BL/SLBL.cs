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

    // public Product GetProduct(int id)
    // {
    //     return _repo.GetProduct(id);
    // }

    public async Task<List<Inventory>> GetInventoryAsync(int storeID)
    {
        return await _repo.GetInventoryAsync(storeID);
    }

    public async Task<List<Store>> GetStoresAsync()
    {
        return await _repo.GetStoresAsync();
    }

    public async Task<Order> UpdateOrdersAsync(Order updateOrder)
    {
        return await _repo.UpdateOrdersAsync(updateOrder);
    }

    public async Task UpdateQuantityOrderAsync(Cart cartItem, int storeID)
    {
        await _repo.UpdateQuantityOrderAsync(cartItem, storeID);
    }

    public async Task UpdateQuantityAsync(Inventory replenishPro, int storeID)
    {
        await _repo.UpdateQuantityAsync(replenishPro, storeID);
    }

    public async Task<List<History>> GetOrderHistoryAsync(int currentID)
    {
        return await _repo.GetOrderHistoryAsync(currentID);
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _repo.GetAllProductsAsync();
    }
    public async Task AddProductAsync(Inventory proToAdd, int storeID)
    {
        await _repo.AddProductAsync(proToAdd, storeID);
    }

}