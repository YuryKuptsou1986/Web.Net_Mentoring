// See https://aka.ms/new-console-template for more information

using KiotaWebApi.Client;
using KiotaWebApi.Client.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace KiotaWebApi
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var authProvider = new AnonymousAuthenticationProvider();
            var adapter = new HttpClientRequestAdapter(authProvider);
            adapter.BaseUrl = "https://localhost:7195";
            var client = new KiotaWebApiClient(adapter);

            try {
                // GET /products
                var products = await client.Api.Products.GetAsync();
                PrintProducts(products);

                var categories = await client.Api.Categories.GetAsync();
                PrintCategories(categories);

            } catch (Exception ex) {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadLine();
        }

        static void PrintProducts(IEnumerable<ProductViewModel> products)
        {
            if (products == null || !products.Any()) {
                Console.WriteLine("Empty products list");
            }

            Console.WriteLine("######################################################");
            Console.WriteLine("#########             Products               #########");
            Console.WriteLine("######################################################");

            foreach (var product in products) {
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

