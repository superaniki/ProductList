// See https://aka.ms/new-console-template for more information


ProductManager productManager = new();

var command = ProductManager.Command.NewProduct;

while (command != ProductManager.Command.Quit)
{
  // Keep adding new products
  while (command == ProductManager.Command.NewProduct)
  {
    var response = productManager.AddNewProduct();
    if (response == ProductManager.ResponseCode.Quit)
    {
      command = ProductManager.Command.Quit;
    }
  }

  // Make a search
  if (command == ProductManager.Command.Search)
  {
    var response = productManager.SearchForProduct(out Product? product);
    if (response == ProductManager.ResponseCode.Success)
    {
      productManager.MarkedProduct = product;
    }
  }

  // List products
  productManager.ListAllProducts();
  // New command
  command = productManager.NewCommand();
}



