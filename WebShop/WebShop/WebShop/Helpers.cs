using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop
{
    internal class Helpers
    {
        //############################################################
        //STATIC VARIABLES
        static string connString = "data source = .\\SQLEXPRESS; initial catalog = Webshop; persist security info = True; Integrated Security = True; TrustServerCertificate=True;";

        //Skapar lista av typ Product för att lägga till produkter i en kundkorg
        public static List<Product> shoppingCart = new List<Product>();
        //############################################################

        //############################################################
        //SHOW DATA & MENUS
        public static void Admin()
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("Press 'A' for Add new product");
                Console.WriteLine("Press 'U' for Update products");
                Console.WriteLine("Press 'F' for Find products");
                Console.WriteLine("Press 'D' for Delete product");
                Console.WriteLine("Press 'C' for Customer");
                Console.WriteLine("Press 'P' for Product categories");
                Console.WriteLine("Press 'S' for Statistics");
                Console.WriteLine("Press any key to go back");

                var key = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (key)
                {
                    case 'a':
                        AddProduct();
                        break;
                    case 'u':
                        UpdateProduct();
                        break;
                    case 'f':
                        Search();
                        break;
                    case 'c':
                        AdminCustomer();
                        break;
                    case 'p':
                        ShowAdminCategories();
                        break;
                    case 's':
                        ShowStatistics();
                        break;
                    case 'd':
                        DeleteProduct();
                        break;
                    default:
                        Console.Clear();
                        return;
                }
            }
        }

        public static void AdminCustomer()
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("Press 'A' for Add a new customer.");
                Console.WriteLine("Press 'E' for Edit customer info.");
                Console.WriteLine("Press 'D' for Delete customer ");
                Console.WriteLine("Press 'O' for Order history");
                Console.WriteLine("Press 'Q' for Quit");

                var key = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (key)
                {
                    case 'a':
                        AddNewCustomer();
                        break;

                    case 'e':
                        EditCustomer();
                        break;

                    case 'o':
                        ShowOrders();
                        break;

                    case 'd':
                        DeleteCustomer();
                        break;

                    case 'q':
                        Console.Clear();
                        return;
                }
            }
        }

        public static void Customer()
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("Press 'A' for Add a new customer");
                Console.WriteLine("Press 'F' for Find products");
                Console.WriteLine("Press 'S' for Shopping ");
                Console.WriteLine("Press 'C' for Cart ");
                Console.WriteLine("Press 'V' for View orders");
                Console.WriteLine("Press 'Q' for Quit");

                var key = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (key)
                {
                    case 'a':
                        AddNewCustomer();
                        break;
                    case 'f':
                        Search();
                        break;
                    case 's':
                        ShowCategories();
                        break;

                    case 'c':
                        ShowCart();
                        break;
                    case 'v':
                        ShowOrders();
                        break;
                    case 'q':
                        Console.Clear();
                        return;
                }
            }
        }

        public static void Search()
        {
            bool searchSuccess = false;
            while (!searchSuccess)
            {
                Console.Clear();
                Console.WriteLine("Please enter your search keyword");
                string keyword = Console.ReadLine().ToLower();

                using (var database = new WebshopContext())
                {
                    var products = database.Products.ToList();
                    var categories = database.Categories.ToList();

                    var productsWithKeyword = products.Where(p => p.ProductName.ToLower().Contains(keyword)).ToList();
                    var categoriesWithKeyword = categories.Where(c => c.Name.ToLower().Contains(keyword)).ToList();

                    if (productsWithKeyword.Any())
                    {
                        foreach (var product in productsWithKeyword)
                        {
                            Console.WriteLine($"Id: {product.Id.ToString().PadRight(7)} Category: {product.Category.Name.PadRight(7)} Name: {product.ProductName.PadRight(35)} {product.Price.ToString().PadRight(7)} SEK");
                        }
                        Console.WriteLine();
                        ShowProductDetails();
                        searchSuccess = true;
                    }
                    else
                    {
                        Console.WriteLine("No product found. Press any key to search again");
                        Console.ReadKey();
                    }
                    Console.WriteLine();
                }

            }
        }

        public static void ShowAdminCategories()
        {
            Console.Clear();
            Console.WriteLine("Pants");
            Console.WriteLine("Jersey");
            Console.WriteLine("Shoes");
            Console.WriteLine();
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
            Console.Clear();
            Admin();
        }

        public static void ShowCart()
        {
            decimal sum = 0;
            Console.Clear();
            Console.WriteLine("Shopping Cart");
            Console.WriteLine("-------------");
            for (int i = 0; i < shoppingCart.Count(); i++)
            {
                Console.WriteLine("Id: " + i + " - " + shoppingCart[i].ProductName + ":\t" + shoppingCart[i].Price + " SEK");
                sum += shoppingCart[i].Price;
            }
            Console.WriteLine();
            Console.WriteLine("Total price: " + sum + " SEK");
            Console.WriteLine();
            Console.WriteLine("Press 'R' to remove an item from your cart");
            Console.WriteLine("Press 'O' to place an order");
            var key = char.ToLower(Console.ReadKey(true).KeyChar);
            switch (key)
            {
                case 'r':
                    Console.WriteLine("Please enter the Id of the item you want to remove from your cart");
                    bool id = int.TryParse(Console.ReadLine(), out int itemToRemove);
                    Console.WriteLine();
                    for (int i = 0; i < shoppingCart.Count(); i++)
                    {
                        if (itemToRemove == i)
                        {
                            shoppingCart.Remove(shoppingCart[i]);
                            Console.WriteLine("The item has been removed. Note that Ids for remaining items have been updated");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("There is no product with the input Id");
                        }
                    }
                    break;
                case 'o':
                    Console.Clear();
                    PlaceOrder();
                    break;
                default:
                    Console.Clear();
                    return;
            }

        }

        public static void ShowCategories()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Press 'P' for Pants");
                Console.WriteLine("Press 'J' for Jersey");
                Console.WriteLine("Press 'S' for Shoes");
                Console.WriteLine("Press 'Q' for Quit");
                Console.WriteLine();

                var key = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (key)
                {
                    case 'p':
                        using (var database = new WebshopContext())
                        {
                            foreach (var product in database.Products.Where(p => p.CategoryId == 1))
                            {
                                Console.WriteLine("Id: " + product.Id + "\tName: " + product.ProductName.PadRight(35) + "\tSize: " + product.ClothingSize + "\tPrice: " + product.Price.ToString().PadRight(7) + " SEK");
                            }
                            Console.WriteLine();
                            ShowProductDetails();
                        }
                        break;

                    case 'j':
                        using (var database = new WebshopContext())
                        {
                            foreach (var product in database.Products.Where(p => p.CategoryId == 2))
                            {
                                Console.WriteLine("Id: " + product.Id + "\tName: " + product.ProductName.PadRight(35) + "\tSize: " + product.ClothingSize + "\tPrice: " + product.Price.ToString().PadRight(7) + " SEK");
                            }
                            Console.WriteLine();
                            ShowProductDetails();
                        }
                        break;

                    case 's':
                        using (var database = new WebshopContext())
                        {
                            foreach (var product in database.Products.Where(p => p.CategoryId == 3))
                            {
                                Console.WriteLine("Id: " + product.Id + "\tName: " + product.ProductName.PadRight(35) + "\tSize: " + product.ShoeSize + "\tPrice: " + product.Price.ToString().PadRight(7) + " SEK");
                            }
                            Console.WriteLine();
                            ShowProductDetails();
                        }
                        break;

                    case 'q':
                        Console.Clear();
                        return;
                }
            }
        }

        public static void ShowOrders()
        {
            Console.Clear();
            Console.WriteLine("Please enter a customer email");
            string email = Console.ReadLine();
            Console.WriteLine();

            using (var database = new WebshopContext())
            {
                var customer = database.Customers.FirstOrDefault(c => c.Email == email);

                if (customer != null)
                {
                    var orders = database.Orders.Where(o => o.CustomerId == customer.Id).Include(o => o.Products).ToList();

                    foreach (var order in orders)
                    {
                        string date = order.Date.ToString("yyyy-MM-dd HH:mm");
                        Console.WriteLine("Date:\t" + date);

                        Console.WriteLine("Products:\t");
                        var products = order.Products
                            .Select(g => new
                            {
                                ProductName = g.ProductName,
                                PricePerItem = g.Price
                            });

                        foreach (var productDetail in products)
                        {
                            Console.WriteLine($"    {productDetail.ProductName}: {productDetail.PricePerItem} SEK");
                        }
                        Console.WriteLine();

                        string paymentMethod = "";
                        if (order.PaymentId == 1)
                        {
                            paymentMethod = "Credit Card";
                        }
                        else if (order.PaymentId == 2)
                        {
                            paymentMethod = "Swish";
                        }
                        else if (order.PaymentId == 3)
                        {
                            paymentMethod = "Klarna";
                        }
                        Console.WriteLine("Payment method: " + paymentMethod);

                        string deliveryMethod = "";
                        if (order.DeliveryId == 1)
                        {
                            deliveryMethod = "PostNord, 0 SEK";
                        }
                        else if (order.DeliveryId == 2)
                        {
                            deliveryMethod = "Instabox, 39 SEK";
                        }
                        else if (order.DeliveryId == 3)
                        {
                            deliveryMethod = "UPS Express, 69 SEK";
                        }
                        Console.WriteLine("Delivery method: " + deliveryMethod);
                        Console.WriteLine("Sum: " + order.Sum + " SEK");
                        Console.WriteLine();
                        Console.WriteLine("Delivery address: " + customer.StreetAddress + " " + customer.Zipcode + " " + customer.City);

                        Console.WriteLine("____________________");
                    }
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("No customer found");
                }
            }
        }

        public static void ShowProductDetails()
        {
            using (var database = new WebshopContext())
            {
                Console.WriteLine("Choose the Product Id for more Info");

                var productInfo = int.TryParse(Console.ReadLine(), out int selection);
                if (!productInfo)
                {
                    Console.WriteLine("Wrong input.");
                    Console.WriteLine("Press any key to go back");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    var selectedproduct = database.Products.FirstOrDefault(p => p.Id == selection);
                    Console.Clear();
                    if (selectedproduct != null)
                    {
                        Console.WriteLine("Brand Name: " + selectedproduct.ProductName);
                        Console.WriteLine("Product Info: " + selectedproduct.ProductInfo);
                        Console.WriteLine("Price: " + selectedproduct.Price);
                        Console.WriteLine("In Stock: " + selectedproduct.Stock);
                        Console.WriteLine("Supplier: " + selectedproduct.Supplier);
                        Console.WriteLine("Size: " + ((selectedproduct.CategoryId == 3) ? selectedproduct.ShoeSize.ToString() : selectedproduct.ClothingSize.ToString()));

                        Console.WriteLine();
                        Console.WriteLine("Press 'B' to buy " + selectedproduct.ProductName);
                        Console.WriteLine("Press anything else to go back");
                        var buy = char.ToLower(Console.ReadKey(true).KeyChar);
                        switch (buy)
                        {
                            case 'b':
                                shoppingCart.Add(selectedproduct);
                                Console.WriteLine("Thank you, " + selectedproduct.ProductName + " has been added to your cart");
                                Console.ReadKey();
                                Console.Clear();
                                break;
                            default:
                                Console.Clear();
                                return;
                        }
                    }
                }
            }
        }

        public static void ShowStatistics()
        {
            Console.Clear();
            //Best selling product
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Best selling product:");
            Console.ForegroundColor = ConsoleColor.White;
            List<dynamic> bestSelling = Helpers.ShowBestSelling();
            foreach (var product in bestSelling)
            {
                Console.WriteLine("   Product name: " + product.ProductName);
                Console.WriteLine("   Amount sold: " + product.Sold);
                Console.WriteLine("   Total sold for (SEK): " + product.TotalSoldSEK);
            }
            Console.WriteLine();

            //Best brand
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Best selling brand:");
            Console.ForegroundColor = ConsoleColor.White;
            List<dynamic> bestBrand = Helpers.ShowBestBrand();
            foreach (var brand in bestBrand)
            {
                Console.WriteLine("   Brand name: " + brand.MostPopularBrand);
                Console.WriteLine("   Sold products " + brand.SoldProducts);
                Console.WriteLine("   Order: " + brand.Orders);
                Console.WriteLine("   Product list: " + brand.ProductList);
                Console.WriteLine();
            }

            //Most loyal customer
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Most loyal customer:");
            Console.ForegroundColor = ConsoleColor.White;
            List<dynamic> loyalCustomer = Helpers.ShowLoyalCustomer();
            foreach (var customer in loyalCustomer)
            {
                Console.WriteLine($"   {customer.FirstName} {customer.LastName}, {customer.City}");
                Console.WriteLine($"   Order count: " + customer.OrderCount);
                Console.WriteLine();
            }
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ShowWindow()
        {
            using (var database = new WebshopContext())
            {
                List<string> showItems = new List<string>();
                var products = database.Products.Where(p => p.SelectedProduct == true).Take(3);
                foreach (var product in products)
                {
                    showItems.Add($"{product.ProductName}: {product.Price} SEK");
                }
                var featuredItems = new Window("Featured Items", 0, 8, showItems);
                featuredItems.Draw();
            }
            Console.WriteLine();
        }

        public static List<dynamic> ShowBestSelling()
        {
            string sql = @"
                        SELECT TOP 1
                            ProductsId AS ProductId,
                            p.ProductName,
                            COUNT(op.ProductsId) AS Sold,
                            SUM(p.Price) AS 'TotalSoldSEK'
                        FROM 
                            OrderProduct op 
                        JOIN Products p ON op.ProductsId = p.Id
                        GROUP BY op.ProductsId ,
                            p.ProductName
                        ORDER BY op.ProductsId
                        ";

            List<dynamic> bestSellingProduct = new List<dynamic>();
            using (var connection = new SqlConnection(connString))
            {
                bestSellingProduct = connection.Query(sql).ToList();
            }
            return bestSellingProduct;
        }

        public static List<dynamic> ShowBestBrand()
        {
            string sql = @"
                        SELECT Top 3 
                                Supplier AS 'MostPopularBrand',
		                        COUNT(p.Supplier) AS 'SoldProducts',
		                        STRING_AGG(o.Id, ', ') AS 'Orders',
		                        STRING_AGG(p.ProductName, ', ' ) AS 'ProductList'
                        FROM Products p
                        JOIN OrderProduct op ON p.Id = op.ProductsId
                        JOIN Orders o ON op.OrdersId = o.Id
                        GROUP BY 
                            p.Supplier
                        ORDER BY 
                            'SoldProducts' DESC
                        ";

            List<dynamic> bestBrand = new List<dynamic>();
            using (var connection = new SqlConnection(connString))
            {
                bestBrand = connection.Query(sql).ToList();
            }
            return bestBrand;
        }

        public static List<dynamic> ShowLoyalCustomer()
        {
            string sql = @"
                        SELECT TOP 3 
                            c.FirstName AS FirstName,
                            c.LastName AS LastName,
                            c.City AS City,
                            COUNT(o.CustomerId) AS 'OrderCount'
                        FROM Customers c
                        JOIN Orders o ON c.Id = o.CustomerId
                        GROUP BY 
                            c.FirstName,
                            c.LastName,
                            c.City
                        ORDER BY	
                            OrderCount DESC
                            ";

            List<dynamic> loyalCustomer = new List<dynamic>();
            using (var connection = new SqlConnection(connString))
            {
                loyalCustomer = connection.Query(sql).ToList();
            }
            return loyalCustomer;
        }
        //############################################################

        //############################################################
        //MANIPULATE DATA
        public static void AddNewCustomer()
        {
            Console.Clear();

            using (var database = new WebshopContext())
            {
                Console.WriteLine("Enter first name");
                var customerFirstName = Console.ReadLine();

                Console.WriteLine("Enter last name");
                var customerLastName = Console.ReadLine();

                Console.WriteLine("Enter street address");
                var customerAddress = Console.ReadLine();

                Console.WriteLine("Enter city");
                var customerCity = Console.ReadLine();

                Console.WriteLine("Enter country");
                var customerCountry = Console.ReadLine();

                Console.WriteLine("Enter zipcode");
                var customerZipcode = Console.ReadLine();

                Console.WriteLine("Enter phone number");
                var customerPhone = Console.ReadLine();

                Console.WriteLine("Enter Email");
                var customerEmail = Console.ReadLine();

                Console.WriteLine("Enter age");
                var customerAge = int.Parse(Console.ReadLine());

                var customer = new Customer
                {
                    FirstName = customerFirstName,
                    LastName = customerLastName,
                    StreetAddress = customerAddress,
                    City = customerCity,
                    Country = customerCountry,
                    Zipcode = customerZipcode,
                    Phone = customerPhone,
                    Email = customerEmail,
                    Age = customerAge

                };

                database.Add(customer);
                database.SaveChanges();
                Console.WriteLine($"User {customerFirstName} {customerLastName} is now registered");
                Console.ReadKey();
            }
        }

        public static void AddProduct()
        {
            Console.Clear();
            using (var database = new WebshopContext())
            {
                Console.WriteLine("Press 'P' for adding pants");
                Console.WriteLine("Press 'J' for adding jersey");
                Console.WriteLine("Press 'S' for adding shoes");

                var key = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (key)
                {
                    case 'p':
                        Console.WriteLine("Enter name of pants:");
                        var newPantsName = Console.ReadLine();

                        Console.WriteLine("Enter product info:");
                        var newPantsInfo = Console.ReadLine();

                        Console.WriteLine("Enter price:");
                        var newPantsPrice = Decimal.Parse(Console.ReadLine());

                        var pantsSizeInput = "";
                        bool validPantsSize = true;

                        while (validPantsSize)
                        {
                            Console.WriteLine("Enter the clothing size (S, M, L, XL):");
                            pantsSizeInput = Console.ReadLine().ToUpper();

                            if (pantsSizeInput == "S" || pantsSizeInput == "M" || pantsSizeInput == "L" || pantsSizeInput == "XL")
                            {
                                validPantsSize = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid jersey size. Enter 'S', 'M', 'L' or 'XL'");
                            }
                        }

                        Console.WriteLine("Enter supplier name:");
                        var newPantsSupplier = Console.ReadLine();

                        Console.WriteLine("Enter number of pants in stock:");
                        var newPantsInStock = int.Parse(Console.ReadLine());

                        var newSelectedPoduct = false;

                        if (Enum.TryParse<WebShop.Models.ClothingSize>(pantsSizeInput, out var newPantSize))
                        {
                            var product = new Product
                            {
                                ProductName = newPantsName,
                                ProductInfo = newPantsInfo,
                                Price = newPantsPrice,
                                CategoryId = 1,
                                ClothingSize = newPantSize,
                                Supplier = newPantsSupplier,
                                Stock = newPantsInStock,
                                SelectedProduct = newSelectedPoduct
                            };

                            database.Add(product);
                            database.SaveChanges();
                            Console.WriteLine($"The pants {product.ProductName} have been added to the product database");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Invalid clothing size. Please enter a valid size.");
                        }

                        break;

                    case 'j':
                        Console.WriteLine("Enter name of jersey:");
                        var newJerseyName = Console.ReadLine();

                        Console.WriteLine("Enter product info:");
                        var newJerseyInfo = Console.ReadLine();

                        Console.WriteLine("Enter price:");
                        var newJerseyPrice = Decimal.Parse(Console.ReadLine());

                        var jerseySizeInput = "";
                        bool validJerseySize = true;

                        while (validJerseySize)
                        {
                            Console.WriteLine("Enter the clothing size (S, M, L, XL):");
                            jerseySizeInput = Console.ReadLine().ToUpper();

                            if (jerseySizeInput == "S" || jerseySizeInput == "M" || jerseySizeInput == "L" || jerseySizeInput == "XL")
                            {
                                validJerseySize = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid jersey size. Enter 'S', 'M', 'L' or 'XL'");
                            }
                        }

                        Console.WriteLine("Enter supplier name:");
                        var newJerseySupplier = Console.ReadLine();

                        Console.WriteLine("Enter number of jerseys in stock:");
                        var newJerseyInStock = int.Parse(Console.ReadLine());

                        var newJerseySelectedProduct = false;

                        if (Enum.TryParse<WebShop.Models.ClothingSize>(jerseySizeInput, out var newJerseySize))
                        {
                            var jersey = new Product
                            {
                                ProductName = newJerseyName,
                                ProductInfo = newJerseyInfo,
                                Price = newJerseyPrice,
                                CategoryId = 2,
                                ClothingSize = newJerseySize,
                                Supplier = newJerseySupplier,
                                Stock = newJerseyInStock,
                                SelectedProduct = newJerseySelectedProduct
                            };
                            database.Add(jersey);
                            database.SaveChanges();
                            Console.WriteLine($"The jersey {jersey.ProductName} has been added to the product database");
                            Console.ReadKey();
                            Console.Clear();
                        }

                        else
                        {
                            Console.WriteLine("Invalid clothing size. Please enter a valid size.");
                        }
                        break;

                    case 's':
                        Console.WriteLine("Enter name of shoes:");
                        var newShoesName = Console.ReadLine();

                        Console.WriteLine("Enter product info:");
                        var newShoesInfo = Console.ReadLine();

                        Console.WriteLine("Enter price:");
                        var newShoesPrice = Decimal.Parse(Console.ReadLine());


                        var shoeSizeInput = "";
                        bool validShoeSize = true;
                        while (validShoeSize)
                        {
                            Console.WriteLine("Enter the shoe size 36-46:");
                            shoeSizeInput = Console.ReadLine();
                            if ((int.Parse(shoeSizeInput) >= 36 && int.Parse(shoeSizeInput) <= 46))
                            {
                                shoeSizeInput = "EU" + shoeSizeInput;
                                validShoeSize = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid shoe size. Enter a size between 36 and 46.");
                            }
                        }

                        Console.WriteLine("Enter supplier name:");
                        var newShoesSupplier = Console.ReadLine();

                        Console.WriteLine("Enter number of shoes in stock:");
                        var newShoesInStock = int.Parse(Console.ReadLine());

                        var newShoesSelectedProduct = false;
                        if (Enum.TryParse<WebShop.Models.ShoeSize>(shoeSizeInput, out var newShoeSize))

                        {
                            var shoes = new Product
                            {
                                ProductName = newShoesName,
                                ProductInfo = newShoesInfo,
                                Price = newShoesPrice,
                                CategoryId = 3,
                                ShoeSize = newShoeSize,
                                Supplier = newShoesSupplier,
                                Stock = newShoesInStock,
                                SelectedProduct = newShoesSelectedProduct
                            };

                            database.Add(shoes);
                            database.SaveChanges();
                            Console.WriteLine($"The shoes {shoes.ProductName} have been added to the product database");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("\"Invalid Shoesize. Please enter a valid size");
                        }
                        break;
                }
            }
        }

        public static void DeleteCustomer()
        {
            Console.Clear();

            using (var database = new WebshopContext())
            {

                foreach (var customer in database.Customers)
                {
                    Console.WriteLine("Id:" + customer.Id + "\t Name:" + customer.FirstName + " " + customer.LastName.PadRight(15) + "\tAddress:" + customer.StreetAddress.PadRight(27) + "City:" + customer.City);
                }
                Console.WriteLine();
                Console.WriteLine("Write the ID of the customer you want to delete");
                var customerId = int.Parse(Console.ReadLine());

                var findCustomer = database.Customers.FirstOrDefault(i => i.Id == customerId);


                if (findCustomer != null)
                {
                    database.Customers.Remove(findCustomer);
                    database.SaveChanges();
                    Console.Write(" User with ID " + customerId + " has been removed.");

                }
                else
                {
                    Console.WriteLine(" No user with ID: " + customerId + " could be found.");
                }

            }
            Thread.Sleep(2000);
        }

        public static void DeleteProduct()
        {
            Console.Clear();

            using (var database = new WebshopContext())
            {

                foreach (var product in database.Products)
                {
                    Console.WriteLine("Id: " + product.Id + "\tName: " + product.ProductName.PadRight(35) + " " + product.Price.ToString().PadRight(7) + " SEK" + " \tCategoryId: " + product.CategoryId);
                }
                Console.WriteLine();
                Console.WriteLine("Write the Product Id that you want to delete");
                var productId = int.Parse(Console.ReadLine());

                var findProduct = database.Products.FirstOrDefault(i => i.Id == productId);


                if (findProduct != null)
                {
                    database.Products.Remove(findProduct);
                    database.SaveChanges();
                    Console.Write(" Product with Id " + productId + " has been removed.");
                }
                else
                {
                    Console.WriteLine(" No Product " + productId + " could be found.");
                }
            }
            Thread.Sleep(2000);

        }

        public static void EditCustomer()
        {
            Console.Clear();

            using (var database = new WebshopContext())
            {

                foreach (var customer in database.Customers)
                {
                    Console.WriteLine("Id:" + customer.Id + "\t Name:" + customer.FirstName + " " + customer.LastName.PadRight(15) + "\tAddress:" + customer.StreetAddress.PadRight(27) + "City:" + customer.City);
                }
                Console.WriteLine();
                Console.WriteLine("Write the ID of the customer you want to edit");
                var customerId = int.Parse(Console.ReadLine());

                var findCustomer = database.Customers.FirstOrDefault(i => i.Id == customerId);

                Console.Clear();
                Console.WriteLine("First Name");
                Console.WriteLine("Last Name");
                Console.WriteLine("Street address");
                Console.WriteLine("City");
                Console.WriteLine("Country");
                Console.WriteLine("Zipcode");
                Console.WriteLine("Phone");
                Console.WriteLine("Email");
                Console.WriteLine("Age");
                Console.WriteLine();
                Console.WriteLine("Write what you want to edit");
                var editCustomer = Console.ReadLine()?.ToLower(); // ? = method only called if input is not null

                Console.WriteLine("Write the new info");
                var newCustomerInfo = Console.ReadLine();

                switch (editCustomer)
                {
                    case "first name":
                        if (newCustomerInfo != null)
                        {
                            findCustomer.FirstName = newCustomerInfo;
                            database.SaveChanges();
                            Console.WriteLine("First name changed to " + newCustomerInfo);
                        }
                        break;
                    case "last name":
                        if (newCustomerInfo != null)
                        {
                            findCustomer.LastName = newCustomerInfo;
                            database.SaveChanges();
                            Console.WriteLine("Last name changed to " + newCustomerInfo);
                        }
                        break;
                    case "street address":
                        if (newCustomerInfo != null)
                        {
                            findCustomer.StreetAddress = newCustomerInfo;
                            database.SaveChanges();
                            Console.WriteLine("Street address changed to " + newCustomerInfo);
                        }
                        break;
                    case "city":
                        if (newCustomerInfo != null)
                        {
                            findCustomer.City = newCustomerInfo;
                            database.SaveChanges();
                            Console.WriteLine("City changed to " + newCustomerInfo);
                        }
                        break;
                    case "country":
                        if (newCustomerInfo != null)
                        {
                            findCustomer.Country = newCustomerInfo;
                            database.SaveChanges();
                            Console.WriteLine("Country changed to " + newCustomerInfo);
                        }
                        break;
                    case "zipcode":
                        if (newCustomerInfo != null)
                        {
                            findCustomer.Zipcode = newCustomerInfo;
                            database.SaveChanges();
                            Console.WriteLine("Zipcode changed to " + newCustomerInfo);
                        }
                        break;
                    case "phone":
                        if (newCustomerInfo != null)
                        {
                            findCustomer.Phone = newCustomerInfo;
                            database.SaveChanges();
                            Console.WriteLine("Phone changed to " + newCustomerInfo);
                        }
                        break;
                    case "email":
                        if (newCustomerInfo != null)
                        {
                            findCustomer.Email = newCustomerInfo;
                            database.SaveChanges();
                            Console.WriteLine("Email changed to " + newCustomerInfo);
                        }
                        break;
                    case "age":
                        if (newCustomerInfo != null)
                        {
                            if (int.TryParse(newCustomerInfo, out int newAge))
                            {
                                findCustomer.Age = newAge;
                                database.SaveChanges();
                                Console.WriteLine("Age changed to " + newCustomerInfo);
                            }
                            else
                            {
                                Console.WriteLine("Invalid age input ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid age input.");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid edit option");
                        break;
                }
                Thread.Sleep(2000);
            }
        }

        public static void PlaceOrder()
        {
            using (var database = new WebshopContext())
            {
                decimal sum = 0;

                Console.WriteLine("Products to checkout:");
                Console.WriteLine("--------------");

                if (shoppingCart.Count > 0)
                {
                    for (int i = 0; i < shoppingCart.Count(); i++)
                    {
                        Console.WriteLine("Id: " + i + " - " + shoppingCart[i].ProductName + ":\t" + shoppingCart[i].Price + " SEK");
                        sum += shoppingCart[i].Price;
                    }
                }
                else
                {
                    Console.WriteLine("There are no products in your cart. Press any key to go back.");
                    Console.ReadKey();
                    Helpers.Customer();
                }

                Console.WriteLine();
                Console.WriteLine("Payment methods");
                Console.WriteLine("--------------");

                var paymentMethods = database.Payments.Select(p => p.PaymentMethod).ToList();
                for (int i = 0; i < paymentMethods.Count(); i++)
                {
                    Console.WriteLine("Id: " + (i + 1) + "\t" + paymentMethods[i]);
                }
                Console.WriteLine();
                Console.WriteLine("Please enter the number of the desired payment method");
                bool paymentId = int.TryParse(Console.ReadLine(), out int paymentMethod);
                Console.WriteLine();

                Console.WriteLine("Please enter your email");
                string email = Console.ReadLine();
                Console.WriteLine();

                var deliveryMethods = database.Deliveries.Select(d => d.DeliveryMethod).ToList();
                var deliveryPrices = database.Deliveries.Select(d => d.Price).ToList();

                for (int i = 0; i < deliveryMethods.Count(); i++)
                {
                    Console.Write("Id: " + (i + 1) + "\t" + deliveryMethods[i]);
                    for (int j = i; j <= i; j++)
                    {
                        Console.WriteLine(", " + deliveryPrices[j] + " SEK");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Please enter the number of the desired delivery method");
                bool deliveryId = int.TryParse(Console.ReadLine(), out int deliveryMethod);
                Console.WriteLine();


                var payment = database.Payments.FirstOrDefault(p => p.Id == paymentMethod);
                var customer = database.Customers.FirstOrDefault(c => c.Email == email);
                var delivery = database.Deliveries.FirstOrDefault(d => d.Id == deliveryMethod);

                if (customer != null && payment != null && delivery != null)
                {
                    var order = new Order
                    {
                        CustomerId = customer.Id,
                        Sum = sum + delivery.Price,
                        Date = DateTime.Now,
                        PaymentId = payment.Id,
                        DeliveryId = delivery.Id,
                        Products = new List<Product>()
                    };

                    foreach (var item in shoppingCart)
                    {
                        var product = database.Products.Find(item.Id);
                        order.Products.Add(product);
                        product.Stock--;

                    }

                    Console.WriteLine($"Total: {order.Sum} inkl. moms 25% ({order.Sum * 25 / 100}) SEK");
                    Console.WriteLine("Press any key to place your order");
                    Console.ReadKey();
                    database.Orders.Add(order);
                    database.SaveChanges();
                    Console.WriteLine("Thank you! Your order has been placed");
                    shoppingCart.Clear();
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("No user or payment method found. Did you write your email correctly and choose an available payment method?");
                }
            }
        }

        public static void UpdateProduct()
        {
            Console.Clear();
            using (var database = new WebshopContext())
            {
                foreach (var product in database.Products)
                {
                    Console.WriteLine("Id: " + product.Id + "\tName: " + product.ProductName.PadRight(35) + " " + product.Price.ToString().PadRight(7) + " SEK" + " \tCategoryId: " + product.CategoryId);
                }
                Console.WriteLine();
                Console.WriteLine("Write the ID of the product you want to edit");
                var productId = int.TryParse(Console.ReadLine(), out int id);

                var findProduct = database.Products.FirstOrDefault(i => i.Id == id);

                Console.Clear();
                Console.WriteLine("Product name");
                Console.WriteLine("Product info");
                Console.WriteLine("Price");
                Console.WriteLine("Size");
                Console.WriteLine("Supplier");
                Console.WriteLine("Stock");
                Console.WriteLine("Selected product");

                Console.WriteLine();
                Console.WriteLine("Write what you want to edit");
                var editProduct = Console.ReadLine()?.ToLower(); // ? = method only called if input is not null

                Console.WriteLine("Write the new info");
                var newProductInfo = Console.ReadLine().ToUpper();

                switch (editProduct)
                {
                    case "product name":
                        if (newProductInfo != null)
                        {
                            findProduct.ProductName = newProductInfo;
                            database.SaveChanges();
                            Console.WriteLine("First name changed to " + newProductInfo);
                        }
                        break;
                    case "product info":
                        if (newProductInfo != null)
                        {
                            findProduct.ProductInfo = newProductInfo;
                            database.SaveChanges();
                            Console.WriteLine("Last name changed to " + newProductInfo);
                        }
                        break;
                    case "price":
                        if (newProductInfo != null)
                        {
                            if (decimal.TryParse(newProductInfo, out decimal newPrice))
                            {
                                findProduct.Price = newPrice;
                                database.SaveChanges();
                                Console.WriteLine("Price changed to " + newProductInfo);
                            }
                            else
                            {
                                Console.WriteLine("Invalid input ");
                            }
                        }
                        break;
                    case "size":
                        var newSize = "";
                        if (newProductInfo != null)
                        {
                            if (findProduct.CategoryId == 1 || findProduct.CategoryId == 2)
                            {
                                switch (newProductInfo)
                                {
                                    case "S":
                                        newSize = "36";
                                        break;
                                    case "M":
                                        newSize = "38";
                                        break;
                                    case "L":
                                        newSize = "40";
                                        break;
                                    case "XL":
                                        newSize = "42";
                                        break;
                                }
                                if (Enum.TryParse<WebShop.Models.ClothingSize>(newSize, out var newProductSize))
                                {
                                    findProduct.ClothingSize = newProductSize;
                                    database.SaveChanges();
                                    Console.WriteLine("Size changed to " + newProductSize);
                                }
                            }

                            if (findProduct.CategoryId == 3)
                            {
                                if (Enum.TryParse<WebShop.Models.ShoeSize>(newProductInfo, out var newShoeSize))
                                {
                                    findProduct.ShoeSize = newShoeSize;
                                    database.SaveChanges();
                                    Console.WriteLine("Size changed to " + newShoeSize);
                                }
                            }
                        }
                        break;
                    case "supplier":
                        if (newProductInfo != null)
                        {
                            findProduct.Supplier = newProductInfo;
                            database.SaveChanges();
                            Console.WriteLine("Supplier changed to " + newProductInfo);
                        }
                        break;
                    case "stock":
                        if (newProductInfo != null)
                        {
                            if (int.TryParse(newProductInfo, out int newStock))
                            {
                                findProduct.Stock = newStock;
                                database.SaveChanges();
                                Console.WriteLine("Stock changed to " + newProductInfo);
                            }
                            else
                            {
                                Console.WriteLine("Invalid input ");
                            }
                        }
                        break;
                    case "Selected product"://bool
                        Console.WriteLine("Do you want to show the product on the front page y/n");
                        var answer = Console.ReadLine().ToLower();
                        if (answer == "y")
                        {
                            findProduct.SelectedProduct = true;
                            database.SaveChanges();
                        }
                        else
                        {
                            findProduct.SelectedProduct = false;
                            database.SaveChanges();
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid edit option");
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
                Console.Clear();
            }
        }
        //############################################################

        //############################################################
        //ADD BULK TEST DATA
        public static void AddCategories()
        {
            using (var database = new WebshopContext())
            {
                if (!database.Categories.Any())
                {
                    var categories = new List<Category>
            {
                new Category {Name = "Pants" },
                new Category {Name = "Jersey" },
                new Category {Name = "Shoes" }
            };

                    database.Categories.AddRange(categories);
                    database.SaveChanges();
                }
            }
        }

        public static void AddDeliveries()
        {
            using (var database = new WebshopContext())
            {
                if (!database.Deliveries.Any())
                {
                    database.Deliveries.AddRange(
                                        new Delivery
                                        {
                                            DeliveryMethod = "PostNord",
                                            Price = 0
                                        },
                                        new Delivery
                                        {
                                            DeliveryMethod = "Instabox",
                                            Price = 39
                                        },
                                        new Delivery
                                        {
                                            DeliveryMethod = "UPS Express",
                                            Price = 69
                                        });
                    database.SaveChanges();
                }
            }
        }

        public static void AddPayments()
        {
            using (var database = new WebshopContext())
            {
                if (!database.Payments.Any())
                {
                    database.Payments.AddRange(
                 new Payment
                 {
                     PaymentMethod = "Credit Card",
                 },
                 new Payment
                 {
                     PaymentMethod = "Swish",
                 }, new Payment
                 {
                     PaymentMethod = "Klarna",
                 });
                    database.SaveChanges();
                }
            }
        }

        public static void AddTestData()
        {
            using (var database = new WebshopContext())
            {
                if (!database.Customers.Any())
                {
                    database.Customers.AddRange(
                    new Customer
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        StreetAddress = "Vintergatan 1",
                        City = "Arjeplog",
                        Country = "Sweden",
                        Zipcode = "95120",
                        Phone = "0724976509",
                        Email = "john.doe@example.com",
                        Age = 63

                    },
                    new Customer
                    {
                        FirstName = "Emma",
                        LastName = "Andersson",
                        StreetAddress = "Björklundsvägen 12",
                        City = "Göteborg",
                        Country = "Sweden",
                        Zipcode = "41120",
                        Phone = "0738123490",
                        Email = "emma.andersson@example.com",
                        Age = 28
                    },
                    new Customer
                    {
                        FirstName = "Oscar",
                        LastName = "Lindgren",
                        StreetAddress = "Skogsvägen 7",
                        City = "Stockholm",
                        Country = "Sweden",
                        Zipcode = "11230",
                        Phone = "0765128765",
                        Email = "oscar.lindgren@example.com",
                        Age = 35
                    },
                    new Customer
                    {
                        FirstName = "Eva",
                        LastName = "Svensson",
                        StreetAddress = "Liljevägen 3",
                        City = "Malmö",
                        Country = "Sweden",
                        Zipcode = "21145",
                        Phone = "0709356821",
                        Email = "eva.svensson@example.com",
                        Age = 42
                    },
                    new Customer
                    {
                        FirstName = "Anders",
                        LastName = "Johansson",
                        StreetAddress = "Ekbacksvägen 9",
                        City = "Uppsala",
                        Country = "Sweden",
                        Zipcode = "75210",
                        Phone = "0726351948",
                        Email = "anders.johansson@example.com",
                        Age = 50
                    });
                    database.SaveChanges();
                }

                if (!database.Products.Any())
                {
                    database.Products.AddRange(
                    new Product
                    {
                        ProductName = "Blue Jeans",
                        ProductInfo = "Strong blue jeans work hard working people",
                        Price = 1299,
                        CategoryId = 1,
                        ClothingSize = ClothingSize.S,
                        Supplier = "Lee",
                        Stock = 25,
                        SelectedProduct = true
                    },
                    new Product
                    {
                        ProductName = "Green Chinos",
                        ProductInfo = "Stylish green chinos to parade at Stureplan",
                        Price = 899,
                        CategoryId = 1,
                        ClothingSize = ClothingSize.M,
                        Supplier = "Arket",
                        Stock = 32,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "Black Sweatpants",
                        ProductInfo = "Comfortable sweatpants ideal to relax at home in front of the fire",
                        Price = 499,
                        CategoryId = 1,
                        ClothingSize = ClothingSize.L,
                        Supplier = "Champion",
                        Stock = 13,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "College Sweater Harvard",
                        ProductInfo = "Great college sweater from Harvard University",
                        Price = 999,
                        CategoryId = 2,
                        ClothingSize = ClothingSize.XL,
                        Supplier = "Harvard University",
                        Stock = 45,
                        SelectedProduct = true
                    },
                    new Product
                    {
                        ProductName = "Wool knitted Icelandic sweater",
                        ProductInfo = "Warm Icelandic sweater for winter's coldest days",
                        Price = 2399,
                        CategoryId = 2,
                        ClothingSize = ClothingSize.S,
                        Supplier = "Mormors Verkstad",
                        Stock = 20,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "Training sweater with zipper",
                        ProductInfo = "Practical trainer sweater with zipper to easily take of at the gym",
                        Price = 599,
                        CategoryId = 2,
                        ClothingSize = ClothingSize.L,
                        Supplier = "Adidas",
                        Stock = 80,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "Dr Martens brown winter boots",
                        ProductInfo = "Salt and sand resistant boots from Dr Martens",
                        Price = 1799,
                        CategoryId = 3,
                        ShoeSize = ShoeSize.EU42,
                        Supplier = "Dr Martens",
                        Stock = 37,
                        SelectedProduct = true
                    },
                    new Product
                    {
                        ProductName = "White and green Adidas Stan Smith",
                        ProductInfo = "Limited edition Stan Smith",
                        Price = 1199,
                        CategoryId = 3,
                        ShoeSize = ShoeSize.EU37,
                        Supplier = "Dr Martens",
                        Stock = 10,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "Flip Flops",
                        ProductInfo = "The only pair of flip flops you will ever need",
                        Price = 399,
                        CategoryId = 3,
                        ShoeSize = ShoeSize.EU40,
                        Supplier = "FlipFlop Kungen",
                        Stock = 93,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "Black suit pants",
                        ProductInfo = "Classy suit pants for special occasions",
                        Price = 999,
                        CategoryId = 1,
                        ClothingSize = ClothingSize.L,
                        Supplier = "Tiger",
                        Stock = 50,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "Pyjamas pants",
                        ProductInfo = "Comfortable cotton pyjamas pants",
                        Price = 249,
                        CategoryId = 1,
                        ClothingSize = ClothingSize.L,
                        Supplier = "Dressmann",
                        Stock = 20,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "Knitted cardigan",
                        ProductInfo = "Cardigan in luxurious alpaca wool",
                        Price = 1299,
                        CategoryId = 2,
                        ClothingSize = ClothingSize.XL,
                        Supplier = "Mormors verkstad",
                        Stock = 5,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "WWF hoodie",
                        ProductInfo = "Hoddie by WWF, 100 SEK per purchase will be donated to WWF",
                        Price = 1199,
                        CategoryId = 2,
                        ClothingSize = ClothingSize.S,
                        Supplier = "WWF",
                        Stock = 43,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "IceBugs GT Pro Max",
                        ProductInfo = "Latest technology from IceBugs",
                        Price = 3499,
                        CategoryId = 3,
                        ShoeSize = ShoeSize.EU41,
                        Supplier = "IceBugs",
                        Stock = 39,
                        SelectedProduct = false
                    },
                    new Product
                    {
                        ProductName = "Summer of '69 New Balance trainers",
                        ProductInfo = "Reedition of classic New Balance model",
                        Price = 1149,
                        CategoryId = 3,
                        ShoeSize = ShoeSize.EU38,
                        Supplier = "NewBalance",
                        Stock = 73,
                        SelectedProduct = false
                    });
                    database.SaveChanges();
                }
            }
        }

        //public static void AddExtraData()
        //{
        //    using (var database = new WebshopContext())
        //    {
        //        database.Products.AddRange(
        //        new Product
        //        {
        //            ProductName = "Black suit pants",
        //            ProductInfo = "Classy suit pants for special occasions",
        //            Price = 999,
        //            CategoryId = 1,
        //            ClothingSize = ClothingSize.L,
        //            Supplier = "Tiger",
        //            Stock = 50,
        //            SelectedProduct = false
        //        },
        //        new Product
        //        {
        //            ProductName = "Pyjamas pants",
        //            ProductInfo = "Comfortable cotton pyjamas pants",
        //            Price = 249,
        //            CategoryId = 1,
        //            ClothingSize = ClothingSize.L,
        //            Supplier = "Dressmann",
        //            Stock = 20,
        //            SelectedProduct = false
        //        },
        //        new Product
        //        {
        //            ProductName = "Knitted cardigan",
        //            ProductInfo = "Cardigan in luxurious alpaca wool",
        //            Price = 1299,
        //            CategoryId = 2,
        //            ClothingSize = ClothingSize.XL,
        //            Supplier = "Mormors verkstad",
        //            Stock = 5,
        //            SelectedProduct = false
        //        },
        //        new Product
        //        {
        //            ProductName = "WWF hoodie",
        //            ProductInfo = "Hoddie by WWF, 100 SEK per purchase will be donated to WWF",
        //            Price = 1199,
        //            CategoryId = 2,
        //            ClothingSize = ClothingSize.S,
        //            Supplier = "WWF",
        //            Stock = 43,
        //            SelectedProduct = false
        //        },
        //        new Product
        //        {
        //            ProductName = "IceBugs GT Pro Max",
        //            ProductInfo = "Latest technology from IceBugs",
        //            Price = 3499,
        //            CategoryId = 3,
        //            ShoeSize = ShoeSize.EU41,
        //            Supplier = "IceBugs",
        //            Stock = 39,
        //            SelectedProduct = false
        //        },
        //        new Product
        //        {
        //            ProductName = "Summer of '69 New Balance trainers",
        //            ProductInfo = "Reedition of classic New Balance model",
        //            Price = 1149,
        //            CategoryId = 3,
        //            ShoeSize = ShoeSize.EU38,
        //            Supplier = "NewBalance",
        //            Stock = 73,
        //            SelectedProduct = false
        //        });
        //        database.SaveChanges();
        //    }
        //}

        //############################################################
        //############################################################

    }
}