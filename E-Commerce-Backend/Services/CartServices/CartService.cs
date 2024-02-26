
using AutoMapper;
using E_Commerce_Backend.Dbcontext;
using E_Commerce_Backend.Jwt;
using E_Commerce_Backend.Models.DTOs.CartDTOs;
using E_Commerce_Backend.Models.ENTITYS;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly ShoesDbcontext _dbcontext;
        private readonly IMapper _mapper;
        private readonly IJwtServices _jwtServices;
        public readonly string HostUrl;

        public CartService(ShoesDbcontext shoesDbcontext, IMapper mapper, IJwtServices jwtServices,IConfiguration configuration)
        {
            _dbcontext = shoesDbcontext;
            _mapper = mapper;
            _jwtServices = jwtServices;
            HostUrl = configuration["HostUrl:url"];
        }

        public async Task AddtoCart(string token, int productid)
        {
            int userId = _jwtServices.GetUserIdFromToken(token);

            if (userId == null)
            {
                throw new Exception("user id not valid");
            }
            var user=await _dbcontext.users.Include(c=>c.cart).ThenInclude(p=>p.cartItems).FirstOrDefaultAsync(i=>i.Id==userId);
            var product = await _dbcontext.product.FirstOrDefaultAsync(p=>p.Id==productid);
            if (user!=null && product!=null)
            {
                if(user.cart==null)
                {
                    user.cart = new Cart
                    {
                        UserId = userId,
                        cartItems = new List<CartItem>()
                    };
                    _dbcontext.cart.Add(user.cart);
                    await _dbcontext.SaveChangesAsync();
                }
                var itemin=user.cart.cartItems.FirstOrDefault(P=>P.ProductId== productid);
                if (itemin!=null)
                {
                    itemin.Quantity=itemin.Quantity+1;
                }
                else
                {
                    var newcartitem = new CartItem
                    {
                        CartId = user.cart.Id,
                        ProductId = productid,
                        Quantity = 1

                    };
                    _dbcontext.cartItems.Add(newcartitem);
                    
                }
                await _dbcontext.SaveChangesAsync();
            }
            
            
        }

        public async Task DeleteCart(string token, int productid)
        {
            int userId = _jwtServices.GetUserIdFromToken(token);
            if (userId == null)
            {
                throw new Exception("user id not valid");
            }
            var user = await _dbcontext.users.Include(c => c.cart).ThenInclude(p => p.cartItems).FirstOrDefaultAsync(i => i.Id == userId);
            var product = await _dbcontext.product.FirstOrDefaultAsync(p => p.Id == productid);
            if (user != null && product != null)
            {
                var item=user.cart.cartItems.FirstOrDefault(ci=>ci.ProductId== productid);
                if (item != null)
                {
                    _dbcontext.cartItems.Remove(item);
                    await _dbcontext.SaveChangesAsync();
                }
            }


        }

        public async Task Quanityplus(string token, int productid)
        {
            int userId = _jwtServices.GetUserIdFromToken(token);
            var user = await _dbcontext.users.Include(c => c.cart).ThenInclude(p => p.cartItems).FirstOrDefaultAsync(i => i.Id == userId);
            var product = await _dbcontext.product.FirstOrDefaultAsync(p => p.Id == productid);
            if (user != null && product != null)
            {
                var item = user.cart.cartItems.FirstOrDefault(ci => ci.ProductId == productid);
                if (item != null)
                {
                    item.Quantity = item.Quantity + 1;
                    await _dbcontext.SaveChangesAsync();
                }
            }

        }
        
        public async Task Quanityminus(string token, int productid)
        {
            int userId = _jwtServices.GetUserIdFromToken(token);
            var user = await _dbcontext.users.Include(c => c.cart).ThenInclude(p => p.cartItems).FirstOrDefaultAsync(i => i.Id == userId);
            var product = await _dbcontext.product.FirstOrDefaultAsync(p => p.Id == productid);
            if (user != null && product != null)
            {
                var item = user.cart.cartItems.FirstOrDefault(ci => ci.ProductId == productid);
                if (item != null)
                {
                    item.Quantity = item.Quantity >= 1 ? item.Quantity - 1 : item.Quantity;
                    if (item.Quantity == 0)
                    {
                        _dbcontext.cartItems.Remove(item);
                        await _dbcontext.SaveChangesAsync();
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
        }

        public async Task<List<CartViewDto>> GetallCartitem(string token)
        {
            try
            {
                int userId = _jwtServices.GetUserIdFromToken(token);

                if (userId == null)
                {
                    throw new Exception("user id not valid");
                }
                var user = await _dbcontext.cart.Include(c => c.cartItems).ThenInclude(p => p.product).FirstOrDefaultAsync(i => i.Id == userId);
                if(user != null)
                {
                    var cartitem = user.cartItems.Select(ci => new CartViewDto
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.product.ProductName,
                        Quantity = ci.Quantity,
                        Price=ci.product.Price,
                        TotalAmount=ci.product.Price*ci.Quantity,
                        ProductImage= HostUrl + ci.product.ProductImage,
                    }).ToList();
                    return cartitem;
                }
                return new List<CartViewDto>();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}
