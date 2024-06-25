using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace web1.Helpers
{
    public class JwtHelper
    {

        /// <summary>
        /// 创建一个JWT令牌
        /// </summary>
        /// <param name="policeNo">警号</param>
        /// <param name="role">用户等级，参考Entities.Student注释</param>
        /// <param name="expiresIn">有效期（单位：分钟）</param>
        /// <returns>一个签发的JWT令牌</returns>
        public static string CreateToken(string policeNo, string role, int expiresIn)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, policeNo),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123cdefefefeasd14a5445411sds65d4asw65f4e"));

            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "PoliceApp",
                audience: "PoliceAppUser",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(expiresIn),
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
}
