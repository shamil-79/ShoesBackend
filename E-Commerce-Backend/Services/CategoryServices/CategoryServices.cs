using AutoMapper;
using E_Commerce_Backend.Dbcontext;
using E_Commerce_Backend.Models.DTOs.CategoryDTOs;
using E_Commerce_Backend.Models.ENTITYS;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services.CategoryServices
{
    public class CategoryServices:ICategoryServices
    {
        private readonly ShoesDbcontext _dbcontext;
        private readonly IMapper _mapper;
        public CategoryServices(ShoesDbcontext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task AddCategory (CategoryDTO categoryDTO)
        {
            var cate=_mapper.Map<Category>(categoryDTO);
            await _dbcontext.categories.AddAsync(cate);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<CategoryViewDTO>> GetAllCategories()
        {
            var allCategory= await _dbcontext.categories.ToListAsync();
            return _mapper.Map<List<CategoryViewDTO>>(allCategory);
           
        }

        public async Task<CategoryViewDTO> GetCategoryById(int id)
        {
            var cateByID=await _dbcontext.categories.FirstOrDefaultAsync(c => c.Id==id);
            return _mapper.Map<CategoryViewDTO>(cateByID);
            
        }

        public async Task RemoveCategory(int id)
        {
            var dele= await _dbcontext.categories.FirstOrDefaultAsync(c=>c.Id==id);
            if (dele!=null)
            {
                _dbcontext.categories.Remove(dele);
                await _dbcontext.SaveChangesAsync();

            }
            
            
        }

        public async Task UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            var update= await _dbcontext.categories.FirstOrDefaultAsync(u=>u.Id==id);
            if (update != null)
            {
                update.Name= categoryDTO.Name;
               await _dbcontext.SaveChangesAsync();


            }
        }
    }
}
