using System.ComponentModel.DataAnnotations;
using Models;

public class History : Default
{
    public Double Total {get; set;}

    public String StoreLocation {get; set;}

    public DateTime OrderDate {get; set;}

    public override string ToString()
    {
        return $"\t{StoreLocation}: \t${Total}   \tPlaced on: {OrderDate}";
    }
}