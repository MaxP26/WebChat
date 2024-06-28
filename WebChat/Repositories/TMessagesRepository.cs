using Azure.Data.Tables;
using WebChat.Models;

namespace WebMessage.Repositories
{
	public class TMessagesRepository
	{
		private readonly TableClient _client;
		public TMessagesRepository(TableClient client)
		{
			_client = client;
		}
		public async Task CreateMessageAsync(TMessage Message)
		{
			await _client.AddEntityAsync<TMessage>(Message);
		}
		public async Task DeleteMessageAsync(TMessage Message)
		{
			await _client.DeleteEntityAsync(Message.PartitionKey, Message.RowKey);
		}
		public async Task UpdateMessageAsync(TMessage Message)
		{
			await _client.UpdateEntityAsync<TMessage>(Message, Message.ETag);
		}
		public async Task<TMessage?> GetMessageById(string MessageId)
		{
			var Message = (await _client.GetEntityAsync<TMessage>("message", MessageId)).Value;
			return Message;
		}
	}
}
