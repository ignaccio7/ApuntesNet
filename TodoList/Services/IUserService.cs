using Microsoft.EntityFrameworkCore;
using TodoList.Dtos;
using TodoList.Models;

namespace TodoList.Services;

public interface IUserService
{
  // List<UserResponseDto> GetAll();
  // UserResponseDto Create(string name);
  Task<List<UserResponseDto>> GetAllAsync();
  Task<User> CreateAsync(string name);
}