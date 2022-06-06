using grease_ranker_api.DTOs;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace grease_ranker_api.Services
{
    public class McDonaldsService
    {
        private readonly string Filename = "mcDonalds_products.json";
        private readonly string ProductsUrl = "https://www.mcdonalds.at/unsere-produkte";

        private readonly Regex rProducts = new(@"href=""(?<url>https:\/\/www\.mcdonalds\.at\/produkt\/[a-zA-Z0-9-]+)""");
        private readonly Regex rName = new(@"<h1>(?<name>.+)<\/h1>");
        private readonly Regex rImageUrl = new(@"<img[^>]*?src\s*=\s*[""']?(?<url>[^'"" >]+?1500x1500[^'"" >]+?)[ '""][^>]*?>");
        private readonly Regex rContent = new(@"<b>Portion<\/b>(?<content>.+?)<b>per 100g<\/b>");
        private readonly Regex rCalories = new(@"<b>(?<calories>\d*\.*?\d)kcal");
        private readonly Regex rProtein = new(@"Eiweiß<\/b><\/p><p><b>(?<protein>\d*\.*?\d)g");

        public async Task<IEnumerable<Product>> GetProducts()
        {
            if (File.Exists(Filename))
            {
                string data = await File.ReadAllTextAsync(Filename);
                return JsonConvert.DeserializeObject<List<Product>>(data) ?? Enumerable.Empty<Product>();
            }
            else
            {
                var urls = await GetProductUrls();
                var products = await GetProducts(urls);
                string data = JsonConvert.SerializeObject(products);
                await File.WriteAllTextAsync(Filename, data);
                return products;
            }
        }

        public async void UpdateProducts()
        {
            var urls = await GetProductUrls();
            var products = await GetProducts(urls);
            string data = JsonConvert.SerializeObject(products);
            await File.WriteAllTextAsync(Filename, data);
        }

        private async Task<List<Product>> GetProducts(List<string> urls)
        {
            var products = new List<Product>();

            foreach (var url in urls)
            {
                var p = await GetProductByUrl(url);
                if (p is null || p.MagicNumber == -1 || p.Protein <= 0) continue;
                products.Add(p);
            }

            products = products
                .GroupBy(x => x.Name)
                .Select(x => x.First())
                .OrderBy(x => x.MagicNumber)
                .ToList();

            int i = 1;
            products.ForEach(x => x.Rank = i++);

            return products;
        }

        private async Task<Product?> GetProductByUrl(string url)
        {
            string html = await GetHtmlByUrl(url);
            html = Regex.Replace(html, @"\r\n?|\n|\t", "");

            string name = rName.Match(html).Groups["name"].ToString()
                .Replace("<br>", " ")
                .Replace("&#8222;", "'")
                .Replace("&#8220;", "'")
                .Replace("&#038;", "&");
            string content = rContent.Match(html).Groups["content"].ToString();

            try
            {
                string imageUrl = rImageUrl.Match(html).Groups["url"].ToString().Replace("768", "480");
                decimal calories = decimal.Parse(rCalories.Match(content).Groups["calories"].ToString().Replace('.', ','));
                decimal protein = decimal.Parse(rProtein.Match(content).Groups["protein"].ToString().Replace('.', ','));
                if (protein <= 0) return null;
                decimal magicNumber = Math.Round(Convert.ToDecimal(calories / protein * 100)) / 100;

                return new Product
                {
                    Url = url,
                    Rank = -1,
                    Name = name,
                    ImageUrl = imageUrl,
                    Calories = calories,
                    Protein = protein,
                    MagicNumber = magicNumber,
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(url);

                return null;
            }
        }

        private async Task<List<string>> GetProductUrls()
        {
            var html = await GetHtmlByUrl(ProductsUrl);
            var urls = new List<string>();

            var matches = rProducts.Matches(html);
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                urls.Add(groups["url"].Value);
            }

            return urls;
        }

        private static async Task<string> GetHtmlByUrl(string productsUrl)
        {
            using var client = new HttpClient();
            using HttpResponseMessage response = client.GetAsync(productsUrl).Result;
            using HttpContent content = response.Content;
            return await content.ReadAsStringAsync();
        }
    }
}
