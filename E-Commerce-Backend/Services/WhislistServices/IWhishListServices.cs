using E_Commerce_Backend.Models.DTOs.WhishListDTOs;

namespace E_Commerce_Backend.Services.WhislistServices
{
    public interface IWhishListServices
    {
        Task<bool> AddToWhishlist(string token, int productid);
        Task RemoveFromWhishlist(string token, int productid);
        Task<List<WhishListViewDTO>> GetWhishList(string token);
    }
}
