namespace E_Commerce_Backend.Jwt
{
    public interface IJwtServices
    {
        int GetUserIdFromToken(string token);
    }
}
