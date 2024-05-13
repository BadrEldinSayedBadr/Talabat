namespace Talabat.APIs.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }

        public int BrandId { get; set; }
        public string ProductBrand { get; set;}
        public int TypeId { get; set; }
        public string ProductType { get; set; }
    }
}
