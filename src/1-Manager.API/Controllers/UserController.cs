using System;
using System.Threading.Tasks;
using AutoMapper;
using Manager.API.Utilities;
using Manager.API.ViewModels;
using Manager.Core.Exceptions;
using Manager.Services.Dtos;
using Manager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manager.API.Controllers
{
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(IMapper mapper, IUserService userService)
    {
      _mapper = mapper;
      _userService = userService;
    }

    [HttpPost]
    // [Authorize]
    [Route("api/v1/users/create")]
    public async Task<IActionResult> Create([FromBody] CreateUserViewModel userViewModel)
    {
      try
      {
        var userDto = _mapper.Map<UserDto>(userViewModel);

        var createdUser = await _userService.Create(userDto);

        return Ok(new ResultViewModel
        {
          Message = "user created successfully.",
          Success = true,
          Data = createdUser
        });
      }
      catch (DomainException ex)
      {
        return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
      }
      catch (Exception ex)
      {
        Console.Write(ex);
        return StatusCode(500);
      }
    }

    [HttpPut]
    // [Authorize]
    [Route("/api/v1/users/update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserViewModel userViewModel)
    {
      try
      {
        var userDto = _mapper.Map<UserDto>(userViewModel);

        var userUpdated = await _userService.Update(userDto);

        return Ok(new ResultViewModel
        {
          Message = "user updated successfully.",
          Success = true,
          Data = userUpdated
        });
      }
      catch (DomainException ex)
      {
        return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex);
      }
    }

    [HttpDelete]
    // [Authorize]
    [Route("/api/v1/users/remove/{id:long}")]
    public async Task<IActionResult> Remove(long id)
    {
      try
      {
        await _userService.Remove(id);

        return Ok(new ResultViewModel
        {
          Message = "user deleted successfully.",
          Success = true,
          Data = null
        });
      }
      catch (DomainException ex)
      {
        return BadRequest(Responses.DomainErrorMessage(ex.Message));
      }
      catch (Exception)
      {
        return StatusCode(500, Responses.ApplicationErrorMessage());
      }
    }

    [HttpGet]
    // [Authorize]
    [Route("/api/v1/users/get/{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
      try
      {
        var user = await _userService.Get(id);

        if (user == null)
          return Ok(new ResultViewModel
          {
            Message = "user not found.",
            Success = true,
            Data = user
          });

        return Ok(new ResultViewModel
        {
          Message = "user found!",
          Success = true,
          Data = user
        });
      }
      catch (DomainException ex)
      {
        return BadRequest(Responses.DomainErrorMessage(ex.Message));
      }
      catch (Exception)
      {
        return StatusCode(500, Responses.ApplicationErrorMessage());
      }
    }

    [HttpGet]
    // [Authorize]
    [Route("/api/v1/users/get-all")]
    public async Task<IActionResult> Get()
    {
      try
      {
        var allUsers = await _userService.Get();

        return Ok(new ResultViewModel
        {
          Message = "users found.",
          Success = true,
          Data = allUsers
        });
      }
      catch (DomainException ex)
      {
        return BadRequest(Responses.DomainErrorMessage(ex.Message));
      }
      catch (Exception)
      {
        return StatusCode(500, Responses.ApplicationErrorMessage());
      }
    }

    [HttpGet]
    // [Authorize]
    [Route("/api/v1/users/get-by-email")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
      try
      {
        var user = await _userService.GetByEmail(email);

        if (user == null)
          return Ok(new ResultViewModel
          {
            Message = "user not found",
            Success = true,
            Data = user
          });
        
        return Ok(new ResultViewModel
        {
          Message = "user found",
          Success = true,
          Data = user
        });
      }
      catch (DomainException ex)
      {
        return BadRequest(Responses.DomainErrorMessage(ex.Message));
      }
      catch (Exception)
      {
        return StatusCode(500, Responses.ApplicationErrorMessage());
      }
    }
    
    [HttpGet]
    // [Authorize]
    [Route("/api/v1/users/search-by-name")]
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
      try
      {
        var allUsers = await _userService.SearchByName(name);

        if (allUsers.Count == 0)
          return Ok(new ResultViewModel
          {
            Message = "user not found",
            Success = true,
            Data = null
          });

        return Ok(new ResultViewModel
        {
          Message = "user found",
          Success = true,
          Data = allUsers
        });
      }
      catch (DomainException ex)
      {
        return BadRequest(Responses.DomainErrorMessage(ex.Message));
      }
      catch (Exception)
      {
        return StatusCode(500, Responses.ApplicationErrorMessage());
      }
    }

    [HttpGet]
    // [Authorize]
    [Route("/api/v1/users/search-by-email")]
    public async Task<IActionResult> SearchByEmail([FromQuery] string email)
    {
      try
      {
        var allUsers = await _userService.SearchByEmail(email);

        if (allUsers.Count == 0)
          return Ok(new ResultViewModel
          {
            Message = "user not found",
            Success = true,
            Data = null
          });

        return Ok(new ResultViewModel
        {
          Message = "user found",
          Success = true,
          Data = allUsers
        });
      }
      catch (DomainException ex)
      {
        return BadRequest(Responses.DomainErrorMessage(ex.Message));
      }
      catch (Exception)
      {
        return StatusCode(500, Responses.ApplicationErrorMessage());
      }
    }
    
  }
}