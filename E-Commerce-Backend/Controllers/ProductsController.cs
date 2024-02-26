 using E_Commerce_Backend.Models.DTOs.ProductDTOs;
using E_Commerce_Backend.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductServices _productServices;
        public ProductsController(IWebHostEnvironment webHostEnvironment, IProductServices productServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _productServices = productServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetallProduct()
        {
            try
            {
                var products = await _productServices.GetAllProducts();
                return Ok(products);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}", Name = "getproduct")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productServices.GetProductById(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("paged-product")]
        public async Task<IActionResult> Getpaged(int pagenum, int pagesize)
        {
            try
            {
                var product = await _productServices.ProductPagination(pagenum, pagesize);
                return Ok(product);


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("paged-by-category")]
        public async Task<IActionResult>pagedbycategory(int categoryid,int pagenum,int pagesize)
        {
            try
            {
                
                return Ok(await _productServices.PaginationByCategory(categoryid, pagenum, pagesize));
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult>Addproduct([FromForm]ProductDTO productDTO, IFormFile image)
        {
            try
            {
                await _productServices.AddProduct(productDTO, image);
                return Ok("new Product Added successfuuly");
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(int id,[FromForm] ProductDTO productDTO, IFormFile image)
        {
            try
            {
                await _productServices.UpdateProduct(id, productDTO, image);
                return Ok("Updated successfully");
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productServices.DeleteProduct(id);  
                return Ok("removed successfully");
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
