using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace turniri.Attributes
{

    public sealed class IsDateFutureAttribute : ValidationAttribute, IClientValidatable
    {

        private readonly bool allowToday;

        public IsDateFutureAttribute(bool allowToday = false)
        {
            this.allowToday = allowToday;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           

            // Compare values
            if (((DateTime)value).Date >= (DateTime)DateTime.Now.Date)
            {
                if (this.allowToday)
                {
                    return ValidationResult.Success;
                }
                if (((DateTime)value).Date > (DateTime)DateTime.Now.Date)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
                                                                               ControllerContext context)
        {
            var rule = new ModelClientValidationRule
                           {
                               ErrorMessage = this.ErrorMessageString,
                               ValidationType = "isdatefuture"
                           };
            rule.ValidationParameters["allowtoday"] = this.allowToday;
            yield return rule;
        }
    }
}