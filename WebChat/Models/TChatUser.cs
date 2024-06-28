using Azure;
using Azure.Data.Tables;

namespace WebChat.Models
{
	public class TChatUser : TableEntity
	{
		protected override string PartKey => "chatuser";

		public string UserId { get; set; }
		public string ChatId { get; set; }
		public TChatUser() : base()
		{
		}
		public TChatUser(string userId, string chatId, string partitionKey, string rowKey, DateTime timestamp, ETag eTag) 
			: base(partitionKey, rowKey, timestamp, eTag) 
		{
			UserId = userId;
			ChatId = chatId;
		}
	}
}
