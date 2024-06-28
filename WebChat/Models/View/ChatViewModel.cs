namespace WebChat.Models.View
{
    public class ChatViewModel
    {
        public string  ChatId {  get; set; }
        public string Title {  get; set; }
        public string OwnerId {  get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
