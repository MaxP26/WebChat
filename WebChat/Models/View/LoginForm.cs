using System.ComponentModel.DataAnnotations;

namespace WebChat.Models.View
{
    public class LoginForm
    {
        public string Nickname { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
