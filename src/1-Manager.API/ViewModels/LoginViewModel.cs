using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModels
{
  public class LoginViewModel
  {
    [Required(ErrorMessage = "login cannot be empty.")]
    public string Login { get; set; }

    [Required(ErrorMessage = "password cannot be empty.")]
    public string Password { get; set; }
  }
}