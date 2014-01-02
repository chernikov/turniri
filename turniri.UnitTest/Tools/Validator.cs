using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.UnitTest.Tools
{
    public class ValidatorException : Exception
    {
        public ValidationAttribute Attribute { get; private set; }

        public ValidatorException(ValidationException ex, ValidationAttribute attribute)
            : base(attribute.GetType().Name, ex)
        {
            Attribute = attribute;
        }
    }

    public class Validator
    {
        public static void ValidateObject<T>(T obj)
        {
            var type = typeof(T);
            var meta = type.GetCustomAttributes(false).OfType<MetadataTypeAttribute>().FirstOrDefault();
            if (meta != null)
            {
                type = meta.MetadataClassType;
            }

            var typeAttributes = type.GetCustomAttributes(typeof(ValidationAttribute), true).OfType<ValidationAttribute>();
            var validationContext = new ValidationContext(obj);
            foreach (var attribute in typeAttributes)
            {
                attribute.Validate(obj, validationContext);
            }

            var propertyInfo = type.GetProperties();
            foreach (var info in propertyInfo)
            {
                var attributes = info.GetCustomAttributes(typeof(ValidationAttribute), true).OfType<ValidationAttribute>();
                foreach (var attribute in attributes)
                {
                    var objPropInfo = obj.GetType().GetProperty(info.Name);
                    try
                    {
                        attribute.Validate(objPropInfo.GetValue(obj, null), validationContext);
                    }
                    catch (ValidationException ex)
                    {
                        throw new ValidatorException(ex, attribute);
                    }
                }
            }
        }
    }
}
