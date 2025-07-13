using FluentValidation;
using FluentValidation.Results;
using PointBoard.Core.Abstractions;
using PointBoard.Core.Domain.Entities;
using PointBoard.Host.Extensions;
using Microsoft.AspNetCore.Mvc;
using PointBoard.Host.Models.Comment;
using PointBoard.Host.Models.Point;
using Swashbuckle.AspNetCore.Annotations;

namespace PointBoard.Host.Controllers;

/// <summary>
/// Controller for managing comments in the system.
/// </summary>
[Route("api/comments")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IRepo<Comment> _commentRepo;
    private readonly IValidator<CommentCreateOrUpdate> _commentValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentsController"/> class.
    /// </summary>
    /// <param name="commentRepo">Repository for comments.</param>
    /// <param name="commentValidator">Validator for comment creation or update models.</param>
    public CommentsController(IRepo<Comment> commentRepo, IValidator<CommentCreateOrUpdate> commentValidator)
    {
        _commentRepo = commentRepo;
        _commentValidator = commentValidator;
    }

    /// <summary>
    /// Gets all comments from the database.
    /// </summary>
    /// <returns>A list of comments or 404 if none found.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CommentShortResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get all comments", Description = "Fetches all comments from the database")]
    public async Task<ActionResult<IEnumerable<CommentShortResponse>>> GetCommentsAsync()
    {
        ICollection<Comment> result = await _commentRepo.GetAllAsync();

        if (result.Count == 0)
            return NotFound();

        List<CommentShortResponse> comments = result.AsEnumerable()
                                                    .Select(comm => new CommentShortResponse
                                                     {
                                                         Id = comm.Id,
                                                         Text = comm.Text,
                                                         BackgroundColor = comm.BackgroundColor,
                                                         PointId = comm.PointId
                                                     })
                                                    .ToList();
        return Ok(comments);
    }

    /// <summary>
    /// Gets a comment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the comment.</param>
    /// <returns>The comment if found, otherwise 404.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Retrieve a comment by ID",
                      Description = "Fetches a comment from the database using its ID.")]
    public async Task<ActionResult<CommentResponse>> GetCommentByIdAsync(Guid id)
    {
        Comment? comment = await _commentRepo.GetByIdAsync(id);

        if (comment == null) return NotFound();

        var commentModel = new CommentResponse
        {
            Id = comment.Id,
            Text = comment.Text,
            BackgroundColor = comment.BackgroundColor,
            PointId = comment.PointId,
            Point = new PointShortResponse()
            {
                Id = comment.PointId,
                X = comment.Point.X,
                Y = comment.Point.Y,
                Radius = comment.Point.Radius,
                Color = comment.Point.Color
            }
        };

        return Ok(commentModel);
    }

    /// <summary>
    /// Gets all comments related to a specific point.
    /// </summary>
    /// <param name="pointId">The unique identifier of the point.</param>
    /// <returns>A list of comments for the point or 404 if none found.</returns>
    [HttpGet("~/api/points/{pointId:guid}/comments")]
    [ProducesResponseType(typeof(IEnumerable<CommentShortResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get comments by point ID",
                      Description = "Returns all comments related to the specified point.")]
    public async Task<ActionResult<IEnumerable<CommentShortResponse>>> GetCommentsByPointId(Guid pointId)
    {
        ICollection<Comment> comments = await _commentRepo.GetAllAsync();

        List<CommentShortResponse> filtered = comments
                                             .Where(c => c.PointId == pointId)
                                             .Select(c => new CommentShortResponse
                                              {
                                                  Id = c.Id,
                                                  Text = c.Text,
                                                  BackgroundColor = c.BackgroundColor,
                                                  PointId = c.PointId
                                              })
                                             .ToList();

        if (filtered.Count == 0)
            return NotFound();

        return Ok(filtered);
    }


    /// <summary>
    /// Creates a new comment or updates an existing one.
    /// </summary>
    /// <param name="commentModel">The comment creation or update model.</param>
    /// <returns>The created comment or validation errors.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Create or update a comment", Description = "Creates or updates a comment")]
    public async Task<ActionResult<CommentResponse>> PostCommentAsync(CommentCreateOrUpdate commentModel)
    {
        ValidationResult validationResult = await _commentValidator.ValidateAsync(commentModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState.ValidationState);
        }

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Text = commentModel.Text,
            BackgroundColor = commentModel.BackgroundColor,
            PointId = commentModel.PointId
        };

        try
        {
            await _commentRepo.CreateAsync(comment);

            var commentResponse = new CommentResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                BackgroundColor = comment.BackgroundColor,
                PointId = comment.PointId
            };
            return CreatedAtAction("GetCommentById", new { id = comment.Id }, commentResponse);
        }
        catch (Exception e)
        {
            return Problem(title: "An error occured during creating comment", detail: e.Message, statusCode: 500);
        }
    }

    /// <summary>
    /// Updates an existing comment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the comment.</param>
    /// <param name="model">The comment update model.</param>
    /// <returns>The updated comment, 404 if not found, or validation errors.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Update comment", Description = "Updates an existing comment")]
    public async Task<ActionResult<CommentResponse>> UpdateCommentAsync(Guid id, CommentCreateOrUpdate model)
    {
        ValidationResult validationResult = await _commentValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState.ValidationState);
        }

        Comment? existing = await _commentRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Text = model.Text;
        existing.BackgroundColor = model.BackgroundColor;
        existing.PointId = model.PointId;

        try
        {
            await _commentRepo.UpdateAsync(existing);

            var response = new CommentResponse
            {
                Id = existing.Id,
                Text = existing.Text,
                BackgroundColor = existing.BackgroundColor,
                PointId = existing.PointId
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return Problem(title: "Error updating comment", detail: ex.Message, statusCode: 500);
        }
    }


    /// <summary>
    /// Deletes a comment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the comment.</param>
    /// <returns>No content if deleted, 404 if not found, or error details.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Delete a comment", Description = "Deletes a comment from the database using its ID.")]
    public async Task<IActionResult> DeleteCommentAsync(Guid id)
    {
        try
        {
            Comment? comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null) return NotFound();

            await _commentRepo.DeleteAsync(comment);

            return NoContent();
        }
        catch (Exception e)
        {
            return Problem(title: "An error occured during deleting comment", detail: e.Message, statusCode: 500);
        }
    }
}