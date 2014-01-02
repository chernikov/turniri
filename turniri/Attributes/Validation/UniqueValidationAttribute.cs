using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using turniri.Model;
using Ninject;

namespace turniri.Attributes.Validation
{
    public abstract class UniqueValidationAttribute : ValidationAttribute
    {
        protected IRepository repository { get; private set; }

        private bool _valid;

        protected UniqueValidationAttribute()
            : base()
        {
            Init();
        }

        protected UniqueValidationAttribute(string errorMessage)
            : base(errorMessage)
        {
            Init();
        }
        protected UniqueValidationAttribute(System.Func<string> errorMessageAccessor)
            : base(errorMessageAccessor)
        {
            Init();
        }

        protected virtual void Init()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            repository = kernel.Get<IRepository>("TransientScoped");
        }

        public override bool IsValid(object value)
        {
            return _valid;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var obj = validationContext.ObjectInstance;

            if (obj == null)
            {
                _valid = true;
                return base.IsValid(value, validationContext);
            }

            _valid = CheckProperty(obj);

            return base.IsValid(value, validationContext);
        }

        protected abstract bool CheckProperty(object value);
    }
}