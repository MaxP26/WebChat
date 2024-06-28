using Azure;
using Azure.Data.Tables;
using WebChat.Models.View;

namespace WebChat.Models
{
	public class TMessage: TableEntity,IMessageViewModel
	{
		public string ChatId {  get; set; }
		public string UserId {  get; set; }
		public string Text { get; set; }
		protected override string PartKey => "message";

        public string UserName { get; set; }
        public string Time { get => this.Timestamp.Value.LocalDateTime.ToString("HH:mm"); }

        public TMessage()
			: base()
		{

		}

		public TMessage(string chatId, string userId, string text, string partitionKey, string rowKey, DateTime timestamp, ETag eTag) 
			: base(partitionKey, rowKey, timestamp, eTag)
        {
			ChatId = chatId;
			UserId = userId;
			Text = text;
		}
	}
}
