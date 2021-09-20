using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Services.Dtos;

namespace Manager.Services.Interfaces
{
  public interface IUserService
  {
    Task<UserDto> Create(UserDto userDto);
    Task<UserDto> Update(UserDto userDto);
    Task Remove(long id);
    Task<UserDto> Get(long id);
    Task<List<UserDto>> Get();
    Task<List<UserDto>> SearchByName(string name);
    Task<List<UserDto>> SearchByEmail(string email);
    Task<UserDto> GetByEmail(string email);
  }
}