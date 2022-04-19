using System.ComponentModel.DataAnnotations;
using Models;

public class Order : Default
{
    private double total = 0.00; 
    private List<Cart> cartItems = new List<Cart>(); 
    private int custID = 0;
    private int storeID = 0;

//------------------------------------------------------------------

    public DateTime DateCreated { get; set; }

    public void AddTotal (double addPrice)
    {
        total += addPrice;
    } 

    public double Total{ get => total;}

    public int CustID {
        get => custID; 
        set
        {
            if(value <= 0)
                throw new ValidationException("Something went wrong with custID");
            
            custID = value;
        }
    }

    public int StoreID {
        get => storeID; 
        set
        {
            if(value <= 0)
                throw new ValidationException("Something went wrong with storeID");
            
            storeID = value;
        }
    }

    

    public void AddCartItems (Product adding, int quan)
    {
        cartItems.Add(new Cart{item = adding, cartQuan = quan});
        AddTotal(adding.Price * quan);
    }

    public List<Cart> CartItem ()
    {
        return cartItems;
    }
    
}