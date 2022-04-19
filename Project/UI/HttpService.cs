using System.Net.Http;
using Models;
using System.Text.Json;
using System.Text;

namespace UI;
public class HttpService
{
    private readonly string _apiBaseUrl = "https://localhost:7242/api/";
    private HttpClient client = new HttpClient();

    public HttpService()
    {
        client.BaseAddress = new Uri(_apiBaseUrl);
    }

    public async Task<int> LoginCheckAsync(string cusName, string cusPass)
    {
        int output = 0;
        try
        {
            HttpResponseMessage response = await client.GetAsync($"Items/GetLogin/{cusName}/{cusPass}");
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            output = JsonSerializer.Deserialize<int>(responseString);
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return output;
    }

    public async Task<Customer> GetCustomerAsync(string cusName)
    {
        Customer cust = new Customer();
        try
        {
            HttpResponseMessage response = await client.GetAsync($"Items/GetCustomer/{cusName}");
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            cust = JsonSerializer.Deserialize<Customer>(responseString)  ?? new Customer();
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return cust;
    }

    public async Task<List<Inventory>> GetInventoryAsync(int storeID)
    {
        List<Inventory> games = new List<Inventory>();

        try 
        {
            HttpResponseMessage response = await client.GetAsync($"Items/GetInventory/{storeID}");
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            games = JsonSerializer.Deserialize<List<Inventory>>(responseString) ?? new List<Inventory>();
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return games;
    }

    public async Task<List<Product>> GetAllProductAsync()
    {

        string url = _apiBaseUrl + "Items/GetAllProducts";
        HttpClient client = new HttpClient();

        List<Product> products = new List<Product>();

        try 
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            products = JsonSerializer.Deserialize<List<Product>>(responseString) ?? new List<Product>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Something bad happened: {ex.Message}");
        }
        return products;
    }

    public async Task<List<Store>> GetStoresAsync()
    {
        List<Store> stores = new List<Store>();

        try
        {
            HttpResponseMessage response = await client.GetAsync($"Items/GetAllStores");
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            stores = JsonSerializer.Deserialize<List<Store>>(responseString)  ?? new List<Store>();
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return stores;
    }

    public async Task<List<History>> GetHistoryAsync(int custID)
    {
        List<History> history = new List<History>();

        try 
        {
            HttpResponseMessage response = await client.GetAsync($"Items/GetHistory/{custID}");
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            history = JsonSerializer.Deserialize<List<History>>(responseString) ?? new List<History>();
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return history;
    }

    public async Task<Customer> CreateCustomerAsync(Customer cusToCreate)
    {
        string json = JsonSerializer.Serialize(cusToCreate);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync("Items/CreateCustomer", content);
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<Customer>(await response.Content.ReadAsStreamAsync()) ?? new Customer();
        }
        catch(HttpRequestException)
        {
            throw;
        }
    }

    public async Task<Product> CreateProductAsync(Product proToCreate)
    {
        string json = JsonSerializer.Serialize(proToCreate);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync("Items/CreateProduct", content);
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<Product>(await response.Content.ReadAsStreamAsync()) ?? new Product();
        }
        catch(HttpRequestException)
        {
            throw;
        }
    }

    public async Task AddProductAsync(Inventory proToAdd, int storeID)
    {
        string json = JsonSerializer.Serialize(proToAdd);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync($"Items/AddProduct/{storeID}", content);
            response.EnsureSuccessStatusCode();
        }
        catch(HttpRequestException)
        {
            throw;
        }
    }

    public async Task<Order> UpdateOrdersAsync(Order orderToCreate)
    {
        string json = JsonSerializer.Serialize(orderToCreate);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        
        try
        {
            HttpResponseMessage response = await client.PostAsync("Items/CreateOrder", content);
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<Order>(await response.Content.ReadAsStreamAsync()) ?? new Order();
        }
        catch (HttpRequestException)
        {
            throw;
        }

    }

    public async Task UpdateQuantityOrderAsync(Cart cartItem, int storeID)
    {
        string json = JsonSerializer.Serialize(cartItem);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PutAsync($"Items/UpdateQuantityOrder/{storeID}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw;
        }
    }

    public async Task UpdateQuantityAsync(Inventory replenishPro, int storeID)
    {
        string json = JsonSerializer.Serialize(replenishPro);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PutAsync($"Items/UpdateQuantity/{storeID}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw;
        }
    }
}