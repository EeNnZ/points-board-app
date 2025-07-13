using System.Drawing;
using FluentValidation;
using PointBoard.Host.Models.Comment;

namespace PointBoard.Host.Validation;

/// <summary>
/// Validator for <see cref="CommentCreateOrUpdate"/> models.
/// </summary>
public class CommentCreateOrUpdateValidator : AbstractValidator<CommentCreateOrUpdate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommentCreateOrUpdateValidator"/> class.
    /// </summary>
    public CommentCreateOrUpdateValidator()
    {
        RuleFor(comment => comment.PointId).NotEmpty();

        RuleFor(comment => comment.BackgroundColor)
           .Length(7)
           .NotEmpty()
           .Must(StartsWithHash)
           .Must(BeAValidHtmlColor);

        RuleFor(comment => comment.Text).NotEmpty();
    }

    private bool BeAValidHtmlColor(string arg)
    {
        try
        {
            Color _ = System.Drawing.ColorTranslator.FromHtml(arg);
            return true;
        }
        catch
        {
            return false;
        }
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