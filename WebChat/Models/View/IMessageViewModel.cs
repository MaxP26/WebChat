namespace WebChat.Models.View
{
    public interface IMessageViewModel
    {
        public string Text { get; }
        public string UserName { get; }
        public string UserId { get; }
        public string Time { get; }
    }
}
