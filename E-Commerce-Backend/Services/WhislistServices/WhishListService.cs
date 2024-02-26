using AutoMapper;
using E_Commerce_Backend.Dbcontext;
using E_Commerce_Backend.Jwt;
using E_Commerce_Backend.Models.DTOs.WhishListDTOs;
using E_Commerce_Backend.Models.ENTITYS;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services.WhislistServices
{
    public class WhishListService : IWhishListServices
    {
        private readonly ShoesDbcontext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string HostUrl;
        private readonly IJwtServices _jwtServices;
        public WhishListService(ShoesDbcontext dbContext, IMapper mapper, IConfiguration configuration, IJwtServices jwtServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            HostUrl = _configuration["HostUrl:url"];
            _jwtServices = jwtServices;
        }

        public async Task<bool> AddToWhishlist(string token, int productid)
        {
            int userid = _jwtServices.GetUserIdFromToken(token);
            if (userid == null)
            {
                throw new Exception("user id not valid");
            }
            var whishing = await _dbContext.whishLists.Include(p => p.products).FirstOrDefaultAsync(p => p.ProductId == productid && p.UserId==userid);
        
            if (whishing == null)
            {
                var whishListdto = new WhishListDTO
                {
                    UserId = userid,
                    ProductId = productid
                };
                var WhishMapper=_mapper.Map<WhishList>(whishListdto);
                await _dbContext.whishLists.AddAsync(WhishMapper);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            _dbContext.whishLists.Remove(whishing);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<WhishListViewDTO>> GetWhishList(string token)
        {
            int userid = _jwtServices.GetUserIdFromToken(token);
            if (userid == null)
            {
                throw new Exception("user id not valid");
            }
            var wishlist=await _dbContext.whishLists.Include(p=>p.products).ThenInclude(c=>c.Category).Where(u=>u.UserId == userid).ToListAsync();
            if(wishlist != null)
            {
                var viewwhislist = wishlist.Select(w => new WhishListViewDTO
                {
                    Id = w.Id,
                    ProductName = w.products.ProductName,
                    ProductDescription = w.products.ProductDescription,
                    ProductType = w.products.ProductType,
                    Price = w.products.Price,
                    Category=w.products?.Category.Name,
                    ProductImage=HostUrl + w.products.ProductImage
                }).ToList();
                return viewwhislist;
            }
            return new List<WhishListViewDTO>();
        }

        public async Task RemoveFromWhishlist(string token, int productid)
        {
            int userid = _jwtServices.GetUserIdFromToken(token);
            if (userid == null)
            {
                throw new Exception("user id not valid");
            }
            var whislist = await _dbContext.whishLists.FirstOrDefaultAsync(i => i.UserId == userid && i.ProductId == productid);
            if(whislist != null)
            {
                _dbContext.whishLists.Remove(whislist);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
