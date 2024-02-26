namespace E_Commerce_Backend.Models.DTOs.WhishListDTOs
{
    public class WhishListViewDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
        public string ProductType { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public string ProductImage { get; set; }
    }
}
