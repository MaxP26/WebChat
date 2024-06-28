using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using WebChat.Models;

namespace WebChat.Identity
{
	public class RoleStore : IRoleStore<TRole>
	{
		private readonly TableClient _tableClient;
		public RoleStore(TableClient tableClient)
		{
			_tableClient = tableClient;
		}
		public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
		{
			var res = await _tableClient.AddEntityAsync<TRole>(role, cancellationToken);
			if (res.IsError)
			{
				return IdentityResult.Failed(new IdentityError() { Description = res.Content.ToString() });
			}
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
		{
			var res = await _tableClient.DeleteEntityAsync(role.PartitionKey,role.RowKey,cancellationToken: cancellationToken);
			if (res.IsError)
			{
				return IdentityResult.Failed(new IdentityError() { Description = res.Content.ToString() });
			}
			return IdentityResult.Success;
		}

		public void Dispose(){}

		public async Task<TRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			var role = await _tableClient.GetEntityAsync<TRole>("role", roleId, cancellationToken: cancellationToken);
			return role;
		}

		public async Task<TRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
		{
			var role = _tableClient.Query<TRole>(role => role.Name.ToUpper() == normalizedRoleName, cancellationToken: cancellationToken)
				.FirstOrDefault();
			return role;
		}

		public async Task<string?> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			return role.Name.ToUpper();
		}

		public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
		{
			return role.RowKey;
		}

		public async Task<string?> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			return role.Name;
		}

		public async Task SetNormalizedRoleNameAsync(TRole role, string? normalizedName, CancellationToken cancellationToken)
		{
			role.Name = normalizedName.ToLower();
		}

		public async Task SetRoleNameAsync(TRole role, string? roleName, CancellationToken cancellationToken)
		{
			role.Name = roleName;
		}

		public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
		{
			var res = await _tableClient.UpdateEntityAsync<TRole>(role,role.ETag,cancellationToken: cancellationToken);
			if (res.IsError)
			{
				return IdentityResult.Failed(new IdentityError() { Description = res.Content.ToString() });
			}
			return IdentityResult.Success;
		}
	}
}
