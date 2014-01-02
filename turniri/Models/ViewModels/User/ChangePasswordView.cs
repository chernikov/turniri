using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using turniri.Attributes.Validation;

namespace turniri.Models.ViewModels.User
{
    public class ChangePasswordView
    {
        public int ID { get; set; }

        [IsUserPassword(ErrorMessage = "Неверный пароль")]
        [Required(ErrorMessage="Введите пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage="Введите новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage="Подтвердите пароль")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}