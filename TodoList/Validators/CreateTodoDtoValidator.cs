using FluentValidation;
using TodoList.Dtos;

namespace Todolist.Validators;

public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
{
  public CreateTodoDtoValidator()
  {
    RuleFor(x => x.Title)
      .NotEmpty().WithMessage("El titulo es obligatorio")
      .MinimumLength(10).WithMessage("El titulo debe tener al menos 10 caracteres");

    RuleFor(x => x.UserId)
      .NotEmpty().WithMessage("El campo 'userId' es obligatorio")
      .GreaterThan(0).WithMessage("El userId debe ser mayor a 0");

  }
}