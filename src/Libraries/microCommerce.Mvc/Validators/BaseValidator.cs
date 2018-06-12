﻿using FluentValidation;
using microCommerce.Mvc.Models;

namespace microCommerce.Mvc.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T> where T : BaseModel
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected BaseValidator()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }
    }
}