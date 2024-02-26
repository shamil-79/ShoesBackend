using E_Commerce_Backend.Models.DTOs.CategoryDTOs;
using E_Commerce_Backend.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _category;
        public CategoryController(ICategoryServices category)
        {
            _category = category;
        }
        [HttpGet]
        public async Task<IActionResult> GetallCategory()
        {
            try
            {
                return Ok(await _category.GetAllCategories());
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }

        }
        [HttpGet("ById")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                return Ok(await _category.GetCategoryById(id));
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                await _category.AddCategory(categoryDTO);
                return Ok();
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> update(int id,[FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                await _category.UpdateCategory(id, categoryDTO);
                return Ok("Updated Success");

            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                await _category.RemoveCategory(id);
                return Ok("Successfully Deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
