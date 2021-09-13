using System.ComponentModel.DataAnnotations;
using System;

namespace AwsLambdaExample.Application
{
    public interface IValidatable { }

    public static class ValidationExtensions
    {
        /// <summary>
        /// Determines whether the specified object is valid using the validation context.
        /// </summary>
        /// <typeparam name="T">The type of object require interface IValidatable</typeparam>
        /// <param name="instance">The object to validate.</param>
        /// <returns>the object has been validated</returns>
        /// <exception cref="ValidationException">The object is not valid.</exception>
        /// <exception cref="ArgumentNullException">instance is null.</exception>
        /// <exception cref="ArgumentException">instance doesn't match the System.ComponentModel.DataAnnotations.ValidationContext.ObjectInstance on validationContext.</exception>
        public static T Validate<T>(this T? instance) where T : class, IValidatable
        {
            if (instance == null) 
            {
                throw new ValidationException("instance to validate is null");
            }
            Validator.ValidateObject(instance, new ValidationContext(instance), true);
            return instance;
        }
    }
}
