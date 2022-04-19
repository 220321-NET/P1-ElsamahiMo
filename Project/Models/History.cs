using System.ComponentModel.DataAnnotations;
using Models;

public class History : Default
{
    private string storeLocation = ""; 
    public Double Total {get; set;}

    public String StoreLocation 
    {
        get => storeLocation;
        set
        {
            if(String.IsNullOrWhiteSpace(value))
                throw new ValidationException("Name cannot be empty");
            
            storeLocation = value.Trim();
        }
    }

    public DateTime OrderDate {get; set;}

    public override string ToString()
    {
        return $"\t{StoreLocation}: \t${Total}   \tPlaced on: {OrderDate}";
    }
}