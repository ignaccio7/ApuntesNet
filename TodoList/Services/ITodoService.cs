using TodoList.Models;
using TodoList.Dtos;
namespace TodoList.Services;

public interface ITodoService
{
  // List<TodoResponseDto> GetAll();
  // TodoResponseDto Create(string title, int userId);
  // bool Update(int id, string title, bool isCompleted);
  // bool Delete(int id);
  Task<List<TodoResponseDto>> GetAllAsync();
  Task<TodoResponseCreateDto> CreateAsync(string title, int userId);
  Task<bool> UpdateAsync(int id, string title, bool isCompleted);
  Task<bool> DeleteAsync(int id);
}