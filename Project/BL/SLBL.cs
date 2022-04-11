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

    public Customer CreateCustomer(Customer newCustomer)
    {
        return _repo.CreateCustomer(newCustomer);
    }

    public int LoginCheck(Customer login)
    {
        return _repo.LoginCheck(login);
    }

    public Customer GetCustomer(Customer cust)
    {
        return _repo.GetCustomer(cust);
    }

    public Product CreateProduct(Product newPro)
    {
        return _repo.CreateProduct(newPro);
    }

    public Product GetProduct(int id)
    {
        return _repo.GetProduct(id);
    }

    public List<Inventory> GetInventory (Store getInv)
    {
        return _repo.GetInventory(getInv);
    }

    public List<Store> GetStores()
    {
        return _repo.GetStores();
    }

    public Order UpdateOrders(Order updateOrder)
    {
        return _repo.UpdateOrders(updateOrder);
    }

    public void UpdateQuantityOrder(Cart cartItem, int storeID)
    {
        _repo.UpdateQuantityOrder(cartItem, storeID);
    }
}