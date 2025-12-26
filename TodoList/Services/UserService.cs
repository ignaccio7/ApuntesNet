using TodoList.Models;
using TodoList.Dtos;

namespace TodoList.Services;

public class UserService : IUserService
{
  private readonly List<User> _users;
  private int _nextId = 1;

  public UserService(List<User> users)
  {
    _users = users;
  }

  public List<UserResponseDto> GetAll()
  {
    return _users.Select(user => new UserResponseDto
    {
      Id = user.Id,
      Name = user.Name
    }).ToList();
  }

  public UserResponseDto Create(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new Exception("El nombre es obligatorio");

    var user = new User
    {
      Id = _nextId++,
      Name = name
    };

    _users.Add(user);

    return new UserResponseDto
    {
      Id = user.Id,
      Name = user.Name
    };
  }

}