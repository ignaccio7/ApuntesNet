using TodoList.Dtos;
using TodoList.Services;
using TodoList.Models;
using TodoList.Validators;
using FluentValidation;

namespace TodoList.Endpoints;

public static class TodoEndpoints
{
  public static void MapTodoEndpoints(this WebApplication app)
  {
    app.MapGet("/todos", (ITodoService service) =>
    {
      var todos = service.GetAll();
      return Results.Ok(todos);
    });

    // app.MapPost("/todos", (CreateTodoDto dto, ITodoService service) =>
    // {
    //   var todo = service.Create(dto.Title, dto.UserId);

    //   return Results.Ok(todo);
    // });

    app.MapPost("/todos", async (CreateTodoDto dto, ITodoService service,IValidator<CreateTodoDto> validator) =>
    {
      var result = await validator.ValidateAsync(dto);

      Console.WriteLine(result);
      Console.WriteLine(result.Errors);

      if(!result.IsValid)
      {
        var errors = result.Errors
          .GroupBy(e => e.PropertyName)
          .ToDictionary(
            g => g.Key,
            g => g.Select(e => e.ErrorMessage).ToArray()
          );

        return Results.ValidationProblem(errors);
      }

      var todo = service.Create(dto.Title, dto.UserId);

      return Results.Ok(todo);
    });

    // app.MapPut("/todos/{id}", (int id, UpdateTodoDto dto, ITodoService service) =>
    // {
    //   var ok = service.Update(id, dto.Title, dto.IsCompleted);
    //   return ok ? Results.Ok() : Results.NotFound();
    // });

    app.MapPut("/todos/{id}", async (int id, UpdateTodoDto dto, IValidator<UpdateTodoDto> validator,ITodoService service) =>
    {
      var result = await validator.ValidateAsync(dto);

      if(!result.IsValid)
      {
        return Results.ValidationProblem(
          result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
              g => g.Key,
              g => g.Select(e => e.ErrorMessage).ToArray()
            )
          ); 
      }

      var ok = service.Update(id, dto.Title, dto.IsCompleted);
      return ok ? Results.Ok() : Results.NotFound();
    });

    app.MapDelete("/todos/{id}", (int id, ITodoService service) =>
    {
      var ok = service.Delete(id);
      return ok ? Results.Ok() : Results.NotFound();
    });

  }
}