using System.ComponentModel.DataAnnotations;
using turniri.Attributes.Validation;

namespace turniri.Models.ViewModels.User
{
    public class BaseUserView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [ValidEmail(ErrorMessage = "Введите корректный Email")]
        [UserEmailValidation(ErrorMessage = "Такой Email уже зарегистрирован")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [ValidLatinValidation(ErrorMessage="Логин должен состоять из латинских букв, цифр и знака \"_\"")]
        [UserLoginValidation(ErrorMessage = "Пользователь с таким логином уже зарегистрирован")]
        public string Login { get; set; }

    }
}