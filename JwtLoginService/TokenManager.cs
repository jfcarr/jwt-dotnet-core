using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtLoginService
{
	public class TokenManager
	{
		private static string Secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

		/// <summary>
		/// Generate a new token.
		/// </summary>
		/// <param name="username">Associate the token with this username.</param>
		/// <returns></returns>
		public static string GenerateToken(string username)
		{
			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
			var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
			var claims = new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, username),
			};
			var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: DateTime.Now.AddMinutes(30));
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			return encodedJwt;
		}

		/// <summary>
		/// Confirm that a given token is valid.
		/// </summary>
		/// <param name="token">Token value to verify.</param>
		/// <returns></returns>
		public static string ValidateToken(string token)
		{
			string username = null;
			ClaimsPrincipal principal = GetPrincipal(token);

			if (principal == null)
				return null;

			ClaimsIdentity identity = null;
			try
			{
				identity = (ClaimsIdentity)principal.Identity;
			}
			catch (NullReferenceException)
			{
				{
					identity = (ClaimsIdentity)principal.Identity;
				}
				return null;
			}

			Claim usernameClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
			username = usernameClaim.Value;

			return username;
		}

		/// <summary>
		/// Retrieve claims detail for a token.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ClaimsPrincipal GetPrincipal(string token)
		{
			try
			{
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

				if (jwtToken == null)
					return null;

				byte[] key = Convert.FromBase64String(Secret);

				TokenValidationParameters parameters = new TokenValidationParameters()
				{
					RequireExpirationTime = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret))
				};

				SecurityToken securityToken;
				ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
				return principal;
			}
			catch (Exception e)
			{
				return null;
			}
		}
	}
}