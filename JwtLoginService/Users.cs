using System.Collections.Generic;
using System.Linq;

namespace JwtLoginService
{
	/// <summary>
	/// User model.
	/// </summary>
	public class User
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	/// <summary>
	/// Manages a list of synthetic users, emulating a database.
	/// </summary>
	public class UserRepository
	{
		public List<User> TestUsers;

		public UserRepository()
		{
			TestUsers = new List<User>() {
				new User() { Username = "User1", Password = "Password1" },
				new User() { Username = "User2", Password = "Password2" }
			};
		}

		/// <summary>
		/// Get user details.
		/// </summary>
		/// <param name="username">User to search for.</param>
		/// <returns></returns>
		public User GetUser(string username)
		{
			try
			{
				return TestUsers.First(user => user.Username.Equals(username));
			}
			catch
			{
				return null;
			}
		}
	}
}