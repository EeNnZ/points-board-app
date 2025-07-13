using FluentValidation;
using PointBoard.Core.Abstractions;
using PointBoard.Core.Domain.Entities;
using PointBoard.DataAccess;
using PointBoard.DataAccess.Repos;
using PointBoard.Host.Extensions;
using PointBoard.Host.Models.Comment;
using PointBoard.Host.Models.Point;
using PointBoard.Host.Validation;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddCors(op => op.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()))
       .AddScoped<DbContext, DataContext>(_ => new DataContext())
       .AddScoped<IRepo<Point>, PointsEfRepo>()
       .AddScoped<IRepo<Comment>, CommentsEfRepo>()
       .AddScoped<IValidator<PointCreateOrUpdate>, PointCreateOrUpdateValidator>()
       .AddScoped<IValidator<CommentCreateOrUpdate>, CommentCreateOrUpdateValidator>()
       .AddDefaultSwagger()
       .AddControllers();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseSwagger()
       .UseSwaggerUI()
       .UseDeveloperExceptionPage();

app.UseRouting()
   .UseAuthorization()
   .UseDefaultFiles()
   .UseStaticFiles();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();