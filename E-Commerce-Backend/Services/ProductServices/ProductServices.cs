using AutoMapper;
using E_Commerce_Backend.Dbcontext;
using E_Commerce_Backend.Models.DTOs.ProductDTOs;
using E_Commerce_Backend.Models.ENTITYS;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services.ProductServices
{
    public class ProductServices : IProductServices
    {
        private readonly ShoesDbcontext _dbcontext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string HostUrl;
        
        public ProductServices(ShoesDbcontext dbcontext,IMapper mapper,IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            HostUrl = _configuration["HostUrl:url"];

        }

        public async Task AddProduct(ProductDTO productDTO, IFormFile image)
        {
            try
            {
                string productimage = null;
                if (image != null && image.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Product", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    productimage = "/Images/Product/" + fileName;
                }
                else
                {
                    productimage = "/Images/common/noimage.png";
                }
                var pro_duct = _mapper.Map<Products>(productDTO);
                pro_duct.ProductImage = productimage;

                await _dbcontext.product.AddAsync(pro_duct);
                await _dbcontext.SaveChangesAsync();




            }catch (Exception ex)
            {
                throw new Exception("Error adding product: " + ex.Message, ex);

            }
        }

        public async Task DeleteProduct(int id)
        {
            var deleteproduct=await _dbcontext.product.FirstOrDefaultAsync(x => x.Id == id);
            _dbcontext.product.Remove(deleteproduct);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<ProductVieweDTO>> GetAllProducts()
        {
            var allproduct=await _dbcontext.product.Include(c=>c.Category).ToListAsync();
            if (allproduct.Count > 0)
            {
                var productswithcategory = allproduct.Select(p => new ProductVieweDTO
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductType = p.ProductType,
                    ProductImage =HostUrl + p.ProductImage,
                    Price = p.Price,
                    Category=p.Category.Name

                }).ToList();
                return productswithcategory;
            }
            return new List<ProductVieweDTO>();
        }

        public async Task<List<ProductVieweDTO>> GetProductByCategory(string categoryName)
        {
            var products =await _dbcontext.product.Include(c => c.Category).Where(c => c.Category.Name == categoryName).Select(p => new ProductVieweDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductType = p.ProductType,
                ProductImage = HostUrl + p.ProductImage,
                Price = p.Price,
                Category = p.Category.Name

            }).ToListAsync();
            if (products != null)
            {
                return products;
            }
            return new List<ProductVieweDTO>();
        }

        public async Task<ProductVieweDTO> GetProductById(int id)
        {
            var productbyid = await _dbcontext.product.Include(c => c.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (productbyid != null)
            {
                ProductVieweDTO oneproduct = new ProductVieweDTO
                {
                    Id= productbyid.Id,
                    ProductName = productbyid.ProductName,
                    ProductDescription = productbyid.ProductDescription,
                    ProductType = productbyid.ProductType,
                    ProductImage=HostUrl + productbyid.ProductImage,
                    Price = productbyid.Price,
                    Category = productbyid.Category.Name
                };
                return oneproduct;
            }
            return new ProductVieweDTO();
        }

        public async Task<List<ProductVieweDTO>> PaginationByCategory(int catagoryid, int pagenum = 1, int pagesize = 10)
        {
            var pagedbycategoryproducts = await _dbcontext.product.Include(p => p.Category).Where(p => p.Category.Id == catagoryid).Skip((pagenum - 1) * pagesize).Take(pagesize).Select(p => new ProductVieweDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductType = p.ProductType,
                ProductImage= HostUrl + p.ProductImage,
                Price = p.Price,
                Category = p.Category.Name
            }).ToListAsync();
            if (pagedbycategoryproducts != null)
            {
                return pagedbycategoryproducts;
            }
            return new List<ProductVieweDTO>();
        }

        public async Task<List<ProductVieweDTO>> ProductPagination(int pagenum = 1, int pagesize = 10)
        {
            var products = await _dbcontext.product.Include(p => p.Category).Skip((pagenum - 1) * pagesize).Take(pagesize).ToListAsync();
            var pagedproducts=products.Select(p => new ProductVieweDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductType = p.ProductType,
                ProductImage = HostUrl + p.ProductImage,
                Price = p.Price,
                Category = p.Category.Name
            }).ToList();

            return pagedproducts;

        }

        public async Task UpdateProduct(int id, ProductDTO productDTO, IFormFile image)
        {
            try
            {
                var product = await _dbcontext.product.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    product.ProductName = productDTO.ProductName;
                    product.ProductDescription = productDTO.ProductDescription;
                    product.ProductType = productDTO.ProductType;
                    product.Price = productDTO.Price;
                    product.CategoryId = productDTO.CategoryId;
                    if (image != null && image.Length > 0)
                    {

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Product", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        product.ProductImage = "/Images/Product/" + fileName;
                    }
                    else
                    {
                        product.ProductImage = product.ProductImage;
                    }
                    await _dbcontext.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException($"Product with ID {id} not found.");
                }
            }catch (Exception ex)
            {
                throw new Exception($"Error updating product with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<List<ProductVieweDTO>> SearchProduct(string searchItem)
        {
            var product= await _dbcontext.product.Where(p=>p.ProductName.Contains(searchItem)).ToListAsync();
            if(product != null)
            {
                return product.Select(p=> new ProductVieweDTO
                {
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductType = p.ProductType,
                    Price= p.Price,
                    ProductImage =HostUrl + p.ProductImage,

                }).ToList();
            }
            return new List<ProductVieweDTO>();
        }
    }
}
