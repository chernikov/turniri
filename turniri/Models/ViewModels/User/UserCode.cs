
using System.ComponentModel.DataAnnotations;

namespace turniri.Models.ViewModels.User
{
    public class UserCode
    {
        [Required(ErrorMessage = "Введите код")]
        public string Code { get; set; }
    }
}