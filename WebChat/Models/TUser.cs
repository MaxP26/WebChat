using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebChat.Models
{
	public class TUser : TableEntity
	{
		public string Nickname { get; set; }
		[DataType(DataType.Password)]
		public string? Password { get; set; }
		protected override string PartKey => "user";

		public TUser()
			: base()
		{
		}
		public TUser(string nickname, string iconPath, string password, string partitionKey, string rowKey, DateTime timestamp, ETag eTag) 
			: base(partitionKey, rowKey, timestamp, eTag)
        {
			Nickname = nickname;
			Password = password;
		}
	}
}
