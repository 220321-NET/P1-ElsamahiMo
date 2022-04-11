using System.ComponentModel.DataAnnotations;
namespace Models;

public class Product : Default
{
    private string itemName = "";
    private double price = 0.00f;

    public string ItemName
    {
        get => itemName;
        set
        {
            if(String.IsNullOrWhiteSpace(value))
                throw new ValidationException("Name cannot be empty");
            
            itemName = value.Trim();
        }
    }
    public double Price
    {
        get => price;
        set
        {
            if(value <= 0.00)
                throw new ValidationException("Price must be greater than zero");
            
            price = value;
        }
    }
}