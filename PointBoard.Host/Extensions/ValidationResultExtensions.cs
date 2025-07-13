using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PointBoard.Host.Extensions;

/// <summary>
/// Provides extension methods for FluentValidation results.
/// </summary>
public static class ValidationResultExtensions
{
    /// <summary>
    /// Adds validation errors from a <see cref="ValidationResult"/> to the specified <see cref="ModelStateDictionary"/>.
    /// </summary>
    /// <param name="result">The validation result containing errors.</param>
    /// <param name="modelState">The model state dictionary to add errors to.</param>
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        foreach (ValidationFailure? error in result.Errors)
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
    }
}