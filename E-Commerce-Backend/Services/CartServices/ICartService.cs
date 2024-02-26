using E_Commerce_Backend.Models.DTOs.CartDTOs;

namespace E_Commerce_Backend.Services.CartServices
{
    public interface ICartService
    {
        Task AddtoCart(string token, int productid);
        Task DeleteCart(string token, int productid);
        Task Quanityplus(string token, int productid);
        Task Quanityminus(string token, int productid);
        public Task<List<CartViewDto>> GetallCartitem(string token);
    }
}
