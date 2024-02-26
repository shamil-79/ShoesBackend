using AutoMapper;
using E_Commerce_Backend.Models.DTOs.CategoryDTOs;
using E_Commerce_Backend.Models.DTOs.ProductDTOs;
using E_Commerce_Backend.Models.DTOs.userDTOS;
using E_Commerce_Backend.Models.DTOs.WhishListDTOs;
using E_Commerce_Backend.Models.ENTITYS;

namespace E_Commerce_Backend.Mapper
{
    public class MainMapper:Profile
    {
        public MainMapper()
        {
            CreateMap<Users,UserRegisterDTO>().ReverseMap();
            CreateMap<Users,UserViewDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category,CategoryViewDTO>().ReverseMap();
            CreateMap<Products,ProductDTO>().ReverseMap();
            CreateMap<WhishList, WhishListDTO>().ReverseMap();

        }
    }
}
