

class ProductManager
{
  public Product? MarkedProduct { set; get; }
  private List<Product> productList = [];
  public enum ResponseCode { Success, Error, Quit };
  public enum Command { NewProduct, Search, Quit };
  public ResponseCode SearchForProduct(out Product? product)
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Search for product ----\n");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("> ");
    var choice = Console.ReadLine() ?? "".Trim().ToLower();
    var result = productList.Find(product => product.ProductName == choice);
    if (result != null)
    {
      product = result;
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("*** Product found! ***\n");
      Console.ReadKey();
      return ResponseCode.Success;
    }
    else
    {
      product = null;
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("*** No product found! ***\n");
      Console.ReadKey();
      return ResponseCode.Error;
    }
  }

  public ResponseCode AddNewProduct()
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("New product ---- ( q for quit )\n");
    Product newProduct = new();
    Console.ForegroundColor = ConsoleColor.White;
    ResponseCode response;

    response = ProductDetailInput<string>("Category", value => newProduct.Category = value);
    if (response == ResponseCode.Quit)
      return response;

    response = ProductDetailInput<string>("Name", value => newProduct.ProductName = value);
    if (response == ResponseCode.Quit)
      return ResponseCode.Quit;

    do
    {
      response = ProductDetailInput<int>("Price", value => newProduct.Price = value);
      if (response == ResponseCode.Quit)
        return ResponseCode.Quit;
    } while (response != ResponseCode.Success);

    productList.Add(newProduct);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\nProduct {newProduct.ProductName} was added!");
    Console.ReadKey();
    return ResponseCode.Success;
  }

  private ResponseCode ProductDetailInput<T>(string label, Action<T> set)
  {
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"{label} > ");
    var input = Console.ReadLine();
    if (input == "q")
    {
      return ResponseCode.Quit;
    }
    if (input != null)
    {
      if (typeof(T) == typeof(int))
      {
        bool success = int.TryParse(input, out int value);
        if (success)
          set((T)(object)value); // int to T runtime conversion that is accepted by the compiler
        else
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Input error : not a number, retry");

          return ResponseCode.Error;
        }
      }
      else
      {
        set((T)(object)input);  // string to T runtime conversion that is accepted by the compiler
      }
    }

    return ResponseCode.Success;
  }

  public void ListAllProducts()
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Product list");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("-------------------------------------------");

    var sortedList = productList.OrderBy(product => product.Price).ToList();
    foreach (Product product in sortedList)
    {
      if (product == MarkedProduct)
        Console.ForegroundColor = ConsoleColor.Yellow;
      else
        Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine($"{product.ProductName.PadRight(15)} {product.Category.PadRight(15)} {product.Price}");
    }
    int sum = productList.Sum(product => product.Price);
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("-------------------------------------------");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"{"Total price:".PadRight(32) + sum}");
    Console.ForegroundColor = ConsoleColor.Blue;

    Console.WriteLine("-------------------------------------------");
    Console.WriteLine("\n");
  }

  public Command NewCommand()
  {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Make a choice : (a) - add new product, (s) - Search for product, (q) - Quit");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("> ");
    Console.ForegroundColor = ConsoleColor.White;
    var choice = Console.ReadLine() ?? "".Trim().ToLower();
    if (choice == "a")
      return Command.NewProduct;
    if (choice == "s")
      return Command.Search;
    return Command.Quit;
  }
}
