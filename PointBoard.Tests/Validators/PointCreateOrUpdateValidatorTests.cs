using PointBoard.Host.Models.Point;
using PointBoard.Host.Validation;

namespace PointBoard.Tests.Validators;

public class PointCreateOrUpdateValidatorTests
{
    private readonly PointCreateOrUpdateValidator _validator = new();

    [Fact]
    public void Should_Pass_When_ValidModel()
    {
        var model = new PointCreateOrUpdate
        {
            X = 100,
            Y = 200,
            Radius = 15,
            Color = "#abcdef"
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Should_Fail_When_RadiusIsNotPositive(int radius)
    {
        var model = new PointCreateOrUpdate
        {
            X = 10,
            Y = 20,
            Radius = radius,
            Color = "#000000"
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Radius");
    }

    [Theory]
    [InlineData("nope")]
    [InlineData("#12345")]
    [InlineData("123456")]
    public void Should_Fail_When_ColorIsInvalid(string color)
    {
        var model = new PointCreateOrUpdate
        {
            X = 10,
            Y = 20,
            Radius = 5,
            Color = color
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Color");
    }
}