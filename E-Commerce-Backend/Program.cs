using E_Commerce_Backend.Dbcontext;
using E_Commerce_Backend.Jwt;
using E_Commerce_Backend.Mapper;
using E_Commerce_Backend.Services.CartServices;
using E_Commerce_Backend.Services.CategoryServices;
using E_Commerce_Backend.Services.OrderSevices;
using E_Commerce_Backend.Services.ProductServices;
using E_Commerce_Backend.Services.UserService;
using E_Commerce_Backend.Services.WhislistServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IUsersService,Userservice>();
builder.Services.AddScoped<ICategoryServices,CategoryServices>();
builder.Services.AddScoped<IProductServices,ProductServices>();
builder.Services.AddScoped<ICartService,CartService>();
builder.Services.AddScoped<IJwtServices,JwtServices>();
builder.Services.AddScoped<IWhishListServices,WhishListService>();
builder.Services.AddScoped<IOrderService,OrderService>();
builder.Services.AddScoped<ShoesDbcontext>();
builder.Services.AddAutoMapper(typeof(MainMapper));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
