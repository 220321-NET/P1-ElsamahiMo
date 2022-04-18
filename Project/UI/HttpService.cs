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

}