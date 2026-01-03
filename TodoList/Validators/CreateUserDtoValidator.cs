using FluentValidation;
using TodoList.Dtos;

namespace TodoList.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
  public CreateUserDtoValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("El campo 'name' es obligatorio")
      .MinimumLength(3).WithMessage("El campo 'name' debe tener al menos 3 caracteres");
  }
  
}
