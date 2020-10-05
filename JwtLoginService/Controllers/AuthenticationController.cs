using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JwtLoginService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthenticationController : ControllerBase
	{
		[HttpPost("login")]
		public async Task<IActionResult> Login(User user)
		{
			User u = new UserRepository().GetUser(user.Username);

			if (u == null) return NotFound("User not found.");

			bool credentials = u.Password.Equals(user.Password);

			if (!credentials) return BadRequest("Could not authenticate user.");

			var token = TokenManager.GenerateToken(u.Username);

			return Ok(token);
		}

		[HttpGet("verify")]
		public async Task<IActionResult> Validate(string token, string username)
		{
			bool exists = new UserRepository().GetUser(username) != null;

			if (!exists) return NotFound("The user was not found.");

			string tokenUsername = TokenManager.ValidateToken(token);

			if (username.Equals(tokenUsername)) return Ok();

			return BadRequest("User/token mismatch.");
		}
	}
}
