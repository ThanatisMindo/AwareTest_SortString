using AwareTest.Model.Model;

namespace AwareTest.Services.IService
{
    public interface IJwtUtilsService
    {
        string GenerateJwtToken(UserModel user);
        int? ValidateJwtToken(string token);
    }
}
