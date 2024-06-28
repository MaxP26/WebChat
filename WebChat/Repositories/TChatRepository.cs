using Azure;
using Azure.Data.Tables;
using NuGet.Common;
using System.Collections.Immutable;
using System.Threading.Tasks.Dataflow;
using WebChat.Models;

namespace WebChat.Repositories
{
	public class TChatRepository
	{
		private readonly TableClient _client;
		public TChatRepository(TableClient client)
		{
			_client = client;
		}
		public async Task CreateChatAsync(TChat chat)
		{
			await _client.AddEntityAsync<TChat>(chat);
		}
		public async Task DeleteChatAsync(TChat chat)
		{
			await _client.DeleteEntityAsync(chat.PartitionKey, chat.RowKey);
		}
		public async Task UpdateChatAsync(TChat chat)
		{
			await _client.UpdateEntityAsync<TChat>(chat, chat.ETag);
		}
		public async Task<TChat?> GetChatById(string chatId)
		{
			var chat = (await _client.GetEntityAsync<TChat>("chat", chatId)).Value;
			return chat;
		}
		public IEnumerable<TUser> GetChatUsers(string chatId)
		{
			var users = _client.Query<TChatUser>(chu => chu.ChatId == chatId)
				.Select(chu => _client.GetEntity<TUser>("user", chu.UserId).Value);
			return users;
		}
		public async IAsyncEnumerable<TMessage> GetChatMessages(string chatId)
		{
			var messages = _client.QueryAsync<TMessage>(m => m.PartitionKey == "message" && m.ChatId == chatId, 20).AsPages();
			var en = messages.GetAsyncEnumerator();
			ValueTask<bool> nextPage;
			Page<TMessage> page;
			nextPage = en.MoveNextAsync();
			while (await nextPage)
			{
				page = en.Current;
				nextPage = en.MoveNextAsync();
				foreach (var message in page.Values.ToImmutableSortedSet(Comparer<TMessage>.Create((m1,m2)=>m1.Time.CompareTo(m2.Time))))
				{
					message.UserName =(await _client.GetEntityAsync<TUser>("user", message.UserId)).Value.Nickname;
					yield return message;
				}
			}
		}
		public IEnumerable<TChat> LoadUserChats(string userId)
		{
			var chats = _client.Query<TChatUser>(chu =>chu.PartitionKey=="chatuser"&& chu.UserId == userId)
				.Select(
				chu => 
				_client.GetEntity<TChat>("chat", chu.ChatId).Value);
			return chats;
		}
        public IEnumerable<TUser> LoadUsers(string chatId)
		{
			var chats = _client.Query<TChatUser>(chu =>chu.PartitionKey=="chatuser"&& chu.UserId == chatId)
				.Select(chu => _client.GetEntity<TUser>("user", chu.RowKey).Value);
			return chats;
		}
		public void JoinChat(string userId,string chatId)
		{
			_client.AddEntity<TChatUser>(new TChatUser {UserId = userId,ChatId = chatId});
		}
	}
}
    