using UI;
using BL;
using DL;

string connectionString = File.ReadAllText("./connectionString.txt");

//Dependency Injection, Whatever that is 
IRepository repo = new DBRepository(connectionString);
ISLBL bl = new SLBL(repo);
new Menu(bl).Start();