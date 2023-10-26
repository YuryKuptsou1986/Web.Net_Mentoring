using ViewModel.Product;
using ViewModel.Category;
using Microsoft.Extensions.Configuration;
using ConsoleWebApiClient.Providers;
using ConsoleWebApiClient.Clients;

namespace ConsoleWebApiClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var settingsProvider = new SettingsProvider(new ConfigurationBuilder());
            var httpClientProvider = new HttpClientProvider();

            var webApiClient = new WebApiClient(httpClientProvider, settingsProvider);

            var settings = settingsProvider.Provide();

            var products = await webApiClient.GetEntitiesAsync<ProductViewModel>(settings.GetProductsEndpoint);
            var categories = await webApiClient.GetEntitiesAsync<CategoryViewModel>(settings.GetCategoriesEndpoint);

            PrintCategories(categories);
            PrintProducts(products);

            Console.ReadLine();
        }

        static void PrintProducts(IEnumerable<ProductViewModel> products) 
        { 
            if(products == null || !products.Any()) {
                Console.WriteLine("Empty products list");
            }

            Console.WriteLine("######################################################");
            Console.WriteLine("#########             Products               #########");
            Console.WriteLine("######################################################");

            foreach (var product in products)
            {
                Console.WriteLine($"id = {product.ProductId}, productName = {product.ProductName}, quantityPerUnit = {product.QuantityPerUnit}");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        static void PrintCategories(IEnumerable<CategoryViewModel> categories) 
        {
            if (categories == null || !categories.Any()) {
                Console.WriteLine("Empty categories list");
            }

            Console.WriteLine("######################################################");
            Console.WriteLine("#########            Categories              #########");
            Console.WriteLine("######################################################");

            foreach (var category in categories) {
                Console.WriteLine($"id = {category.CategoryId}, categoryName = {category.CategoryName}, description = {category.Description}");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}