using System.Collections.Generic;
using Manager.Core.Exceptions;
using Manager.Domain.Validators;

namespace Manager.Domain.Entities
{
  public class User : Base
  {public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    protected User() { }

    public User(string name, string email, string password)
    {
      this.Name = name;
      this.Email = email;
      this.Password = password;
      this._errors = new List<string>();
      this.Validate();
    }

    public void ChangeName(string name)
    {
      this.Name = name;
      this.Validate();
    }

    public void ChangePassword(string password)
    {
      this.Password = password;
      this.Validate();
    }

    public void ChangeEmail(string email)
    {
      this.Email = email;
      this.Validate();
    }

    public override bool Validate()
    {
      var validation = (new UserValidator()).Validate(this);

      if (validation.IsValid) return true;

      foreach (var error in validation.Errors)
        this._errors.Add(error.ErrorMessage);

      throw new DomainException("some fields are invalid" + this._errors);
    }
  }
}