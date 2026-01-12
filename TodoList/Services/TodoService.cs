using TodoList.Models;
using TodoList.Dtos;
using TodoList.Exceptions;

namespace TodoList.Services;

public class TodoService : ITodoService
{
  // private readonly List<User> _users;
  private readonly List<Todo> _todos;
  private int _nextId = 1;

  // public TodoService(List<User> users, List<Todo> todos)
  // {
  //   _users = users;
  //   _todos = todos;
  // }
  public TodoService(List<Todo> todos)
  {
    _todos = todos;
  }

  public List<TodoResponseDto> GetAll()
  {
    return _todos.Select(t => new TodoResponseDto
    {
      Id = t.Id,
      Title = t.Title,
      IsCompleted = t.IsCompleted,
      UserId = t.User.Id,
      UserName = t.User.Name
    }).ToList();
  }

  public TodoResponseDto Create(string title, int userId)
  {
    // var user = _users.Find(u => u.Id == userId);
    // Console.WriteLine(user);
    // if (user == null)
    // {
    //   throw new Exception("Usuario no existe");
    // }
    var todo = new Todo
    {
      Id = _nextId++,
      Title = title,
      // User = user
    };

    _todos.Add(todo);
    // user.Todos.Add(todo);

    return new TodoResponseDto
    {
      Id = todo.Id,
      Title = todo.Title,
      IsCompleted = todo.IsCompleted,
      UserId = 1,
      UserName = "1"
      // UserId = todo.User.Id,
      // UserName = todo.User.Name
    };
  }

  public bool Update(int id, string title, bool isCompleted)
  {
    var todo = _todos.Find(t => t.Id == id);
    if (todo == null)
    {
      throw new NotFoundException("Todo no existe");
    }

    todo.Title = title;
    todo.IsCompleted = isCompleted;

    Console.WriteLine("El todo es:::::::::::::::::::::::::");
    Console.WriteLine($"Id: {todo.Id}");
    Console.WriteLine($"Title: {todo.Title}");
    Console.WriteLine($"IsCompleted: {todo.IsCompleted}");
    Console.WriteLine($"UserId: {todo.User.Id}");


    return true;
  }

  public bool Delete(int id)
  {
    var todo = _todos.Find(t => t.Id == id);
    if (todo == null)
    {
      throw new Exception("Todo no existe");
    }

    _todos.Remove(todo);
    // todo.User.Todos.Remove(todo);
    return true;
  }

}
