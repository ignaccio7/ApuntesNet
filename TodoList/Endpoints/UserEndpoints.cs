using TodoList.Dtos;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.Endpoints;

public static class UserEndpoints
{
  // Es como decir “Quiero que WebApplication tenga un método nuevo llamado MapUserEndpoints()”
  // antes -> UserEndpoints.MapUserEndpoints(app);
  // despues -> app.MapUserEndpoints();
  public static void MapUserEndpoints(this WebApplication app)
  {
    app.MapGet("/users", (IUserService service) =>
    {
      var users = service.GetAll();
      return Results.Ok(users);
    });

    app.MapPost("/users", (CreateUserDto dto, IUserService service) =>
    {
      var user = service.Create(dto.Name);
      return Results.Ok(user);
    });
  }
}