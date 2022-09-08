using Assessment.Models.EntityModel;
using Assessment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Assessment.Infrastructure.DataProvider
{
    public interface IUserDataProvider
    {
        ServiceResponse RegisterUser(RegisterBindingModel model);
        ServiceResponse Authenticate(AuthenticateRequest model);
        UserTable getUser(long userId);
    }
    public class UserDataProvider : IUserDataProvider
    {
        public ServiceResponse RegisterUser(RegisterBindingModel model)
        {
            ServiceResponse response = new ServiceResponse();

            var _context = new Entities();
            if (_context.UserTables.Any(x => x.UserName.ToLower() == model.UserName.ToLower()))
            {
                response.IsSuccess = false;
                response.Message = "There is already account with this username";
                return response;
            }

            var saveModel = new UserTable();
            saveModel.UserName = model.UserName;
            saveModel.Password = Crypto.Encrypt(model.Password);

            saveModel.CreateDate = DateTime.UtcNow;
            saveModel.CreatedBy = 0;
            saveModel.ModifiedDate = DateTime.UtcNow;
            saveModel.ModifiedBy = 0;

            _context.UserTables.AddOrUpdate(saveModel);
            _context.SaveChanges();

            response.IsSuccess = true;
            response.Message = "The user has been successfully registered";

            return response;
        }

        public ServiceResponse Authenticate(AuthenticateRequest model)
        {
            ServiceResponse response = new ServiceResponse();
            var _context = new Entities();
            var user = _context.UserTables.SingleOrDefault(x => x.UserName == model.Username);

            if (user == null || Crypto.Decrypt(user.Password) != model.Password)
                return new ServiceResponse() { IsSuccess = false, Message = "Username or password is incorrect" };

            AuthenticateResponse authenticateResponse = new AuthenticateResponse() { UserId = user.UserId, UserName = user.UserName };

            IJwtUtils _jwtUtils = new JwtUtils ();
            authenticateResponse.Token = _jwtUtils.GenerateToken(user);
            response.Data = authenticateResponse;
            response.IsSuccess = true;
            return response;
        }

        public UserTable getUser(long userId)
        {
            var _context = new Entities();
            var user = _context.UserTables.Find(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}