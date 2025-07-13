using PointBoard.Core.Abstractions;
using PointBoard.Core.Domain.Entities;
using PointBoard.Host.Controllers;
using PointBoard.Host.Models.Point;

namespace PointBoard.Tests.Controllers;

public class PointsControllerTests
{
    private readonly Mock<IRepo<Point>> _repo = new();
    private readonly Mock<IValidator<PointCreateOrUpdate>> _validator = new();
    private readonly PointsController _controller;

    public PointsControllerTests()
    {
        _controller = new PointsController(_repo.Object, _validator.Object);
    }

    [Fact]
    public async Task GetPointsAsync_ReturnsNotFound_WhenEmpty()
    {
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Point>());

        var result = await _controller.GetPointsAsync();

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetPointByIdAsync_ReturnsOk_WhenFound()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Point { Id = id, X = 1, Y = 1, Radius = 1, Color = "blue" });

        var result = await _controller.GetPointByIdAsync(id);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PostPointAsync_ReturnsBadRequest_WhenInvalid()
    {
        _validator.Setup(v => v.ValidateAsync(It.IsAny<PointCreateOrUpdate>(), default))
                  .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("X", "Required") }));

        var result = await _controller.PostPointAsync(new PointCreateOrUpdate());

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task UpdatePointAsync_ReturnsOk_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Point { Id = id, X = 1, Y = 1, Radius = 1, Color = "red" });
        
        _validator.Setup(v => v.ValidateAsync(It.IsAny<PointCreateOrUpdate>(), default))
                  .ReturnsAsync(new ValidationResult());
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Point>())).ReturnsAsync((Point p) => p);

        var model = new PointCreateOrUpdate { X = 10, Y = 20, Radius = 5, Color = "green" };

        var result = await _controller.UpdatePointAsync(id, model);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task UpdatePointAsync_ReturnsNotFound_WhenMissing()
    {
        var id = Guid.NewGuid();
        
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Point?)null);
        
        _validator.Setup(v => 
                             v.ValidateAsync(It.IsAny<PointCreateOrUpdate>(), default))
                  .ReturnsAsync(new ValidationResult());

        var result = await _controller.UpdatePointAsync(Guid.NewGuid(), new PointCreateOrUpdate
        {
            X = 0,
            Y = 0,
            Radius = 10,
            Color = "#FFFFFF"
        });

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeletePointAsync_ReturnsNotFound_WhenMissing()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Point?)null);

        var result = await _controller.DeletePointAsync(Guid.NewGuid());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeletePointAsync_ReturnsProblem_WhenException()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Point());
        _repo.Setup(r => r.DeleteAsync(It.IsAny<Point>())).ThrowsAsync(new Exception("fail"));

        var result = await _controller.DeletePointAsync(Guid.NewGuid());

        var problem = result as ObjectResult;
        problem.Should().NotBeNull();
        problem!.StatusCode.Should().Be(500);
    }
}
