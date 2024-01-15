using AwareTest.Model.Model;
using System.Collections.Generic;

namespace AwareTest.Services.IService
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequestModel model);
        IEnumerable<UserModel> GetAll();
        UserModel GetById(int id);
    }
}
