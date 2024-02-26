using E_Commerce_Backend.Models.DTOs.CategoryDTOs;

namespace E_Commerce_Backend.Services.CategoryServices
{
    public interface ICategoryServices
    {
        Task AddCategory(CategoryDTO categoryDTO);
        Task RemoveCategory(int id);
        Task UpdateCategory(int id, CategoryDTO categoryDTO);
        Task<List<CategoryViewDTO>> GetAllCategories();
        Task<CategoryViewDTO> GetCategoryById(int id);
    }
}
