using AutoMapper;
using E_Commerce_Backend.Dbcontext;
using E_Commerce_Backend.Models.DTOs.userDTOS;
using E_Commerce_Backend.Models.ENTITYS;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services.UserService
{
    public class Userservice : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly ShoesDbcontext _dbcontext;
        private readonly IConfiguration _configuration;
        public Userservice(ShoesDbcontext shoesDbcontext, IMapper Imapper, IConfiguration configuration)
        {
            _mapper = Imapper;
            _dbcontext = shoesDbcontext;
            _configuration = configuration;
        }
        public async Task<bool> RegistrationUser(UserRegisterDTO userRegisterDTO)
        {
            var isexist=_dbcontext.users.FirstOrDefault(e=>e.Email == userRegisterDTO.Email);
            if (isexist != null)
            {
                return false;
            }
            var encryt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password,encryt);
            userRegisterDTO.Password = hashPassword;

            var user=_mapper.Map<Users>(userRegisterDTO);
            if (!_dbcontext.users.Any()) 
            {
                user.Role = "admin";
            }
            else
            {
                user.Role = "user"; 
            }
            _dbcontext.users.Add(user);
            await _dbcontext.SaveChangesAsync();
            return true;

        }
        public async Task<List<UserViewDTO>> GetUser()
        {
            var users=await _dbcontext.users.ToListAsync();
            var mapeduser=_mapper.Map<List<UserViewDTO>>(users);
            return mapeduser;

        }
        public async Task<UserViewDTO> GetUserById(int id)
        {
            var user=await _dbcontext.users.FirstOrDefaultAsync(e=>e.Id==id);
            if (user == null)
            {
                return null;
            }
            return new UserViewDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,

            };

        }
        public async Task<Users> Loging(UserLoginDTO userLoginDTO)
        {
            var checking=await _dbcontext.users.FirstOrDefaultAsync(ep=>ep.Email==userLoginDTO.Email);
            
            return checking;

        }
        public async Task<bool> BlockUser(int Userid)
        {
            var user= await _dbcontext.users.FirstOrDefaultAsync(u=>u.Id==Userid);
            if(user == null)
            {
                return false;
            }
            user.isBlocked = true;
            await _dbcontext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UnBlockUser(int Userid)
        {
            var user = await _dbcontext.users.FirstOrDefaultAsync(u => u.Id == Userid);
            if (user == null)
            {
                return false;
            }
            user.isBlocked = false;
            await _dbcontext.SaveChangesAsync();
            return true;
        }


    }
}
