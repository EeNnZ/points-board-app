using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PointBoard.Host.Controllers;
using PointBoard.Core.Abstractions;
using PointBoard.Core.Domain.Entities;
using PointBoard.Host.Models.Comment;
using FluentValidation;
using FluentValidation.Results;

namespace PointBoard.Tests.Controllers;

public class CommentsControllerTests
{
    private readonly Mock<IRepo<Comment>> _repo = new();
    private readonly Mock<IValidator<CommentCreateOrUpdate>> _validator = new();
    private readonly CommentsController _controller;

    public CommentsControllerTests()
    {
        _controller = new CommentsController(_repo.Object, _validator.Object);
    }

    [Fact]
    public async Task GetCommentsAsync_ReturnsNotFound_WhenEmpty()
    {
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Comment>());

        var result = await _controller.GetCommentsAsync();

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetCommentsAsync_ReturnsOk_WhenHasData()
    {
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Comment>
        {
            new() { Id = Guid.NewGuid(), Text = "Test", BackgroundColor = "#fff", PointId = Guid.NewGuid() }
        });

        var result = await _controller.GetCommentsAsync();

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetCommentByIdAsync_ReturnsOk_WhenFound()
    {
        var id = Guid.NewGuid();
        var pointId = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Comment
        {
            Id = id,
            Text = "T",
            BackgroundColor = "#aaa",
            PointId = pointId,
            Point = new Point { Id = pointId, X = 1, Y = 2, Radius = 3, Color = "#ccc" }
        });

        var result = await _controller.GetCommentByIdAsync(id);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PostCommentAsync_ReturnsBadRequest_WhenValidationFails()
    {
        _validator.Setup(v => v.ValidateAsync(It.IsAny<CommentCreateOrUpdate>(), default))
                  .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Text", "Required") }));

        var result = await _controller.PostCommentAsync(new CommentCreateOrUpdate());

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task UpdateCommentAsync_ReturnsOk_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        var pointId = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Comment { Id = id, Text = "old", BackgroundColor = "#aaa", PointId = pointId });
        _validator.Setup(v => v.ValidateAsync(It.IsAny<CommentCreateOrUpdate>(), default))
                  .ReturnsAsync(new ValidationResult());
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Comment>())).ReturnsAsync((Comment c) => c);

        var model = new CommentCreateOrUpdate { Text = "updated", BackgroundColor = "#123", PointId = pointId };

        var result = await _controller.UpdateCommentAsync(id, model);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task UpdateCommentAsync_ReturnsNotFound_WhenMissing()
    {
        var id = Guid.NewGuid();
        
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Comment?)null);

        _validator.Setup(v => 
                             v.ValidateAsync(It.IsAny<CommentCreateOrUpdate>(), default))
                  .ReturnsAsync(new ValidationResult());

        var result = await _controller.UpdateCommentAsync(Guid.NewGuid(), new CommentCreateOrUpdate());

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteCommentAsync_ReturnsNotFound_WhenMissing()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Comment?)null);

        var result = await _controller.DeleteCommentAsync(Guid.NewGuid());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteCommentAsync_ReturnsProblem_WhenException()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Comment());
        _repo.Setup(r => r.DeleteAsync(It.IsAny<Comment>())).ThrowsAsync(new Exception("boom"));

        var result = await _controller.DeleteCommentAsync(Guid.NewGuid());

        var problem = result as ObjectResult;
        problem.Should().NotBeNull();
        problem!.StatusCode.Should().Be(500);
    }
}
