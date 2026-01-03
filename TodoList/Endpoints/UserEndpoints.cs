using TodoList.Dtos;
using TodoList.Models;
using TodoList.Services;
using TodoList.Validators;
using FluentValidation;

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

    app.MapPost("/users", async (
      CreateUserDto dto, 
      IValidator<CreateUserDto> validator, 
      IUserService service
    ) =>
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

      var user = service.Create(dto.Name);
      return Results.Ok(user);
    });
  }
}