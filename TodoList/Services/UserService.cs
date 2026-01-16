using Microsoft.EntityFrameworkCore;
using TodoList.Models;
using TodoList.Dtos;
using TodoList.Context;

namespace TodoList.Services;

// public class UserService : IUserService
// {
// private readonly List<User> _users;
// private int _nextId = 1;  

// public UserService(List<User> users)
// {
//   _users = users;
// }  

// public List<UserResponseDto> GetAll()
// {
//   return _users.Select(user => new UserResponseDto
//   {
//     Id = user.Id,
//     Name = user.Name
//   }).ToList();
// }

// public UserResponseDto Create(string name)
// {
//   if (string.IsNullOrWhiteSpace(name))
//     throw new Exception("El nombre es obligatorio");

//   var user = new User
//   {
//     Id = _nextId++,
//     Name = name
//   };

//   _users.Add(user);

//   return new UserResponseDto
//   {
//     Id = user.Id,
//     Name = user.Name
//   };
// }

public class UserService : IUserService
{
  private readonly AppDbContext _context;

  public UserService(AppDbContext context)
  {
    _context = context;
  }

  public async Task<List<UserResponseDto>> GetAllAsync()
  {
    return await _context.Users
    .Select(u => new UserResponseDto
    {
      Id = u.Id,
      Name = u.Name
    })
    .ToListAsync();
  }

  public async Task<User> CreateAsync(string name)
  {
    var user = new User
    {
      Name = name
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return user;
  }

}