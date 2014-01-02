using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace turniri.Attributes
{
    public class MinValueIfAttribute : ValidationAttribute
    {
        private string DependentProperty { get; set; }
        private object TargetValue { get; set; }
        private int MinValue { get; set; }

        public MinValueIfAttribute(string dependentProperty, object targetValue, int minValue)
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
            MinValue = minValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = validationContext.ObjectInstance.GetType().GetProperty(DependentProperty);
            if (field != null)
            {
                var dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                if ((dependentValue == null && TargetValue == null) ||
                    (dependentValue != null && dependentValue.Equals(TargetValue)))
                {
                    var valueInt = Convert.ToInt32(value);
                    if (valueInt < MinValue)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}