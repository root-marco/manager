using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EscNet.Cryptography.Interfaces;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.Dtos;
using Manager.Services.Interfaces;

namespace Manager.Services.Services
{
  public class UserService : IUserService
  {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IRijndaelCryptography _rijndaelCryptography;

    public UserService(
        IMapper mapper,
        IUserRepository userRepository,
        IRijndaelCryptography rijndaelCryptography)
    {
      _mapper = mapper;
      _userRepository = userRepository;
      _rijndaelCryptography = rijndaelCryptography;
    }

    public async Task<UserDto> Create(UserDto userDto)
    {
      var existingUser = await _userRepository.GetByEmail(userDto.Email);

      if (existingUser != null)
        throw new DomainException("email already exists.");

      var user = _mapper.Map<User>(userDto);
      user.Validate();
      user.ChangePassword(_rijndaelCryptography.Encrypt(user.Password));

      var createdUser = await _userRepository.Create(user);

      return _mapper.Map<UserDto>(createdUser);
    }

    public async Task<UserDto> Update(UserDto userDto)
    {
      var existingUser = await _userRepository.Get(userDto.Id);

      if (existingUser == null)
        throw new DomainException("user not found.");

      var user = _mapper.Map<User>(userDto);
      user.Validate();
      user.ChangePassword(_rijndaelCryptography.Encrypt(user.Password));

      var updatedUser = await _userRepository.Update(user);

      return _mapper.Map<UserDto>(updatedUser);
    }

    public async Task Remove(long id)
    {
      await _userRepository.Remove(id);
    }

    public async Task<UserDto> Get(long id)
    {
      var user = await _userRepository.Get(id);

      return _mapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> Get()
    {
      var users = await _userRepository.Get();

      return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<List<UserDto>> SearchByName(string name)
    {
      var users = await _userRepository.SearchByName(name);

      return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<List<UserDto>> SearchByEmail(string email)
    {
      var users = await _userRepository.SearchByEmail(email);

      return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto> GetByEmail(string email)
    {
      var user = await _userRepository.GetByEmail(email);

      return _mapper.Map<UserDto>(user);
    }
  }
}