using FluentValidation;
using Manager.Domain.Entities;

namespace Manager.Domain.Validators
{
  public class UserValidator : AbstractValidator<User>
  {
    public UserValidator()
    {
      RuleFor(user => user)
        .NotEmpty().WithMessage("entity cannot be empty.")
        .NotNull().WithMessage("entity cannot be null.");

      RuleFor(user => user.Name)
        .NotEmpty().WithMessage("name cannot be empty.")
        .NotNull().WithMessage("name cannot be null.")
        .MinimumLength(3).WithMessage("minimum characters for NAME: 3")
        .MaximumLength(80).WithMessage("maximum characters for NAME: 80");

      RuleFor(user => user.Password)
        .NotEmpty().WithMessage("password cannot be empty.")
        .NotNull().WithMessage("password cannot be null.")
        .MinimumLength(6).WithMessage("minimum characters for PASSWORD: 6")
        .MaximumLength(80).WithMessage("maximum characters for PASSWORD: 80");

      RuleFor(user => user.Email)
        .NotEmpty().WithMessage("email cannot be empty.")
        .NotNull().WithMessage("email cannot be null.")
        .MinimumLength(10).WithMessage("minimum characters for EMAIL: 10")
        .MaximumLength(180).WithMessage("minimum characters for EMAIL: 10")
        .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
        .WithMessage("invalid email.");
    }
  }
}