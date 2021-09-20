using System;
using System.Threading.Tasks;
using Manager.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Manager.API.Controllers
{
  [ApiController]
  public class UserController : ControllerBase
  {
    // private readonly IMapper _mapper;
    // private readonly IUserService _userService;
    //
    // public UserController(IMapper mapper, IUserService userService)
    // {
    //   _mapper = mapper;
    //   _userService = userService;
    // }

    [HttpPost]
    [Route("api/v1/users/create")]
    public async Task<IActionResult> Create([FromBody] CreateUserViewModel userViewModel)
    {
      try
      {
        return Ok();
      }
      // catch (DomainException ex)
      // {
      //   return BadRequest();
      // }
      catch (Exception)
      {
        return StatusCode(500, "error");
      }
    }
  }
}