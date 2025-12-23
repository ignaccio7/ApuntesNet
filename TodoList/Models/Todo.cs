namespace TodoList.Models;

public class Todo
{
  public int Id { get; set; }
  public string Title { get; set; }
  public bool IsCompleted { get; set; }

  // Relacion -> un todo tiene un usuario
  public User User { get; set; } = null!;
}