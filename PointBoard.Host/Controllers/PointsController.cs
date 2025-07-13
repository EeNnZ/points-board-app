using FluentValidation;
using FluentValidation.Results;
using PointBoard.Core.Abstractions;
using PointBoard.Core.Domain.Entities;
using PointBoard.Host.Extensions;
using Microsoft.AspNetCore.Mvc;
using PointBoard.Host.Models.Point;
using Swashbuckle.AspNetCore.Annotations;

namespace PointBoard.Host.Controllers;

/// <summary>
/// Controller for managing points in the system.
/// </summary>
[Route("api/points")]
[ApiController]
public class PointsController : ControllerBase
{
    private readonly IRepo<Point> _pointsRepo;
    private readonly IValidator<PointCreateOrUpdate> _commentValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PointsController"/> class.
    /// </summary>
    /// <param name="pointsRepo">Repository for points.</param>
    /// <param name="commentValidator">Validator for point creation or update models.</param>
    public PointsController(IRepo<Point> pointsRepo, IValidator<PointCreateOrUpdate> commentValidator)
    {
        _pointsRepo = pointsRepo;
        _commentValidator = commentValidator;
    }

    /// <summary>
    /// Gets all points from the database.
    /// </summary>
    /// <returns>A list of points or 404 if none found.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PointShortResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get all points", Description = "Fetches all points from the database")]
    public async Task<ActionResult<IEnumerable<PointShortResponse>>> GetPointsAsync()
    {
        ICollection<Point> result = await _pointsRepo.GetAllAsync();

        if (result.Count == 0)
            return NotFound();

        IEnumerable<PointShortResponse> points = result
                                                .AsEnumerable()
                                                .Select(point => new PointShortResponse
                                                 {
                                                     Id = point.Id,
                                                     X = point.X,
                                                     Y = point.Y,
                                                     Radius = point.Radius,
                                                     Color = point.Color
                                                 });

        return Ok(points);
    }

    /// <summary>
    /// Gets a point by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the point.</param>
    /// <returns>The point if found, otherwise 404.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PointShortResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get point", Description = "Get point from the database using id")]
    public async Task<ActionResult<PointShortResponse>> GetPointByIdAsync(Guid id)
    {
        Point? point = await _pointsRepo.GetByIdAsync(id);

        if (point == null) return NotFound();

        var pointModel = new PointShortResponse
        {
            Id = point.Id,
            X = point.X,
            Y = point.Y,
            Radius = point.Radius,
            Color = point.Color
        };

        return Ok(pointModel);
    }

    /// <summary>
    /// Creates a new point or updates an existing one.
    /// </summary>
    /// <param name="model">The point creation or update model.</param>
    /// <returns>The created point or validation errors.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PointResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create new or update existing point",
                      Description = "Create new point or update existing point")]
    public async Task<ActionResult<PointResponse>> PostPointAsync(PointCreateOrUpdate model)
    {
        ValidationResult validationResult = await _commentValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState.ValidationState);
        }

        var point = new Point
        {
            Id = Guid.NewGuid(),
            X = model.X,
            Y = model.Y,
            Radius = model.Radius,
            Color = model.Color
        };

        try
        {
            await _pointsRepo.CreateAsync(point);
            return CreatedAtAction("GetPointById", new { id = point.Id }, point);
        }
        catch (Exception e)
        {
            return Problem(title: "An error occured during point creation", detail: e.Message, statusCode: 500);
        }
    }

    /// <summary>
    /// Updates an existing point by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the point.</param>
    /// <param name="model">The point update model.</param>
    /// <returns>The updated point, 404 if not found, or validation errors.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Update point", Description = "Update existing point by ID")]
    public async Task<ActionResult<PointResponse>> UpdatePointAsync(Guid id, PointCreateOrUpdate model)
    {
        ValidationResult validationResult = await _commentValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState.ValidationState);
        }

        Point? existing = await _pointsRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.X = model.X;
        existing.Y = model.Y;
        existing.Radius = model.Radius;
        existing.Color = model.Color;

        try
        {
            Point? updatedPoint = await _pointsRepo.UpdateAsync(existing);

            if (updatedPoint == null)
                return Problem(title: "Error updating point", detail: "Point not found after update", statusCode: 500);

            var pointResponse = new PointResponse
            {
                Id = updatedPoint.Id,
                X = updatedPoint.X,
                Y = updatedPoint.Y,
                Radius = updatedPoint.Radius,
                Color = updatedPoint.Color
            };

            return Ok(pointResponse);
        }
        catch (Exception e)
        {
            return Problem(title: "Error updating point", detail: e.Message, statusCode: 500);
        }
    }


    /// <summary>
    /// Deletes a point by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the point.</param>
    /// <returns>No content if deleted, 404 if not found, or error details.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Delete point", Description = "Remove point from the database")]
    public async Task<IActionResult> DeletePointAsync(Guid id)
    {
        try
        {
            Point? point = await _pointsRepo.GetByIdAsync(id);
            if (point == null) return NotFound();

            await _pointsRepo.DeleteAsync(point);
            return NoContent();
        }
        catch (Exception e)
        {
            return Problem(title: "Point deletion failed", detail: e.Message, statusCode: 500);
        }
    }
}