namespace WebShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Following 4 methods will only run if database is not populated ↴
            Helpers.AddDeliveries();
            Helpers.AddPayments();
            Helpers.AddCategories();
            Helpers.AddTestData();


            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("╔════════════════════════╗");
                Console.WriteLine("║                        ║");
                Console.WriteLine("║     Welcome to our     ║");
                Console.WriteLine("║        webshop         ║");
                Console.WriteLine("║                        ║");
                Console.WriteLine("╚════════════════════════╝");
                Console.WriteLine();

                //Micke's magic window
                Helpers.ShowWindow();

                Console.WriteLine("Press 'a' for Admin");
                Console.WriteLine("Press 'c' for Customer");

                var key = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (key)
                {
                    case 'a':
                        Helpers.Admin();
                        break;

                    case 'c':
                        Helpers.Customer();
                        break;
                }
            }
        }
    }
}