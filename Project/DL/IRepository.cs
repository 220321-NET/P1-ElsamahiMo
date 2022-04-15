using Models;

namespace DL;

public interface IRepository
{
    Customer CreateCustomer(Customer newCustomer);
    int LoginCheck (Customer login);
    Customer GetCustomer(Customer cust);
    Product CreateProduct(Product newPro);
    Product GetProduct(int id);
    List<Inventory> GetInventory(Store getInv);
    List<Store> GetStores();
    Order UpdateOrders(Order updateOrder);
    void UpdateQuantityOrder(Cart cartItem, int storeID);
    Inventory UpdateQuantity(int newQuan, Inventory replenishPro, Store replenishStore);
    List<History> GetOrderHistory(Customer current);
    List<Product> GetAllProducts();
}