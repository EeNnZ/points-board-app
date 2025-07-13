using PointBoard.Host.Models.Comment;
using PointBoard.Host.Validation;

namespace PointBoard.Tests.Validators;

public class CommentCreateOrUpdateValidatorTests
{
    private readonly CommentCreateOrUpdateValidator _validator = new();

    [Fact]
    public void Should_Pass_When_ValidModel()
    {
        var model = new CommentCreateOrUpdate
        {
            Text = "Valid comment",
            BackgroundColor = "#aabbcc",
            PointId = Guid.NewGuid()
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Fail_When_TextIsEmpty(string text)
    {
        var model = new CommentCreateOrUpdate
        {
            Text = text,
            BackgroundColor = "#fff",
            PointId = Guid.NewGuid()
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Text");
    }

    [Theory]
    [InlineData("123456")]
    [InlineData("notacolor")]
    [InlineData("#12")]
    [InlineData("#xyzxyz")]
    public void Should_Fail_When_ColorIsInvalid(string color)
    {
        var model = new CommentCreateOrUpdate
        {
            Text = "Hello",
            BackgroundColor = color,
            PointId = Guid.NewGuid()
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "BackgroundColor");
    }

    [Fact]
    public void Should_Fail_When_PointId_IsEmpty()
    {
        var model = new CommentCreateOrUpdate
        {
            Text = "Hi",
            BackgroundColor = "#fff",
            PointId = Guid.Empty
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PointId");
    }
}
