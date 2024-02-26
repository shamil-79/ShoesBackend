using E_Commerce_Backend.Models.DTOs.userDTOS;
using E_Commerce_Backend.Models.ENTITYS;

namespace E_Commerce_Backend.Services.UserService
{
    public interface IUsersService
    {
        public Task<bool> RegistrationUser(UserRegisterDTO userRegisterDTO);
        public Task<List<UserViewDTO>> GetUser();
        public Task<UserViewDTO> GetUserById(int id);
        public Task<Users> Loging(UserLoginDTO userLoginDTO);
        public Task<bool> BlockUser(int Userid);
        public Task<bool> UnBlockUser(int Userid);
    }
}
