using FluentValidation;
using PointBoard.Host.Models.Point;

namespace PointBoard.Host.Validation;

/// <summary>
/// Validator for <see cref="PointCreateOrUpdate"/> models.
/// </summary>
public class PointCreateOrUpdateValidator : AbstractValidator<PointCreateOrUpdate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PointCreateOrUpdateValidator"/> class.
    /// </summary>
    public PointCreateOrUpdateValidator()
    {
        RuleFor(point => point.Color)
           .Length(7)
           .NotEmpty()
           .Must(StartsWithHash);

        RuleFor(point => point.Radius).GreaterThan(0);
    }

    /// <summary>
    /// Checks if the string starts with a hash (#) character.
    /// </summary>
    /// <param name="arg">The string to check.</param>
    /// <returns>True if the string starts with '#', otherwise false.</returns>
    private bool StartsWithHash(string arg)
    {
        return arg.StartsWith("#");
    }
}