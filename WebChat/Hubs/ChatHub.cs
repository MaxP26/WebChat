using Microsoft.AspNetCore.SignalR;
using WebChat.Repositories;
using WebMessage.Repositories;

namespace WebChat.Hubs
{
    public class ChatHub: Hub
    {
        private readonly TChatRepository _chatRepository;
        private readonly TMessagesRepository _messagesRepository;
        public ChatHub(TChatRepository chatRepository,TMessagesRepository messagesRepository) : base()
        {
            _chatRepository = chatRepository;
            _messagesRepository = messagesRepository;
        }
        public async Task SendMessage(string chatId, string message)
        {
            var userId=Context.UserIdentifier;
            var chat = await _chatRepository.GetChatById(chatId);
            var users = _chatRepository.GetChatUsers(chatId).Select(u=>u.RowKey);
            if (chat == null ||users.All(user=>user!=userId))
            {
                return;
            }
            var mes = new Models.TMessage()
            {
                UserId = userId,
                ChatId = chatId,
                Text = message,
            };
            await _messagesRepository.CreateMessageAsync(mes);
            await Clients.Users(users).SendAsync("ReceiveMessage",chatId,userId,message,mes.Time); 
        }

    }
}
