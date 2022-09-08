using Assessment.Infrastructure.DataProvider;
using Assessment.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assessment.Infrastructure.Helpers
{
    public class UserSessionHelper
    {
        public UserTable CurrentUser { get; set; }
    }
    public class SessionHelper
    {
        public static string Token
        {
            get
            {
                var request = HttpContext.Current.Request;
                var authorization = request.Headers["Authorization"]?.Split(' ').Last();

                return authorization;
            }
        }

        public static long UserId
        {
            get
            {
                return UserDetail?.CurrentUser?.UserId > 0 ? UserDetail.CurrentUser.UserId : 0;
            }
        }

        public static UserSessionHelper UserDetail
        {
            get
            {
                IJwtUtils _jwtUtils = new JwtUtils();
                UserSessionHelper userData;
                if (_jwtUtils.ValidateToken(Token, out userData))
                {
                    return userData;
                }
                return null;
            }
        }
    }
}