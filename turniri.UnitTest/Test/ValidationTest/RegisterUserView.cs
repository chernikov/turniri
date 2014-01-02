using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using turniri.Model;
using turniri.Models.ViewModels.User;
using turniri.Tools;
using turniri.UnitTest.Mock;
using turniri.UnitTest.Mock.Http;
using turniri.UnitTest.Tools;

namespace turniri.UnitTest
{
    [TestFixture]
    public class RegisterUserViewValidation
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_EmailIsEmpty_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = string.Empty,
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111"
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "ValidEmailAttribute")]
        public void Validate_EmailIsNotCorrect_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "chernikov",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111"
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "UserEmailValidationAttribute")]
        public void Validate_EmailIsAlreadyExists_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "chernikov@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111"
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_PasswordIsEmpty_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                Password = "",
                ConfirmPassword = "123456",
                Captcha = "1111"
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_ConfirmPasswordIsEmpty_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "",
                Captcha = "1111"
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_ConfirmEmailDoesNotMatch_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                ConfirmEmail = "",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111"
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_CountryIsEmpty_FailValidation()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                ConfirmEmail = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111"
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
            Assert.IsNotNull(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_CityIsEmpty_FailValidate()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                ConfirmEmail = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111",
                Country = "Россия",
                City = "",
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
            Assert.IsNotNull(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_PhoneIsEmpty_FailValidate()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                ConfirmEmail = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111",
                Country = "Россия",
                City = "Москва",
                Phone = ""
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
            Assert.IsNotNull(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "ValidPhoneAttribute")]
        public void Validate_PhoneIsIncorrect_FailValidate()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                ConfirmEmail = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111",
                Country = "Россия",
                City = "Москва",
                Phone = "fjkdsj"
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
            Assert.IsNotNull(registerUserView);
        }


        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_LoginIsEmpty_FailValidate()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Login = "",
                Email = "rollinx@gmail.com",
                ConfirmEmail = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111",
                Country = "Россия",
                City = "Москва",
                Phone = "+7(495) 123 45 67"
            };

            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "UserLoginAttribute")]
        public void Validate_LoginIsAlreadyExist_FailValidate()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Login = "admin",
                Email = "rollinx@gmail.com",
                ConfirmEmail = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111",
                Country = "Россия",
                City = "Москва",
                Phone = "+7(495) 123 45 67"
            };

            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ValidatorException), UserMessage = "ValidLatinAttribute")]
        public void Validate_LoginIsNotLatin_FailValidate()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Login = "Сержант Пеппер",
                Email = "rollinx@gmail.com",
                ConfirmEmail = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111",
                Country = "Россия",
                City = "Москва",
                Phone = "+7(495) 123 45 67",
            };

            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        public void Validate_AllDataIsOk_ValidatePassed()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Login = "rollinx",
                Email = "rollinx@gmail.com",
                ConfirmEmail = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111",
                Country = "Россия",
                City = "Москва",
                Phone = "+7(495) 123 45 67"
            };

            Validator.ValidateObject<RegisterUserView>(registerUserView);
            Assert.IsNotNull(registerUserView);
        }

    }
}
