using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models;

public class Customer : Default
{
    private string name = "";
    private string pass = "";

    public string Name 
    {
        get => name;
        set
        {
            if(String.IsNullOrWhiteSpace(value))
                throw new ValidationException("Name cannot be empty");
            
            name = value.Trim();
        }
    }

    public string Pass 
    {
        get => pass; 
        set
        {
            if(String.IsNullOrWhiteSpace(value))
                throw new ValidationException("Password cannot be empty");
            
            pass = value.Trim();
        }
    }

    public List<Order> OrderHistory {get; set;} = new List<Order>();

}