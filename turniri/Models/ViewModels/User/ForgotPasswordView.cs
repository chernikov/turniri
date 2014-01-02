using System.ComponentModel.DataAnnotations;
using turniri.Attributes.Validation;

namespace turniri.Models.ViewModels.User
{
    public class ForgotPasswordView
    {
        [Required(ErrorMessage = "Введите Email")]
        [ValidEmail(ErrorMessage = "Введенный Email не найден в нашей базе")]
        public string Email { get; set; }
    }
}