using TodoList.Dtos;
using TodoList.Services;
using TodoList.Models;

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

    app.MapPost("/todos", (CreateTodoDto dto, ITodoService service) =>
    {
      var todo = service.Create(dto.Title, dto.UserId);

      return Results.Ok(todo);
    });

    app.MapPut("/todos/{id}", (int id, UpdateTodoDto dto, ITodoService service) =>
    {
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