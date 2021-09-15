using Microsoft.EntityFrameworkCore;
using EMa.Data.DataContext;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace EMa.Common.Helpers
{
    public class Authorization
    {
        /// <summary>
        /// Get UserId, ChildName, PhoneNumber from token
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public static async Task<(string UserId, string PhoneNumber, string ChildName, long Expires)> GetInfoFromToken(string tokenString)
        {
            (string UserId, string PhoneNumber, string ChildName, long Expires) result = (string.Empty, string.Empty, string.Empty, 0);

            if (!string.IsNullOrEmpty(tokenString))
            {
                var jwtEncodedString = tokenString.Replace("Bearer ", "");
                var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
                result.UserId = token.Claims.First(c => c.Type == "id").Value;
                result.PhoneNumber = token.Claims.First(c => c.Type == "phone").Value;
                result.ChildName = token.Claims.First(c => c.Type == "childName").Value;
                result.Expires = long.Parse(token.Claims.First(c => c.Type == "exp").Value);
            }

            return await Task.FromResult(result);
        }
    }
}