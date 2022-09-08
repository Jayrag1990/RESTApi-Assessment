using Assessment.Infrastructure.Helpers;
using Assessment.Models.EntityModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace Assessment.Infrastructure.DataProvider
{
    public interface IJwtUtils
    {
        string GenerateToken(UserTable user);
        bool ValidateToken(string token, out UserSessionHelper userData);
    }
    public class JwtUtils : IJwtUtils
    {
        public string GenerateToken(UserTable user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, SerializeObject(user))}),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token, out UserSessionHelper userData)
        {
            userData = null;
            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigSettings.SecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var usernameClaim = jwtToken.Claims.First(x => x.Type == ClaimTypes.UserData);
                string userDataValue = usernameClaim?.Value;

                if (string.IsNullOrEmpty(userDataValue))
                    return false;

                try
                {
                    userData = new UserSessionHelper() {CurrentUser = DeserializeObject<UserTable>(userDataValue) };
                }
                catch (Exception ex)
                {
                }

                if (userData == null || userData.CurrentUser == null)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static T DeserializeObject<T>(string json)
        {
            T obj = default(T);
            obj = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return obj;
        }

        public static string SerializeObject<T>(T objectData)
        {
            string defaultJson = JsonConvert.SerializeObject(objectData);
            return defaultJson;
        }
    }
}