using E_Commerce_Backend.Models.DTOs.ProductDTOs;

namespace E_Commerce_Backend.Services.ProductServices
{
    public interface IProductServices
    {
        Task  AddProduct(ProductDTO productDTO,IFormFile image);
        Task DeleteProduct(int id);
        Task UpdateProduct(int id,ProductDTO productDTO,IFormFile image);
        public Task<List<ProductVieweDTO>> GetAllProducts();
        public Task<ProductVieweDTO> GetProductById(int id);
        public Task<List<ProductVieweDTO>> GetProductByCategory(string categoryName);
        public Task<List<ProductVieweDTO>> ProductPagination(int pagenum = 1, int pagesize = 10);
        public Task<List<ProductVieweDTO>> PaginationByCategory(int catagoryid,int pagenum = 1, int pagesize = 10);
        Task<List<ProductVieweDTO>> SearchProduct(string searchItem);
    }
}
