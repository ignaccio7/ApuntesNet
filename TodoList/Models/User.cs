namespace TodoList.Models;

public class User
{
  public int Id { get; set; }
  public string Name { get; set; }

  // Relacion -> un usuario tiene muchos todos
  // que sera opcional porque puede crearse un user sin todos
  public List< Todo>? Todos { get; set; } = new();
}