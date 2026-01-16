using TodoList.Models;
using TodoList.Dtos;
using TodoList.Exceptions;
using Microsoft.EntityFrameworkCore;
using TodoList.Context;

namespace TodoList.Services;

public class TodoService : ITodoService
{
  // private readonly List<User> _users;
  // private readonly List<Todo> _todos;
  // private int _nextId = 1;

  // public TodoService(List<User> users, List<Todo> todos)
  // {
  //   _users = users;
  //   _todos = todos;
  // }
  // public TodoService(List<Todo> todos)
  // {
  //   _todos = todos;
  // }

  // public List<TodoResponseDto> GetAll()
  // {
  //   return _todos.Select(t => new TodoResponseDto
  //   {
  //     Id = t.Id,
  //     Title = t.Title,
  //     IsCompleted = t.IsCompleted,
  //     UserId = t.User.Id,
  //     UserName = t.User.Name
  //   }).ToList();
  // }

  // public TodoResponseDto Create(string title, int userId)
  // {
  //   // var user = _users.Find(u => u.Id == userId);
  //   // Console.WriteLine(user);
  //   // if (user == null)
  //   // {
  //   //   throw new Exception("Usuario no existe");
  //   // }
  //   var todo = new Todo
  //   {
  //     Id = _nextId++,
  //     Title = title,
  //     // User = user
  //   };

  //   _todos.Add(todo);
  //   // user.Todos.Add(todo);

  //   return new TodoResponseDto
  //   {
  //     Id = todo.Id,
  //     Title = todo.Title,
  //     IsCompleted = todo.IsCompleted,
  //     UserId = 1,
  //     UserName = "1"
  //     // UserId = todo.User.Id,
  //     // UserName = todo.User.Name
  //   };
  // }

  // public bool Update(int id, string title, bool isCompleted)
  // {
  //   var todo = _todos.Find(t => t.Id == id);
  //   if (todo == null)
  //   {
  //     throw new NotFoundException("Todo no existe");
  //   }

  //   todo.Title = title;
  //   todo.IsCompleted = isCompleted;

  //   Console.WriteLine("El todo es:::::::::::::::::::::::::");
  //   Console.WriteLine($"Id: {todo.Id}");
  //   Console.WriteLine($"Title: {todo.Title}");
  //   Console.WriteLine($"IsCompleted: {todo.IsCompleted}");
  //   Console.WriteLine($"UserId: {todo.User.Id}");


  //   return true;
  // }

  // public bool Delete(int id)
  // {
  //   var todo = _todos.Find(t => t.Id == id);
  //   if (todo == null)
  //   {
  //     throw new Exception("Todo no existe");
  //   }

  //   _todos.Remove(todo);
  //   // todo.User.Todos.Remove(todo);
  //   return true;
  // }

  private readonly AppDbContext _context;

  public TodoService(AppDbContext context)
  {
    _context = context;
  }

  public async Task<List<TodoResponseDto>> GetAllAsync()
  {
    return await _context.Todos
      .Include(t => t.User) // JOIN
      .Select(t => new TodoResponseDto
      {
        Id = t.Id,
        Title = t.Title,
        IsCompleted = t.IsCompleted,
        UserId = t.User.Id,
        UserName = t.User.Name
      })
      .ToListAsync();
  }

  public async Task<TodoResponseCreateDto> CreateAsync(string title, int userId)
  {
    var user = await _context.Users.AnyAsync(u => u.Id == userId);
    if (!user)
    {
      throw new Exception("El usuario no existe");
    }

    var todo = new Todo
    {
      Title = title,
      UserId = userId,
      IsCompleted = false
    };

    _context.Todos.Add(todo);
    await _context.SaveChangesAsync();

    // return todo;    
    return new TodoResponseCreateDto
    {
      Id = todo.Id,
      Title = todo.Title,
      IsCompleted = todo.IsCompleted,
      UserId = todo.UserId,
    };    
  }

  public async Task<bool> UpdateAsync(int id, string title, bool isCompleted)
  {
    var todo = await _context.Todos.FindAsync(id);
    if (todo is null)
    {
      return false;
    }

    todo.Title = title;
    todo.IsCompleted = isCompleted;

    await _context.SaveChangesAsync();
    return true;    
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var todo = await _context.Todos.FindAsync(id);
    if (todo is null) return false;

    _context.Todos.Remove(todo);
    await _context.SaveChangesAsync();
    return true;
  }

}
