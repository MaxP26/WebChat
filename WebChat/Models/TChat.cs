using Azure;
using Azure.Data.Tables;

namespace WebChat.Models
{
	public class TChat : TableEntity
	{
		public string OwnerId {  get; set; }
		public string Title {  get; set; }
		public IEnumerable<TUser> Users { get; set; }
		protected override string PartKey => "chat";
		public IEnumerable<TMessage> Messages { get; set; }
		public TChat()
			:base()
		{
		}

        public TChat(string ownerId, string title,string partitionKey,string rowKey,DateTime timestamp,ETag eTag):base(partitionKey,rowKey,timestamp,eTag)
        {
            OwnerId = ownerId;
            Title = title;
        }
    }
}
