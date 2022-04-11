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

    public Customer CreateCustomer(Customer newCustomer)
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

    public int LoginCheck (Customer login)
    {
        bool found = false;
        bool correct = false;

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        
        using SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE customerName = @customerName", connection);

        cmd.Parameters.AddWithValue("@customerName", login.Name);

        SqlDataReader read = cmd.ExecuteReader();
        if(read.HasRows)
            found = true;
        read.Close();

        using SqlCommand cmd2 = new SqlCommand("SELECT * FROM Customers WHERE customerName = @customerName AND customerPass = @customerPass", connection);

        cmd2.Parameters.AddWithValue("@customerName", login.Name);
        cmd2.Parameters.AddWithValue("@customerPass", login.Pass);

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

    public Customer GetCustomer(Customer cust)
    {
        Customer returnCust = new Customer();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE customerName = @customerName", connection);
        cmd.Parameters.AddWithValue("@customerName", cust.Name);

        SqlDataReader read = cmd.ExecuteReader();

        if(read.Read())
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

    public Product CreateProduct(Product newPro)
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

    public Product GetProduct(int id)
    {   

        Product temp = new Product();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE id = @productId", connection);
        cmd.Parameters.AddWithValue("@productId", id);
        
        SqlDataReader read = cmd.ExecuteReader();

        if(read.Read())
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

    public List<Inventory> GetInventory(Store getInv)
    {

        List<Inventory> inv = new List<Inventory>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Inventory WHERE storeID = @storeId", connection);
        cmd.Parameters.AddWithValue("@storeId", getInv.Id);
        
        SqlDataReader read = cmd.ExecuteReader();

        while(read.Read())
        {
            Product tempPro = GetProduct(read.GetInt32(2));
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

    public void UpdateQuantityOrder(Cart cartItem, int storeID)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        int newQuan = 0;
        using SqlCommand cmd2 = new SqlCommand("SELECT * FROM Inventory WHERE productID = @proID AND storeID = @storeID", connection);
        cmd2.Parameters.AddWithValue("@proID", cartItem.item.Id);
        cmd2.Parameters.AddWithValue("@storeID", storeID);
        SqlDataReader readLol = cmd2.ExecuteReader();
        

        if (readLol.Read())
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

    public Order UpdateOrders(Order updateOrder)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("INSERT INTO Orders(dateCreated, total, storeID, customerID) OUTPUT INSERTED.Id VALUES (@date, @total, @storeID, @customerID)", connection);

        cmd.Parameters.AddWithValue("@date", updateOrder.DateCreated);
        cmd.Parameters.AddWithValue("@total", updateOrder.Total());
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

    public List<Store> GetStores()
    {
        List<Store> allStores = new List<Store>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Stores", connection);
        SqlDataReader read = cmd.ExecuteReader();

        while(read.Read())
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

}