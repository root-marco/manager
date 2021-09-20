using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModels
{
  public class CreateUserViewModel
  {
    [Required(ErrorMessage = "name cannot be empty")]
    [MinLength(3, ErrorMessage = "minimum characters for NAME: 3")]
    [MaxLength(80, ErrorMessage = "maximum characters for NAME: 80")]
    public string Name { get; set; }

    [Required(ErrorMessage = "email cannot be empty.")]
    [MinLength(10, ErrorMessage = "minimum characters for EMAIL: 10")]
    [MaxLength(180, ErrorMessage = "minimum characters for EMAIL: 10")]
    [RegularExpression(
      @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
      ErrorMessage = "invalid email.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "password cannot be empty.")]
    [MinLength(6, ErrorMessage = "minimum characters for PASSWORD: 6")]
    [MaxLength(80, ErrorMessage = "maximum characters for PASSWORD: 80")]
    public string Password { get; set; }
  }
}