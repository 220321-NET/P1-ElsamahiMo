using Models;

public class Inventory
{

    public Product invPro {get; set;} = new Product();

    public int quan {get; set;}

    public override string ToString()
    {
        return $"{invPro.ItemName} [{quan} in stock]";
    }

}