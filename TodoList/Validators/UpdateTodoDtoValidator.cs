using FluentValidation;
using TodoList.Dtos;

namespace TodoList.Validators;

public class UpdateTodoDtoValidator : AbstractValidator<UpdateTodoDto>
{

  public UpdateTodoDtoValidator()
  {
    RuleFor(x => x.Title)
      .NotEmpty().WithMessage("El titulo es obligatorio")
      .MinimumLength(10).WithMessage("El titulo debe tener al menos 10 caracteres");
  }

}