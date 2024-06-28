using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using System.Net.WebSockets;
using WebChat.Models;

namespace WebChat.Identity
{
	public class UserStore : IUserStore<TUser>,IUserPasswordStore<TUser>
	{
		private readonly TableClient _tableClient;
		public UserStore(TableClient tableClient)
		{
			_tableClient = tableClient;
		}
		public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
		{
			var res = await _tableClient.AddEntityAsync<TUser>(user, cancellationToken);
			if (res.IsError)
			{
				return IdentityResult.Failed(new IdentityError() { Description = res.Content.ToString() });
			}
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
		{
			var res = await _tableClient.DeleteEntityAsync(user.PartitionKey, user.RowKey, cancellationToken: cancellationToken);
			if (res.IsError)
			{
				return IdentityResult.Failed(new IdentityError { Description = res.Content.ToString() });
			}
			return IdentityResult.Success;
		}

		public void Dispose()
		{

		}

		public async Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			var user = await _tableClient.GetEntityAsync<TUser>("user", userId, cancellationToken: cancellationToken);
			return user;
		}

		public async Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			var user = _tableClient.Query<TUser>(user => user.Nickname == normalizedUserName, cancellationToken: cancellationToken)
				.FirstOrDefault();
			return user;
		}

		public async Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.Nickname;
		}

		public async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.RowKey;
		}

		public async Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.Nickname;
		}

		public async Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken)
		{
			user.Nickname = normalizedName;
		}

		public async Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken)
		{
			user.Nickname = userName;
		}

		public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
		{
			var res = await _tableClient.UpdateEntityAsync(user, user.ETag, cancellationToken: cancellationToken);
			if (res.IsError)
			{
				return IdentityResult.Failed(new IdentityError() { Description = res.Content.ToString() });
			}
			return IdentityResult.Success;
		}

		public async Task SetPasswordHashAsync(TUser user, string? passwordHash, CancellationToken cancellationToken)
		{
			user.Password=passwordHash;
		}

		public  async Task<string?> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.Password;
		}

		public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.Password != null;
		}
	}
}
