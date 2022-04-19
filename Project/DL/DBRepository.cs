using Microsoft.Data.SqlClient;
using System.Data;
using Models;
namespace DL;

public class DBRepository : IRepository
{
    private readonly string _connectionString;

    public DBRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Customer> CreateCustomerAsync(Customer newCustomer)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("INSERT INTO Customers(customerName, customerPass) OUTPUT INSERTED.Id VALUES (@customerName, @customerPass)", connection);

        cmd.Parameters.AddWithValue("@customerName", newCustomer.Name);
        cmd.Parameters.AddWithValue("@customerPass", newCustomer.Pass);

        cmd.ExecuteScalar();
        connection.Close();

        return newCustomer;
    }

    public async Task<int> LoginCheckAsync (string cusName, string cusPass)
    {
        bool found = false;
        bool correct = false;

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        
        using SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE customerName = @customerName", connection);

        cmd.Parameters.AddWithValue("@customerName", cusName);

        SqlDataReader read = cmd.ExecuteReader();
        if(read.HasRows)
            found = true;
        read.Close();

        using SqlCommand cmd2 = new SqlCommand("SELECT * FROM Customers WHERE customerName = @customerName AND customerPass = @customerPass", connection);

        cmd2.Parameters.AddWithValue("@customerName", cusName);
        cmd2.Parameters.AddWithValue("@customerPass", cusPass);

        SqlDataReader read2 = cmd2.ExecuteReader();
        if(read2.HasRows)
            correct = true;
        read2.Close();

        // if the user name is found and the pass word is correct
        if(correct)
            return 2;

        // if the user name is found but the pass word was incorrect
        if(found)
            return 1;

        // if the user name wasn't found at all
        return 0;

    }

    public async Task<Customer> GetCustomerAsync(string cusName)
    {
        Customer returnCust = new Customer();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE customerName = @customerName", connection);
        cmd.Parameters.AddWithValue("@customerName", cusName);

        SqlDataReader read = cmd.ExecuteReader();

        if(await read.ReadAsync())
        {
            int tempID = read.GetInt32(0);
            string name = read.GetString(1);
            string pass = read.GetString(2);
        
            returnCust.Id = tempID;
            returnCust.Name = name;
            returnCust.Pass= pass;
            
        }

        read.Close();
        connection.Close();

        return returnCust;
    }

    public async Task<Product> CreateProductAsync(Product newPro)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("INSERT INTO Products(productName, price) OUTPUT INSERTED.Id VALUES (@productName, @price)", connection);

        cmd.Parameters.AddWithValue("@productName", newPro.ItemName);
        cmd.Parameters.AddWithValue("@price", newPro.Price);

        cmd.ExecuteScalar();
        connection.Close();

        return newPro;
    }

    public async Task<Product> GetProductAsync(int id)
    {   

        Product temp = new Product();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE id = @productId", connection);
        cmd.Parameters.AddWithValue("@productId", id);
        
        SqlDataReader read = cmd.ExecuteReader();

        if(await read.ReadAsync())
        {
            int tempID = read.GetInt32(0);
            string name = read.GetString(1);
            double price = read.GetDouble(2);
        
            temp.Id = tempID;
            temp.ItemName = name;
            temp.Price = price;
            
        }

        read.Close();

        return temp;

    }

    public async Task<List<Inventory>> GetInventoryAsync(int storeID)
    {

        List<Inventory> inv = new List<Inventory>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Inventory WHERE storeID = @storeId", connection);
        cmd.Parameters.AddWithValue("@storeId", storeID);
        
        SqlDataReader read = cmd.ExecuteReader();

        while(await read.ReadAsync())
        {
            Product tempPro =  await GetProductAsync(read.GetInt32(2));
            int quan = read.GetInt32(1);

            Inventory tempInv = new Inventory{
                invPro = tempPro,
                quan = quan
            };

            inv.Add(tempInv);
        }

        read.Close();
        connection.Close();

        List<Inventory> SortedList = inv.OrderBy(o=>o.invPro.Id).ToList();

        return SortedList;
    }

    public async Task UpdateQuantityOrderAsync(Cart cartItem, int storeID)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        int newQuan = 0;
        using SqlCommand cmd2 = new SqlCommand("SELECT * FROM Inventory WHERE productID = @proID AND storeID = @storeID", connection);
        cmd2.Parameters.AddWithValue("@proID", cartItem.item.Id);
        cmd2.Parameters.AddWithValue("@storeID", storeID);
        SqlDataReader readLol = cmd2.ExecuteReader();
        

        if (await readLol.ReadAsync())
        {
            newQuan = (readLol.GetInt32(1) - cartItem.cartQuan);
            readLol.Close();

            using SqlCommand cmd = new SqlCommand("UPDATE Inventory SET quantity = @quan WHERE productID = @proID AND storeID = @storeID", connection);
            cmd.Parameters.AddWithValue("@quan", newQuan);
            cmd.Parameters.AddWithValue("@proID", cartItem.item.Id);
            cmd.Parameters.AddWithValue("@storeID", storeID);
            cmd.ExecuteScalar();
        }
        
        connection.Close();
        
        


    }

    public async Task<Order> UpdateOrdersAsync(Order updateOrder)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("INSERT INTO Orders(dateCreated, total, storeID, customerID) OUTPUT INSERTED.Id VALUES (@date, @total, @storeID, @customerID)", connection);

        cmd.Parameters.AddWithValue("@date", updateOrder.DateCreated);
        cmd.Parameters.AddWithValue("@total", updateOrder.Total);
        cmd.Parameters.AddWithValue("@storeID", updateOrder.StoreID);
        cmd.Parameters.AddWithValue("@customerID", updateOrder.CustID);

        try
        {
            updateOrder.Id = (int) cmd.ExecuteScalar();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        connection.Close();

        return updateOrder;
    }

    public async Task<List<Store>> GetStoresAsync()
    {
        List<Store> allStores = new List<Store>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Stores", connection);
        SqlDataReader read = cmd.ExecuteReader();

        while(await read.ReadAsync())
        {
            int id = read.GetInt32(0);
            string location = read.GetString(1);

            Store tempStore = new Store{
                Id = id,
                StoreLocation = location
            };
            allStores.Add(tempStore);
        }

        read.Close();
        connection.Close();

        return allStores;
    }

    public async Task UpdateQuantityAsync(Inventory replenishPro, int storeID)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("UPDATE Inventory SET quantity = @quan WHERE productID = @proID AND storeID = @storeID", connection);
        
        cmd.Parameters.AddWithValue("@quan", replenishPro.quan);
        cmd.Parameters.AddWithValue("@proID", replenishPro.invPro.Id);
        cmd.Parameters.AddWithValue("@storeID", storeID);
        cmd.ExecuteScalar();

        connection.Close();
    }

    public async  Task<List<History>> GetOrderHistoryAsync(int currentID)
    {
        List<History> history = new List<History>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT Stores.storeLocation, Orders.total, Orders.dateCreated FROM Orders JOIN Customers ON Orders.customerID = Customers.id JOIN Stores ON Orders.storeID = Stores.id WHERE Orders.customerID = @custID", connection);
        cmd.Parameters.AddWithValue("@custID", currentID);

        SqlDataReader read = cmd.ExecuteReader();

        while (await read.ReadAsync())
        {
            History temp = new History{
                StoreLocation = read.GetString(0),
                Total = read.GetDouble(1),
                OrderDate = read.GetDateTime(2)
            };

            history.Add(temp);
        }

        read.Close();
        connection.Close();

        return history;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        List<Product> allPro = new List<Product>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Products",connection);
        SqlDataReader read = cmd.ExecuteReader();

        while (await read.ReadAsync())
        {
            Product temp = new Product{
                Id = read.GetInt32(0),
                ItemName = read.GetString(1),
                Price = read.GetDouble(2)
            };

            allPro.Add(temp);
        }

        read.Close();
        connection.Close();

        return allPro;
    }

    public async Task AddProductAsync(Inventory proToAdd, int storeID)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("INSERT INTO Inventory(quantity, productID, storeID) OUTPUT INSERTED.Id VALUES (@quan, @proID, @storeID)", connection);
        cmd.Parameters.AddWithValue("@quan", proToAdd.quan);
        cmd.Parameters.AddWithValue("@proID", proToAdd.invPro.Id);
        cmd.Parameters.AddWithValue("@storeID", storeID);

        cmd.ExecuteScalar();
        connection.Close();

    }
}