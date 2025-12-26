using TodoList.Dtos;

namespace TodoList.Services;

public interface IUserService
{
  List<UserResponseDto> GetAll();
  UserResponseDto Create(string name);
}