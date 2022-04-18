using UI;
// using BL;
// using DL;

//string connectionString = File.ReadAllText("C:\\Users\\Elsam\\Revature\\P1-ElsamahiMo\\Project\\UI\\connectionString.txt");

//Dependency Injection, Whatever that is 
// IRepository repo = new DBRepository(connectionString);
// ISLBL bl = new SLBL(repo);

HttpService http = new HttpService();
await new Menu(http).Start();