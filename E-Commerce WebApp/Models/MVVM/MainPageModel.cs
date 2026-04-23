namespace E_Commerce_WebApp.Models.MVVM
{
    public class MainPageModel
    {
        public List<Product>? SliderProducts { get; set; } //slider 
        public List<Product>? NewProducts { get; set; } //new products
        public List<Product>? SpecialProducts { get; set; } //özel
        public List<Product>? DiscountedProducts { get; set; }//indirimli
        public List<Product>? HighlightedProducts { get; set; }//öne cıkanlar
        public List<Product>? TopsellerProducts { get; set; } //cok satanlar
        public List<Product>? StarProducts { get; set; } //yıldızlı ürünler
        public List<Product>? OpportunityProducts { get; set; } //fırsat ürünler
        public List<Product>? NotableProducts { get; set; } //dikkat ceken
        public Product? Productofday { get; set; } // Günün ürünü
        public Product? ProductDetails { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public List<Product>? RelatedProducts { get; set; }
    }
}
