namespace grease_ranker_api.DTOs
{
    public class Product
    {
        public string? Name { get; set; }
        public int Rank { get; set; }
        public string? ImageUrl { get; set; }
        public string? Url { get; set; }
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal MagicNumber { get; set; }
    }
}
