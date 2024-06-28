using Azure.Data.Tables.Sas;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace WebChat.Models.View
{
    public class RegisterForm
    {
        public string Nickname { get; set; }
        [DataType(DataType.Password)]
        [Compare("PasswordConfirm", ErrorMessage = "passwords do not match")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
