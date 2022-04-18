using Models;
using System.ComponentModel.DataAnnotations;

namespace UI;

internal class Menu
{

    private readonly HttpService _httpService;

    public Menu(HttpService httpService) 
    { 
        _httpService = httpService;
    }

    public async Task Start()
    {
        bool exit = false;
        do
        {
            Transition();
            Console.WriteLine("Welcome to the Games Palace");
            Console.WriteLine("[0]: Login");
            Console.WriteLine("[1]: Create Account");
            Console.WriteLine("[x]: Exit");

            Input:
            string? response = Console.ReadLine();

            switch(response.Trim().ToUpper())
            {
                case "0": 
                    await LoginAsync();
                    break;
                case "1":
                    await Signup();
                    break;
                case "14383421":
                    await Admin();
                    break;
                case "X":
                    Console.WriteLine("Goodbye.");
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Input not acceppted, Please try again.");
                    goto Input;
            }

        }while (!exit);
    }

    public void Transition()
    {
        Console.WriteLine("\n-------------------------------------------\n");
    }

    public async Task LoginAsync()
    {
        Transition();

        EnterLogin:
        Console.WriteLine("Please enter your name");
        string? name = Console.ReadLine();
        Console.WriteLine("Please enter the password for your account");
        string? password = Console.ReadLine();

        Customer login = new Customer();

        try
        {
            login.Name = name;
            login.Pass = password;
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e.Message);
            goto EnterLogin;
        }

        int results = await _httpService.LoginCheckAsync(login.Name,login.Pass);
        switch(results)
        {
            case 0:
                InputLogin0:
                Console.WriteLine("Account name does not exsist");
                Console.WriteLine("Try again? (Y/N)");
                string? responseLogin0 = Console.ReadLine();
                if (responseLogin0.Trim().ToUpper()[0] == 'Y')
                    goto EnterLogin;
                else if (responseLogin0.Trim().ToUpper()[0] == 'N')
                    break;
                else 
                {
                    Console.WriteLine("Input not recognized");
                    goto InputLogin0;
                }
            case 1: 
                InputLogin1:
                Console.WriteLine("Password is incorrect");
                Console.WriteLine("Try again? (Y/N)");
                string? responseLogin1 = Console.ReadLine() ?? "Q";
                if (responseLogin1.Trim().ToUpper()[0] == 'Y')
                    goto EnterLogin;
                else if (responseLogin1.Trim().ToUpper()[0] == 'N')
                    break;
                else 
                {
                    Console.WriteLine("Input not recognized");
                    goto InputLogin1;
                }
            case 2:
                Console.WriteLine("Login successful!");
                Customer current = await _httpService.GetCustomerAsync(login.Name);
                CustomerMenu(current);
                break;
        }
    }

    public async Task Signup()
    {
        Transition();
        Console.WriteLine("Welcome new customer please provide some information before getting started.");
        
        EnterCustomerInfo:
        Console.WriteLine("What is your name? ");
        string? name = Console.ReadLine();

        Console.WriteLine("Please create a password");
        string? password = Console.ReadLine();

        Customer newCustomer = new Customer();

        try
        {
            newCustomer.Name = name;
            newCustomer.Pass = password;
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e.Message);
            goto EnterCustomerInfo;
        }

        Customer createdCustomer = await _httpService.CreateCustomerAsync(newCustomer);
        if (createdCustomer != null)
            Console.WriteLine("\nAccount created successfully");
    }

//--------------------------------------------------------------------
//End of main menu funtions
//--------------------------------------------------------------------

    public void CustomerMenu(Customer current)
    {   
        bool customerExit = false;
        do
        {
            CustomerMenuInput:
            Transition();
            Console.WriteLine($"Welcome {current.Name}.");
            Console.WriteLine("What would you like to do today?");
            Console.WriteLine("[0]: Shop games");
            Console.WriteLine("[1]: View order history");
            Console.WriteLine("[x]: Go back");
            string? cmResponse = Console.ReadLine();

            switch(cmResponse.Trim().ToUpper()[0])
            {
                case '0':
                    ShopGames(current);
                    break;
                case '1':
                    DisplayHistory(current);
                    break;
                case 'X':
                    customerExit = true;
                    break;
                default:
                    Console.WriteLine("Input not acceppted, Please try again.");
                    goto CustomerMenuInput;
            }
        }while(!customerExit);
    }

    public async Task ShopGames(Customer current)
    {
        Order currentOrder = new Order();
        int count = 0;

        Transition();
        Console.WriteLine("Which store would you like to shop at?");
        Store shopAt = await SelectStoreAsync();

        KeepShopping:
        Transition();
        Console.WriteLine("Select the game you would like to add to your cart.");
        Inventory shopPro = SelectInventory(shopAt);

        Transition();

        shopConfirm:
        Console.WriteLine($"Are you should you would like to add \n{shopPro.invPro.ItemName} at ${shopPro.invPro.Price} to your cart (Y/N)");
        string? shopInput = Console.ReadLine();

        switch(shopInput.Trim().ToUpper()[0])
        {
            case 'Y':
                QuanInput:
                Console.WriteLine($"How many copies of {shopPro.invPro.ItemName} would you like to add to your cart");
                int quanInput = Int32.Parse(Console.ReadLine());

                if(quanInput <= shopPro.quan && quanInput > 0)
                {
                    Cart shopCart = new Cart{item = shopPro.invPro, cartQuan = quanInput};
                    AddToCart(current, shopAt, shopCart, currentOrder, count);
                    count++;
                }
                else
                {
                    Console.WriteLine($"Quantity you wish to purcahse must be in the range of (0-{shopPro.quan})");
                    goto QuanInput;
                }
                break;
            case 'N':
                Console.WriteLine("Item not added to cart");
                break;
            default:
                Console.WriteLine("Invalid input, Try again");
                goto shopConfirm;
                return;
        }

        if (count > 0)
        {
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("[0]: Keep Shoping");
            Console.WriteLine("[1]: Check out");
            Console.WriteLine("[x]: Cancel");

            string? cartInput = Console.ReadLine();

            switch(cartInput.Trim().ToUpper()[0])
            {
                case '0':
                    goto KeepShopping;
                case '1':
                    CheckOut(currentOrder);
                    break;
                case 'X':
                    break;
            }
        }

    }

    public void AddToCart(Customer current, Store shopAt, Cart shopCart, Order currentOrder, int count)
    {
        if(count == 0)
        {
            currentOrder.CustID = current.Id;
            currentOrder.StoreID = shopAt.Id;
        }

        currentOrder.AddCartItems(shopCart.item, shopCart.cartQuan);


    }

    public void CheckOut(Order currentOrder)
    {
        
        // for(int i = 0; i < currentOrder.CartItem().Count(); i++)
        //     _bl.UpdateQuantityOrder(currentOrder.CartItem()[i], currentOrder.StoreID);

        // currentOrder.DateCreated = DateTime.Now;
        // if(_bl.UpdateOrders(currentOrder) != null)
        // {
        //     Console.WriteLine("Order Placed!!!");
        // }
    }

    public void DisplayHistory(Customer current)
    {   
        Transition();
        Console.WriteLine($"This is the order history for the user: {current.Name}\n");
        
        // List<History> history = _bl.GetOrderHistory(current);
        // for (int i = 0; i < history.Count(); i++)
        // {
        //     Console.WriteLine($"[{i+1}] {history[i].ToString()}");
        // }

        Console.WriteLine("\nPress Enter to continue");
        Console.ReadLine();
    }

//--------------------------------------------------------------------
//End of customer menu funtions
//--------------------------------------------------------------------

    public async Task Admin()
    {
        bool adminExit = false;
        do
        {
            Transition();
            Console.WriteLine("Welcome to the Admin Menu");
            Console.WriteLine("[0]: Replenish Stocks");
            Console.WriteLine("[1]: View Inventory");
            Console.WriteLine("[2]: Add Product");
            Console.WriteLine("[3]: Add Product to Store");
            Console.WriteLine("[x]: Go back");

            AdminInput:
                string? response = Console.ReadLine();

                switch(response.Trim().ToUpper()[0])
                {
                    case '0': 
                        ReplenishStock();
                        break;
                    case '1':
                        ViewInventory();
                        break;
                    case '2':
                        await AddProduct();
                        break;
                    case '3':
                        await AddProductToStoreAsync();
                        break;
                    case 'X':
                        adminExit = true;
                        break;
                    default:
                        Console.WriteLine("Input not acceppted, Please try again.");
                        goto AdminInput;
                }
        }while(!adminExit);
    }

        public async Task ReplenishStock()
    {
        Transition();
        Console.WriteLine("Please choose a location to replenish stocks at");
        Store? replenishStore = await SelectStoreAsync();

        Transition();
        Inventory? replenishPro = SelectInventory(replenishStore);

        Console.WriteLine($"Please enter the new quantity of {replenishPro.invPro.ItemName}");
        int newQuan = Convert.ToInt32(Console.ReadLine());

        // Inventory newReplenish = _bl.UpdateQuantity(newQuan, replenishPro, replenishStore);

        // Console.WriteLine($"Here is your newly updated product: {newReplenish.ToString()}");
    }

    public async Task ViewInventory()
    {
        Transition();
        Console.WriteLine("Which store would you like to view the inventory for?");
        Store? viewStore = await SelectStoreAsync();

        // Console.WriteLine($"Here is the Inventory for the {viewStore.StoreLocation} store:");
        // List<Inventory> inventory = _bl.GetInventory(viewStore);

        // if (inventory.Count == 0)
        // {
        //     Console.WriteLine("This store has no inventory");
        //     return;
        // }
        // for (int i = 0; i < inventory.Count; i++)
        //     Console.WriteLine($"[{i}]: {inventory[i].ToString()}");

        // Console.WriteLine("Press enter to continue");
        // Console.ReadLine();
        
    }

    public async Task AddProduct() 
    {
        Transition();
        
        EnterProductInfo:
        Console.WriteLine("What is the name of the game you would like to add?");
        string? proName = Console.ReadLine();

        Console.WriteLine("What is the price?");
        double? proPrice = Convert.ToDouble(Console.ReadLine());

        Product newPro = new Product();

        try
        {
            newPro.ItemName = proName;
            newPro.Price = proPrice.Value;
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e.Message);
            goto EnterProductInfo;
        }

        Product createdProduct = await _httpService.CreateProductAsync(newPro);
        if (createdProduct != null)
            Console.WriteLine("\nProduct created successfully");
    }

    public async Task AddProductToStoreAsync()
    {
        Transition();
        Console.WriteLine("Which store do you want to add a product to?");
        Store? addProStore = await SelectStoreAsync();

        Transition();
        Console.WriteLine($"Which Product would you like to add to {addProStore.StoreLocation} location");
        Product addPro = await SelectProductsAsync();

        //TODO: FINISH THIS SHIT

    }

    public async Task<Product> SelectProductsAsync()
    {
        Transition();
        Console.WriteLine("Here are all the Products");
        
        List<Product> allPro = await _httpService.GetAllProductAsync();

        InvInput:
        for (int i = 0; i < allPro.Count; i++)
            Console.WriteLine(allPro[i].ToString());
        
        int proSelect;

        if(Int32.TryParse(Console.ReadLine(), out proSelect) && ((proSelect-1) >= 0 && (proSelect-1) < allPro.Count))
            return allPro[proSelect-1];
        else
        {
            Console.WriteLine("Invalid input, Try again");
            goto InvInput;
        }
    }

//--------------------------------------------------------------------
//End of admin menu funtions
//--------------------------------------------------------------------
//The rest are functions called by multiple sources
// used by both admin and customer
//--------------------------------------------------------------------

    public async Task<Store?> SelectStoreAsync()
    {
        Console.WriteLine("Here are all the stores by state: ");
        List<Store> allStores = await _httpService.GetStoresAsync();

        if(allStores.Count == 0)
            return null;

        SelectInput:
        for (int i = 0; i < allStores.Count; i++)
            Console.WriteLine(allStores[i].ToString());

        int select;

        if(Int32.TryParse(Console.ReadLine(), out select) && ((select-1) >= 0 && (select-1) < allStores.Count))
            return allStores[select-1];
        else
        {
            Console.WriteLine("Invalid input, Try again");
            goto SelectInput;
        }
        // return new Store();
    } 

    
    public Inventory SelectInventory(Store getInv)
    {
        // Console.WriteLine($"Here is the Inventory for the {getInv.StoreLocation} store:");
        // List<Inventory> inventory = _bl.GetInventory(getInv);

        // if (inventory.Count == 0)
        //     return null;
        
        // InvInput:
        // for (int i = 0; i < inventory.Count; i++)
        //     Console.WriteLine($"[{i}]: {inventory[i].ToString()}");
        
        // int proSelect;

        // //this if block is different from the two previous becuase I want a 0-N list in order,
        // //not one that skips numbers. This could happen if a store does not hold every piece of product
        // //which is why I am using proSelect instead of proSelect-1. it is printing the number from the for loop, not the object ID
        // //Hopefully some of that made any sense
        // if(Int32.TryParse(Console.ReadLine(), out proSelect) && ((proSelect) >= 0 && (proSelect) < inventory.Count))
        //     return inventory[proSelect];
        // else
        // {
        //     Console.WriteLine("Invalid input, Try again");
        //     goto InvInput;
        // }
        return new Inventory();
    }

}
